import sys
import multiprocessing
import cx_Oracle
import time
import os.path
import linecache
from functools import partial


def main():
    ##
    ##

    prepare_process('wdi_tests.txt', 'extract')


def prepare_process(file_name, what):

    main_dir = os.path.dirname(__file__)
    config_file = os.path.join(main_dir, file_name)

    reader = open(config_file, 'r')

    # Each line in the file is a table name
    # halt the process when there's nothing to read
    tables = reader.read().splitlines()

    #write_stream = open( os.path.join(main_dir, 'adjustments.log'), "w")
    #write_stream.close()

    pool = multiprocessing.Pool()
    pool.map(partial(process, what=what), tables)

    # for table_name in tables:
    #     if table_name is not "":
    #         try:
    #             if what == "update":
    #                 update(table_name)
    #                 print("FINISHED PROCESSING  " + table_name )
    #             elif what == "extract":
    #
    #                 extract(table_name)
    #             else:
    #                 revert(table_name)
    #         except Exception, e:
    #             print(str(e))


def process(table_name, what):

    if table_name is not "":
        try:
            if what == "update":
                update(table_name)
                print("FINISHED PROCESSING  " + table_name)
            elif what == "extract":

                extract(table_name)
            else:
                revert(table_name)
        except Exception, e:
            print(str(e) + "   " + table_name )


def extract(table_name):

    connection, schema = get_connection_details()
    curs = connection.cursor()

    # check if there is a extracts directory and create one if there is not.
    main_dir = os.path.dirname(__file__)
    extract_dir = os.path.join(main_dir, "extracts")

    # extracts will be created in a separate directory. create one if
    # not available
    if not os.path.exists(extract_dir):
        os.mkdir(extract_dir, os.O_CREAT)

    extract_file = os.path.join(extract_dir, table_name + '.txt')
    log_file = os.path.join(extract_dir, table_name + '.log')
    extract = open(extract_file, "w")
    log = open(log_file, "w")

    # Encapsulate ROW in extra quotes so that sql statement is not confused for keyword
    if table_name == "ROW":
        table_name = '"ROW"'

    # Ordinates array is made up of lower left x, y and upper right x, y
    # these ordinates are the bounds of Area 1 delivery and we need to change
    # values for different areas or remove them completely if we intend to run a full extract.
    # 1003 indicates a rectangle with just exterior
    # .. and it is a polygon
    ordinate_array = 'mdsys.sdo_ordinate_array (1771418.16551, 6615230.86392, 1832491.89111 ,6657288.57261 )),'
    elem_info = 'mdsys.sdo_elem_info_array (1, 1003, 3),'
    geom_type = 'mdsys.sdo_geometry (2003, null, null,'
    options = "'mask=ANYINTERACT querytype=WINDOW')= 'TRUE'"

    count_query = 'SELECT COUNT(*) FROM ' + schema + '.' + table_name
    #sql_string = 'SELECT rowid, g3e_fid,g3e_geometry from ' + schema + '.' + table_name #+ ' rec WHERE mdsys.sdo_relate (rec.G3E_GEOMETRY,' + geom_type + elem_info + ordinate_array + options

    # SQL statement for full extract
    sql_string = 'select rowid,g3e_fid,g3e_geometry from ' + schema + '.' + table_name

    total_records = curs.execute(count_query).fetchone()[0]
    curs.execute(sql_string)
    print "Processing  : ", table_name
    processed_count = 0
    table_start = time.clock()
    seg_start = time.clock()

    for a_row in curs:

        try:
            # Prepare identifier
            id = a_row[0]
            fid = a_row[1]
            coords = get_coordinates(a_row[2])
            # just append new coordinate values to the existing record line
            extract.write(str(id) + '|' + str(fid) + '#')

            for a_coord in coords:
                extract.write(str(a_coord) + ",")

            extract.write('\n')
            processed_count += 1

            if processed_count % 25000 == 0:
                duration = time.clock() - seg_start
                lapsed = time.clock() - table_start
                estimates = ((total_records - processed_count)/25000)*duration
                lapsed_string = beautify_time_in_seconds(lapsed)
                estimates_string = beautify_time_in_seconds(estimates)
                seg_start = time.clock()
                print table_name, " ", processed_count, " processed in : ", lapsed_string, "Est.Completion : ", estimates_string

        except Exception, e:
            log.write(e, '\n')

    curs.close()
    connection.close()
    extract.close()
    log.close()

    print "Extracted  " + table_name + " in : ", beautify_time_in_seconds(time.clock() - table_start)


