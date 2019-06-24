set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddServiceLineInstallCommand.log

--**************************************************************************************

--SCRIPT NAME: AddServiceLineInstallCommand.sql
--**************************************************************************************
-- AUTHOR		: Joe Lundy 
-- Date 	      	: 12-MAR-2018
-- Jira NUMBER	  	:
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Install Service Line custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

DELETE FROM G3E_CUSTOMCOMMAND WHERE G3E_USERNAME = 'Install Service Line';
Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Install Service Line','Installs a new service line.','Joe Lundy',null,0,0,'Install Service Line',null,4,8454160,0,0,1,null,'ccServiceLineInstall:GTechnology.Oncor.CustomAPI.ccServiceLineInstall',null);

DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'SERVICE_LINE_INSTALL';
INSERT INTO SYS_GENERALPARAMETER (SUBSYSTEM_NAME, PARAM_NAME, SUBSYSTEM_COMPONENT, PARAM_VALUE) values ('SERVICE_LINE_INSTALL', 'CU', 'CU_DEFAULT', 'SU10AQ(IC)');

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

