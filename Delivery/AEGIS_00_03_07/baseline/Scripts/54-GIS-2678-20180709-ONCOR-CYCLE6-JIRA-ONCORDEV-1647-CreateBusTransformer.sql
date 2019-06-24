set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2678, 'ONCORDEV-1647-CreateBusTransformer');
spool c:\temp\2678-20180709-ONCOR-CYCLE6-JIRA-ONCORDEV-1647-CreateBusTransformer.log
--**************************************************************************************
--SCRIPT NAME: 2678-20180709-ONCOR-CYCLE6-JIRA-ONCORDEV-1647-CreateBusTransformer.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 09-JUL-2018
-- CYCLE			: 6

-- JIRA NUMBER			: ONCORDEV-1647
-- PRODUCT VERSION		: 10.2.04
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Script to insert in custom command
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}
execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;

REM INSERTING into G3E_CUSTOMCOMMAND
SET DEFINE OFF;
Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE,G3E_AONO) values (g3e_customcommand_seq.nextval,'Bus Transformers','Command to Bus transformers together','Shubham Agarwal',null,0,0,'Bus Transformers',null,0,8912912,1,0,1,null,sysdate,'ccCreateBusTransformer:GTechnology.Oncor.CustomAPI.ccCreateBusTransformers',null);

COMMIT;
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2678);

