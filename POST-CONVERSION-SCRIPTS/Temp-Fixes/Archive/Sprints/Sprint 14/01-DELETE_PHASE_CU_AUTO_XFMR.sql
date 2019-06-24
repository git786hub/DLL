SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL DELETE_PHASE_CU_AUTO_XFMR.LOG

ALTER TABLE B$COMP_UNIT_N DISABLE ALL TRIGGERS;
ALTER TABLE B$AUTO_XFMR_UNIT_N DISABLE ALL TRIGGERS;

DELETE
FROM GIS.B$COMP_UNIT_N CU
    WHERE 1=1 
        AND CU.G3E_FNO = 34
        AND CU.G3E_CID > 1
        AND CU.LTT_ID = 0
        AND CU.CU_C IS NULL
        AND EXISTS
        (
            SELECT 1
            FROM GIS.B$AUTO_XFMR_UNIT_N AXR
            WHERE 1=1 
                AND AXR.G3E_FID = CU.G3E_FID
                AND AXR.G3E_CID = CU.G3E_CID
                AND AXR.PHASE_C IS NULL 
                AND AXR.LTT_ID = 0
        );
      
DELETE 
FROM GIS.B$AUTO_XFMR_UNIT_N AXR
    WHERE 1=1 
    AND AXR.G3E_CID > 1
    AND AXR.LTT_ID = 0
    AND AXR.PHASE_C IS NULL
    AND NOT EXISTS
    (
        select 1
        FROM GIS.B$COMP_UNIT_N CU
        WHERE 1=1 
            AND CU.G3E_FID = AXR.G3E_FID
            AND CU.G3E_CID = AXR.G3E_CID
            AND CU.LTT_ID = 0
            AND CU.G3E_CID > 1
            AND CU.CU_C IS NULL
    );
        
COMMIT;
ALTER TABLE B$COMP_UNIT_N ENABLE ALL TRIGGERS;
ALTER TABLE B$AUTO_XFMR_UNIT_N ENABLE ALL TRIGGERS;


SPOOL OFF;