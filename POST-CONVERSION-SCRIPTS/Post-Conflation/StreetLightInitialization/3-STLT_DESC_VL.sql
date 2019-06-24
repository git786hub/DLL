set echo on
set linesize 1000
set pagesize 300
set trimspool on
SPOOL STLT_DESC_INSERTS.LOG


TRUNCATE TABLE GIS_ONC.STLT_DESC_VL;

INSERT INTO GIS_ONC.STLT_DESC_VL
    (DESCRIPTION_ID,DESCRIPTION,MSLA_DATE
    )
SELECT DISTINCT DESCRIPTION_ID,OWNER_NAME,MSLA_DATE 
FROM GIS_ONC.STLT_ACCOUNT_MAPPING;
COMMIT;


spool off;