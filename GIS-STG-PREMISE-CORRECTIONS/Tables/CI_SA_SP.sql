/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table GIS_STG.CI_SA_SP
-----------------------------------------------------------------------------*/
CREATE TABLE GIS_STG.CI_SA_SP 
(
SP_ID CHAR(10 BYTE) DEFAULT ' ',
SA_SP_ID CHAR(10 BYTE) NOT NULL,
SA_ID CHAR(10 BYTE) DEFAULT ' ',
CONSTRAINT "CI_SA_SP_PK" PRIMARY KEY ("SA_SP_ID")
);