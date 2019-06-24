
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2676, 'ONCOR-JIRA1863_DuctBank_Drop_VINT_YR');

spool c:\temp\2676-20180705-ONCOR-JIRA1863_DuctBank_Drop_VINT_YR.log
--**************************************************************************************
--SCRIPT NAME: 2676-20180705-ONCOR-JIRA1863_DuctBank_Drop_VINT_YR.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 05-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1863
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Drop column VINT_YR from DuctBank attribute component
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---**Drop column VINT_YR from DuctBank attribute component****

Delete From G3E_ATTRIBUTE where g3e_field='VINT_YR' and g3e_cno=2201;
COMMIT;

Alter Table B$DUCTBANK_N  Drop Column VINT_YR;

execute CREATE_VIEWS.CREATE_LTT_VIEW('B$DUCTBANK_N');
execute CREATE_TRIGGERS.CREATE_LTT_TRIGGER('B$DUCTBANK_N');

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;
execute MG3ECreateOptableViews;

execute GDOTRIGGERS.CREATE_GDOTRIGGERS; 
EXECUTE G3E_DynamicProcedures.Generate;


--------------------------------------------------------
--  DDL for Trigger B$DUCTBANK_N_HT
-- Removed DUCTBANK_N_HTPKG.VINT_YR:= :old.VINT_YR; from trigger
--------------------------------------------------------
CREATE OR REPLACE EDITIONABLE TRIGGER B$DUCTBANK_N_HT 
AFTER UPDATE OR DELETE OR INSERT ON B$DUCTBANK_N
FOR EACH ROW
BEGIN
	IF(LTT_ADMIN.V_LTTMODE ='POST' and DELETING and :old.ltt_status = 'COPIED') THEN
		DUCTBANK_N_HTPKG.LABEL_TEXT:= :old.LABEL_TEXT;
		DUCTBANK_N_HTPKG.g3e_fid:= :old.g3e_fid;
	END IF;

	IF(LTT_ADMIN.V_LTTMODE ='POST' and DELETING and :old.ltt_status = 'DELETE') THEN  --Case when feature is deleted and posted
		insert into ASSET_HISTORY(CHANGE_DATE,CHANGE_OPERATION,CHANGE_UID,G3E_ANO,G3E_CID,G3E_CNO,G3E_FID,G3E_FNO,G3E_IDENTIFIER,VALUE_NEW,VALUE_OLD) values (SYSDATE, 'DELETE',USER, NULL,:OLD.G3E_CID,:OLD.G3E_CNO,:OLD.G3E_FID, :OLD.G3E_FNO,(SELECT LTT_NAME FROM LTT_IDENTIFIERS WHERE LTT_ID = :OLD.LTT_ID), NULL,NULL);
	DUCTBANK_N_HTPKG.G3E_FID:=0;
	END IF;

	IF((LTT_ADMIN.V_LTTMODE ='POST' and UPDATING and :new.ltt_status is null) or ( LTT_ADMIN.V_LTTMODE = 'POST' and :new.ltt_status = 'COPIED' and UPDATING)) THEN  --Case when feature is updated and posted
		if (DUCTBANK_N_HTPKG.g3e_fid = :new.g3e_fid) then
			 if ((DUCTBANK_N_HTPKG.LABEL_TEXT <> :new.LABEL_TEXT) or (DUCTBANK_N_HTPKG.LABEL_TEXT is null and :new.LABEL_TEXT is not null) or (DUCTBANK_N_HTPKG.LABEL_TEXT is not null and :new.LABEL_TEXT is null)) then
				insert into ASSET_HISTORY(CHANGE_DATE,CHANGE_OPERATION,CHANGE_UID,G3E_ANO,G3E_CID,G3E_CNO,G3E_FID,G3E_FNO,VALUE_NEW,VALUE_OLD,G3E_IDENTIFIER)
    values (SYSDATE, 'UPDATED', USER,220101,:NEW.G3E_CID,:NEW.G3E_CNO,:NEW.G3E_FID,:NEW.G3E_FNO, substr(:new.LABEL_TEXT,1,100),substr(DUCTBANK_N_HTPKG.LABEL_TEXT,1,100),
    (SELECT LTT_NAME FROM LTT_IDENTIFIERS WHERE LTT_ID = :OLD.LTT_ID));
			 end if;

		 end if;

		 if (DUCTBANK_N_HTPKG.g3e_fid =0 or DUCTBANK_N_HTPKG.g3e_fid is null) then --Case of first time feature creation and post
			insert into ASSET_HISTORY(CHANGE_DATE,CHANGE_OPERATION,CHANGE_UID,G3E_ANO,G3E_CID,G3E_CNO,G3E_FID,G3E_FNO,G3E_IDENTIFIER,VALUE_NEW,VALUE_OLD) values
  (sysdate,'CREATE',user,NULL,:new.G3E_CID,:new.G3E_CNO,:new.G3E_FID, :new.G3E_FNO,(SELECT LTT_NAME FROM LTT_IDENTIFIERS WHERE LTT_ID = :OLD.LTT_ID),NULL,NULL);
		 end if;

		DUCTBANK_N_HTPKG.G3E_FID:= 0;
	 END IF;
	 EXCEPTION WHEN OTHERS THEN
	 RAISE;
END;
/
Show Errors

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2676);

