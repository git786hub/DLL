
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2649, 'ONCOR_JIRA1390-LBM_UTL-pkg');
spool c:\temp\2649-20180529-ONCOR_JIRA1390-LBM_UTL-pkg.log
--**************************************************************************************
--SCRIPT NAME: 2649-20180529-ONCOR_JIRA1390-LBM_UTL-pkg.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 29-MAY-2018
-- CYCLE				: 03.06
-- JIRA NUMBER			: 1390
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Changes to PurgeExpiredArchivedLandbase procedure
--                      : Added "Order by" to SQL statement
--						: Added "Raise" to EXCEPTION block 
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************


create or replace PACKAGE BODY LBM_UTL
IS
  -----------------------------------------------------------------------------
   --  Version History
   -----------------------------------------------------------------------------
   --  Date, Name, Version
   --    Modification
   -----------------------------------------------------------------------------
   --  18.01.2018 Pramod,    10.03.00.00 Added Procedure/function to MergeLandbaseBoundaries
  --   22.02.2018 Jyothsna,  10.03.00.00 Added Procedure/function to DETECTOVERLAPPINGPOLYGONS
  --   27.04.2018 Pramod,    10.03.00.00 Modifed DETECTOVERLAPPINGPOLYGONS Procedure 
  --   04.05.2018 Pramod,    10.03.00.00 Procedure DetectPolygonEdgeMisMatch added
  --   29.05.2018 Pramod,    10.03.00.00 Made changes to PurgeExpiredArchivedLandbase procedure
 -----********************************************************************************************************

	-- Variable declaration
	cPackage   CONSTANT  VARCHAR2(30) := 'LBM_UTL';
	gErrorMsg varchar2(4000);

	FUNCTION  GetGraphicCompG3eTable(pG3eFno in NUMBER) RETURN varchar2;
	PROCEDURE DeleteFeatureInstance(pG3eFno in NUMBER,pG3eFid in NUMBER);
	FUNCTION GetVertex(pGeometry IN SDO_GEOMETRY,pVertexNum IN NUMBER) RETURN SDO_GEOMETRY;

------------------------------------------------------------------------------------------------------------
   -- Procedure MergeLandbaseBoundaries
   -- Merge Selected source Landbase feature into Target selected landbase feature
   -- Delete Source Feature instance from component table
-----------------------------------------------------------------------------------------------------------
PROCEDURE MergeLandbaseBoundaries(pG3eFno_trg in NUMBER,pG3eFid_trg in NUMBER,pG3eFno_src in NUMBER,pG3eFid_src in NUMBER)
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.MergeLandbaseBoundaries';
	vG3eTable	VARCHAR2(30);
	vG3eTable1	VARCHAR2(30);
	vMergeStatus VARCHAR2(200);
	sdoGeometry SDO_GEOMETRY;
	geom_trg 	SDO_GEOMETRY;
	geom_src 	SDO_GEOMETRY;
	vSQLStmt VARCHAR2(4000);
BEGIN
		--***Get primary Geo Graphic component table fro source and target feature ***
		vG3eTable:=GetGraphicCompG3eTable(pG3eFno_trg);
		vG3eTable1:=GetGraphicCompG3eTable(pG3eFno_src);

		--***Get Geometries fro given feature instance ***
		vSQLStmt:= 'select g3e_geometry from '||vG3eTable||' where g3e_fid='||pG3eFid_trg;
		Execute immediate vSQLStmt into geom_trg;
		vSQLStmt:='select g3e_geometry from '||vG3eTable1||' where g3e_fid='||pG3eFid_src;
		Execute immediate vSQLStmt into geom_src;

		---Merge Source Feature and Traget feature instance geometry
		sdoGeometry:=SDO_GEOM.SDO_UNION (geom_trg,geom_src, 0.005);

		--***updated Merge Geometry to Traget Feature instance component table***
		vSQLStmt:='update '||vG3eTable||' set G3E_GEOMETRY=:1 where g3e_fid=:2';
		Execute immediate vSQLStmt using sdoGeometry,pG3eFid_trg;

		---**Delete Source Feature instance from component table ****
		DeleteFeatureInstance(pG3eFno_src,pG3eFid_src);
	Exception when others then
		gErrorMsg := vMethodName||'-'||SQLERRM;
		RAISE_APPLICATION_ERROR (-20007, gErrorMsg);
