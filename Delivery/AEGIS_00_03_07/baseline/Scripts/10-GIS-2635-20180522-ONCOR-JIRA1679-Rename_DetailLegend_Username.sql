
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2635, 'ONCOR-JIRA1679-Rename_DetailLegend_Username');
spool c:\temp\2635-20180522-ONCOR-JIRA1679-Rename_DetailLegend_Username.log
--**************************************************************************************
--SCRIPT NAME: 2635-20180522-ONCOR-JIRA1679-Rename_DetailLegend_Username.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 22-MAY-2018
-- CYCLE			: 03.06
-- JIRA NUMBER		: 1679
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Rename "Detail Distribution Legend" to "Detail Design Legend"
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

UPDATE G3E_LEGENDSETTINGS SET G3E_USERNAME='Detail Design Legend' WHERE G3E_USERNAME='Detail Distribution Legend';

COMMIT;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2635);

