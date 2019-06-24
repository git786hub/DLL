SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL DELETE_PHASE_CU_PRI_SWITCH.LOG

ALTER TABLE GIS.B$PRI_SWITCH_UNIT_N DISABLE ALL TRIGGERS;
                        
DELETE
    FROM GIS.B$PRI_SWITCH_UNIT_N SW
    WHERE 1=1
        AND SW.LTT_ID = 0
        AND SW.G3E_CID > 1
        AND SW.G3E_FNO IN (13, 39)
        AND SW.PHASE_C IN ('X', 'x', NULL, 'UNK')
        AND NOT EXISTS 
            (
                SELECT 1
                FROM GIS.B$COMP_UNIT_N CU
                WHERE 1=1
                    AND LTT_ID = 0
                    AND CU_C IS NULL
                    AND SW.G3E_CID = CU.G3E_CID
                    AND SW.G3E_FID = CU.G3E_FID
                    AND SW.G3E_FNO = CU.G3E_FNO
            );    
COMMIT;

ALTER TABLE GIS.B$PRI_SWITCH_UNIT_N ENABLE ALL TRIGGERS;

SPOOL OFF;