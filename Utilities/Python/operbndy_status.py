import cx_Oracle
import math

# database connections
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

# statement to check the count
stmt  = 'select count(*) from gis.b$operbndy_n opn join gis.b$operbndy_ar opa on opn.g3e_fid = opa.g3e_fid'
border = "-" * 80
results = []

# function for executing queries and gathering results
def operbndy_status(connection_string, statement, env, result_array):
    border = "-" * 15
    status = "Checking: {}".format(env)
    connect = "Connecting..."
    print(border)
    try:
        print(connect)
        connection = cx_Oracle.connect(connection_string)
        cursor = connection.cursor()
        cursor.execute(str(stmt))
    except:
        print("Failed to connect.")
    try:
        print(status)
        for result in cursor:
            if int(result[0]) == 0:
                result_array.append([env,"Empty"])
            else:
                result_array.append([env,"Present"])
    except:
        print("Failed to parse results")
    
    connection.close()

# execute functions
operbndy_status(dev_con_str,stmt,"DEV", results)
operbndy_status(sbx_con_str,stmt,"SBX", results)
operbndy_status(pts_con_str,stmt,"PTS", results)
operbndy_status(tst_con_str,stmt,"TST", results)
operbndy_status(crt_con_str,stmt,"CRT", results)

# print out headers
print("\n\n")
print("Environment\tOp. Boundaries Status")
print("-" * 11 + "\t" + "-" * 21)

# print out results
for result in results:
    print(result[0] + "\t\t" + result[1])