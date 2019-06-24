set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2260, '995_RemoveFeatureCC');
spool c:\temp\2260-20171120-ONCOR-CYCLE6-JIRA-995_RemoveFeatureCC.log
--**************************************************************************************
--SCRIPT NAME: 2260-20171120-ONCOR-CYCLE6-JIRA-995_RemoveFeatureCC.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 20-NOV-2017
-- CYCLE		: 6
-- JIRA NUMBER	:995
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Remove Feature';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values (g3e_customcommand_seq.nextval,'Remove Feature','Provides the ability for a user to transition features from a specific set of feature states to one of the available remove states.','Intergraph Corporation','Remove Feature',8,8,'Remove Feature','Changes feature state to remove states.',4,8912912,0,0,1,null,sysdate,'ccRemoveFeature:GTechnology.Oncor.CustomAPI.ccRemoveFeature',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2260);

