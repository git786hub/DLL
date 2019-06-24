CREATE OR REPLACE TRIGGER GIS_STG.BIU_STG_CULIB_STD_ATTR_TBL_TR 
BEFORE UPDATE ON GIS_STG.STG_CULIB_STANDARD_ATTR

FOR EACH ROW 
BEGIN 
  IF UPDATING THEN
   
    SELECT SYSDATE INTO :NEW.AUD_CREATE_DT FROM dual;
		SELECT USER INTO :NEW.AUD_CREATE_USR_ID FROM dual;
  END IF;
END;
/