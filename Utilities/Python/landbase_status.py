"""

"""

import cx_Oracle

from settings import dev_server, sbx_server, pts_server, tst_server, crt_server, user, dev_pw, sbx_pw, pts_pw, tst_pw, crt_pw

# Connection strings for all active and available environments
# DEV
dev_con_str = "{}/{}@{}/DGSDEV".format(user,dev_pw, dev_server)
# SBX
sbx_con_str = '{}/{}@{}/DGSSBX'.format(user,sbx_pw, sbx_server)
# PTS
pts_con_str = '{}/{}@{}/DGSPTS'.format(user,pts_pw, pts_server)
# TST
tst_con_str = '{}/{}@{}/DGSTST1'.format(user,tst_pw, tst_server)
# CRT
crt_con_str = '{}/{}@{}/DGSCRT'.format(user,crt_pw, crt_server)

# list of all landbase tables
table_list = ["GIS.B$AIRPORT_AR",
"GIS.B$AIRPORT_N",
"GIS.B$AIRPORT_T",
"GIS.B$BUILDING_CL_AR",
"GIS.B$BUILDING_CL_N",
"GIS.B$BUILDING_CL_T",
"GIS.B$BUILDING_DET_AR",
"GIS.B$BUILDING_ML_AR",
"GIS.B$BUILDING_ML_DET_T",
"GIS.B$BUILDING_ML_N",
"GIS.B$BUILDING_ML_T",
"GIS.B$CENSUS_BNDY_AR",
"GIS.B$CENSUS_BNDY_N",
"GIS.B$CENSUS_BNDY_T",
"GIS.B$CERT_BNDY_AR",
"GIS.B$CERT_BNDY_N",
"GIS.B$COUNTY_AR",
"GIS.B$COUNTY_N",
"GIS.B$COUNTY_T",
"GIS.B$DEM_LN",
"GIS.B$DEM_N",
"GIS.B$DETAILIND_T",
"GIS.B$HYDROLOGY_AR",
"GIS.B$HYDROLOGY_LN",
"GIS.B$HYDROLOGY_N",
"GIS.B$HYDROLOGY_T",
"GIS.B$HYPERLINK_N",
"GIS.B$INSTITUTION_N",
"GIS.B$INSTITUTION_S",
"GIS.B$INSTITUTION_T",
"GIS.B$LANDMARK_AR",
"GIS.B$LANDMARK_N",
"GIS.B$LANDMARK_T",
"GIS.B$LAND_AUDIT_N",
"GIS.B$MANMADE_LN",
"GIS.B$MISC_LABEL_T",
"GIS.B$MUNICIPALITY_AR",
"GIS.B$MUNICIPALITY_N",
"GIS.B$OPERBNDY_AR",
"GIS.B$OPERBNDY_N",
"GIS.B$OPERBNDY_T",
"GIS.B$PARCEL_CL_AR",
"GIS.B$PARCEL_CL_N",
"GIS.B$PARCEL_CL_S",
"GIS.B$PARCEL_CL_T",
"GIS.B$PARCEL_ML_AR",
"GIS.B$PARCEL_ML_N",
"GIS.B$PARCEL_ML_S",
"GIS.B$PARCEL_ML_T",
"GIS.B$PIPELINE_CL_LN",
"GIS.B$PIPELINE_CL_N",
"GIS.B$PIPELINE_CL_T",
"GIS.B$PIPELINE_ML_LN",
"GIS.B$PIPELINE_ML_N",
"GIS.B$PIPELINE_ML_T",
"GIS.B$POLITICALBNDY_AR",
"GIS.B$POLITICALBNDY_N",
"GIS.B$POLITICALBNDY_T",
"GIS.B$RAILROAD_LN",
"GIS.B$RAILROAD_N",
"GIS.B$RAILROAD_T",
"GIS.B$RESTRICTED_AR",
"GIS.B$RESTRICTED_N",
"GIS.B$ROW_LN",
"GIS.B$ROW_N",
"GIS.B$STREETCTR_CL_LN",
"GIS.B$STREETCTR_CL_N",
"GIS.B$STREETCTR_CL_T",
"GIS.B$STREETCTR_ML_LN",
"GIS.B$STREETCTR_ML_N",
"GIS.B$STREETCTR_ML_T",
"GIS.B$STREETNAME_CL_N",
"GIS.B$TAXDISTRICT_AR",
"GIS.B$TAXDISTRICT_N",
"GIS.B$TAXDISTRICT_T",
"GIS.B$TELECOMBNDY_AR",
"GIS.B$TELECOMBNDY_N",
"GIS.B$TELECOMBNDY_T",
"GIS.B$TEXAS_SECTION_AR",
"GIS.B$TEXAS_SECTION_N",
"GIS.B$TEXAS_SECTION_T",
"GIS.B$THIRD_PARTY_AR",
"GIS.B$THIRD_PARTY_N",
"GIS.B$THIRD_PARTY_T",
"GIS.B$TRANSIT_N",
"GIS.B$TRANSIT_S",
"GIS.B$TRANSIT_T",
"GIS.B$TXSTATE_AR",
"GIS.B$TXSTATE_N",
"GIS.B$ZIPCODE_AR",
"GIS.B$ZIPCODE_N",
"GIS.B$ZIPCODE_T"]

# initialize arrays and set variables
stmts = []
stmt  = 'select count(*) from '
border = "-" * 80
results = []

# function for connecting and executing queries
def landbase_status(connection_string, statement, env, result_array):
    border = "-" * 15
    status = "Checking: {}".format(env)
    connect = "Connecting to {}...".format(env)
    print(border)
    try:
        print(connect)
        connection = cx_Oracle.connect(connection_string)
        cursor = connection.cursor()
    except:
        print("Unable to connect.")
    for tab in table_list:
        sm = stmt + tab
        cursor.execute(str(sm))
        # unpack results of query
        for result in cursor:
            if int(result[0]) == 0:
                result_array.append([env, tab, "Empty"])
            else:
                result_array.append([env, tab, "Present"])
    # close connection
    connection.close()

#execute the queries that test for emptiness
# landbase_status(dev_con_str,stmt,"DEV", results)
# landbase_status(sbx_con_str,stmt,"SBX", results)
landbase_status(pts_con_str,stmt,"PTS", results)
# landbase_status(tst_con_str,stmt,"TST", results)
# landbase_status(crt_con_str,stmt,"CRT", results)

#print out the headers
print("\n\n")
print("Environment\tTable\t\t\tStatus")
print("-" * 11 + "\t" + "-" * 5 + "\t\t\t" + "-" * 6)

#print out the results
count = 0
for result in results:
    if len(result[1]) > 15:
        print(result[0] + "\t\t" + result[1] + "\t" + result[2])
        count += 1
    else:
        print(result[0] + "\t\t" + result[1] + "\t\t" + result[2])
        count += 1
    if count == 92:
        count = 0
        print("-" * 47)