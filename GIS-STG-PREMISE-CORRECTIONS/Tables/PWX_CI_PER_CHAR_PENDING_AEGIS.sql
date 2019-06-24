/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table GIS_STG.PWX_CI_PER_CHAR_PENDING_AEGIS
-----------------------------------------------------------------------------*/
CREATE TABLE "GIS_STG"."PWX_CI_PER_CHAR_PENDING_AEGIS" 
   (	"PER_ID" CHAR(10 BYTE), 
	"CHAR_TYPE_CD" CHAR(8 BYTE), 
	"CHAR_VAL" CHAR(16 BYTE) DEFAULT ' ', 
	"EFFDT" DATE, 
	"ADHOC_CHAR_VAL" VARCHAR2(254 BYTE) DEFAULT ' ', 
	"VERSION" NUMBER(5,0) DEFAULT 1, 
	"CHAR_VAL_FK1" VARCHAR2(50 BYTE) DEFAULT ' ', 
	"CHAR_VAL_FK2" VARCHAR2(50 BYTE) DEFAULT ' ', 
	"CHAR_VAL_FK3" VARCHAR2(50 BYTE) DEFAULT ' ', 
	"CHAR_VAL_FK4" VARCHAR2(50 BYTE) DEFAULT ' ', 
	"CHAR_VAL_FK5" VARCHAR2(50 BYTE) DEFAULT ' ', 
	"SRCH_CHAR_VAL" VARCHAR2(50 BYTE) DEFAULT ' ', 
	"PROCESS_FLG" VARCHAR2(1 BYTE), 
	"INSERT_DTTM" DATE
   );