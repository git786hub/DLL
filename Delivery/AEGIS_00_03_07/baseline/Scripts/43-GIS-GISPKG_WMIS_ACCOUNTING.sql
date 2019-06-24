
set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\GISPKG_WMIS_ACCOUNTING.log
--**************************************************************************************
--SCRIPT NAME: GISPKG_WMIS_ACCOUNTING.sql
--**************************************************************************************
-- AUTHOR			: SAGARWAL
-- DATE				: 01-MAR-2018
-- CYCLE			: 6

-- JIRA NUMBER		: ONCORDEV-1333
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Script to create package GISPKG_WMIS_ACCOUNTING in GIS_ONC schema
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

-- {Add script code}

create or replace 
package GIS_ONC.GISPKG_WMIS_ACCOUNTING
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
    procedure ImportVintageYearData   ;        
   
end;
/

create or replace 
package body  GIS_ONC.GISPKG_WMIS_ACCOUNTING
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
    
procedure ImportVintageYearData  
as
  v_sqlStatement VARCHAR2(4000);
  v_jobStatus G3E_JOB.G3E_JOBSTATUS%TYPE;
  v_Configuration G3E_GENERALPARAMETER_OPTABLE.G3E_VALUE%TYPE;    
	  
    begin    
      dbms_output.put_line ('ImportVintageYearData started');        
      
	   select g3e_value into v_Configuration
                         from G3E_GENERALPARAMETER_OPTABLE 
                         where g3e_name = 'AllConfigurations';
                         
        LTT_USER.SetConfiguration(v_Configuration);
		
      for cur in (select  WR_NO, VINTAGE_YEAR from STG_VINTAGE_YEAR) loop
	  
        update g3e_job set VINTAGE_YR = cur.VINTAGE_YEAR where WR_NBR = cur.WR_NO;        
        select G3E_JOBSTATUS into v_jobStatus from g3e_job where WR_NBR = cur.WR_NO;
        
        if (v_jobStatus = 'Closed') then        
          begin     		  
            update COMP_UNIT_N set VINTAGE_YR = cur.VINTAGE_YEAR where WR_ID = TO_CHAR(cur.WR_NO);            
            update STG_VINTAGE_YEAR set status = 'SUCCESS' where WR_NO = cur.WR_NO;
            commit;			
          exception when others then --Failure has occurred during the update of CU records so set the status as FAILURE
			rollback;
            update STG_VINTAGE_YEAR set status = 'FAILURE' where WR_NO = cur.WR_NO;
            commit;
          end;        
		  
        else -- Job is not yet closed, do not update CU records and set the status to pending in staging table
			update STG_VINTAGE_YEAR set status = 'PENDING' where WR_NO = cur.WR_NO;
            commit;
        end if;
          
     end loop;        
        
        dbms_output.put_line ('ImportVintageYearData ended');    
    exception
        when others then
          rollback;
          raise_application_error(-20001, 'GISPKG_WMIS_ACCOUNTING.ImportVintageYearData: ' || SQLERRM);
    end ImportVintageYearData;
end;
/

create or replace public synonym GISPKG_WMIS_ACCOUNTING for GIS_ONC.GISPKG_WMIS_ACCOUNTING;
grant execute on GISPKG_WMIS_ACCOUNTING to administrator;
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
