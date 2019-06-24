set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddGISAuto_ServiceLineParams.log

--**************************************************************************************

--SCRIPT NAME: AddGISAuto_ServiceLineParams.sql
--**************************************************************************************
-- AUTHOR		: Joe Lundy 
-- Date 	      	: 14-MAR-2018
-- Jira NUMBER	  	:
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add general parameters for ServiceLinePlugin.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'GISAUTO_SERVICELINE';
INSERT INTO SYS_GENERALPARAMETER (SUBSYSTEM_NAME, PARAM_NAME, SUBSYSTEM_COMPONENT, PARAM_VALUE) values ('GISAUTO_SERVICELINE', 'GEOCODE_TOLERANCE_OH', 'TOLERANCE', 150);
INSERT INTO SYS_GENERALPARAMETER (SUBSYSTEM_NAME, PARAM_NAME, SUBSYSTEM_COMPONENT, PARAM_VALUE) values ('GISAUTO_SERVICELINE', 'GEOCODE_TOLERANCE_UG', 'TOLERANCE', 150);

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
