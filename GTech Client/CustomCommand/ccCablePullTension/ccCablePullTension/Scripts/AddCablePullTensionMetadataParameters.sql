
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddCablePullTensionMetadataParameters.log

--**************************************************************************************
--SCRIPT NAME: AddCablePullTensionMetadataParameters.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET/RPGABRYS
-- Date 	      	    : 02-MAR-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata parameters for Cable Pull Tension custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

-- Remove implementation if exists so script can be reinstalled
DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'CablePullTensionCC';

-- Add implementation
Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('CablePullTensionCC','Report','ReportFileName','C:\DesignTools\Other\DesignToolReports.xlsx','The full path to the report workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('CablePullTensionCC','Report','ReportName','Cable Pull Tension Report','The name of the spreadsheet tab in the report workbook that contains the report.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('CablePullTensionCC','JamRatio','JamRatioLowerBound','2.9','The lower bound value used in the Jam Ratio calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('CablePullTensionCC','JamRatio','JamRatioUpperBound','3.1','The upper bound value used in the Jam Ratio calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('CablePullTensionCC','COF','LowCOF','0.4','The low coefficient of friction value.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('CablePullTensionCC','COF','HighCOF','0.25','The high coefficient of friction value.');

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

