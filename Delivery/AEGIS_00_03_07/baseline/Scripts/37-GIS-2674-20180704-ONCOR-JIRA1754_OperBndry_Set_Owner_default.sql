
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2674, 'ONCOR-JIRA1754_OperBndry_Set_Owner_default');
spool c:\temp\2674-20180704-ONCOR-JIRA1754_OperBndry_Set_Owner_default.log
--**************************************************************************************
--SCRIPT NAME: 2674-20180704-ONCOR-JIRA1754_OperBndry_Set_Owner_default.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 04-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1754
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Set OPERBNDY_N columns owner_fno and owner_fid default to 0
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************


Alter Table B$OPERBNDY_N modify OWNER_FID  NUMBER(10) DEFAULT 0;  
Alter Table B$OPERBNDY_N modify OWNER_FNO  NUMBER(10) DEFAULT 0;

execute GDOTRIGGERS.CREATE_GDOTRIGGERS; 
execute G3E_DynamicProcedures.Generate;

Alter table B$OPERBNDY_N disable all triggers;

Update B$OPERBNDY_N set OWNER_FID=0,OWNER_FNO=0 Where g3e_fno=208 and ltt_id=0;
COMMIT;

Alter table B$OPERBNDY_N enable all triggers;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2674);

