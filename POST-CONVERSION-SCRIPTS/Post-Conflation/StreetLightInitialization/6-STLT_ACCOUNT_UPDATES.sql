SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL STLT_ACCOUNT_UPDATES.LOG

--format esi location

UPDATE GIS_ONC.STLT_ACCOUNT SA SET SA.ESI_LOCATION = LPAD (SA.ESI_LOCATION,10,'0') ;
COMMIT;

--Update account values

UPDATE GIS_ONC.STLT_ACCOUNT SA
  SET
    (
      SA.OWNER_CODE
    ,SA.RATE_CODE
    ,SA.DESCRIPTION_ID
    )
    =
    (SELECT   NVL (SAM.OWNER_CODE,SA.OWNER_CODE) OWNER_CODE
      ,NVL (SAM.RATE_CODE,SA.RATE_CODE) RATE_CODE
      ,NVL (SAM.DESCRIPTION_ID,SA.DESCRIPTION_ID) DESCRIPTION_ID
      FROM GIS_ONC.STLT_ACCOUNT_MAPPING SAM
      WHERE SA.ESI_LOCATION = SAM.ESI_LOCATION
    )
  WHERE EXISTS
    (SELECT 1 FROM GIS_ONC.STLT_ACCOUNT_MAPPING SAM WHERE SA.ESI_LOCATION = SAM.ESI_LOCATION
    ) ;
    
COMMIT;

SPOOL OFF;