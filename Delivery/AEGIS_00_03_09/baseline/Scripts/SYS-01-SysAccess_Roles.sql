set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\SysAccess_Roles.log

--**************************************************************************************
-- SCRIPT NAME: SysAccess_Roles.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 05-APR-2018
-- PRODUCT VERSION: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Business and Functional roles (per System Access DDD)
-- RUN AS				: SYS as SYSBDA
--**************************************************************************************
--  10-MAY-2018, Rich Adase: Granted PRIV_EDIT to all business roles (except VIEWONLY)
--                           Added note to run as SYS user
--  08-AUG-2018, Pramod Kurella: Added role PRIV_MGMT_RESTRICTION
--  08-AUG-2018, Rich Adase: Changed approach to only create missing roles, no longer drop and recreate
--**************************************************************************************


declare
  PROCEDURE CreateRole (roleName IN VARCHAR2) IS
    objCount NUMBER;
  begin
    select count(*) into objCount from dba_roles where ROLE = roleName;
    if (objCount = 0) then
      execute immediate 'create role '||roleName;
    end if;
  end;
begin

  -- FUNCTIONAL ROLES

  CreateRole('PRIV_ADMIN');
  CreateRole('PRIV_ADV_QUERY');
  CreateRole('PRIV_CATALOG');
  CreateRole('PRIV_DESIGN_ALL');
  CreateRole('PRIV_DESIGN_FIBER');
  CreateRole('PRIV_DESIGN_NET');
  CreateRole('PRIV_EDIT');
  CreateRole('PRIV_FA_CORRECTIONS');
  CreateRole('PRIV_MAINTENANCE');
  CreateRole('PRIV_MGMT_JU');
  CreateRole('PRIV_MGMT_LAND');
  CreateRole('PRIV_MGMT_SERVICE');
  CreateRole('PRIV_MGMT_SSTA');
  CreateRole('PRIV_MGMT_STLT');
  CreateRole('PRIV_MGMT_VALUELIST');
  CreateRole('PRIV_OMS_CONNECTIVITY');
  CreateRole('PRIV_SUPPORT');
  CreateRole('PRIV_TRACE_UPDATE');
  CreateRole('PRIV_MGMT_RESTRICTION');
  
  -- BUSINESS ROLES
  
  CreateRole('CORRECTIONS');
  CreateRole('ELEC_DESIGN');
  CreateRole('ELEC_DESIGN_ADV');
  CreateRole('ELEC_DESIGN_NET');
  CreateRole('FIBER_ADMIN');
  CreateRole('FIBER_CONNECTIVITY');
  CreateRole('FIBER_DESIGN');
  CreateRole('JU_ADMIN');
  CreateRole('LAND_MGMT');
  CreateRole('OPERATIONS');
  CreateRole('STLT_ADMIN');
  CreateRole('SUPPORT');
  CreateRole('SYS_ADMIN');
  CreateRole('VIEWONLY');

  -- SPECIAL ROLES

  -- CreateRole('EVERYONE'); -- dropping and recreating this role requires re-granting to all users in order to connect; better to leave it in place (Rich Adase, 11-MAY-2018)
  CreateRole('NOBODY');

  -- INTERFACE ROLES
  CreateRole('PRIV_INT_CCB');
  CreateRole('PRIV_INT_DEIS');

end;
/

-- Grant functional roles to business roles

grant PRIV_FA_CORRECTIONS     to CORRECTIONS;
grant PRIV_MAINTENANCE        to CORRECTIONS;

grant PRIV_DESIGN_ALL         to ELEC_DESIGN;
grant PRIV_EDIT               to ELEC_DESIGN;
grant PRIV_TRACE_UPDATE       to ELEC_DESIGN;

grant PRIV_DESIGN_ALL         to ELEC_DESIGN_ADV;
grant PRIV_EDIT               to ELEC_DESIGN_ADV;
grant PRIV_MGMT_SSTA          to ELEC_DESIGN_ADV;
grant PRIV_OMS_CONNECTIVITY   to ELEC_DESIGN_ADV;
grant PRIV_TRACE_UPDATE       to ELEC_DESIGN_ADV;

