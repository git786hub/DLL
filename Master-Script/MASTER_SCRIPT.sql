UNDEFINE ENVIRONMENT_DEPLOYMENT_VARS

/*Define Named Substitution Varialbles: DEV*/
ACCEPT ENVIRONMENT_DEPLOYMENT_VARS CHAR PROMPT 'Enter path to the variables script for this environment' DEFAULT DEV_ENVIRONMENT_DEPLOYMENT_VARIABLES.sql
/*Define Named Substitution Varialbles: PTS*/
--ACCEPT ENVIRONMENT_DEPLOYMENT_VARS CHAR PROMPT 'Enter path to the variables script for this environment' DEFAULT PTS_ENVIRONMENT_DEPLOYMENT_VARIABLES.sql
/*Define Named Substitution Varialbles: TST*/
--ACCEPT ENVIRONMENT_DEPLOYMENT_VARS CHAR PROMPT 'Enter path to the variables script for this environment' DEFAULT TST_ENVIRONMENT_DEPLOYMENT_VARIABLES.sql
/*Define Named Substitution Varialbles: CRT*/
--ACCEPT ENVIRONMENT_DEPLOYMENT_VARS CHAR PROMPT 'Enter path to the variables script for this environment' DEFAULT CRT_ENVIRONMENT_DEPLOYMENT_VARIABLES.sql

@@&ENVIRONMENT_DEPLOYMENT_VARS
DEFINE
--
@@AEGIS_SYS_GENERALPARAMETER_Updates.sql
--@@AEGIS_GIS_INFA_ACL.sql