END MergeLandbaseBoundaries;

------------------------------------------------------------------------------------------------------------
   -- Procedure PurgeExpiredArchivedLandbase
   -- this procedure purge Expired  Archived Landbase feature instance from component table
-----------------------------------------------------------------------------------------------------------
PROCEDURE PurgeExpiredArchivedLandbase
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.PurgeExpiredArchivedLandbase';
	nArchiveExpireDays	NUMBER(5):=0;
	vSQLStmt	VARCHAR2(4000);
	TYPE TypeFeature IS RECORD (g3eFno NUMBER,g3eFid NUMBER);
	TYPE TypeFeatures IS Table OF TypeFeature;
	purgeFeatures TypeFeatures;
BEGIN

	---***Get Archive Landbase Expire Days from SYS_GENERALPARAMETER ****
	Select to_number(PARAM_VALUE) into nArchiveExpireDays from SYS_GENERALPARAMETER where SUBSYSTEM_NAME='LANDBASE' AND SUBSYSTEM_COMPONENT='LBM_UTL.PurgeExpiredArchivedLandbase' AND PARAM_NAME='ArchivedLandbaseExpireDays';
	
	
	vSQLStmt:='SELECT G3E_FNO,G3E_FID  FROM LAND_AUDIT_N WHERE STAGE=''Archived'' AND LAST_EDITED_DATE <=SYSDATE-'||nArchiveExpireDays||' order by G3E_FNO';
	Execute immediate vSQLStmt bulk collect into purgeFeatures;
	
	FOR indx in 1..purgeFeatures.count
    LOOP
		BEGIN
			DBMS_OUTPUT.PUT_LINE('Deleting fid '||purgeFeatures(indx).g3eFid);
			----***Delete Expired Landbase Feature instance from component table ***
			DeleteFeatureInstance(purgeFeatures(indx).g3eFno,purgeFeatures(indx).g3eFid);
		Exception when others then
			DBMS_OUTPUT.PUT_LINE('Error while Deleting fid '||purgeFeatures(indx).g3eFid||'from component table '||sqlerrm);
			Raise;
		END;
    END LOOP;
	
	Exception
	WHEN NO_DATA_FOUND then
		RAISE_APPLICATION_ERROR (-20001,'Parameter ArchivedLandbaseExpireDays missing in SYS_GENERALPARAMETER Table');
	when others then
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20002, gErrorMsg);
END PurgeExpiredArchivedLandbase;



------------------------------------------------------------------------------------------------------------
   -- FUNCTION  FeaturesAreValidForMerge
   -- check source and Traget features are trouching and overalpping each other 
   -- Return True if features are touching and overalpping otherwise throws the message
 -----------------------------------------------------------------------------------------------------------
FUNCTION  FeaturesAreValidForMerge(pG3eFno_trg in NUMBER,pG3eFid_trg in NUMBER,pG3eFno_src in NUMBER,pG3eFid_src in NUMBER) RETURN VARCHAR2
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.FeaturesAreValidForMerge';
	vMergeStatus VARCHAR2(200);
	iCnt	NUMBER(3):=0;
	vSQLStmt VARCHAR2(4000);
	
