
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2423, '1155_ccManagePreferredCUs');
spool c:\temp\2423-20180111-ONCOR-CYCLE6-JIRA-1155_ccManagePreferredCUs.log
--**************************************************************************************
--SCRIPT NAME: 2423-20180111-ONCOR-CYCLE6-JIRA-1155_ccManagePreferredCUs.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\HKONDA
-- DATE		: 11-JAN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************
execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;

insert into g3e_customcommand (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE) 
values (g3e_customcommand_seq.nextval,'Manage Preferred CUs','Allows user to manage preferred CUs',user,
'Allows user to manage preferred CUs',0,0,'Manage Preferred CUs','Manage Preferred CUs',0,0,1,0,1,null,sysdate,'ccManagePreferredCUs:GTechnology.Oncor.CustomAPI.ccManagePreferredCUs');

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2423);

