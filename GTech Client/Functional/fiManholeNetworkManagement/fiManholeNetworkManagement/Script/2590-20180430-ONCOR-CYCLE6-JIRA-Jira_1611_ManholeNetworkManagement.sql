
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2590, 'Jira_1611_ManholeNetworkManagement');
spool c:\temp\2590-20180430-ONCOR-CYCLE6-JIRA-Jira_1611_ManholeNetworkManagement.log
--**************************************************************************************
--SCRIPT NAME: 2590-20180430-ONCOR-CYCLE6-JIRA-Jira_1611_ManholeNetworkManagement.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\SKAMARAJ
-- DATE		: 30-APR-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Insert into G3E_FUNCTIONALINTERFACE (G3E_FINO,G3E_USERNAME,G3E_INTERFACE,G3E_ARGUMENTPROMPT,G3E_ARGUMENTTYPE,G3E_EDITDATE,G3E_DESCRIPTION) values (G3E_FUNCTIONALINTERFACE_seq.nextval,'Manhole Network Management','fiManholeNetworkManagement:GTechnology.Oncor.CustomAPI.fiManholeNetworkManagement',null,null,SYSDATE,'Manhole Network Management');


UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Manhole Network Management'),G3E_FUNCTIONALORDINAL=1,
G3E_FUNCTIONALTYPE='AddNew' WHERE G3E_FIELD in ('NETWORK_MANAGED_YN','NETWORK_RESTRICTED_YN') AND G3E_CNO IN (SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME IN ('Manhole Attributes'));

update G3E_TABATTRIBUTE set G3E_DEFAULT='' where g3e_ano in 
(select g3e_ano from g3e_attribute where g3e_field in ('NETWORK_MANAGED_YN','NETWORK_RESTRICTED_YN') AND g3e_cno=10601);

ALTER TABLE B$MANHOLE_N MODIFY NETWORK_MANAGED_YN DEFAULT 'N';
ALTER TABLE B$MANHOLE_N MODIFY NETWORK_RESTRICTED_YN DEFAULT 'N';

execute create_triggers.create_ltt_trigger('B$MANHOLE_N');
execute create_views.create_ltt_view('B$MANHOLE_N');
execute GDOTRIGGERS.CREATE_GDOTRIGGERS('MANHOLE_N');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2590);