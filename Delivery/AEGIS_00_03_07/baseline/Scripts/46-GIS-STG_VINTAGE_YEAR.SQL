
set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool %TEMP%\STG_VINTAGE_YEAR.log
--**************************************************************************************
--SCRIPT NAME: STG_VINTAGE_YEAR.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 26-FEB-2018
-- CYCLE			: 6

-- JIRA NUMBER		: ONCORDEV-1332
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create table GIS_STG.STG_VINTAGE_YEAR. This table is created i GIS_STG schema
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}
declare
    tmpCnt number;
begin
    -- check for the existence of object and then drop if they exist.    
    select count(*) into tmpCnt from all_tables where table_name = 'STG_VINTAGE_YEAR';
    if tmpCnt > 0 then
        execute immediate 'DROP TABLE "GIS_STG"."STG_VINTAGE_YEAR"';
    end if;
end;
/

--------------------------------------------------------
--  DDL for Table GIS_STG.STG_VINTAGE_YEAR
--------------------------------------------------------

  CREATE TABLE GIS_STG.STG_VINTAGE_YEAR 
   (	STG_VINTAGE_YEAR_ID NUMBER(15,0), 
	PROCESS_DATE DATE, 
	PROCESS_RUN_ID NUMBER(15,0), 
	WR_NO NUMBER(8,0), 
	VINTAGE_YEAR NUMBER(4,0), 
	STATUS VARCHAR2(8), 
	CAPARS_DATE DATE, 
	DIS_DATE DATE, 
	AUD_CREATE_USR_ID VARCHAR2(30), 
	AUD_MOD_USR_ID VARCHAR2(30), 
	AUD_CREATE_TS TIMESTAMP (6), 
	AUD_MOD_TS TIMESTAMP (6)
   );


   CREATE OR REPLACE PUBLIC SYNONYM STG_VINTAGE_YEAR FOR GIS_STG.STG_VINTAGE_YEAR;   
   GRANT SELECT ON STG_VINTAGE_YEAR TO ADMINISTRATOR;
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
