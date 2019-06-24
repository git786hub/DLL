--**************************************************************************************
-- SCRIPT NAME: G3E_JOB-DesignerIDAttributes.sql
--**************************************************************************************
-- AUTHOR						: Barry Scott
-- DATE							: 29-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Adds DESIGNER_RACFID and DESIGNER_NM to G3E_ATTRIBUTE
--                  : Redefines Designer Assignment attribute to use DESIGNER_RACFID
--**************************************************************************************

set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\G3E_JOB-DesignerIDAttributes.sql.log

declare
	cnt number;
begin
	select count(1) into cnt from g3e_attribute where g3e_cno=73 and g3e_field='DESIGNER_RACFID';
	
	if 0=cnt then
		update g3e_attribute
		set g3e_field='DESIGNER_RACFID'
		where g3e_cno=73 and g3e_field='DESIGNER_UID';
	end if;
	
	select count(1) into cnt from g3e_attribute where g3e_ano=7336 and g3e_cno=73 and g3e_field='DESIGNER_UID';

	if 0=cnt then
		insert into g3e_attribute
		columns(g3e_ano,g3e_cno,g3e_vno,g3e_field,g3e_username,g3e_format,g3e_precision,g3e_domaintable,g3e_domainfield,g3e_required,g3e_fino,g3e_functionalordinal,g3e_functionaltype,g3e_interfaceargument,g3e_tooltip,g3e_foreignkeytable,
			g3e_foreignkeyfield,g3e_hypertext,g3e_pno,g3e_copy,g3e_excludefromedit,g3e_datatype,g3e_additionalreffields,g3e_importfield,g3e_excludefromreplace,g3e_editdate,g3e_widthintwips,g3e_catalogcopyvalue,g3e_localecomment,g3e_breakcopy,g3e_copyattribute,
			g3e_wraptext,g3e_mergeexpression,g3e_rino,g3e_riarggroupno,g3e_unique,g3e_comprelativeano,g3e_functionalvalidation,g3e_role,g3e_fkqrino,g3e_fkqarggroupno,g3e_description)
		values(7336,73,NULL,'DESIGNER_UID','Designer UserID',NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,'Designer User ID',NULL,NULL,0,NULL,0,0,10,NULL,NULL,0,SYSDATE,NULL,NULL,NULL,0,0,1,NULL,NULL,NULL,0,NULL,1,'EVERYONE',NULL,NULL,NULL);
	end if;
	
	select count(1) into cnt from g3e_attribute where g3e_ano=7337 and g3e_cno=73 and g3e_field='DESIGNER_NM';

	if 0=cnt then
		insert into g3e_attribute
		columns(g3e_ano,g3e_cno,g3e_vno,g3e_field,g3e_username,g3e_format,g3e_precision,g3e_domaintable,g3e_domainfield,g3e_required,g3e_fino,g3e_functionalordinal,g3e_functionaltype,g3e_interfaceargument,g3e_tooltip,g3e_foreignkeytable,
			g3e_foreignkeyfield,g3e_hypertext,g3e_pno,g3e_copy,g3e_excludefromedit,g3e_datatype,g3e_additionalreffields,g3e_importfield,g3e_excludefromreplace,g3e_editdate,g3e_widthintwips,g3e_catalogcopyvalue,g3e_localecomment,g3e_breakcopy,g3e_copyattribute,
			g3e_wraptext,g3e_mergeexpression,g3e_rino,g3e_riarggroupno,g3e_unique,g3e_comprelativeano,g3e_functionalvalidation,g3e_role,g3e_fkqrino,g3e_fkqarggroupno,g3e_description)
		values(7337,73,NULL,'DESIGNER_NM','Designer Name',NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,'Designer User ID',NULL,NULL,0,NULL,0,0,10,NULL,NULL,0,SYSDATE,NULL,NULL,NULL,0,0,1,NULL,NULL,NULL,0,NULL,1,'EVERYONE',NULL,NULL,NULL);
	end if;
	
	commit;
end;		
/

spool off;
