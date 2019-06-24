set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2685, 'ONCOR_JIRA1807-Floor_Color_Attr_Readonly');
spool c:\temp\2685-20180711-ONCOR_JIRA1816-Floor_Color_Attr_Readonly.log
--**************************************************************************************
--SCRIPT NAME: 2685-20180711-ONCOR_JIRA1816-Floor_Color_Attr_Readonly.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 11-JUL-2018
-- CYCLE			: 00.03.07
-- JIRA NUMBER		: 1816
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Set raster color attributes to readonly for floor features
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Update g3e_tabattribute set g3e_readonly=1 where g3e_ano in (select  g3e_ano from g3e_attribute where g3e_field in ('G3E_TRANSPARENTCOLORSRGB',
'G3E_TRANSPARENTCOLORSNDXORGRY')) and g3e_dtno in 
(select g3e_dtno from g3e_dialog where g3e_fno=14200);

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2685);
