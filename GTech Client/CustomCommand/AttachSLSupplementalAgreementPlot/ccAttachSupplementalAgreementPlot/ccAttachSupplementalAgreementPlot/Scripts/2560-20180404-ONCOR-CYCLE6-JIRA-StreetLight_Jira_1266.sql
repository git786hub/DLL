set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2560, 'StreetLight_Jira_1266');
spool c:\temp\2560-20180404-ONCOR-CYCLE6-JIRA-StreetLight_Jira_1266.log
--**************************************************************************************
--SCRIPT NAME: 2560-20180404-ONCOR-CYCLE6-JIRA-StreetLight_Jira_1266.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\SKAMARAJ
-- DATE		: 04-APR-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

delete from G3E_CUSTOMCOMMAND where g3e_username in ('Attach Supplemental Agreement Plot');


execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;

INSERT INTO VL_HYPERLINK_TYPE (VL_KEY,VL_VALUE,UNUSED,ORDINAL) VALUES ('SUPPLEPLOT','Street Light Supplemental Plot',0,3);


Insert into g3e_customcommand (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE,G3E_AONO) values (g3e_customcommand_seq.nextval,'Attach Supplemental Agreement Plot','Attach Supplemental Agreement Plot','HCCI','Attach Supplemental Agreement Plot',0,0,'Attach Supplemental Agreement Plot','Attach Supplemental Agreement Plot',4,8388656,0,0,1,'Attach Supplemental Agreement Plot',SYSDATE,'ccAttachSupplementalAgreementPlot:GTechnology.Oncor.CustomAPI.ccAttachSupplementalAgreementPlot',null);


--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2560);


