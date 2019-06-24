set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2637, '1705');
spool c:\temp\2637-20180522-ONCOR-CYCLE6-JIRA-1705.log
--**************************************************************************************
--SCRIPT NAME: 2637-20180522-ONCOR-CYCLE6-JIRA-1705.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\UAVOTE
-- DATE		: 22-MAY-2018
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

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,g3e_pno,g3e_default) VALUES(6020217,(select g3e_ano from g3e_attribute where g3e_field='BANKED_YN' and g3e_cno=6002),150,0,60202,326,'N');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,g3e_pno,g3e_default) VALUES(6030217,(select g3e_ano from g3e_attribute where g3e_field='BANKED_YN' and g3e_cno=6002),150,0,60302,326,'N');
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO,g3e_pno,g3e_default) VALUES(6010217,(select g3e_ano from g3e_attribute where g3e_field='BANKED_YN' and g3e_cno=6002),150,1,60102,326,'N');

Update g3e_tabattribute set g3e_ordinal=115 where g3e_ano=250031023 and g3e_dtno in (60202,60302,60102);
Update g3e_tabattribute set g3e_ordinal=116 where g3e_ano=250031024 and g3e_dtno in (60202,60302,60102);
Update g3e_tabattribute set g3e_ordinal=40 where g3e_ano=600204 and g3e_dtno in (60202,60302,60102);
Update g3e_tabattribute set g3e_ordinal=50 where g3e_ano=600205 and g3e_dtno in (60202,60302,60102);
delete g3e_tabattribute where g3e_ano = 600214 and g3e_dtno in (60202,60302);

Update g3e_tabattribute set G3E_READONLY=1 where g3e_ano in (2102,600202,600204,600205,600207,250031023,250031024) and g3e_dtno in (60202,60302);
Update g3e_tabattribute set G3E_READONLY=0 where g3e_ano in (1107) and g3e_dtno in (60202,60302);
--Update g3e_tabattribute set G3E_PNO= null where g3e_ano in (600209) and g3e_dtno in (60202,60302);
Update g3e_tabattribute set G3E_PNO= 236 where g3e_ano in (600212) and g3e_dtno in (60202,60302);

update g3e_attribute set g3e_required = 0 where g3e_ano in (600202,600207);
update g3e_attribute set g3e_required = 1 where g3e_ano in (600210,250031025);

COMMIT;

execute MG3ElanguageSubTableUtils.SynchronizeSubTables;
execute MG3EOTCreateOptimizedTables;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2637);