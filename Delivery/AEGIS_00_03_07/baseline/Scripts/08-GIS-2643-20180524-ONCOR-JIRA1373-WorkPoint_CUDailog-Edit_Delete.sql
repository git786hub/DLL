
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2643, 'ONCOR-JIRA1373-WorkPoint_CUDailog-Edit_Delete');
spool c:\temp\2643-20180524-ONCOR-JIRA1373-WorkPoint_CUDailog-Edit_Delete.log
--**************************************************************************************
--SCRIPT NAME: 2643-20180524-ONCOR-JIRA1373-WorkPoint_CUDailog-Edit_Delete.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 24-MAY-2018
-- CYCLE			: 03.06
-- JIRA NUMBER		: 1373
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Delete CU Attribute Edit dialog tab from Workpoint
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************


Delete from g3e_dialog where g3e_fno=191 and g3e_type='Edit' and g3e_dtno=191202;
Delete from g3e_dialogtab where g3e_dtno=191202;

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2643);

