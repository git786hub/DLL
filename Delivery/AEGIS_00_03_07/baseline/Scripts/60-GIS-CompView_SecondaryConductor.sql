set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\CompView_SecondaryConductor.log
--**************************************************************************************
-- SCRIPT NAME: CompView_SecondaryConductor.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 16-JUL-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Component views for Secondary Conductor features
--**************************************************************************************

--
-- TODO: Expand to include other components besides linears
--

    -- avoid cascading delete from G3E_COMPONENTVIEWDEFINITION
    alter table G3E_LEGENDENTRY disable constraint M_R_G3E_LEGENDENTRY_VNO;

--
-- SECONDARY CONDUCTOR - OH 
--

    -- geo linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 5300;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 5300;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCOND_L" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_L     G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 53
          AND G.G3E_CNO = 5303
      );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (5300,'V_SECCOND_L',53,5303,530000,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5300000,5300,5303,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0',53);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5300001,5300,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',53);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5300002,5300,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',53);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5300003,5300,  11,'NETWORK_ID NETWORK_ID 0',53);


    -- detail linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 5302;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 5302;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCOND_DL" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , G.G3E_DETAILID
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_DL    G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 53
          AND G.G3E_CNO = 5351
      );


    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (5302,'V_SECCOND_DL',53,5351,530200,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5302000,5302,5351,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0,G3E_DETAILID G3E_DETAILID 0',53);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5302001,5302,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',53);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5302002,5302,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',53);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (5302003,5302,  11,'NETWORK_ID NETWORK_ID 0',53);


--
-- SECONDARY CONDUCTOR - UG 
--

    -- geo linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 6300;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 6300;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCONDUG_L" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_L     G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 63
          AND G.G3E_CNO = 5303
      );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (6300,'V_SECCONDUG_L',63,5303,630000,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6300000,6300,5303,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0',63);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6300001,6300,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',63);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6300002,6300,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',63);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6300003,6300,  11,'NETWORK_ID NETWORK_ID 0',63);


    -- detail linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 6302;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 6302;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCONDUG_DL" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , G.G3E_DETAILID
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_DL    G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 63
          AND G.G3E_CNO = 5351
    );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (6302,'V_SECCONDUG_DL',63,5351,630200,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6302000,6302,5351,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0,G3E_DETAILID G3E_DETAILID 0',63);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6302001,6302,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',63);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6302002,6302,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',63);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (6302003,6302,  11,'NETWORK_ID NETWORK_ID 0',63);


--
-- SECONDARY CONDUCTOR - OH NETWORK 
--

    -- geo linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 9600;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 9600;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCONDOHN_L" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_L     G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 96
          AND G.G3E_CNO = 5303
      );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (9600,'V_SECCONDOHN_L',96,5303,960000,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9600000,9600,5303,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0',96);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9600001,9600,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',96);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9600002,9600,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',96);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9600003,9600,  11,'NETWORK_ID NETWORK_ID 0',96);


    -- detail linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 9602;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 9602;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCONDOHN_DL" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , G.G3E_DETAILID
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_DL    G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 96
          AND G.G3E_CNO = 5351
    );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (9602,'V_SECCONDOHN_DL',96,5351,960200,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9602000,9602,5351,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0,G3E_DETAILID G3E_DETAILID 0',96);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9602001,9602,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',96);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9602002,9602,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',96);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9602003,9602,  11,'NETWORK_ID NETWORK_ID 0',96);


--
-- SECONDARY CONDUCTOR - UG NETWORK
--
    -- geo linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 9700;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 9700;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCONDUGN_L" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , C.BUNDLE_C
        , C.LABEL_TEXT
        , COMM.FEATURE_STATE_C
        , CONN.NETWORK_ID
        FROM SEC_COND_L     G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 97
          AND G.G3E_CNO = 5303
      );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (9700,'V_SECCONDUGN_L',97,5303,970000,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9700000,9700,5303,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0',97);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9700001,9700,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',97);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9700002,9700,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',97);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9700003,9700,  11,'NETWORK_ID NETWORK_ID 0',97);


    -- detail linear

    delete from G3E_COMPONENTVIEWDEFINITION where G3E_VNO = 9702;
    delete from G3E_COMPONENTVIEWCOMPOSITION where G3E_VNO = 9702;

      CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS"."V_SECCONDUGN_DL" AS 
      (
        SELECT 
          G.G3E_ID
        , G.G3E_FNO
        , G.G3E_CNO
        , G.G3E_FID
        , G.G3E_CID
        , G.G3E_GEOMETRY
        , G.G3E_DETAILID
        , COMM.FEATURE_STATE_C
        FROM SEC_COND_DL    G
        JOIN SEC_COND_N     C    on G.G3E_FID = C.G3E_FID
        JOIN COMMON_N       COMM on G.G3E_FID = COMM.G3E_FID
        JOIN CONNECTIVITY_N CONN on G.G3E_FID = CONN.G3E_FID
        WHERE G.G3E_FNO = 97
          AND G.G3E_CNO = 5351
    );

    Insert into G3E_COMPONENTVIEWDEFINITION (G3E_VNO,G3E_VIEW,G3E_FNO,G3E_CNO,G3E_LENO,G3E_FIELDS,G3E_ATTRAFFECTSMEMBERSHIP) values (9702,'V_SECCONDUGN_DL',97,5351,970200,null,0);

    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9702000,9702,5351,'G3E_ID G3E_ID 0,G3E_FNO G3E_FNO 0,G3E_CNO G3E_CNO 0,G3E_FID G3E_FID 0,G3E_CID G3E_CID 0,G3E_GEOMETRY G3E_GEOMETRY 0,G3E_DETAILID G3E_DETAILID 0',97);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9702001,9702,5301,'BUNDLE_C BUNDLE_C 0, LABEL_TEXT LABEL_TEXT 0',97);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9702002,9702,   1,'FEATURE_STATE_C FEATURE_STATE_C 0',97);
    Insert into G3E_COMPONENTVIEWCOMPOSITION (G3E_CVCNO,G3E_VNO,G3E_CNO,G3E_FIELDS,G3E_FNO) values (9702003,9702,  11,'NETWORK_ID NETWORK_ID 0',97);


    alter table G3E_LEGENDENTRY enable constraint M_R_G3E_LEGENDENTRY_VNO;


spool off;
