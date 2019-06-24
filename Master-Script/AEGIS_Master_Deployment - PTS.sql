set echo on 

spool AEGIS_Master_Deployment.log

#Tables

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Tables/ETL_PROCESS_CONTROL.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Tables/GIS_PURGE.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Tables/INTERFACE_DELETES.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Tables/INTERFACE_DELETES_REF_SEQ.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Tables/INTERFACE_LOG.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_CATEGORY.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_CONSTRUCTION_UNIT.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_MATERIAL.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_STANDARD_ATTR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_UNIT_ASSEMBLY.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_UNIT_ATTRIBUTE.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Tables/STG_CULIB_VALID_ATTR_VAL.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-VOUCHER-DDL/Tables/STG_FERC_ACCOUNT.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-WR-DDL/Tables/STG_COUNTY_CODE.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-VOUCHER-DDL/Tables/STG_VOUCHER_CONTRIBUTION_TYPE.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-WR-DDL/Tables/STG_TOWN_CODE.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-WR-DDL/Tables/STG_UTILITY_THIRD_PARTY.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-MAXIMO-WO-DDL/Tables/STG_MAXIMO_WORKORDER.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-VINTAGE-YEAR-DDL/Tables/STG_VINTAGE_YEAR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-TRC-ACTIVE-ATTACHMENTS/Tables/STG_TRC_ACTIVE_ATTACHMENTS.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/OPBNDY/OPERBNDY_LOOKUP.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-T811-DDL/STG_TX811.sql"

#Views

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Views/STG_CULIB_MACRO_UNIT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Views/STG_CULIB_UNITATTRIBUTE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/SLOTS_views/AREA_LIGHT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/SLOTS_views/STL_POLES_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/SLOTS_views/STREETLIGHT_CONTROL_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/SLOTS_views/STREETLIGHT_STANDARD_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/SLOTS_views/STREETLIGHT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/AMS_COLLECTORS_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/AMS_ROUTERS_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/AUTO_SWITCHGEARS_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/AUTO_TRANSFORMERS_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/BREAKER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/CAPACITOR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/CONDUIT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/CONNECTIVITY_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/FUSE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/GUY_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/JUNCTION_POINT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/MANHOLE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/OH_TRANSFORMER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PADS_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/POLE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_ENCLOSURE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_PULL_BOX_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_SWITCHGEAR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_UG_COND_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRIMARY_BOX_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRIMARY_OVERHEAD_CONDUCTOR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/RECLOSER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/RISER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SEC_BOX_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SEC_ENCLOSURE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SEC_SWITCHGEAR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SECONDARY_CONNECTIVITY_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SERVICE_LINE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SUBSTATION_BREAKER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SUBSTATION_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/TRANS_TOWER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/UG_TRANSFORMER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/VAULTS_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/VOLTAGE_REGULATOR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SEC_COND_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SEC_WIRE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/OH_XFMR_BANK_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/OH_XFMR_UNIT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_OH_COND_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_OH_WIRE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/OPBNDY/OBERBNDY_COND_STRC_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/OPBNDY/OPERBNDY_FEEDER_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/OPBNDY/OPERBNDY_SVCCTR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/ATTACH_EQPT_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/ATTACH_WIRELINE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PRI_UG_WIRE_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/PAD_MOUNT_XFMR_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS_ONC-Shared-Feature-Views/SWITCH_VW.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-REQUEST-WR-ESTIMATE/Views/WRVIEW_VOUCHER.sql"

#Triggers

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Triggers/BIU_ETL_PROCESS_CTRL_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Triggers/BIU_GIS_PURGE_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Triggers/BIU_INTERFACE_DELETES_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Triggers/BIU_INTERFACE_LOG_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Triggers/BIU_INT_DELETES_REF_SEQ_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_CATEGORY_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_CU_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_MATERIAL_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_STD_ATTR_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_ASSEMBLY_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_UNIT_ATTR_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-CULIB-DDL/Triggers/BIU_STG_CULIB_VALID_VAL_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-VOUCHER-DDL/Triggers/BIU_STG_FERC_ACCOUNT_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-VOUCHER-DDL/Triggers/BIU_STG_VCHR_CONT_TYPE_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-WR-DDL/Triggers/BIU_STG_COUNTY_CODE_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-WR-DDL/Triggers/BIU_STG_TOWN_CODE_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-REF-WR-DDL/Triggers/BIU_STG_UTIL_THIRD_PARTY_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-MAXIMO-WO-DDL/Triggers/BIU_STG_MAX_WORKORDER_TBL_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-REF-VOUCHER-DDL/Triggers/BIU_REFWMIS_FERC_ACCOUNT_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-REF-VOUCHER-DDL/Triggers/BIU_REFWMIS_VOUCHER_TYPE_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-REF-WR-DDL/Triggers/BIU_REFWMIS_COUNTY_CODE_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-REF-WR-DDL/Triggers/BIU_REFWMIS_POWER_COMPANY_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-REF-WR-DDL/Triggers/BIU_REFWMIS_TOWN_CODE_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-TRC-ACTIVE-ATTACHMENTS/Triggers/BIU_STG_TRC_ACT_ATTACH_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-VINTAGE-YEAR-DDL/Triggers/BIU_STG_VINTAGE_YEAR_TR.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-T811-DDL/BIU_STG_TX811_TR.sql"

#Sequences

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-T811-DDL/STG_TX811_SEQ.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Sequences/AUD_ETL_CONTROL_SEQ.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/OPBNDY/OPERBNDY_FID_SEQ.sql"

#Packages

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Packages/ETL_CONTROL_PKG.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Packages/ETL_CONTROL_PKG Body.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Packages/ETL_INTERFACE_PKG.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-ONC-Shared-DDL/Packages/ETL_INTERFACE_PKG Body.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/GIS-STG-VINTAGE-YEAR/Packages/GISPKG_WMIS_ACCOUNTING.sql"

#Grants

@"/data/app/oracle/admin/Releases/RELEASE_ID/Master-Script/AEGIS_GIS_INFA_GRANTS.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/Master-Script/AEGIS_GIS_ONC_GRANTS.sql"

@"/data/app/oracle/admin/Releases/RELEASE_ID/Master-Script/AEGIS_GIS_STG_GRANTS.sql"

#Other

@"/data/app/oracle/admin/Releases/RELEASE_ID/Master-Script/AEGIS_GIS_INFA_ACL.sql" WV00452.test.corp.oncor.com

@"/data/app/oracle/admin/Releases/RELEASE_ID/Master-Script/AEGIS_SYS_GENERALPARAMETER_EdgeFrontier_Hosts.sql" WV00452.test.corp.oncor.com

spool off