def beautify_time_in_seconds(secs):

    mins, secs = divmod(secs, 60)
    hrs, mins = divmod(mins, 60)

    return str(hrs) + " Hrs " + str(mins) + " mins " + format(secs, '.2f') + " secs"


def get_connection_details(file_name="config.txt"):

    # It assumes that the connection parameters are set in a config
    # file sitting in the same directory as this file
    main_dir = os.path.dirname(__file__)
    conf_file = os.path.join(main_dir, file_name)

    # it assumes that the file will only have one line in the form of
    # <user_name>/password@service
    db_command = linecache.getline(conf_file, 1).rstrip().split('=')[1]
    schema = linecache.getline(conf_file, 2).split('=')[1]

    return cx_Oracle.connect(db_command), schema.rstrip()


def update(file_name):

    print "Started processing : " + file_name
    table_name = file_name.split("-")[0]

    # get the file name
    main_dir = os.path.dirname(__file__)
    write_stream = open(os.path.join(main_dir, 'adjustments.log'), "a")
    data_file = os.path.join(main_dir, 'updates\\' + file_name + '.txt')
    log = open(os.path.join(main_dir, 'out_of_jobs.txt'), "a")

    if table_name == "ROW":
        table_name = '"ROW"'

    # get file object
    read_stream = open(data_file, "r")
    #data = read_stream.read().splitlines()

    ''' read file and parse it to get old and new geometries. The geometry
     from oracle table is separated by a pipe (|) and the geometry from
     adjust.IT is separated by a dollar ($) '''

    # define a counter for status
    counter = 0
    out_count = 0
    connection, schema = get_connection_details()
    curs = connection.cursor()
    start = time.clock()

    # Disable triggers on table so that geometry updates are processed
    # without any hiccups
    curs.execute("ALTER TABLE " + schema + "." + table_name + " DISABLE ALL TRIGGERS")


    #pool = multiprocessing.Pool()
    #pool.map(partial(int_update, table_name = table_name, schema=schema, curs=curs, write_stream=write_stream),data)

    for a_line in read_stream:

        ''' split it by |. Info at index 0 is unique identifier string
         and at 1 is a string of oracle, adjust.IT geometries separated
         by $ '''
        info = a_line.split("|")
        identifier = info[0]
        geoms = info[1].split("$")
        original_geometry = geoms[0].split(',')
        adjusted_geometry = geoms[1].split(',')

        # source data is now in 3D and extra coordinate info i.e 1.0, 0.0 which
        # will not have any adjusted coordinate and will get a NoShift comment.
        # These checks shall now be cancelled.
        # if 'OUT' in adjusted_geometry or 'OUT\n' in adjusted_geometry:
        #     log.write(a_line)
        #     out_count += 1
        #     continue

        try:
            counter += 1
            if counter % 10000 == 0:
                print("Processed   " + str(counter) + "  records. Time taken   ", time.clock() - start)
            start = time.clock()

            sql_string = "SELECT G3E_GEOMETRY FROM " + schema + "." + table_name + \
                         " WHERE ROWID = CHARTOROWID('" + identifier + "')"

            curs.execute(sql_string)
            res = curs.fetchall()

            for a_rec in res:
                # points and lines have different attributes/methods to set geometries
                # so separate the process based on geometry type
                oracle_record = a_rec[0]

                # Coordinate array for point geometries is differently maintained in 3D format
                # for example a pole geometry is maintained like (1234.3467, 3456.356, 0,1,0,0)
                if oracle_record.SDO_GTYPE == 3001:
                    factor = 3
                else:
                    factor = 4

                # oracle and adjusted geometries should be of the size - same
                # number of vertices
                if len(adjusted_geometry) == len(original_geometry) + (len(original_geometry) / (factor - 1)):
                    # if oracle_record.SDO_GTYPE == 4001:
                    #     # point geometry
                    #     # get SDO_POINT type and set x, y values
                    #     sdo_point = oracle_record.SDO_POINT
                    #     new_geometry = adjusted_geometry
                    #     x = float(new_geometry[0])
                    #     y = float(new_geometry[1])
                    #
                    #     setattr(sdo_point, 'X', x)
                    #     setattr(sdo_point, 'Y', y)
                    #     update_oracle_record(oracle_record, identifier, table_name, schema, curs)

                    # else:
                    # line geometry or a polygon. Handle line geometry for now
                    old_geometry = oracle_record.SDO_ORDINATES
                    line_coords = old_geometry.aslist()
                    # make a list of adjusted coordinates and set it to oracle object.
                    # before that, remove old geometries from the object.
                    old_geometry.trim(len(line_coords))
                    # new_geometry = [float(element) for element in adjusted_geometry]
                    new_geometry = []

                    for ind, element in enumerate(adjusted_geometry):
                        if (ind + 1) % factor != 0:
                            new_geometry.extend([float(element)])

                    old_geometry.extend(new_geometry)
                    update_oracle_record(oracle_record, identifier, table_name, schema, curs)
                else:
                    print(table_name + "    " + identifier + "    mismatch in the geometry")

        except Exception, e:
            write_stream.write(str(e) + '\t' + str(identifier) + "\n")

    # Assuming required changes been applied to DB, commit changes and close
    # the connections
    print( " Excluded  " , out_count , " Processed " , counter )
    connection.commit()
    curs.close()
    connection.close()
    write_stream.close()
    read_stream.close()


