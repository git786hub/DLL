set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\ccSessionManagement.sql.log
--**************************************************************************************
--SCRIPT NAME: ccSessionManagement.sql
--**************************************************************************************
-- AUTHOR			: Daniel Scott / Barry Scott
-- DATE				: 
-- JIRA NUMBER		: 
-- PRODUCT VERSION	: 10.03.00
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: 
--**************************************************************************************
-- Script Body
--**************************************************************************************

execute create_sequence.create_metadata_sequences;

update g3e_generalparameter set g3e_value=0 where g3e_name='CustomCommandAutoStartCCNO';

delete from g3e_customcommand where g3e_username='SessionManagementAutoStart';

insert into gis.g3e_customcommand
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_editdate,g3e_interface)
values(g3e_customcommand_seq.nextval,'SessionManagementAutoStart','Auto Start Command','Daniel Scott',0,0,'SessionManagementAutoStart',0,0,1,0,1,sysdate,'ccSessionManagement:GTechnology.Oncor.CustomAPI.ccSessionManagement');

update g3e_generalparameter set g3e_value=(select g3e_ccno from g3e_customcommand where g3e_username='SessionManagementAutoStart') where g3e_name='CustomCommandAutoStartCCNO';
 
commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
