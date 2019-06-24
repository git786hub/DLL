set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\VL_CU_ACTIVITY_DisplayNames.sql.log
--**************************************************************************************
--SCRIPT NAME: VL_CU_ACTIVITY_DisplayNames.sql
--**************************************************************************************
-- AUTHOR         : Rich Adase
-- DATE           : 03-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Update CU Activity value list to display codes instead of full words.
--                   This should reduce confusion for testing issues like ONCORDEV-1968.
--**************************************************************************************

update VL_CU_ACTIVITY set VL_VALUE = VL_KEY;


spool off;
