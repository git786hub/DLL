/*-----------------------------------------------------------------------------
ALTER Tables for CIS_ESI_LOCATIONS
--Add PREM_ID for Informatica replication use
--Add CCB_CCCD_EXP_DT for Informatica replication use
--Add CRIT_CUST_CD for Informatica replication use
--Add MAJR_CUST_CD for Informatica replication use
--Add Primary Key Constraint (CIS_ESI_LOCATIONS_PK) to PREM_ID for Informatica replication use
-----------------------------------------------------------------------------*/
ALTER TABLE GIS_STG.CIS_ESI_LOCATIONS
ADD PREM_ID VARCHAR2(10)
;
ALTER TABLE GIS_STG.CIS_ESI_LOCATIONS
ADD CCB_CCCD_EXP_DT VARCHAR2(50)
;
ALTER TABLE GIS_STG.CIS_ESI_LOCATIONS
ADD CRIT_CUST_CD VARCHAR2(16)
;
ALTER TABLE GIS_STG.CIS_ESI_LOCATIONS
ADD MAJR_CUST_CD VARCHAR2(16)
;
ALTER TABLE GIS_STG.CIS_ESI_LOCATIONS
ADD ESI_PRE VARCHAR2(30)
;
ALTER TABLE GIS_STG.CIS_ESI_LOCATIONS ADD CONSTRAINT CIS_ESI_LOCATIONS_PK PRIMARY KEY (PREM_ID);