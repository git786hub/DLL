DROP TABLE GIS_ONC.ETL_PROCESS_CONTROL CASCADE CONSTRAINTS;
CREATE TABLE GIS_ONC.ETL_PROCESS_CONTROL
(
COMPONENT_NM VARCHAR2(10) NOT NULL,
PROCESS_RUN_ID NUMBER NOT NULL,
PROCESS_ID NUMBER NOT NULL,
PROCESS_NM VARCHAR2(250) NOT NULL,
PROCESS_START_DT TIMESTAMP NOT NULL,
PROCESS_END_DT TIMESTAMP,
LWM_DT TIMESTAMP,
HWM_DT TIMESTAMP,
PROCESS_STATUS_DESC VARCHAR2(30) NOT NULL,
AUD_CREATE_USR_ID  VARCHAR2(30) DEFAULT USER, 
AUD_MOD_USR_ID  VARCHAR2(30),
AUD_CREATE_DT   TIMESTAMP DEFAULT SYSDATE,
AUD_MOD_DT  TIMESTAMP, 
CONSTRAINT PROCESS_CONTROL_COMPOSITE_PK PRIMARY KEY (COMPONENT_NM, PROCESS_RUN_ID)
);

