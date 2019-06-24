set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\ImplementSetDefaultActivityFI.sql.log
--**************************************************************************************
--SCRIPT NAME: ImplementSetDefaultActivityFI.sql
--**************************************************************************************
-- AUTHOR			: Barry Scott
-- DATE				: 29-JUN-2018
-- JIRA NUMBER		: 
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to install the FI Set Default CU Activity on the Ancillary CU Component
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- Install the FI on the Ancillary CU Component (CNO 22)
update g3e_attribute set
g3e_fino=(select g3e_fino from g3e_functionalinterface where g3e_username='Set Default CU Activity'),
g3e_functionalordinal=(select max(g3e_functionalordinal)+1 from g3e_attribute where g3e_cno=21),
g3e_functionaltype='AddNew',
g3e_interfaceargument=null
where g3e_cno=22 and g3e_field='ACTIVITY_C';

commit;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;


