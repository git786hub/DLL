
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddFieldActivityCommand.log

--**************************************************************************************

--SCRIPT NAME: AddFieldActivityCommand.sql
--**************************************************************************************
-- AUTHOR		: Joe Lundy 
-- Date 	      	: 12-MAR-2018
-- Jira NUMBER	  	:
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Request ESI Location custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

DELETE FROM G3E_CUSTOMCOMMAND WHERE G3E_USERNAME = 'Field Activity';
Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Field Activity','Sets activity codes for selected features.','Joe Lundy',null,0,0,'Field Activity',null,4,8454160,0,0,1,null,'ccFieldActivity:GTechnology.Oncor.CustomAPI.ccFieldActivity',null);


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

