  CREATE OR REPLACE VIEW "GIS_ONC"."OBERBNDY_COND_STRC_VW" ("G3E_FID", "G3E_FNO") AS 
  SELECT G3E_FID,
  G3E_FNO
FROM GIS.COMMON_N CN
WHERE CN.G3E_FNO IN (110, 107, 2500, 117, 106,108) --structures that conductors are on, there will be more / different in the future
;