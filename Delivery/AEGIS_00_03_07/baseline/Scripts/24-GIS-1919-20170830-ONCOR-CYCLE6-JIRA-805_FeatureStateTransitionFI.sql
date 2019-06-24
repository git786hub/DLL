set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(1919, '805_FeatureStateTransitionFI');
spool c:\temp\1919-20170830-ONCOR-CYCLE6-JIRA-805_FeatureStateTransitionFI.log
--**************************************************************************************
--SCRIPT NAME: 1919-20170830-ONCOR-CYCLE6-JIRA-805_FeatureStateTransitionFI.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 30-AUG-2017
-- CYCLE		: 6

-- JIRA NUMBER	:805
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:FI copies the attribute value for which interface configured to Connectivity/Ductivity components and if new state is INI,associated virtual point is deleted and fix connectivity to eliminate the gap left by deleted feature.
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************
execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;

INSERT INTO G3E_FUNCTIONALINTERFACE (G3E_FINO,G3E_USERNAME,G3E_INTERFACE,G3E_ARGUMENTPROMPT,G3E_ARGUMENTTYPE,G3E_EDITDATE,G3E_DESCRIPTION) VALUES (G3E_FUNCTIONALINTERFACE_SEQ.NEXTVAL,'Feature State Transition','fiFeatureStateTransition:GTechnology.Oncor.CustomAPI.fiFeatureStateTransition',null,null,SYSDATE,'Copies the attribute value for which interface configured to Connectivity/Ductivity components and if new state is INI,associated virtual point is deleted and fix connectivity to eliminate the gap left by deleted feature.');

UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Feature State Transition'),G3E_FUNCTIONALORDINAL=1,G3E_FUNCTIONALTYPE='Update' WHERE G3E_USERNAME='Feature State' AND G3E_FIELD='FEATURE_STATE_C' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Common Attributes');

COMMIT;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(1919);

