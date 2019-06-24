
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2497, '1185_RequestTreeTrimmingEstimateCC');
spool c:\temp\2497-20180221-ONCOR-CYCLE6-JIRA-1185_RequestTreeTrimmingEstimateCC.log
--**************************************************************************************
--SCRIPT NAME: 2497-20180221-ONCOR-CYCLE6-JIRA-1185_RequestTreeTrimmingEstimateCC.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 21-FEB-2018
-- CYCLE		: 6
-- JIRA NUMBER	:1185
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Request Tree Trimming Estimate';

insert into g3e_customcommand 
columns(g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface,g3e_aono)
values (g3e_customcommand_seq.nextval,'Request Tree Trimming Estimate','Custom placement for the Tree Trimming Voucher feature class,includes automated form and plot generation for submission of a request for estimate to Vegetation Management','Intergraph Corporation','Request Tree Trimming Estimate',8,8,'Request Tree Trimming Estimate','Request Tree Trimming Estimate',1,8388624,0,0,1,null,sysdate,'ccTreeTrimRequestEstimate:GTechnology.Oncor.CustomAPI.ccTreeTrimRequestEstimate',null);

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2497);

