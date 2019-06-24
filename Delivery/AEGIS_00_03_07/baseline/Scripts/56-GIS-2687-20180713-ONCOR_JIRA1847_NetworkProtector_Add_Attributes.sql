
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2687, 'ONCOR_JIRA1847_NetworkProtector_Add_Attributes');
spool c:\temp\2687-20180713-ONCOR_JIRA1847_NetworkProtector_Add_Attributes.log
--**************************************************************************************
--SCRIPT NAME: 2687-20180713-ONCOR_JIRA1847_NetworkProtector_Add_Attributes.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 13-JUL-2018
-- CYCLE			: 00.03.07
-- JIRA NUMBER		: 1847
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Add attributes to Network Protector Attribute component
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---***Add columns to Network Protector component table***

Alter Table B$NET_PROTECTOR_N Add MFR_C	           VARCHAR2(30);
Alter Table B$NET_PROTECTOR_N Add MFR_D	           DATE;
Alter Table B$NET_PROTECTOR_N Add MODEL	           VARCHAR2(30);
Alter Table B$NET_PROTECTOR_N Add SWITCH_NBR	   VARCHAR2(30);
Alter Table B$NET_PROTECTOR_N Add RELAY_CT_TYPE	   VARCHAR2(30);
Alter Table B$NET_PROTECTOR_N Add RELAY_MFR_D	   DATE;
Alter Table B$NET_PROTECTOR_N Add RELAY_MODEL	   VARCHAR2(50);
Alter Table B$NET_PROTECTOR_N Add RELAY_SERIAL_NBR VARCHAR2(50);
Alter Table B$NET_PROTECTOR_N Add FIRE_PROBES_YN   VARCHAR2(1);


execute CREATE_VIEWS.CREATE_LTT_VIEW('B$NET_PROTECTOR_N');
execute CREATE_TRIGGERS.CREATE_LTT_TRIGGER('B$NET_PROTECTOR_N');


---***Add columns to Network Protector Attribute Cmponent ****

insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570416,15704,NULL,'MFR_C','Manufacturer',NULL,0,'Manufacturer',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570417,15704,NULL,'MFR_D','Manufacture Date','Date',0,'Manufacture Date',0,NULL,0,0,8,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570418,15704,NULL,'MODEL','Model',NULL,0,'Model',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570419,15704,NULL,'SWITCH_NBR','Switch Number',NULL,0,'Switch Number',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570420,15704,NULL,'RELAY_CT_TYPE','Relay CT Type',NULL,0,'Relay CT Type',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570421,15704,NULL,'RELAY_MFR_D','Relay Manufacture Date','Date',0,'Relay Manufacture Date',0,NULL,0,0,8,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570422,15704,NULL,'RELAY_MODEL','Relay Model',NULL,0,'Relay Model',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570423,15704,NULL,'RELAY_SERIAL_NBR','Relay Serial Number',NULL,0,'Relay Serial Number',0,NULL,0,0,10,0,0,0,1,1,0,sysdate);
insert into G3E_ATTRIBUTE(G3E_ANO,G3E_CNO,G3E_VNO,G3E_FIELD,G3E_USERNAME,G3E_FORMAT,G3E_REQUIRED,G3E_TOOLTIP,G3E_HYPERTEXT,G3E_PNO,G3E_COPY,G3E_EXCLUDEFROMEDIT,G3E_DATATYPE,G3E_EXCLUDEFROMREPLACE,G3E_BREAKCOPY,G3E_COPYATTRIBUTE,G3E_WRAPTEXT,G3E_FUNCTIONALVALIDATION,G3E_UNIQUE,G3E_EDITDATE)  Values(1570424,15704,NULL,'FIRE_PROBES_YN','Fire Probes Installed',NULL,0,'Fire Probes Installed',0,326,0,0,10,0,0,0,1,1,0,sysdate);


---***Add attributes to Network Protector attribute Dialog tab****

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730116,1570416,33,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730117,1570417,34,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730118,1570418,35,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730119,1570419,55,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730120,1570420,65,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730121,1570421,72,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730122,1570422,73,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15730123,1570423,74,0,157301,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(15730124,1570424,120,0,157301,326,'Y');

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720116,1570416,33,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720117,1570417,34,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720118,1570418,35,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720119,1570419,55,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720120,1570420,65,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720121,1570421,72,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720122,1570422,73,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15720123,1570423,74,0,157201,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(15720124,1570424,120,0,157201,326,'Y');

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710116,1570416,33,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710117,1570417,34,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710118,1570418,35,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710119,1570419,55,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710120,1570420,65,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710121,1570421,72,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710122,1570422,73,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO) VALUES(15710123,1570423,74,1,157101,NULL);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,G3E_PNO,G3E_DEFAULT) VALUES(15710124,1570424,120,1,157101,326,'Y');

COMMIT;

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
exec adm_support.set_finish(2687);

