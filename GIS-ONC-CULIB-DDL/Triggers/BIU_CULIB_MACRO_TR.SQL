CREATE OR REPLACE EDITIONABLE TRIGGER GIS_ONC.BIU_CULIB_MACRO_TR 
BEFORE UPDATE ON GIS_ONC.CULIB_MACRO
FOR EACH ROW
BEGIN
  IF UPDATING THEN
    SELECT SYSDATE INTO :NEW.AUD_CREATE_DT FROM DUAL;
    SELECT USER INTO :NEW.AUD_CREATE_USR_ID FROM DUAL;
  END IF;
END;


/
ALTER TRIGGER GIS_ONC.BIU_CULIB_MACRO_TR ENABLE;