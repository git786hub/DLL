SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL GRAPHIC_LENGTH_UPDATE.LOG

ALTER TABLE B$COMMON_N DISABLE ALL TRIGGERS;

UPDATE B$COMMON_N
SET LENGTH_GRAPHIC_Q = 0
WHERE LENGTH_GRAPHIC_Q IS NULL
AND G3E_FNO IN 
(
	8		/*Primary Conductor - OH*/
	,9		/*Primary Conductor - UG*/
	,53		/*Secondary Conductor - OH*/
	,54		/*Service Line*/
	,63		/*Secondary Conductor - UG*/
	,84		/*Primary Conductor - OH Network*/
	,85		/*Primary Conductor - UG Network*/
	,96		/*Secondary Conductor - OH Network*/
	,97		/*Secondary Conductor - UG Network*/
	,104	/*Conduit*/
	,2200	/*Duct Bank*/
)
;

UPDATE B$COMMON_N
SET LENGTH_GRAPHIC_FT = LENGTH_GRAPHIC_Q * 3.281
WHERE LENGTH_GRAPHIC_FT IS NULL
AND G3E_FNO IN 
(
	8		/*Primary Conductor - OH*/
	,9		/*Primary Conductor - UG*/
	,53		/*Secondary Conductor - OH*/
	,54		/*Service Line*/
	,63		/*Secondary Conductor - UG*/
	,84		/*Primary Conductor - OH Network*/
	,85		/*Primary Conductor - UG Network*/
	,96		/*Secondary Conductor - OH Network*/
	,97		/*Secondary Conductor - UG Network*/
	,104	/*Conduit*/
	,2200	/*Duct Bank*/
)
;

COMMIT;

ALTER TABLE B$COMMON_N ENABLE ALL TRIGGERS;

SPOOL OFF;