set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2640, '1364');
spool c:\temp\2640-20180524-ONCOR-CYCLE6-JIRA-1364.log
--**************************************************************************************
--SCRIPT NAME: 2640-20180524-ONCOR-CYCLE6-JIRA-1364.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\UAVOTE
-- DATE		: 24-MAY-2018
-- CYCLE		: 
-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************
-- {Add script code}

update g3e_attribute set g3e_username = 'Primary Voltage' where g3e_ano = 340203 and g3e_cno = 3402;
update g3e_attribute set g3e_username = 'Secondary Voltage' where g3e_ano = 340204 and g3e_cno = 3402;

COMMIT;

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;
--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2640);