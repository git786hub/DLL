DROP TABLE GIS_STG.STG_COUNTY_CODE CASCADE CONSTRAINTS;
CREATE TABLE GIS_STG.STG_COUNTY_CODE 
   (	STG_COUNTY_CODE_ID NUMBER GENERATED ALWAYS AS IDENTITY, 
	PROCESS_DT DATE, 
	PROCESS_RUN_ID NUMBER, 
	CNTY_C NUMBER(3,0), 
	CNTY_DESC_T VARCHAR2(20 BYTE), 
	WTHR_ZONE_C NUMBER(3,0), 
	COUNTY_TAX_CODE NUMBER(4,0), 
	AUD_CREATE_USR_ID VARCHAR2(30 BYTE) DEFAULT USER, 
	AUD_MOD_USR_ID VARCHAR2(30 BYTE), 
	AUD_CREATE_DT TIMESTAMP (6) DEFAULT SYSDATE, 
	AUD_MOD_DT TIMESTAMP (6)
   );