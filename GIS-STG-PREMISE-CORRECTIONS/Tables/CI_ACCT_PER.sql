/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table GIS_STG.CI_ACCT_PER
-----------------------------------------------------------------------------*/
CREATE TABLE GIS_STG.CI_ACCT_PER 
(
ACCT_ID CHAR(10 BYTE) NOT NULL,
PER_ID CHAR(10 BYTE) NOT NULL,
MAIN_CUST_SW CHAR(1 BYTE) DEFAULT ' ',
CONSTRAINT "CI_ACCT_PER_PK" PRIMARY KEY ("ACCT_ID", "PER_ID")
);