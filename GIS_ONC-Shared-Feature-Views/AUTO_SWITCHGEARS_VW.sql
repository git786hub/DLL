
  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."AUTO_SWITCHGEARS_VW" ("OGGY_H", "OGGX_H", "G3E_FNO", "FEATURE_STATE_C", "LONGITUDE", "OWNER1_ID", "G3E_FID", "LOCATION", "LATITUDE", "STRUCTURE_ID", "G3E_GEOMETRY", "VINTAGE_YR") AS 
  SELECT CN.OGGY_H,
  CN.OGGX_H,
  CN.G3E_FNO,
  CN.FEATURE_STATE_C,
  CN.LONGITUDE,
  CN.OWNER1_ID,
  CN.G3E_FID,
  CN.LOCATION,
  CN.LATITUDE,
  CN.STRUCTURE_ID,
  PS.G3E_GEOMETRY,
  CUN.VINTAGE_YR
FROM GIS.COMMON_N CN
INNER JOIN GIS.PRISWGEAR_P PS
ON CN.G3E_FID = PS.G3E_FID
INNER JOIN GIS.COMP_UNIT_N CUN
ON CN.G3E_FID = CUN.G3E_FID;
