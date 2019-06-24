

set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2694, 'ONCOR_JIRA1971_update_ReadWriteControl_Type');
spool c:\temp\2694-20180723-ONCOR_JIRA1971_update_ReadWriteControl_Type.log
--**************************************************************************************
--SCRIPT NAME: 2694-20180723-ONCOR_JIRA1971_update_ReadWriteControl_Type.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 23-JUL-2018
-- CYCLE				: 00.03.08
-- JIRA NUMBER			: 1971
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: update "Read Write Control Definition interface" type from "Attribute Edit Control Definition" to "Attribute Edit Control GUI"  
--                        which will allow user to update attribute programtically and prevent user to edit from Feature explorer.
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************


Update G3E_RELATIONINTERFACE set G3E_TYPE = 'Attribute Edit Control GUI' where g3e_username = 'Read Write Control Definition'; 

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2694);

