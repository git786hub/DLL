DROP TABLE GIS_STG.STG_CULIB_MATERIAL CASCADE CONSTRAINTS;
  CREATE TABLE GIS_STG.STG_CULIB_MATERIAL 
   (	STG_CULIB_MATERIAL_ID NUMBER GENERATED ALWAYS AS IDENTITY, 
	PROCESS_DT DATE, 
	PROCESS_RUN_ID NUMBER, 
	MATERIAL_ITEM_ID VARCHAR2(8 BYTE), 
	MATERIAL_SOURCE_CODE VARCHAR2(1 BYTE), 
	MMS_UNIT_COST NUMBER(12,4), 
	UNIT_OF_MEASURE_CODE VARCHAR2(5 BYTE), 
	MMS_UNIT_OF_MEASURE_CODE VARCHAR2(5 BYTE), 
	SALVAGE_VALUE_AMT NUMBER(12,4), 
	WIRE_SAG_PCT NUMBER(2,0), 
	MATERIAL_DESC VARCHAR2(40 BYTE), 
	MINIMUM_ISSUE_QTY NUMBER(3,0), 
	PRIME_ACCT NUMBER(3,0), 
	UNIT_OF_PROPERTY_CODE NUMBER(4,0), 
	EXPIRED_YN VARCHAR2(1 BYTE), 
	UOM_CONVERSION_FACTOR NUMBER(6,2), 
	CAPITALIZED_YN VARCHAR2(1 BYTE), 
	PMMS_STD_CODE VARCHAR2(3 BYTE), 
	PREV_CAPITALIZED_YN VARCHAR2(1 BYTE), 
	PMMS_YN VARCHAR2(1 BYTE), 
	UPDATE_YN VARCHAR2(1 BYTE), 
	ALLOWANCE_FACTOR NUMBER(3,0), 
	VIRTUAL_STOREROOM_YN VARCHAR2(2 BYTE), 
	AUD_CREATE_USR_ID VARCHAR2(30 BYTE) DEFAULT USER, 
	AUD_MOD_USR_ID VARCHAR2(30 BYTE), 
	AUD_CREATE_DT TIMESTAMP (6) DEFAULT SYSDATE, 
	AUD_MOD_DT TIMESTAMP (6)
   );

