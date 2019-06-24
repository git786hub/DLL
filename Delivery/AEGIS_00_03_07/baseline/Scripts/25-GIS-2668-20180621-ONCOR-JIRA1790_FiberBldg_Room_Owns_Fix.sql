
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2668, 'ONCOR-JIRA1790_FiberBldg_Room_Owns_fix');
spool c:\temp\2668-20180621-ONCOR-JIRA1790_FiberBldg_Room_Owns_Fix.log
--**************************************************************************************
--SCRIPT NAME: 2668-20180621-ONCOR-JIRA1790_FiberBldg_Room_Owns_fix.sql
--**************************************************************************************
-- AUTHOR			: INGRNET\PVKURELL
-- DATE				: 21-JUN-2018
-- CYCLE			: 00.03.07
-- JIRA NUMBER		: 1790
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Modifed view to rename REF_ROOM_TYPES to VL_ROOM_TYPES to fix jira 1790
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************


CREATE OR REPLACE FORCE EDITIONABLE VIEW GC_ROOM_I(FEATURE_TYPE,NAME,G3E_FID) AS 
  SELECT
A.FEATURE_TYPE,
A.NAME,
A.G3E_FID FROM
GC_ROOM A,
(select DISTINCT(FEATURE_TYPE_ENG) FROM VL_ROOM_TYPES) B
WHERE
A.FEATURE_TYPE=B.FEATURE_TYPE_ENG(+);
 
create public synonym GC_ROOM_I for GC_ROOM_I;
Grant SELECT on GC_ROOM_I to DESIGNER;
Grant SELECT on GC_ROOM_I to SUPERVISOR;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2668);


