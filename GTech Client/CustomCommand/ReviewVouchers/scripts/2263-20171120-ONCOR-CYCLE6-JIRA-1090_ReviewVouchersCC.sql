set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2263, '1090_ReviewVouchersCC');
spool c:\temp\2263-20171120-ONCOR-CYCLE6-JIRA-1090_ReviewVouchersCC.log
--**************************************************************************************
--SCRIPT NAME: 
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 20-NOV-2017
-- CYCLE		: 6
-- JIRA NUMBER	:1090
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************
execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE,G3E_AONO) values (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Review Vouchers','Provides the user a way to review all vouchers for all Work Points in the active WR.','Intergraph Corporation','Review Vouchers',10,10,'Review Vouchers','Provides Vouchers in the active WR.',0,16,0,0,(SELECT MAX(G3E_MENUORDINAL)+1 FROM G3E_CUSTOMCOMMAND),null,SYSDATE,'ccReviewVouchers:GTechnology.Oncor.CustomAPI.ccReviewVouchers',NULL);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2263);

