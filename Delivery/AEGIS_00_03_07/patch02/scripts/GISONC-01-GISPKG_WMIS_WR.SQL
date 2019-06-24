
set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\GISPKG_WMIS_WR.log
--**************************************************************************************
--SCRIPT NAME: GISPKG_WMIS_WR.sql
--**************************************************************************************
-- AUTHOR         : SAGARWAL
-- DATE           : 26-FEB-2018
-- PRJ IDENTIFIER : G/TECHNOLOGY - ONCOR
-- PROGRAM DESC   : script to generate package GISPKG_WMIS_WR in GIS_ONC schema
--**************************************************************************************

create or replace 
package   GIS_ONC.GISPKG_WMIS_WR
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to support WMIS interface .
-- History:
--     22-FEB-2018, v0.1    Hexagon, Initial creation
-- ---------------------------------------------------------------------------------------------------------------------------------
as
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This stored procedure is called when request is made to the GIS_CreateUpdateJob web service. 
    -- This stored procedure is used to create an entry in the G3E_JOB table is WR does not already exists based on the input parameter
    -- This stored procedure is used to update the entry in the G3E_JOB table if an entry corresponding to the WR already exists based on the input paramter
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure CreateUpdateJob
    (
      p_WR_Number in	NUMBER,                 --G3E_JOB.G3E_IDENTIFIER and G3E_JOB.WR_NBR
      p_WR_Creation_Date in	DATE,             --G3E_JOB.G3E_DATECREATED
      p_WR_Name in	VARCHAR2,                 --G3E_JOB.G3E_IDENTIFIER
      p_WR_Type in	VARCHAR2,                 --G3E_JOB.WR_TYPE_C
      p_Designer_Assignment	 in VARCHAR2,     --G3E_JOB.DESIGNER_UID
      p_WR_Status in	VARCHAR2,               --G3E_JOB.WR_STATUS_C
      p_Customer_Required_Date in	DATE,       --G3E_JOB.WR_CUSTOMER_REQD_D
      p_Construction_Ready_Date in	DATE,     --G3E_JOB.WR_CONSTRUCT_READY_D
      p_Street_Number in	VARCHAR2,           --G3E_JOB.WR_HOUSE_NBR
      p_Street_Name in	VARCHAR2,             --G3E_JOB.WR_STREET_NM
      p_Town in	NUMBER,                       --G3E_JOB.WR_TOWN_C
      p_County in	NUMBER,                     --G3E_JOB.WR_COUNTY_C
      p_Crew_Headquarters in	VARCHAR2,       --G3E_JOB.WR_CREW_HQ_C
      p_Management_Activity_Code in	VARCHAR2  --G3E_JOB.WR_MGMT_ACTIVITY_C
    );
         
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This stored procedure is called when request is made to GIS_UpdateWritebackStatus web service, or a response from the WMIS_WriteBack. 
    -- This stored procedure updaes the G3E_JOB.WMIS_STATUS_C based on the inout p_Writeback_Status
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure UpdateWritebackStatus
    (
        p_WR_Number in	NUMBER, --G3E_JOB.WR_NBR
        p_Writeback_Status in	VARCHAR2 --G3E_JOB.WMIS_STATUS_C
    );    
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This stored procedure is called when request is made to GIS_RequestBatch web service. 
    -- This stored procedure updaes the G3E_JOB.WMIS_STATUS_C to "NEW".
    -- This stored procedure adds a record to the GIS_ONC.GISAUTO_QUEUE table to queue the batch request.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure RequestBatch( 
                           p_WR_Number in	NUMBER,
                           p_TransId in varchar2,
                           p_xml in clob
                          );
   
end;
/

