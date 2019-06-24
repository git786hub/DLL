  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."UG_TRANSFORMER_VW" ( "OGGY_H", "OGGX_H", "OWNER1_ID", "G3E_FNO", "FEATURE_STATE_C", "G3E_FID", "LOCATION", "STRUCTURE_ID",  "FEEDER_1_ID", "SSTA_C", "PROTECTIVE_DEVICE_FID", "STATUS_OPERATED_C", "STATUS_NORMAL_C", "NETWORK_ID", "G3E_GEOMETRY", "PHASE_Q", "KVA_Q", "COMPANY_ID","VOLT_PRI_Q", "VOLT_SEC_Q") AS 
  SELECT 
  CN.OGGY_H,
  CN.OGGX_H,
  CN.OWNER1_ID,
  CN.G3E_FNO,
  CN.FEATURE_STATE_C,
  CN.G3E_FID,
  CN.LOCATION,
  CN.STRUCTURE_ID,
  CTN.FEEDER_1_ID,
  CTN.SSTA_C,
  CTN.PROTECTIVE_DEVICE_FID,
  CTN.STATUS_OPERATED_C,
  CTN.STATUS_NORMAL_C,
  CTN.NETWORK_ID, 
  XUS.G3E_GEOMETRY,
  XUN.PHASE_Q,
  XUN.KVA_Q,
  XUN.COMPANY_ID,
  XUN.VOLT_PRI_Q,
  XUN.VOLT_SEC_Q  
FROM GIS.COMMON_N CN
INNER JOIN GIS.CONNECTIVITY_N CTN
ON CN.G3E_FID = CTN.G3E_FID
INNER JOIN GIS.XFMR_UG_UNIT_N XUN
ON CN.G3E_FID = XUN.G3E_FID
INNER JOIN GIS.XFMR_UG_S XUS
ON CN.G3E_FID = XUS.G3E_FID
;