
set echo on
set linesize 1000
set pagesize 300
set define off

whenever sqlerror exit -1
whenever oserror exit -1

spool c:\temp\AddGuyingCommand.log

--**************************************************************************************

--SCRIPT NAME: AddGuyingCommand.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 02-OCT-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Guying custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values 
(G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Guying','Calculates appropriate guy and anchor specifications to support a Pole in various installation scenarios.','Hexagon',null,
0,0,'Guying',null,4,8912912,0,0,1,null,'ccGuying:GTechnology.Oncor.CustomAPI.ccGuying',null);

--**************************************************************************************
-- End Script Body
--**************************************************************************************