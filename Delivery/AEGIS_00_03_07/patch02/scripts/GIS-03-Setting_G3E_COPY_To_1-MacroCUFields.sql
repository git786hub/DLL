
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2700, 'ONCORDEV-1982-Setting_G3E_COPY_To_1-MacroCUFields');
spool c:\temp\2700-20180727-ONCOR-CYCLE6-JIRA-ONCORDEV-1982-Setting_G3E_COPY_To_1-MacroCUFields.log
--**************************************************************************************
--SCRIPT NAME: 2700-20180727-ONCOR-CYCLE6-JIRA-ONCORDEV-1982-Setting_G3E_COPY_To_1-MacroCUFields.sql
--**************************************************************************************
-- AUTHOR		: SAGARWAL
-- DATE			: 27-JUL-2018
-- JIRA NUMBER		: ONCORDEV-1982
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to set g3e_copy to 1 for Macro Fields
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

UPDATE G3E_ATTRIBUTE SET G3E_COPY = 1 WHERE G3E_ANO IN (2103,2204);

COMMIT;
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2700);

