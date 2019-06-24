import os
from operator import itemgetter
from itertools import groupby

import cx_Oracle
from openpyxl import load_workbook

# Global variables/constants
CURRENT_WORKING_DIRECTORY = os.getcwd()

#dev_con_str = 'GIS/gisdev#12345@LV00109.build.corp.oncor.com:1727/DGSDEV'
#connection = cx_Oracle.connect(connection_string)
#cursor = connection.cursor()
#connection, schema = get_connection_details()
#cursor = connection.cursor()
# cursor.execute(sql_string)

# Classes

class Table:
    def __init__(self, name, file, rows, column_names):
        self.name = name
        self.name = file
        self.rows = rows
        self.column_names = column_names

    def load(self, connection):
        cursor = connection.cursor()
        table_name = self.name
        for row in self.rows: 
            cursor.execute("INSERT INTO GIS." 
                            + table_name 
                            + " (" 
                            + ','.join(self.column_names) 
                            + ")" 
                            + " VALUES (" 
                            + ','.join(row) 
                            + ")"
            )
        cursor.execute("COMMIT")
        connection.close()

class Worksheet:
    def __init__(self, sheet, filename):
        self.sheet = sheet
        self.filename = filename


# Load .xlsx files, given a list of filenames.
def extract(files):
    worksheets = []
    for file in files:
        if ".xlsx" in str(file):
                wb = load_workbook(filename=str(file), read_only=True)
                # The .active attribute retrieves the active worksheet in the workbook.
                ws = Worksheet(wb.active, file)
                worksheets.append(ws)
    return worksheets


# Returns list of tables given list of Worksheet objects
def process(worksheets):
    table_list = []
    for worksheet in worksheets:
        sheet_rows = []
        sheet_headers = []
        for cell in worksheet.sheet[1]:
            sheet_headers.append(cell.value)
        for row in worksheet.sheet.rows:
            row_cell_values = []
            for cell in row:
                row_cell_values.append(cell.value)
            sheet_rows.append(row_cell_values)
        table = Table("name", worksheet.filename, sheet_rows, sheet_headers)
        table_list.append(table)
    return table_list


def get_mapping():
    sheet = extract(["Excel-to-Table_Mapping.xlsx"])[0]
    mapping = []
    # print(sheet)
    table = process([sheet])[0]
    #remove header from rows
    table.rows.pop(0)
    # sort by filename
    table.rows.sort(key=itemgetter(0))
    for row in table.rows:
        mapping.append(row)
    return mapping


def load(tables, connection):
    for table in tables:
        table.load(connection)


def uniques(_list):
    seen = set()
    uniq = []
    for x in _list:
        if x not in seen:
            uniq.append(x)
            seen.add(x)
    return uniq


def dupes(_list):
    seen = {}
    dupes = []

    for x in _list:
        if x not in seen:
            seen[x] = 1
        else:
            if seen[x] == 1:
                dupes.append(x)
            seen[x] += 1
    return dupes

# For each table, need to get the actual table_name from the mapping. 
# For each table that maps to the same worksheet, need to 
# def map(mapping, tables, worksheets):
#     for row in mapping:
#         # file = [t.file for t in tables if t.file == row[0]][0]
#     if mode == "multi_tab":
#         #remove header from rows
#         table.rows.pop(0)
#         # sort by filename
#         table.rows.sort(key=itemgetter(0))

#         #table_y = groupby(table.rows, itemgetter(0))
#         for elt, items in table_y:
#             for item in items:
#                 print(item)
    # elif mode == "normal":
    #     for row in table:
    #         print(row)
    #         4
    # else:
    #     print("Invalid mode supplied to map() method. Available modes are: \"normal\" and \"mutl_tab\".")


def debug(sheets):
    for sheet in sheets:
        for row in sheet.rows:
            row_cell_values = []
            for cell in row:
                row_cell_values.append(cell.value)
            print(str(row_cell_values))


# Main Method
def main():
    #(process(load()))
    #debug
    #print(process(extract(["Vault Equipment.xlsx","StreetLightCoordinates.xlsx"]))[0].column_names)
    #print(process(extract())[0].rows)
    #map("multi_tab")
    #process(extract([i[0] for i in get_mapping()]))
    # for i in get_mapping():
    #     print("{} : {}".format(i[0],i[1]))
    
    print(dupes([i[0] for i in get_mapping()]))
    print(dupes(['hi','hi','hi','hello','good morning']))


if __name__ == '__main__':
    main()

# For each .csv file
    # load the data into object/array
        # open a .csv file as a reader object
        # unpack
    # connect to oracle database (GIS schema owner ind dev for now, later we can use a service account) 
    # verify that the table exists (try-catch)
    # for row in CSV
        # build parameterized insert statement
        # execute insert statement
        # collect row count
    # execute commit
    # close connection
# output .log file to show:
    # 1 - How many tables where written to
    # 2 - How many records for each table were inserted
    # 3 - What table names where writtien to
    # 4 - If there were any errors, show them.