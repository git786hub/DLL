
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2326, '1109_ccReplaceFeature');
spool c:\temp\2326-20171205-ONCOR-CYCLE6-JIRA-1109_ccReplaceFeature.log
--**************************************************************************************
--SCRIPT NAME: 2326-20171205-ONCOR-CYCLE6-JIRA-1109_ccReplaceFeature.sql
--**************************************************************************************
-- AUTHOR		    : INGRNET\HKONDA
-- DATE			    : 05-DEC-2017
-- CYCLE		    : 6
-- JIRA NUMBER	    : 1109
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Replace Feature';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface) 
values (g3e_customcommand_seq.nextval,'Replace Feature','Provides the ability for a user to replace a feature, the existing feature is processed for removal, and a new copy of the feature is processed as an installation.',user,'Provides the ability for a user to replace a feature, the existing feature is processed for removal, and a new copy of the feature is processed as an installation.',0,0,'Replace Feature','Replace Feature',0,9437200,1,1,1,null,sysdate,'ccReplaceFeature:GTechnology.Oncor.CustomAPI.ccReplaceFeature');


commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2326);

