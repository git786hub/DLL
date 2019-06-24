#!/usr/bin/env python
"""
This module is supposed to read the text files that have the rowids,
g3e_fids and coordinate. Then query the g3e_fids that should be
excluded. Then create a new files that exludes those g3e_fids. It can
be imported and used as a module, or ran independently for manhole files
as long as this script is ran in the same DIR as the text files.
"""
import os
import linecache

import cx_Oracle

__author__ = "Josh Mascorro"
__version__ = "1.0.0"
__status__ = "Development"

# Constants
SQL_QUERY = "select distinct g3e_fid from gis.f2g_map_dn where g3e_fno = 106"


# Methods
def main():
    # Query g3e_fids to remove and place in array
    g3e_fids_to_remove = excluded_g3efids(
        'GIS/gisdev#12345@LV00109.build.corp.oncor.com:1727/DGSDEV',
        SQL_QUERY)
    # Removes undesired records from manhole text files
    exclude(g3e_fids_to_remove,
            ['B$MANHOLE_N.txt',
            'B$MANHOLE_S.txt', 
            'B$MANHOLE_T.txt',
            'B$MANHOLE_ID_T.txt'],
            ['B$MANHOLE_N_WO_EXCLUSIONS.txt', 
            'B$MANHOLE_S_WO_EXCLUSIONS.txt', 
            'B$MANHOLE_T_WO_EXCLUSIONS.txt',
            'B$MANHOLE_ID_T_WO_EXCLUSIONS.txt'])


def excluded_g3efids(connection_string, sql_query):
    # Create connection to DB
    connection = cx_Oracle.connect(connection_string)
    # Create cursor instance
    cursor = connection.cursor()
    # Execute the query
    cursor.execute(sql_query)
    # Add all of the g3e_fids to an array -- validate not null
    g3e_fids = [row[0] for row in cursor if row[0] is not None]
    # Return g3e_fids to exclude
    return g3e_fids


def exclude(g3e_fids, filepaths, new_filepaths):
    # convert fids to strings
    g3e_fids_s = [str(i) for i in g3e_fids]
    # Iterate through each file_name string
    for f,f_new in zip(filepaths,new_filepaths):
        # If file cannot be found, continue with the list of files
        try:
            # create reader for original file
            original_file = open(f, 'r')
        except FileNotFoundError:
            print("The file: '" + f + "' cannot be found.")
            continue
        # create writer for new file
        new_file = open(f_new, 'w')
        # put all lines of txt file in an array
        original_lines = original_file.read().splitlines()
        for o_line in original_lines:
            # get g3e_fid from line
            g3e_fid = o_line.split('|')[1].split('#')[0]
            # check if g3e_fid is in the list of g3e_fids to exclude
            if g3e_fid in g3e_fids_s:
                continue
            else:   
                new_file.write(o_line)
                new_file.write("\n")
                
        original_file.close()
        new_file.close()


if __name__ == "__main__":
    main()