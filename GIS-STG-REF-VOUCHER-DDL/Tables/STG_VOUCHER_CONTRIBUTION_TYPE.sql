DROP TABLE GIS_STG.STG_VOUCHER_CONTRIBUTION_TYPE CASCADE CONSTRAINTS;
  CREATE TABLE GIS_STG.STG_VOUCHER_CONTRIBUTION_TYPE 
   (	STG_VCHR_CNTR_TYP_ID NUMBER GENERATED ALWAYS AS IDENTITY, 
	PROCESS_DT DATE NOT NULL ENABLE, 
	PROCESS_RUN_ID NUMBER, 
	VOUCHER_CNTRB_TYPE_CODE VARCHAR2(5 BYTE), 
	ACTIVE_YN VARCHAR2(2 BYTE), 
	VOUCHER_CNTRB_VC VARCHAR2(1 BYTE), 
	CODE_DESC VARCHAR2(30 BYTE), 
	REQ_NO NUMBER(4,0), 
	STANDARD_CHARGE_AMT NUMBER(11,0), 
	USE_IN_EST_YN VARCHAR2(1 BYTE), 
	AUD_CREATE_USR_ID VARCHAR2(30 BYTE) DEFAULT USER, 
	AUD_MOD_USR_ID VARCHAR2(30 BYTE), 
	AUD_CREATE_DT TIMESTAMP (6) DEFAULT SYSDATE, 
	AUD_MOD_DT TIMESTAMP (6)
   );

