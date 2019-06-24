set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2654, '1725');
spool c:\temp\2654-20180531-ONCOR-CYCLE6-JIRA-1725.log
--**************************************************************************************
--SCRIPT NAME: 2654-20180531-ONCOR-CYCLE6-JIRA-1725.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\UAVOTE
-- DATE		: 31-MAY-2018
-- CYCLE		: 
-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************
-- {Add script code}

CREATE OR REPLACE VIEW V_STREETCTR_CL_T (G3E_ID, G3E_FNO, G3E_CNO, G3E_FID, G3E_CID, G3E_GEOMETRY, G3E_ALIGNMENT, FULLNAME, STAGE) AS 
  (SELECT G.G3E_ID G3E_ID, G.G3E_FNO G3E_FNO, G.G3E_CNO G3E_CNO, G.G3E_FID G3E_FID, G.G3E_CID G3E_CID, G.G3E_GEOMETRY G3E_GEOMETRY, G.G3E_ALIGNMENT G3E_ALIGNMENT, NG1.FULLNAME FULLNAME ,NG.STAGE FROM LAND_AUDIT_N NG, STREETCTR_CL_T G, STREETCTR_CL_N NG1 WHERE G.G3E_FNO = 231 AND G.G3E_CNO = 23104 AND G.G3E_FID = NG1.G3E_FID AND G.G3E_FID = NG.G3E_FID);

UPDATE G3E_COMPONENTVIEWCOMPOSITION SET G3E_FIELDS='' WHERE G3E_CNO=23101 AND G3E_VNO=23101;
update g3e_label set g3e_content='[FULLNAME]' where g3e_lfno = 2312011;

CREATE OR REPLACE VIEW V_STREETCTR_ML_T (G3E_ID, G3E_FNO, G3E_CNO, G3E_FID, G3E_CID, G3E_GEOMETRY, G3E_ALIGNMENT, NAME, STAGE) AS 
  (SELECT G.G3E_ID G3E_ID, G.G3E_FNO G3E_FNO, G.G3E_CNO G3E_CNO, G.G3E_FID G3E_FID, G.G3E_CID G3E_CID, G.G3E_GEOMETRY G3E_GEOMETRY, G.G3E_ALIGNMENT G3E_ALIGNMENT, NG1.NAME NAME,NG.STAGE FROM LAND_AUDIT_N NG, STREETCTR_ML_T G, STREETCTR_ML_N NG1 WHERE G.G3E_FNO = 232 AND G.G3E_CNO = 23204 AND G.G3E_FID = NG1.G3E_FID AND G.G3E_FID = NG.G3E_FID);

UPDATE G3E_COMPONENTVIEWCOMPOSITION SET G3E_FIELDS='' WHERE G3E_CNO=23201 AND G3E_VNO=23201;

update g3e_label set g3e_content='[NAME]' where g3e_lfno = 2322011;

COMMIT;

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;
execute MG3ECreateOptableViews;
execute LTT_Role.Create_Public_Synonym;
execute LTT_Role.Create_Component_View_Synonym;
execute LTT_Role.Grant_Privs_To_Role;
execute ComponentQuery.Generate;
execute ComponentViewQuery.Generate;
execute GDOTRIGGERS.CREATE_GDOTRIGGERS; 
EXECUTE G3E_DynamicProcedures.Generate; 
execute G3E_MANAGEMODLOG.SyncToComponentView('V_STREETCTR_CL_T');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_STREETCTR_ML_T');
--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2654);