def int_update(a_line, table_name, schema, curs, write_stream):
    ##
    ##

    ''' split it by |. Info at index 0 is unique identifier string
     and at 1 is a string of oracle, adjust.IT geometries separated
     by $ '''
    info = a_line.split("|")
    identifier = info[0]
    geoms = info[1].split("$")
    original_geometry = geoms[0].split(',')
    adjusted_geometry = geoms[1].split(',')

    # source data is now in 3D and extra coordinate info i.e 1.0, 0.0 which
    # will not have any adjusted coordinate and will get a NoShift comment.
    # These checks shall now be cancelled.
    # if 'OUT' in adjusted_geometry or 'OUT\n' in adjusted_geometry:
    #     log.write(a_line)
    #     out_count += 1
    #     continue

    try:
        # counter += 1
        # if counter % 10000 == 0:
        #     print("Processed   " + str(counter) + "  records. Time taken   ", time.clock() - start)
        # start = time.clock()

        sql_string = "SELECT G3E_GEOMETRY FROM " + schema + "." + table_name + \
                     " WHERE ROWID = CHARTOROWID('" + identifier + "')"

        curs.execute(sql_string)
        res = curs.fetchall()

        for a_rec in res:
            # points and lines have different attributes/methods to set geometries
            # so separate the process based on geometry type
            oracle_record = a_rec[0]

            # Coordinate array for point geometries is differently maintained in 3D format
            # for example a pole geometry is maintained like (1234.3467, 3456.356, 0,1,0,0)
            if oracle_record.SDO_GTYPE == 3001:
                factor = 3
            else:
                factor = 4

            # oracle and adjusted geometries should be of the size - same
            # number of vertices
            if len(adjusted_geometry) == len(original_geometry) + (len(original_geometry) / (factor - 1)):
                # if oracle_record.SDO_GTYPE == 4001:
                #     # point geometry
                #     # get SDO_POINT type and set x, y values
                #     sdo_point = oracle_record.SDO_POINT
                #     new_geometry = adjusted_geometry
                #     x = float(new_geometry[0])
                #     y = float(new_geometry[1])
                #
                #     setattr(sdo_point, 'X', x)
                #     setattr(sdo_point, 'Y', y)
                #     update_oracle_record(oracle_record, identifier, table_name, schema, curs)

                # else:
                # line geometry or a polygon. Handle line geometry for now
                old_geometry = oracle_record.SDO_ORDINATES
                line_coords = old_geometry.aslist()
                # make a list of adjusted coordinates and set it to oracle object.
                # before that, remove old geometries from the object.
                old_geometry.trim(len(line_coords))
                # new_geometry = [float(element) for element in adjusted_geometry]
                new_geometry = []

                for ind, element in enumerate(adjusted_geometry):
                    if (ind + 1) % factor != 0:
                        new_geometry.extend([float(element)])

                old_geometry.extend(new_geometry)
                update_oracle_record(oracle_record, identifier, table_name, schema, curs)
            else:
                print(table_name + "    " + identifier + "    mismatch in the geometry")

    except Exception, e:
        write_stream.write(str(e) + '\t' + str(identifier) + "\n")

    # Assuming required changes been applied to DB, commit changes and close
    # the connections
    #print( " Excluded  " , out_count , " Processed " , counter )
    #connection.commit()


