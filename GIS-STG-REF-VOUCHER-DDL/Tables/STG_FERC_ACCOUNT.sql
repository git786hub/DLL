DROP TABLE GIS_STG.STG_FERC_ACCOUNT CASCADE CONSTRAINTS;
  CREATE TABLE GIS_STG.STG_FERC_ACCOUNT 
   (	STG_FERC_ACCOUNT_ID NUMBER GENERATED ALWAYS AS IDENTITY, 
	PROCESS_DT DATE NOT NULL ENABLE, 
	PROCESS_RUN_ID NUMBER NOT NULL ENABLE, 
	GRAPHIC_ACTIVITY_CODE VARCHAR2(2 BYTE) NOT NULL ENABLE, 
	PRIME_ACCT NUMBER(3,0) NOT NULL ENABLE, 
	SUB_ACCT NUMBER(4,0) NOT NULL ENABLE, 
	CODE_DESC VARCHAR2(30 BYTE), 
	USE_CODE VARCHAR2(1 BYTE), 
	C_M_O_FLAG VARCHAR2(1 BYTE), 
	AUD_CREATE_USR_ID VARCHAR2(30 BYTE) DEFAULT USER, 
	AUD_MOD_USR_ID VARCHAR2(30 BYTE), 
	AUD_CREATE_DT TIMESTAMP (6) DEFAULT SYSDATE, 
	AUD_MOD_DT TIMESTAMP (6)
   );

