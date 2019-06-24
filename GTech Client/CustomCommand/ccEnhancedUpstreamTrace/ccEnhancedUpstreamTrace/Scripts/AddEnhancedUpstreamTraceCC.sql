
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddEnhancedUpstreamTraceCC.log

--**************************************************************************************

--SCRIPT NAME: AddEnhancedUpstreamTraceCC.sql
--**************************************************************************************
-- AUTHOR		        : 
-- Date 	      	    : 27-JUN-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03.0000.30005
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Enhanced Upstream Trace custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Enhanced Upstream Trace','The command will trace the network and identify all possible sources for the selected feature.','Hexagon',null,0,0,'Enhanced Upstream Trace',null,1,1048576,0,0,1,null,'ccEnhancedUpstreamTrace:GTechnology.Oncor.CustomAPI.ccEnhancedUpstreamTrace',null);

INSERT INTO G3E_TRACEFEATURE ( G3E_TFNO, G3E_TNO, G3E_FNO ) VALUES ( 
G3E_TRACEFEATURE_SEQ.NEXTVAL, 
(Select G3e_Id From G3e_Trace Where G3e_Username = 'Trace Feeder Actual'), 59);

INSERT INTO G3E_TRACEFEATURE ( G3E_TFNO, G3E_TNO, G3E_FNO ) VALUES ( 
G3E_TRACEFEATURE_SEQ.NEXTVAL, 
(Select G3e_Id From G3e_Trace Where G3e_Username = 'Trace Feeder Actual'), 60);

INSERT INTO G3E_TRACEFEATURE ( G3E_TFNO, G3E_TNO, G3E_FNO ) VALUES ( 
G3E_TRACEFEATURE_SEQ.NEXTVAL, 
(Select G3e_Id From G3e_Trace Where G3e_Username = 'Trace Feeder Actual'), 98);

INSERT INTO G3E_TRACEFEATURE ( G3E_TFNO, G3E_TNO, G3E_FNO ) VALUES ( 
G3E_TRACEFEATURE_SEQ.NEXTVAL, 
(Select G3e_Id From G3e_Trace Where G3e_Username = 'Trace Feeder Actual'), 99);

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
