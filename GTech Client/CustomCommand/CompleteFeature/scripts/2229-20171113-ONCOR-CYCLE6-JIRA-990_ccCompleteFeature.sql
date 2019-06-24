
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2229, '990_ccCompleteFeature');
spool c:\temp\2229-20171113-ONCOR-CYCLE6-JIRA-990_ccCompleteFeature.log
--**************************************************************************************
--SCRIPT NAME: 2229-20171113-ONCOR-CYCLE6-JIRA-990_ccCompleteFeature.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\HKONDA
-- DATE		: 13-NOV-2017
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
values (g3e_customcommand_seq.nextval,'Complete Feature','Allows features to be translated from proposed or as-built states to the corresponding completed state',user,
'Allows features to be translated from proposed or as-built states to the corresponding completed state',0,0,'Complete Feature','Complete Feature',0,8388624,1,1,1,null,sysdate,'ccCompleteFeature:GTechnology.Oncor.CustomAPI.ccCompleteFeature');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2229);

