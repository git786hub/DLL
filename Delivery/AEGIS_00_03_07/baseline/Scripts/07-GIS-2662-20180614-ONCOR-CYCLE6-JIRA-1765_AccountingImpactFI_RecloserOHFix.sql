
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2662, '1765_AccountingImpactFI_RecloserOHFix');
spool c:\temp\2662-20180614-ONCOR-CYCLE6-JIRA-1765_AccountingImpactFI_RecloserOHFix.log
--**************************************************************************************
--SCRIPT NAME: 2662-20180614-ONCOR-CYCLE6-JIRA-1765_AccountingImpactFI_RecloserOHFix.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 14-JUN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

UPDATE G3E_PLACEMENTCONFIGURATION SET G3E_RINO=(SELECT G3E_RINO FROM G3E_RELATIONINTERFACE WHERE G3E_USERNAME='Accounting Impact') WHERE G3E_FNO IN (SELECT DISTINCT G3E_FNO FROM G3E_FEATURECOMPONENT WHERE G3E_CNO IN (21,22,19001)) AND G3E_RINO IS NULL;

COMMIT;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2662);

