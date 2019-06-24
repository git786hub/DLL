
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2512, 'ONCORDEV-842-ServiceLineAssociatedCost');
spool c:\temp\2512-20180301-ONCOR-CYCLE6-JIRA-ONCORDEV-842-ServiceLineAssociatedCost.log
--**************************************************************************************
--SCRIPT NAME: 2512-20180301-ONCOR-CYCLE6-JIRA-ONCORDEV-842-ServiceLineAssociatedCost.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 01-MAR-2018
-- CYCLE			: 6

-- JIRA NUMBER		: ONCORDEV-842
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create FI Service Line Associated Cost
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

SET DEFINE OFF;

execute Create_Sequence.Create_Metadata_Sequences;

Insert into G3E_FUNCTIONALINTERFACE (G3E_FINO,G3E_USERNAME,G3E_INTERFACE,G3E_ARGUMENTPROMPT,G3E_ARGUMENTTYPE,G3E_EDITDATE,G3E_DESCRIPTION) values (G3E_FUNCTIONALINTERFACE_SEQ.nextval,'Service Line Associated Cost Install','fiServicLineAssociatedCostInstall:GTechnology.Oncor.CustomAPI.fiServicLineAssociatedCostInstall',null,null,sysdate,'FI to update Activity to IA if the Placement type is Associated for Service Line');


update G3E_ATTRIBUTE set g3e_fino = (SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME = 'Service Line Associated Cost Install'), g3e_functionalordinal = (SELECT MAX(nvl(g3e_functionalordinal, 0))+1 FROM G3E_ATTRIBUTE WHERE G3E_CNO = 5401), g3e_functionaltype = 'Update' where g3e_cno = 5401 and g3e_field = 'PLACEMENT_TYPE_C';

update G3E_ATTRIBUTE set g3e_fino = (SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME = 'Service Line Associated Cost Install'), g3e_functionalordinal = (SELECT MAX(nvl(g3e_functionalordinal, 0))+1 FROM G3E_ATTRIBUTE WHERE G3E_CNO = 21), g3e_functionaltype = 'Update' where g3e_cno = 21 and g3e_field = 'G3E_CID';

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2512);

