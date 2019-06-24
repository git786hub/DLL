set echo on
set linesize 1000
set pagesize 300
set trimspool on
set define off

spool c:\temp\SYS_GENERALPARAMETER.sql.log
--**************************************************************************************
--SCRIPT NAME: SYS_GENERALPARAMETER.sql
--**************************************************************************************
-- AUTHOR         : Barry Scott
-- DATE           : 17-APR-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Master script for the SYS_GENERALPARAMETER table
--**************************************************************************************
-- Modified:
--  18-JUL-2018, Rich Adase -- Reorganized to only insert missing entries, rather than drop and recreate the entire table
--  08-AUG-2018, Dick Azzam -- Added parameters GISAuto_WMIS/ERRORREPORTING and TRC_IMPORT/LttUserConfig
--**************************************************************************************


declare

  PROCEDURE InsertIfMissing (p_subsystem_name IN VARCHAR2, p_subsystem_component IN VARCHAR2, p_param_name IN VARCHAR2
                            ,p_param_value IN VARCHAR2, p_param_desc IN VARCHAR2) IS
    objCount NUMBER;
  begin
    select count(*) into objCount 
    from SYS_GENERALPARAMETER 
    where SUBSYSTEM_NAME = p_subsystem_name
      and NVL(SUBSYSTEM_COMPONENT,'*') = NVL(p_subsystem_component,'*')
      and PARAM_NAME = p_param_name
    ;
    
    if (objCount = 0) then 
      insert into sys_generalparameter (subsystem_name,subsystem_component,param_name,param_value,param_desc)
        values(p_subsystem_name,p_subsystem_component,p_param_name,p_param_value,p_param_desc);
    end if;
  end;

