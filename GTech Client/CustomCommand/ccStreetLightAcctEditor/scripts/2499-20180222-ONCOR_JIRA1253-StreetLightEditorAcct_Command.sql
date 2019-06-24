set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2499, 'ONCOR_JIRA1253-StreetLightEditorAcct_Command');
spool c:\temp\2499-20180222-ONCOR_JIRA1253-StreetLightEditorAcct_Command.log
--**************************************************************************************
--SCRIPT NAME: 2499-20180222-ONCOR_JIRA1253-StreetLightEditorAcct_Command.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 22-FEB-2018
-- CYCLE			: 7
-- JIRA NUMBER		: 1253
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Add commnad "Street Light Account Editor" 
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

execute create_sequence.create_metadata_sequences;

delete from g3e_customcommand where g3e_username='Street Light Account Editor';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values(g3e_customcommand_seq.nextval,'Street Light Account Editor','Street Light Account Editor',null,null,3,3,'Street Light Account Editor',null,1,8388624,0,0,1,null,SYSDATE,'ccStreetLightAcctEditor:GTechnology.Oncor.CustomAPI.ccStreetLightAcctEditor',null);

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2499);

