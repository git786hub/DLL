
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2414, 'ONCORDEV-1121_CUSelectionRelatedMetadata');
spool c:\temp\2414-20180108-ONCOR-CYCLE6-JIRA-ONCORDEV-1121_CUSelectionRelatedMetadata.log
--**************************************************************************************
--SCRIPT NAME: 2414-20180108-ONCOR-CYCLE6-JIRA-ONCORDEV-1121_CUSelectionRelatedMetadata.sql
--**************************************************************************************
-- AUTHOR		: SAGARWAL
-- DATE		: 08-JAN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}
   
CREATE OR REPLACE FORCE EDITIONABLE VIEW "V_CUSELECTION_ACU_VL" ("CU_TYPE", "CU", "CU_DESC", "CATEGORY_C") AS 
  select distinct decode(b.TYPE_C, 'A', 'ACU') cu_type, a.cu_code cu,a.CU_DESC,b.category_c
from CULIB_UNIT a,CULIB_CATEGORY b
where a.CATEGORY_C=b.CATEGORY_C  and b.TYPE_C = 'A' and ((EFFECTIVE_D <= SYSDATE or EFFECTIVE_D is null) AND (EXPIRATION_D >= SYSDATE or  EXPIRATION_D is null));

CREATE OR REPLACE FORCE EDITIONABLE VIEW "V_CUSELECTION_AMCU_VL" ("CU", "CU_DESC", "CU_TYPE", "CATEGORY_C", "CU_CODE", "EXPIRATION_D", "EFFECTIVE_D") AS 
  select c.mu_ID cu,a.CU_DESC,'AMCU' cu_type, b.CATEGORY_C, a.cu_code, A.EXPIRATION_D, A.EFFECTIVE_D
from CULIB_UNIT a,CULIB_CATEGORY b, CULIB_MACRO c, CULIB_MACROUNIT d
where a.CATEGORY_C=b.CATEGORY_C and b.category_c = c.category_c and c.MU_ID = d.MU_ID and d.CU_ID = a.cu_code 
 and  ((EFFECTIVE_D <= SYSDATE or EFFECTIVE_D is null) AND (EXPIRATION_D >= SYSDATE or  EXPIRATION_D is null)) and b.TYPE_C = 'A';
 
  CREATE OR REPLACE FORCE EDITIONABLE VIEW "V_CUSELECTION_CU_VL" ("CU", "CU_DESC", "CU_TYPE", "CATEGORY_C", "EXPIRATION_D", "EFFECTIVE_D") AS 
  select  a.cu_code cu,a.CU_DESC, decode(b.TYPE_C, 'C', 'CU', 'A', 'ACU', 'M', 'MCU') cu_type, b.CATEGORY_C, A.EXPIRATION_D, A.EFFECTIVE_D
from CULIB_UNIT a,CULIB_CATEGORY b
where a.CATEGORY_C=b.CATEGORY_C and  ((EFFECTIVE_D <= SYSDATE or EFFECTIVE_D is null) AND (EXPIRATION_D >= SYSDATE or  EXPIRATION_D is null));


  CREATE OR REPLACE FORCE EDITIONABLE VIEW "V_CUSELECTION_MCU_VL" ("CU", "CU_DESC", "G3E_FNO", "CU_TYPE", "CATEGORY_C", "CU_CODE", "EXPIRATION_D", "EFFECTIVE_D") AS 
  select c.mu_ID cu,a.CU_DESC,'1' g3E_fno, 'MCU' cu_type, b.CATEGORY_C, a.cu_code, A.EXPIRATION_D, A.EFFECTIVE_D
from CULIB_UNIT a,CULIB_CATEGORY b, CULIB_MACRO c, CULIB_MACROUNIT d
where a.CATEGORY_C=b.CATEGORY_C and b.category_c = c.category_c and c.MU_ID = d.MU_ID and d.CU_ID = a.cu_code and PRIMARY_YN = 'Y' 
 and  ((EFFECTIVE_D <= SYSDATE or EFFECTIVE_D is null) AND (EXPIRATION_D >= SYSDATE or  EXPIRATION_D is null));

REM INSERTING into IW_GENERALPARAMETER_ONCOR

--Insert into General Parameter Table

REM INSERTING into SYS_GENERALPARAMETER

