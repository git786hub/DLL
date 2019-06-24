
set echo on
set linesize 1000
set pagesize 300
set define off

whenever sqlerror exit -1
whenever oserror exit -1

spool c:\temp\AddStreetLightVoltageDropCommandSupportTables.log

--**************************************************************************************

--SCRIPT NAME: AddStreetLightVoltageDropCommandSupportTables.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 21-AUG-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add tables to support the Street Light Voltage Drop custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

CREATE TABLE TOOLS_SL_BALLAST (
BINO				NUMBER GENERATED ALWAYS AS IDENTITY,
CU					VARCHAR2 (30),
DESCRIPTION			VARCHAR2 (80),
VOLTAGE				NUMBER,
REGULATION			NUMBER,
POWER_FACTOR		NUMBER,
MAX_AMPS			NUMBER,
EDIT_DATE_DT		DATE
);
/

ALTER TABLE TOOLS_SL_BALLAST ADD CONSTRAINT M_P_TOOLS_SL_BALLAST PRIMARY KEY (BINO);

CREATE OR REPLACE PUBLIC SYNONYM TOOLS_SL_BALLAST FOR TOOLS_SL_BALLAST;

CREATE TRIGGER T_BIUR_TOOLS_SL_BALLAST_EDATE
BEFORE INSERT OR UPDATE ON TOOLS_SL_BALLAST
FOR EACH ROW
BEGIN

  :NEW.EDIT_DATE_DT := SYSDATE;
END;
/

CREATE TABLE TOOLS_SL_NOMINAL_VOLTAGE(
VOLT_Q			VARCHAR2 (1),
DEFAULT_YN      VARCHAR2 (1),
EDIT_DATE_DT	DATE
);
/

ALTER TABLE TOOLS_SL_NOMINAL_VOLTAGE ADD CONSTRAINT M_P_TOOLS_SL_NOMINAL_VOLTAGE PRIMARY KEY (BINO);

CREATE TRIGGER T_BIUR_TOOLS_SL_NOMINAL_VOLTAGE_EDATE
BEFORE INSERT OR UPDATE ON TOOLS_SL_NOMINAL_VOLTAGE
FOR EACH ROW
BEGIN

	:NEW.EDIT_DATE_DT := SYSDATE;
END;
/


--**************************************************************************************
-- End Script Body
--**************************************************************************************