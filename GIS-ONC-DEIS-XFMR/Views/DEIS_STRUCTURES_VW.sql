CREATE OR REPLACE FORCE EDITIONABLE VIEW "GIS_ONC"."DEIS_STRUCTURES_VW" ("FEATURE","G3E_FNO","G3E_CNO","G3E_FID","G3E_CID","STRUCTURE_ID","CHANGE_OPERATION","CHANGE_DATE")
AS
WITH STRUCTURE_FNO AS
		( SELECT DISTINCT O.G3E_OWNERFNO G3E_FNO
			,F.G3E_USERNAME FEATURE
			FROM GIS.G3E_OWNERSHIP O
			INNER JOIN GIS.G3E_FEATURE F ON O.G3E_OWNERFNO = F.G3E_FNO
			WHERE O.G3E_SOURCEFNO IN ( 59,60,34 )
		)
	,STRUCTURES AS
		(SELECT  S.FEATURE
			,C.G3E_FNO
			,C.G3E_FID
			,C.G3E_CNO
			,C.G3E_CID
			,C.STRUCTURE_ID
			,'CREATE' CHANGE_OPERATION
			,TO_DATE('01/01/2000','dd/MM/yyyy') CHANGE_DATE
			FROM COMMON_N C
			INNER JOIN STRUCTURE_FNO S   ON S.G3E_FNO = C.G3E_FNO
			INNER JOIN GIS.G3E_FEATURE F ON C.G3E_FNO = F.G3E_FNO
			WHERE 1 = 1
		)
	,HIST AS
		( SELECT DISTINCT S.FEATURE
			,AH.G3E_FNO
			,AH.G3E_FID
			,AH.G3E_CNO
			,AH.G3E_CID
			,FIRST_VALUE(AH.CHANGE_OPERATION) OVER (PARTITION BY AH.G3E_FNO,AH.G3E_FID,AH.G3E_CID ORDER BY AH.CHANGE_DATE DESC) CHANGE_OPERATION
			,FIRST_VALUE(AH.CHANGE_DATE) OVER (PARTITION BY AH.G3E_FNO,AH.G3E_FID,AH.G3E_CID ORDER BY AH.CHANGE_DATE DESC) CHANGE_DATE
				--,MAX(CHANGE_DATE) MAX_CHANGE_DATE
			FROM ASSET_HISTORY AH
			INNER JOIN STRUCTURE_FNO S ON AH.G3E_FNO = S.G3E_FNO
			WHERE 1 = 1
				AND AH.G3E_CNO = 1
		)
	SELECT  COALESCE(H.FEATURE,S.FEATURE) FEATURE
		,COALESCE(H.G3E_FNO,S.G3E_FNO) G3E_FNO
		,COALESCE(H.G3E_CNO,S.G3E_CNO) G3E_CNO
		,COALESCE(H.G3E_FID,S.G3E_FID) G3E_FID
		,COALESCE(H.G3E_CID,S.G3E_CID) G3E_CID
		,S.STRUCTURE_ID
		,COALESCE(H.CHANGE_OPERATION,S.CHANGE_OPERATION) CHANGE_OPERATION
		,COALESCE(H.CHANGE_DATE,S.CHANGE_DATE) CHANGE_DATE
		FROM STRUCTURES S
		FULL OUTER JOIN HIST H ON S.G3E_FNO = H.G3E_FNO AND S.G3E_CNO = H.G3E_CNO AND S.G3E_FID = H.G3E_FID AND S.G3E_CID = H.G3E_CID
		WHERE
			(
				S.G3E_FID IS NOT NULL
				OR H.CHANGE_OPERATION = 'DELETE'
			);
			
GRANT SELECT ON GIS.ASSET_HISTORY TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT ON GIS.G3E_OWNERSHIP TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT ON GIS_ONC.DEIS_STRUCTURES_VW TO GIS_INFA;
GRANT SELECT ON GIS_ONC.DEIS_TRANSFORMER_PHASE_VW TO GIS_INFA;
/