set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2684, 'ONCOR_JIRA1854-XfmrUG_TieXMfrID');
spool c:\temp\2684-20180711-ONCOR_JIRA1854-XfmrUG_TieXMfrID.log
--**************************************************************************************
--SCRIPT NAME: 2684-20180711-ONCOR_JIRA1854-XfmrUG_TieXMfrID.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 11-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1854
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Remove attribute "Tie Transformar ID" from Placement and Edit dialog tab
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Delete from g3e_tabattribute where g3e_ano=(select g3e_ano from g3e_attribute where g3e_field='TIE_XFMR_ID' and g3e_cno=6002) and g3e_readonly=0;

COMMIT;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2684);

