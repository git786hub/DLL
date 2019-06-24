
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2228, '1088_AbandonFeature');
spool c:\temp\2228-20171113-ONCOR-CYCLE6-JIRA-1088_AbandonFeature.log
--**************************************************************************************
--SCRIPT NAME: 2228-20171113-ONCOR-CYCLE6-JIRA-1088_AbandonFeature.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\HKONDA
-- DATE		: 13-NOV-2017
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

delete from g3e_customcommand where g3e_username='Abandon Feature';

insert into g3e_customcommand (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE) 
values (g3e_customcommand_seq.nextval,'Abandon Feature','Allows features to be translated from a specific set of feature states to one of the available abandon states.',user,
'Allows features to be translated from a specific set of feature states to one of the available abandon states.',0,0,'Abandon Feature','Abandon Feature',0,8388624,1,1,1,null,sysdate,'ccAbandonFeature:GTechnology.Oncor.CustomAPI.ccAbandonFeature');

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2228);

