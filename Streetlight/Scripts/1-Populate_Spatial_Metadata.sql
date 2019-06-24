		DECLARE
		    RESULT_COUNT_1 NUMBER(2,0) := 0;
		    RESULT_COUNT_2 NUMBER(2,0) := 0;
		    SQL_STMT_1 VARCHAR(400 BYTE) := 'INSERT INTO user_sdo_geom_metadata (table_name, column_name, diminfo) VALUES (''B$MUNICIPALITY_AR'', ''G3E_GEOMETRY'', mdsys.sdo_dim_array(mdsys.sdo_dim_element(''X'',6358613.5,6800726.23,0.005),mdsys.sdo_dim_element(''Y'',1058011.74,2045630.38,0.005), mdsys.sdo_dim_element(''Z'',0,0,0.005)))';
		    SQL_STMT_2 VARCHAR(400 BYTE) := 'INSERT INTO user_sdo_geom_metadata (table_name, column_name, diminfo) VALUES (''B$COUNTY_AR'', ''G3E_GEOMETRY'', mdsys.sdo_dim_array(mdsys.sdo_dim_element(''X'',6358613.5,6800726.23,0.005),mdsys.sdo_dim_element(''Y'',1058011.74,2045630.38,0.005), mdsys.sdo_dim_element(''Z'',0,0,0.005)))';
		BEGIN
		    SELECT COUNT(*) INTO RESULT_COUNT_1 FROM user_sdo_geom_metadata WHERE table_name = 'B$MUNICIPALITY_AR';
		    SELECT COUNT(*) INTO RESULT_COUNT_2 FROM user_sdo_geom_metadata WHERE table_name = 'B$COUNTY_AR';
		    
		    IF RESULT_COUNT_1 < 1 THEN
		        EXECUTE IMMEDIATE SQL_STMT_1;
				EXECUTE IMMEDIATE 'COMMIT';
		    END IF;
		    IF RESULT_COUNT_2 < 1 THEN
		        EXECUTE IMMEDIATE SQL_STMT_2;
		        EXECUTE IMMEDIATE 'COMMIT';
		    END IF;
END;