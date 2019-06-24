
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2262, '1036_CorrectionsModeCC');
spool c:\temp\2262-20171120-ONCOR-CYCLE6-JIRA-1036_CorrectionsModeCC.log
--**************************************************************************************
--SCRIPT NAME: 2262-20171120-ONCOR-CYCLE6-JIRA-1036_CorrectionsModeCC.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 20-NOV-2017
-- CYCLE		: 6

-- JIRA NUMBER	:1036
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Corrections Mode';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values (g3e_customcommand_seq.nextval,'Corrections Mode','Toggles the state of a session property to indicate whether the session is design mode or corrections mode','Intergraph Corporation','Corrections Mode',9,9,'Corrections Mode','Indicate whether the session is design mode or corrections mode',0,16,1,0,1,null,sysdate,'ccCorrectionsMode:GTechnology.Oncor.CustomAPI.ccCorrectionsMode',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2262);

