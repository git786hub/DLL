SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL PRI_SWITCH_TIE_SSTA_FEEDER_UPDATE.LOG 
ALTER TABLE B$CONNECTIVITY_N DISABLE ALL TRIGGERS;
    MERGE INTO B$CONNECTIVITY_N C1 USING
    ( WITH FDR AS
        (SELECT DISTINCT CN.G3E_ID
          ,CN.G3E_FNO
          ,CN.G3E_CNO
          ,CN.G3E_FID
          ,CN.G3E_CID
          ,CN.SSTA_C SSTA_M
          ,CN.FEEDER_1_ID FEEDER_M
          ,CN.NODE_1_ID NODE_1_M
          ,CN.NODE_2_ID NODE_2_M
          FROM B$CONNECTIVITY_N CN
          WHERE 1                  = 1
            AND CN.G3E_FNO        IN (13,39)
            AND CN.STATUS_NORMAL_C = 'OPEN'
            AND
            (
              CN.NODE_1_ID    <> 0
              OR CN.NODE_2_ID <> 0
            )
            AND CN.NODE_1_ID     <> CN.NODE_2_ID
            AND CN.LTT_ID         = 0
            AND CN.NODE_1_ID NOT IN (1611502932,1591636420,1651492981) -- Remove Supernodes
            AND CN.NODE_2_ID NOT IN (1611502932,1591636420,1651492981) -- Remove Supernodes
        )
      ,CONNECTED AS
        (SELECT   FDR.G3E_ID
          ,FDR.G3E_FNO
          ,FDR.G3E_CNO
          ,FDR.G3E_FID
          ,FDR.G3E_CID
          ,FDR.SSTA_M
          ,FDR.FEEDER_M
          ,DECODE (FDR.NODE_1_M,CON.NODE_1_ID,CON.SSTA_C,CON.NODE_2_ID,CON.SSTA_C) SSTA_N1
          ,DECODE (FDR.NODE_1_M,CON.NODE_1_ID,CON.FEEDER_1_ID,CON.NODE_2_ID,CON.FEEDER_1_ID) FEEDER_N1
          ,DECODE (FDR.NODE_2_M,CON.NODE_1_ID,CON.SSTA_C,CON.NODE_2_ID,CON.SSTA_C) SSTA_N2
          ,DECODE (FDR.NODE_2_M,CON.NODE_1_ID,CON.FEEDER_1_ID,CON.NODE_2_ID,CON.FEEDER_1_ID) FEEDER_N2
          ,CON.G3E_FID G3E_FID_N
          ,CON.SSTA_C SSTA_N
          ,CON.FEEDER_1_ID FEEDER_N
          ,FDR.NODE_1_M
          ,FDR.NODE_2_M
          ,DECODE (FDR.NODE_1_M,CON.NODE_1_ID,1,CON.NODE_2_ID,1,0) NODE1
          ,DECODE (FDR.NODE_2_M,CON.NODE_1_ID,1,CON.NODE_2_ID,1,0) NODE2
          ,CON.NODE_1_ID NODE_1_N
          ,CON.NODE_2_ID NODE_2_N
          ,DECODE (FDR.NODE_1_M,CON.NODE_1_ID,CON.NODE_1_ID,CON.NODE_2_ID,CON.NODE_2_ID,NULL) NODE1_N
          ,DECODE (FDR.NODE_2_M,CON.NODE_1_ID,CON.NODE_1_ID,CON.NODE_2_ID,CON.NODE_2_ID,NULL) NODE2_N
            --       ,DENSE_RANK () OVER (PARTITION BY fdr.G3E_FID ORDER BY decode (fdr.NODE_1_M,con.NODE_1_ID,con.NODE_2_ID,con.NODE_2_ID,con.NODE_1_ID,0)) node1_seq
            --       ,COUNT(1) OVER (PARTITION BY fdr.G3E_FID)    AS node1_COUNT
          FROM FDR
          LEFT JOIN B$CONNECTIVITY_N CON
          ON
            (
              (
                FDR.NODE_1_M       = CON.NODE_1_ID
                AND FDR.NODE_1_M  <> 0
                AND CON.NODE_1_ID <> 0
              )
              OR
              (
                FDR.NODE_1_M       = CON.NODE_2_ID
                AND FDR.NODE_1_M  <> 0
                AND CON.NODE_2_ID <> 0
              )
              OR
              (
                FDR.NODE_2_M       = CON.NODE_1_ID
                AND FDR.NODE_2_M  <> 0
                AND CON.NODE_1_ID <> 0
              )
              OR
              (
                FDR.NODE_2_M       = CON.NODE_2_ID
                AND FDR.NODE_2_M  <> 0
                AND CON.NODE_2_ID <> 0
              )
            )
          WHERE 1              = 1
            AND FDR.G3E_FID   <> CON.G3E_FID
            AND CON.NODE_1_ID <> CON.NODE_2_ID
            AND CON.LTT_ID     = 0
        )
      , NEWFDR AS
        (SELECT   G3E_ID
          ,G3E_FNO
          ,G3E_CNO
          ,G3E_FID
          ,G3E_CID
          ,MAX (SSTA_M) SSTA_M
          ,MAX (FEEDER_M) FEEDER_M
          ,MAX (SSTA_N1) SSTA_N1
          ,MAX (FEEDER_N1) FEEDER_N1
          ,MAX (SSTA_N2) SSTA_N2
          ,MAX (FEEDER_N2) FEEDER_N2
          FROM CONNECTED
          GROUP BY G3E_ID
          ,G3E_FNO
          ,G3E_CNO
          ,G3E_FID
          ,G3E_CID
        )
      SELECT   G3E_ID
        ,G3E_FNO
        ,G3E_CNO
        ,G3E_CID
        ,G3E_FID
        ,NVL (SSTA_N1,SSTA_M) SSTA_C
        ,NVL (FEEDER_N1,FEEDER_M) FEEDER_1_ID
        ,NVL (SSTA_N2,SSTA_M) TIE_SSTA_C
        ,NVL (FEEDER_N2,FEEDER_M) FEEDER_2_ID
        FROM NEWFDR
    ) C2 ON
    (
      C1.G3E_ID = C2.G3E_ID AND C1.G3E_FNO = C2.G3E_FNO AND C1.G3E_CNO = C2.G3E_CNO AND C1.G3E_FID = C2.G3E_FID AND C1.G3E_CID = C2.G3E_CID
    )
    WHEN MATCHED THEN
      UPDATE
        SET C1.SSTA_C   = C2.SSTA_C
        ,C1.FEEDER_1_ID = C2.FEEDER_1_ID
        ,C1.TIE_SSTA_C  = C2.TIE_SSTA_C
        ,C1.FEEDER_2_ID = C2.FEEDER_2_ID ;
    
      UPDATE B$CONNECTIVITY_N
        SET TIE_SSTA_C        = SSTA_C
        ,FEEDER_2_ID          = FEEDER_1_ID
        WHERE 1               = 1
          AND G3E_FNO        IN (13,39)
          AND STATUS_NORMAL_C = 'OPEN'
          AND SSTA_C         IS NOT NULL
          AND TIE_SSTA_C     IS NULL
          AND LTT_ID          = 0
          AND
          (
            NODE_1_ID NOT    IN (1611502932,1591636420,1651492981)
            OR NODE_2_ID NOT IN (1611502932,1591636420,1651492981)
          ) ;
          
COMMIT;
ALTER TABLE B$CONNECTIVITY_N ENABLE ALL TRIGGERS;
SPOOL OFF;
