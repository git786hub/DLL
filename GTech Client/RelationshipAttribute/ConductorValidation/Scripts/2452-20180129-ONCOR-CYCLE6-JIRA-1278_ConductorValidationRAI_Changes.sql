set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2452, '1278_ConductorValidationRAI_Changes');
spool c:\temp\2452-20180129-ONCOR-CYCLE6-JIRA-1278_ConductorValidationRAI_Changes.log
--**************************************************************************************
--SCRIPT NAME: 2452-20180129-ONCOR-CYCLE6-JIRA-1278_ConductorValidationRAI_Changes.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 29-JAN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

DELETE FROM G3E_ATTRIBUTEVALIDATION WHERE g3e_activefno IN (53,63,96,97) AND G3E_VINO=(select g3e_vino from g3e_validationinterface where g3e_username = 'Conductor Validation');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2452);

