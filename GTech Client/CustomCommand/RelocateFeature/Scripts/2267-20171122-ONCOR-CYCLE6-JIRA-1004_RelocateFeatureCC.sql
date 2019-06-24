
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2267, '1004_RelocateFeatureCC');
spool c:\temp\2267-20171122-ONCOR-CYCLE6-JIRA-1004_RelocateFeatureCC.log
--**************************************************************************************
--SCRIPT NAME: 2267-20171122-ONCOR-CYCLE6-JIRA-1004_RelocateFeatureCC.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\SKAMARAJ
-- DATE		: 22-NOV-2017
-- CYCLE		: 6
-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Relocate Feature';

insert into g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values (g3e_customcommand_seq.nextval,'Relocate Feature','Relocate Feature','ICC',null,0,0,'Relocate Feature','Relocate Feature',4,9437200,0,0,1,'Relocate Feature',sysdate,'ccRelocateFeature:GTechnology.Oncor.CustomAPI.ccRelocateFeature',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2267);

