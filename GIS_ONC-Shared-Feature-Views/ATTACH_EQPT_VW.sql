  CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."ATTACH_EQPT_VW" ("STRUCTURE_ID", "OGGX_H", "OGGY_H", "G3E_FID", "G3E_CNO", "E_ABANDON_D", "E_ATTACHED_D", "E_ATTACHMENT_STATUS", "E_ATTACH_COMPANY", "E_ATTACH_HEIGHT_FT", "E_ATTACH_POSITION_C", "E_ATTACH_SOURCE_C", "E_ATTACH_TYPE_C", "E_AUDIT_D", "E_BRACKET_ARM", "E_MAINTAINER_1", "E_MAINTAINER_2", "E_PERMIT_NUMBER", "E_REMOVED_D", "E_WEIGHT", "G3E_GEOMETRY") AS 
  SELECT
CN.STRUCTURE_ID,
CN.OGGX_H,
CN.OGGY_H,
AEN.G3E_FID,
AEN.G3E_CNO,
AEN.E_ABANDON_D,
AEN.E_ATTACHED_D,
AEN.E_ATTACHMENT_STATUS,
AEN.E_ATTACH_COMPANY,
AEN.E_ATTACH_HEIGHT_FT,
AEN.E_ATTACH_POSITION_C,
AEN.E_ATTACH_SOURCE_C,
AEN.E_ATTACH_TYPE_C,
AEN.E_AUDIT_D,
AEN.E_BRACKET_ARM,
AEN.E_MAINTAINER_1,
AEN.E_MAINTAINER_2,
AEN.E_PERMIT_NUMBER,
AEN.E_REMOVED_D,
AEN.E_WEIGHT,
PL.G3E_GEOMETRY
FROM GIS.ATTACH_EQPT_N AEN
INNER JOIN GIS.COMMON_N CN
ON AEN.G3E_FID = CN.G3E_FID
INNER JOIN POLE_S PL
on AEN.G3E_FID = PL.G3E_FID and AEN.G3E_FNO = PL.G3E_FNO;