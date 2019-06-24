set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\SYS_GENERALPARAMETER.sql.log
--**************************************************************************************
--SCRIPT NAME: SYS_GENERALPARAMETER.sql
--**************************************************************************************
-- AUTHOR			: Barry Scott
-- DATE				: 17-APR-2018
-- CYCLE			: 
-- JIRA NUMBER		: 
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Master script for the SYS_GENERALPARAMETER table
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

alter table gis_onc.sys_generalparameter drop primary key cascade;

drop table gis_onc.sys_generalparameter cascade constraints;

drop public synonym sys_generalparameter;

drop sequence gis_onc.sys_generalparam_seq;

create sequence gis_onc.sys_generalparam_seq
  start with 1
  maxvalue 9999999999999999999999999999
  minvalue 1
  nocycle
  nocache
  noorder;

create table gis_onc.sys_generalparameter
(
  id                   number                   default gis_onc.sys_generalparam_seq.nextval not null primary key,
  subsystem_name       varchar2(40 byte)        not null,
  subsystem_component  varchar2(40 byte),
  param_name           varchar2(40 byte)        not null,
  param_value          varchar2(4000 byte)      not null,
  param_desc           varchar2(400 byte)
)
tablespace oncordev
pctused    0
pctfree    10
initrans   1
maxtrans   255
storage    (
            initial          64k
            next             1m
            minextents       1
            maxextents       unlimited
            pctincrease      0
            buffer_pool      default
           )
logging 
nocompress 
nocache
noparallel
monitoring;

alter table gis_onc.sys_generalparameter add constraint sys_generalparameter_unique unique(subsystem_name,subsystem_component,param_name);

create or replace public synonym sys_generalparameter for gis_onc.sys_generalparameter;

grant select on gis_onc.sys_generalparameter to designer;
grant select on gis_onc.sys_generalparameter to gis_onc with grant option;


---------------------------------------------------------------------
-- Add/modify statements below to repopulate the newly-created table.
---------------------------------------------------------------------

insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','ANO_OGGX','108','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','ANO_OGGY','109','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','ANO_OGGZ','110','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightHistoricalLine','10012','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightHistoricalStacking','50','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightHistoricalSymbol','12120','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('ASSET_HISTORY_TRACKING','Streetlight_Tracking','StreetLightMovementThreshold','100','Distance to measure against to determine if streetlight will be displayed.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Ancillary CU Attribute','2203','G3E_ANO for the CU Attribute in Ancillary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Ancillary CU Attributes Component','22','G3E_CNO for Ancillary CU Attributes Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Ancillary CU Qty Attribute','2206','G3E_ANO for the Quantity Attribute in Ancillary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Ancillary Macro CU Attribute','2204','G3E_ANO for the Macro CU Attribute in Ancillary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Activity Field Name','ACTIVITY_C','G3E_FIELD for the Activity Attribute in Ancillary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Category Mapping Table','CULIB_CATEGORY','Defines CU Category Mapping table');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Description Field Name','CU_DESC','Defines the Description field name in the compatible unit table');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Installed WR','WR_ID','Defines the Installed WR field on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Edited WR','WR_EDITED','Defines the Edited WR field on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Length Flag Field Name','LENGTH_FLAG','Defines the Length flag field on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Library Table','CULIB_UNIT','Defines the CU Library table name');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Macro Library Table','CULIB_MACROUNIT','Defines the Macro Library Table name');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Prime Account # Field Name','PRIME_ACCT_ID','Defines the Primary Account on CU or Ancillay CU component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Property Unit Field Name','PROP_UNIT_ID','Defines the Primary Unit on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Quantity/Length Name','QTY_LENGTH_Q','Defines the Quantity/Length name field on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Retirement Type Name','RETIREMENT_C','Defines the Retirement Type field on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU Unit CNO Field Name','UNIT_CNO','Defines the Unit CNO field name on CU or Ancillary component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','CU UnitCID Field Name','UNIT_CID','Defines the Unit CID field name in the compatible unit table');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Primary CU Attribute','2102','G3E_ANO for the CU Attribute in Primary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Primary CU Attributes Component','21','G3E_CNO for Primary CU Attributes Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Primary CU Qty Attribute','2107','G3E_ANO for the Quantity Attribute in Primary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('CUSelection','CUSelection','Primary Macro CU Attribute','2103','G3E_ANO for the Macro CU Attribute in Primary CU Component');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','GIS_CreateFieldTransaction','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8013/interface','Url to the EdgeFrontier GIS_CreateFieldTransaction System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','GIS_CreateUpdateJob','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8010/interface','Url to the EdgeFrontier GIS_CreateUpdateJob System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','GIS_RequestBatch','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8012/interface','Url to the EdgeFrontier GIS_RequestBatch System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','GIS_UpdateWritebackStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8011/interface','Url to the EdgeFrontier GIS_UpdateWritebackStatus System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','NJUNS_CheckTicketStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8050/interface','Url to the EdgeFrontier NJUNS_CheckTicketStatus System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','NJUNS_MemberCodeUpdate','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8052/interface','Url to the EdgeFrontier NJUNS_MemberCodeUpdate System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','NJUNS_SubmitTicket','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8051/interface','Url to the EdgeFrontier NJUNS_SubmitTicket System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','SendEmail','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8089/interface','Url to the EdgeFrontier SendEmail System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','WMIS_SendBatchResults','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8030/interface','Url to the EdgeFrontier WMIS_SendBatchResults System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','WMIS_UpdateStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8031/interface','Url to the EdgeFrontier WMIS_UpdateStatus System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('EdgeFrontier','WMIS_WriteBack','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8032/interface','Url to the EdgeFrontier WMIS_WriteBack System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('GISAUTO_DEIS','URL','DEIS_RESPONSE_WEB_ADDRESS','http://uc-vm-oncor-03.ingrnet.com:8061/interface','The EdgeFrontier system to call to send the DEIS transaction result.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('GISAUTO_DEIS','Placement','XFMR_LABEL_XY_OFFSET','-20,0','The XY offset from the Transformer for the placement of the Transformer label.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('GISAUTO_SERVICELINE','TOLERANCE','GEOCODE_TOLERANCE_OH','150','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('GISAUTO_SERVICELINE','TOLERANCE','GEOCODE_TOLERANCE_UG','150','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('MaximoWO','ErrorLoggingMail','FromEmailAddr','richard.azzam@hexagonsi.com','The "From" email address for the Maximo Interface');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('MaximoWO','ErrorLoggingMail','NotificationEmailAddr','richard.azzam@hexagonsi.com','The "To" email address for the Maximo Interface');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('MaximoWO','ErrorLoggingMail','Subject','Maximo Interface Errors','The "Subject" email address for the Maximo Interface');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('MaximoWO','LttUserConfig','LttUserConfig','GIS','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('PremiseCorrections','ErrorLoggingMail','FromEmailAddress','richard.gabrys@hexagonsi.com','The "From" for the "Premise Correction Errors" email.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('PremiseCorrections','ErrorLoggingMail','ToEmailAddress','richard.gabrys@hexagonsi.com','The "To" for the "Premise Correction Errors" email.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('REQUEST_ESI_LOCATION_CC','','ATTACHMENT_LOCATION','\\LUNDY6800\SharedTest','Location where the Request ESI Location custom command will create the spreadsheet which will be sent to the Call Center using Edge Frontier.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('REQUEST_ESI_LOCATION_CC','','TO_ADDRESS','joseph.lundy@hexagonsi.com','The "To" recipient of the "GIS ESI Location Request" email.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('SEND_EMAIL','','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8089/interface','Url to the Edgefrontier Send Email System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('SERVICE_LINE_INSTALL','CU_DEFAULT','CU','SU10AQ(IC)','');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('StreetlightBilling','ErrorLoggingMail','FromEmailAddress','richard.gabrys@hexagonsi.com','The "From" for the "Street Light count threshold exceeded" email.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('StreetlightBilling','ErrorLoggingMail','ToEmailAddress','richard.gabrys@hexagonsi.com','The "To" for the "Street Light count threshold exceeded" email.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('TREETRIMMING_COST_ESTIMATES','','VegMgmtEstimate_Plot','TreeTrimmingPlot','Plot template to create Vegetation Management PlotPDF');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('TREETRIMMING_COST_ESTIMATES','','VegMgmtEstimate_Sheet','D:\VegetationManagementEstimate_Sheet_files\VegetationManagementEstimate_Sheet.html','HTML sheet template to create Vegetation Management WR Activity Sheet');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','NonJobHighlightProperty','Color','255','The override RGB color to use when highlighting the non-WR features that will be updated using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','NonJobHighlightProperty','FillColor','255','The override fill RGB color to use when highlighting the non-WR features that will be updated using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','NonJobHighlightProperty','Width','1','The overall width to use when highlighting the non-WR features that will be updated using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','TraceHighlightProperty','Color','65280','The override RGB color to use when highlighting the features that are traced using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','TraceHighlightProperty','FillColor','65280','The override fill RGB color to use when highlighting the features that are traced using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','TraceHighlightProperty','Width','2','The overall width to use when highlighting the features that are traced using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','UpdateHighlightProperty','Color','16711680','The override RGB color to use when highlighting the features that will be updated using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','UpdateHighlightProperty','FillColor','16711680','The override fill RGB color to use when highlighting the features that will be updated using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('UpdateTrace','UpdateHighlightProperty','Width','1','The overall width to use when highlighting the features that will be updated using the Update Trace module.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('WMIS','WMIS_UpdateStatus','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8031/interface','Url to the Edgefrontier WMIS_UpdateStatus System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('WMIS','WMIS_WRITERBACK','EF_URL','http://uc-vm-oncor-03.ingrnet.com:8032/interface','Url to the Edgefrontier WMIS_WRITEBACK System');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('WMIS','WMIS_WRITEBACK','ConnectionString','User Id=gis;Password=gistst1#12345;Data Source=DGSTST1','Connection string for writeback polling.');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('WMIS','WMIS_WritebackPollingInterval','Polling Interval','70','Polling Interval for monitoring DB for the WMIS_STATUS change in sec');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('StltSplmnt_AttachName','StltSplmnt_AttachName','StltSplmnt_AttachName','Supplemental Plot','Street Light supplemental plot attachment name');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('StreetLightAgreementForms','StreetLightAgreementFormsWithMSLA','StreetLightWithMSLA_URL','\\IN-GPS1832\Share\StreetLightWithMSLA.htm','Street Light Supplemental Agreement Forms With MSLA');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('StreetLightAgreementForms','StreetLightAgreementFormsWithOutMSLA','StreetLightWithWithOutMSLA_URL','\\IN-GPS1832\Share\StreetLightWithWithOutMSLA.htm','Street Light Supplemental Agreement Forms WithOut MSLA');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('StltSplmntPlot','StltSplmntPlot','StltSplmnt_Plot','Supplemental Plot','Street Light Supplemental Agreement Plot');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('LANDBASE','LBM_UTL.DetectPolygonEdgeMismatch','BoundarySnappingDistance','500','The distance parameter to be used as Snapping Distance while detecting boundary edge mismatches');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('LANDBASE','LBM_UTL.PurgeExpiredArchivedLandbase','ArchivedLandbaseExpireDays','365','Number of days after last edit date to purge archived landbase feature instances');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('TREETRIMMING_COST_ESTIMATES',null,'VegMgmtEstimate_Sheet','D:\VegetationManagementEstimate_Sheet_files\VegetationManagementEstimate_Sheet.html','HTML sheet template to create Vegetation Management WR Activity Sheet');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('TREETRIMMING_COST_ESTIMATES',null,'VegMgmtEstimate_Plot','TreeTrimmingPlot','Plot template to create Vegetation Management PlotPDF');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('REPLACEOFFSET','REPLACEOFFSETX','JobMgmt_ReplaceOffsetX','-4','Replace Feature X Offset');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values('REPLACEOFFSET','REPLACEOFFSETY','JobMgmt_ReplaceOffsetY','4','Replace Feature Y Offset');
insert into sys_generalparameter columns(subsystem_name,subsystem_component,param_name,param_value,param_desc)values ('OwningCompany',null,'OwningCompany_Default','Oncor','Owning company default code.');

commit;

spool off;