begin

  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','ANO_OGGX','108','');
  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','ANO_OGGY','109','');
  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','ANO_OGGZ','110','');
  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightHistoricalLine','10012','');
  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightHistoricalStacking','50','');
  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightHistoricalSymbol','12120','');
  InsertIfMissing('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightMovementThreshold','100','Distance to measure against to determine if streetlight will be displayed.');
  InsertIfMissing('AttachJobDocumentCC','File_Extensions','FILE_EXTENSIONS','All files (*.*)|*.*|PDF files (*.pdf)|*.pdf|Dgn files (*.dgn)|*.dgn|Word files (*.docx)|*.docx|Excel files (*.xlsx)|*.xlsx','File filter for the select openFile dialog.');
  InsertIfMissing('AttachJobDocumentCC','SP_Selectable_Types','FILE_TYPES','As-Built Redlines,Engineering Drawings,Pictures,Plots,Reports,Other','List of SharePoint file types for the Combo Box on the main form. The list must be comma delimited with no spaces.');

  InsertIfMissing('CUSelection','CUSelection','Ancillary CU Attribute','2203','G3E_ANO for the CU Attribute in Ancillary CU Component');
  InsertIfMissing('CUSelection','CUSelection','Ancillary CU Attributes Component','22','G3E_CNO for Ancillary CU Attributes Component');
  InsertIfMissing('CUSelection','CUSelection','Ancillary CU Qty Attribute','2206','G3E_ANO for the Quantity Attribute in Ancillary CU Component');
  InsertIfMissing('CUSelection','CUSelection','Ancillary Macro CU Attribute','2204','G3E_ANO for the Macro CU Attribute in Ancillary CU Component');
  InsertIfMissing('CUSelection','CUSelection','CU Activity Field Name','ACTIVITY_C','G3E_FIELD for the Activity Attribute in Ancillary CU Component');
  InsertIfMissing('CUSelection','CUSelection','CU Category Mapping Table','CULIB_CATEGORY','Defines CU Category Mapping table');
  InsertIfMissing('CUSelection','CUSelection','CU Description Field Name','CU_DESC','Defines the Description field name in the compatible unit table');
  InsertIfMissing('CUSelection','CUSelection','CU Installed WR','WR_ID','Defines the Installed WR field on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU Edited WR','WR_EDITED','Defines the Edited WR field on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU Length Flag Field Name','LENGTH_FLAG','Defines the Length flag field on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU Library Table','CULIB_UNIT','Defines the CU Library table name');
  InsertIfMissing('CUSelection','CUSelection','CU Macro Library Table','CULIB_MACROUNIT','Defines the Macro Library Table name');
  InsertIfMissing('CUSelection','CUSelection','CU Prime Account # Field Name','PRIME_ACCT_ID','Defines the Primary Account on CU or Ancillay CU component');
  InsertIfMissing('CUSelection','CUSelection','CU Property Unit Field Name','PROP_UNIT_ID','Defines the Primary Unit on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU Quantity/Length Name','QTY_LENGTH_Q','Defines the Quantity/Length name field on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU Retirement Type Name','RETIREMENT_C','Defines the Retirement Type field on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU Unit CNO Field Name','UNIT_CNO','Defines the Unit CNO field name on CU or Ancillary component');
  InsertIfMissing('CUSelection','CUSelection','CU UnitCID Field Name','UNIT_CID','Defines the Unit CID field name in the compatible unit table');
  InsertIfMissing('CUSelection','CUSelection','Primary CU Attribute','2102','G3E_ANO for the CU Attribute in Primary CU Component');
  InsertIfMissing('CUSelection','CUSelection','Primary CU Attributes Component','21','G3E_CNO for Primary CU Attributes Component');
  InsertIfMissing('CUSelection','CUSelection','Primary CU Qty Attribute','2107','G3E_ANO for the Quantity Attribute in Primary CU Component');
  InsertIfMissing('CUSelection','CUSelection','Primary Macro CU Attribute','2103','G3E_ANO for the Macro CU Attribute in Primary CU Component');
 
  InsertIfMissing('Doc_Management','GT_SharePoint','ROOT_PATH','Oncor F2G Gap Analysis','Name of root site');
  InsertIfMissing('Doc_Management','GT_SharePoint','SP_URL','https://share.intergraph.com/sgi/infr/UCServices','SharePoint site URL');
  InsertIfMissing('Doc_Management','GT_SharePoint','JOBWO_REL_PATH','/sgi/infr/UCServices/Oncor%20F2G%20Gap%20Analysis/F2G Project/Development/Integration/Document Management/Job Documents','Path to Job Docs.');

  InsertIfMissing('EdgeFrontier','GIS_CreateFieldTransaction','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8013/interface','Url to the EdgeFrontier GIS_CreateFieldTransaction System');
  InsertIfMissing('EdgeFrontier','GIS_CreateUpdateJob','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8010/interface','Url to the EdgeFrontier GIS_CreateUpdateJob System');
  InsertIfMissing('EdgeFrontier','GIS_RequestBatch','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8012/interface','Url to the EdgeFrontier GIS_RequestBatch System');
  InsertIfMissing('EdgeFrontier','GIS_UpdateWritebackStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8011/interface','Url to the EdgeFrontier GIS_UpdateWritebackStatus System');
  InsertIfMissing('EdgeFrontier','NJUNS_CheckTicketStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8050/interface','Url to the EdgeFrontier NJUNS_CheckTicketStatus System');
  InsertIfMissing('EdgeFrontier','NJUNS_MemberCodeUpdate','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8052/interface','Url to the EdgeFrontier NJUNS_MemberCodeUpdate System');
  InsertIfMissing('EdgeFrontier','NJUNS_SubmitTicket','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8051/interface','Url to the EdgeFrontier NJUNS_SubmitTicket System');
  InsertIfMissing('EdgeFrontier','SendEmail','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8089/interface','Url to the EdgeFrontier SendEmail System');
  InsertIfMissing('EdgeFrontier','WMIS_SendBatchResults','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8030/interface','Url to the EdgeFrontier WMIS_SendBatchResults System');
  InsertIfMissing('EdgeFrontier','WMIS_UpdateStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8031/interface','Url to the EdgeFrontier WMIS_UpdateStatus System');
  InsertIfMissing('EdgeFrontier','WMIS_WriteBack','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8032/interface','Url to the EdgeFrontier WMIS_WriteBack System');

  InsertIfMissing('GISAUTO_DEIS','URL','DEIS_RESPONSE_WEB_ADDRESS','http://uc-vm-oncor-03.ingrnet.com:8061/interface','The EdgeFrontier system to call to send the DEIS transaction result.');
  InsertIfMissing('GISAUTO_DEIS','Placement','XFMR_LABEL_XY_OFFSET','-20,0','The XY offset from the Transformer for the placement of the Transformer label.');

  InsertIfMissing('GISAUTO_WMIS','ERRORREPORTING','EmailFromAddress','richard.azzam@hexagonsi.com','!!!Needs to be configured for ONCOR Systems.!!!Email From Address for Error reporting in the WMIS Batch Processing.');

  InsertIfMissing('GISAUTO_SERVICELINE','TOLERANCE','GEOCODE_TOLERANCE_OH','150','');
  InsertIfMissing('GISAUTO_SERVICELINE','TOLERANCE','GEOCODE_TOLERANCE_UG','150','');
  
  InsertIfMissing('GISAUTO_WMIS','ERRORREPORTING','EmailFromAddress','richard.azzam@hexagonsi.com','!!!Needs to be configured for ONCOR Systems.!!!Email From Address for Error reporting in the WMIS Batch Processing.');

  InsertIfMissing('LANDBASE','LBM_UTL.DetectPolygonEdgeMismatch','BoundarySnappingDistance','500','The distance parameter to be used as Snapping Distance while detecting boundary edge mismatches');
  InsertIfMissing('LANDBASE','LBM_UTL.PurgeExpiredArchivedLandbase','ArchivedLandbaseExpireDays','365','Number of days after last edit date to purge archived landbase feature instances');

  InsertIfMissing('MaximoWO','ErrorLoggingMail','FromEmailAddr','richard.azzam@hexagonsi.com','The "From" email address for the Maximo Interface');
  InsertIfMissing('MaximoWO','ErrorLoggingMail','NotificationEmailAddr','richard.azzam@hexagonsi.com','The "To" email address for the Maximo Interface');
  InsertIfMissing('MaximoWO','ErrorLoggingMail','Subject','Maximo Interface Errors','The "Subject" email address for the Maximo Interface');
  InsertIfMissing('MaximoWO','LttUserConfig','LttUserConfig','GIS','');

  InsertIfMissing('OwningCompany',null,'OwningCompany_Default','Oncor','Owning company default code.');

  InsertIfMissing('PremiseCorrections','ErrorLoggingMail','FromEmailAddress','richard.gabrys@hexagonsi.com','The "From" for the "Premise Correction Errors" email.');
  InsertIfMissing('PremiseCorrections','ErrorLoggingMail','ToEmailAddress','richard.gabrys@hexagonsi.com','The "To" for the "Premise Correction Errors" email.');

  InsertIfMissing('REPLACEOFFSET','REPLACEOFFSETX','JobMgmt_ReplaceOffsetX','-4','Replace Feature X Offset');
  InsertIfMissing('REPLACEOFFSET','REPLACEOFFSETY','JobMgmt_ReplaceOffsetY','4','Replace Feature Y Offset');

  InsertIfMissing('REQUEST_ESI_LOCATION_CC','','ATTACHMENT_LOCATION','\\LUNDY6800\SharedTest','Location where the Request ESI Location custom command will create the spreadsheet which will be sent to the Call Center using Edge Frontier.');
  InsertIfMissing('REQUEST_ESI_LOCATION_CC','','TO_ADDRESS','joseph.lundy@hexagonsi.com','The "To" recipient of the "GIS ESI Location Request" email.');

  InsertIfMissing('SEND_EMAIL','','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8089/interface','Url to the Edgefrontier Send Email System');

  InsertIfMissing('SERVICE_LINE_INSTALL','CU_DEFAULT','CU','SU10AQ(IC)','');

  InsertIfMissing('StltSplmnt_AttachName','StltSplmnt_AttachName','StltSplmnt_AttachName','Supplemental Plot','Street Light supplemental plot attachment name');

  InsertIfMissing('StltSplmntPlot','StltSplmntPlot','StltSplmnt_Plot','Supplemental Plot','Street Light Supplemental Agreement Plot');

  InsertIfMissing('StreetLightAgreementForms','StreetLightAgreementFormsWithMSLA','StreetLightWithMSLA_URL','\\IN-GPS1832\Share\StreetLightWithMSLA.htm','Street Light Supplemental Agreement Forms With MSLA');
  InsertIfMissing('StreetLightAgreementForms','StreetLightAgreementFormsWithOutMSLA','StreetLightWithWithOutMSLA_URL','\\IN-GPS1832\Share\StreetLightWithWithOutMSLA.htm','Street Light Supplemental Agreement Forms WithOut MSLA');

  InsertIfMissing('StreetlightBilling','ErrorLoggingMail','FromEmailAddress','richard.gabrys@hexagonsi.com','The "From" for the "Street Light count threshold exceeded" email.');
  InsertIfMissing('StreetlightBilling','ErrorLoggingMail','ToEmailAddress','richard.gabrys@hexagonsi.com','The "To" for the "Street Light count threshold exceeded" email.');

  InsertIfMissing('TREETRIMMING_COST_ESTIMATES',null,'VegMgmtEstimate_Sheet','D:\VegetationManagementEstimate_Sheet_files\VegetationManagementEstimate_Sheet.html','HTML sheet template to create Vegetation Management WR Activity Sheet');
  InsertIfMissing('TREETRIMMING_COST_ESTIMATES',null,'VegMgmtEstimate_Plot','TreeTrimmingPlot','Plot template to create Vegetation Management PlotPDF');

  InsertIfMissing('TRC_IMPORT','LttUserConfig','LttUserConfig','GIS','Used by TRC_IMPORT Interface for short term transaction to get the correct GTech Configuration');

  InsertIfMissing('UpdateTrace','NonJobHighlightProperty','Color','255','The override RGB color to use when highlighting the non-WR features that will be updated using the Update Trace module.');
  InsertIfMissing('UpdateTrace','NonJobHighlightProperty','FillColor','255','The override fill RGB color to use when highlighting the non-WR features that will be updated using the Update Trace module.');
  InsertIfMissing('UpdateTrace','NonJobHighlightProperty','Width','1','The overall width to use when highlighting the non-WR features that will be updated using the Update Trace module.');
  InsertIfMissing('UpdateTrace','TraceHighlightProperty','Color','65280','The override RGB color to use when highlighting the features that are traced using the Update Trace module.');
  InsertIfMissing('UpdateTrace','TraceHighlightProperty','FillColor','65280','The override fill RGB color to use when highlighting the features that are traced using the Update Trace module.');
  InsertIfMissing('UpdateTrace','TraceHighlightProperty','Width','2','The overall width to use when highlighting the features that are traced using the Update Trace module.');
  InsertIfMissing('UpdateTrace','UpdateHighlightProperty','Color','16711680','The override RGB color to use when highlighting the features that will be updated using the Update Trace module.');
  InsertIfMissing('UpdateTrace','UpdateHighlightProperty','FillColor','16711680','The override fill RGB color to use when highlighting the features that will be updated using the Update Trace module.');
  InsertIfMissing('UpdateTrace','UpdateHighlightProperty','Width','1','The overall width to use when highlighting the features that will be updated using the Update Trace module.');

  InsertIfMissing('WMIS','WMIS_UpdateStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8031/interface','Url to the Edgefrontier WMIS_UpdateStatus System');
  InsertIfMissing('WMIS','WMIS_WRITERBACK','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8032/interface','Url to the Edgefrontier WMIS_WRITEBACK System');
  InsertIfMissing('WMIS','WMIS_WRITEBACK','ConnectionString','User Id=gis;Password=gistst1#12345;Data Source=DGSTST1','Connection string for writeback polling.');
  InsertIfMissing('WMIS','WMIS_WritebackPollingInterval','Polling Interval','70','Polling Interval for monitoring DB for the WMIS_STATUS change in sec');
	InsertIfMissing('WMIS','Job Creation','RACFID Suffix','@CORP.ONCOR.COM','Suffix for Kerberos database accounts');

end;
/

spool off;
