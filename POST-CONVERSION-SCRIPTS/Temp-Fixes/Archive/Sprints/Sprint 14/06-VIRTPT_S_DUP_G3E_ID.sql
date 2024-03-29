SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL VIRTPT_S_DUP_G3E_ID.LOG

CREATE TABLE GIS.B$VIRTPT_S_DUPS_TEMP AS
  SELECT   G3E_ID
    ,G3E_FNO
    ,G3E_CNO
    ,G3E_FID
    ,LTT_ID
    ,LTT_TID
    ,IDCOUNT
    ,FIDRANK
    FROM
      (SELECT   COUNT ( *) OVER (PARTITION BY VPT.G3E_ID) IDCOUNT
        ,DENSE_RANK () OVER (PARTITION BY VPT.G3E_ID ORDER BY VPT.G3E_FID) FIDRANK
        ,VPT.*
        FROM B$VIRTUALPT_S VPT
        WHERE 1          = 1
          AND VPT.LTT_ID = 0
      ) V
    WHERE 1         = 1
      AND V.IDCOUNT > 1
      AND V.FIDRANK > 1;

ALTER TABLE B$VIRTUALPT_S DISABLE ALL TRIGGERS;

UPDATE GIS.B$VIRTUALPT_S VPT
  SET VPT.G3E_ID = GIS.B$VIRTUALPT_S_PKG.GETSEQUENCEVALUE
  WHERE EXISTS
    (SELECT   1
      FROM GIS.B$VIRTPT_S_DUPS_TEMP V
      WHERE 1            = 1
        AND VPT.G3E_ID  = V.G3E_ID
        AND VPT.G3E_FNO = V.G3E_FNO
        AND VPT.G3E_CNO = V.G3E_CNO
        AND VPT.G3E_FID = V.G3E_FID
        AND VPT.LTT_ID  = V.LTT_ID
    ) ;
COMMIT;
ALTER TABLE B$VIRTUALPT_S ENABLE ALL TRIGGERS;

DROP TABLE GIS.B$VIRTPT_S_DUPS_TEMP;
SPOOL OFF; 