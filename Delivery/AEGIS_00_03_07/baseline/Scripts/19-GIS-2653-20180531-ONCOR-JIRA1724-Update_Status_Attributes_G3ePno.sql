
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2653, 'ONCOR-JIRA1724-Update_Status_Attributes_G3ePno');
spool c:\temp\2653-20180531-ONCOR-JIRA1724-Update_Status_Attributes_G3ePno.log
--**************************************************************************************
--SCRIPT NAME: 2653-20180531-ONCOR-JIRA1724-Update_Status_Attributes_G3ePno.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 31-MAY-2018
-- CYCLE				: 0.03.06
-- JIRA NUMBER			: 1724
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Configure the Status picklist for both Normal Status and As-Operated Status columns
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************


Update G3E_ATTRIBUTE Set G3E_PNO=(Select G3E_PNO From G3E_PICKLIST WHERE G3E_TABLE='VL_STATUS') WHERE G3E_FIELD IN ('STATUS_NORMAL_C','STATUS_OPERATED_C') AND G3E_CNO=11;

Update G3E_TABATTRIBUTE SET G3E_PNO=(Select G3E_PNO From G3E_PICKLIST WHERE G3E_TABLE='VL_STATUS') WHERE G3E_ANO IN (SELECT G3E_ANO FROM G3E_ATTRIBUTE WHERE G3E_FIELD IN ('STATUS_NORMAL_C','STATUS_OPERATED_C') AND G3E_CNO=11) AND G3E_DTNO IN (SELECT G3E_DTNO FROM G3E_DIALOGTAB WHERE G3E_USERNAME='Connectivity Attributes');

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2653);


