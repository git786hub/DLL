--**************************************************************************************
-- SCRIPT NAME: G3E_JOB-DesignerIDTriggers.sql
--**************************************************************************************
-- AUTHOR           : Barry Scott
-- DATE             : 29-MAR-2018
-- PRODUCT VERSION  : 10.3.0
-- PRJ IDENTIFIER   : G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     : New triggers to set the DESIGNER ID columns to ensure
--                      proper operation of the Job Environment queries due to Kerberos IDs
--**************************************************************************************
-- Modified:
--   04-SEP-2018, Barry Scott: Added WHEN filter for cases when both fields are null and
--                               removed exception for that case in body of trigger.
--   05-SEP-2018, Rich Adase: Added exception handler to swallow errors for now, to expedite
--                               testing; need to follow up later to see if exceptions were
--                               actually thrown as part of WMIS batch processing
--**************************************************************************************

set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\G3E_JOB-DesignerIDTriggers.sql.log

--
-- W_M_BIR_DESIGNER_ID  (Trigger)
--
CREATE OR REPLACE TRIGGER GIS.W_M_BIR_DESIGNER_ID
BEFORE INSERT OR UPDATE OF DESIGNER_UID,DESIGNER_RACFID
ON GIS.G3E_JOB
FOR EACH ROW
WHEN(NEW.DESIGNER_UID IS NOT NULL AND NEW.DESIGNER_RACFID IS NOT NULL)

DECLARE
	SUFFIX SYS_GENERALPARAMETER.PARAM_VALUE%TYPE;
	I NUMBER(2);

BEGIN

	-- If :new.DESIGNER_RACFID is not null and :new.DESIGNER_UID is null:
	--	o Ensure :new.DESIGNER_RACFID does not contain an @ (trim @ and any trailing characters)
	--	o Set :new.DESIGNER_UID to :new.DESIGNER_RACFID + suffix from SYS_GENERALPARAMETER
	--
	-- If :new.DESIGNER_RACFID is null and :new.DESIGNER_UID is not null:
	--	o If :new.DESIGNER_UID does not contain an @:
	--		o Set :new.DESIGNER_RACFID to :new.DESIGNER_UID
	--		o Append suffix from SYS_GENERALPARAMETER to :new.DESIGNER_UID
	--	o Else
	--		o Set :new.DESIGNER_RACFID to the prefix (before the @) of :new.DESIGNER_UID
	--
	-- If :new.DESIGNER_RACFID is not null and :new.DESIGNER_UID is not null:
	--	o Ensure DESIGNER_RACFID does not contain an @ (trim @ and any trailing characters)
	--	o Ensure the RACFID in DESIGNER_UID agrees with DESIGNER_RACFID
	--	o If DESIGNER_UID does not contain an @, then append suffix from SYS_GENERALPARAMETER to :new.DESIGNER_UID
	
	SELECT PARAM_VALUE INTO SUFFIX FROM SYS_GENERALPARAMETER
	WHERE SUBSYSTEM_NAME='WMIS' AND SUBSYSTEM_COMPONENT='Job Creation' AND PARAM_NAME='RACFID Suffix';
	
	IF :NEW.DESIGNER_RACFID IS NOT NULL AND :NEW.DESIGNER_UID IS NULL THEN
		I:=INSTR(:NEW.DESIGNER_RACFID,'@');
		
		IF 0<I THEN
			:NEW.DESIGNER_RACFID:=SUBSTR(:NEW.DESIGNER_RACFID,1,I-1);
		END IF;
		
		:NEW.DESIGNER_UID:=:NEW.DESIGNER_RACFID||SUFFIX;
	END IF;
	
	IF :NEW.DESIGNER_RACFID IS NULL AND :NEW.DESIGNER_UID IS NOT NULL THEN
		I:=INSTR(:NEW.DESIGNER_UID,'@');
		
		IF 0<I THEN
			:NEW.DESIGNER_RACFID:=SUBSTR(:NEW.DESIGNER_UID,1,I-1);
		ELSE
			:NEW.DESIGNER_RACFID:=:NEW.DESIGNER_UID;
			:NEW.DESIGNER_UID:=:NEW.DESIGNER_UID||SUFFIX;
		END IF;
	
	END IF;
	
	IF :NEW.DESIGNER_RACFID IS NOT NULL AND :NEW.DESIGNER_UID IS NOT NULL THEN
		I:=INSTR(:NEW.DESIGNER_RACFID,'@');
		
		IF 0<I THEN
			:NEW.DESIGNER_RACFID:=SUBSTR(:NEW.DESIGNER_RACFID,1,I-1);
		END IF;

		I:=INSTR(:NEW.DESIGNER_UID,'@');
		
		IF 0=I THEN
			:NEW.DESIGNER_UID:=:NEW.DESIGNER_UID||SUFFIX;
			I:=INSTR(:NEW.DESIGNER_UID,'@');
		END IF;
		
		IF :NEW.DESIGNER_RACFID!=SUBSTR(:NEW.DESIGNER_UID,1,I) THEN
			:NEW.DESIGNER_RACFID:=SUBSTR(:NEW.DESIGNER_UID,1,I-1);
		END IF;
		
	END IF;

exception
  when others then
    null;	
END;
/

spool off;