set echo on
set feedback on

spool GIS_STG_Deployment_Script.log

--Drop Script
@Master-Script/GIS_STG_Drop_Script_11.29.sql

--Tables
@GIS-ONC-T811-DDL/STG_TX811.sql

--Triggers
@GIS-ONC-T811-DDL/BIU_STG_TX811_TR.sql