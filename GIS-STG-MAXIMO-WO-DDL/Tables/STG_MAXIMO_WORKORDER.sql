DROP TABLE GIS_STG.STG_MAXIMO_WORKORDER CASCADE CONSTRAINTS;
  CREATE TABLE "GIS_STG"."STG_MAXIMO_WORKORDER" 
   (	"STG_MAXIMO_WORKORDER_ID" NUMBER GENERATED ALWAYS AS IDENTITY, 
	"PROCESS_DT" DATE, 
	"PROCESS_RUN_ID" NUMBER, 
	"WO_NUM" VARCHAR2(10 BYTE), 
	"STATUS" VARCHAR2(16 BYTE), 
	"STATUS_DATE" DATE, 
	"WORK_TYPE" VARCHAR2(5 BYTE), 
	"REPORT_DATE" DATE, 
	"WORK_ORDER_ID" NUMBER, 
	"CG_CAPITAL" NUMBER, 
	"CG_EVENT_ID" VARCHAR2(12 BYTE), 
	"CG_ORIG_EVENT_ID" VARCHAR2(12 BYTE), 
	"SUBSTATION_NM" VARCHAR2(300 BYTE), 
	"FEEDER_NM" VARCHAR2(300 BYTE), 
	"WORK_LOCATION" VARCHAR2(300 BYTE), 
	"PROCESS_STATUS" VARCHAR2(30 BYTE), 
	"AUD_CREATE_USR_ID" VARCHAR2(30 BYTE) DEFAULT USER, 
	"AUD_MOD_USR_ID" VARCHAR2(30 BYTE) , 
	"AUD_CREATE_DT" TIMESTAMP (6)  DEFAULT SYSDATE, 
	"AUD_MOD_DT" TIMESTAMP (6)
   );