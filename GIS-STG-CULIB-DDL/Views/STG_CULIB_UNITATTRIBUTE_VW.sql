CREATE OR REPLACE VIEW GIS_STG.STG_CULIB_UNITATTRIBUTE_VW AS
(
SELECT
sca.STG_CULIB_STANDARD_ATTR_ID,
scv.STG_CULIB_VALID_ATTR_VAL_ID,
sca.PROCESS_RUN_ID AS PROCESS_RUN_ID,
sca.PROCESS_DT AS PROCESS_DT,
sca.CU_ID AS CU_ID,
sca.CU_CATEGORY_CODE AS CATEGORY_C,
sca.CU_ATTRIBUTE_CODE AS ATTRIBUTE_ID,
scv.CU_VALID_VAL AS ATTR_KEY,
scv.CU_VALID_VAL AS ATTR_VALUE,
sca.AUD_CREATE_USR_ID AS AUD_CREATE_USR_ID,
sca.AUD_MOD_USR_ID AS AUD_MOD_USR_ID,
sca.AUD_CREATE_DT AS AUD_CREATE_DT,
sca.AUD_MOD_DT AS AUD_MOD_DT
FROM GIS_STG.STG_CULIB_STANDARD_ATTR sca, GIS_STG.STG_CULIB_VALID_ATTR_VAL scv
WHERE 
sca.PROCESS_RUN_ID = scv.PROCESS_RUN_ID AND
sca.PROCESS_DT = scv.PROCESS_DT AND
sca.CU_VALID_ATTR_ID = scv.CU_VALID_ATTR_ID 
)
;

