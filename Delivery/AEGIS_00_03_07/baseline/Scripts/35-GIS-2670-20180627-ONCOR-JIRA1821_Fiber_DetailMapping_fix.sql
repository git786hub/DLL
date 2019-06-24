
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2670, 'ONCOR-JIRA1821_Fiber_DetailMapping_fix');
spool c:\temp\2670-20180627-ONCOR-JIRA1821_Fiber_DetailMapping_fix.log
--**************************************************************************************
--SCRIPT NAME: 2670-20180627-ONCOR-JIRA1821_Fiber_DetailMapping_fix.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 27-JUN-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER			: 1821
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Remove Fiber Cable and Transformer UG Network contained triggering cno from G3E_DETAILMAPPING 
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Update G3E_WORKMGMT SET G3E_PARAMETER1=NULL WHERE G3E_TYPE='Region';
update G3E_ATTRIBUTE set g3e_fino=null,g3e_functionaltype=null,g3e_functionalordinal=null where g3e_ano in (1411007,721005,1182006) and g3e_fino=62;

Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=14900 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7233;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=12200 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7233;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=2900 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7233;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=2800 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7233;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=14900 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7235;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=12200 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7235;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=2900 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7235;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=2800 AND G3E_CONTAINEDFNO=7200 AND G3E_CONTAINEDTRIGGERINGCNO=7235;

Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=106 AND G3E_CONTAINEDFNO=99 AND G3E_CONTAINEDTRIGGERINGCNO=6009;
Delete from G3E_DETAILMAPPING Where G3E_REFERENCINGFNO=117 AND G3E_CONTAINEDFNO=99 AND G3E_CONTAINEDTRIGGERINGCNO=6009;


COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2670);

