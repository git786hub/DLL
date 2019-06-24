set define on
column dcol new_value SDATE noprint
select to_char(sysdate,'YYYYMMDD"T"HH24MI"+"SS') dcol from dual;
spool MASTER_CLONE_&SDATE..log

UNDEFINE ENVIRONMENT_SPECIFIC_VARS
UNDEFINE ENV_REFRESH_SOURCE
UNDEFINE ENV_REFRESH_TARGET


ACCEPT ENV_REFRESH_SOURCE CHAR PROMPT 'Enter up to four (4) letter abbreviation for environment source. Ex: DEV or TST1'
ACCEPT ENV_REFRESH_TARGET CHAR PROMPT 'Enter up to four (4) letter abbreviation for environment target. Ex: PTS or CRT'

column myaliasENVVARS new_value ENVIRONMENT_SPECIFIC_VARS
select '&ENV_REFRESH_TARGET._ENVIRONMENT_DEPLOYMENT_VARIABLES.sql' myaliasENVVARS from dual;

@@Env_Specific_Variables/&ENVIRONMENT_SPECIFIC_VARS
@@Actions/STD_ENVIRONMENT_VARIABLES.sql
@@Actions/Update_SYS_GENERALPARAMETER.sql
@@Actions/Update_G3E_CONFIGURATIONS.sql
@@Actions/Update_G3E_GENERALPARAMETER.sql
@@Actions/Update_G3E_DATACONNECTION.sql
@@Actions/Alter_Users.sql
@@Actions/Restart_G3E_FID_SEQ.sql
@@Actions/Truncate_Pending_Edits.sql
--@@Actions/Restart_CORRELATION_ID_SEQ.sql

COMMIT;