grant PRIV_DESIGN_ALL         to ELEC_DESIGN_NET;
grant PRIV_DESIGN_NET         to ELEC_DESIGN_NET;
grant PRIV_EDIT               to ELEC_DESIGN_NET;
grant PRIV_TRACE_UPDATE       to ELEC_DESIGN_NET;

grant PRIV_DESIGN_ALL         to FIBER_ADMIN;
grant PRIV_DESIGN_FIBER       to FIBER_ADMIN;
grant PRIV_EDIT               to FIBER_ADMIN;

grant PRIV_EDIT               to FIBER_CONNECTIVITY;

grant PRIV_DESIGN_ALL         to FIBER_DESIGN;
grant PRIV_DESIGN_FIBER       to FIBER_DESIGN;
grant PRIV_EDIT               to FIBER_DESIGN;

grant PRIV_EDIT               to JU_ADMIN;
grant PRIV_MGMT_JU            to JU_ADMIN;
grant PRIV_MGMT_VALUELIST     to JU_ADMIN;

grant PRIV_DESIGN_ALL         to LAND_MGMT;
grant PRIV_EDIT               to LAND_MGMT;
grant PRIV_MAINTENANCE        to LAND_MGMT;
grant PRIV_MGMT_LAND          to LAND_MGMT;

grant PRIV_EDIT               to OPERATIONS;
grant PRIV_MAINTENANCE        to OPERATIONS;
grant PRIV_TRACE_UPDATE       to OPERATIONS;

grant PRIV_DESIGN_ALL         to STLT_ADMIN;
grant PRIV_EDIT               to STLT_ADMIN;
grant PRIV_MGMT_STLT          to STLT_ADMIN;
grant PRIV_MGMT_VALUELIST     to STLT_ADMIN;
grant PRIV_TRACE_UPDATE       to STLT_ADMIN;

grant PRIV_ADV_QUERY          to SUPPORT;
grant PRIV_CATALOG            to SUPPORT;
grant PRIV_DESIGN_ALL         to SUPPORT;
grant PRIV_EDIT               to SUPPORT;
grant PRIV_MAINTENANCE        to SUPPORT;
grant PRIV_MGMT_LAND          to SUPPORT;
grant PRIV_MGMT_VALUELIST     to SUPPORT;
grant PRIV_SUPPORT            to SUPPORT;
grant PRIV_TRACE_UPDATE       to SUPPORT;
grant PRIV_MGMT_RESTRICTION   to SUPPORT;

grant PRIV_ADMIN              to SYS_ADMIN;
grant PRIV_ADV_QUERY          to SYS_ADMIN;
grant PRIV_CATALOG            to SYS_ADMIN;
grant PRIV_DESIGN_ALL         to SYS_ADMIN;
grant PRIV_DESIGN_FIBER       to SYS_ADMIN;
grant PRIV_DESIGN_NET         to SYS_ADMIN;
grant PRIV_EDIT               to SYS_ADMIN;
grant PRIV_FA_CORRECTIONS     to SYS_ADMIN;
grant PRIV_MAINTENANCE        to SYS_ADMIN;
grant PRIV_MGMT_JU            to SYS_ADMIN;
grant PRIV_MGMT_LAND          to SYS_ADMIN;
grant PRIV_MGMT_SERVICE       to SYS_ADMIN;
grant PRIV_MGMT_SSTA          to SYS_ADMIN;
grant PRIV_MGMT_STLT          to SYS_ADMIN;
grant PRIV_MGMT_VALUELIST     to SYS_ADMIN;
grant PRIV_OMS_CONNECTIVITY   to SYS_ADMIN;
grant PRIV_SUPPORT            to SYS_ADMIN;
grant PRIV_TRACE_UPDATE       to SYS_ADMIN;

spool off;