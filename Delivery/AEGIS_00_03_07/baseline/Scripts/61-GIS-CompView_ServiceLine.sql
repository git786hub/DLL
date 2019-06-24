set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\CompView_ServiceLine.log

--**************************************************************************************
-- SCRIPT NAME: CompView_ServiceLine.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 07-JUN-2018
-- PRODUCT VERSION: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Component views for Service Line
--                    -- Converted outer join of COMP_UNIT_N to a subquery for performance (was obstructing data publish)
--                    -- Limited to only include primary CUs from COMP_UNIT_N
--**************************************************************************************
-- Modified:
--  16-JUL-2018, Rich Adase -- Converted subquery to inner join with where clause on CNO
--                             Added corresponding metadata statements
--**************************************************************************************


--
-- TODO: Expand to include other components besides label
--

    -- avoid cascading delete from G3E_COMPONENTVIEWDEFINITION
    alter table G3E_LEGENDENTRY disable constraint M_R_G3E_LEGENDENTRY_VNO;

--
-- SERVICE LINE
--

    -- label

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 5401;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 5401;

    CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SERVICELINE_T" AS 
    (
      SELECT
        G.G3E_ID
      , G.G3E_FNO
      , G.G3E_CNO
      , G.G3E_FID
      , G.G3E_CID
      , G.G3E_GEOMETRY
      , G.G3E_ALIGNMENT
      , NG.CONFIG_C
      ,	COMM.FEATURE_STATE_C
      , CU.ACTIVITY_C
        FROM SERVICE_LINE_T   G
        JOIN SERVICE_LINE_N   NG   on G.G3E_FID = NG.G3E_FID
        JOIN COMMON_N         COMM on G.G3E_FID = COMM.G3E_FID
        JOIN COMP_UNIT_N      CU   on G.G3E_FID = CU.G3E_FID
      WHERE G.G3E_FNO = 54
        AND G.G3E_CNO = 5403
        AND CU.G3E_CNO = 21
    );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (5401,'V_SERVICELINE_T',54,5403,540100,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5401000,5401,5403,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0,G3E_ALIGNMENT G3E_ALIGNMENT 0',54);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5401001,5401,5401,'CONFIG_C CONFIG_C 0',54);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5401002,5401,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',54);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5401003,5401,  21,'ACTIVITY_C ACTIVITY_C 0',54);


    alter table G3E_LEGENDENTRY enable constraint M_R_G3E_LEGENDENTRY_VNO;


spool off;
