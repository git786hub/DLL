set echo on
set linesize 1000
set pagesize 300
set trimspool on
SET SERVEROUTPUT ON
SPOOL SAUTOMATION_UPDATES.LOG 

ALTER TABLE B$COMP_UNIT_N DISABLE ALL TRIGGERS;

DECLARE

  CURSOR C1
  IS
    WITH CU_MAP AS
        (SELECT   CT.CU_TYPE
          ,SA.OWNR_ID_H
          ,SA.FEAT_ID_H
          ,SA.WR_ID
          ,SA.MACRO_CU_C
          ,SA.CU_C
          ,SA.ACTIVITY_C
          ,SA.CIAC_C
          ,SA.WM_SEQ
          ,SA.VINTAGE_YR
          ,SA.PRIME_ACCT_ID
          ,SA.PROP_UNIT_ID
          ,SA.LENGTH_FLAG
          ,SA.QTY_LENGTH_Q
          FROM SAUTOMATION_MAPPING_TEMP SA
          INNER JOIN SAUTOMATION_CU_TYPE_TEMP CT
          ON SA.CU_C = CT.CU_C
        )
      SELECT
          SA.CU_TYPE
          ,
          CASE
            WHEN SA.CU_TYPE = 'CESBATANCILARY' THEN CS.FEAT_ID_H
            ELSE SA.OWNR_ID_H
          END FEAT_ID_H
        ,SA.WR_ID
        ,SA.MACRO_CU_C
        ,SA.CU_C
        ,SA.ACTIVITY_C
        ,SA.CIAC_C
        ,SA.WM_SEQ
        ,SA.VINTAGE_YR
        ,SA.PRIME_ACCT_ID
        ,SA.PROP_UNIT_ID
        ,SA.LENGTH_FLAG
        ,SA.QTY_LENGTH_Q
        FROM CU_MAP SA
        LEFT JOIN CU_MAP CS
        ON SA.OWNR_ID_H   = CS.OWNR_ID_H
          AND CS.CU_TYPE  = 'CESBAT'
        WHERE 1           = 1
          AND SA.CU_TYPE IN ('ANCILARY','CESBATANCILARY') ;

  V_COMP_UNIT_ROW B$COMP_UNIT_N%ROWTYPE;
  V_COMP_UNIT_ROW_COUNT NUMBER         := 0;
  V_COMP_UNIT_ERROR_COUNT NUMBER         := 0;
  V_COMP_UNIT_DUP_COUNT NUMBER         := 0;
  V_F2G_SQL_STR         VARCHAR2 (200) := 'SELECT DISTINCT M.FEAT_ID_H ,M.G3E_FID ,M.G3E_FNO FROM GIS.F2G_MAP M WHERE M.FEAT_ID_H = :FEAT_ID';
  V_DUP_COUNT_STR       varchar2(500) := 'select count(*) ROW_COUNT '
                                          || 'from b$comp_unit_n cu '
                                          || 'where 1=1 '
                                          || 'and cu.G3E_FNO = :G3E_FNO '
                                          || 'and cu.G3E_CNO = :G3E_CNO '
                                          || 'and cu.G3E_FID = :G3E_FID '
                                          || 'and cu.WR_ID = :WR_ID '
                                          || 'and cu.CU_C = :CU_C '
                                          || 'and cu.MACRO_CU_C = :MACRO_CU_C '
                                          || 'and cu.ACTIVITY_C = :ACTIVITY_C '
                                          || 'and cu.CIAC_C = :CIAC_C '
                                          || 'and cu.VINTAGE_YR = :VINTAGE_YR '
                                          || 'and cu.PRIME_ACCT_ID = :PRIME_ACCT_ID '
                                          || 'and cu.PROP_UNIT_ID = :PROP_UNIT_ID '
                                          || 'and cu.WM_SEQ = :WM_SEQ '
                                          ;
  V_CU_CID_SQL_STR      VARCHAR2 (200) := 'SELECT COUNT ( *) + 1 ROW_COUNT' 
                                        ||' FROM B$COMP_UNIT_N' 
                                        || ' WHERE 1 = 1 AND G3E_FID = :G3E_FID AND G3E_FNO = :G3E_FNO AND LTT_ID = 0 AND G3E_CNO = 22';
  procedure LOG_COMP_UNIT_ROW (COMP_UNIT_ROW GIS.B$COMP_UNIT_N%ROWTYPE)
  is 
  begin
    DBMS_OUTPUT.PUT ('   COMP_UNIT_ROW -  ') ;
    DBMS_OUTPUT.PUT ('G3E_ID:' ||TO_CHAR (COMP_UNIT_ROW.G3E_ID) || '|') ;
    DBMS_OUTPUT.PUT ('G3E_FNO:' ||TO_CHAR (COMP_UNIT_ROW.G3E_FNO) || '|') ;
    DBMS_OUTPUT.PUT ('G3E_CNO:' ||TO_CHAR (COMP_UNIT_ROW.G3E_CNO) || '|') ;
    DBMS_OUTPUT.PUT ('G3E_FID:' ||TO_CHAR (COMP_UNIT_ROW.G3E_FID) || '|') ;
    DBMS_OUTPUT.PUT ('G3E_CID:' ||TO_CHAR (COMP_UNIT_ROW.G3E_CID) || '|') ;
    DBMS_OUTPUT.PUT ('WR_ID:' ||TO_CHAR (COMP_UNIT_ROW.WR_ID) || '|') ;
    DBMS_OUTPUT.PUT ('CU_C:' ||TO_CHAR (COMP_UNIT_ROW.CU_C) || '|') ;
    DBMS_OUTPUT.PUT ('MACRO_CU_C:' ||TO_CHAR (COMP_UNIT_ROW.MACRO_CU_C) || '|') ;
    DBMS_OUTPUT.PUT ('ACTIVITY_C:' ||TO_CHAR (COMP_UNIT_ROW.ACTIVITY_C) || '|') ;
    DBMS_OUTPUT.PUT ('CIAC_C:' ||TO_CHAR (COMP_UNIT_ROW.CIAC_C) || '|') ;
    DBMS_OUTPUT.PUT ('LENGTH_FLAG:' ||TO_CHAR (COMP_UNIT_ROW.LENGTH_FLAG) || '|') ;
    DBMS_OUTPUT.PUT ('QTY_LENGTH_Q:' ||TO_CHAR (COMP_UNIT_ROW.QTY_LENGTH_Q) || '|') ;
    DBMS_OUTPUT.PUT ('UNIT_CNO:' ||TO_CHAR (COMP_UNIT_ROW.UNIT_CNO) || '|') ;
    DBMS_OUTPUT.PUT ('UNIT_CID:' ||TO_CHAR (COMP_UNIT_ROW.UNIT_CID) || '|') ;
    DBMS_OUTPUT.PUT ('LTT_ID:' ||TO_CHAR (COMP_UNIT_ROW.LTT_ID) || '|') ;
    DBMS_OUTPUT.PUT ('LTT_STATUS:' ||TO_CHAR (COMP_UNIT_ROW.LTT_STATUS) || '|') ;
    DBMS_OUTPUT.PUT ('LTT_DATE:' ||TO_CHAR (COMP_UNIT_ROW.LTT_DATE) || '|') ;
    DBMS_OUTPUT.PUT ('LTT_TID:' ||TO_CHAR (COMP_UNIT_ROW.LTT_TID) || '|') ;
    DBMS_OUTPUT.PUT ('VINTAGE_YR:' ||TO_CHAR (COMP_UNIT_ROW.VINTAGE_YR) || '|') ;
    DBMS_OUTPUT.PUT ('PRIME_ACCT_ID:' ||TO_CHAR (COMP_UNIT_ROW.PRIME_ACCT_ID) || '|') ;
    DBMS_OUTPUT.PUT ('PROP_UNIT_ID:' ||TO_CHAR (COMP_UNIT_ROW.PROP_UNIT_ID) || '|') ;
    DBMS_OUTPUT.PUT ('CU_DESC:' ||TO_CHAR (COMP_UNIT_ROW.CU_DESC) || '|') ;
    DBMS_OUTPUT.PUT ('WM_SEQ:' ||TO_CHAR (COMP_UNIT_ROW.WM_SEQ) || '|') ;
    DBMS_OUTPUT.PUT ('RETIREMENT_C:' ||TO_CHAR (COMP_UNIT_ROW.RETIREMENT_C) || '|') ;
    DBMS_OUTPUT.PUT ('GLG_ACCT_H:' ||TO_CHAR (COMP_UNIT_ROW.GLG_ACCT_H) || '|') ;
    DBMS_OUTPUT.PUT ('WR_EDITED:' ||TO_CHAR (COMP_UNIT_ROW.WR_EDITED) || '|') ;
    DBMS_OUTPUT.PUT ('ACU_ANO:' ||TO_CHAR (COMP_UNIT_ROW.ACU_ANO) || '|') ;
    DBMS_OUTPUT.PUT_LINE ('FIELD_DESIGN_XML:' ||TO_CHAR (COMP_UNIT_ROW.FIELD_DESIGN_XML)) ;
  end LOG_COMP_UNIT_ROW;
 
