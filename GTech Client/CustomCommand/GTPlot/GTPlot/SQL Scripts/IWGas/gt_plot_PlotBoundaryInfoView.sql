set serveroutput on size 999999;
set echo on;
spool PlotBoundaryInfoView.log;
select 'Date: ' || TO_CHAR(new_time(sysdate,'GMT','PST'), 'DD-MON-YYYY HH:MI:SS') FROM DUAL;
select 'User: '|| user || ' on database ' || global_name, '  (term='||USERENV('TERMINAL')||')' as MYCONTEXT from   global_name;
--*****************************************************************************************
--*   SCRIPT NAME: PlotBoundaryInfoView.sql
--*****************************************************************************************     
--*                           
--*   Programmer: Paul Adams
--*
--*   Tracking Number: 
--*
--*   Product Version Number: 10.00.
--*
--*   Release Number:
--*
--*   Project Identifier: IndustryWare
--*   
--*   Program Description: Implements the plotting interfaces plot boundary info view
--*
--*****************************************************************************************

-- Sample user:
--SELECT * FROM V_PLOTBNDY_GTPLOT_INFO WHERE work_order_id = '109093'  AND plan_id = '101'

DROP VIEW V_PLOTBNDY_GTPLOT_INFO;

CREATE OR REPLACE FORCE VIEW V_PLOTBNDY_GTPLOT_INFO (
   G3E_ID,
   G3E_FNO,
   G3E_FID,
   G3E_CNO,
   G3E_CID,
   BATCH_DELETE_SHAPE,
   BNDRY_E,
   BNDRY_N,
   BNDRY_NE,
   BNDRY_NW,
   BNDRY_S,
   BNDRY_SE,
   BNDRY_SW,
   BNDRY_W,
   DISP_SCALE,
   MUNICIPALITY_NAME,
   NAME,
   ORIENTATION,
   OWNER_FID,
   OWNER_FNO,
   PAPER_SIZE,
   PRODUCT_TYPE,
   SCALE,
   SHEET_NUM,
   STDMAPCONG_SEQ,
   STDMAP_1000_FT_REF,
   STDMAP_100_FT_REF,
   STDMAP_200_FT_REF,
   STDMAP_400_FT_REF,
   SYSTEM_ID,
   TOTAL_SHEETS,
   JOB_PLACE_NAME,
   JOB_PLACE_DATE,
   INSTALL_DATE,
   ABANDON_DATE,
   JOB_MODIFY_NAME,
   JOB_MODIFY_DATE,
   OWNER1_ID,
   OWNER2_ID,
   ASSEMBLYOWNER_ID,
   GPS_X_COORD,
   GPS_Y_COORD,
   STATE,
   LOCATION,
   PURCHASE_ORD_NBR,
   INSTALL_WO_NBR,
   RETIRE_WO_NBR,
   IN_SERVICE_DATE,
   ABANDONMENT_PROJECT_NUMBER,
   ASSET_YEAR,
   DATE_INSTALLED_SOURCE,
   IPID,
   PLAN_ID,
   SAP_WRK_ID,
   REGION_NAME,
   DMWO_NUMBER,
   USER_ID,
   JOB_STATE,
   SITE_IND,
   TAX_RATE_CODE,
   UNIQUE_ADDRESS_ID,
   PIPELINE_ID,
   LEGACY,
   CARS_NUMBER,
   PROJECT_NUM,
   PROJECT_NUM_LAST_MOD,
   SAP_TYPE,
   G3E_IDENTIFIER,
   G3E_OWNER,
   G3E_STATUS,
   G3E_CREATION,
   G3E_POSTED,
   G3E_CLOSED,
   G3E_FIELDUSER,
   G3E_JOBCLASS,
   G3E_JOBSTATE,
   G3E_DESCRIPTION,
   G3E_PROCESSING,
   G3E_PROCESSINGSTATUS,
   COMPANY_ID,
   G3E_POSTABLE,
   TRANSM_WO_IND,
   ORIGINATOR_NAME,
   COMMENTS,
   TEMPIDENTIFIER,
   BUS_UNIT,
   JOB_PHASE,
   CHANGE_TYPE_CD,
   ORIGINATING_DOC_ID,
   ORIGINATING_DOC_DT,
   PROJECT_DESC,
   "SYSDATE"
)
AS
   SELECT   A.G3E_ID,
            A.G3E_FNO,
            A.G3E_FID,
            A.G3E_CNO,
            A.G3E_CID,
            A.BATCH_DELETE_SHAPE,
            A.BNDRY_E,
            A.BNDRY_N,
            A.BNDRY_NE,
            A.BNDRY_NW,
            A.BNDRY_S,
            A.BNDRY_SE,
            A.BNDRY_SW,
            A.BNDRY_W,
            A.DISP_SCALE,
            A.MUNICIPALITY_NAME,
            A.NAME,
            A.ORIENTATION,
            A.OWNER_FID,
            A.OWNER_FNO,
            A.PAPER_SIZE,
            A.PRODUCT_TYPE,
            A.SCALE,
            A.SHEET_NUM,
            A.STDMAPCONG_SEQ,
            A.STDMAP_1000_FT_REF,
            A.STDMAP_100_FT_REF,
            A.STDMAP_200_FT_REF,
            A.STDMAP_400_FT_REF,
            A.SYSTEM_ID,
            A.TOTAL_SHEETS,
            B.JOB_PLACE_NAME,
            B.JOB_PLACE_DATE,
            B.INSTALL_DATE,
            B.ABANDON_DATE,
            B.JOB_MODIFY_NAME,
            B.JOB_MODIFY_DATE,
            B.OWNER1_ID,
            B.OWNER2_ID,
            B.ASSEMBLYOWNER_ID,
            B.GPS_X_COORD,
            B.GPS_Y_COORD,
            B.STATE,
            B.LOCATION,
            B.PURCHASE_ORD_NBR,
            B.INSTALL_WO_NBR,
            B.RETIRE_WO_NBR,
            B.IN_SERVICE_DATE,
            B.ABANDONMENT_PROJECT_NUMBER,
            B.ASSET_YEAR,
            B.DATE_INSTALLED_SOURCE,
            B.IPID,
            B.PLAN_ID,
            B.SAP_WRK_ID,
            B.REGION_NAME,
            B.DMWO_NUMBER,
            B.USER_ID,
            B.JOB_STATE,
            B.SITE_IND,
            B.TAX_RATE_CODE,
            B.UNIQUE_ADDRESS_ID,
            B.PIPELINE_ID,
            B.LEGACY,
            B.CARS_NUMBER,
            B.PROJECT_NUM,
            B.PROJECT_NUM_LAST_MOD,
            B.SAP_TYPE,
            C.G3E_IDENTIFIER,
            C.G3E_OWNER,
            C.G3E_STATUS,
            C.G3E_CREATION,
            C.G3E_POSTED,
            C.G3E_CLOSED,
            C.G3E_FIELDUSER,
            C.G3E_JOBCLASS,
            C.G3E_JOBSTATE,
            C.G3E_DESCRIPTION,
            C.G3E_PROCESSING,
            C.G3E_PROCESSINGSTATUS,
            C.COMPANY_ID,
            C.G3E_POSTABLE,
            C.TRANSM_WO_IND,
            C.ORIGINATOR_NAME,
            C.COMMENTS,
            C.TEMPIDENTIFIER,
            C.BUS_UNIT,
            C.JOB_PHASE,
            C.CHANGE_TYPE_CD,
            C.ORIGINATING_DOC_ID,
            C.ORIGINATING_DOC_DT,
            C.PROJECT_DESC,
            TO_CHAR (SYSDATE, 'YYYY/MM/DD') "SYSDATE"
     FROM   PLOTBNDY_N A, COMMON_N B, G3E_JOB C
    WHERE   A.G3E_FID = B.G3E_FID
            AND B.JOB_MODIFY_NAME = C.G3E_DESCRIPTION;
            
DROP PUBLIC SYNONYM V_PLOTBNDY_GTPLOT_INFO;

CREATE PUBLIC SYNONYM V_PLOTBNDY_GTPLOT_INFO FOR V_PLOTBNDY_GTPLOT_INFO;


GRANT DELETE, INSERT, SELECT, UPDATE ON V_PLOTBNDY_GTPLOT_INFO TO DESIGNER;

GRANT SELECT ON V_PLOTBNDY_GTPLOT_INFO TO FINANCE;

GRANT SELECT ON V_PLOTBNDY_GTPLOT_INFO TO MARKETING;

GRANT SELECT ON V_PLOTBNDY_GTPLOT_INFO TO PUBLIC;




spool off;
