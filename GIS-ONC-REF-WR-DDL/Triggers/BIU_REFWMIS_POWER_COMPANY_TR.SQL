CREATE OR REPLACE TRIGGER GIS_ONC.BIU_REFWMIS_POWER_COMPANY_TR
BEFORE INSERT OR UPDATE ON GIS_ONC.REFWMIS_POWER_COMPANY

FOR EACH ROW
BEGIN
  IF UPDATING THEN

	SELECT SYSDATE INTO :NEW.AUD_MOD_TS FROM dual;
	SELECT USER INTO :NEW.AUD_MOD_USR_ID FROM dual;
  END IF;


END;