create or replace 
package body GIS_ONC.GISPKG_WMIS_WR
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to support the GIS interface with WMIS.
-- History:
--     26-FEB-2018, v0.1    Hexagon, Initial creation
--     24-JUL-2018, Rich Adase -- Fixed update in CreateUpdateJob so that it does not overwrite G3E_JOBTYPE (was incorrectly updating with p_WR_Type)
-- ---------------------------------------------------------------------------------------------------------------------------------
as
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
     procedure CreateUpdateJob
    (
      p_WR_Number in	NUMBER,                 --G3E_JOB.G3E_IDENTIFIER and G3E_JOB.WR_NBR
      p_WR_Creation_Date in	DATE,             --G3E_JOB.G3E_DATECREATED
      p_WR_Name in	VARCHAR2,                 --G3E_JOB.G3E_DESCRIPTION
      p_WR_Type in	VARCHAR2,                 --G3E_JOB.WR_TYPE_C
      p_Designer_Assignment	 in VARCHAR2,     --G3E_JOB.DESIGNER_UID
      p_WR_Status in	VARCHAR2,               --G3E_JOB.WR_STATUS_C
      p_Customer_Required_Date in	DATE,       --G3E_JOB.WR_CUSTOMER_REQD_D
      p_Construction_Ready_Date in	DATE,     --G3E_JOB.WR_CONSTRUCT_READY_D
      p_Street_Number in	VARCHAR2,           --G3E_JOB.WR_HOUSE_NBR
      p_Street_Name in	VARCHAR2,             --G3E_JOB.WR_STREET_NM
      p_Town in	NUMBER,                       --G3E_JOB.WR_TOWN_C
      p_County in	NUMBER,                     --G3E_JOB.WR_COUNTY_C
      p_Crew_Headquarters in	VARCHAR2,       --G3E_JOB.WR_CREW_HQ_C
      p_Management_Activity_Code in	VARCHAR2  --G3E_JOB.WR_MGMT_ACTIVITY_C
    )
    As
       v_WRCheck        NUMBER(1);
       v_JobType G3E_JOB.G3E_JOBTYPE%TYPE;
       v_JobStatus G3E_JOB.G3E_JOBSTATUS%TYPE;   
	     v_Configuration G3E_GENERALPARAMETER_OPTABLE.G3E_VALUE%TYPE;
	   
    begin    
        
      dbms_output.put_line ('CreateUpdateJob started');  
        
      v_WRCheck:=0;
      v_JobType:='WR-DESIGN'; --Default setting of Job Type
        
      select count(*) into v_WRCheck from g3e_job where WR_NBR = p_WR_Number;
         
      if (v_WRCheck = 0) then --Need to create job        
        case p_WR_Type  
          when  'CTCAP' THEN
            v_JobType := 'WR-CTCAP';
          when  'MAPCO' THEN
            v_JobType := 'WR-MAPCOR';
          when  'MPCOR' THEN
            v_JobType := 'WR-MAPCOR';
          when  'ESTG' THEN
            v_JobType := 'WR-ESTIMATE';
          else
            v_JobType:='WR-DESIGN';
        end case;
          
        case p_WR_Type
          when 'CTAP' then
            v_JobStatus:='ConstructionComplete';
          else
            v_JobStatus:='Design';
        end case; 
        
        begin
        
		      select g3e_value 
          into v_Configuration
          from G3E_GENERALPARAMETER_OPTABLE 
          where g3e_name = 'AllConfigurations';
                         
          LTT_USER.SetConfiguration(v_Configuration);
          LTT_ADMIN.CreateJob(p_WR_Number);
		
          insert into g3e_job (G3E_IDENTIFIER,G3E_DESCRIPTION,G3E_JOBTYPE,G3E_JOBSTATUS, WR_NBR,G3E_DATECREATED,WR_TYPE_C,DESIGNER_UID,WR_STATUS_C,WR_CUSTOMER_REQD_D,WR_CONSTRUCT_READY_D,WR_HOUSE_NBR,WR_STREET_NM,WR_TOWN_C,WR_COUNTY_C,WR_CREW_HQ_C,WR_MGMT_ACTIVITY_C)
          values (p_WR_Number,p_WR_Name,v_JobType,v_JobStatus,p_WR_Number,p_WR_Creation_Date,p_WR_Type,p_Designer_Assignment,p_WR_Status,p_Customer_Required_Date,p_Construction_Ready_Date,
                  p_Street_Number,p_Street_Name,p_Town,p_County,p_Crew_Headquarters,p_Management_Activity_Code);
          commit;
         
        exception when others then
           delete from gis.ltt_identifiers where ltt_name = p_WR_Number;
           commit;
           raise_application_error(-20003, 'Error creating job record:' || SQLERRM);
        end;
         
      elsif (v_WRCheck > 0) then --Need to update the job record, assuming that G3E_IDENTIFIER for a given WR_NBR would not change and not needed to be updated
        begin
             
          update g3e_job 
          set G3E_DATECREATED       = p_WR_Creation_Date
            , G3E_DESCRIPTION       = p_WR_Name
            , DESIGNER_UID          = p_Designer_Assignment
            , WR_STATUS_C           = p_WR_Status
            , WR_CUSTOMER_REQD_D    = p_Customer_Required_Date
            , WR_CONSTRUCT_READY_D  = p_Construction_Ready_Date
            , WR_HOUSE_NBR          = p_Street_Number
            , WR_STREET_NM          = p_Street_Name
            , WR_TOWN_C             = p_Town
            , WR_COUNTY_C           = p_County
            , WR_CREW_HQ_C          = p_Crew_Headquarters
            , WR_MGMT_ACTIVITY_C    = p_Management_Activity_Code
          ;
            
          update g3e_job set WMIS_STATUS_C = 'SUCCESS' where WR_NBR = p_WR_Number;
          commit;
             
        exception when others then         
          rollback; --undo everything that is updated
          update g3e_job set WMIS_STATUS_C = 'FAILURE' where WR_NBR = p_WR_Number;
          commit;
            raise_application_error(-20002, 'Error updating job record: '|| SQLERRM);  
        end;
      end if;
    
      dbms_output.put_line ('CreateUpdateJob ended');
    
    exception
      when others then
        raise_application_error(-20003, 'GISPKG_WMIS_WR.CreateUpdateJob: ' || SQLERRM);    
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    --  Public procedure Request Batch
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure RequestBatch( 
                           p_WR_Number in	NUMBER,
                           p_TransId in varchar2,
                           p_xml in clob
                          )
    
    as
        tmpCnt number;
    begin
        -- test to see if the job exists in the G3E_Job table.
        select count(*) into tmpCnt from GIS.G3E_JOB where wr_nbr = p_WR_Number;
        if tmpCnt > 0 then
            begin
                update gis.g3e_job set wmis_status_c = 'BATCH' where wr_nbr = p_WR_Number; 
            exception
               when others then
               raise_application_error(-20001, 'Failed to update GIS job record: '|| SQLERRM);
               return;
            end;
            insert into GIS_ONC.GISAUTO_QUEUE 
                (REQUEST_SERVICE_NM, REQUEST_STATUS, REQUEST_SYSTEM_NM,
                 REQUEST_TRANSACT_ID, REQUEST_WR_NBR, REQUEST_XML,STATUS_DT) 
              values
                (
                'GIS_RequestBatch','NEW','WMIS',to_number(p_TransId),p_WR_Number,to_clob(p_xml),sysdate
                );
                commit;
        else
            raise_application_error(-20002, 'Specified WR does not exist in the GIS job table.');
            return;
        end if;
    
    exception
        when others then
            raise_application_error(-20003, 'GISPKG_WMIS_WR.RequestBatch: ' || SQLERRM); 
    end;

 -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
	procedure UpdateWritebackStatus
    (
        p_WR_Number in	NUMBER, --G3E_JOB.WR_NBR
        p_Writeback_Status in	VARCHAR2 --G3E_JOB.WMIS_STATUS_C
    )
    as
        v_sqlStatement                      VARCHAR2(4000);
        v_tmpStatus varchar2(15);
        v_RowsUpdated number;
        WORK_ORDER_NOT_FOUND exception;
    begin
    
       -- dbms_output.put_line ('UpdateWritebackStatus started');
        
        update g3E_job set WMIS_STATUS_C = p_Writeback_Status where WR_NBR = p_WR_Number;
        v_RowsUpdated := 0 +  sql%rowcount;
        begin
        if v_RowsUpdated = 0 then
            raise WORK_ORDER_NOT_FOUND;
        end if;
        end;
       
       -- dbms_output.put_line ('UpdateWritebackStatus ended');
    
    exception
        when WORK_ORDER_NOT_FOUND then
            raise_application_error(-20002, 'WorkOrder not found.');
        when others then
            raise_application_error(-20003, 'GISPKG_WMIS_WR.UpdateWritebackStatus: ' || SQLERRM);
    end;
       
end;
/


spool off;
