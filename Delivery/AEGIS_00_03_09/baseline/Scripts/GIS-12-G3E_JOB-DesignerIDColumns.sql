--**************************************************************************************
-- SCRIPT NAME: G3E_JOB-DesignerIDColumns.sql
--**************************************************************************************
-- AUTHOR						: Barry Scott
-- DATE							: 29-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Adds DESIGNER_RACFID and DESIGNER_NM to G3E_JOB
--**************************************************************************************

set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\G3E_JOB-DesignerIDColumns.sql.log

declare
	cnt number;
begin
	select count(1) into cnt from all_tab_columns where owner='GIS' and table_name='G3E_JOB' and column_name='DESIGNER_RACFID'; 
	if cnt < 1 then
		execute immediate ('ALTER TABLE GIS.G3E_JOB ADD DESIGNER_RACFID VARCHAR2(30)');
	end if;
	
	select count(1) into cnt from all_tab_columns where owner='GIS' and table_name='G3E_JOB' and column_name='DESIGNER_NM'; 
	if cnt < 1 then
		execute immediate ('ALTER TABLE GIS.G3E_JOB ADD DESIGNER_NM VARCHAR2(30)');
	end if;
end;
/

spool off;