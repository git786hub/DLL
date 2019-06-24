/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table GIS_STG.CI_SA
-----------------------------------------------------------------------------*/
CREATE TABLE GIS_STG.CI_SA 
(
SA_ID CHAR(10 BYTE) NOT NULL,
ACCT_ID CHAR(10 BYTE) DEFAULT ' ',
CONSTRAINT "CI_SA_PK" PRIMARY KEY ("SA_ID")
);