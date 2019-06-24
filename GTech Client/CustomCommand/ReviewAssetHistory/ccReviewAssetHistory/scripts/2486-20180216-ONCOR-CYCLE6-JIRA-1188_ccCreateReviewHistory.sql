
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2486, '1188_ccCreateReviewHistory');
spool c:\temp\2486-20180216-ONCOR-CYCLE6-JIRA-1188_ccCreateReviewHistory.log
--**************************************************************************************
--SCRIPT NAME: 2486-20180216-ONCOR-CYCLE6-JIRA-1188_ccCreateReviewHistory.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\SKAMARAJ
-- DATE		: 16-FEB-2018
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

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,
G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE,G3E_AONO) 
values (g3e_customcommand_seq.nextval,'Review Asset History','Review Asset History','ICC',null,0,0,'Review Asset History','Review Asset History',4,0,0,0,1,null,
SYSDATE,'ccReviewAssetHistory:GTechnology.Oncor.CustomAPI.ccReviewAssetHistory',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2486);