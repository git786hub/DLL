
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2682, 'ONCOR_JIRA1900-PPOD_Stylerule_Filter_Fix');
spool c:\temp\2682-20180711-ONCOR_JIRA1900-PPOD_Stylerule_Filter_Fix.log
--**************************************************************************************
--SCRIPT NAME: 2682-20180711-ONCOR_JIRA1900-PPOD_Stylerule_Filter_Fix.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 11-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1900
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Update PPOD Style rule filter to fix sybmbol appearing char A on Map window instead of actual symbol
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---**Style Rule :Primary Point of Delivery Symbol***
Update G3E_stylerule set g3e_filter=replace(g3e_filter,'PME','''PME''') WHERE G3E_RULE='Primary Point of Delivery Symbol';
Update G3E_stylerule set g3e_filter=replace(g3e_filter,'SME','''SME''') WHERE G3E_RULE='Primary Point of Delivery Symbol';

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2682);

