
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2369, '1193_CCAttachSiteDrawing');
spool c:\temp\2369-20171215-ONCOR-CYCLE6-JIRA-1193_CCAttachSiteDrawing.log
--**************************************************************************************
--SCRIPT NAME: 2369-20171215-ONCOR-CYCLE6-JIRA-1193_CCAttachSiteDrawing.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\KAPPANA
-- DATE				: 15-DEC-2017
-- CYCLE			: 6
-- JIRA NUMBER		: 1193
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Configure custom command
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Attach Site Drawing';

Insert into G3E_CUSTOMCOMMAND
columns(G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_EDITDATE,G3E_INTERFACE,G3E_AONO)
values (g3e_customcommand_seq.nextval,'Attach Site Drawing','To generate a PDF from an open plot window and attach it to a selected Permit or Easement feature','HCCI',null,0,0,'Attach Site Drawing','Attach Site Drawing to Pemit or Easement',0,9437232,1,0,1,null,sysdate,'ccAttachSiteDrawing:GTechnology.Oncor.CustomAPI.ccAttachSiteDrawing',null);

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2369);


