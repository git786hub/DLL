/*---------------------------------------------------------------------------------------------------------------------------------
-- Name: EdgeFrontier_PKG_Updates.sql
--
-- Description:
--     This script updates the: 
--           GIS_ONC.GISPKG_WMIS_WR
--           GIS.GISPKG_EF_UTIL
--     packages to resolve issue descovered during ONCOR Devlopment Integration
--          testing.
-- Build: 00.03.05
-- History:
--     09-May-2018,     Hexagon, Updates.
-- 
-- ---------------------------------------------------------------------------------------------------------------------------------*/


CREATE OR REPLACE package GIS_ONC.GISPKG_WMIS_WR
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
    -- This stored procedure 
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

CREATE OR REPLACE package body GIS_ONC.GISPKG_WMIS_WR
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to support the GIS interface with WMIS.
-- History:
--     26-FEB-2018, v0.1    Hexagon, Initial creation
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
        
		  select g3e_value into v_Configuration
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
             
             update g3e_job set G3E_DATECREATED = p_WR_Creation_Date, G3E_DESCRIPTION =p_WR_Name, G3E_JOBTYPE = p_WR_Type, DESIGNER_UID = p_Designer_Assignment ,WR_STATUS_C = p_WR_Status
             ,WR_CUSTOMER_REQD_D = p_Customer_Required_Date,WR_CONSTRUCT_READY_D =p_Construction_Ready_Date,WR_HOUSE_NBR =p_Street_Number,WR_STREET_NM =p_Street_Name,WR_TOWN_C =p_Town
             ,WR_COUNTY_C =p_County,WR_CREW_HQ_C =p_Crew_Headquarters,WR_MGMT_ACTIVITY_C =p_Management_Activity_Code where WR_NBR = p_WR_Number;
             
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
                'GIS_RequestBatch','NEW','WMIS',to_number(p_TransId),p_WR_Number,p_xml,sysdate
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

CREATE OR REPLACE PACKAGE GIS.GISPKG_EF_UTIL AS 

  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
  function GenerateResultString
    (
      status	                            in	VARCHAR2,
      result_code	                        in	VARCHAR2,
      error_message	                      in	VARCHAR2
    ) return VARCHAR2;
    
  function EF_UpdateTicketStatus
  (
    ticket_status in VARCHAR2,
    ticket_number in VARCHAR2
  ) return VARCHAR2;
  
  function EF_UpdateTicketId
  (
    ticket_id in VARCHAR2,
    ticket_number in VARCHAR2,
    pole_number in VARCHAR2
  ) return VARCHAR2;
  
  function EF_UpdateMemberCodes
  (
    njunsMemberCode in VARCHAR2,
    njunsMemberName in VARCHAR2,
    njunsMemberDescr in VARCHAR2,
    attacherCode in VARCHAR2,
    stateCode in VARCHAR2,
    countyCode in VARCHAR2
  ) return VARCHAR2;
  
  function EF_UpdateWritebackStatus
  (
    wr_number in VARCHAR2,
    writeback_status in VARCHAR2
  ) return VARCHAR2;
  
