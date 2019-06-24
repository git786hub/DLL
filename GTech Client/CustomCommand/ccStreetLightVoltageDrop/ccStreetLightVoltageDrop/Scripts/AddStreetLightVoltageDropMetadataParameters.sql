
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddStreetLightVoltageDropMetadataParameters.log

--**************************************************************************************
--SCRIPT NAME: AddStreetLightVoltageDropMetadataParameters.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET/RPGABRYS
-- Date 	      	    : 28-FEB-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata parameters for Street Light Voltage Drop custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

-- Remove implementation if exists so script can be reinstalled
DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'StreetLightVoltageDropCC';

-- Add implementation
Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('StreetLightVoltageDropCC','','TraceName','Street Light Calculation','The G3E_TRACE.G3E_USERNAME of the trace to run.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('StreetLightVoltageDropCC','','ReportFileName','C:\\ReportTemplates\\DesignToolReports.xlsx','The full path to the report workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('StreetLightVoltageDropCC','','ReportName','Street Light Voltage Drop','The name of the spreadsheet tab in the report workbook that contains the report.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('StreetLightVoltageDropCC','','NominalVoltages','120,240,277,480','Comma separated list of available nominal voltages.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('StreetLightVoltageDropCC','','NominalVoltageDefault','120','Default value to use for nominal voltage.');

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

