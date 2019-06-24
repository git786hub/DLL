
set echo on
set linesize 1000
set pagesize 300
set trimspool on
set define off

whenever sqlerror exit -1
whenever oserror exit -1

--exec adm_support.set_start(ADM_SCRIPTLOG_SEQ.NEXTVAL, 'Add_fiESILocationUpdate_Metadata');

spool c:\temp\Add_fiESILocationUpdate_Metadata.log

--**************************************************************************************

--SCRIPT NAME: Add_fiESILocationUpdate_Metadata.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 06-DEC-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for fiESILocationUpdate functional interface.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_FUNCTIONALINTERFACE (G3E_FINO,G3E_USERNAME,G3E_INTERFACE,G3E_ARGUMENTPROMPT,G3E_ARGUMENTTYPE,G3E_DESCRIPTION) values 
(G3E_FUNCTIONALINTERFACE_SEQ.NEXTVAL,'Update Premise Data','fiESILocationUpdate:GTechnology.Oncor.CustomAPI.fiESILocationUpdate',null,null,
'This functional interface validates the ESI Location entered by the user. The validation will check that the ESI Location does not exist on another Premise record in GIS and that the ESI Location exists in the CIS_ESI_LOCATIONS table.');

UPDATE G3E_ATTRIBUTE 
SET G3E_FINO = (SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME = 'Update Premise Data'),
G3E_FUNCTIONALORDINAL = 1, G3E_FUNCTIONALTYPE = 'SetValue', G3E_FUNCTIONALVALIDATION = 0
WHERE G3E_ANO = 550401;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
--exec adm_support.set_finish(?);
