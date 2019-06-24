BEGIN
    FOR v_LoopCounter IN 1..72 LOOP
        INSERT INTO SYS_GENERALPARAMETER (ID, SUBSYSTEM_NAME, SUBSYSTEM_COMPONENT, PARAM_NAME, PARAM_VALUE)
        VALUES (gis_onc.sys_generalparam_seq.nextval, 'GuyingCC', 'Summary Bottom Value', 'Page ' || v_LoopCounter, v_LoopCounter * 63);
    END LOOP;
END;
/
