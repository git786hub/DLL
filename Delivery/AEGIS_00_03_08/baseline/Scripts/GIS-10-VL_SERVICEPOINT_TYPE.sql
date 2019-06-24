set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\VL_SERVICEPOINT_TYPE.log
--**************************************************************************************
-- SCRIPT NAME: VL_SERVICEPOINT_TYPE.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 01-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Service Point Type value list
--**************************************************************************************

delete from VL_SERVICEPOINT_TYPE;

insert into VL_SERVICEPOINT_TYPE (VL_KEY, VL_VALUE, UNUSED, ORDINAL) values ('HOSPITAL',  'Hospital',             0, 1);
insert into VL_SERVICEPOINT_TYPE (VL_KEY, VL_VALUE, UNUSED, ORDINAL) values ('NONRES',    'Non-Residential',      0, 2);
insert into VL_SERVICEPOINT_TYPE (VL_KEY, VL_VALUE, UNUSED, ORDINAL) values ('RESSINGLE', 'Residential Single',   0, 3);
insert into VL_SERVICEPOINT_TYPE (VL_KEY, VL_VALUE, UNUSED, ORDINAL) values ('RESMULTI',  'Residential Multiple', 0, 4);


spool off;
