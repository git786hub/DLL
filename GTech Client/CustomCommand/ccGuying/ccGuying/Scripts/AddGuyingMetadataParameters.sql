
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddGuyingMetadataParameters.log

--**************************************************************************************
--SCRIPT NAME: AddGuyingMetadataParameters.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET/RPGABRYS
-- Date 	      	    : 02-MAR-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata parameters for Guying custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

-- Remove implementation if exists so script can be reinstalled
DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'GuyingCC';

-- Add implementation
Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('GuyingCC','Template','WorkbookPath','\\gabrys6420\testdesigntools\2010 Guying Program 120110.xls','The full path to the tool workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('GuyingCC','Template','WorkbookName','2010 Guying Program 120110.xls','The name of the tool workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('GuyingCC','Template','LocalTrustedLocation','C:\Design Tools\Temp\','Trusted location on the client machine where the master workbook will be copied and updated.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('GuyingCC','Validation','ValidationChecks','Interface!G24,Interface!H24,Interface!I24,Interface!G44,Interface!H44,Interface!I44,Interface!G25,Interface!H25,Interface!I25,Interface!G45,Interface!H45,Interface!I45,Sidewalk Guy!G13,Sidewalk Guy!G31,Sidewalk Guy!G14,Sidewalk Guy!G32,Sidewalk Guy!G16,Sidewalk Guy!G34,Sidewalk Guy!G17,Sidewalk Guy!G35,Sidewalk Guy!G18,Sidewalk Guy!G36','Cells to validate.  Comma separated list in the form of tab!cell.');

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

