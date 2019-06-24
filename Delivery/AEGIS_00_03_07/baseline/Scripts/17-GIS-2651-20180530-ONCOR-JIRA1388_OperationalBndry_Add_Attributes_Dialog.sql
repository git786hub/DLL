
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2651, 'ONCOR-JIRA1388_OperationalBndry_Add_Attributes_Dialog');
spool c:\temp\2651-20180530-ONCOR-JIRA1388_OperationalBndry_Add_Attributes_Dialog.log
--**************************************************************************************
--SCRIPT NAME: 2651-20180530-ONCOR-JIRA1388_OperationalBndry_Add_Attributes_Dialog.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 30-MAY-2018
-- CYCLE				: 03.06
-- JIRA NUMBER			: 1388
-- PRODUCT VERSION		: 10.03
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Add attribute to Operational Boundary dialog
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---***Add attribute Operational Boundary dialog tab***

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO) values (20830107,208011000,30,0,208301);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO) values (20820107,208011000,30,0,208201);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO) values (20810107,208011000,30,1,208101);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO) values (20830106,208011001,35,0,208301);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO) values (20820106,208011001,35,0,208201);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_READONLY,G3E_DTNO) values (20810106,208011001,35,1,208101);

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2651);


