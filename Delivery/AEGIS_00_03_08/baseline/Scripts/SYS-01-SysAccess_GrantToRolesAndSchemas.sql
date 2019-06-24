
set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\SysAccess_GrantToRolesAndSchemas.log
--**************************************************************************************
--SCRIPT NAME: SysAccess_GrantToRolesAndSchemas.sql
--**************************************************************************************
-- AUTHOR           : INGRNET\RRADASE
-- DATE             : 15-JUN-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Grant permissions to functional roles and other schemas
-- RUN AS				: SYS as SYSBDA
--**************************************************************************************
--
--  !!!   NOTE: RUN AS SYS USER WITH SYSDBA PRIVILEGES   !!!
--
--**************************************************************************************
-- Modified:
--  01-AUG-2018, Rich Adase -- Reorganized and added value list grants
--**************************************************************************************

--
-- SCHEMA GRANTS
--

-- GIS objects

GRANT SELECT, INSERT, UPDATE            ON GIS.G3E_JOB                      TO GIS_ONC WITH GRANT OPTION;

GRANT SELECT                            ON GIS.B$COMMON_N                   TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT, UPDATE                    ON GIS.B$PREMISE_N                  TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT, UPDATE                    ON GIS.B$STREETLIGHT_N              TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT                            ON GIS.LTT_IDENTIFIERS              TO GIS_ONC WITH GRANT OPTION;

GRANT SELECT, INSERT, UPDATE, DELETE    ON GIS.CUSELECT_USERPREF            TO GIS_ONC WITH GRANT OPTION;

GRANT EXECUTE                           ON G3E_MANAGEMODLOG                 TO GIS_ONC WITH GRANT OPTION;


-- GIS_ONC objects

grant SELECT    on GIS_ONC.CULIB_ATTRIBUTE          to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_CATEGORY           to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_MACRO              to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_MACROUNIT          to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_MATERIAL           to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_UNIT               to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_UNITATTRIBUTE      to GIS WITH GRANT OPTION;
grant SELECT    on GIS_ONC.CULIB_UNITMATERIAL       to GIS WITH GRANT OPTION;


-- GIS_STG objects

grant EXECUTE   on GIS_STG.MAXIMO_IMPORT            to GIS;


--
-- ROLE GRANTS
--

-- GIS objects

grant SELECT    on GIS.ASSET_HISTORY                to PRIV_EDIT;

grant SELECT    on GIS.CUSELECT_USERPREF            to PRIV_EDIT;
grant SELECT    on GIS.V_CUSELECTION_ACU_VL         to PRIV_EDIT;
grant SELECT    on GIS.V_CUSELECTION_AMCU_VL        to PRIV_EDIT;
grant SELECT    on GIS.V_CUSELECTION_CU_VL          to PRIV_EDIT;
grant SELECT    on GIS.V_CUSELECTION_MCU_VL         to PRIV_EDIT;

