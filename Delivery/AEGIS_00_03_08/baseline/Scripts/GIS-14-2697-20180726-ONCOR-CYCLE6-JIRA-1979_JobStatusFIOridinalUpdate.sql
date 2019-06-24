
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2697, '1979_JobStatusFIOridinalUpdate');
spool c:\temp\2697-20180726-ONCOR-CYCLE6-JIRA-1979_JobStatusFIOridinalUpdate.log
--**************************************************************************************
--SCRIPT NAME: 2697-20180726-ONCOR-CYCLE6-JIRA-1979_JobStatusFIOridinalUpdate.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 26-JUL-2018
-- CYCLE		: 6

-- JIRA NUMBER	:1979
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	: Configure the Job Status FI to G3E_CID of Voucher Attributes.
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

UPDATE G3E_ATTRIBUTE SET G3E_FUNCTIONALORDINAL=5 WHERE G3E_FIELD ='G3E_ID' AND G3E_FUNCTIONALTYPE='Update' AND G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Job Status') AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');

UPDATE G3E_ATTRIBUTE SET G3E_FUNCTIONALORDINAL=4 WHERE G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='SetUserIdentifier') AND G3E_USERNAME='Requestor' AND G3E_FIELD='REQUEST_UID' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');


UPDATE G3E_ATTRIBUTE SET G3E_FUNCTIONALORDINAL=3 WHERE G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Set System Date') AND G3E_USERNAME='Date Requested' AND G3E_FIELD='REQUEST_D' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');

UPDATE G3E_ATTRIBUTE SET G3E_FUNCTIONALORDINAL=2 WHERE G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Set Job Attribute') AND G3E_USERNAME='Design/As Built' AND G3E_FIELD='DESIGN_ASBUILT_C' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');

UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Job Status'),G3E_FUNCTIONALORDINAL=1,
G3E_FUNCTIONALTYPE='AddNew' WHERE G3E_FIELD ='G3E_CID' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2697);

