
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2397, '1153_ccCopyAncillaries');
spool c:\temp\2397-20180102-ONCOR-CYCLE6-JIRA-1153_ccCopyAncillaries.log
--**************************************************************************************
--SCRIPT NAME: 2397-20180102-ONCOR-CYCLE6-JIRA-1153_ccCopyAncillaries.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\HKONDA
-- DATE		: 02-JAN-2018
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

delete from g3e_customcommand where g3e_username='Copy Ancillaries';

insert into g3e_customcommand 
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface)
values (g3e_customcommand_seq.nextval,'Copy Ancillaries','Command will allow the user to identify one or more features that will inherit the Ancillary CUs from the source feature',user,'Command will allow the user to identify one or more features that will inherit the Ancillary CUs from the source feature',0,0,'Copy Ancillaries','Copy Ancillaries',4,9437200,0,0,1,'Copy Ancillaries',sysdate,'ccCopyAncillaries:GTechnology.Oncor.CustomAPI.ccCopyAncillaries');

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2397);