BEGIN

  FOR SAUTO IN C1
  LOOP
    --DBMS_OUTPUT.PUT_LINE (TO_CHAR (SAUTO.CU_TYPE) ||'|FEAT_ID_H:'||TO_CHAR (SAUTO.FEAT_ID_H) ||'|CU_C:' || SAUTO.CU_C) ;
    V_COMP_UNIT_ROW := NULL;

    DECLARE
      V_FEAT_ID_H  NUMBER;
      V_COMP_COUNT NUMBER;
      V_DUP_COUNT NUMBER;
    BEGIN
      EXECUTE IMMEDIATE V_F2G_SQL_STR 
                        INTO V_FEAT_ID_H, V_COMP_UNIT_ROW.G3E_FID, V_COMP_UNIT_ROW.G3E_FNO 
                        USING SAUTO.FEAT_ID_H;
      --DBMS_OUTPUT.PUT ('  F2G_MAP -  G3E_FID:' || V_COMP_UNIT_ROW.G3E_FID || '|G3E_FNO|' || V_COMP_UNIT_ROW.G3E_FNO) ;

      V_COMP_UNIT_ROW.G3E_CNO       := 22;
      V_COMP_UNIT_ROW.WR_ID         := SAUTO.WR_ID;
      V_COMP_UNIT_ROW.CU_C          := SAUTO.CU_C;
      V_COMP_UNIT_ROW.MACRO_CU_C    := SAUTO.MACRO_CU_C;
      V_COMP_UNIT_ROW.ACTIVITY_C    := SAUTO.ACTIVITY_C;
      V_COMP_UNIT_ROW.CIAC_C        := SAUTO.CIAC_C;
      V_COMP_UNIT_ROW.LENGTH_FLAG   := SAUTO.LENGTH_FLAG;
      V_COMP_UNIT_ROW.QTY_LENGTH_Q  := SAUTO.QTY_LENGTH_Q;
      V_COMP_UNIT_ROW.LTT_ID        := 0;
      V_COMP_UNIT_ROW.LTT_TID       := NULL;
      V_COMP_UNIT_ROW.VINTAGE_YR    := SAUTO.VINTAGE_YR;
      V_COMP_UNIT_ROW.PRIME_ACCT_ID := SAUTO.PRIME_ACCT_ID;
      V_COMP_UNIT_ROW.PROP_UNIT_ID  := SAUTO.PROP_UNIT_ID;
      V_COMP_UNIT_ROW.WM_SEQ        := SAUTO.WM_SEQ;
      
      --DUPLICATE?
      BEGIN
        EXECUTE IMMEDIATE V_DUP_COUNT_STR 
                        INTO V_DUP_COUNT 
                        USING V_COMP_UNIT_ROW.G3E_FNO,
                        V_COMP_UNIT_ROW.G3E_CNO,
                        V_COMP_UNIT_ROW.G3E_FID,
                        V_COMP_UNIT_ROW.WR_ID,
                        V_COMP_UNIT_ROW.CU_C,
                        V_COMP_UNIT_ROW.MACRO_CU_C,
                        V_COMP_UNIT_ROW.ACTIVITY_C,
                        V_COMP_UNIT_ROW.CIAC_C,
                        V_COMP_UNIT_ROW.VINTAGE_YR,
                        V_COMP_UNIT_ROW.PRIME_ACCT_ID,
                        V_COMP_UNIT_ROW.PROP_UNIT_ID,
                        V_COMP_UNIT_ROW.WM_SEQ;
        
        IF V_DUP_COUNT > 0 THEN
                V_COMP_UNIT_DUP_COUNT := V_COMP_UNIT_DUP_COUNT + 1;
                LOG_COMP_UNIT_ROW(V_COMP_UNIT_ROW);
                DBMS_OUTPUT.PUT_LINE ('##DUPLICATE: Found ' || TO_CHAR (V_DUP_COUNT) || ' existing in b$comp_unit_n' ) ;
          CONTINUE;
        END IF;

      EXCEPTION
        WHEN OTHERS THEN
          LOG_COMP_UNIT_ROW(V_COMP_UNIT_ROW);
          DBMS_OUTPUT.PUT_LINE ('##ERROR:  Error checking for duplicate records in COMP_UNIT_N') ;
          Continue;
      END;      
      
      -- GET CID
      BEGIN
        EXECUTE IMMEDIATE V_CU_CID_SQL_STR 
                        INTO V_COMP_UNIT_ROW.G3E_CID 
                        USING V_COMP_UNIT_ROW.G3E_FID,  V_COMP_UNIT_ROW.G3E_FNO;
        --DBMS_OUTPUT.PUT_LINE ('|CID:' || TO_CHAR (V_COMP_UNIT_ROW.G3E_CID)) ;

      EXCEPTION

      WHEN OTHERS THEN
        V_COMP_UNIT_ERROR_COUNT := V_COMP_UNIT_ERROR_COUNT + 1;
        LOG_COMP_UNIT_ROW(V_COMP_UNIT_ROW);
        DBMS_OUTPUT.PUT_LINE ('##ERROR:  Error retrieving CID from COMP_UNIT_N') ;
        Continue;

      END;
      
      -- Set G3E_ID
      V_COMP_UNIT_ROW.G3E_ID := B$COMP_UNIT_N_PKG.GETSEQUENCEVALUE;
      

      --LOG_COMP_UNIT_ROW(V_COMP_UNIT_ROW);
 

      INSERT INTO B$COMP_UNIT_N VALUES V_COMP_UNIT_ROW;

      V_COMP_UNIT_ROW_COUNT := V_COMP_UNIT_ROW_COUNT + 1;

    EXCEPTION

    WHEN TOO_MANY_ROWS THEN
        V_COMP_UNIT_ERROR_COUNT := V_COMP_UNIT_ERROR_COUNT + 1;
      DBMS_OUTPUT.PUT_LINE ('##ERROR     Multiple FID values returned from F2G_MAP for FEAT_ID_H - SQL: ' || V_F2G_SQL_STR) ;

    WHEN NO_DATA_FOUND THEN
        V_COMP_UNIT_ERROR_COUNT := V_COMP_UNIT_ERROR_COUNT + 1;
      DBMS_OUTPUT.PUT_LINE ('##ERROR     No values returned from F2G_MAP') ;-- for FEAT_ID_H - SQL: ' || V_F2G_SQL_STR) ;

    WHEN OTHERS THEN
        V_COMP_UNIT_ERROR_COUNT := V_COMP_UNIT_ERROR_COUNT + 1;
      DBMS_OUTPUT.PUT_LINE ('  ##ERROR CODE ' || SQLCODE || ' : ' || SUBSTR (SQLERRM,1,64) || ' ') ;
      DBMS_OUTPUT.PUT ('  ' || DBMS_UTILITY.FORMAT_ERROR_STACK) ;
      DBMS_OUTPUT.PUT ('  ' || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE) ;
    END;

  END LOOP;
  DBMS_OUTPUT.PUT_LINE ('----------------Duplicates: ' || TO_CHAR (V_COMP_UNIT_DUP_COUNT)) ;
  DBMS_OUTPUT.PUT_LINE ('----------------Errors: ' || TO_CHAR (V_COMP_UNIT_ERROR_COUNT)) ;
  DBMS_OUTPUT.PUT_LINE ('----------------Rows Inserted: ' || TO_CHAR (V_COMP_UNIT_ROW_COUNT)) ;
  COMMIT;

END;

/

ALTER TABLE B$COMP_UNIT_N ENABLE ALL TRIGGERS;

spool off;
