/*-----------------------------------------------------------------------------
Create tables used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Table GIS_STG.CI_PREM
-----------------------------------------------------------------------------*/
CREATE TABLE GIS_STG.CI_PREM 
(
PREM_ID CHAR(10 BYTE) NOT NULL,
PREM_TYPE_CD CHAR(8 BYTE) DEFAULT ' ',
CONSTRAINT "CI_PREM_PK" PRIMARY KEY ("PREM_ID")
);