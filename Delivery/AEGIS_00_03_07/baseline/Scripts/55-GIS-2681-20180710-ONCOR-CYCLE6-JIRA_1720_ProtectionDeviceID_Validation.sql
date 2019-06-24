set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2681, '1720_ProtectionDeviceID_Validation');
spool c:\temp\2681-20180710-ONCOR-CYCLE6-JIRA_1720_ProtectionDeviceID_Validation.log
--**************************************************************************************
--SCRIPT NAME: 2681-20180710-ONCOR-CYCLE6-JIRA_1720_ProtectionDeviceID_Validation.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE			: 10-JUL-2018
-- CYCLE		: 6

-- JIRA NUMBER		:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to delete duplicate entries for the Protective Device ID Validation
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

delete from G3E_ATTRIBUTEVALIDATION a where a.rowid > any (select b.rowid from G3E_ATTRIBUTEVALIDATION b where a.g3e_activefno = b.g3e_activefno and a.g3e_relatedfno = b.g3e_relatedfno and a.g3e_activeano = b.g3e_relatedano) and a.g3e_vino=(select g3e_vino from g3e_validationinterface where g3e_username = 'Protective DeviceID Validation');

COMMIT;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2681);

