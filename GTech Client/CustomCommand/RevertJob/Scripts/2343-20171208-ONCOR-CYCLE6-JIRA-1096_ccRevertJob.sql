
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2343, '1096_ccRevertJob');
spool c:\temp\2343-20171208-ONCOR-CYCLE6-JIRA-1096_ccRevertJob.log
--**************************************************************************************
--SCRIPT NAME: 2343-20171208-ONCOR-CYCLE6-JIRA-1096_ccRevertJob.sql
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

delete from g3e_customcommand where g3e_username='Revert Job';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values (g3e_customcommand_seq.nextval,'Revert Job','Revert Job','ICC','Revert Job',0,0,'Revert Job','Revert Job',0,8388624,1,0,1,'Revert Job',sysdate,'ccRevertJob:GTechnology.Oncor.CustomAPI.ccRevertJob',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2343);

