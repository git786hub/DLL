set echo on
set linesize 1000
set pagesize 300
set trimspool on
SPOOL STLT_RATECODE_INSERTS.LOG

TRUNCATE TABLE GIS_ONC.STLT_RATECODE_VL;

INSERT INTO GIS_ONC.STLT_RATECODE_VL (RATE_CODE) VALUES ('560');
INSERT INTO GIS_ONC.STLT_RATECODE_VL (RATE_CODE) VALUES ('690');

COMMIT;

spool off;