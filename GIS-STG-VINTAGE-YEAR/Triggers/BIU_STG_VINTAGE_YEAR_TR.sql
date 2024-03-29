CREATE OR REPLACE EDITIONABLE TRIGGER GIS_STG.BIU_STG_VINTAGE_YEAR_TR 
BEFORE UPDATE ON GIS_STG.STG_VINTAGE_YEAR

FOR EACH ROW 
BEGIN 
  IF UPDATING THEN
	SELECT SYSDATE INTO :NEW.AUD_MOD_DT FROM dual;
	SELECT USER INTO :NEW.AUD_MOD_USR_ID FROM dual;
  END IF;
END;
/