BEGIN
	vSQLStmt:='SELECT count(a.G3E_FID) FROM '||GetGraphicCompG3eTable(pG3eFno_trg)||' a,'||GetGraphicCompG3eTable(pG3eFno_src)||' b
	WHERE a.G3E_FID ='||pG3eFid_trg||' AND b.G3E_FID ='||pG3eFid_src ||'AND SDO_RELATE (a.G3E_GEOMETRY, b.G3E_GEOMETRY, ''mask=touch+overlapbdyintersect'') = ''TRUE''';
	Execute immediate  vSQLStmt into icnt;
	IF( iCnt>0 ) THEN
		vMergeStatus:='TRUE';
	ELSE
		vMergeStatus:='The selected landbase boundaries cannot be merged because their geometries do not overlap one another or share a common edge';
	END IF;
	RETURN vMergeStatus;
	Exception when others then
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20003, gErrorMsg);
END FeaturesAreValidForMerge;


------------------------------------------------------------------------------------------------------------
   -- FUNCTION GetGraphicCompG3eTable
   -- Return Primary Geo graphic component table for given Fno
 -----------------------------------------------------------------------------------------------------------
FUNCTION GetGraphicCompG3eTable(pG3eFno in NUMBER) RETURN VARCHAR2
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.GetGraphicCompG3eTable';
	vG3eTable VARCHAR2(30);

BEGIN
	Select g3e_table into vG3eTable from g3e_feature f,g3e_component c where f.g3e_primaryGeographiccno=c.g3e_cno and f.g3e_fno=pG3eFno;
	return vG3eTable;
	Exception when others then
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20007, gErrorMsg);
End GetGraphicCompG3eTable;

------------------------------------------------------------------------------------------------------------
   -- PROCEDURE DeleteFeatureInstance
   -- Delete feature instance from component table for given fno and fid
 -----------------------------------------------------------------------------------------------------------
PROCEDURE DeleteFeatureInstance(pG3eFno in NUMBER,pG3eFid in NUMBER)
is
vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.DeleteFeatureInstance';
BEGIN
FOR  rec in (Select c.g3e_table,c.g3e_cno from g3e_featurecomponent fc,g3e_component c where fc.g3e_cno=c.g3e_cno and fc.g3e_fno=pG3eFno order by fc.g3e_deleteordinal)
LOOP
	BEGIN
		Execute immediate 'Delete from '||rec.g3e_table||' where g3e_fid=:1' using pG3eFid;
	Exception when NO_DATA_FOUND then
		null;
	when others then
		RAISE_APPLICATION_ERROR (-20007,'Error while Deleting Fid '||pG3eFid|| ' from Component Table '||rec.g3e_table||' - G3E_CNO '||rec.g3e_cno);
	END;
END LOOP;
Exception when others then
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20007, gErrorMsg);
END DeleteFeatureInstance;

------------------------------------------------------------------------------------------------------------
   -- Procedure CREATESDOMETADATA
   -- Setup the SDO metadata required for spatial index
   -- Add entry into user_sdo_geom_metadata for given component table
-----------------------------------------------------------------------------------------------------------
PROCEDURE CREATESDOMETADATA (pTable in varchar2)
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.CREATESDOMETADATA';
    nCount pls_integer;
    vTable user_tab_columns.table_name%type;
BEGIN
    vTable := upper(pTable);
    select count(*) into nCount from user_tab_columns where column_name = 'G3E_GEOMETRY' and table_name = vTable;
    IF nCount > 0 THEN
        delete from user_sdo_geom_metadata where table_name = vTable;
        insert into user_sdo_geom_metadata
             values (vTable, 'G3E_GEOMETRY', mdsys.sdo_dim_array(mdsys.sdo_dim_element('X',815817.6871,5138271.5949, 0.005),
                                                                 mdsys.sdo_dim_element('Y',1123580.3580,4997943.5036, 0.005),
																 mdsys.sdo_dim_element('Z',0,0, 0.005)),3666);
    ELSE
        RAISE_APPLICATION_ERROR (-20004,'Table '||vTable||' does not exist');
    END IF;
Exception when others then
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20005, gErrorMsg);
END CREATESDOMETADATA;


------------------------------------------------------------------------------------------------------------
   -- PROCEDURE DETECTOVERLAPPINGPOLYGONS
   -- Procedure to Find the Boundary polygons that are Overlapped and store the results in Landbase Analysis table.
 -----------------------------------------------------------------------------------------------------------
PROCEDURE DetectOverlappingPolygons(pSrcFno NUMBER,pSelfOverlap BOOLEAN default FALSE)
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.DetectOverlappingPolygons';
BEGIN
	if(pSelfOverlap=TRUE) then 
		DetectOverlappingPolygons(pSrcFno,pSrcFno);
	ELSE
		For rec in (SELECT DISTINCT SOURCE_FNO,MAPPING_FNO FROM LBM_FEATURE WHERE LBM_INTERFACE ='DETECTOVERLAPPINGPOLYGONS' AND SOURCE_FNO=pSrcFno)
		Loop
			DetectOverlappingPolygons(rec.SOURCE_FNO,rec.MAPPING_FNO);
		END LOOP;
	END IF;	

EXCEPTION
	WHEN NO_DATA_FOUND THEN 
		RAISE_APPLICATION_ERROR (-20006, SQLERRM);
	WHEN OTHERS THEN
		RAISE_APPLICATION_ERROR (-20007, SQLERRM);
END DetectOverlappingPolygons;
	


------------------------------------------------------------------------------------------------------------
   -- PROCEDURE DETECTOVERLAPPINGPOLYGONS
   -- Procedure to Find the Boundary polygons that are Overlapped and store the results in Landbase Analysis table.
 -----------------------------------------------------------------------------------------------------------
PROCEDURE DetectOverlappingPolygons(pSrcFno NUMBER,pTrgFno NUMBER)
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.DETECTOVERLAPPINGPOLYGONS';
	
	vSrcTable  G3E_COMPONENT.G3E_TABLE%Type;
	nSrcCno  G3E_COMPONENT.G3E_CNO%Type;
	
	vTrgTable G3E_COMPONENT.G3E_TABLE%Type;
	nTrgCno  G3E_COMPONENT.G3E_CNO%Type;
	
	vSQLStmt VARCHAR2(4000);
	
	TYPE OverlappingFid IS RECORD ( srcFid NUMBER(10),trgFid NUMBER(10));
	TYPE typeOverlappingFid IS TABLE OF OverlappingFid;
	tOverlappingFids typeOverlappingFid;

BEGIN
	
	Execute Immediate 'SELECT distinct C.G3E_CNO,C.G3E_TABLE FROM LBM_FEATURE LB,G3E_FEATURE F,G3E_COMPONENT C WHERE LB.SOURCE_FNO=:1 AND LB.SOURCE_FNO=F.G3E_FNO AND F.G3E_PRIMARYGEOGRAPHICCNO=C.G3E_CNO and LBM_INTERFACE =''DETECTOVERLAPPINGPOLYGONS'''  into nSrcCno,vSrcTable using pSrcFno;
	
	IF(pSrcFno=pTrgFno) THEN
		vTrgTable:=vSrcTable;
		nTrgCno:=nSrcCno;
	ELSE
		Execute Immediate 'SELECT distinct C.G3E_CNO,C.G3E_TABLE FROM LBM_FEATURE LB,G3E_FEATURE F,G3E_COMPONENT C WHERE LB.MAPPING_FNO=:1 AND LB.MAPPING_FNO=F.G3E_FNO AND F.G3E_PRIMARYGEOGRAPHICCNO=C.G3E_CNO and LBM_INTERFACE =''DETECTOVERLAPPINGPOLYGONS''' into nTrgCno,vTrgTable using pTrgFno;
	END IF;

	----***Delete existing analysis results from lbm_analysisresult table for given source fno***
	Execute Immediate 'DELETE FROM LBM_ANALYSISRESULT WHERE G3E_FNO=:1 AND G3E_FNO2=:2 AND LBM_INTERFACE =''DETECTOVERLAPPINGPOLYGONS'' and created_by=user' using pSrcFno,pTrgFno;
	
	---***Query to find Overlapped Poylgons****
	 vSQLStmt:='SELECT distinct a.G3E_FID as srcFid ,b.g3e_fid as trgFid FROM '||vSrcTable||' a, '||vTrgTable||' b,LAND_AUDIT_N a1,LAND_AUDIT_N b1 WHERE a.G3E_FID <> b.G3E_FID AND A1.G3E_FNO='||pSrcFno||' AND A.G3E_CNO='||nSrcCno||' AND B1.G3E_FNO='||pTrgFno||' AND B.G3E_CNO='||nTrgCno||' and a.g3e_fid=a1.g3e_fid and b.g3e_fid=b1.g3e_fid and a1.stage=''Accepted'' and b1.stage=''Accepted'' AND SDO_RELATE (B.G3E_GEOMETRY, A.G3E_GEOMETRY, ''mask=equal+overlapbdyintersect+overlapbdydisjoint'') = ''TRUE'' order by a.g3e_fid';
	
	EXECUTE IMMEDIATE vSQLStmt  BULK COLLECT INTO tOverlappingFids;
	---**Log into LBM_ANALYSISRESULT table for all detected overalpping polygons****
	FOR indx in 1..tOverlappingFids.count
    LOOP
		Execute IMMEDIATE 'INSERT INTO LBM_ANALYSISRESULT (ID,G3E_FNO,G3E_FNO2,LBM_INTERFACE,G3E_FID,G3E_FID2) VALUES((SELECT NVL(MAX(ID),0)+1 FROM LBM_ANALYSISRESULT),:srcFno,:trgFno,:iName,:srcfid,:trgFid)' using pSrcFno,pTrgFno,'DETECTOVERLAPPINGPOLYGONS',tOverlappingFids(indx).srcFid,tOverlappingFids(indx).trgFid;
    END LOOP;
	---****Update Stage to pending for all detected Polygons***
	For rec in (SELECT DISTINCT G3E_FID FROM LBM_ANALYSISRESULT WHERE LBM_INTERFACE ='DETECTOVERLAPPINGPOLYGONS' and g3e_fno=pSrcFno and G3E_FNO2=pTrgFno and created_by=user)
	LOOP
		Execute IMMEDIATE 'Update LAND_AUDIT_N set stage=:stage where g3e_fid=:g3eFid' using 'Pending',rec.G3E_FID;
	END Loop;
	
