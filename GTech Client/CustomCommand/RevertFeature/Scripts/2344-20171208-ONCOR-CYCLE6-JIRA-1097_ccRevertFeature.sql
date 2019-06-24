
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2344, '1097_ccRevertFeature');
spool c:\temp\2344-20171208-ONCOR-CYCLE6-JIRA-1097_ccRevertFeature.log
--**************************************************************************************
--SCRIPT NAME: 2344-20171208-ONCOR-CYCLE6-JIRA-1097_ccRevertFeature.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\SKAMARAJ
-- DATE		: 08-DEC-2017
-- CYCLE		: 6
-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Revert Feature';

Insert into g3e_customcommand
columns(G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE,G3E_AONO)
values (g3e_customcommand_seq.nextval,'Revert Feature','Revert Feature','ICC','Revert Feature',0,0,'Revert Feature','Revert Feature',0,9437200,1,0,1,'Revert Feature',sysdate,'ccRevertFeature:GTechnology.Oncor.CustomAPI.ccRevertFeature',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2344);

