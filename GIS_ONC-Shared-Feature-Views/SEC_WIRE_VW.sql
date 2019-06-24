  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."SEC_WIRE_VW" ("G3E_FID", "G3E_FNO", "G3E_CNO", "MATERIAL_C", "SIZE_C", "G3E_GEOMETRY") AS
  SELECT
          CN.G3E_FID,
          CN.G3E_FNO,
          PCUN.G3E_CNO,
          PCUN.MATERIAL_C,
          PWN.SIZE_C,
          PCU.G3E_GEOMETRY
     FROM GIS.COMMON_N CN
          INNER JOIN GIS.SEC_WIRE_N PCUN
             ON CN.G3E_FID = PCUN.G3E_FID
          INNER JOIN GIS.SEC_COND_L PCU
             ON CN.G3E_FID = PCU.G3E_FID
          INNER JOIN GIS.SEC_WIRE_N PWN
             ON CN.G3E_FID = PWN.G3E_FID;
