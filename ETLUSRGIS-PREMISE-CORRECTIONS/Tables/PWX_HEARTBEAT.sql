/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table ETLUSERGIS.PWX_HEARTBEAT
-----------------------------------------------------------------------------*/
CREATE TABLE "ETLUSRGIS"."PWX_HEARTBEAT" 
   (	"WORKFLOW_RUN_ID" NUMBER(9,0), 
	"PC_MAPPING_NAME" VARCHAR2(100 BYTE), 
	"SRC_UPD_DT" TIMESTAMP (6), 
	"PWX_DTL__CAPXTIMESTAMP" TIMESTAMP (6), 
	"PC_UPD_DT" TIMESTAMP (6), 
	"TGT_UPD_DT" TIMESTAMP (6) DEFAULT SYSDATE
   );