set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\CleanUpSynonymsAndTables.log
--**************************************************************************************
--SCRIPT NAME: CleanUpSynonymsAndTables.sql
--**************************************************************************************
-- AUTHOR         : Rich Adase
-- DATE           : 09-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Cleans up obsolete synonyms and tables
--**************************************************************************************

-- Clean up obsolete picklist table for Field Check Status

drop public synonym VL_FIELD_CHECK_STATUS;
delete from GIS.G3E_PICKLIST where G3E_TABLE = 'VL_FIELD_CHECK_STATUS';

-- Clean up WMIS reference tables from GIS schema (since they are now in GIS_ONC)
--  and delete obsolete picklist metadata

drop table GIS.REFWMIS_COUNTY_CODE;
drop table GIS.REFWMIS_FERC_ACCOUNT;
drop table GIS.REFWMIS_POWER_COMPANY;
drop table GIS.REFWMIS_TOWN_CODE;
drop table GIS.REFWMIS_VOUCHER_TYPE;
delete from GIS.G3E_PICKLIST where G3E_TABLE like 'REFWMIS_%';


spool off;
