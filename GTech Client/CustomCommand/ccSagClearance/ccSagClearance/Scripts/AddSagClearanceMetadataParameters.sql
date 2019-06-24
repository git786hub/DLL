
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddSagClearanceMetadataParameters.log

--**************************************************************************************
--SCRIPT NAME: AddSagClearanceMetadataParameters.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET/RPGABRYS
-- Date 	      	    : 02-MAR-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata parameters for Sag Clearance custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

-- Remove implementation if exists so script can be reinstalled
DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'SagClearance';

-- Add implementation
Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SagClearance','Template','WorkbookPath','C:\Design Tools\Other\SagClearances2011 - not protected.xlsx','The full path to the tool workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SagClearance','Template','WorkbookName','SagClearances2011 - not protected.xlsx','The name of the tool workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SagClearance','Template','LocalTrustedLocation','C:\Design Tools\Temp\','Trusted location on the client machine where the master workbook will be copied and updated.');

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

