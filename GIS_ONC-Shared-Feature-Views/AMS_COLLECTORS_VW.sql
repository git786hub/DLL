
  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."AMS_COLLECTORS_VW" ("G3E_GEOMETRY", "OGGY_H", "OGGX_H", "G3E_FNO", "FEATURE_STATE_C", "LONGITUDE", "OWNER1_ID", "G3E_FID", "LOCATION", "LATITUDE", "STRUCTURE_ID") AS 
  SELECT ACS.G3E_GEOMETRY,
  CN.OGGY_H,
  CN.OGGX_H,
  CN.G3E_FNO,
  CN.FEATURE_STATE_C,
  CN.LONGITUDE,
  CN.OWNER1_ID,
  CN.G3E_FID,
  CN.LOCATION,
  CN.LATITUDE,
  CN.STRUCTURE_ID
FROM GIS.COMMON_N CN
INNER JOIN GIS.AMSCOLLECTOR_S ACS
ON CN.G3E_FID = ACS.G3E_FID;