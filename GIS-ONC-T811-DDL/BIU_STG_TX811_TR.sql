CREATE OR REPLACE TRIGGER BIU_STG_TX811_TR
BEFORE INSERT OR UPDATE ON GIS_STG.STG_TX811
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        SELECT SYSDATE INTO :NEW.AUD_CREATE_TS FROM DUAL;
        SELECT USER INTO :NEW.AUD_CREATE_USR_ID FROM DUAL;
    END IF;
        SELECT SYSDATE INTO :NEW.AUD_MOD_TS FROM DUAL;
        SELECT USER INTO :NEW.AUD_MOD_USR_ID FROM DUAL;
END;
/