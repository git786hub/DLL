** README ** 

-- IMPORTANT --
The file "post_extract_filter.py" should be placed in the SAME DIRECTORY
as the MANHOLE text files from the extract. Look for these filenames:'B$MANHOLE_N_WO_EXCLUSIONS.txt',
'B$MANHOLE_S_WO_EXCLUSIONS.txt', 'B$MANHOLE_T_WO_EXCLUSIONS.txt','B$MANHOLE_ID_T_WO_EXCLUSIONS.txt'.


-- SUMMARY -- 
This module reads the text files from the extract files
(output of the network_adjustments.py script) that have rowids,
g3e_fids and coordinates. Then queries the g3e_fids that should be
excluded. Then creates new files that exlude those g3e_fids.

It can be imported and used as a module, or ran independently for manhole files
as long as this script is ran in the same DIR as the text files.

-- MODULE USE EXAMPLE --

import post_extract_filter as pef

g3e_fids_to_remove = pef.excluded_g3efids("<SCHEMA>/<PASSWORD>@<HOSTNAME>:<PORT>/<DB_NAME>", 
			"SELECT G3E_FID FROM GIS.F2G_MAP_DN WHERE G3E_FNO = <FNO_TO_EXCLUDE>)

pef.exclude(g3e_fids_to_remove, 
	    ["<TABLE_NAME_1>.txt","<TABLE_NAME_2>.txt" ... ], 
	    ["<NEW_TABLE_NAME_1>.txt", "<NEW_TABLE_NAME_2>.txt" ...])

