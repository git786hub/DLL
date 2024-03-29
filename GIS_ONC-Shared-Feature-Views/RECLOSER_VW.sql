
  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."RECLOSER_VW" ("OGGY_H", "OGGX_H", "FEATURE_STATE_C", "G3E_FNO", "OWNER1_ID", "LONGITUDE", "G3E_FID", "LOCATION", "LATITUDE", "STRUCTURE_ID", "STATUS_NORMAL_C", "FEEDER_1_ID", "PROTECTIVE_DEVICE_FID", "STATUS_OPERATED_C", "SSTA_C", "VOLT_1_Q", "DEVICE_ID", "ENABLED_YN", "VINTAGE_YR", "TYPE_C", "G3E_GEOMETRY") AS 
  SELECT CN.OGGY_H,
  CN.OGGX_H,
  CN.FEATURE_STATE_C,
  CN.G3E_FNO,
  CN.OWNER1_ID,
  CN.LONGITUDE,
  CN.G3E_FID,
  CN.LOCATION,
  CN.LATITUDE,
  CN.STRUCTURE_ID,
  CTN.STATUS_NORMAL_C,
  CTN.FEEDER_1_ID,
  CTN.PROTECTIVE_DEVICE_FID,
  CTN.STATUS_OPERATED_C,
  CTN.SSTA_C,
  CTN.VOLT_1_Q,
  SN.DEVICE_ID,
  SN.ENABLED_YN,
  CUN.VINTAGE_YR,
  RBN.TYPE_C,
  RCS.G3E_GEOMETRY
FROM GIS.COMMON_N CN
INNER JOIN GIS.RECLOSER_UNIT_N RBN
ON CN.G3E_FID = RBN.G3E_FID
INNER JOIN GIS.RECLOSER_S RCS
ON CN.G3E_FID = RCS.G3E_FID
INNER JOIN GIS.COMP_UNIT_N CUN
ON CN.G3E_FID = CUN.G3E_FID
INNER JOIN GIS.CONNECTIVITY_N CTN
ON CN.G3E_FID = CTN.G3E_FID
LEFT JOIN GIS.SCADA_N SN
ON CN.G3E_FID = SN.G3E_FID;
