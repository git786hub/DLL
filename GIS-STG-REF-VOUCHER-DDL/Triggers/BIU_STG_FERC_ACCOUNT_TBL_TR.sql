 CREATE OR REPLACE TRIGGER GIS_STG.BIU_STG_FERC_ACCOUNT_TBL_TR
BEFORE UPDATE OR UPDATE ON GIS_STG.STG_FERC_ACCOUNT

FOR EACH ROW 
BEGIN 
  IF UPDATING THEN
   
	SELECT SYSDATE INTO :NEW.AUD_MOD_DT FROM dual;
	SELECT USER INTO :NEW.AUD_MOD_USR_ID FROM dual;
  END IF;
END;
/