grant SELECT on GIS.VL_ADDRESS_PREFIX to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ADDRESS_PREFIX to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_AMSCOLL_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_AMSCOLL_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_AREALT_LAMP_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_AREALT_LAMP_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_AREALT_LAMP_USE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_AREALT_LAMP_USE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_AREALT_WATTAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_AREALT_WATTAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ARRESTOR_CLASS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ARRESTOR_CLASS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ARRESTOR_VOLTAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ARRESTOR_VOLTAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ATTACH_COMPANY to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ATTACH_COMPANY to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ATTACH_MAINT to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ATTACH_MAINT to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ATTACH_SOURCE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ATTACH_SOURCE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ATTACH_STATUS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ATTACH_STATUS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_AUTOXFMR_BANK_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_AUTOXFMR_BANK_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_BYPASS_FUSE_RATING to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_BYPASS_FUSE_RATING to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CAP_BANK_FUNCTION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CAP_BANK_FUNCTION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CAP_CONTROL_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CAP_CONTROL_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CIAC_CODE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CIAC_CODE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CIRCUIT_BREAKER_USE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CIRCUIT_BREAKER_USE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CNTY_FILING_LOC to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CNTY_FILING_LOC to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_COMMS_ATTACH_POSITION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_COMMS_ATTACH_POSITION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_COMMS_ATTACH_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_COMMS_ATTACH_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CONDUIT_USE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CONDUIT_USE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CONFIGURATION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CONFIGURATION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CORRECT_TAG_STATUS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CORRECT_TAG_STATUS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_COUNTY to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_COUNTY to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_CU_ACTIVITY to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_CU_ACTIVITY to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_DA_ANTENNA_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_DA_ANTENNA_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_DA_EQPT_POWER to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_DA_EQPT_POWER to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_DA_RADIO_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_DA_RADIO_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_DESIGN_RESP to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_DESIGN_RESP to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_DOCNOTE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_DOCNOTE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_DUCT_STATE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_DUCT_STATE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_EASEMENT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_EASEMENT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_EQP_ATTACH_POSITION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_EQP_ATTACH_POSITION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_EQP_ATTACH_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_EQP_ATTACH_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_EXCH to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_EXCH to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FACILITY_FRONT_CODE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FACILITY_FRONT_CODE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FEEDER_POSITION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FEEDER_POSITION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FEEDER_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FEEDER_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FEED_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FEED_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FLOOR_TYPES to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FLOOR_TYPES to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FORCABLE_POSITION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FORCABLE_POSITION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FORCABLE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FORCABLE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FORMATION_DUCT_SIZE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FORMATION_DUCT_SIZE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FORMATION_DUCT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FORMATION_DUCT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_FUSE_INTERRUPT_RATING to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_FUSE_INTERRUPT_RATING to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_GUY_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_GUY_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_HYPERLINK_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_HYPERLINK_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_INNER_DUCT_COLOR to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_INNER_DUCT_COLOR to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_INSULATION_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_INSULATION_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_INSULATION_VOLTAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_INSULATION_VOLTAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_LANDBASE_STAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_LANDBASE_STAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_LIFECYCLE_STATES to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_LIFECYCLE_STATES to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_LOC_GRADE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_LOC_GRADE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_MAINTAINED_BY to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_MAINTAINED_BY to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_MH_STACKING to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_MH_STACKING to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_MH_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_MH_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_MIC to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_MIC to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_MISC_STRUCTURE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_MISC_STRUCTURE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_MSNGR_ATTACH_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_MSNGR_ATTACH_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NETFUSE_INSIDEOUTSIDE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NETFUSE_INSIDEOUTSIDE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NETPROT_MOUNT to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NETPROT_MOUNT to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NETPROT_RELAY_MFR to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NETPROT_RELAY_MFR to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NETPROT_RELAY_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NETPROT_RELAY_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NETPROT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NETPROT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NETWORK_NBR to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NETWORK_NBR to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_NODE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_NODE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_OPERATIONAL_BND_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_OPERATIONAL_BND_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_OP_SERVICE_CENTER to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_OP_SERVICE_CENTER to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ORIENTATION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ORIENTATION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_OWNER_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_OWNER_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PAD_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PAD_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PAD_SIZE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PAD_SIZE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PARCEL_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PARCEL_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PARCEL_ZONING to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PARCEL_ZONING to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PERMIT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PERMIT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PHASE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PHASE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PHASE_POSITION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PHASE_POSITION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PIPELINE_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PIPELINE_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PIPELINE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PIPELINE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PLAT_SOURCE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PLAT_SOURCE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_POLE_CLASS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_POLE_CLASS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_POLE_CONST_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_POLE_CONST_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_POLE_HEIGHT to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_POLE_HEIGHT to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_POLE_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_POLE_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_POLE_OWNER to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_POLE_OWNER to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_POLITICAL_BND_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_POLITICAL_BND_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PPOD_CLASS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PPOD_CLASS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PREMISE_DWELLING_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PREMISE_DWELLING_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PULLBOX_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PULLBOX_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PULLBOX_SIZE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PULLBOX_SIZE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_PULLBOX_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_PULLBOX_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RACK_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RACK_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RACK_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RACK_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RECLOSER_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RECLOSER_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RETIREMENT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RETIREMENT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RIGHTWAY_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RIGHTWAY_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RISER_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RISER_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_RISER_USAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_RISER_USAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_ROOM_TYPES to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_ROOM_TYPES to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SCADA_DEVICE_FUNCTION to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SCADA_DEVICE_FUNCTION to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SECBOX_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SECBOX_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SECBOX_SIZE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SECBOX_SIZE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SECBOX_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SECBOX_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SECBOX_USE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SECBOX_USE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SECCONDNODE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SECCONDNODE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SECCOND_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SECCOND_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SERVICEPOINT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SERVICEPOINT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SERVICE_CODE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SERVICE_CODE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SERVICE_PLACEMENT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SERVICE_PLACEMENT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SSWG_ENCLOSURE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SSWG_ENCLOSURE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_STATUS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_STATUS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_STLT_CONN_STATUS to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_STLT_CONN_STATUS to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_STLT_LAMP_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_STLT_LAMP_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_STLT_WATTAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_STLT_WATTAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_STREET_DIR to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_STREET_DIR to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SUBSTATION_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SUBSTATION_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SUB_XFMR_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SUB_XFMR_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SWITCH_BUSHING_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SWITCH_BUSHING_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SWITCH_CONTROL_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SWITCH_CONTROL_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SWITCH_MEDIUM_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SWITCH_MEDIUM_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_SWITCH_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_SWITCH_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TELECOM_ANT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TELECOM_ANT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TELECOM_RADIO_SYSTEM to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TELECOM_RADIO_SYSTEM to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TRAFFIC_ACCOUNT to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TRAFFIC_ACCOUNT to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TRAFFIC_CITY to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TRAFFIC_CITY to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TRAFFIC_RATE_CODE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TRAFFIC_RATE_CODE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TRANS_TOWER_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TRANS_TOWER_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_TRIP_CURRRENT_RATING to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_TRIP_CURRRENT_RATING to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_VAULT_ACC_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_VAULT_ACC_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_VAULT_EQPT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_VAULT_EQPT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_VAULT_VOLTAGE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_VAULT_VOLTAGE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_WIRE_MATERIAL to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_WIRE_MATERIAL to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_WIRE_PHASE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_WIRE_PHASE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_WIRE_SIZE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_WIRE_SIZE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_WIRE_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_WIRE_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_XFMR_BANK_CONFIG to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_XFMR_BANK_CONFIG to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_XFMR_BANK_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_XFMR_BANK_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_XFMR_PROTECTION_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_XFMR_PROTECTION_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_XFMR_PROT_TYPE to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_XFMR_PROT_TYPE to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;
grant SELECT on GIS.VL_YES_NO to EVERYONE;   grant SELECT, INSERT, UPDATE, DELETE on GIS.VL_YES_NO to INTERFACE, PRIV_SUPPORT, PRIV_ADMIN;

