set echo on
set feedback on

spool GIS_ONC_Deployment_Script.log

-- Drop Script
@GIS_ONC_Drop_Script_11.29.sql

-- Tables
@OPBNDY/Tables/OPERBNDY_LOOKUP.sql
@Streetlight/DDL/GIS_ONC.STLT_BOUNDARY.sql

-- Views
	-- Feature Views
@GIS_ONC-Shared-Feature-Views/AMS_COLLECTORS_VW.sql
@GIS_ONC-Shared-Feature-Views/AMS_ROUTERS_VW.sql
@GIS_ONC-Shared-Feature-Views/ATTACH_EQPT_VW.sql
@GIS_ONC-Shared-Feature-Views/ATTACH_WIRELINE_VW.sql
@GIS_ONC-Shared-Feature-Views/AUTO_SWITCHGEARS_VW.sql
@GIS_ONC-Shared-Feature-Views/AUTO_XFMR_BANK_VW.sql
@GIS_ONC-Shared-Feature-Views/AUTO_XFMR_UNIT_VW.sql
@GIS_ONC-Shared-Feature-Views/BREAKER_VW.sql
@GIS_ONC-Shared-Feature-Views/CAPACITOR_VW.sql
@GIS_ONC-Shared-Feature-Views/CONDUIT_VW.sql
@GIS_ONC-Shared-Feature-Views/CONNECTIVITY_VW.sql
@GIS_ONC-Shared-Feature-Views/FUSE_VW.sql
@GIS_ONC-Shared-Feature-Views/GUY_VW.sql
@GIS_ONC-Shared-Feature-Views/JUNCTION_POINT_VW.sql
@GIS_ONC-Shared-Feature-Views/MANHOLE_VW.sql
@GIS_ONC-Shared-Feature-Views/OH_XFMR_BANK_VW.sql
@GIS_ONC-Shared-Feature-Views/OH_XFMR_UNIT_VW.sql
@GIS_ONC-Shared-Feature-Views/PADS_VW.sql
@GIS_ONC-Shared-Feature-Views/POLE_VW.sql
@GIS_ONC-Shared-Feature-Views/PRIMARY_BOX_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_ENCLOSURE_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_OH_COND_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_OH_WIRE_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_PULL_BOX_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_SWITCHGEAR_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_UG_COND_VW.sql
@GIS_ONC-Shared-Feature-Views/PRI_UG_WIRE_VW.sql
@GIS_ONC-Shared-Feature-Views/RECLOSER_VW.sql
@GIS_ONC-Shared-Feature-Views/RISER_VW.sql
@GIS_ONC-Shared-Feature-Views/SECONDARY_CONNECTIVITY_VW.sql
@GIS_ONC-Shared-Feature-Views/SEC_BOX_VW.sql
@GIS_ONC-Shared-Feature-Views/SEC_COND_VW.sql
@GIS_ONC-Shared-Feature-Views/SEC_ENCLOSURE_VW.sql
@GIS_ONC-Shared-Feature-Views/SEC_SWITCHGEAR_VW.sql
@GIS_ONC-Shared-Feature-Views/SEC_WIRE_VW.sql
@GIS_ONC-Shared-Feature-Views/SERVICE_LINE_VW.sql
@GIS_ONC-Shared-Feature-Views/SUBSTATION_BREAKER_VW.sql
@GIS_ONC-Shared-Feature-Views/SUBSTATION_VW.sql
@GIS_ONC-Shared-Feature-Views/SWITCH_VW.sql
@GIS_ONC-Shared-Feature-Views/TRANS_TOWER_VW.sql
@GIS_ONC-Shared-Feature-Views/UG_TRANSFORMER_VW.sql
@GIS_ONC-Shared-Feature-Views/VAULTS_VW.sql
@GIS_ONC-Shared-Feature-Views/VOLTAGE_REGULATOR_VW.sql
	-- OPBNDY Views
@OPBNDY/Views/OBERBNDY_COND_STRC_VW.sql
@OPBNDY/Views/OPERBNDY_FEEDER_VW.sql
@OPBNDY/Views/OPERBNDY_SVCCTR_VW.sql

-- Sequences
@GIS-ONC-T811-DDL/FME_INDEX_SEQ.sql
@OPBNDY/Sequences/OPERBNDY_FID_SEQ.sql