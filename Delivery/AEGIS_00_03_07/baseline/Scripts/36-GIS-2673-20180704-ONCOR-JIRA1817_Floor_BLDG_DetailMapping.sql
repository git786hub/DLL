
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2673, 'ONCOR-JIRA1817_Floor_BLDG_DetailMapping.sql');
spool c:\temp\2673-20180704-ONCOR-JIRA1817_Floor_BLDG_DetailMapping.log
--**************************************************************************************
--SCRIPT NAME: 2673-20180704-ONCOR-JIRA1817_Floor_BLDG_DetailMapping.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 04-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1817
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Add Floor Detail Miscellanesous label and leader line component to referencing Building detail window
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************


Insert into G3E_DETAILMAPPING (G3E_DMROWNO,G3E_REFERENCINGFNO,G3E_CONTAINEDFNO,G3E_CONTAINEDTRIGGERINGCNO) values ((select max(g3e_dmrowno)+1 from g3e_detailmapping),217,14200,14241);
Insert into G3E_DETAILMAPPING (G3E_DMROWNO,G3E_REFERENCINGFNO,G3E_CONTAINEDFNO,G3E_CONTAINEDTRIGGERINGCNO) values ((select max(g3e_dmrowno)+1 from g3e_detailmapping),217,14200,43);
Insert into G3E_DETAILMAPPING (G3E_DMROWNO,G3E_REFERENCINGFNO,G3E_CONTAINEDFNO,G3E_CONTAINEDTRIGGERINGCNO) values ((select max(g3e_dmrowno)+1 from g3e_detailmapping),217,14200,44);


Insert into G3E_DETAILMAPPING (G3E_DMROWNO,G3E_REFERENCINGFNO,G3E_CONTAINEDFNO,G3E_CONTAINEDTRIGGERINGCNO) values ((select max(g3e_dmrowno)+1 from g3e_detailmapping),14100,14200,43);
Insert into G3E_DETAILMAPPING (G3E_DMROWNO,G3E_REFERENCINGFNO,G3E_CONTAINEDFNO,G3E_CONTAINEDTRIGGERINGCNO) values ((select max(g3e_dmrowno)+1 from g3e_detailmapping),14100,14200,44);

COMMIT;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2673);