function EF_WriteBackStatus
  (
    wr_number in VARCHAR2,
    wr_status in VARCHAR2
  ) return varchar2;
  
  function EF_UpdateStatus
  (
    wr_number in VARCHAR2,
    wr_status in VARCHAR2
  ) return VARCHAR2;
  
  function EF_RequestBatch
  (
    request_trans_id in VARCHAR2,
    request_system_name in VARCHAR2,
    request_service_name in VARCHAR2,
    request_xml in VARCHAR2,
    wb_number in VARCHAR2
  ) return VARCHAR2;
  
  function EF_CreateUpdateJob(
    WRNumber VARCHAR2,
    WRStatus VARCHAR2,
    PercentDesignComplete VARCHAR2,
    PercentConstructionComplete VARCHAR2,
    CrewHQ VARCHAR2,
    MgmtActivityCode VARCHAR2,
    WRType VARCHAR2,
    ConstReadyDate VARCHAR2,
    CustRequiredDate VARCHAR2,
    WRName VARCHAR2,
    WRScope VARCHAR2,
    HouseNumber VARCHAR2,
    HouseNumberFract VARCHAR2,
    LeadingDir VARCHAR2,
    StreetName VARCHAR2,
    StreetType VARCHAR2,
    TrailingDir VARCHAR2,
    Town VARCHAR2,
    County VARCHAR2,
    DesignerAssignment VARCHAR2,
    DualCert VARCHAR2,
    DualCertPwrCo1 VARCHAR2,
    DualCertPwrCo2 VARCHAR2,
    AssocCost VARCHAR2,
    ElectricInService VARCHAR2) RETURN varchar2;
    
    function EF_InsertSvcActivityRecord
    (
      p_service_order_no	                in	varchar2,
      p_esi_location	                    in	varchar2,
      p_address	                            in	varchar2,
      p_town_nm	                            in	varchar2,
      p_service_info_code	                in	varchar2,
      --p_trans_date	                        in	varchar2,
      p_o_or_u_code	                        in	varchar2,
      p_service_center_code	                in	varchar2,
      p_flnx_h	                            in	varchar2,
      p_flny_h	                            in	varchar2,
      p_trf_co_h	                        in	varchar2,
      p_trf_flnx_h	                        in	varchar2,
      p_trf_flny_h	                        in	varchar2,
      p_cu_id	                            in	varchar2,
      p_mgmt_activity_code	                in	varchar2,
     -- p_gis_process_flag	                in	varchar2,
     -- p_status_c	                        in	varchar2,
      p_msg_t	                            in	varchar2,
      p_dwell_type_c	                    in	varchar2,
      p_unit_h	                            in	varchar2,
      p_user_id	                            in	varchar2,
      p_exist_prem_gangbase	                in	varchar2,
      p_remarks_mobile	                    in	varchar2,
      p_meter_latitude	                    in	varchar2,
      p_meter_longitude	                    in	VARCHAR2
    ) return VARCHAR2;
      
  function EF_QueueDEISTransaction
  (
    trans_id in VARCHAR2,
    system_name in VARCHAR2,
    service_name in VARCHAR2,
    payload_xml in CLOB,
    wr_number in VARCHAR2
  ) return VARCHAR2;


END GISPKG_EF_UTIL;
/

CREATE OR REPLACE PACKAGE BODY GIS.GISPKG_EF_UTIL AS

function GenerateResultString
    (
      status	                            in	VARCHAR2,
      result_code	                        in	VARCHAR2,
      error_message	                      in	VARCHAR2
    ) return VARCHAR2 AS
  BEGIN
    return utl_lms.format_message('%s^_%s^_%s^_%s', status, result_code, error_message, replace( TO_CHAR(SYSDATE),' ','T'));
  END GenerateResultString;
  
function EF_UpdateTicketStatus
  (
    ticket_status in VARCHAR2,
    ticket_number in VARCHAR2
  ) return varchar2
  AS
  pragma autonomous_transaction;
  BEGIN
    UPDATE B$NJUNS_TICKET_N 
    SET TICKET_STATUS = ticket_status
    WHERE TICKET_NUMBER = ticket_number;
    commit;
    return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'ERR', 'Failed to update ticket status: '||sqlerrm);
END EF_UpdateTicketStatus;  

function EF_UpdateTicketId
  (
    ticket_id in VARCHAR2,
    ticket_number in VARCHAR2,
    pole_number in VARCHAR2
  ) return varchar2
  AS
  pragma autonomous_transaction;
  BEGIN
    /*UPDATE  B$NJUNS_TICKET_N 
    SET 	NJUNS_TICKET_ID = ticket_id,
            TICKET_NUMBER = ticket_number
    WHERE 	POLE_NUMBER = pole_number;
  
    commit;*/
    return GISPKG_EF_UTIL.GenerateResultString('NOT IMPLEMENTED', '', '');  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'ERR', 'Failed to update ticket id: '||sqlerrm);
END EF_UpdateTicketId;  

function EF_UpdateMemberCodes
  (
    njunsMemberCode in VARCHAR2,
    njunsMemberName in VARCHAR2,
    njunsMemberDescr in VARCHAR2,
    attacherCode in VARCHAR2,
    stateCode in VARCHAR2,
    countyCode in VARCHAR2
  ) return varchar2
  AS
  pragma autonomous_transaction;
  BEGIN
    commit;
    return GISPKG_EF_UTIL.GenerateResultString('NOT IMPLEMENTED', '', '');  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'ERR', 'Failed to update member codes: '||sqlerrm);
