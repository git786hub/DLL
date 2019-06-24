
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2351, '1192_AttachJobDocument');
spool c:\temp\2351-20171212-ONCOR-CYCLE6-JIRA-1192_AttachJobDocument.log
--**************************************************************************************
--SCRIPT NAME: 2351-20171212-ONCOR-CYCLE6-JIRA-1192_AttachJobDocument.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\KAPPANA
-- DATE				: 12-DEC-2017
-- CYCLE			: 6
-- JIRA NUMBER		: 1192
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

begin create_sequence.create_metadata_sequence('G3E_CUSTOMCOMMAND','G3E_CCNO','G3E_CUSTOMCOMMAND_SEQ');end;
/

delete from g3e_customcommand where g3e_username='Attach Job Document';

insert into g3e_customcommand (g3e_ccno,g3e_username,g3e_description,g3e_author,g3e_comments,g3e_largebitmap,g3e_smallbitmap,g3e_tooltip,g3e_statusbartext,g3e_commandclass,g3e_enablingmask,g3e_modality,g3e_selectsetenablingmask,g3e_menuordinal,g3e_localecomment,g3e_editdate,g3e_interface)
values (g3e_customcommand_seq.nextval,'Attach Job Document','Attach Job Document to active jobs design area','HCCI',null,0,0,'Attach Job Document','Attach Job Document to active jobs design area',0,8388624,1,0,1,null,sysdate,'ccAttachJobDocument:GTechnology.Oncor.CustomAPI.ccAttachJobDocument');

CREATE TABLE FILETYPES(KEY VARCHAR2(100),VALUE VARCHAR2(100));
INSERT INTO FILETYPES VALUES('Text Files (*.txt)','Text Files (*.txt)|*.txt');
INSERT INTO FILETYPES VALUES('PDF files (*.pdf)','PDF files (*.pdf)|*.pdf');
INSERT INTO FILETYPES VALUES('Microsoft Word Files (*.doc; *.docx)','Microsoft Word Files (*.doc; *.docx)| *.doc ; *.docx');
INSERT INTO FILETYPES VALUES('Microsoft Excel Files(*.xls; *.xlsx)','Microsoft Excel Files(*.xls; *.xlsx)| *.xls ; *.xlsx');
INSERT INTO FILETYPES VALUES('All Files (*.*)','All Files (*.*)|*.*');

CREATE OR REPLACE PUBLIC SYNONYM FILETYPES FOR FILETYPES;
grant SELECT on FILETYPES to PUBLIC;
  
Insert into G3E_DATASUPPORT (G3E_DSNO,G3E_TABLE,G3E_COPYDATA,G3E_EDITDATE,G3E_GRANTPRIVILEGE,G3E_GRANTROLE,G3E_OBJECTTYPE) values (G3E_DATASUPPORT_SEQ.NEXTVAL,'FILETYPES',0,SYSDATE,'SELECT','SUPERVISOR,DESIGNER,MARKETING,FINANCE','TABLE');

execute LTT_Role.CREATE_ALL_ROLES;
execute LTT_Role.GRANT_PACKAGE_TO_ROLE;
execute LTT_Role.GRANT_PRIVS_TO_ROLE;
execute LTT_ROLE.GRANT_METADATA_TO_ROLE;
execute LTT_ROLE.CREATE_PUBLIC_SYNONYM;
execute LTT_ROLE.create_component_view_synonym;
execute LTT_ROLE.createsynpicklist;
execute LTT_ROLE.CREATE_METADATA_SYNONYM;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2351);


