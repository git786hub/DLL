--
-- Run All Scripts
-- v00.03.07 
--
-- NOTE: RUN CONNECTED AS USER SYS (as SYSDBA)
--

@@01-SYS-SysAccess_PublicSynonyms.SQL
@@02-SYS-SysAccess_GrantToRolesAndSchemas.sql

commit;
