set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2634, 'ONCOR-JIRA1703_Capacitor_Add_Attribute');
spool c:\temp\2634-20180522-ONCOR-JIRA1703_Capacitor_Add_Attribute.log
--**************************************************************************************
--SCRIPT NAME: 2634-20180522-ONCOR-JIRA1703_Capacitor_Add_Attribute.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 22-MAY-2018
-- CYCLE			: 03.06
-- JIRA NUMBER		: 1703
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Add model number attribute to CAPACITOR_BANK_N component
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************
Alter table B$CAPACITOR_BANK_N add MODEL_Q VARCHAR2(5);

execute CREATE_VIEWS.CREATE_LTT_VIEW('B$CAPACITOR_BANK_N');
execute CREATE_TRIGGERS.CREATE_LTT_TRIGGER('B$CAPACITOR_BANK_N');

Insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(40113,401,NULL,'MODEL_Q','Model Number',null,0,'Model Number',0,null,0,1,10,0,0,0,1,1,0,sysdate);

---***Add attribute capacitor dialog tab***
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_PNO,G3E_DTNO) values (430113,40113,85,1,null,4301);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_PNO,G3E_DTNO) values (420113,40113,85,1,null,4201);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_PNO,G3E_DTNO) values (410113,40113,85,1,null,4101);

---***Changes As per DFS
Update g3e_attribute set g3e_required=0 where g3e_field in ('CAPACITOR_CONN','CONTROL_STATUS') and g3e_cno=401;
Update g3e_attribute set g3e_required=1 where g3e_field='ACU_CONTROL' and g3e_cno=401;

---***Add SCADA Enabled to Capacitor Placement dialog****
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (430114,331003,27,326,0,'N',4301);

COMMIT;

---***Execute Package****
EXECUTE MG3ElanguageSubTableUtils.SynchronizeSubTables;
EXECUTE MG3EOTCreateOptimizedTables;
EXECUTE MG3ECreateOptableViews;
EXECUTE ComponentQuery.Generate;
EXECUTE ComponentViewQuery.Generate;
execute GDOTRIGGERS.CREATE_GDOTRIGGERS; 
execute StaticPost.Generate;



--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2634);

