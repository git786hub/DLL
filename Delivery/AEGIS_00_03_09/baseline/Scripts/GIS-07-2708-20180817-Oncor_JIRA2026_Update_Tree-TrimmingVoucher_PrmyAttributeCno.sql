set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2708, 'Oncor_JIRA2026_Update_Tree-TrimmingVoucher_PrmyAttributeCno.sql');
spool c:\temp\2708-20180817-Oncor_JIRA2026_Update_Tree-TrimmingVoucher_PrmyAttributeCno.log
--**************************************************************************************
--SCRIPT NAME: 2708-20180817-Oncor_JIRA2026_Update_Tree-TrimmingVoucher_PrmyAttributeCno.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 17-AUG-2018
-- CYCLE				: 00.03.09
-- JIRA NUMBER			: 2026
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: update Tree-TrimmingVoucher PrimaryAttribute cno to "VOUCHER_N" attribute component 
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************


update g3e_feature set g3e_primaryattributecno=(select g3e_cno from g3e_component where g3e_table='VOUCHER_N') where g3e_username='Tree-trimming Voucher';

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2708);

