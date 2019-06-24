DELETE FROM gis_onc.stlt_boundary;

DROP TABLE gis.stlt_boundary_temp;

CREATE TABLE gis.stlt_boundary_temp (
    bnd_fno        NUMBER(5,0),
    bnd_fname      VARCHAR2(80 BYTE),
    bnd_type_ano   NUMBER(9,0),
    bnd_type       VARCHAR2(80 BYTE),
    bnd_id_ano     NUMBER(9,0),
    bnd_value      VARCHAR2(80 BYTE)
);

DECLARE
    TYPE bdny_value_type IS RECORD ( bndy_type       VARCHAR2(30),
    bndy_value      VARCHAR2(30) );
    TYPE bndy_value_table IS
        TABLE OF bdny_value_type;
    bnd_fno         NUMBER(5,0) := 0;
    bnd_type_ano    NUMBER(9,0) := NULL;
    bnd_type        VARCHAR2(80 BYTE);
    bnd_value       VARCHAR(80 BYTE);
    bnd_id_ano      NUMBER(9,0) := 0;
    tablename       VARCHAR2(30 BYTE);
    field_name      VARCHAR2(30 BYTE);
    field_type      VARCHAR2(30 BYTE);
    feature_name    VARCHAR2(80 BYTE);
    attibute_name   VARCHAR(80 BYTE);
    sql_stmt        VARCHAR2(200);
    bnd_table       bndy_value_table;
    cnt             INT := 0;
BEGIN

/* LOOP THROUGH INITAL LIST OF ATTRIBUTESA FOR ALL LANDBASE BOUNDARIES */
    FOR i IN (
        SELECT
            feat.g3e_fno,
            feat.g3e_username,
            feat.g3e_primaryattributecno cno,
            'B$' || comp.g3e_table tablename,
            attr.g3e_ano,
            attr.g3e_field,
            attr.g3e_username fieldname
        FROM
            g3e_feature feat
            JOIN g3e_component comp ON comp.g3e_cno = feat.g3e_primaryattributecno
            JOIN g3e_attribute attr ON attr.g3e_cno = feat.g3e_primaryattributecno
        WHERE
            (
                feat.g3e_username = 'Municipality Boundary'
                AND attr.g3e_field IN (
                    'CITY_TYPE',
                    'STATE'
                )
            )
            OR (
                feat.g3e_username = 'County Boundary'
                AND attr.g3e_field = 'NAME'
            )
        ORDER BY
            g3e_fno
    ) LOOP
     
/* SET LOCAL VARIABLES FOR RECORD IN CURRENT ITERATION*/
        bnd_fno := i.g3e_fno;
        tablename := i.tablename;
        feature_name := i.g3e_username;

/* SET ANO VALUES BASED ON WHETHER FIELD VALUE IS A BOUNDARY TYPE OR BOUNDARY NAME */
        IF
            i.g3e_field LIKE '%NAME%' --and i.g3e_field not like '%CITY%'
        THEN
            bnd_id_ano := i.g3e_ano;
            field_name := i.g3e_field;
			
			/* RETRIEVE BOUNDARY VALUES FOR EACH LANDBASE TABLE*/
            sql_stmt := 'SELECT '
                        || field_name
                        || ','
                        || field_name
                        || ' FROM '
                        || tablename;

            EXECUTE IMMEDIATE sql_stmt BULK COLLECT
            INTO bnd_table;
            FOR j IN 1..bnd_table.count LOOP
                bnd_value := bnd_table(j).bndy_value;
		  
				/* INSERT RECORDS INTO STREETLIGHT BOUNDARY TEMP TABLE */
                INSERT INTO gis.stlt_boundary_temp (
                    bnd_fno,
                    bnd_type_ano,
                    bnd_type,
                    bnd_id_ano,
                    bnd_value,
                    bnd_fname
                ) VALUES (
                    bnd_fno,
                    bnd_type_ano,
                    bnd_type,
                    bnd_id_ano,
                    bnd_value,
                    feature_name
                );
            END LOOP;
            bnd_table.DELETE;
        
        ELSIF i.g3e_field LIKE '%STATE%' THEN
            /* INSERT STATE BOUNDARY RECORD INTO STLT TEMP TABLE */
            INSERT INTO gis.stlt_boundary_temp (
                bnd_fno,
                bnd_type_ano,
                bnd_type,
                bnd_id_ano,
                bnd_value,
                bnd_fname
            ) VALUES (
                234,
                2340104,
                'State',
                2340101,
                'Texas',
                'Municipality Boundary'
            );

        ELSIF i.g3e_field LIKE '%TYPE%' THEN
            bnd_type_ano := i.g3e_ano;
            field_type := i.g3e_field;
            bnd_id_ano := i.g3e_ano;
			
			/* RETRIEVE BOUNDARY TYPES FROM THE LANDBASE TABLE */
            sql_stmt := 'SELECT '
                        || field_type
                        || ','
                        || 'CITY_NAME'
                        || ' FROM '
                        || tablename ;

            EXECUTE IMMEDIATE sql_stmt BULK COLLECT
            INTO bnd_table;
            FOR j IN 1..bnd_table.count LOOP
                bnd_type := bnd_table(j).bndy_type;
                bnd_value := bnd_table(j).bndy_value;
		  
				/* INSERT RECORDS FOR EACH BOUNDARY TYPE INTO THE STREETLIGHT BOUNDARY TEMP TABLE*/
                INSERT INTO gis.stlt_boundary_temp (
                    bnd_fno,
                    bnd_type_ano,
                    bnd_type,
                    bnd_id_ano,
                    bnd_value,
                    bnd_fname
                ) VALUES (
                    bnd_fno,
                    bnd_type_ano,
                    bnd_type,
                    bnd_id_ano,
                    bnd_value,
                    feature_name
                );

            END LOOP;
			/* RESET THE TABLE TYPE VARIABLE */

            bnd_table.DELETE;
        END IF;

		/* RESET LOCAL VARIABLES FOR NEXT ITERATION*/

        field_type := NULL;
        field_name := NULL;
        tablename := NULL;
        bnd_type := NULL;
        bnd_fno := NULL;
        bnd_id_ano := NULL;
        bnd_type_ano := NULL;
        cnt := cnt + 1;
    END LOOP;
--
/* POPULATE STREETLIGHT BOUNDARY TABLE USING THE TEMP TABLE */

    INSERT INTO gis_onc.stlt_boundary (
        bnd_fno,
        bnd_type_ano,
        bnd_type,
        bnd_id_ano
    )
        SELECT DISTINCT
            bnd_fno,
            bnd_type_ano,
            bnd_type,
            bnd_id_ano
        FROM
            gis.stlt_boundary_temp commit;
END;