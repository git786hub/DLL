set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\ValueListDatatypeChanges.log
--**************************************************************************************
-- SCRIPT NAME: ValueListDatatypeChanges.sql
--**************************************************************************************
-- AUTHOR          : INGRNET\RRADASE
-- DATE            : 24-JUL-2018
-- PRODUCT VERSION  : 10.3.0
-- PRJ IDENTIFIER  : G/TECHNOLOGY - ONCOR
-- PROGRAM DESC    : Changes to value list tables to align datatypes with values imported from WMIS
--**************************************************************************************


-- VL_FUSE_INTERRUPT_RATING: change value from NUMBER(8,2) to VARCHAR2(10)

  DROP TABLE "GIS"."VL_FUSE_INTERRUPT_RATING";
  CREATE TABLE "GIS"."VL_FUSE_INTERRUPT_RATING" (
    "VL_KEY"    VARCHAR2(10 BYTE), 
    "VL_VALUE"  VARCHAR2(10 BYTE), 
    "UNUSED"    NUMBER(1,0) DEFAULT 0, 
    "ORDINAL"   NUMBER(10,0), 
    CONSTRAINT "P_P_VL_FUSE_INTERRUPT_RAT_01" PRIMARY KEY ("VL_KEY")
   );

-- VL_PAD_SIZE: change key from VARCHAR2(2) to VARCHAR2(15)

  DROP TABLE "GIS"."VL_PAD_SIZE";
  CREATE TABLE "GIS"."VL_PAD_SIZE" (
    "VL_KEY"    VARCHAR2(10 BYTE), 
    "VL_VALUE"  VARCHAR2(30 BYTE), 
    "UNUSED"    NUMBER(1,0) DEFAULT 0, 
    "ORDINAL"   NUMBER(10,0), 
    CONSTRAINT "P_P_VL_PAD_S_01" PRIMARY KEY ("VL_KEY")
   );

-- VL_PULLBOX_MATERIAL: change key from VARCHAR2(2) to VARCHAR2(10)

  DROP TABLE "GIS"."VL_PULLBOX_MATERIAL";
  CREATE TABLE "GIS"."VL_PULLBOX_MATERIAL" (
    "VL_KEY"    VARCHAR2(10 BYTE), 
    "VL_VALUE"  VARCHAR2(30 BYTE), 
    "UNUSED"    NUMBER(1,0) DEFAULT 0, 
    "ORDINAL"   NUMBER(10,0), 
    CONSTRAINT "P_P_VL_PULLBOX_MATER_01" PRIMARY KEY ("VL_KEY")
   );

-- VL_RACK_MATERIAL: change key from VARCHAR2(3) to VARCHAR2(10)

  DROP TABLE "GIS"."VL_RACK_MATERIAL";
  CREATE TABLE "GIS"."VL_RACK_MATERIAL" (
    "VL_KEY"    VARCHAR2(10 BYTE), 
    "VL_VALUE"  VARCHAR2(30 BYTE), 
    "UNUSED"    NUMBER(1,0) DEFAULT 0, 
    "ORDINAL"   NUMBER(10,0), 
    CONSTRAINT "P_P_VL_RACK_MATER_01" PRIMARY KEY ("VL_KEY")
   );

-- VL_SECBOX_SIZE: change key from VARCHAR2(3) to VARCHAR2(10)

  DROP TABLE "GIS"."VL_SECBOX_SIZE";
  CREATE TABLE "GIS"."VL_SECBOX_SIZE" (
    "VL_KEY"    VARCHAR2(10 BYTE), 
    "VL_VALUE"  VARCHAR2(30 BYTE), 
    "UNUSED"    NUMBER(1,0) DEFAULT 0, 
    "ORDINAL"   NUMBER(10,0), 
    CONSTRAINT "P_P_VL_SECBOX_S_01" PRIMARY KEY ("VL_KEY")
   );


spool off;
