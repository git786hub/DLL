set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\GISPKG_EF_UTIL.log
--**************************************************************************************
-- SCRIPT NAME: GISPKG_EF_UTIL.sql
--**************************************************************************************
-- AUTHOR			    : Hexagon
-- DATE				    : 16-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Extends EdgeFrontier Capability
--**************************************************************************************

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

spool off;