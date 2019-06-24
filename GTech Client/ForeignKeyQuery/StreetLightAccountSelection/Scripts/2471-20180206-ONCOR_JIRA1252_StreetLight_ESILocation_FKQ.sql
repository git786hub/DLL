
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2471, 'ONCOR_JIRA1252_StreetLight_ESILocation_FKQ');
spool c:\temp\2471-20180206-ONCOR_JIRA1252_StreetLight_ESILocation_FKQ.log
--**************************************************************************************
--SCRIPT NAME: 2471-20180206-ONCOR_JIRA1252_StreetLight_ESILocation_FKQ.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 06-FEB-2018
-- CYCLE			: 6
-- JIRA NUMBER		: 1252
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: configure Street Light Account Foreign Query interface
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Insert into G3E_RELATIONINTERFACE (G3E_RINO,G3E_USERNAME,G3E_INTERFACE,G3E_TYPE,G3E_EDITDATE,G3E_DESCRIPTION) values (G3E_RELATIONINTERFACE_SEQ.NEXTVAL,'STLT_ESILocationSelection','fkqStreetLightAccount:GTechnology.Oncor.CustomAPI.fkqStreetLightAccount','Foreign Key Query',SYSDATE,'Query Street Light Account table to select ESI Location');

Update G3E_ATTRIBUTE set G3E_FKQRINO=(Select G3E_RINO from G3E_RELATIONINTERFACE where G3E_USERNAME='STLT_ESILocationSelection') where G3E_FIELD='ACCOUNT_ID' and G3E_CNO=(select G3E_CNO from G3E_COMPONENT where G3E_NAME='STREETLIGHT_N');

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2471);

