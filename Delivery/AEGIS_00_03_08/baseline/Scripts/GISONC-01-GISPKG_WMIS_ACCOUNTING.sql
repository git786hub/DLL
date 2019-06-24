set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool %TEMP%\GISPKG_WMIS_ACCOUNTING.log
--**************************************************************************************
--SCRIPT NAME: GISPKG_WMIS_ACCOUNTING.sql
--**************************************************************************************
-- AUTHOR           : SAGARWAL
-- DATE             : 01-MAR-2018
-- PRJ IDENTIFIER   : G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     : Script to create package GISPKG_WMIS_ACCOUNTING in GIS_ONC schema
--**************************************************************************************
-- Modified:
--  31-JUL-2018, Rich Adase -- Updated with revised source provided by Matthew Inman
--**************************************************************************************


create or replace package       GIS_ONC.GISPKG_WMIS_ACCOUNTING
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to import Vintage Year data.
-- History:
--     22-FEB-2018, v0.1    Hexagon, Initial creation
-- ---------------------------------------------------------------------------------------------------------------------------------
as
    -- -----------------------------------------------------------------------------------------------------------------------------
  -- This procedure imports Vintage Year data as delivered to a staging table from WMIS on a periodic basis.
  -- When the staging table is loaded, the loading process calls this stored procedure to perform the import.
  -- Vintage Year data is used to update the GIS job table, setting the Vintage Year job property for each affected WR
  -- For WRs that have not yet been closed, this data will be used later during Closure batch processing.
  -- For WRs that have already been closed,  the stored procedure immediately updates Vintage Year for all included CUs;
  -- this requires that the Oracle user calling the stored procedure has been granted the SHORTTERMTRANSACTIONS role,
  -- which allows updates directly to master records, without requiring an active job or subsequent posting
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure ImportVintageYearData(RUN_ID in number);

end;
/

create or replace package body  GIS_ONC.GISPKG_WMIS_ACCOUNTING
  -- ---------------------------------------------------------------------------------------------------------------------------------
  -- Description:
  --     Package to support the import of Vintage Year data import in GIS.
  -- History:
  --     04-DEC-2017, v0.1    Hexagon, Initial creation
  -- ---------------------------------------------------------------------------------------------------------------------------------
as
  -- -----------------------------------------------------------------------------------------------------------------------------
  -- Public procedure, see package specification
  -- -----------------------------------------------------------------------------------------------------------------------------
  procedure ImportVintageYearData(RUN_ID in number)
  as
  
    V_SQLSTATEMENT    VARCHAR2(4000);
    V_JOBSTATUS       G3E_JOB.G3E_JOBSTATUS%TYPE;
    V_CONFIGURATION   G3E_GENERALPARAMETER_OPTABLE.G3E_VALUE%TYPE;
  
  BEGIN
  
    DBMS_OUTPUT.PUT_LINE ('ImportVintageYearData started');
    
    SELECT  G3E_VALUE
      INTO V_CONFIGURATION
      FROM G3E_GENERALPARAMETER_OPTABLE
      WHERE G3E_NAME = 'AllConfigurations';
    
    LTT_USER.SETCONFIGURATION(V_CONFIGURATION);
    
    FOR CUR IN
    (SELECT  WR_NO
      ,VINTAGE_YEAR
      FROM STG_VINTAGE_YEAR
    ) 
    LOOP
  
      DBMS_OUTPUT.PUT_LINE('WR: '|| CUR.WR_NO || ' Y: '|| CUR.VINTAGE_YEAR);
  
      BEGIN

        UPDATE G3E_JOB
          SET VINTAGE_YR = CUR.VINTAGE_YEAR
          WHERE WR_NBR = CUR.WR_NO;
          
        SELECT  G3E_JOBSTATUS
          INTO V_JOBSTATUS
          FROM G3E_JOB
          WHERE WR_NBR = CUR.WR_NO;
        
        DBMS_OUTPUT.PUT(' v_jobStatus: '|| V_JOBSTATUS);
      
        IF (V_JOBSTATUS = 'Closed') THEN
        
          BEGIN
          
            UPDATE COMP_UNIT_N
              SET VINTAGE_YR = CUR.VINTAGE_YEAR
              WHERE WR_ID = TO_CHAR(CUR.WR_NO);
            UPDATE STG_VINTAGE_YEAR
              SET STATUS = 'SUCCESS'
              WHERE WR_NO = CUR.WR_NO;
          
            COMMIT;
          
          EXCEPTION
            WHEN OTHERS THEN --Failure has occurred during the update of CU records so set the status as FAILURE

              ROLLBACK;
              
              DBMS_OUTPUT.PUT_LINE('WR'|| CUR.WR_NO ||' Not found. Error Msg:'|| SQLERRM);
              UPDATE STG_VINTAGE_YEAR
                SET STATUS = 'FAILURE'
                WHERE WR_NO = CUR.WR_NO;
          
              COMMIT;
          
          END;
          
        ELSE -- Job is not yet closed, do not update CU records and set the status to pending in staging table
      
          UPDATE STG_VINTAGE_YEAR
            SET STATUS = 'PENDING'
            WHERE WR_NO = CUR.WR_NO;
          
          COMMIT;
      
        END IF;
      
      EXCEPTION
        WHEN NO_DATA_FOUND THEN

          DBMS_OUTPUT.PUT_LINE('WR'|| CUR.WR_NO ||' Not found. Error Msg:'|| SQLERRM);
          UPDATE STG_VINTAGE_YEAR
            SET STATUS = 'FAILURE'
            WHERE WR_NO = CUR.WR_NO;
          
          COMMIT;
          
      END;
  
    END LOOP;
  
    DBMS_OUTPUT.PUT_LINE ('ImportVintageYearData ended');
  
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      RAISE_APPLICATION_ERROR( - 20001,'GISPKG_WMIS_ACCOUNTING.ImportVintageYearData: ' || SQLERRM);
  END IMPORTVINTAGEYEARDATA;
END;
/


spool off;
