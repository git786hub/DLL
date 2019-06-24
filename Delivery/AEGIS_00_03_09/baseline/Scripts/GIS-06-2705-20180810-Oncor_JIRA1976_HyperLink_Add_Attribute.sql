
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2705, 'Oncor_JIRA1976_HyperLink_Add_Attribute');
spool c:\temp\2705-20180810-Oncor_JIRA1976_HyperLink_Add_Attribute.log
--**************************************************************************************
--SCRIPT NAME: 2705-20180810-Oncor_JIRA1976_HyperLink_Add_Attribute.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 10-AUG-2018
-- CYCLE				: 00.03.09
-- JIRA NUMBER			: 1976
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Add FILENAME_T attribute to Hyperlink and Job_Hyperlink attribute component****
--                        Remove Hyperlink dialog from Placement and drop vl_hyperlink_type value list table****
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---**Add col filename_t to hyperlink and job hyperlink component table and modify TYPE_C data length***
Alter Table B$HYPERLINK_N Add FILENAME_T	VARCHAR2(80);
Alter Table B$HYPERLINK_N MODIFY TYPE_C	    VARCHAR2(30);

Alter Table JOB_HYPERLINK_N Add FILENAME_T	VARCHAR2(80);
Alter Table JOB_HYPERLINK_N MODIFY TYPE_C	VARCHAR2(30);

execute CREATE_VIEWS.CREATE_LTT_VIEW('B$HYPERLINK_N');
execute CREATE_TRIGGERS.CREATE_LTT_TRIGGER('B$HYPERLINK_N');

---***Add columns to component****
Insert into G3E_ATTRIBUTE (G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE) values (813004,8130,null,'FILENAME_T','File Name',null,0,'File Name',0,NULL,0,0,10,0,1,1,1,1,0,sysdate);

Insert into G3E_ATTRIBUTE (G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE) values (204,2,null,'FILENAME_T','File Name',null,0,'File Name',0,NULL,0,0,10,0,1,1,1,1,0,SYSDATE);


---***Add attributes to dialog tab***

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (15020803,204,40,NULL,1,150208);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (15010803,204,40,NULL,1,150108);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (510030388,813004,4,NULL,1,812004);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (510030389,813004,4,NULL,1,811004);

---Drop Picklist VL_HYPERLINK_TYPE***
Update G3E_TABATTRIBUTE Set g3e_pno=null WHERE G3E_PNO=(SELECT G3E_PNO FROM G3E_PICKLIST WHERE G3E_TABLE='VL_HYPERLINK_TYPE');
Update G3E_ATTRIBUTE Set g3e_pno=null WHERE G3E_PNO=(SELECT G3E_PNO FROM G3E_PICKLIST WHERE G3E_TABLE='VL_HYPERLINK_TYPE');

Delete FROM G3E_PICKLIST WHERE G3E_TABLE='VL_HYPERLINK_TYPE';

Drop table VL_HYPERLINK_TYPE;
Drop public Synonym VL_HYPERLINK_TYPE;

----Drop Hyperlink placement dialog tab ***

update g3e_dialog set g3e_dtno=150108 where g3e_dtno=53109;
update g3e_dialog set g3e_dtno=150208 where g3e_dtno=53209;

Delete from g3e_dialog where G3E_TYPE NOT IN ('Edit','Review') and g3e_dtno in (select g3e_dtno from g3e_dialogtab where g3e_username IN ('Job Hyperlink Attributes','Hyperlink Attributes'));

--***Drop orphan dialog tabs***
Delete from g3e_dialogtab dt where g3e_username IN ('Job Hyperlink Attributes','Hyperlink Attributes') and not exists (select 1 from g3e_dialog dg where dg.g3e_dtno=dt.g3e_dtno);

---***SET Hyperlink tab attributes to readonly in edit mode ***
Update G3E_TABATTRIBUTE Set G3E_READONLY=1 WHERE G3E_DTNO IN (SELECT distinct dg.G3E_DTNO FROM G3E_DIALOG DG ,G3E_DIALOGTAB DT WHERE DG.G3E_DTNO=DT.G3E_DTNO AND DT.G3E_USERNAME IN ('Job Hyperlink Attributes','Hyperlink Attributes')) AND G3E_READONLY=0; 


---***Execute Packages****
execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;
execute MG3ECreateOptableViews;

execute GDOTRIGGERS.CREATE_GDOTRIGGERS; 
execute G3E_DynamicProcedures.Generate;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2705);

