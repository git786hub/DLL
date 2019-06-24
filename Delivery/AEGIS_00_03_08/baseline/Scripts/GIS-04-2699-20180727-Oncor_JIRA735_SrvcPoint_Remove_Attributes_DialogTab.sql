set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2699, 'Oncor_JIRA735_SrvcPoint_Remove_Attributes_DialogTab');
spool c:\temp\2699-20180727-Oncor_JIRA735_SrvcPoint_Remove_Attributes_DialogTab.log
--**************************************************************************************
--SCRIPT NAME: 2699-20180727-Oncor_JIRA735_SrvcPoint_Remove_Attributes_DialogTab.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 27-JUL-2018
-- CYCLE			: 00.03.08
-- JIRA NUMBER		: 735
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Remove 'CITY_C','COUNTY_C','ZIP_C' from Service Point Attribute dialog tab
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Delete from g3e_tabattribute where g3e_ano in (select g3e_ano from g3e_attribute where g3e_field in ('CITY_C','COUNTY_C','ZIP_C') and g3e_cno=5501) 
and g3e_dtno in (select g3e_dtno from g3e_dialogtab where g3e_username='Service Point Attributes');

Delete from g3e_attribute where g3e_field in ('CITY_C','COUNTY_C','ZIP_C') and g3e_cno=5501;

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;
execute MG3ECreateOptableViews;

execute GDOTRIGGERS.CREATE_GDOTRIGGERS; 
execute G3E_DynamicProcedures.Generate;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2699);

