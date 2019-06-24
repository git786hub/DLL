set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\SysAccess_CustomCommand.log
--**************************************************************************************
--SCRIPT NAME: SysAccess_CustomCommand.sql
--**************************************************************************************
-- AUTHOR         : INGRNET\RRADASE
-- DATE           : 11-JUN-2018
-- PRODUCT VERSION	: 10.3
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Metadata for custom commands
--**************************************************************************************
-- Modified:
--  31-JUL-2018, Rich Adase -- Added Design Tools commands, Joint Use > Import Feature, fixed CCNO conflicts
--**************************************************************************************

--
-- ANALYSIS
--

-- Analysis->Enhanced Upstream Trace
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=97 or G3E_USERNAME='Enhanced Upstream Trace';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (97,'Enhanced Upstream Trace','ccEnhancedUpstreamTrace:GTechnology.Oncor.CustomAPI.ccEnhancedUpstreamTrace','The command will trace the network and identify all possible sources for the selected feature.','Hexagon',
          1,1048576,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=97;
update G3E_ROLEASSOCIATION set G3E_ROLE='EVERYONE' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=97;

-- Analysis->Landbase->Copy Landbase Boundary
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=62 or G3E_USERNAME='Copy Landbase Boundary';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (62,'Copy Landbase Boundary','ccCopyLandbaseBoundary:GTechnology.Oncor.CustomAPI.ccCopyLandbaseBoundary','Copy Landbase Boundary','Hexagon',
          1,9437200,0,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=62;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=62;

-- Analysis->Landbase->Detect Overlapping Boundaries
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=78 or G3E_USERNAME='Detect Overlapping Boundaries';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (78,'Detect Overlapping Boundaries','ccDetectOverlappingBoundary:GTechnology.Oncor.CustomAPI.ccDetectOverlappingAnalysis','Detect Overlapping Boundaries','Hexagon',
          0,8388624,1,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=78;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=78;

-- Analysis->Landbase->Detect Polygon Edge Mismatch
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=79 or G3E_USERNAME='Detect Polygon Edge Mismatch';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (79,'Detect Polygon Edge Mismatch','ccDetectPoylgonEdgeMismatch:GTechnology.Oncor.CustomAPI.ccDetectPoylgonEdgeMismatch','Detect Polygon Edge Mismatch','Hexagon',
          0,8388624,1,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=79;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=79;

-- Analysis->Landbase->Divide Landbase Boundary
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=66 or G3E_USERNAME='Divide Landbase Boundary';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (66,'Divide Landbase Boundary','ccDivideLandbaseBoundary:GTechnology.Oncor.CustomAPI.ccDivideLandbaseBoundary','Divide Landbase Boundary','Hexagon',
          1,9437200,0,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=66;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=66;

-- Analysis->Landbase->Manual Landbase Review
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=61 or G3E_USERNAME='Manual Landbase Review';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (61,'Manual Landbase Review','ccManualLandbaseReview:GTechnology.Oncor.CustomAPI.ccManualLandbaseReview','Manual Landbase Review','Hexagon',
          0,8388608,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=61;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=61;

-- Analysis->Landbase->Merge Landbase Boundaries
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=59 or G3E_USERNAME='Merge Landbase Boundaries';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (59,'Merge Landbase Boundaries','ccMergeLandbaseBoundary:GTechnology.Oncor.CustomAPI.ccMergeLandbase','Merge selected landbase boundaries','Hexagon',
          1,9437200,0,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=59;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=59;

-- Analysis->Landbase->Purge Expired Landbase
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=60 or G3E_USERNAME='Purge Expired Landbase';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (60,'Purge Expired Landbase','ccPurgeExpiredLandbase:GTechnology.Oncor.CustomAPI.ccPurgeExpiredArchivedLandbase','Purge Expired Archived Landbase','Hexagon',
          0,8388624,1,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=60;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_LAND' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=60;

-- Analysis->Review Asset History
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=47 or G3E_USERNAME='Review Asset History';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (47,'Review Asset History','ccReviewAssetHistory:GTechnology.Oncor.CustomAPI.ccReviewAssetHistory','Review Asset History','Hexagon',
          4,0,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=47;
update G3E_ROLEASSOCIATION set G3E_ROLE='EVERYONE' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=47;

-- Analysis->Update Trace
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=54 or G3E_USERNAME='Update Trace';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (54,'Update Trace','ccUpdateTrace:GTechnology.Oncor.CustomAPI.ccUpdateTrace','The command updates feature connectivity attributes using the results of the Trace Primary/Secondary and Trace Primary/Secondary Proposed traces.','Hexagon',
          4,9437200,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=54;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_TRACE_UPDATE' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=54; 

--
-- EDIT
--

-- Edit->Corrections Mode
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=82 or G3E_USERNAME='Corrections Mode';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (82,'Corrections Mode','ccCorrectionsMode:GTechnology.Oncor.CustomAPI.ccCorrectionsMode','Toggles the state of a session property to indicate whether the session is design mode or corrections mode','Hexagon',
          0,16,1,0,1,9,9);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=82;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=82; 

-- Edit->Documents->Attach Job Document 
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=45 or G3E_USERNAME='Attach Job Document';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (45,'Attach Job Document','ccAttachJobDocument:GTechnology.Oncor.CustomAPI.ccAttachJobDocument','Saves a document to SharePoint in the folder for the active Job WR and creates a Hyperlink on the Design Area for the job.','Hexagon',
          0,8454160,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=45;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=45; 

-- Edit->Documents->Attach Site Drawing
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=86 or G3E_USERNAME='Attach Site Drawing';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (86,'Attach Site Drawing','ccAttachSiteDrawing:GTechnology.Oncor.CustomAPI.ccAttachSiteDrawing','To generate a PDF from an open plot window and attach it to a selected Permit or Easement feature','Hexagon',
          0,9437232,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=86;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=86; 

-- Edit->Documents->Display Construction Redlines
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=46 or G3E_USERNAME='Display Construction Redlines';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (46,'Display Construction Redlines','ccDisplayConstructionRedlines:GTechnology.Oncor.CustomAPI.ccDisplayConstructionRedlines','The ability to view job-related redline files in the G/Technology map windows will be made possible through the use of the Display Construction Redlines custom command','Hexagon',
          0,16,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=46;
update G3E_ROLEASSOCIATION set G3E_ROLE='EVERYONE' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=46; 

-- Edit->Features->Abandon Feature
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=80 or G3E_USERNAME='Abandon Feature';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (80,'Abandon Feature','ccAbandonFeature:GTechnology.Oncor.CustomAPI.ccAbandonFeature','Allows features to be translated from a specific set of feature states to one of the available abandon states.','Hexagon',
          0,8388624,1,1,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=80;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=80; 

-- Edit->Features->Bus Transformers
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=73 or G3E_USERNAME='Bus Transformers';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (73,'Bus Transformers','ccCreateBusTransformer:GTechnology.Oncor.CustomAPI.ccCreateBusTransformers','Command to Bus transformers together','Hexagon',
          0,8912912,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=73;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=73;

-- Edit->Features->Complete Feature
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=40 or G3E_USERNAME='Complete Feature';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (40,'Complete Feature','ccCompleteFeature:GTechnology.Oncor.CustomAPI.ccCompleteFeature','Allows features to be translated from proposed or as-built states to the corresponding completed state','Hexagon',
          0,8388624,1,1,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=40;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_OMS_CONNECTIVITY' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=40;

-- Edit->Features->Copy Ancillaries
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=87 or G3E_USERNAME='Copy Ancillaries';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (87,'Copy Ancillaries','ccCopyAncillaries:GTechnology.Oncor.CustomAPI.ccCopyAncillaries','Command will allow the user to identify one or more features that will inherit the Ancillary CUs from the source feature','Hexagon',
          4,9437200,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=87;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=87; 

-- Edit->Features->Relocate Feature
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=83 or G3E_USERNAME='Relocate Feature';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (83,'Relocate Feature','ccRelocateFeature:GTechnology.Oncor.CustomAPI.ccRelocateFeature','Relocate Feature','Hexagon',
          4,9437200,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=83;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=83; 

-- Edit->Features->Remove Feature
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=81 or G3E_USERNAME='Remove Feature';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (81,'Remove Feature','ccRemoveFeature:GTechnology.Oncor.CustomAPI.ccRemoveFeature','Provides the ability for a user to transition features from a specific set of feature states to one of the available remove states.','Hexagon',
          4,8912912,0,0,1,8,8);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=81;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=81; 

-- Edit->Features->Replace Feature
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=90 or G3E_USERNAME='Replace Feature';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (90,'Replace Feature','ccReplaceFeature:GTechnology.Oncor.CustomAPI.ccReplaceFeature','Provides the ability for a user to replace a feature, the existing feature is processed for removal, and a new copy of the feature is processed as an installation.','Hexagon',
          0,9437200,1,1,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=90;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=90; 

-- Edit->Features->Revert Feature
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=85 or G3E_USERNAME='Revert Feature';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (85,'Revert Feature','ccRevertFeature:GTechnology.Oncor.CustomAPI.ccRevertFeature','Revert Feature','Hexagon',
          0,9437200,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=85;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=85; 

--
-- JOBS
--

-- Jobs->Create Alternate Design
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=50 or G3E_USERNAME='Create Alternate Design';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (50,'Create Alternate Design','ccCreateAlternateDesign:GTechnology.Oncor.CustomAPI.ccCreateAlternateDesign','Creates an Alternate Design for the Active Job','Hexagon',
          0,16,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=50;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=50;

-- Jobs->Post Job
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=89 or G3E_USERNAME='Post Job';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (89,'Post Job','ccPostJob:GTechnology.Oncor.CustomAPI.PostJob','Posts the edits for the active job.','Hexagon',
          0,8388624,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=89;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_EDIT' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=89;

-- Jobs->Revert Job
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=84 or G3E_USERNAME='Revert Job';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (84,'Revert Job','ccRevertJob:GTechnology.Oncor.CustomAPI.ccRevertJob','Revert Job','Hexagon',
          0,8388624,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=84;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_SUPPORT' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=84;

-- Jobs->WMIS->Manage Preferred CUs
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=49 or G3E_USERNAME='Manage Preferred CUs';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (49,'Manage Preferred CUs','ccManagePreferredCUs:GTechnology.Oncor.CustomAPI.ccManagePreferredCUs','Allows user to manage preferred CUs','Hexagon',
          0,0,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=49;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=49;

-- Jobs->WMIS->Mark for Approval
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=71 or G3E_USERNAME='Mark for Approval';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (71,'Mark for Approval','ccMarkForApproval:GTechnology.Oncor.CustomAPI.ccMarkForApproval','Command to mark a WR for approval.','Hexagon',
          0,8388624,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=71;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=71;

-- Jobs->WMIS->Mark for Closure 
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=72 or G3E_USERNAME='Mark for Closure';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (72,'Mark for Closure','ccMarkForClosure:GTechnology.Oncor.CustomAPI.ccMarkForClosure','Command to mark a WR for closure.','Hexagon',
          0,8388624,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=72;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=72;

-- Jobs->WMIS->Review Vouchers
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=48 or G3E_USERNAME='Review Vouchers';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (48,'Review Vouchers','ccReviewVouchers:GTechnology.Oncor.CustomAPI.ccReviewVouchers','Provides the user a way to review all vouchers for all Work Points in the active WR.','Hexagon',
          0,16,0,0,2,10,10);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=48;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=48;

-- Jobs->WMIS->Write to WMIS
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=114 or G3E_USERNAME='Write to WMIS';

insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP)
	values (114,'Write to WMIS','ccWriteToWMIS:GTechnology.Oncor.CustomAPI.WriteToWMIS','Writes CU and other pertinent data to WMIS','Hexagon',
					0,8388624,1,null,1,null,null);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=114;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=114;

--
-- TOOLS
--

-- Tools->Design Tools->Cable Pull Tension
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=91 or G3E_USERNAME='Cable Pull Tension';
Insert into G3E_CUSTOMCOMMAND (
	G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
	G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) 
  values (91,'Cable Pull Tension','Calculates the tension and side wall bearing pressure for a selected cable and conduit.','Hexagon',
	null,0,0,'Cable Pull Tension',null,4,8454144,0,0,1,null,'ccCablePullTension:GTechnology.Oncor.CustomAPI.ccCablePullTension',null);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=91;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=91;

-- Tools->Design Tools->Guying Program
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=92 or G3E_USERNAME='Guying' or G3E_USERNAME='Guying Program';
Insert into G3E_CUSTOMCOMMAND (
	G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
	G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) 
  values (92,'Guying Program','Calculates appropriate guy and anchor specifications to support a Pole in various installation scenarios.','Hexagon',
	null,0,0,'Guying',null,4,8912912,0,0,1,null,'ccGuying:GTechnology.Oncor.CustomAPI.ccGuying',null);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=92;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=92;

-- Tools->Design Tools->Sag Clearance
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=93 or G3E_USERNAME='Sag Clearance';
Insert into G3E_CUSTOMCOMMAND (
	G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
	G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) 
  values (93,'Sag Clearance','Calculates the minimum clearance of a Conductor.','Hexagon',null,
	0,0,'Sag Clearance',null,4,9437200,0,0,1,null,'ccSagClearance:GTechnology.Oncor.CustomAPI.ccSagClearance',null);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=93;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=93;

-- Tools->Design Tools->Secondary Calculator
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=94 or G3E_USERNAME='Secondary Calculator';
Insert into G3E_CUSTOMCOMMAND (
	G3E_CCNO,G3E_USERNAME,G3E_DESCRIPTION,G3E_AUTHOR,G3E_COMMENTS,G3E_LARGEBITMAP,G3E_SMALLBITMAP,G3E_TOOLTIP,G3E_STATUSBARTEXT,G3E_COMMANDCLASS,
	G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LOCALECOMMENT,G3E_INTERFACE,G3E_AONO) 
  values (94,'Secondary Calculator','Calculates the flicker and voltage drop for a single, user selected secondary network.','Hexagon',null,
	0,0,'Secondary Calculator',null,4,9437200,0,0,1,null,'ccSecondaryCalculator:GTechnology.Oncor.CustomAPI.ccSecondaryCalculator',null);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=94;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=94;

-- Tools->Design Tools->Street Light Voltage Drop
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=95 or G3E_USERNAME='Street Light Voltage Drop' or G3E_USERNAME='Street Light Voltage Drop Calculator';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (95,'Street Light Voltage Drop','ccStreetLightVoltageDrop:GTechnology.Oncor.CustomAPI.ccStreetLightVoltageDrop','Street Light Voltage Drop','Hexagon',
          4,9502736,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=95;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=95;

-- Tools->Joint Use->Edit NJUNS Ticket Data
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=96 or G3E_USERNAME = 'Edit NJUNS Ticket Data';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (96,'Edit NJUNS Ticket Data','fkqEditNJUNSTicket:GTechnology.Oncor.CustomAPI.ccNJUNSTicket','Edit NJUNS Ticket Data','Hexagon',
          4,1114128,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=96;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=96;

-- Tools->Joint Use->Import Attachments
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=99 or G3E_USERNAME = 'Import Attachments';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (99,'Import Attachments','ccImportAttachCSV:ccImportAttachCSV.ccImportAttachCSV','Import attachment from a CSV file. Requires the PRIV_MGMT_JU role.','Hexagon',
          4,65552,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=99;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_JU' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=99;

-- Tools->Request Tree Trimming Estimate
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=88 or G3E_USERNAME='Request Tree Trimming Estimate';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (88,'Request Tree Trimming Estimate','ccTreeTrimRequestEstimate:GTechnology.Oncor.CustomAPI.ccTreeTrimRequestEstimate','Custom placement for the Tree Trimming Voucher feature class,includes automated form and plot generation for submission of a request for estimate to Vegetation Management','Hexagon',
          1,8388624,0,0,1,8,8);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=88;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=88;

-- Tools->Service->Field Activity
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=58 or G3E_USERNAME='Field Activity';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (58,'Field Activity','ccFieldActivity:GTechnology.Oncor.CustomAPI.ccFieldActivity','Sets activity codes for selected features.','Hexagon',
          4,8454160,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=58;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_FA_CORRECTIONS' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=58;

-- Tools->Service->Field Activity Error Resolution
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=56 or G3E_USERNAME='Field Activity Error Resolution';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (56,'Field Activity Error Resolution','ccFieldActivityErrorResolution:GTechnology.Oncor.CustomAPI.ccFieldActivityErrorResolution','Correct and resubmit the errors associated with the field activity processing','Hexagon',
          4,8454144,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=56;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_FA_CORRECTIONS' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=56;

-- Tools->Service->Install Service Line
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=57 or G3E_USERNAME='Install Service Line';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (57,'Install Service Line','ccServiceLineInstall:GTechnology.Oncor.CustomAPI.ccServiceLineInstall','Installs a new service line.','Hexagon',
          4,8454160,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=57;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_FA_CORRECTIONS' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=57;

-- Tools->Service->Request ESI Location
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=55 or G3E_USERNAME='Request ESI Location';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (55,'Request ESI Location','ccRequestESILocation:GTechnology.Oncor.CustomAPI.ccRequestESILocation','Requests an ESI Location for a Premise','Hexagon',
          4,8454160,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=55;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_EDIT' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=55;

-- Tools->Street Light->Attach Supplemental Agreement Plot
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=68 or G3E_USERNAME='Attach Supplemental Agreement Plot';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (68,'Attach Supplemental Agreement Plot','ccAttachSupplementalAgreementPlot:GTechnology.Oncor.CustomAPI.ccAttachSupplementalAgreementPlot','Attach Supplemental Agreement Plot','Hexagon',
          4,8388656,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=68;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=68;

-- Tools->Street Light->Generate Supplemental Agreement Plot
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=76 or G3E_USERNAME='Generate Supplemental Agreement Plot';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (76,'Generate Supplemental Agreement Plot','ccSupplementalAgreementPlot:GTechnology.Oncor.CustomAPI.ccSupplementalAgreementPlot','Generate Supplemental Agreement Plot','Hexagon',
          4,8912912,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=76;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=76;

-- Tools->Street Light->Import Street Lights
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=70 or G3E_USERNAME='Import Street Lights';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (70,'Import Street Lights','ccStreetLightImportTool:GTechnology.Oncor.CustomAPI.ccStreetLightImportTool','The import of customer-owned Street Light data will be accomplished using this custom command','Hexagon',
          1,8388624,0,0,4,8,8);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=70;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_STLT' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=70;

-- Tools->Street Light->Street Light Account Editor
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=64 or G3E_USERNAME='Street Light Account Editor';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (64,'Street Light Account Editor','ccStreetLightAcctEditor:GTechnology.Oncor.CustomAPI.ccStreetLightAcctEditor','Street Light Account Editor','Hexagon',
          1,8388624,0,0,1,3,3);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=64;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_STLT' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=64;

-- Tools->Street Light->Street Light History
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=77 or G3E_USERNAME='Street Light Location History';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (77,'Street Light Location History','ccStreetLightHistory:GTechnology.Oncor.CustomAPI.StreetLightHistory','Streetlight location history.','Hexagon',
          0,16,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=77;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_MGMT_STLT' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=77;

-- Tools->Street Light->Supplemental Agreement
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=75 or G3E_USERNAME='Supplemental Agreement';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (75,'Supplemental Agreement','ccSupplementalAgreementForms:GTechnology.Oncor.CustomAPI.ccSupplementalAgreementForms','Supplemental Agreement','Hexagon',
          4,8912912,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=75;
update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=75;


--
-- GT Plot Commands
--

-- GTNewPlotWindow
--delete from G3E_CUSTOMCOMMAND where G3E_CCNO=110 or G3E_USERNAME='New Plot Window...';
--insert into G3E_CUSTOMCOMMAND
--   (G3E_CCNO, G3E_INTERFACE, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL)
-- Values
--   (110, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.NewPlotWindow', 'New Plot Window...', 'Custom command used to create new Plot Windows', 'Paul Adams', 'Intergraph Canada Ltd.', 
--   0, 0, 'New Plot Window...', 1, 0, 0, 0, 1);
--update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=110;
--update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=110;

---- WorkspacePlots
--delete from G3E_CUSTOMCOMMAND where G3E_CCNO=111 or G3E_USERNAME='Workspace Plots...';
--insert into G3E_CUSTOMCOMMAND
--   (G3E_CCNO, G3E_INTERFACE, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL)
-- values
--   (111, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.WorkspacePlots', 'Workspace Plots...', 'Custom command used to manage Workspace Plots', 'Paul Adams', 'Intergraph Canada Ltd.',
--   0, 0, 'Workspace Plots...', 1, 0, 0, 0, 1);
--update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=111;
--update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=111;

---- PrintActiveMapWindow
--delete from G3E_CUSTOMCOMMAND where G3E_CCNO=112 or G3E_USERNAME='Print Active Map Window...' or G3E_USERNAME = 'Print Active Map Window';
--insert into G3E_CUSTOMCOMMAND
--   (G3E_CCNO, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL, G3E_INTERFACE)
-- values
--   (112, 'Print Active Map Window', 'Custom command used to Print the Active Map Windows', 'Paul Adams', 'Intergraph Canada Ltd.', 0, 0, 'Print Active Map Window...', 1, 0, 0, 0, 1, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.PrintActiveMapWindow');
--update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=112;
--update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=112;

---- PrintActiveMapWindowSettings
--delete from G3E_CUSTOMCOMMAND where G3E_CCNO=113 or G3E_USERNAME='Print Active Map Window Settings...';
--Insert into G3E_CUSTOMCOMMAND
--   (G3E_CCNO, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL, G3E_INTERFACE)
-- Values
--   (113, 'Print Active Map Window Settings...', 'Custom command used to show the Print Active Map Windows Settings', 'Paul Adams', 'Intergraph Canada Ltd.', 0, 0, 'Print Active Map Window Settings...', 1, 0, 0, 0, 1, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.PrintActiveMapWindowSettings');
--update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=113;
--update G3E_ROLEASSOCIATION set G3E_ROLE='PRIV_DESIGN_ALL' where G3E_TYPE='CustomCommand' and G3E_OBJECTCONTROLID=113;

--
-- AUTO START COMMANDS (no role association necessary)
--

-- GIS Automation Broker
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=53 or G3E_USERNAME='GIS Automation Broker';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (53,'GIS Automation Broker','ccGISAutomationBroker:GTechnology.Oncor.CustomAPI.ccGISAutomationBroker','The command polls the automation queue for pending requests and delegates those requests to the appropriate plug-in module. The command is launched by the GISAutomationMonitor.','Hexagon',
          4,8388624,0,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=53;

-- Session Management
delete from G3E_CUSTOMCOMMAND where G3E_CCNO=52 or G3E_USERNAME='Session Management';
insert into G3E_CUSTOMCOMMAND (
          G3E_CCNO,G3E_USERNAME,G3E_INTERFACE,G3E_DESCRIPTION,G3E_AUTHOR,
          G3E_COMMANDCLASS,G3E_ENABLINGMASK,G3E_MODALITY,G3E_SELECTSETENABLINGMASK,G3E_MENUORDINAL,G3E_LARGEBITMAP,G3E_SMALLBITMAP) 
  values (52,'Session Management','ccSessionManagement:GTechnology.Oncor.CustomAPI.ccSessionManagement','Auto Start Command','Hexagon',
          0,0,1,0,1,0,0);
update G3E_CUSTOMCOMMAND set G3E_TOOLTIP = G3E_USERNAME, G3E_STATUSBARTEXT = G3E_DESCRIPTION where G3E_CCNO=52;

commit; 

spool off;
