
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2652, 'ONCOR-JIRA1723-Update_Connectivity_Attributes-G3eCopy');
spool c:\temp\2652-20180531-ONCOR-JIRA1723-Update_Connectivity_Attributes-G3eCopy.log
--**************************************************************************************
--SCRIPT NAME: 2652-20180531-ONCOR-JIRA1723-Update_Connectivity_Attributes-G3eCopy.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 31-MAY-2018
-- CYCLE			: 0.03.06
-- JIRA NUMBER		: 1723
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Set g3e_copy=0 for Connectivity Attributes 
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='NETWORK_ID' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='FEEDER_1_ID' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='FEEDER_2_ID' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='STATUS_NORMAL_C' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='VOLT_1_Q' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_FEEDER_1_ID' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_FEEDER_NBR' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_FEEDER_2_ID' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_TIE_SSTA_C' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_TIE_FEEDER_NBR' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_VOLT_1_Q' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='PP_NETWORK_ID' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='FEEDER_TYPE_C' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='TIE_SSTA_C' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='FEEDER_NBR' and g3e_cno=11;
Update G3E_ATTRIBUTE Set g3e_copy=0 Where G3E_FIELD='TIE_FEEDER_NBR' and g3e_cno=11;

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2652);

