WITH county_centroid
AS (
	SELECT /*+ ordered */ 'County' as type
    ,tab.NAME
		,sdo_geom.sdo_centroid(spa.G3E_GEOMETRY, meta.diminfo) AS geom
	FROM B$COUNTY_AR spa
	JOIN B$COUNTY_N tab ON tab.G3E_FID = spa.G3E_FID
		,user_sdo_geom_metadata meta
	WHERE meta.table_name = 'B$COUNTY_AR'
	)

--MUNICIPALITY BOUNDARY  
,municipality_centriod
AS
  (
  	SELECT /*+ ordered */ tab.CITY_TYPE TYPE
		,tab.CITY_NAME NAME
    ,sdo_geom.sdo_centroid(spa.G3E_GEOMETRY, meta.diminfo) AS geom
	FROM B$MUNICIPALITY_AR spa
	JOIN B$MUNICIPALITY_N tab ON tab.G3E_FID = spa.G3E_FID
		,user_sdo_geom_metadata meta
	WHERE meta.table_name = 'B$MUNICIPALITY_AR'
  )
  
SELECT ASSO.ACCOUNT_ID,
        c.type
    ,SB.BND_CLASS
	,c.NAME
	,c.geom.sdo_point.x x_coordinates
	,c.geom.sdo_point.y y_cooridnates
FROM county_centroid c
JOIN GIS_ONC.STLT_BOUNDARY SB ON SB.BND_TYPE = c.TYPE
JOIN GIS_ONC.STLT_ACCT_BNDY_ASSOC ASSO ON ASSO.BOUNDARY_CLASS = SB.BND_CLASS
JOIN GIS_ONC.STLT_ACCT_BNDY_ASSOC ASSO ON ASSO.BOUNDARY_ID = c.NAME
UNION ALL
SELECT c.type,
   ASSO.ACCOUNT_ID
    ,SB.BND_CLASS
	,c.NAME
	,c.geom.sdo_point.x x_coordinates
	,c.geom.sdo_point.y y_cooridnates
FROM municipality_centriod c
JOIN GIS_ONC.STLT_BOUNDARY SB ON SB.BND_TYPE = c.TYPE
JOIN GIS_ONC.STLT_ACCT_BNDY_ASSOC ASSO ON ASSO.BOUNDARY_CLASS = SB.BND_CLASS
JOIN GIS_ONC.STLT_ACCT_BNDY_ASSOC ASSO ON ASSO.BOUNDARY_ID = c.NAME;
