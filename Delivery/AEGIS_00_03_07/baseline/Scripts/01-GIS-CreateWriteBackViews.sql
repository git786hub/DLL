SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON

SPOOL c:\temp\CreateWriteBackViews.sql.log
--**************************************************************************************
--SCRIPT NAME: CreateWriteBackViews.sql
--**************************************************************************************
-- AUTHOR		: SAGARWAL
-- DATE			: 02-MAR-2018
-- JIRA NUMBER		: ONCORDEV-1329
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		:
-- SOURCE DATABASE	: Script to create Writeback views in schema GIS_STG
--**************************************************************************************
-- Modified:
--  16-MAY-2018 - Changes to WRVIEW_VOUCHER.  Moved views to GIS_ONC.
--  25-JUN-2018, Rich Adase: Reorganized WRVIEW_WORKPOINT to ensure both point and span records are generated
--                           Added a REPLACE to WRVIEW_CU to remove # delimiter from WM_SEQ (workaround until code fix) 
--*************************************************************************************

DROP PUBLIC SYNONYM WRVIEW_WORKPOINT;
DROP PUBLIC SYNONYM WRVIEW_CU;
DROP PUBLIC SYNONYM WRVIEW_VOUCHER;
DROP PUBLIC SYNONYM WRVIEW_OVERRIDE;

CREATE OR REPLACE VIEW GIS_ONC.WRVIEW_WORKPOINT
AS
  SELECT 
    wp.WR_NBR       AS WR_NBR
  , wp.WP_NBR       AS WP_NBR
  , wp.G3E_FID      AS PS_SEQ_NBR
  , 'P'             AS PS_TYPE_C
  , 0               AS SPAN_LENGTH_FT
  , 0               AS PS_FROM_NBR
  , 0               AS PS_TO_NBR
  , wp.STRUCTURE_ID AS STRUCTURE_ID_FROM
  , '0'             AS STRUCTURE_ID_TO
  , (SELECT MAX(conn.VOLT_1_Q)
      FROM CONNECTIVITY_N conn
      JOIN COMMON_N       comm ON conn.G3E_FID = comm.G3E_FID
      WHERE comm.STRUCTURE_ID = wp.STRUCTURE_ID)      AS OPERATING_VOLTAGE
  , (SELECT MAX(LTRIM(conn.FEEDER_1_ID, conn.SSTA_C))
      FROM CONNECTIVITY_N conn
      JOIN COMMON_N comm ON conn.G3E_FID = comm.G3E_FID
      WHERE comm.STRUCTURE_ID = wp.STRUCTURE_ID)      AS FEEDER_NBR
  FROM  WORKPOINT_N     wp
    union all
  select 
    wp1.WR_NBR              AS WR_NBR
  , RANK() OVER (PARTITION BY wp1.WR_NBR ORDER BY wp1.WP_NBR, cu.WM_SEQ) + 5000 as WP_NBR
  , wp1.G3E_FID             AS PS_SEQ_NBR
  , 'S'                     AS PS_TYPE_C
  , NVL(cu.QTY_LENGTH_Q, 0) AS SPAN_LENGTH_FT
  , wp1.WP_NBR              AS PS_FROM_NBR
  , wp2.WP_NBR              AS PS_TO_NBR
  , wp1.STRUCTURE_ID        AS STRUCTURE_ID_FROM
  , wp2.STRUCTURE_ID        AS STRUCTURE_ID_TO
  , (SELECT MAX(conn.VOLT_1_Q)
      FROM CONNECTIVITY_N conn
      JOIN COMMON_N       comm ON conn.G3E_FID = comm.G3E_FID
      WHERE comm.STRUCTURE_ID = wp1.STRUCTURE_ID)      AS OPERATING_VOLTAGE
  , (SELECT MAX(LTRIM(conn.FEEDER_1_ID, conn.SSTA_C))
      FROM CONNECTIVITY_N conn
      JOIN COMMON_N       comm ON conn.G3E_FID = comm.G3E_FID
      WHERE comm.STRUCTURE_ID = wp1.STRUCTURE_ID)      AS FEEDER_NBR
  from WORKPOINT_N    wp1
  join WORKPOINT_CU_N cu  on (wp1.G3E_FID = cu.G3E_FID)
  join WORKPOINT_N    wp2 on (wp1.WR_NBR = wp2.WR_NBR and cu.WP_RELATED = wp2.WP_NBR)
;

