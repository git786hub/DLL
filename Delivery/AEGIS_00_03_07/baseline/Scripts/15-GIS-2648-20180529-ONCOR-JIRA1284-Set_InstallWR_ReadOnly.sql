

set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2648, 'ONCOR-JIRA1284-Set_InstallWR_ReadOnly');
spool c:\temp\2648-20180529-ONCOR-JIRA1284-Set_InstallWR_ReadOnly.log
--**************************************************************************************
--SCRIPT NAME: 2648-20180529-ONCOR-JIRA1284-Set_InstallWR_ReadOnly.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 29-MAY-2018
-- CYCLE				: 03.06
-- JIRA NUMBER			: 1284	
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Set atribute "Install WR" to readonly 
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---**Set attribute Install WR to readonly on Ancillary CU Attribute tab***
Update g3e_tabattribute set g3e_readonly=1 where g3e_ano=2201 and g3e_dtno=61206;
---**Set attribute Install WR to readonly on CU Attribute tab***
Update g3e_tabattribute set g3e_readonly=1 where g3e_ano=2101 and g3e_dtno in (61215,61205);

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2648);

