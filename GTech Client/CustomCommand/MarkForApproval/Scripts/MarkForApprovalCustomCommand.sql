set echo on
set linesize 1000
set pagesize 300
set trimspool on
set define off

spool c:\temp\MarkForApprovalCustomCommand.sql.log

--**************************************************************************************
--SCRIPT NAME: MarkForClosureCustomCommand.sql.sql
--**************************************************************************************
-- AUTHOR		          : Barry Scott
-- Date 	      	    : 22-Mar-2018
-- Jira NUMBER	  	  : ONCORDEV-1164
-- PRODUCT VERSION  	: 10.3.0
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Mark for Approval custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Mark for Approval';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_interface,g3e_aono,g3e_editdate)
values(g3e_customcommand_seq.nextval,'Mark for Approval','Command to mark a WR for approval.','Barry Scott',null,null,null,null,null,0,8388624,1,null,1,null,'ccMarkForApproval:GTechnology.Oncor.CustomAPI.ccMarkForApproval',null,sysdate);

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************

spool off;