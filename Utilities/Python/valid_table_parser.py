"""
This is a module that will parse through FRAMME and Network Converted Data and Sharyland Converted Data and pull out only valid tables that can be copied to a text file.
"""


import csv
import sys


__author__ ="Keith Wertsching"
__version__ ="1.0.0"
__status__="Development"


def CreateValidTableList():
        #Open up the log file based on what the user types in
        f = input("What is the name of the Log file?: ")   
        fr3 = open(f,"rt")
        PL_Read = fr3.readlines()
        PL = []
        for line in PL_Read:
                if "exported" in line:
                    PL.append(line.rstrip('\n'))
        

        #Create lists for total tables, invalid tables, and valid tables
        total_tables = []
        Invalid_Tables = []
        Valid_Tables = []
        for line in PL:
                if ". . exported" in line:
                        total_tables.append(line)
                        if "0 KB       0 rows" in line:
                                line2 = line.split('"')
                                item = line2[3]
                                item = line2[3].strip("$B")
                                Invalid_Tables.append(item)
                        else:
                                line2 = line.split('"')
                                item = line2[3]
                                item = line2[3].strip("$B")
                                Valid_Tables.append(item)
                                
                        

        #Front end to show all of the variables and let the user interact with the module
        print(len(total_tables), "tables were included in the truncate list")
        print("")
        print("")
        print(len(Invalid_Tables), "tables imported 0 rows with 0 KB of data, thus should be considered invalid")
        print("")
        print("")
        print(len(Valid_Tables), "tables imported valid rows with a non-zero amount of data, thus should be considered valid")
        prompt = ""
        while prompt != "quit":
            prompt = input("Would you like to see invalid or valid tables?: ")
            if prompt == "valid":
                print("Valid Tables:")
                print("")
                for table in Valid_Tables:
                    print(table)
            elif prompt == "invalid":
                print("Invalid Tables:")
                print("")
                for table in Invalid_Tables:
                    print(table)

CreateValidTableList()



#Ask about these tables
"""
F2G_SL_ORPHANS
F2G_SL_PROCESSED
F2G_ORPHANS_NONGRA_WR
F2G_ORPHANS_GRA_WR
F2G_INVALID_GRA_WR
"""

