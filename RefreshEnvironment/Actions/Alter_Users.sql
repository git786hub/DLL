spool ALTER_Users_&SDATE..log

DECLARE
	PARAM_GIS_PASS VARCHAR2(20);
	PARAM_GIS_ONC_PASS VARCHAR2(20);
	PARAM_GIS_STG_PASS VARCHAR2(20);
	PARAM_GIS_AUTO_PASS VARCHAR2(20);
    
BEGIN
    PARAM_GIS_PASS := '&GIS_PASS';
    PARAM_GIS_ONC_PASS := '&GIS_ONC_PASS';
    PARAM_GIS_STG_PASS := '&GIS_STG_PASS';
    PARAM_GIS_AUTO_PASS := '&GISAUTOMATOR_PASS';
    
    execute immediate 'alter user gis identified by ' || PARAM_GIS_PASS;
    execute immediate 'alter user gis_onc identified by ' || PARAM_GIS_ONC_PASS;
    execute immediate 'alter user gis_stg identified by ' || PARAM_GIS_STG_PASS;
    execute immediate 'alter user GISAUTOMATOR identified by ' || PARAM_GIS_AUTO_PASS;   

    COMMIT;
END;
/
