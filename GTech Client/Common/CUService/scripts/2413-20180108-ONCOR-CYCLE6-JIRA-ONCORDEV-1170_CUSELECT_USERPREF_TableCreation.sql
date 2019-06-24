
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2413, 'ONCORDEV-1170_CUSELECT_USERPREF_TableCreation');
spool c:\temp\2413-20180108-ONCOR-CYCLE6-JIRA-ONCORDEV-1170_CUSELECT_USERPREF_TableCreation.log
--**************************************************************************************
--SCRIPT NAME: 2413-20180108-ONCOR-CYCLE6-JIRA-ONCORDEV-1170_CUSELECT_USERPREF_TableCreation.sql
--**************************************************************************************
-- AUTHOR		: SAGARWAL
-- DATE			: 08-JAN-2018
-- CYCLE		: 6

-- JIRA NUMBER		: 1170
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create table CUSELECT_USERPREF with constraints
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

--------------------------------------------------------
--  DDL for Table CUSELECT_USERPREF
--------------------------------------------------------
alter table "CULIB_UNIT" add constraint CU_CODE_PK primary key("CU_CODE");

  CREATE TABLE "CUSELECT_USERPREF" 
   (	"PREF_UID" VARCHAR2(30 BYTE), 
	"CU_CODE" VARCHAR2(50 BYTE), 
	"CU_CATEGORY_CODE" VARCHAR2(15 BYTE)
   ) ;
--------------------------------------------------------
--  Ref Constraints for Table CUSELECT_USERPREF
--------------------------------------------------------

  ALTER TABLE "CUSELECT_USERPREF" ADD CONSTRAINT "CU_CATEGORY_CODE_FK" FOREIGN KEY ("CU_CATEGORY_CODE")
	  REFERENCES "CULIB_CATEGORY" ("CATEGORY_C") ENABLE;


--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2413);

