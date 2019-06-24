

set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2689, 'ONCOR_JIRA1849-PrimaryUnitAttribute_Add_Attributes');
spool c:\temp\2689-20180713-ONCOR_JIRA1849-PrimaryUnitAttribute_Add_Attributes.log
--**************************************************************************************
--SCRIPT NAME: 2689-20180713-ONCOR_JIRA1849-PrimaryUnitAttribute_Add_Attributes.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 13-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1849
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Add attributes to Primary Switch Unit attribute component
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************


---**Create Value list****
CREATE TABLE VL_SWITCH_BUSHING_TYPE
(
	VL_KEY    	VARCHAR2(10) NOT NULL, 
	VL_VALUE    VARCHAR2(30) ,
	UNUSED      NUMBER(1),    
	ORDINAL     NUMBER(10)  
);

CREATE TABLE VL_SWITCH_MEDIUM_TYPE
(
	VL_KEY    	VARCHAR2(10) NOT NULL, 
	VL_VALUE    VARCHAR2(30) ,
	UNUSED      NUMBER(1),    
	ORDINAL     NUMBER(10)  
);


Insert into G3E_PICKLIST (G3E_PNO,G3E_USERNAME,G3E_TABLE,G3E_KEYFIELD,G3E_VALUEFIELD,G3E_VALIDATION,G3E_PUBLISH) values (382,'Switch Bushing Type','VL_SWITCH_BUSHING_TYPE','VL_KEY','VL_VALUE','G3ERestricted',0);
Insert into G3E_PICKLIST (G3E_PNO,G3E_USERNAME,G3E_TABLE,G3E_KEYFIELD,G3E_VALUEFIELD,G3E_VALIDATION,G3E_PUBLISH) values (383,'Switch Medium Type','VL_SWITCH_MEDIUM_TYPE','VL_KEY','VL_VALUE','G3ERestricted',0);


---***Add columns to Primary Switch Unit Attribute component table***

  
Alter Table B$PRI_SWITCH_UNIT_N Add BUSHING_TYPE_C VARCHAR2(20);
Alter Table B$PRI_SWITCH_UNIT_N Add MEDIUM_TYPE_C VARCHAR2(20);
Alter Table B$PRI_SWITCH_UNIT_N Add MFR_C VARCHAR2(30);
Alter Table B$PRI_SWITCH_UNIT_N Add MFR_D DATE; 
Alter Table B$PRI_SWITCH_UNIT_N Add MODEL VARCHAR2(30); 


execute CREATE_VIEWS.CREATE_LTT_VIEW('B$PRI_SWITCH_UNIT_N');
execute CREATE_TRIGGERS.CREATE_LTT_TRIGGER('B$PRI_SWITCH_UNIT_N');

---***Add columns to Primary Switch Unit attribute Component ****

insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(130207,1302,NULL,'BUSHING_TYPE_C','Bushing Type','',0,'Bushing Type',0,382,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(130208,1302,NULL,'MEDIUM_TYPE_C','Medium Type','',0,'Medium Type',0,383,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(130209,1302,NULL,'MFR_C','Manufacturer','',0,'Manufacturer',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(130210,1302,NULL,'MFR_D','Manufacture Date','Date',0,'Manufacture Date',0,NULL,0,0,8,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(130211,1302,NULL,'MODEL','Model','',0,'Model',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);


---***Add attributes to Primary OH Unit Dialog tab attributes ****

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(1330204,130207,23,0,13302,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330205,130208,24,0,13302,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330206,130209,25,0,13302,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330207,130210,26,0,13302,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330208,130211,27,0,13302,NULL);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(1320204,130207,23,0,13202,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320205,130208,24,0,13202,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320206,130209,25,0,13202,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320207,130210,26,0,13202,NULL);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320208,130211,27,0,13202,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310204,130207,23,1,13102,382);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310205,130208,24,1,13102,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310206,130209,25,1,13102,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310207,130210,26,1,13102,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310208,130211,27,1,13102,NULL);

---***Add atributes to Primary UG Unit atribute dialog tab***

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(1330401,130207,23,0,13304,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330402,130208,24,0,13304,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330403,130209,25,0,13304,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330404,130210,26,0,13304,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1330405,130211,27,0,13304,NULL);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(1320401,130207,23,0,13204,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320402,130208,24,0,13204,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320403,130209,25,0,13204,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320404,130210,26,0,13204,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1320405,130211,27,0,13204,NULL);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(1310401,130207,23,1,13104,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310402,130208,24,1,13104,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310403,130209,25,1,13104,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310404,130210,26,1,13104,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(1310405,130211,27,1,13104,NULL);

---***Add atributes to Primary UG Unit NETWORK attribute dialog tab***

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(9030211,130207,33,0,90302,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9030212,130208,34,0,90302,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9030213,130209,35,0,90302,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9030214,130210,36,0,90302,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9030215,130211,37,0,90302,NULL);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(9020211,130207,33,0,90202,382,'T-BODY');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9020212,130208,34,0,90202,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9020213,130209,35,0,90202,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9020214,130210,36,0,90202,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9020215,130211,37,0,90202,NULL);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9010209,130207,33,1,90102,382);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9010210,130208,34,1,90102,383);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9010211,130209,35,1,90102,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9010212,130210,36,1,90102,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(9010213,130211,37,1,90102,NULL);

COMMIT;

---***Create public synonym on picklist and grant privileges***

Create public SYNONYM VL_SWITCH_BUSHING_TYPE for VL_SWITCH_BUSHING_TYPE;
Grant Select on VL_SWITCH_BUSHING_TYPE to DESIGNER,FINANCE,MARKETING;
Grant Select,Insert,Update,Delete on VL_SWITCH_BUSHING_TYPE to SUPERVISOR,ADMINISTRATOR;

Create public SYNONYM VL_SWITCH_MEDIUM_TYPE for VL_SWITCH_MEDIUM_TYPE;
Grant Select on VL_SWITCH_MEDIUM_TYPE to DESIGNER,FINANCE,MARKETING;
Grant Select,Insert,Update,Delete on VL_SWITCH_MEDIUM_TYPE to SUPERVISOR,ADMINISTRATOR;

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
exec adm_support.set_finish(2689);

