set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2696, '1873_UpdateG3ECopyForCUDefaultAttributes');
spool c:\temp\2696-20180726-ONCOR-CYCLE6-JIRA-1873_UpdateG3ECopyForCUDefaultAttributes.log
--**************************************************************************************
--SCRIPT NAME: 2696-20180726-ONCOR-CYCLE6-JIRA-1873_UpdateG3ECopyForCUDefaultAttributes.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 26-JUL-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

UPDATE G3E_ATTRIBUTE SET G3E_COPY=1 WHERE G3E_FIELD IN('CU_DESC','PRIME_ACCT_NBR','PROP_UNIT_ID','RETIREMENT_C','QTY_LENGTH_Q','WR_EDITED') AND G3E_CNO IN(22,21);
update g3e_attribute set g3e_fino = null where g3e_cno = 22 and g3e_fino = (select g3e_fino from g3e_functionalinterface where g3E_username = 'Set CU Standard Attributes');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2696);

