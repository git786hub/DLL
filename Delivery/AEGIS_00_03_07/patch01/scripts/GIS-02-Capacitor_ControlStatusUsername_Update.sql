set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2690, '1826_Capacitor_ControlStatusUsername_Update');
spool c:\temp\2690-20180716-ONCOR-CYCLE6-JIRA-1826_Capacitor_ControlStatusUsername_Update.log
--**************************************************************************************
--SCRIPT NAME: 2690-20180716-ONCOR-CYCLE6-JIRA-1826_Capacitor_ControlStatusUsername_Update.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 16-JUL-2018
-- CYCLE		: 6

-- JIRA NUMBER	:1826
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	: Updating the G3E_USERNAME of Control Status by removing the new line character that is interfering with CU processing.
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

UPDATE G3E_ATTRIBUTE SET G3E_USERNAME='Control Status' WHERE G3E_FIELD='CONTROL_STATUS' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Capacitor Bank Attributes' AND G3E_TABLE='CAPACITOR_BANK_N');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2690);

