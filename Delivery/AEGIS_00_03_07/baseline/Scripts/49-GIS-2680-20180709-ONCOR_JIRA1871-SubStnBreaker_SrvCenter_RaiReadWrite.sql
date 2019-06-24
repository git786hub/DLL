set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2680, 'ONCOR_JIRA1871-SubStnBreaker_SrvCenter_RaiReadWrite');
spool c:\temp\2680-20180709-ONCOR_JIRA1871-SubStnBreaker_SrvCenter_RaiReadWrite.log
--**************************************************************************************
--SCRIPT NAME: 2680-20180709-ONCOR_JIRA1871_SubStnBreaker_SrvCenter_RaiReadWrite.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 09-JUL-2018
-- CYCLE			: 00.03.07
-- JIRA NUMBER		: 1871
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Configure "ReadWriteControl" interface on Service_Center attributes for SubStation Breaker and Substation Breaker Network to edit attribute by user with PRIV_MGMT_SSTA role.
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values
(G3E_RELATIONIFACEARGS_SEQ.NEXTVAL,G3E_RELATIONIFACEARGS_SEQ.CURRVAL,1,'PRIV_MGMT_SSTA',SYSDATE);

update g3e_Attribute set G3E_RINO = (select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'Read Write Control Definition'),G3E_RIARGGROUPNO = (select G3E_RIARGGROUPNO from G3E_RELATIONIFACEARGS where g3e_value = 'PRIV_MGMT_SSTA') where g3e_cno = (SELECT G3E_CNO FROM G3E_COMPONENT WHERE  G3E_NAME='SUB_BREAKER_N') and g3e_field = 'SERVICE_CENTER_C';

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2680);

