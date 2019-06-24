
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2660, 'BizRules_FuseCoordination');
spool c:\temp\BizRules_FuseCoordination.log
--**************************************************************************************
--SCRIPT NAME: BizRules_FuseCoordination.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\HKONDA
-- DATE		   : 11-JUN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.03
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

DROP TABLE "GIS_ONC"."FUSE_COORDINATION";
DROP PUBLIC SYNONYM "FUSE_COORDINATION";

CREATE TABLE "GIS_ONC"."FUSE_COORDINATION" (
  "LINK_SIZE_C"   VARCHAR2(5 BYTE)   NOT NULL ENABLE 
, "SIZE_ORDINAL"   NUMBER(3)   NOT NULL ENABLE 
);
COMMENT ON COLUMN "GIS_ONC"."FUSE_COORDINATION"."LINK_SIZE_C" IS 'Fuse link size';
COMMENT ON COLUMN "GIS_ONC"."FUSE_COORDINATION"."SIZE_ORDINAL"  IS 'Ordinal value allowing all fuse ratings to be sorted, with the highest number indicating the highest rating.';

CREATE PUBLIC SYNONYM "FUSE_COORDINATION" FOR "GIS_ONC"."FUSE_COORDINATION";

GRANT SELECT ON FUSE_COORDINATION TO PRIV_EDIT;
GRANT SELECT ON FUSE_COORDINATION TO EVERYONE;

Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('102' ,12 );
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('150' ,11 ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('101' ,11 ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('125' ,10 ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('100' ,9  ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('80' ,8   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('75' ,8   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('65' ,7   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('60' ,7   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('50' ,6   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('40' ,5   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('30' ,4   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('25' ,3   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('20' ,2   ); 
Insert into GIS_ONC.FUSE_COORDINATION (LINK_SIZE_C,SIZE_ORDINAL) values ('15' ,1   );  

COMMIT;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2660);

