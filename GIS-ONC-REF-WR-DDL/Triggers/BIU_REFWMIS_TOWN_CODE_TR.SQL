CREATE OR REPLACE TRIGGER GIS_ONC.BIU_REFWMIS_TOWN_CODE_TR
BEFORE INSERT OR UPDATE ON GIS_ONC.REFWMIS_TOWN_CODE

FOR EACH ROW
BEGIN
  IF UPDATING THEN

	SELECT SYSDATE INTO :NEW.AUD_MOD_TS FROM dual;
	SELECT USER INTO :NEW.AUD_MOD_USR_ID FROM dual;
  END IF;


END;