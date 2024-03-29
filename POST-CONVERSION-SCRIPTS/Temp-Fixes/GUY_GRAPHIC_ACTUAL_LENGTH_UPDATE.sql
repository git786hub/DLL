SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL GUY_GRAPHIC_ACTUAL_LENGTH_UPDATE.LOG

ALTER TABLE B$COMMON_N DISABLE ALL TRIGGERS;


UPDATE B$COMMON_N CO
SET CO.LENGTH_GRAPHIC_Q = 17 / 3.281,
    CO.LENGTH_ACTUAL_Q  = 17,
    CO.LENGTH_GRAPHIC_FT = 17
WHERE 1=1
AND CO.LENGTH_GRAPHIC_Q IS NULL
AND CO.LENGTH_ACTUAL_Q IS NULL
AND CO.LTT_ID = 0
AND EXISTS
(
	SELECT 1 
    FROM GIS.B$GUY_N GUY
    WHERE 1=1
    AND GUY.G3E_FID = CO.G3E_FID
    AND GUY.TYPE_C = 'DN'
)
;

COMMIT;

ALTER TABLE B$COMMON_N ENABLE ALL TRIGGERS;

SPOOL OFF;