END EF_UpdateMemberCodes;  

function EF_UpdateWritebackStatus
  (
    wr_number in VARCHAR2,
    writeback_status in VARCHAR2
  ) return varchar2
  AS
  
  v_writeback_status varchar2(15);
  pragma autonomous_transaction;
  
  STATUS_NO_UPDATE exception;
  UNKNOWN_ERROR exception;
  PRAGMA EXCEPTION_INIT(STATUS_NO_UPDATE, -20002);
  PRAGMA EXCEPTION_INIT(UNKNOWN_ERROR, -20003);
  
  BEGIN

    v_writeback_status := upper(writeback_status);
    
    GISPKG_WMIS_WR.UpdateWriteBackStatus(TO_NUMBER(wr_number), v_writeback_status);
    
    commit;
    return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');  
  exception
    when STATUS_NO_UPDATE then
            rollback;
            dbms_output.put_line( sqlerrm);
            return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISUWS03',sqlerrm);

    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISUWS02', 'Failed to update WMIS_STATUS_C: '||sqlerrm);
END EF_UpdateWritebackStatus;  

 function EF_WriteBackStatus
  (
    wr_number in VARCHAR2,
    wr_status in VARCHAR2
  ) return varchar2
  AS
    v_writeback_status varchar2(15);
    pragma autonomous_transaction;
  BEGIN
    v_writeback_status := upper(wr_status);
--dbms_output.put_line('Status: ' ||v_writeback_status);
--dbms_output.put_line('WR: ' ||wr_number);
    if v_writeback_status = 'SUCCESS' then
        v_writeback_status := 'WRITEBACK';
    end if;

    GISPKG_WMIS_WR.UpdateWriteBackStatus(TO_NUMBER(wr_number), v_writeback_status);

    commit;
    if v_writeback_status = 'WRITEBACK' then
        return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');
    else 
        return GISPKG_EF_UTIL.GenerateResultString(v_writeback_status, '', 'WriteBack message to Informatica failed.');
    end if;  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'WMISWB02', 'Failed to update WMIS_STATUS_C: '||sqlerrm);
END EF_WriteBackStatus;

  function EF_UpdateStatus
  (
    wr_number in VARCHAR2,
    wr_status in VARCHAR2
  ) return varchar2
  AS
  pragma autonomous_transaction;
  BEGIN
    
    commit;
    return GISPKG_EF_UTIL.GenerateResultString('NOT IMPLEMENTED', '', '');  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', '-20002', 'Failed to update status: '||sqlerrm);
END EF_UpdateStatus; 

  function EF_RequestBatch
  (
    request_trans_id in VARCHAR2,
    request_system_name in VARCHAR2,
    request_service_name in VARCHAR2,
    request_xml in VARCHAR2,
    wb_number in VARCHAR2
  ) return varchar2
  AS
  pragma autonomous_transaction;
  NO_JOB_UPDATE exception;
  NO_JOB exception;
  PRAGMA EXCEPTION_INIT(NO_JOB_UPDATE, -20001);
  PRAGMA EXCEPTION_INIT(NO_JOB, -20002);
    
  BEGIN
  
    commit;
    GIS_ONC.GISPKG_WMIS_WR.REQUESTBATCH(wb_number, request_trans_id , request_xml);
    
    return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');  
  exception
    when NO_JOB then
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISRB02', sqlerrm);
    when NO_JOB_UPDATE then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISRB03', sqlerrm);
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISRB01', 'Batch Request Failed: '||sqlerrm);
END EF_RequestBatch;

  function EF_CreateUpdateJob
  (
    WRNumber VARCHAR2,
    WRStatus VARCHAR2,
    PercentDesignComplete VARCHAR2,
    PercentConstructionComplete VARCHAR2,
    CrewHQ VARCHAR2,
    MgmtActivityCode VARCHAR2,
    WRType VARCHAR2,
    ConstReadyDate VARCHAR2,
    CustRequiredDate VARCHAR2,
    WRName VARCHAR2,
    WRScope VARCHAR2,
    HouseNumber VARCHAR2,
    HouseNumberFract VARCHAR2,
    LeadingDir VARCHAR2,
    StreetName VARCHAR2,
    StreetType VARCHAR2,
    TrailingDir VARCHAR2,
    Town VARCHAR2,
    County VARCHAR2,
    DesignerAssignment VARCHAR2,
    DualCert VARCHAR2,
    DualCertPwrCo1 VARCHAR2,
    DualCertPwrCo2 VARCHAR2,
    AssocCost VARCHAR2,
    ElectricInService VARCHAR2
  ) return varchar2
  AS
  pragma autonomous_transaction;
  BEGIN
    commit;
    
    GISPKG_WMIS_WR.CREATEUPDATEJOB(TO_NUMBER(WRNumber), SYSDATE, WRName, WRType, DesignerAssignment, WRStatus, 
    TO_DATE(CustRequiredDate), TO_DATE(ConstReadyDate),  HouseNumber, StreetName, TO_NUMBER(Town), TO_NUMBER(County), CrewHQ, MgmtActivityCode);
    return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISCUJ02', 'Failed to create/update job: '||sqlerrm);
