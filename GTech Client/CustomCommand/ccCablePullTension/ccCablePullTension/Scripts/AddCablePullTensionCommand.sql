
set echo on
set linesize 1000
set pagesize 300
set define off

whenever sqlerror exit -1
whenever oserror exit -1

spool c:\temp\AddCablePullTensionCommand.log

--**************************************************************************************

--SCRIPT NAME: AddCablePullTensionCommand.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 14-JUL-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for Cable Pull Tension custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_CUSTOMCOMMAND (G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) values 
(G3E_CUSTOMCOMMAND_SEQ.NEXTVAL,'Cable Pull Tension','Calculates the tension and side wall bearing pressure for a selected cable and conduit.','Hexagon',null,0,0,'Cable Pull Tension',null,4,8454144,0,0,1,null,'ccCablePullTension:GTechnology.Oncor.CustomAPI.ccCablePullTension',null);

--**************************************************************************************
-- End Script Body
--**************************************************************************************