set echo on
set linesize 1000
set pagesize 300
set trimspool on
SET SERVEROUTPUT off
SPOOL SAUTOMATION_CLEANUP.LOG

Drop table SAUTOMATION_MAPPING_TEMP;
Drop table SAUTOMATION_CU_TYPE_TEMP;