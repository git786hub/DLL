set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2661, 'ONCORDEV-1727-Correct-AttributeValidation');
spool c:\temp\2661-20180612-ONCOR-CYCLE6-JIRA-ONCORDEV-1727-Correct-AttributeValidation.log
--**************************************************************************************
--SCRIPT NAME: 2661-20180612-ONCOR-CYCLE6-JIRA-ONCORDEV-1727-Correct-AttributeValidation.sql
--**************************************************************************************
-- AUTHOR				: SAGARWAL
-- DATE					: 12-JUN-2018
-- CYCLE				: 6
-- JIRA NUMBER			: ONCORDEV-1727
-- PRODUCT VERSION		: 10.2.04
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Script to correct attributevaliadtion metadata for the operator RelatedNotEqualTo
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************
execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;
-- {Add script code}
DELETE FROM G3E_ATTRIBUTEVALIDATION WHERE G3E_OPERATOR = 'RelatedNotEqualTo' AND G3E_ACTIVEANO = (SELECT G3E_ANO FROM G3E_ATTRIBUTE WHERE
G3E_CNO = 1 AND G3E_FIELD = 'FEATURE_STATE_C') AND G3E_RELATEDANO = (SELECT G3E_ANO FROM G3E_ATTRIBUTE WHERE
G3E_CNO = 1 AND G3E_FIELD = 'FEATURE_STATE_C');


declare
V_COUNT NUMBER DEFAULT 0;
begin
for cur in (select G3E_SOURCEFNO,G3E_CONNECTINGFNO from G3E_NODEEDGECONN_ELEC GROUP BY G3E_SOURCEFNO,G3E_CONNECTINGFNO ORDER BY G3E_SOURCEFNO)
	loop
		begin
			V_COUNT := V_COUNT +1;
			
			gao_log.put_line('ATTRVAL_CONNECTIVITYCONFIG , ACTIVE FNO : '|| cur.G3E_SOURCEFNO ||sqlerrm);
			gao_log.put_line('ATTRVAL_CONNECTIVITYCONFIG , RELATED FNO : '|| cur.G3E_CONNECTINGFNO ||sqlerrm);
						
			
			--Table:Common Attributes,Field:Feature State,Operator:RelatedNotEqualTo
			Insert into G3E_ATTRIBUTEVALIDATION (G3E_AVNO,G3E_ACTIVEFNO,G3E_ACTIVEANO,G3E_RELATEDFNO,G3E_RELATEDANO,G3E_OPERATOR,G3E_OPERATIONTYPE,G3E_VINO,G3E_INTERFACEARGUMENT,G3E_VALIDATIONVALUE,G3E_PRIORITY,G3E_EDITDATE,G3E_MOUSEMOVECANDIDATEFILTER,G3E_RELATIONSHIPTYPE) values 
			(G3E_ATTRIBUTEVALIDATION_SEQ.nextval,cur.G3E_SOURCEFNO,(SELECT G3E_ANO FROM G3E_ATTRIBUTE WHERE G3E_FIELD = 'FEATURE_STATE_C' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Common Attributes' AND G3E_NAME='COMMON_N')),cur.G3E_CONNECTINGFNO,(SELECT G3E_ANO FROM G3E_ATTRIBUTE WHERE G3E_FIELD = 'FEATURE_STATE_C' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Common Attributes' AND G3E_NAME='COMMON_N')),'RelatedNotEqualTo',1,null,null,'PPR;PPA;ABR;ABA;OSR;OSA;LIP','P1',SYSDATE,1,(SELECT G3E_TYPE FROM G3E_RELATIONSHIP WHERE G3E_USERNAME='Electrical Connectivity' AND G3E_TABLE='G3E_NODEEDGECONN_ELEC'));		
			
			gao_log.put_line('ATTRVAL_CONNECTIVITYCONFIG , ROW COUNT : '|| V_COUNT);
		EXCEPTION
			WHEN OTHERS THEN null;
			V_COUNT := 0;
			gao_log.put_line('ATTRVAL_CONNECTIVITYCONFIG', 'Terminated with errors: '||sqlerrm);
		end;	
	end loop;
end;
/
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2661);