REM INSERTING into SYS_GENERALPARAMETER
SET DEFINE OFF;
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values (1,'CU Category Mapping Table','CULIB_CATEGORY','Defines CU Category Mapping table','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Activity Field Name','ACTIVITY_C','G3E_FIELD for the Activity Attribute in Ancillary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Ancillary CU Attributes Component','22','G3E_CNO for Ancillary CU Attributes Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Primary CU Attributes Component','21','G3E_CNO for Primary CU Attributes Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Primary CU Qty Attribute','2107','G3E_ANO for the Quantity Attribute in Primary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Ancillary CU Qty Attribute','2206','G3E_ANO for the Quantity Attribute in Ancillary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Library Table','CULIB_UNIT','Defines the CU Library table name','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Macro Library Table','CULIB_MACROUNIT','Defines the Macro Library Table name','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Primary CU Attribute','2102','G3E_ANO for the CU Attribute in Primary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Ancillary CU Attribute','2203','G3E_ANO for the CU Attribute in Ancillary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Primary Macro CU Attribute','2103','G3E_ANO for the Macro CU Attribute in Primary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'Ancillary Macro CU Attribute','2204','G3E_ANO for the Macro CU Attribute in Ancillary CU Component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU UnitCID Field Name','UNIT_CID','Defines the Unit CID field name in the compatible unit table','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Description Field Name','CU_DESC','Defines the Description field name in the compatible unit table','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Prime Account # Field Name','PRIME_ACCT_ID','Defines the Primary Account on CU or Ancillay CU component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Property Unit Field Name','PROP_UNIT_ID','Defines the Primary Unit on CU or Ancillary component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Unit CNO Field Name','UNIT_CNO','Defines the Unit CNO field name on CU or Ancillary component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Length Flag Field Name','LENGTH_FLAG','Defines the Length flag field on CU or Ancillary component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Retirement Type Name','RETIREMENT_C','Defines the Retirement Type field on CU or Ancillary component','CUSelection','CUSelection');
Insert into SYS_GENERALPARAMETER (ID,PARAM_NAME,PARAM_VALUE,PARAM_DESC,SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT) values ((select max(ID)+1 from SYS_GENERALPARAMETER),'CU Quantity/Length Name','QTY_LENGTH_Q','Defines the Quantity/Length name field on CU or Ancillary component','CUSelection','CUSelection');

--Insertion in General Parameter Table ends

update G3E_RELATIONINTERFACE set g3e_username = 'CU Selection FKQ', g3e_interface = 'FkqCUSelection:GTechnology.Oncor.CustomAPI.GTFkqCUSelection' where g3e_username = 'IWFkqCUSelection';

DELETE FROM G3E_RELATIONARGUMENT WHERE G3E_RINO  = (select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ');

Insert into G3E_RELATIONARGUMENT (G3E_RANO,G3E_RINO,G3E_ARGUMENTORDINAL,G3E_ARGUMENTNAME,G3E_ARGUMENTDESCRIPTION,G3E_ARGUMENTTYPE,G3E_EDITDATE) values (G3E_RELATIONARGUMENT_SEQ.nextval,(select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ'),2,'Aggregate Feature','Either "Y" or "N" to indicate whether the affected feature is aggregate',10,SYSDATE);

Insert into G3E_RELATIONARGUMENT (G3E_RANO,G3E_RINO,G3E_ARGUMENTORDINAL,G3E_ARGUMENTNAME,G3E_ARGUMENTDESCRIPTION,G3E_ARGUMENTTYPE,G3E_EDITDATE) values (G3E_RELATIONARGUMENT_SEQ.nextval,(select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ'),3,'Category Filter','An optional comma-delimited string of one or more CU category codes',10,SYSDATE);

Insert into G3E_RELATIONARGUMENT (G3E_RANO,G3E_RINO,G3E_ARGUMENTORDINAL,G3E_ARGUMENTNAME,G3E_ARGUMENTDESCRIPTION,G3E_ARGUMENTTYPE,G3E_EDITDATE) values (G3E_RELATIONARGUMENT_SEQ.nextval,(select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ'),1,'Primary/Ancillary','Either "PRIMARY" or "ANCILLARY", which indicates which dialog should be displaye',10,SYSDATE);

REM INSERTING into G3E_RELATIONIFACEARGS

--The interface argument will eventaully need to be populated for each tab attribute instead of attribute, this is just to populate test data.

delete from G3E_RELATIONIFACEARGS where G3E_RIARGGROUPNO = (select G3E_FKQARGGROUPNO from g3e_Attribute where g3E_cno =22 and g3e_Ano =2203 and G3E_FKQRINO = ((select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ')));

delete from G3E_RELATIONIFACEARGS where G3E_RIARGGROUPNO = (select G3E_FKQARGGROUPNO from g3e_Attribute where g3E_cno =21 and g3e_Ano =2102 and G3E_FKQRINO = ((select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ')));

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO) +1 from G3E_RELATIONIFACEARGS),1,'PRIMARY',sysdate);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO)from G3E_RELATIONIFACEARGS),3,'PRISWITCH',sysdate);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO)from G3E_RELATIONIFACEARGS),2,'Y',sysdate);


update g3e_Attribute set G3E_FKQRINO = (select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ'), G3E_FKQARGGROUPNO = (select max(G3E_RIARGGROUPNO) from G3E_RELATIONIFACEARGS where g3e_value = 'PRIMARY') where g3e_cno = 21 and g3e_field = 'CU_C';


Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO) +1 from G3E_RELATIONIFACEARGS),1,'ANCILLARY',sysdate);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO)from G3E_RELATIONIFACEARGS),3,'""',sysdate);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO) from G3E_RELATIONIFACEARGS),2,'N',sysdate);

update g3e_Attribute set G3E_FKQRINO = (select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'CU Selection FKQ'), G3E_FKQARGGROUPNO = (select max(G3E_RIARGGROUPNO) from G3E_RELATIONIFACEARGS where g3e_value = 'ANCILLARY') where g3e_cno = 22 and g3e_field = 'CU_C';

  
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2414);

