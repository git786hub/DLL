
  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."SERVICE_LINE_VW" ("OGGY_H", "OGGX_H", "OWNER1_ID", "G3E_FNO", "FEATURE_STATE_C", "LONGITUDE", "G3E_FID", "LOCATION", "LATITUDE", "STRUCTURE_ID", "VINTAGE_YR", "STATUS_OPERATED_C", "FEEDER_1_ID", "TYPE_C", "SIZE_C", "G3E_GEOMETRY") AS 
  SELECT CN.OGGY_H,
  CN.OGGX_H,
  CN.OWNER1_ID,
  CN.G3E_FNO,
  CN.FEATURE_STATE_C,
  CN.LONGITUDE,
  CN.G3E_FID,
  CN.LOCATION,
  CN.LATITUDE,
  CN.STRUCTURE_ID,
  CUN.VINTAGE_YR,
  CTN.STATUS_OPERATED_C,
  CTN.FEEDER_1_ID,
  SLN.TYPE_C,
  SLN.SIZE_C,
  SLL.G3E_GEOMETRY
FROM GIS.SERVICE_LINE_N SLN
INNER JOIN GIS.COMMON_N CN
ON SLN.G3E_FID = CN.G3E_FID
INNER JOIN GIS.SERVICE_LINE_L SLL
ON CN.G3E_FID = SLL.G3E_FID
INNER JOIN GIS.CONNECTIVITY_N CTN
ON CN.G3E_FID = CTN.G3E_FID
LEFT JOIN GIS.COMP_UNIT_N CUN
ON CN.G3E_FID = CUN.G3E_FID;
