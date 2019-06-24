
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2657, 'ONCORDEV-1736_OrientationMetadataInterfaceArgument');
spool c:\temp\2657-20180611-ONCOR-CYCLE6-JIRA-ONCORDEV-1736_OrientationMetadataInterfaceArgument.log
--**************************************************************************************
--SCRIPT NAME: 2657-20180611-ONCOR-CYCLE6-JIRA-ONCORDEV-1736_OrientationMetadataInterfaceArgument.sql
--**************************************************************************************
-- AUTHOR				: SAGARWAL
-- DATE					: 11-JUN-2018
-- CYCLE				: 6

-- JIRA NUMBER			: ONCORDEV-1736
-- PRODUCT VERSION		: 10.2.04
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Script to chnge Interface Argument for Simple Copy RAI to include copy when not null
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}
UPDATE G3E_ATTRIBUTEVALIDATION SET G3e_INTERFACEARGUMENT = '0C20010000001000000001000000010000000800000000000100000030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000' WHERE G3E_VINO =(SELECT G3e_VINO FROM 
G3E_VALIDATIONINTERFACE WHERE G3E_USERNAME = 'SimpleCopy') AND G3E_aCTIVEANO = (SELECT 
G3E_ANO FROM G3E_aTTRIBUTE WHERE G3E_FIELD = 'ORIENTATION_C' AND G3E_CNO = 1);

COMMIT;
--**************************************************************************************
-- End Script Body

--**************************************************************************************
commit;
spool off;
exec adm_support.set_finish(2657);

