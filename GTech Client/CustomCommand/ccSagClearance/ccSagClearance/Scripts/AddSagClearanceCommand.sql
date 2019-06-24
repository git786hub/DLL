
set echo on
set linesize 1000
set pagesize 300
set define off

whenever sqlerror exit -1
whenever oserror exit -1

spool c:\temp\AddSagClearanceCommand.log

--**************************************************************************************

--SCRIPT NAME: AddSagClearanceCommand.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 22-SEP-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Sag Clearance custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values 
(G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Sag Clearance','Calculates the minimum clearance of a Conductor.','Hexagon',null,
0,0,'Sag Clearance',null,4,9437200,0,0,1,null,'ccSagClearance:GTechnology.Oncor.CustomAPI.ccSagClearance',null);

--**************************************************************************************
-- End Script Body
--**************************************************************************************