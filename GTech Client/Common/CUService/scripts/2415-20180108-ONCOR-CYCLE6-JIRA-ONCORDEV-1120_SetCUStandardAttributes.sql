
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2415, 'ONCORDEV-1120_SetCUStandardAttributes');
spool c:\temp\2415-20180108-ONCOR-CYCLE6-JIRA-ONCORDEV-1120_SetCUStandardAttributes.log
--**************************************************************************************
--SCRIPT NAME: 2415-20180108-ONCOR-CYCLE6-JIRA-ONCORDEV-1120_SetCUStandardAttributes.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 08-JAN-2018
-- CYCLE			: 6

-- JIRA NUMBER		: 1120
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create entry for the new FI which sets CU Standard Attributes when CU_C is set
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

REM INSERTING into G3E_FUNCTIONALINTERFACE
SET DEFINE OFF;

execute Create_Sequence.Create_Metadata_Sequences;

SET DEFINE OFF;
Insert into G3E_FUNCTIONALINTERFACE (G3E_FINO,G3E_USERNAME,G3E_INTERFACE,G3E_ARGUMENTPROMPT,G3E_ARGUMENTTYPE,G3E_EDITDATE,G3E_DESCRIPTION) values (G3E_FUNCTIONALINTERFACE_SEQ.nextval,'Set CU Standard Attributes','fiSetCUStandardAttributes:GTechnology.Oncor.CustomAPI.fiSetCUStandardAttributes',null,null,sysdate,null);

UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Set CU Standard Attributes'),G3E_FUNCTIONALORDINAL=3,G3E_FUNCTIONALTYPE='SetValue',G3E_INTERFACEARGUMENT= null WHERE G3E_CNO = 21 AND G3E_FIELD='CU_C';

UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Set CU Standard Attributes'),G3E_FUNCTIONALORDINAL=3,G3E_FUNCTIONALTYPE='SetValue',G3E_INTERFACEARGUMENT= null WHERE G3E_CNO = 22 AND G3E_FIELD='CU_C';

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2415);

