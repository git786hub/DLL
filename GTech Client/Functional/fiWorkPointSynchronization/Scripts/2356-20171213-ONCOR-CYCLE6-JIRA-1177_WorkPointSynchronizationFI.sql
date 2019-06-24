set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2356, '1177_WorkPointSynchronizationFI');
spool c:\temp\2356-20171213-ONCOR-CYCLE6-JIRA-1177_WorkPointSynchronizationFI.log
--**************************************************************************************
--SCRIPT NAME: 2356-20171213-ONCOR-CYCLE6-JIRA-1177_WorkPointSynchronizationFI.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 13-DEC-2017
-- CYCLE		: 6
-- JIRA NUMBER	:1177
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_FUNCTIONALINTERFACE','G3E_FINO','G3E_FUNCTIONALINTERFACE_SEQ');end;
/

update g3e_attribute set
g3e_fino=null,g3e_functionalordinal=null,g3e_functionaltype=null
where g3e_field='STRUCTURE_ID' and g3e_username='Structure ID' and g3e_cno=(select g3e_cno from g3e_component where g3e_username='Work Point Attributes');

delete from g3e_functionalinterface
where g3e_username='Work Point Synchronization';

insert into g3e_functionalinterface
columns(g3e_fino,g3e_username,g3e_interface,g3e_argumentprompt,g3e_argumenttype,g3e_editdate,g3e_description)
values (g3e_functionalinterface_seq.nextval,'Work Point Synchronization','fiWorkPointSynchronization:GTechnology.Oncor.CustomAPI.fiWorkPointSynchronization',null,null,sysdate,'Synchronizes all CU and Ancillary CU activity from the associated structure and all features owned by that structure.');

--Work Point Attributes FI Update
update g3e_attribute set
g3e_fino=(select g3e_fino from g3e_functionalinterface where g3e_username='Work Point Synchronization'),
g3e_functionalordinal=1,
g3e_functionaltype='SetValue'
where g3e_field='STRUCTURE_ID' and g3e_username='Structure ID' and g3e_cno=(select g3e_cno from g3e_component where g3e_username='Work Point Attributes');

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2356);

