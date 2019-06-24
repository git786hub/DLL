
set echo on
set linesize 1000
set pagesize 300
set trimspool on
set define off

whenever sqlerror exit -1
whenever oserror exit -1

--exec adm_support.set_start(ADM_SCRIPTLOG_SEQ.NEXTVAL, 'AddUpdateTraceCommand');

spool c:\temp\AddUpdateTraceCommand.log

--**************************************************************************************

--SCRIPT NAME: AddUpdateTraceCommand.sql
--**************************************************************************************
-- AUTHOR		        : 
-- Date 	      	    : 29-JAN-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03.0000.30005
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Update Trace custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Update Trace','The command updates feature connectivity attributes using the results of the Trace Primary/Secondary and Trace Primary/Secondary Proposed traces.','Hexagon',null,0,0,'Update Trace',null,4,9437200,0,0,1,null,'ccUpdateTrace:GTechnology.Oncor.CustomAPI.ccUpdateTrace',null);

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
--exec adm_support.set_finish(?);
