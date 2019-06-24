set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2647, '1714');
spool c:\temp\2647-20180529-ONCOR-CYCLE6-JIRA-1714.log
--**************************************************************************************
--SCRIPT NAME: 2647-20180529-ONCOR-CYCLE6-JIRA-1714.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\UAVOTE
-- DATE		: 29-MAY-2018
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

--Increase the data size of SHEETNAME attribute to VARCHAR2(80) on Tax District non-graphic component.
ALTER TABLE B$TAXDISTRICT_N modify SHEETNAME VARCHAR2(80);

execute create_triggers.create_ltt_trigger('B$TAXDISTRICT_N');
execute create_views.create_ltt_view('B$TAXDISTRICT_N');
execute GDOTRIGGERS.CREATE_GDOTRIGGERS('TAXDISTRICT_N');

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;
execute MG3ECreateOptableViews;
execute ComponentQuery.Generate;
execute ComponentViewQuery.Generate;
execute GDOTRIGGERS.CREATE_GDOTRIGGERS;
execute StaticPost.Generate;
EXECUTE G3E_DynamicProcedures.Generate;
--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2647);