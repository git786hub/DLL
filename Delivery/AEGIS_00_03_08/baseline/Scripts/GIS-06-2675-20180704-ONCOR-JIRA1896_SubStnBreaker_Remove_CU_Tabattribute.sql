
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2675, 'ONCOR-JIRA1896_SubStnBreaker_Remove_CU_Tabattribute');
spool c:\temp\2675-20180704-ONCOR-JIRA1896_SubStnBreaker_Remove_CU_Tabattribute.log
--**************************************************************************************
--SCRIPT NAME: 2675-20180704-ONCOR-JIRA1896_SubStnBreaker_Remove_CU_Tabattribute.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\PVKURELL
-- DATE				    : 04-JUL-2018
-- CYCLE			      : 00.03.07
-- JIRA NUMBER		  : 1896
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Remove CU from substation Breaker Dialog tab and create Ancillary CU dialog specific to Substation Breakers to disable FKQ
--**************************************************************************************
-- Modified:
--  01-AUG-2018, Rich Adase -- No need to create a specialized ancillary CU tab; FKQ is allowed to run as usual
--**************************************************************************************

---**Delete CU code from Substation Breaker Attributes dialog tab**
Delete from g3e_tabattribute where g3e_ano=2203 and g3e_dtno in (select g3e_dtno from g3e_dialogtab where g3e_username='Substation Breaker Attributes');

/*
---***Ancillary CU  dialog specific to Substation Breaker and Substation Breaker Network***
Insert into G3E_DIALOGTAB (G3E_DTNO,G3E_USERNAME,G3E_ORIENTATION) values (16203,'Ancillary CU Attributes','H');

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620301,2201,10,null,0,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620302,221000,20,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620303,2202,30,228,0,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620304,2203,40,null,0,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620305,2204,50,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620306,2206,60,null,0,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620307,2207,70,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620308,2208,80,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (1620309,2209,90,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203010,2210,100,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203011,2211,110,30035,0,'N',16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203012,2217,120,332,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203013,2214,130,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203014,2215,140,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203015,2216,150,null,1,null,16203);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DEFAULT,G3E_DTNO) values (16203016,2219,160,null,0,null,16203);

---***Add Ancillary CU dialog to Substation Breaker and Substation Breaker- NETWORK ****
Update G3E_DIALOG SET G3E_DTNO=16203 Where G3e_FNO IN (16,91) AND G3E_DTNO=61206;
*/


spool off;
exec adm_support.set_finish(2675);

