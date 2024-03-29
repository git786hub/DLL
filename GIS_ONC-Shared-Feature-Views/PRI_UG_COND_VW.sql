  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."PRI_UG_COND_VW" ("OWNER1_ID", "OWNER2_ID", "FEATURE_STATE_C", "G3E_FNO", "G3E_FID", "G3E_CNO", "STRUCTURE_ID", "STATUS_NORMAL_C", "PROTECTIVE_DEVICE_FID", "STATUS_OPERATED_C", "NETWORK_ID", "FEEDER_1_ID", "SSTA_C", "VOLT_1_Q", "VOLT_2_Q", "LENGTH_ACTUAL_Q", "WIRE_Q", "G3E_GEOMETRY", "VINTAGE_YR") AS 
  SELECT CN.OWNER1_ID,
  CN.OWNER2_ID,
  CN.FEATURE_STATE_C,
  CN.G3E_FNO,
  CN.G3E_FID,
  PCUN.G3E_CNO,
  CN.STRUCTURE_ID,
  CTN.STATUS_NORMAL_C,
  CTN.PROTECTIVE_DEVICE_FID,
  CTN.STATUS_OPERATED_C,
  CTN.NETWORK_ID,
  CTN.FEEDER_1_ID,
  CTN.SSTA_C,
  CTN.VOLT_1_Q,
  CTN.VOLT_2_Q,
  CN.LENGTH_ACTUAL_Q,
  PCUN.WIRE_Q,
  PCU.G3E_GEOMETRY,
  CUN.VINTAGE_YR
FROM GIS.PRI_COND_UG_N PCUN
INNER JOIN GIS.COMMON_N CN 
ON PCUN.G3E_FID = CN.G3E_FID
INNER JOIN GIS.PRI_COND_UG_L PCU
ON PCUN.G3E_FID = PCU.G3E_FID
LEFT JOIN GIS.COMP_UNIT_N CUN
ON PCUN.G3E_FID = CUN.G3E_FID
INNER JOIN GIS.CONNECTIVITY_N CTN
ON PCUN.G3E_FID = CTN.G3E_FID;