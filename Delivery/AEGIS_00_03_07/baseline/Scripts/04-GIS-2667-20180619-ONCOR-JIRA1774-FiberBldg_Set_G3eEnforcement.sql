
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2667, 'ONCOR-JIRA1774-FiberBldg_Set_G3eEnforcement');
spool c:\temp\2667-20180619-ONCOR-JIRA1774-FiberBldg_Set_G3eEnforcement.log
--**************************************************************************************
--SCRIPT NAME: 2667-20180619-ONCOR-JIRA1774-FiberBldg_Set_G3eEnforcement.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 19-JUN-2018
-- CYCLE			: 00.03.07
-- JIRA NUMBER		: 1774
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	    : set connectivity optional for fiber building 
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Update G3E_NODEEDGECONNECTIVITY Set G3e_Enforcement=0 where G3e_SourceFno=14100;
Update G3E_NODEEDGECONNECTIVITY Set G3e_Enforcement=0 where G3e_ConnectingFno=14100;

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2667);

