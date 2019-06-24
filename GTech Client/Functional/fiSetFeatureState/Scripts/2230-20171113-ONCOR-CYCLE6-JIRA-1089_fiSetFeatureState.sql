set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2230, '1089_fiSetFeatureState');
spool c:\temp\2230-20171113-ONCOR-CYCLE6-JIRA-1089_fiSetFeatureState.log
--**************************************************************************************
--SCRIPT NAME: 2230-20171113-ONCOR-CYCLE6-JIRA-1089_fiSetFeatureState.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\HKONDA
-- DATE			: 13-NOV-2017
-- CYCLE		: 6
-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_FUNCTIONALINTERFACE','G3E_FINO','G3E_FUNCTIONALINTERFACE_SEQ');end;
/

insert into g3e_functionalinterface
columns(g3e_fino,g3e_username,g3e_interface,g3e_argumentprompt,g3e_argumenttype,g3e_editdate,g3e_description)
values (g3e_functionalinterface_seq.nextval,'Set Feature State','fiSetFeatureState:GTechnology.Oncor.CustomAPI.fiSetFeatureState',null,null,sysdate,'Determines the appropriate feature state based on the active job type and status');
  
update g3e_attribute set
g3e_fino=(select g3e_fino from g3e_functionalinterface where g3e_username='Set Feature State'),
g3e_functionalordinal=1,
g3e_functionaltype='AddNew'
where g3e_field='G3E_ID' and g3e_cno=(select g3e_cno from g3e_component where g3e_username='Common Attributes');
   
update g3e_tabattribute set g3e_default = null where g3e_tano=3430300 and g3e_ano=101;

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2230);

