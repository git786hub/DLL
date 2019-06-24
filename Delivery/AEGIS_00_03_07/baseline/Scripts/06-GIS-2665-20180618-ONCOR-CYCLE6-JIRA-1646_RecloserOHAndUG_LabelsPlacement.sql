set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2665, '1646_RecloserOHAndUG_LabelsPlacement');
spool c:\temp\2665-20180618-ONCOR-CYCLE6-JIRA-1646_RecloserOHAndUG_LabelsPlacement.log
--**************************************************************************************
--SCRIPT NAME: 2665-20180618-ONCOR-CYCLE6-JIRA-1646_RecloserOHAndUG_LabelsPlacement.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 18-JUN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

--Recloser - OH

Insert into G3E_PLACEMENTCONFIGURATION (G3E_PCROWNO,G3E_PCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_INTERFACEARGUMENT,G3E_TOOLTIP,G3E_FEATUREINDEX,G3E_REFRESH,G3E_EDITDATE,G3E_SILENTRGNO,G3E_LOCALECOMMENT,G3E_REQUIRED,G3E_REPEATING,G3E_CCNO,G3E_PLACECOMPONENT,G3E_RINO,G3E_RIARGGROUPNO,G3E_DETAILPLACEMENTCNO) values (G3E_PLACEMENTCONFIGURATION_SEQ.nextval,(SELECT DISTINCT G3E_PCNO FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - OH'),'Recloser - OH',14,(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Recloser Label'),(SELECT MAX(G3E_ORDINAL)+1 FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - OH'),0,7,'0C20010000001000000001000000010000000C20010000001000000000000000050000000800000000000300000026004800390008000000000000000000080000000000000000000800000000000000000008000000000000000000080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','Recloser - OH',1,1,SYSDATE,null,null,1,0,null,1,NULL,null,null);


Insert into G3E_PLACEMENTCONFIGURATION (G3E_PCROWNO,G3E_PCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_INTERFACEARGUMENT,G3E_TOOLTIP,G3E_FEATUREINDEX,G3E_REFRESH,G3E_EDITDATE,G3E_SILENTRGNO,G3E_LOCALECOMMENT,G3E_REQUIRED,G3E_REPEATING,G3E_CCNO,G3E_PLACECOMPONENT,G3E_RINO,G3E_RIARGGROUPNO,G3E_DETAILPLACEMENTCNO) values (G3E_PLACEMENTCONFIGURATION_SEQ.nextval,(SELECT DISTINCT G3E_PCNO FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - OH'),'Recloser - OH',14,(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Recloser Label Large'),(SELECT MAX(G3E_ORDINAL)+1 FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - OH'),0,10,'0C20010000001000000001000000080000000800000000000C000000310034002F0031002F0031003400300034002F002D00310008000000000001000000300008000000000001000000300008000000000001000000310008000000000001000000300008000000000001000000310000000800000000000100000030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','Recloser - OH',1,1,SYSDATE,null,null,1,0,null,1,NULL,null,null);


--Recloser - UG

Insert into G3E_PLACEMENTCONFIGURATION (G3E_PCROWNO,G3E_PCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_INTERFACEARGUMENT,G3E_TOOLTIP,G3E_FEATUREINDEX,G3E_REFRESH,G3E_EDITDATE,G3E_SILENTRGNO,G3E_LOCALECOMMENT,G3E_REQUIRED,G3E_REPEATING,G3E_CCNO,G3E_PLACECOMPONENT,G3E_RINO,G3E_RIARGGROUPNO,G3E_DETAILPLACEMENTCNO) values (G3E_PLACEMENTCONFIGURATION_SEQ.nextval,(SELECT DISTINCT G3E_PCNO FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - UG'),'Recloser - UG',15,(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Recloser Label'),(SELECT MAX(G3E_ORDINAL)+1 FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - UG'),0,7,'0C20010000001000000001000000010000000C20010000001000000000000000050000000800000000000300000026004800390008000000000000000000080000000000000000000800000000000000000008000000000000000000080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','Recloser - UG',1,1,SYSDATE,null,null,1,0,null,1,NULL,null,null);


Insert into G3E_PLACEMENTCONFIGURATION (G3E_PCROWNO,G3E_PCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_INTERFACEARGUMENT,G3E_TOOLTIP,G3E_FEATUREINDEX,G3E_REFRESH,G3E_EDITDATE,G3E_SILENTRGNO,G3E_LOCALECOMMENT,G3E_REQUIRED,G3E_REPEATING,G3E_CCNO,G3E_PLACECOMPONENT,G3E_RINO,G3E_RIARGGROUPNO,G3E_DETAILPLACEMENTCNO) values (G3E_PLACEMENTCONFIGURATION_SEQ.nextval,(SELECT DISTINCT G3E_PCNO FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - UG'),'Recloser - UG',15,(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Recloser Label Large'),(SELECT MAX(G3E_ORDINAL)+1 FROM G3E_PLACEMENTCONFIGURATION WHERE G3E_USERNAME='Recloser - UG'),0,10,'0C20010000001000000001000000080000000800000000000C000000310035002F0031002F0031003400300034002F002D00310008000000000001000000300008000000000001000000300008000000000001000000310008000000000001000000300008000000000001000000310000000800000000000100000030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000','Recloser - UG',1,1,SYSDATE,null,null,1,0,null,1,NULL,null,null);


COMMIT;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2665);
