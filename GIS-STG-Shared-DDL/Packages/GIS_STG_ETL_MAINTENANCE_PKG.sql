create or replace PACKAGE                 ETL_MAINTENANCE_PKG
IS
-- Version 1.0: 11/16/2018
-- Developer: Kevin Dulay
-- Release Comments:
-----Version 1.0: Contains shared component for truncate procedure. Will include gather stats in next release.
-- Package will contain all functions related to maintaining staging tables, and other ETL interface related activities within the GIS_STG schema.
-- Package Comments:
----Functions do not contain inserts into INTERFACE_LOG table for errored or failed executions

FUNCTION TRUNCATE_TABLE
(
    P_TABLE_NM VARCHAR2
)
RETURN VARCHAR2;

END ETL_MAINTENANCE_PKG;
/
create or replace PACKAGE BODY                 ETL_MAINTENANCE_PKG
IS

FUNCTION TRUNCATE_TABLE
(
  P_TABLE_NM VARCHAR2
)
RETURN VARCHAR2 IS
    O_STATUS VARCHAR2(20);
    V_CODE NUMBER;
    V_ERRM VARCHAR2(1000);
    V_SQL VARCHAR2(4000);
    V_MSG VARCHAR2(1000);
BEGIN

V_SQL := 'TRUNCATE TABLE ' || P_TABLE_NM;

DBMS_OUTPUT.PUT_LINE('Executing SQL: '||V_SQL);

execute immediate V_SQL;

O_STATUS := 'SUCCESS';

DBMS_OUTPUT.PUT_LINE('Status is: '||O_STATUS);

RETURN O_STATUS;

EXCEPTION

		WHEN VALUE_ERROR THEN
			V_CODE := SQLCODE;
			V_ERRM := SUBSTR(SQLERRM ,1 ,100);
      O_STATUS := 'FAIL ERROR';
			DBMS_OUTPUT.PUT_LINE ('' || SYSTIMESTAMP || ' Error code ' || V_CODE || ': ' || V_ERRM);
      RETURN V_ERRM;

		WHEN OTHERS THEN
			V_CODE := SQLCODE;
			V_ERRM := SUBSTR(SQLERRM ,1 ,100);
			O_STATUS := 'FAIL OTHERS';
      DBMS_OUTPUT.PUT_LINE ('' || SYSTIMESTAMP || ' Error code ' || V_CODE || ': ' || V_ERRM);
      RETURN V_ERRM;



END TRUNCATE_TABLE;

END ETL_MAINTENANCE_PKG
;
/