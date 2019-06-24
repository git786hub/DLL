
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2709, 'ONCORDEV-2030-SetUpstreamNodeDefault');
spool c:\temp\2709-20180821-ONCOR-CYCLE6-JIRA-ONCORDEV-2030-SetUpstreamNodeDefault.log
--**************************************************************************************
--SCRIPT NAME: 2709-20180821-ONCOR-CYCLE6-JIRA-ONCORDEV-2030-SetUpstreamNodeDefault.sql
--**************************************************************************************
-- AUTHOR		: SAGARWAL
-- DATE			: 21-AUG-2018
-- CYCLE		: 6
-- JIRA NUMBER		: ONCORDEV-2030
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to set default for the UPSTREAM_NODE and PP_UPSTREAM_NODE 
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

ALTER TABLE B$CONNECTIVITY_N  MODIFY (UPSTREAM_NODE DEFAULT 1 );
ALTER TABLE B$CONNECTIVITY_N  MODIFY (PP_UPSTREAM_NODE DEFAULT 1 );

execute create_triggers.create_ltt_trigger('B$CONNECTIVITY_N');
execute create_views.create_ltt_view('B$CONNECTIVITY_N');
execute GDOTRIGGERS.CREATE_GDOTRIGGERS('CONNECTIVITY_N');

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2709);