grant EXECUTE on GIS.LBM_UTL to PRIV_MGMT_LAND;

-- GIS_ONC objects

grant SELECT, INSERT, UPDATE            on GIS_ONC.COMMAND_LOG              to EVERYONE;
grant DELETE                            on GIS_ONC.COMMAND_LOG              to PRIV_SUPPORT;

grant SELECT    on GIS_ONC.CULIB_ATTRIBUTE          to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_CATEGORY           to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_MACRO              to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_MACROUNIT          to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_MATERIAL           to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_UNIT               to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_UNITATTRIBUTE      to PRIV_EDIT;
grant SELECT    on GIS_ONC.CULIB_UNITMATERIAL       to PRIV_EDIT;
grant SELECT    on GIS_ONC.WR_VALIDATION_RULE       to PRIV_EDIT;

GRANT SELECT    ON GIS_ONC.FUSE_COORDINATION        TO EVERYONE;

GRANT EXECUTE   ON GIS_ONC.GISPKG_WMIS_ACCOUNTING   TO PRIV_SUPPORT, PRIV_ADMIN;
GRANT EXECUTE   ON GIS_ONC.GISPKG_WMIS_CULIBRARY    TO PRIV_SUPPORT, PRIV_ADMIN;
GRANT EXECUTE   ON GIS_ONC.GISPKG_WMIS_WR           TO PRIV_SUPPORT, PRIV_ADMIN;

-- GIS_STG objects

GRANT SELECT    ON GIS_STG.STG_VINTAGE_YEAR         TO PRIV_SUPPORT, PRIV_ADMIN;


spool off;