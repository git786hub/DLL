/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table GIS_STG.CI_SP
-----------------------------------------------------------------------------*/
CREATE TABLE GIS_STG.CI_SP 
(
SP_ID CHAR(10 BYTE) NOT NULL,
SP_TYPE_CD CHAR(8 BYTE) DEFAULT ' ',
PREM_ID CHAR(10 BYTE) DEFAULT ' ',
CONSTRAINT "CI_SP_PK" PRIMARY KEY ("SP_ID")
);