END EF_CreateUpdateJob;  

function EF_InsertSvcActivityRecord
  (
      p_service_order_no	                in	varchar2,
      p_esi_location	                    in	varchar2,
      p_address	                            in	varchar2,
      p_town_nm	                            in	varchar2,
      p_service_info_code	                in	varchar2,
      --p_trans_date	                        in	varchar2,
      p_o_or_u_code	                        in	varchar2,
      p_service_center_code	                in	varchar2,
      p_flnx_h	                            in	varchar2,
      p_flny_h	                            in	varchar2,
      p_trf_co_h	                        in	varchar2,
      p_trf_flnx_h	                        in	varchar2,
      p_trf_flny_h	                        in	varchar2,
      p_cu_id	                            in	varchar2,
      p_mgmt_activity_code	                in	varchar2,
     -- p_gis_process_flag	                in	varchar2,
     -- p_status_c	                        in	varchar2,
      p_msg_t	                            in	varchar2,
      p_dwell_type_c	                    in	varchar2,
      p_unit_h	                            in	varchar2,
      p_user_id	                            in	varchar2,
      p_exist_prem_gangbase	                in	varchar2,
      p_remarks_mobile	                    in	varchar2,
      p_meter_latitude	                    in	varchar2,
      p_meter_longitude	                    in	VARCHAR2
  ) return varchar2
  AS
  status_message varchar(4000);
  pragma autonomous_transaction;
  BEGIN
    
    status_message:= GISPKG_CCB_ESILOCATION.InsertSvcActivityRecordIFACE(
      p_service_order_no,
      p_esi_location,
      p_address,
      p_town_nm,
      p_service_info_code,
     -- p_trans_date,
      p_o_or_u_code,
      p_service_center_code,
      p_flnx_h,
      p_flny_h,
      p_trf_co_h,
      p_trf_flnx_h,
      p_trf_flny_h,
      p_cu_id,
      p_mgmt_activity_code,
     -- p_gis_process_flag,
     -- p_status_c,
      p_msg_t,
      p_dwell_type_c,
      p_unit_h,
      p_user_id,
      p_exist_prem_gangbase,
      p_remarks_mobile,
      p_meter_latitude,
      p_meter_longitude);
      
      commit;
      
      return GISPKG_EF_UTIL.GenerateResultString('Success','', '');
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', 'GISCUJ02', 'Failed to insert service activity: '||sqlerrm);
END EF_InsertSvcActivityRecord; 
  
 

  function EF_QueueDEISTransaction
  (
    trans_id in VARCHAR2,
    system_name in VARCHAR2,
    service_name in VARCHAR2,
    payload_xml in CLOB,
    wr_number in VARCHAR2
  ) return VARCHAR2 
  AS
  pragma autonomous_transaction;
  BEGIN
    commit;
    return GISPKG_EF_UTIL.GenerateResultString('NOT IMPLEMENTED', '', '');  
  exception
    when others then
        rollback;
        return GISPKG_EF_UTIL.GenerateResultString('FAILURE', '-20002', 'Failed to Queue DEIS Transaction: '||sqlerrm);
END EF_QueueDEISTransaction;

END GISPKG_EF_UTIL;
/