def revert(table_name):

    # get the file name
    main_dir = os.path.dirname(__file__)
    write_stream = open(os.path.join(main_dir, 'rollback.log'), "a")
    data_file = os.path.join(main_dir, 'extracts\\' + table_name + '.txt')

    if table_name == "ROW":
        table_name = '"ROW"'

    # get file object
    read_stream = open(data_file, "r")

    ''' read file and parse it to get old and new geometries. The geometry
     from oracle table is separated by a pipe (|) and the geometry from
     adjust.IT is separated by a dollar ($) '''

    # define a counter for status
    counter = 0
    connection, schema = get_connection_details()
    curs = connection.cursor()
    start = time.clock()

    for a_line in read_stream:

        ''' split it by |. Info at index 0 is unique identifier string
         and at 1 is a string of oracle, adjust.IT geometries separated
         by $ '''
        info = a_line.split("|")
        identifier = info[0]
        # extract file has trailing new line character and comma at the end of
        # each line. Strip them out before processing it for updates
        original_geometry = info[1].rstrip().rstrip(",").split(',')

        try:
            counter += 1
            if counter % 10000 == 0:
                print("Processed   " + str(counter) + "  records. Time taken   ", time.clock() - start)
            start = time.clock()

            # oracle and adjusted geometries should be of the size - same
            # number of vertices
            sql_string = "SELECT G3E_GEOMETRY FROM " + schema + "." + table_name + \
                         " WHERE ROWID = CHARTOROWID('" + identifier + "')"
            curs.execute(sql_string)
            res = curs.fetchall()

            for a_rec in res:
                # points and lines have different attributes/methods to set geometries
                # so separate the process based on geometry type
                oracle_record = a_rec[0]
                old_geometry = oracle_record.SDO_ORDINATES
                line_coords = old_geometry.aslist()

                # make a list of adjusted coordinates and set it to oracle object.
                # before that, remove old geometries from the object.
                old_geometry.trim(len(line_coords))
                new_geometry = [float(element) for element in original_geometry]

                old_geometry.extend(new_geometry)
                update_oracle_record(oracle_record, identifier, table_name, schema, curs)

        except Exception, e:
            write_stream.write(str(e) + '\t' + str(identifier) + "\n")

    # Assuming required changes been applied to DB, commit changes and close
    # the connections
    print(" Processed ", counter)
    connection.commit()
    curs.close()
    connection.close()
    write_stream.close()
    read_stream.close()


def update_oracle_record(record, identifier, table_name, schema, curs):

    update_string = 'UPDATE ' + schema + '.' + table_name + \
                    ' SET G3E_GEOMETRY = :1 WHERE ROWID = CHARTOROWID(:2 )'
    # print(update_string)
    # connection = curs.connection
    # typeObj = connection.gettype("SDO_GEOMETRY")
    # elementInfoTypeObj = connection.gettype("SDO_ELEM_INFO_ARRAY")
    # ordinateTypeObj = connection.gettype("SDO_ORDINATE_ARRAY")
    # obj = typeObj.newobject()
    # obj.SDO_GTYPE = record.SDO_GTYPE
    # obj.SDO_ELEM_INFO = elementInfoTypeObj.newobject()
    # obj.SDO_ELEM_INFO.extend(record.SDO_ELEM_INFO.aslist())
    # obj.SDO_ORDINATES = ordinateTypeObj.newobject()
    # obj.SDO_ORDINATES.extend(record.SDO_ORDINATES.aslist())
    #print(obj.SDO_ORDINATES.aslist())
    #print(obj.SDO_ELEM_INFO.aslist())
    curs.execute(update_string, (record, identifier))


def oracle_object_info(obj):

    if obj.type.iscollection:
        details = []

        for value in obj.aslist():
            if isinstance(value, cx_Oracle.Object):
                value = oracle_object_info(value)
            details.append("{:10f}".format(value))
    else:
        details = {}

        for attr in obj.type.attributes:
            value = getattr(obj, attr.name)

            if value is None:
                details[attr.name] = None
                continue
            elif isinstance(value, cx_Oracle.Object):
                value = oracle_object_info(value)
            details[attr.name] = value

    return details


def get_coordinates(geometry):

    attributes = oracle_object_info(geometry)
    if attributes['SDO_POINT'] is None:
        return attributes['SDO_ORDINATES']
    else:
        coords = attributes['SDO_POINT']
        return [coords['X'], coords['Y']]


if __name__ == "__main__":
    sys.exit(int(main() or 0))