EXCEPTION
	WHEN NO_DATA_FOUND THEN 
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20008, gErrorMsg);
	WHEN OTHERS THEN
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20009, gErrorMsg);
END DetectOverlappingPolygons;

------------------------------------------------------------------------------------------------------------
   -- PROCEDURE DetectPolygonEdgeMismatch
   --Detect Polygon(s) having Edge Mistmatch with Neighbouring Polygon(s) and detected Fids will be persist in Landbase Analysis table.
 -----------------------------------------------------------------------------------------------------------
PROCEDURE DetectPolygonEdgeMismatch
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.DetectPolygonEdgeMismatch';
BEGIN
	For rec in (SELECT DISTINCT SOURCE_FNO FROM LBM_FEATURE WHERE LBM_INTERFACE ='DetectPolygonEdgeMismatch')
	Loop
		DetectPolygonEdgeMismatch(rec.SOURCE_FNO);
	END LOOP;
EXCEPTION 
	WHEN OTHERS THEN
		RAISE_APPLICATION_ERROR (-20010, SQLERRM);
END DetectPolygonEdgeMismatch;

------------------------------------------------------------------------------------------------------------
   -- PROCEDURE DetectPolygonEdgeMismatch
   -- For Input Source Feature Detect Polygon(s) having Edge Mistmatch with Neighbouring Polygon(s) and detected Fids will be persist in Landbase Analysis table.
 -----------------------------------------------------------------------------------------------------------
