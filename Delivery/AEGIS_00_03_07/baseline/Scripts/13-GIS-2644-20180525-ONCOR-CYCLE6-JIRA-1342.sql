set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2644, '1342');
spool c:\temp\2644-20180525-ONCOR-CYCLE6-JIRA-1342.log
--**************************************************************************************
--SCRIPT NAME: 2644-20180525-ONCOR-CYCLE6-JIRA-1342.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\UAVOTE
-- DATE		: 25-MAY-2018
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

update G3E_FINDINTERSECTION set g3e_Role = 'NOBODY' where g3e_username ='Find Street Intersection';

update G3E_FINDMETHOD set g3e_Role = 'NOBODY' where g3e_username  ='Intersecting Street Segments';

COMMIT;

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2644);