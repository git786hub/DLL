
set echo on
set linesize 1000
set pagesize 300
set define off

whenever sqlerror exit -1
whenever oserror exit -1

spool c:\temp\AddStreetLightVoltageDropCommand.log

--**************************************************************************************

--SCRIPT NAME: AddStreetLightVoltageDropCommand.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 21-AUG-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Street Light Voltage Drop custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values 
(G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Street Light Voltage Drop','Calculates the voltage drop for a Street Light network.','Hexagon',null,
0,0,'Street Light Voltage Drop',null,4,9437200,0,0,1,null,'ccStreetLightVoltageDrop:GTechnology.Oncor.CustomAPI.ccStreetLightVoltageDrop',null);

--**************************************************************************************
-- End Script Body
--**************************************************************************************