PROCEDURE DetectPolygonEdgeMismatch(pSrcFno NUMBER)
IS
	vMethodName  CONSTANT VARCHAR2(60) := cPackage || '.DetectPolygonEdgeMismatch';
	vInterfaceName varchar2(50):='DetectPolygonEdgeMismatch';

	vSrcTable  G3E_COMPONENT.G3E_TABLE%Type;
	vTrgTable G3E_COMPONENT.G3E_TABLE%Type;
	vSQLStmt varchar2(4000);

	TYPE EdgeMisMatchFid IS RECORD ( srcfno NUMBER(5),srcFid NUMBER(10),trgFno NUMBER(5),trgFid NUMBER(10),distance Number(10));
	TYPE typeEdgeMisMatchFid IS TABLE OF EdgeMisMatchFid;
	tDetectedFids typeEdgeMisMatchFid;
	
	nCnt	NUMBER:=0;
	nVertex	Number:=0;
	srcGeom SDO_GEOMETRY;
	nPrevFid	NUMBER:=0;
	nSnappingDistance NUMBER:=0;
	vTrgGeomIndx varchar2(100);
BEGIN
---***Get Snapping distance parameter Value from SYS_GENERALPARAMETER***
	Select to_number(PARAM_VALUE) into nSnappingDistance from SYS_GENERALPARAMETER where SUBSYSTEM_NAME='LANDBASE' AND SUBSYSTEM_COMPONENT='LBM_UTL.DetectPolygonEdgeMismatch' AND PARAM_NAME='BoundarySnappingDistance';
	
	FOR rec in (select source_fno as SrcFno,mapping_fno as TrgFno
				,(select g3e_table from g3e_component c,g3e_feature where g3e_fno=source_fno and g3e_primaryGeographiccno=g3e_cno) as SrcG3eTable
				,(select g3e_table from g3e_component c,g3e_feature where g3e_fno=mapping_fno and g3e_primaryGeographiccno=g3e_cno) as TrgG3eTable
			from LBM_FEATURE WHERE LBM_INTERFACE=vInterfaceName and source_fno=pSrcFno)
	LOOP
	BEGIN
		
		vSrcTable:=rec.SrcG3eTable;
		vTrgTable:=rec.TrgG3eTable;
			
		select Index_Name into vTrgGeomIndx from user_indexes where table_name='B$'||rec.TrgG3eTable AND ITYP_NAME='SPATIAL_INDEX';
		
		----***Delete Detected Polygon Fids from lbm_analysisresult table for given source fno***
		vSQLStmt:='DELETE FROM LBM_ANALYSISRESULT WHERE G3E_FNO=:1 AND G3E_FNO2=:2 AND LBM_INTERFACE =''DetectPolygonEdgeMismatch'' and created_by=user';
		Execute Immediate vSQLStmt using rec.SrcFno,rec.TrgFno;
		---**Query to find all Neighbouring polygons with in Snapping Distance****
		vSQLStmt:='SELECT /*+ LEADING(a) USE_NL(b a) INDEX(b '||vTrgGeomIndx||') */ distinct A.G3E_FNO AS srcFno,a.G3E_FID as srcFid,b.g3e_fno as trgFno,b.g3e_fid as trgFid,sdo_nn_distance (1) as dist FROM '||vSrcTable||' a, '||vTrgTable||' b,LAND_AUDIT_N a1,LAND_AUDIT_N b1 WHERE a.G3E_FID <> b.G3E_FID AND A1.G3E_FNO='||rec.SrcFno||' AND B1.G3E_FNO='||rec.TrgFno||' and a.g3e_fid=a1.g3e_fid and b.g3e_fid=b1.g3e_fid and a1.stage=''Accepted'' and b1.stage=''Accepted'' AND SDO_NN(b.G3E_GEOMETRY, a.G3E_GEOMETRY,''sdo_num_res=6'',1) = ''TRUE'' and sdo_nn_distance(1)<='||nSnappingDistance||' order by a.g3e_fid,sdo_nn_distance(1)';
			
		EXECUTE IMMEDIATE vSQLStmt  BULK COLLECT INTO tDetectedFids;
		nPrevFid:=0;
		FOR indx in 1..tDetectedFids.count
		LOOP
			---For Each Polyon (Fid) will have adjacent Polygons and query returns multiple records for each hence cache Polygon Fid and Geometry***
			IF(nPrevFid<>tDetectedFids(indx).srcFid) THEN
				vSQLStmt:='Select g3e_geometry from '||vSrcTable||' where g3e_fid=:1';
				Execute Immediate vSQLStmt into srcGeom using tDetectedFids(indx).srcFid;
				nPrevFid:=tDetectedFids(indx).srcFid;
				nVertex:=sdo_util.GetNumVertices(srcGeom);
				vSQLStmt:='Truncate table LBM_SDOGeomTemp';
				Execute IMMEDIATE vSQLStmt;
				FOR vtx in 0..nVertex-1
				LOOP
					EXECUTE IMMEDIATE 'insert into LBM_SDOGeomTemp(G3eFid,Geometry) values (:1,:2)' USING tDetectedFids(indx).srcFid,GetVertex(srcGeom,vtx);
				END Loop;
			END IF;
			---***Log into LBM_ANALYSISRESULT table if snapping distance between two polygons >0 i.e two polygons are not touching****
			IF(tDetectedFids(indx).distance>0) THEN
				----**Log into LBM_ANALYSISRESULT table if distance>0 i.e gap between polygons exists****
				INSERT INTO LBM_ANALYSISRESULT (ID,G3E_FNO,G3E_FNO2,LBM_INTERFACE,G3E_FID,G3E_FID2) VALUES((SELECT NVL(MAX(ID),0)+1 FROM LBM_ANALYSISRESULT),tDetectedFids(indx).srcFno,tDetectedFids(indx).trgFno,'DetectPolygonEdgeMismatch',tDetectedFids(indx).srcFid,tDetectedFids(indx).trgFid);
			ELSE
				---***Check polygons touching with adjacent polygons****
				vSQLStmt:='SELECT count(1) as cnt FROM '||vSrcTable||' a, '||vTrgTable||' b WHERE a.G3E_FID <> b.G3E_FID AND a.G3E_FID=:1 AND B.G3E_FID=:2 AND SDO_RELATE (B.G3E_GEOMETRY, A.G3E_GEOMETRY, ''mask=touch'') = ''TRUE''';

				Execute Immediate vSQLStmt into nCnt using tDetectedFids(indx).srcFid,tDetectedFids(indx).trgFid;
				---***If Polygons are touching then above Query returns 1 else 0****
				IF(nCnt=1) then				
					---**check Polygon having Edge Mismatch with the polygon touching*****
					vSQLStmt:='select /*+ INDEX(c '||vTrgGeomIndx||')*/ count(1) from '||vTrgTable||' c,LBM_SDOGeomTemp lc where SDO_NN(c.G3E_GEOMETRY,lc.Geometry,''sdo_num_res='||nVertex||''',1) = ''TRUE'' AND sdo_nn_distance(1)>0 AND sdo_nn_distance(1)<='||nSnappingDistance||' and g3e_fid='||tDetectedFids(indx).trgFid;
					EXECUTE immediate vSQLStmt into nCnt;
					----**Log into LBM_ANALYSISRESULT table if detected Edge Mismatch****
					IF(ncnt>0) THEN
						INSERT INTO LBM_ANALYSISRESULT (ID,G3E_FNO,G3E_FNO2,LBM_INTERFACE,G3E_FID,G3E_FID2) VALUES((SELECT NVL(MAX(ID),0)+1 FROM LBM_ANALYSISRESULT),tDetectedFids(indx).srcFno,tDetectedFids(indx).trgFno,'DetectPolygonEdgeMismatch',tDetectedFids(indx).srcFid,tDetectedFids(indx).trgFid);
					End if;
				END IF;
			END IF;
		END LOOP;
		
	EXCEPTION when others then
		RAISE_APPLICATION_ERROR (-20013, SQLERRM);
	END;
	END LOOP;
	---****Update Stage to pending for all detected Polygons***

	For rec in (SELECT DISTINCT G3E_FID FROM LBM_ANALYSISRESULT WHERE LBM_INTERFACE =vInterfaceName and g3e_fno=pSrcFno and created_by=user)
	LOOP
		Execute IMMEDIATE 'Update LAND_AUDIT_N set stage=:stage where g3e_fid=:g3eFid' using 'Pending',rec.G3E_FID;
	END Loop;
	
EXCEPTION
	WHEN NO_DATA_FOUND THEN 
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20011, gErrorMsg);
	WHEN OTHERS THEN
		gErrorMsg := vMethodName||SQLERRM;
		RAISE_APPLICATION_ERROR (-20012, gErrorMsg);
END DetectPolygonEdgeMismatch;

-----------------------------------------------------------------------------
     -- FUNCTION GetVertexPoint
     -- convert SDO point to Sdo_geometry at specified vertex Number and returns sdo_geometry
-----------------------------------------------------------------------------   
FUNCTION GetVertex(pGeometry IN SDO_GEOMETRY,pVertexNum IN NUMBER) RETURN SDO_GEOMETRY
  IS
    nDims NUMBER:=0;
    point SDO_POINT_TYPE:=SDO_POINT_TYPE(0,0,0);
  BEGIN
    nDims:=pGeometry.get_dims();
    point.X:=pGEOMETRY.sdo_ordinates(pVertexNum*nDims +1);
    point.Y:=pGEOMETRY.sdo_ordinates(pVertexNum*nDims +2);
    point.Z:=pGEOMETRY.sdo_ordinates(pVertexNum*nDims +3);
    RETURN SDO_GEOMETRY(3001, 3666, NULL,
                        SDO_ELEM_INFO_ARRAY( 1,1,1, 4,1,0),
                        SDO_ORDINATE_ARRAY( point.X, point.Y, point.Z)); 
    
  END GetVertex;

End LBM_UTL;
/
show errors

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2649);
