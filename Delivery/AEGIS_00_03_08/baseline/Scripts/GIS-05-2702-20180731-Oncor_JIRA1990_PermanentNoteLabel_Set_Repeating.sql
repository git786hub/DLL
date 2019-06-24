set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2702, 'Oncor_JIRA1990_PermanentNoteLabel_Set_Repeating');
spool c:\temp\2702-20180731-Oncor_JIRA1990_PermanentNoteLabel_Set_Repeating.log
--**************************************************************************************
--SCRIPT NAME: 2702-20180731-Oncor_JIRA1990_PermanentNoteLabel_Set_Repeating.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 31-JUL-2018
-- CYCLE				: 00.03.08
-- JIRA NUMBER			: 1990
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Set Permanent Note Label feature component to repeating
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Update g3e_featurecomponent set g3e_repeating=1 where g3e_cno=22501 and g3e_fno=225;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2702);

