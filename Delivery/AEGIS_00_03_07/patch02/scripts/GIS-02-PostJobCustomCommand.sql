set echo on
set linesize 1000
set pagesize 300
set trimspool on
set define off

spool c:\temp\PostJobCustomCommand.sql.log

--**************************************************************************************
--SCRIPT NAME: PostJobCustomCommand.sql
--**************************************************************************************
-- AUTHOR		          : Barry Scott
-- Date 	      	    : 25-Apr-2018
-- Jira NUMBER	  	  : ONCORDEV-1168
-- PRODUCT VERSION  	: 10.3.0
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Post Job custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Post Job';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values(
	g3e_customcommand_seq.nextval, -- g3e_ccno(not null)
	'Post Job', -- g3e_username(not null)
	'Posts the edits for the active job.', -- g3e_description
	'Barry Scott', -- g3e_author
	null, -- g3e_comments
	null, -- g3e_largebitmap
	null, -- g3e_smallbitmap
	null, -- g3e_tooltip
	null, -- g3e_statusbartext
	0, -- g3e_commandclass(not null)
	8388624, -- g3e_enablingmask(not null)
	1, -- g3e_modality(not null)
	null, -- g3e_selectsetenablingmask(not null)
	1, -- g3e_menuordinal(not null)
	null, -- g3e_localecomment
	sysdate, -- g3e_editdate(not null)
	'ccPostJob:GTechnology.Oncor.CustomAPI.PostJob', -- g3e_interface(not null)
	null -- g3e_aono
);

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************

spool off;