SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL PREMISE_STREET_CUSTOMER_UPDATE.LOG

ALTER TABLE B$PREMISE_N DISABLE ALL TRIGGERS;

UPDATE B$PREMISE_N
SET MAJOR_CUSTOMER_C = DECODE(MAJOR_CUSTOMER_C, NULL, 'N', MAJOR_CUSTOMER_C)
   ,CRITICAL_CUSTOMER_C = DECODE(CRITICAL_CUSTOMER_C, NULL, 'N', CRITICAL_CUSTOMER_C)
WHERE 1=1
    AND MAJOR_CUSTOMER_C IS NULL
    AND CRITICAL_CUSTOMER_C IS NULL
;

UPDATE B$PREMISE_N
SET STREET_TYPE_C = DECODE(STREET_TYPE_C, NULL, 'UNKNOWN', STREET_TYPE_C)
   ,STREET_NM = DECODE(STREET_NM, NULL, 'UNKNOWN', STREET_NM)
WHERE 1=1
    AND STREET_TYPE_C IS NULL
    AND STREET_NM IS NULL
;

COMMIT;

ALTER TABLE B$PREMISE_N ENABLE ALL TRIGGERS;

SPOOL OFF;