set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2422, 'ONCORDEV-1173_SignificantAncillaryCreationFI');
spool c:\temp\2422-20180111-ONCOR-CYCLE6-JIRA-ONCORDEV-1173_SignificantAncillaryCreationFI.log
--**************************************************************************************
--SCRIPT NAME: 2422-20180111-ONCOR-CYCLE6-JIRA-ONCORDEV-1173_SignificantAncillaryCreationFI.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 11-JAN-2018
-- CYCLE			: 6
-- JIRA NUMBER		:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create metadata for the FI Significant Ancillary Creation
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

begin create_sequence.create_metadata_sequence('G3E_FUNCTIONALINTERFACE','G3E_FINO','G3E_FUNCTIONALINTERFACE_SEQ');end;
/

insert into g3e_functionalinterface
columns(g3e_fino,g3e_username,g3e_interface,g3e_argumentprompt,g3e_argumenttype,g3e_editdate,g3e_description)
values (g3e_functionalinterface_seq.nextval,'Significant Ancillary Creation','fSignificantAncillaryCreation:GTechnology.Oncor.CustomAPI.fSignificantAncillaryCreation',null,null,sysdate,null);

UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Significant Ancillary Creation'),G3E_FUNCTIONALORDINAL=nvl((select max(G3E_FUNCTIONALORDINAL)+1 from G3E_ATTRIBUTE where g3E_cno =401),1),G3E_FUNCTIONALTYPE='SetValue',G3E_INTERFACEARGUMENT= null WHERE G3E_CNO = 401 AND G3E_FIELD='ACU_CONTROL';
UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Significant Ancillary Creation'),G3E_FUNCTIONALORDINAL=nvl((select max(G3E_FUNCTIONALORDINAL)+1 from G3E_ATTRIBUTE where g3E_cno =1102),1),G3E_FUNCTIONALTYPE='SetValue',G3E_INTERFACEARGUMENT= null WHERE G3E_CNO = 1102 AND G3E_FIELD='ACU_BRACKET';
UPDATE G3E_ATTRIBUTE SET G3E_FINO=(SELECT G3E_FINO FROM G3E_FUNCTIONALINTERFACE WHERE G3E_USERNAME='Significant Ancillary Creation'),G3E_FUNCTIONALORDINAL=nvl((select max(G3E_FUNCTIONALORDINAL)+1 from G3E_ATTRIBUTE where g3E_cno =1102),1),G3E_FUNCTIONALTYPE='SetValue',G3E_INTERFACEARGUMENT= null WHERE G3E_CNO = 1102 AND G3E_FIELD='ACU_LINK';

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2422);

