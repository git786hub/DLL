SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL COMMON_DUP_G3E_ID.LOG

CREATE TABLE GIS.B$COMMON_N_DUPS_TEMP AS
  SELECT   G3E_ID
    ,G3E_FNO
    ,G3E_CNO
    ,G3E_FID
    ,LTT_ID
    ,LTT_TID
    ,IDCOUNT
    ,FIDRANK
    FROM
      (SELECT   COUNT ( *) OVER (PARTITION BY CON.G3E_ID) IDCOUNT
        ,DENSE_RANK () OVER (PARTITION BY CON.G3E_ID ORDER BY CON.G3E_FID) FIDRANK
        ,CON.*
        FROM B$COMMON_N CON
        WHERE 1          = 1
          AND CON.LTT_ID = 0
      ) C
    WHERE 1         = 1
      AND C.IDCOUNT > 1
      AND C.FIDRANK > 1;

ALTER TABLE B$COMMON_N DISABLE ALL TRIGGERS;

UPDATE GIS.B$COMMON_N UCON
  SET UCON.G3E_ID = GIS.B$COMMON_N_PKG.GETSEQUENCEVALUE
  WHERE EXISTS
    (SELECT   1
      FROM GIS.B$COMMON_N_DUPS_TEMP C
      WHERE 1            = 1
        AND UCON.G3E_ID  = C.G3E_ID
        AND UCON.G3E_FNO = C.G3E_FNO
        AND UCON.G3E_CNO = C.G3E_CNO
        AND UCON.G3E_FID = C.G3E_FID
        AND UCON.LTT_ID  = C.LTT_ID
    ) ;
COMMIT;
ALTER TABLE B$COMMON_N ENABLE ALL TRIGGERS;

DROP TABLE GIS.B$COMMON_N_DUPS_TEMP;
SPOOL OFF; 