
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2487, '1188_AssetHistoryViewTables');
spool c:\temp\2487-20180219-ONCOR-CYCLE6-JIRA-1188_AssetHistoryViewTables.log
--**************************************************************************************
--SCRIPT NAME: 2487-20180219-ONCOR-CYCLE6-JIRA-1188_AssetHistoryViewTables.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\SKAMARAJ
-- DATE		: 19-FEB-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

  CREATE TABLE "ASSET_HISTORY_VIEW" 
   (	
   "VIEW_ID" NUMBER(5,0) NOT NULL ENABLE, 
   "VIEW_NM" VARCHAR2(30 BYTE), 
   CONSTRAINT "UNIQUE_ASSET_HISTORY_VIEW" UNIQUE ("VIEW_ID")  
   );
   COMMENT ON COLUMN "ASSET_HISTORY_VIEW"."VIEW_ID" IS 'Unique identifier';
   COMMENT ON COLUMN "ASSET_HISTORY_VIEW"."VIEW_NM" IS 'Name of the saved view';
      
  CREATE TABLE "ASSET_HISTORY_VIEWFILTER" 
   (	
    "VIEW_ID" NUMBER(5,0), 
	"FILTER_COLUMN" VARCHAR2(30 BYTE), 
	"FILTER_VALUE" VARCHAR2(1000 BYTE), 
	CONSTRAINT "FK_VIEW_F_ID" FOREIGN KEY ("VIEW_ID")
	REFERENCES "ASSET_HISTORY_VIEW" ("VIEW_ID") ENABLE
   );

   COMMENT ON COLUMN "ASSET_HISTORY_VIEWFILTER"."VIEW_ID" IS 'Foreign key to the view table';
   COMMENT ON COLUMN "ASSET_HISTORY_VIEWFILTER"."FILTER_COLUMN" IS 'Name of column to filter';
   COMMENT ON COLUMN "ASSET_HISTORY_VIEWFILTER"."FILTER_VALUE" IS 'Comma-delimited list of values to include';   
   
  CREATE TABLE "ASSET_HISTORY_VIEWSORT" 
   (	
    "VIEW_ID" NUMBER(5,0), 
	"SORT_COLUMN" VARCHAR2(30 BYTE), 
	"SORT_PRIORITY" NUMBER(2,0), 
	"SORT_DIRECTION" VARCHAR2(4 BYTE), 
	 CONSTRAINT "FK_VIEW_S_ID" FOREIGN KEY ("VIEW_ID")
	 REFERENCES "ASSET_HISTORY_VIEW" ("VIEW_ID") ENABLE
   );

   COMMENT ON COLUMN "ASSET_HISTORY_VIEWSORT"."VIEW_ID" IS 'Foreign key to the view table';
   COMMENT ON COLUMN "ASSET_HISTORY_VIEWSORT"."SORT_COLUMN" IS 'Name of column to sort';
   COMMENT ON COLUMN "ASSET_HISTORY_VIEWSORT"."SORT_PRIORITY" IS 'Order in which multiple sorts should be applied';
   COMMENT ON COLUMN "ASSET_HISTORY_VIEWSORT"."SORT_DIRECTION" IS 'ASC or DESC';


CREATE OR REPLACE PUBLIC SYNONYM ASSET_HISTORY_VIEW FOR ASSET_HISTORY_VIEW;
GRANT SELECT ON ASSET_HISTORY_VIEW TO EVERYONE;

CREATE OR REPLACE PUBLIC SYNONYM ASSET_HISTORY_VIEWFILTER FOR ASSET_HISTORY_VIEWFILTER;
GRANT SELECT ON ASSET_HISTORY_VIEWFILTER TO EVERYONE;

CREATE OR REPLACE PUBLIC SYNONYM ASSET_HISTORY_VIEWSORT FOR ASSET_HISTORY_VIEWSORT;
GRANT SELECT ON ASSET_HISTORY_VIEWSORT TO EVERYONE;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2487);