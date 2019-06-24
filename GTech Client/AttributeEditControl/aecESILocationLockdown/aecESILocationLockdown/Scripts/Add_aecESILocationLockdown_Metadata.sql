
set echo on
set linesize 1000
set pagesize 300
set trimspool on
set define off

whenever sqlerror exit -1
whenever oserror exit -1

--exec adm_support.set_start(ADM_SCRIPTLOG_SEQ.NEXTVAL, 'Add_aecESILocationLockdown_Metadata');

spool c:\temp\Add_aecESILocationLockdown_Metadata.log

--**************************************************************************************

--SCRIPT NAME: Add_aecESILocationLockdown_Metadata.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 12-DEC-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata for aecESILocationLockdown attribute edit control.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

Insert into G3E_RELATIONINTERFACE (G3E_RINO,G3E_USERNAME,G3E_INTERFACE,G3E_TYPE,G3E_DESCRIPTION) values 
(G3E_RELATIONINTERFACE_SEQ.NEXTVAL,'Allow edit if not posted','aecEditIfNotPosted:GTechnology.Oncor.CustomAPI.aecEditIfNotPosted','Attribute Edit Control',
'Prevent attribute from being populated once the attribute value has been set and posted.');

Insert into G3E_RELATIONARGUMENT (G3E_RANO,G3E_RINO,G3E_ARGUMENTORDINAL,G3E_ARGUMENTNAME,G3E_ARGUMENTDESCRIPTION,G3E_ARGUMENTTYPE) values 
(G3E_RELATIONARGUMENT_SEQ.NEXTVAL,
(SELECT G3E_RINO FROM G3E_RELATIONINTERFACE WHERE G3E_USERNAME = 'Allow edit if not posted'),
1,'Attribute Field Name','Name of the field to check to see if it is populated and posted.',10);

Insert into G3E_RELATIONARGUMENT (G3E_RANO,G3E_RINO,G3E_ARGUMENTORDINAL,G3E_ARGUMENTNAME,G3E_ARGUMENTDESCRIPTION,G3E_ARGUMENTTYPE) values 
(G3E_RELATIONARGUMENT_SEQ.NEXTVAL,
(SELECT G3E_RINO FROM G3E_RELATIONINTERFACE WHERE G3E_USERNAME = 'Allow edit if not posted'),
2,'Component Table Name','Name of the table to check.',10);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE) values 
(G3E_RELATIONIFACEARGS_SEQ.NEXTVAL,5504,1,'PREMISE_NBR');

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE) values 
(G3E_RELATIONIFACEARGS_SEQ.NEXTVAL,5504,2,'B$PREMISE_N');

UPDATE G3E_ATTRIBUTE SET G3E_RINO = (SELECT G3E_RINO FROM G3E_RELATIONINTERFACE WHERE G3E_USERNAME = 'Allow edit if not posted'),
G3E_RIARGGROUPNO = 5504
WHERE G3E_ANO IN (550401,550406,550407,550408,550409,550410,550411,550412,550413,550414,550415,550416);

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
--exec adm_support.set_finish(?);
