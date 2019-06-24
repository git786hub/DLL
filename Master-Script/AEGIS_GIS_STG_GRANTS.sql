rem CREATE USER GIS_STG IDENTIFIED BY <password> DEFAULT TABLESPACE GIS_TS TEMPORARY TABLESPACE TEMP PROFILE SERVICE_PROFILE ACCOUNT UNLOCK ;
  -- 6 Roles for GIS_STG 
  GRANT ADMINISTRATOR TO GIS_STG WITH ADMIN OPTION;
  GRANT CONNECT TO GIS_STG WITH ADMIN OPTION;
  GRANT DESIGNER TO GIS_STG;
  GRANT IMP_FULL_DATABASE TO GIS_STG;
  ALTER USER GIS_STG DEFAULT ROLE ALL;
  -- 29 System Privileges for GIS_STG 
  GRANT ALTER ANY PROCEDURE TO GIS_STG;
  GRANT CREATE ANY CONTEXT TO GIS_STG;
  GRANT CREATE ANY INDEX TO GIS_STG;
  GRANT CREATE ANY PROCEDURE TO GIS_STG;
  GRANT CREATE ANY TRIGGER TO GIS_STG;
  GRANT CREATE PROCEDURE TO GIS_STG;
  GRANT CREATE PUBLIC DATABASE LINK TO GIS_STG;
  GRANT CREATE PUBLIC SYNONYM TO GIS_STG;
  GRANT CREATE ROLE TO GIS_STG;
  GRANT CREATE SEQUENCE TO GIS_STG;
  GRANT CREATE SESSION TO GIS_STG;
  GRANT CREATE TABLE TO GIS_STG;
  GRANT CREATE TRIGGER TO GIS_STG;
  GRANT CREATE TYPE TO GIS_STG;
  GRANT CREATE VIEW TO GIS_STG;
  GRANT DROP ANY CONTEXT TO GIS_STG;
  GRANT DROP ANY ROLE TO GIS_STG;
  GRANT DROP PUBLIC DATABASE LINK TO GIS_STG;
  GRANT DROP PUBLIC SYNONYM TO GIS_STG;
  GRANT EXECUTE ANY PROCEDURE TO GIS_STG;
  GRANT EXECUTE ANY TYPE TO GIS_STG;
  GRANT GRANT ANY OBJECT PRIVILEGE TO GIS_STG;
  GRANT GRANT ANY PRIVILEGE TO GIS_STG;
  GRANT GRANT ANY ROLE TO GIS_STG;
  GRANT SELECT ANY DICTIONARY TO GIS_STG;
  GRANT SELECT ANY SEQUENCE TO GIS_STG;
  GRANT SELECT ANY TABLE TO GIS_STG;
  GRANT UNLIMITED TABLESPACE TO GIS_STG WITH ADMIN OPTION;
  GRANT UPDATE ANY TABLE TO GIS_STG;
  -- 1 Tablespace Quota for GIS_STG 
  ALTER USER GIS_STG QUOTA UNLIMITED ON USERS;
  -- 4 Object Privileges for GIS_STG 
    GRANT DELETE, INSERT, SELECT, UPDATE ON GIS.ATTACH_EQPT_N TO GIS_STG;
    GRANT DELETE, INSERT, SELECT, UPDATE ON GIS.ATTACH_WIRELINE_N TO GIS_STG;
    GRANT DELETE, INSERT, SELECT ON GIS.MAXIMOWO_N TO GIS_STG;
    GRANT DELETE, INSERT, SELECT, UPDATE ON GIS.TRC_ACTIVE_PERMITS TO GIS_STG;
  -- 43 Privileges for GIS_STG from Grant Master
        GRANT SELECT ON GIS_STG.COND_STRUCTURES_VW TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.FEEDER_BND_VW TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.STG_CULIB_MACRO_UNIT_VW TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.STG_CULIB_UNITATTRIBUTE_VW TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.SVCCTR_BNDY_VW TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.WRVIEW_CU TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.WRVIEW_OVERRIDE TO GIS_STG_RW;
        GRANT SELECT ON GIS_STG.WRVIEW_WORKPOINT TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.CIS_ESI_LOCATIONS TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.CIS_ESI_LOCATIONS_LOG TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.ETL_BALANCE_CONTROL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.ETL_BATCH_CONTROL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.ETL_PROCESS_CONTROL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.GIS_PURGE TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.INTERFACE_CONTROL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.INTERFACE_LOG TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.INTERFACE_XML_DATA TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.LOG_TRC_IMPORT TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.MAXIMOIMPORT_ERR_LOG TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.OPERBNDY_LOOKUP TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_COUNTY_CODE TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_CATEGORY TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_CONSTRUCTION_UNIT TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_MATERIAL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_STANDARD_ATTR TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_UNIT_ASSEMBLY TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_UNIT_ATTRIBUTE TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_CULIB_VALID_ATTR_VAL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_FERC_ACCOUNT TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_MAXIMO_WORKORDER TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_OPERBNDY_AR TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_OPERBNDY_N TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_SERVICE_ACTIVITY TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_TOWN_CODE TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_TRC_ACTIVE_ATTACHMENTS TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_TRC_ACTIVE_PERMITS TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_TRC_ATTACHMENT TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_TRC_POLE_ATTACHMENT TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_UTILITY_THIRD_PARTY TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_VINTAGE_YEAR TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.STG_VOUCHER_CONTRIBUTION_TYPE TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.SUB_INTERFACE_CONTROL TO GIS_STG_RW;
        GRANT SELECT,INSERT,UPDATE,DELETE ON GIS_STG.TRC_ALTERED_ATTACH_TMP TO GIS_STG_RW;