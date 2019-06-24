set echo on
set linesize 1000
set pagesize 300
set trimspool on
SPOOL SAUTOMATIONTABLE.LOG

CREATE TABLE SAUTOMATION_MAPPING_TEMP
    (
      OWNR_ID_H    NUMBER (10,0)
    ,FEAT_ID_H     NUMBER (10,0)
    ,WR_ID         VARCHAR2 (30 BYTE)
    ,MACRO_CU_C    VARCHAR2 (30 BYTE)
    ,CU_C          VARCHAR2 (30 BYTE)
    ,ACTIVITY_C    VARCHAR2 (10 BYTE)
    ,CIAC_C        VARCHAR2 (1 BYTE)
    ,WM_SEQ        VARCHAR2 (12 BYTE)
    ,VINTAGE_YR    NUMBER (4,0)
    ,PRIME_ACCT_ID NUMBER (5,0)
    ,PROP_UNIT_ID  NUMBER (5,0)
    ,LENGTH_FLAG   VARCHAR2 (1 BYTE)
    ,QTY_LENGTH_Q  NUMBER (5,0)
    ) ;
    
CREATE TABLE SAUTOMATION_CU_TYPE_TEMP
    (
    CU_C          VARCHAR2 (30 BYTE)
    ,CU_TYPE    VARCHAR2 (30 BYTE)
    ) ;    
    

spool off;