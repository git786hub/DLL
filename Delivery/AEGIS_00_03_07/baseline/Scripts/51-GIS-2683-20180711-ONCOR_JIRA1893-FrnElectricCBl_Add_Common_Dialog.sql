
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2683, 'ONCOR_JIRA1893-FrnElectricCBl_Add_Common_Dialog');
spool c:\temp\2683-20180711-ONCOR_JIRA1893-FrnElectricCBl_Add_Common_Dialog.log
--**************************************************************************************
--SCRIPT NAME: 2683-20180711-ONCOR_JIRA1893-FrnElectricCBl_Add_Common_Dialog.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 11-JUL-2018
-- CYCLE			: 00.03.07
-- JIRA NUMBER		: 1893
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Add common dialog tab to Foreign electric Cable feature
-- SOURCE DATABASE	:
--**************************************************************************************

Insert into G3E_DIALOG (G3E_DROWNO,G3E_DNO,G3E_TYPE,G3E_FNO,G3E_ORDINAL,G3E_DTNO) values (186301,18630,'Placement',186,20,34303);
Insert into G3E_DIALOG (G3E_DROWNO,G3E_DNO,G3E_TYPE,G3E_FNO,G3E_ORDINAL,G3E_DTNO) values (186201,18620,'Edit',186,20,34203);
Insert into G3E_DIALOG (G3E_DROWNO,G3E_DNO,G3E_TYPE,G3E_FNO,G3E_ORDINAL,G3E_DTNO) values (186101,18610,'Review',186,20,34103);

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2683);

