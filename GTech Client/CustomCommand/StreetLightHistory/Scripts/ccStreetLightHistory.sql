set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\ccStreetLight.sql.log
--**************************************************************************************
--SCRIPT NAME: ccStreetLight.sql
--**************************************************************************************
-- AUTHOR			: Tim Hooker 
-- DATE				: 
-- JIRA NUMBER		: 
-- PRODUCT VERSION	: 10.03.00
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: 
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Streetlight Location History';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_editdate,g3e_interface)
values(g3e_customcommand_seq.nextval,'Streetlight Location History','Streetlight location history.','Tim Hooker',0,0,'Streetlight Location History',0,16,0,0,1,sysdate,'ccStreetLightHistory:GTechnology.Oncor.CustomAPI.StreetLightHistory');
		
commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
