
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2421, 'ONCORDEV-1172_SetDefaultActivity');
spool c:\temp\2421-20180111-ONCOR-CYCLE6-JIRA-ONCORDEV-1172_SetDefaultActivity.log
--**************************************************************************************
--SCRIPT NAME: 2421-20180111-ONCOR-CYCLE6-JIRA-ONCORDEV-1172_SetDefaultActivity.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 11-JAN-2018
-- CYCLE			: 6
-- JIRA NUMBER		: ONCORDEV-1172
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create metadata for the FI Set Default CU Activity
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

begin create_sequence.create_metadata_sequence('G3E_FUNCTIONALINTERFACE','G3E_FINO','G3E_FUNCTIONALINTERFACE_SEQ');end;
/

insert into g3e_functionalinterface
columns(g3e_fino,g3e_username,g3e_interface,g3e_argumentprompt,g3e_argumenttype,g3e_editdate,g3e_description)
values (g3e_functionalinterface_seq.nextval,'Set Default CU Activity','fiSetDefaultCUActivity:GTechnology.Oncor.CustomAPI.fiSetDefaultCUActivity',null,null,sysdate,null);

update g3e_attribute set
g3e_fino=(select g3e_fino from g3e_functionalinterface where g3e_username='Set Default CU Activity'),
g3e_functionalordinal=(select max(g3e_functionalordinal)+1 from g3e_attribute where g3e_cno=21),
g3e_functionaltype='AddNew',
g3e_interfaceargument=null
where g3e_cno=21 and g3e_field='ACTIVITY_C';

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2421);

