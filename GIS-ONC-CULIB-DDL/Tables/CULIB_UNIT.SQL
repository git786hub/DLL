DROP TABLE GIS_ONC.CULIB_UNIT CASCADE CONSTRAINTS;
CREATE TABLE GIS_ONC.CULIB_UNIT
(
ID NUMBER GENERATED ALWAYS AS IDENTITY,
CU_ID	VARCHAR2(15) NOT NULL,
CU_DESC	VARCHAR2(120),
CATEGORY_C	VARCHAR2(15) NOT NULL,
PRIME_ACCT_NBR	NUMBER(3,0),
RETIREMENT_NBR	NUMBER(1,0),
PROPERTY_UNIT_C	NUMBER(4,0),
EFFECTIVE_D	DATE,
EXPIRATION_D	DATE,
MATERIAL_AMT	NUMBER(11,2),
UNIT_OF_MEASURE_C	VARCHAR2(5),
SHORT_WORK_INSTRUCTION	VARCHAR2(10),
AUD_CREATE_USR_ID  VARCHAR2(30) DEFAULT USER, 
AUD_MOD_USR_ID  VARCHAR2(30),
AUD_CREATE_DT   TIMESTAMP(6) DEFAULT SYSDATE,
AUD_MOD_DT  TIMESTAMP(6) 
);

ALTER TABLE GIS_ONC.CULIB_UNIT ADD CONSTRAINT CULIB_UNIT_PK PRIMARY KEY (CU_ID);