CREATE OR REPLACE VIEW GIS_ONC.WRVIEW_CU
AS
   SELECT 
    wp.WR_NBR                                        AS WR_NBR
  , wp.WP_NBR                                        AS WP_NBR
  , REPLACE(cu.WM_SEQ,'#','')                        AS CU_SEQ_NBR
  , cu.G3E_FID                                       AS G3E_FID
  , cu.CU_C                                          AS CU_C
 , cu.MACRO_CU_C                                    AS MACRO_CU_C
  , cu.ACTIVITY_C                                    AS ACTIVITY_C
  , DECODE (cu.LENGTH_FLAG, 'L', cu.QTY_LENGTH_Q, 1) AS QTY_LENGTH_Q
  , cu.VINTAGE_YR                                    AS VINTAGE_YR
  , 'C'                                              AS LABOR_CLASS_C
  , cu.CIAC_C                                        AS CIAC_C
  FROM WORKPOINT_N    wp
  JOIN WORKPOINT_CU_N cu ON wp.G3E_FID = cu.G3E_FID
  JOIN (SELECT ROWNUM AS NUM FROM DUAL CONNECT BY ROWNUM <= 100) 
                      n  ON DECODE(cu.LENGTH_FLAG, 'L', 1, cu.QTY_LENGTH_Q) >= n.NUM
  WHERE NVL(cu.WP_RELATED,0) = 0
    union all
  select 
    wp1.WR_NBR                                        AS WR_NBR
  , RANK() OVER (PARTITION BY wp1.WR_NBR ORDER BY wp1.WP_NBR, cu.WM_SEQ) + 5000 
                                                      AS WP_NBR
  , REPLACE(cu.WM_SEQ,'#','')                         AS CU_SEQ_NBR
  , cu.G3E_FID                                        AS G3E_FID
  , cu.CU_C                                           AS CU_C
  , cu.MACRO_CU_C                                     AS MACRO_CU_C
  , cu.ACTIVITY_C                                     AS ACTIVITY_C
  , DECODE (cu.LENGTH_FLAG, 'L', cu.QTY_LENGTH_Q, 1)  AS QTY_LENGTH_Q
  , cu.VINTAGE_YR                                     AS VINTAGE_YR
  , 'C'                                               AS LABOR_CLASS_C
  , cu.CIAC_C                                         AS CIAC_C
  from WORKPOINT_N    wp1
  join WORKPOINT_CU_N cu  on (wp1.G3E_FID = cu.G3E_FID)
  join WORKPOINT_N    wp2 on (wp1.WR_NBR = wp2.WR_NBR and cu.WP_RELATED = wp2.WP_NBR)
;

CREATE OR REPLACE VIEW GIS_ONC.WRVIEW_VOUCHER
AS
  SELECT 
    wp.WR_NBR           AS WR_NBR
  , wp.WP_NBR           AS WP_NBR
  , v.REQUEST_UID       AS REQUEST_UID
  , v.REQUEST_D         AS REQUEST_D
  , v.VOUCHER_C         AS VOUCHER_C
  , v.FERC_PRIME_ACCT   AS FERC_PRIME_ACCT
  , v.FERC_SUB_ACCT     AS FERC_SUB_ACCT
  , v.COST_COMPONENT_C  AS COST_COMPONENT_C
  , v.AMOUNT_USD        AS AMOUNT_USD
  , '[GIS] ' || SUBSTR(v.COMMENTS, 1, 234) AS COMMENTS
	FROM WORKPOINT_N wp 
  JOIN VOUCHER_N   v ON wp.G3E_FID = v.G3E_FID
;

CREATE OR REPLACE VIEW GIS_ONC.WRVIEW_OVERRIDE
AS
  SELECT 
    vo.G3E_IDENTIFIER     AS WR_NBR
  , vo.RULE_NM            AS RULE_NM
  , vo.G3E_FNO            AS G3E_FNO
  , vo.G3E_FID            AS G3E_FID
  , vo.STRUCTURE_ID       AS STRUCTURE_ID
  , (SELECT MAX(comm.G3E_FID)
     FROM COMMON_N comm
     WHERE comm.STRUCTURE_ID = vo.STRUCTURE_ID)
                          AS STRUCTURE_FID
  , vo.ERROR_MSG          AS ERROR_MSG
  , vo.OVERRIDE_UID       AS OVERRIDE_UID
  , vo.OVERRIDE_COMMENTS  AS OVERRIDE_COMMENTS
  , vo.OVERRIDE_D         AS OVERRIDE_D
  FROM WR_VALIDATION_OVERRIDE vo
  ;


CREATE PUBLIC SYNONYM WRVIEW_WORKPOINT FOR GIS_ONC.WRVIEW_WORKPOINT;
CREATE PUBLIC SYNONYM WRVIEW_CU FOR GIS_ONC.WRVIEW_CU;
CREATE PUBLIC SYNONYM WRVIEW_VOUCHER FOR GIS_ONC.WRVIEW_VOUCHER;
CREATE PUBLIC SYNONYM WRVIEW_OVERRIDE FOR GIS_ONC.WRVIEW_OVERRIDE;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
SPOOL OFF;