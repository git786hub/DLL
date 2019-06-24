
set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\GISPKG_WMIS_CULIBRARY.log
--**************************************************************************************
--SCRIPT NAME: GISPKG_WMIS_CULIBRARY.sql
--**************************************************************************************
-- AUTHOR						: SAGARWAL
-- DATE							: 26-FEB-2018
-- PRODUCT VERSION				: 10.2.04
-- PRJ IDENTIFIER				: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC					: Script to create procedure GISPKG_WMIS_CULIBRARY in GIS_ONC schema
--**************************************************************************************
-- MODIFIED BY:
--  Rich Adase, 18-JUN-2018: Fixed PopulateValueLists query to include CATEGORY_C in join criteria
--**************************************************************************************


CREATE OR REPLACE PUBLIC SYNONYM CULIB_UNIT FOR GIS_ONC.CULIB_UNIT;
GRANT SELECT ON CULIB_UNIT TO GIS_ONC;

CREATE OR REPLACE PUBLIC SYNONYM CUSELECT_USERPREF FOR GIS_ONC.CUSELECT_USERPREF;
GRANT SELECT, INSERT, DELETE, UPDATE ON CUSELECT_USERPREF TO GIS_ONC;

CREATE OR REPLACE PUBLIC SYNONYM CULIB_UNITATTRIBUTE FOR GIS_ONC.CULIB_UNITATTRIBUTE;
GRANT SELECT ON CULIB_UNITATTRIBUTE TO GIS_ONC;

CREATE OR REPLACE PUBLIC SYNONYM CULIB_ATTRIBUTE FOR GIS_ONC.CULIB_ATTRIBUTE;
GRANT SELECT ON CULIB_ATTRIBUTE TO GIS_ONC;

create or replace 
package GIS_ONC.GISPKG_WMIS_CULIBRARY
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to manage CU library tables.
-- History:
--     22-FEB-2018 - 00.01 - Hexagon - Initial creation
-- ---------------------------------------------------------------------------------------------------------------------------------
as
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure populates value lists in the GIS database that are associated with CU standard attributes as defined in WMIS.
    -- After the local CU library tables are refreshed from WMIS, this procedure may be called to update the data in value list tables 
    -- associated with G/Technology attributes that are mapped to CU standard attributes, using the distinct set of values across all 
    -- CUs carrying that attribute
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure PopulateValueLists;
         
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure is used to remove expired CUs from user preferred CU lists.  
    -- This stored procedure is expected to be called after local CU library tables are refreshed from WMIS. 
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure UpdatePreferredCUs; 
   
end;
/

create or replace 
package body  GIS_ONC.GISPKG_WMIS_CULIBRARY
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to support the GIS interface with WMIS.
-- History:
--     04-DEC-2017 - v0.1 - Hexagon - Initial creation
-- ---------------------------------------------------------------------------------------------------------------------------------
as

procedure PopulateValueLists
--
-- History:
--     04-DEC-2017 - v0.1 - Hexagon - Initial creation
--     18-JUN-2018 - 00.03.07 - Rich Adase (Hexagon) - Fixed query to join CULIB_ATTRIBUTE by category as well as attribute ID
-- 
as
   v_tableName varchar2(30);
   v_sql varchar2(4000);
   
 begin

  dbms_output.put_line ('PopulateValueLists started');
  v_tableName:='-1';  
  
  for cur in (select distinct p.G3E_TABLE,  p.G3E_KEYFIELD, ua.ATTR_KEY, p.G3E_VALUEFIELD, ua.ATTR_VALUE
                from GIS_ONC.CULIB_UNIT            u
                join GIS_ONC.CULIB_UNITATTRIBUTE   ua on (u.CATEGORY_C = ua.CATEGORY_C and u.CU_ID = ua.CU_ID)
                join GIS_ONC.CULIB_ATTRIBUTE       a  on (ua.CATEGORY_C = a.CATEGORY_C and ua.ATTRIBUTE_ID = a.ATTRIBUTE_ID)
                join GIS.G3E_ATTRIBUTEINFO_OPTABLE ao on a.G3E_ANO = ao.G3E_ANO
                join GIS.G3E_PICKLIST_OPTABLE      p  on ao.G3E_PNO = p.G3E_PNO
                order by p.G3E_TABLE, ua.ATTR_VALUE
             ) loop   
    dbms_output.put_line('Table name ' || cur.g3e_Table);
    
    if (cur.g3e_Table <> v_tableName) then
           v_tableName:=cur.g3e_Table;       
      execute immediate 'truncate table ' || v_tableName; --Delete only first time 
      commit;
      dbms_output.put_line('table name ' || cur.g3e_Table);
    end if;
    
    v_sql:= 'insert into ' || cur.g3e_Table || '(' || cur.G3E_KEYFIELD || ',' || cur.G3E_VALUEFIELD || ') values (:4,:5)';
    execute immediate v_sql using cur.ATTR_KEY, cur.ATTR_VALUE;
  end loop;
    
  commit;
  dbms_output.put_line ('PopulateValueLists ended');
   
  exception
    when others then
    rollback;
    raise_application_error(-20001, 'GISPKG_WMIS_WR.PopulateValueLists: ' || SQLERRM);
end PopulateValueLists;

    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    
procedure UpdatePreferredCUs
as
  v_sqlStatement                      VARCHAR2(4000);
  begin
  
  dbms_output.put_line ('UpdatePreferredCUs started');
            
  delete from CUSELECT_USERPREF where CU_CODE in (select CU_CODE from culib_unit where sysdate < EFFECTIVE_D or  sysdate > EXPIRATION_D);
  commit;
  dbms_output.put_line ('UpdatePreferredCUs ended');
    
  exception
    when others then
    raise_application_error(-20003, 'GISPKG_WMIS_WR.UpdatePreferredCUs: ' || SQLERRM);
  end UpdatePreferredCUs;
end;
/

CREATE OR REPLACE PUBLIC SYNONYM GISPKG_WMIS_CULIBRARY FOR GIS_ONC.GISPKG_WMIS_CULIBRARY;
GRANT EXECUTE ON GISPKG_WMIS_CULIBRARY TO PRIV_SUPPORT, PRIV_ADMIN;
--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;

