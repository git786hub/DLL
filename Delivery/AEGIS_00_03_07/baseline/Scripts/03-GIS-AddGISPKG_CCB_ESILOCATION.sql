
set echo on
set linesize 1000
set pagesize 300
set define off

--exec adm_support.set_start(ADM_SCRIPTLOG_SEQ.NEXTVAL, 'AddGISPKG_CCB_ESILOCATION');

spool c:\temp\AddGISPKG_CCB_ESILOCATION.log

--**************************************************************************************

--SCRIPT NAME: AddGISPKG_CCB_ESILOCATION.sql
--**************************************************************************************
-- AUTHOR               : INGRNET\RPGABRYS
-- Date                 : 07-MAY-2018
-- Jira NUMBER          : 1343
-- PRODUCT VERSION      : 10.3
-- PRJ IDENTIFIER       : G/TECHNOLOGY - ONCOR
-- PROGRAM DESC         : Update CreatePremiseCorrectionsReport.
-- SOURCE DATABASE      :
--**************************************************************************************
-- Script Body
--************************************************************************************** 

-- Remove implementation if exists so script can be reinstalled
DROP PACKAGE GIS_ONC.GISPKG_CCB_ESILOCATION;
DROP PACKAGE GIS.GISPKG_CCB_ESILOCATION;
DROP PACKAGE GIS_STG.GISPKG_CCB_ESILOCATION;
DROP PUBLIC SYNONYM GISPKG_CCB_ESILOCATION;

-- Add implementation
GRANT SELECT, UPDATE ON GIS.B$PREMISE_N TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT, UPDATE ON GIS.B$STREETLIGHT_N TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT ON GIS.B$COMMON_N TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT, UPDATE ON STLT_ACCOUNT TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT ON LTT_IDENTIFIERS TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT ON G3E_JOB TO GIS_ONC WITH GRANT OPTION;
GRANT SELECT ON SYS_GENERALPARAMETER TO GIS_ONC WITH GRANT OPTION;

CREATE OR REPLACE PUBLIC SYNONYM G3E_MANAGEMODLOG FOR GIS.G3E_MANAGEMODLOG;
GRANT EXECUTE ON G3E_MANAGEMODLOG TO GIS_ONC WITH GRANT OPTION;

create or replace 
package GIS_ONC.GISPKG_CCB_ESILOCATION
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to support the GIS interface with CC&B.
-- History:
--     25-JAN-2018, v0.1    Hexagon, Initial creation
-- ---------------------------------------------------------------------------------------------------------------------------------
as
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure parses a single field used to represent an address into multiple fields. 
    -- This stored procedure will be called to parse field activity records and premise update records received from CC&B. 
    -- This procedure will be called from database trigger CIS_ESI_LOCATIONS_BIUR_ADDRESS when a record is inserted 
    -- or the address is updated in the CIS_ESI_LOCATIONS table. 
    -- This procedure will also be called from stored procedure InsertServiceActivityRecord after a record is added 
    -- to the STG_SERVICE_ACTIVITY table.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure ParseAddress
    (
        p_Address                           in  VARCHAR2,       -- Unparsed address string
        p_House_Number                      out NUMBER,         -- House number from input address
        p_Direction_Indicator               out VARCHAR2,       -- Leading Direction Indicator from input address
        p_Street_Name                       out VARCHAR2,       -- Street Name from input address
        p_Street_Type                       out VARCHAR2,       -- Street Type from input address
        p_Direction_Indicator_Trailing      out VARCHAR2,       -- Trailing Direction Indicator from input address
        p_Dwelling_Type                     out VARCHAR2,       -- Dwelling Type from input address
        p_Unit                              out VARCHAR2        -- Unit from input address
    );
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure prepares the Street Light billing data for the extraction process to CC&B. 
    -- An Informatica job will be scheduled to call this stored procedure before the Street Light billing information 
    -- has been extracted to CC&B.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure PreProcessStreetLightBilling;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure prepares the Street Light billing data for the next extraction process to CC&B. 
    -- An Informatica job will be scheduled to call this stored procedure after the Street Light billing information 
    -- has been extracted to CC&B.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure PostProcessStreetLightBilling;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure validates that the ESI Location passed in to the procedure does not exist on another Premise record. 
    -- This stored procedure will be called from a functional interface in G/Technology when a user is entering an ESI Location 
    -- for a Premise record. 
    -- This stored procedure will also be called by database trigger CIS_ESI_LOCATIONS_BIUR_PREMISE 
    -- when a Premise record with a premise status of 'Retired' is received from CC&B.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure ValidateESILocation
    (
        p_ESI_Location                      in VARCHAR2,        -- ESI Location to check for validation
        p_G3E_FID                           in NUMBER,          -- G3E_FID to exclude from validation
        p_Existing_G3E_FID                  out NUMBER,         -- G3E_FID if one exists that has the same ESI Location
        p_Job_ID                            out VARCHAR2,       -- Job ID if one exists that contains another G3E_FID with the same ESI Location
        p_Status                            out NUMBER          -- 1 if duplicate ESI Location is not found, otherwise 0
    );
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure queries the CIS_ESI_LOCATIONS and returns the record matching the passed in ESI Location. 
    -- This stored procedure will be called from the fiESILocationUpdate functional interface 
    -- when a user enters an ESI Location in G/Technology.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure GetESILocationRecord
    (
        p_ESI_Location                      in  VARCHAR2,                    -- ESI Location to use for query
        p_Cursor                            out G3E_DBTYPES.G3E_REF_CURSOR   -- ESI Location cursor to return
    );
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure updates the Premise record in GIS with the updates received from CC&B. 
    -- This stored procedure will be called from database trigger CIS_ESI_LOCATIONS_BIUR_PREMISE 
    -- when premise records are received from CC&B and loaded into the CIS_ESI_LOCATIONS table.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure UpdatePremise
    (
        p_ESI_Location                      in  VARCHAR2,       -- ESI Location to check for update
        p_Status                            out NUMBER          -- 1 if ESI Location is found, otherwise 0
    );
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure queries the CIS_ESI_LOCATIONS_LOG table and reports on any unreported errors resulting 
    -- from an attempt to update the GIS data with premise corrections from CC&B. 
    -- This stored procedure will be called from an Informatica job.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure CreatePremiseCorrectionsReport;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This function calls InsertFieldActivityRecord and InsertServiceActivityRecord to insert records
    -- into the STG_SERVICE_ACTIVITY and SERVICE_ACTIVITY tables, respectively. 
    -- This funtion will return a status of SUCCESS or FAILURE along with the error message for a FAILURE.
    -- This procedure will be called from the GIS_CreateFieldTransaction web service.
    -- -----------------------------------------------------------------------------------------------------------------------------
    function InsertSvcActivityRecordIface
    (
      p_service_order_no	                in	varchar2,
      p_esi_location	                    in	varchar2,
      p_address	                            in	varchar2,
      p_town_nm	                            in	varchar2,
      p_service_info_code	                in	varchar2,
      p_o_or_u_code	                        in	varchar2,
      p_service_center_code	                in	varchar2,
      p_flnx_h	                            in	varchar2,
      p_flny_h	                            in	varchar2,
      p_trf_co_h	                        in	varchar2,
      p_trf_flnx_h	                        in	varchar2,
      p_trf_flny_h	                        in	varchar2,
      p_cu_id	                            in	varchar2,
      p_mgmt_activity_code	                in	varchar2,
      p_msg_t	                            in	varchar2,
      p_dwell_type_c	                    in	varchar2,
      p_unit_h	                            in	varchar2,
      p_user_id	                            in	varchar2,
      p_exist_prem_gangbase	                in	varchar2,
      p_remarks_mobile	                    in	varchar2,
      p_meter_latitude	                    in	varchar2,
      p_meter_longitude	                    in	VARCHAR2
    ) return VARCHAR2;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This function returns the Structure ID of the connected Service Line for the passed in Service Point G3E_FID. 
    -- This function will be called from the CCB_PREMISE_MAPPING_VW database view.
    -- -----------------------------------------------------------------------------------------------------------------------------
    function GetServiceStructureID
    (
       p_G3E_FID                            in NUMBER           -- G3E_FID of Service Point
    )
    return VARCHAR2;                                            -- Structure ID of connected Service Line
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This function queries the master data using the passed in table name, field name, and g3e_id to determine 
    -- if the passed in field name is populated and posted. 
    -- This function will be called from the aecEditIfNotPosted attribute edit control interface.
    -- -----------------------------------------------------------------------------------------------------------------------------
    function CheckIfPopulatedAndPosted
    (
       p_Table_Name                         in VARCHAR2,        -- Name of table to query
       p_Field_Name                         in VARCHAR2,        -- Column in table to query
       p_G3E_ID                             in NUMBER           -- G3E_ID to check in table
    )
    return NUMBER;                                              -- 0 if query returns zero records, otherwise 1.
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This function queries the CIS_ESI_LOCATIONS table to determine if the passed in Premise ESI Location exists. 
    -- This function will be called from the fiESILocationUpdate functional interface. 
    -- This function returns a value of zero or one. 
    --      A value of zero indicates that the ESI Location is invalid. 
    --      A value of one indicates that the ESI Location is valid.
    -- -----------------------------------------------------------------------------------------------------------------------------
    function ValidatePremise
    (
       p_ESI_Location                       in  VARCHAR2        -- ESI Location to check for validation
    )
    return NUMBER;                                              -- 0 if query returns zero records, otherwise 1.
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This function queries the CIS_ESI_LOCATIONS table to determine if the passed in Street Light ESI Location exists. 
    -- This function will be called from the Street Light Account Editor custom command. 
    -- This function returns a value of zero or one. 
    --      A value of zero indicates that the ESI Location is invalid. 
    --      A value of one indicates that the ESI Location is valid.
    -- -----------------------------------------------------------------------------------------------------------------------------
    function ValidateStreetLightAccount
    (
       p_ESI_Location                       in  VARCHAR2        -- ESI Location to check for validation
    )
    return NUMBER;                                              -- 0 if query returns zero records, otherwise 1.
    
end;
/

create or replace 
package body GIS_ONC.GISPKG_CCB_ESILOCATION
-- ---------------------------------------------------------------------------------------------------------------------------------
-- Description:
--     Package to support the GIS interface with CC&B.
-- History:
--     04-DEC-2017, v0.1    Hexagon, Initial creation
-- ---------------------------------------------------------------------------------------------------------------------------------
as
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure ParseAddress
    (
        p_Address                           in  VARCHAR2,       -- Unparsed address string
        p_House_Number                      out NUMBER,         -- House number from input address
        p_Direction_Indicator               out VARCHAR2,       -- Leading Direction Indicator from input address
        p_Street_Name                       out VARCHAR2,       -- Street Name from input address
        p_Street_Type                       out VARCHAR2,       -- Street Type from input address
        p_Direction_Indicator_Trailing      out VARCHAR2,       -- Trailing Direction Indicator from input address
        p_Dwelling_Type                     out VARCHAR2,       -- Dwelling Type from input address
        p_Unit                              out VARCHAR2        -- Unit from input address
    )
    As
       type v_directions_array is TABLE of CIS_ESI_LOCATIONS.DIRECTION_INDICATOR%TYPE;
       V_Directions V_Directions_Array;
       
       type v_street_types_array is TABLE of CIS_ESI_LOCATIONS.STREET_TYPE%TYPE;
       v_StreetTypes v_street_types_array;
       
       type v_dwelling_type_array is TABLE of CIS_ESI_LOCATIONS.DWELLING_TYPE%TYPE;
       v_DwellingTypes v_dwelling_type_array;
       
       v_Address            CIS_ESI_LOCATIONS.ADDRESS%TYPE;
       v_TempString         CIS_ESI_LOCATIONS.ADDRESS%TYPE;
       v_RemainingString    CIS_ESI_LOCATIONS.ADDRESS%TYPE;
       v_NumberCheck        NUMBER(1);
       
    begin
    
        dbms_output.put_line ('ParseAddress started');
        
        if (length(p_Address) > 0) then
        
            v_Directions := v_Directions_Array('N','E','S','W','NE','SE','NW','SW');
            V_Streettypes := V_Street_Types_Array('ALLEY', 'ALY', 'ANX', 'AVE', 'BAY', 'BEND', 'BLUFF', 'BLVD', 'BREAK', 'BRG', 'BUS', 'BYP', 
                                                  'CIR', 'CONN', 'COR', 'COURT', 'COVE', 'COVES', 'CP', 'CR', 'CRES', 'CRK', 'CT', 'CTR', 'CV', 
                                                  'DR', 'DRIVE', 'DV', 'ENT', 'ESMT', 'EST', 'EXPY', 'EXT', 'FLS', 'FRWY', 'FRY', 'FWY', 'GATE', 
                                                  'GLADE', 'GRN', 'GRV', 'GTWY', 'HTS', 'HWY', 'IH', 'INLET', 'JCT', 'KNOLL', 'LANE', 'LK', 'LN', 
                                                  'LNDG', 'LOOP', 'PASS', 'PATH', 'PIKE', 'PK', 'PKWY', 'PKY', 'PL', 'PLACE', 'PLAZA', 'PLZ', 
                                                  'PRKWY', 'RD', 'RDG', 'RIDGE', 'RIV', 'RNCH', 'ROAD', 'ROW', 'ROWE', 'RUN', 'SH', 'SHWY', 
                                                  'SPUR', 'ST', 'TER', 'TERR', 'TLWY', 'TOLWY', 'TOWAY', 'TPK', 'TR', 'TRACE', 'TRAIL', 'TRAK', 
                                                  'TRCE', 'TRL', 'TRLR', 'TRLS', 'UHWY', 'WALK', 'WAY', 'WY', 'XING');
            v_DwellingTypes := v_dwelling_type_array('APT', 'BARN', 'BLDG', 'CAM', 'CATV', 'CBLBX', 'CELL', 'CLBH', 'CLTWR', 'CLUB', 'CONC', 
                                                     'DUPLX', 'FRNT', 'GAR', 'GARG', 'GATE', 'GRDL', 'GRDLT', 'GRGE', 'HLTS', 'HNGR', 'HOUSE', 
                                                     'HP', 'HSE', 'HSELT', 'HSMT', 'HWLT', 'LDLTG', 'LIGHT', 'LNDRY', 'LOT', 'LOWER', 'MAIN', 
                                                     'MHP', 'ODLT', 'OFC', 'PARK', 'PERM', 'PKNG', 'POOL', 'PUMP', 'REAR', 'RES', 'RV', 'SGLT', 
                                                     'SGNL', 'SHOP', 'SIDE', 'SIGN', 'SIGN', 'SIRN', 'SLS', 'SP', 'SPACE', 'SPC', 'SPKLR', 
                                                     'SPRK', 'STE', 'STLG', 'STOR', 'SUITE', 'TEL', 'TEMP', 'TRLR', 'TRLT', 'UNIT', 'UPSTR', 
                                                     'WELL', 'WELL', 'WHSE');
            
            
            -- Set the input address string to uppercase and trim the leading and trailing spaces.
            v_Address := upper(trim(p_Address));
            
            -- Trim any internal spaces to ensure that there are no places where two or more spaces are adjacent.
            v_Address := regexp_replace(v_Address, '[[:space:]]+', ' ');
            
            -- Check the first delimited field from the left.
            -- If it is a numeric integer then this is the address number. 
            --      Set p_House_Number to this substring. 
            --      Truncate substring from the address string.
            v_TempString := regexp_substr(v_Address, '[^[:space:]]+', 1, 1);
            
            dbms_output.put_line ('House Number check: '||v_TempString);
            
            Select Case
              When Regexp_Like (V_Tempstring, '^[[:digit:]]+$') Then
                1
              else
                0
            end case into v_NumberCheck from dual;
            
            if (v_NumberCheck = 1) then
                p_House_Number := v_TempString;
                v_RemainingString := substr(v_Address, INSTR(v_Address, ' ') + 1);
            else
                v_RemainingString := v_Address;
                p_House_Number := null;
            end if;
            
            dbms_output.put_line ('House Number: '||p_House_Number);
            dbms_output.put_line ('Address after House Number: '||v_RemainingString);
            
            -- Check the first delimited field from the left.
            -- If it is one of the direction values listed in the directions value list then use this as the leading direction indicator. 
            -- Set p_Direction_Indicator to this substring. Truncate substring from the address string.
            v_TempString := regexp_substr(v_RemainingString, '[^[:space:]]+', 1, 1);
            
            dbms_output.put_line ('Leading Direction check: '||v_TempString);
            
            if (v_TempString member of v_Directions) then
                p_Direction_Indicator := v_TempString;
                v_RemainingString := substr(v_RemainingString, INSTR(v_RemainingString, ' ') + 1);
            else
                p_Direction_Indicator := null;
            end if;
            
            dbms_output.put_line ('Leading Direction: '||p_Direction_Indicator);
            dbms_output.put_line ('Address after Leading Direction: '||v_RemainingString);
            
            -- Check the remaining address string delimited fields looking for the dwelling type.  
            -- To do this look for any occurrence of the character strings found in the dwelling type list where it is preceded and followed by a space.
            -- If the dwelling type is found, then set p_Dwelling_Type to this substring and check for another delimited field that comes after, 
            -- to the right of the found dwelling type in the address string.  If a substring is found, then it will be the unit number. 
            -- Set p_Unit to this substring. If not found then unit number is blank.  Truncate found substrings from the address string.
            v_TempString := substr(v_RemainingString, INSTR(v_RemainingString, ' ', -1, 2));
            v_TempString := regexp_substr(v_TempString, '[^[:space:]]+', 1, 1);
            
            dbms_output.put_line ('Dwelling Type check: '||v_TempString);
            
            if (v_TempString member of v_DwellingTypes) then
                p_Dwelling_Type := v_TempString;
                p_Unit := substr(v_RemainingString, INSTR(v_RemainingString, ' ', -1, 1) + 1);
                v_RemainingString := substr(v_RemainingString, 1, INSTR(v_RemainingString, ' ', -1, 2) - 1);
            else
                p_Dwelling_Type := null;
                p_Unit := null;
            end if;
            
            dbms_output.put_line ('Dwelling Type: '||p_Dwelling_Type);
            dbms_output.put_line ('Unit: '||p_Unit);
            dbms_output.put_line ('Address after Dwelling Type: '||v_RemainingString);
            
            -- Check the remaining address string delimited fields looking for the post direction type. 
            -- If it is one of the direction values then use this as the trailing direction indicator.  
            -- Set p_Direction_Indicator_Trailing to this substring. Truncate substring from the address string.
            v_TempString := substr(v_RemainingString, INSTR(v_RemainingString, ' ', -1, 1) + 1);
            
            dbms_output.put_line ('Trailing Direction check: '||v_TempString);
            
            if (v_TempString member of v_Directions) then
                p_Direction_Indicator_Trailing := v_TempString;
                v_RemainingString := substr(v_RemainingString, 1, INSTR(v_RemainingString, ' ', -1, 1) - 1);
            else
                p_Direction_Indicator_Trailing := null;
            end if;
            
            dbms_output.put_line ('Trailing Direction: '||p_Direction_Indicator_Trailing);
            dbms_output.put_line ('Address after Trailing Direction: '||v_RemainingString);
            
            -- Check the remaining address string delimited fields looking for street type from the street type list.  
            -- To do this look for any occurrence of the character strings found in the street type list where it is preceded and followed by a space. 
            -- If the street type is found, then set p_Street_Type to this substring. Truncate substring from the address string.
            v_TempString := substr(v_RemainingString, INSTR(v_RemainingString, ' ', -1, 1) + 1);
            
            dbms_output.put_line ('Street Type check: '||v_TempString);
            
            if (v_TempString member of v_StreetTypes) then
                p_Street_Type := v_TempString;
                v_RemainingString := substr(v_RemainingString, 1, INSTR(v_RemainingString, ' ', -1, 1) - 1);
            else
                p_Street_Type := null;
            end if;
            
            dbms_output.put_line ('Street Type: '||p_Street_Type);
            dbms_output.put_line ('Address after Street Type: '||v_RemainingString);
            
            -- At this point the remaining address string will be the street name. Set p_Street_Name to the remaining address string.
            p_Street_Name := trim(v_RemainingString);
            
            dbms_output.put_line ('Street Name: '||p_Street_Name);
        
        end if;
        dbms_output.put_line ('ParseAddress ended');
    
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.ParseAddress: ' || SQLERRM);    
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure PreProcessStreetLightBilling
    as
        cursor curThresholdErrors is
            select 
                st.ESI_LOCATION,
                st.DESCRIPTION_ID,
                st.PREVIOUS_COUNT,
                st.CURRENT_COUNT,
                st.THRESHOLD_PERCENT,
                (select case when CURRENT_COUNT = 0 or PREVIOUS_COUNT = 0 then 100 
                  else trunc(abs(((CURRENT_COUNT - PREVIOUS_COUNT) / CURRENT_COUNT * 100))) end case 
                  from STLT_ACCOUNT where ESI_LOCATION = st.ESI_LOCATION) PERCENT_OF_CHANGE
            from STLT_ACCOUNT st
            Where 
                st.THRESHOLD_STATE = 'ERROR'
                and st.THRESHOLD_OVERRIDE = 'N';
                
        v_toAddress       varchar2(600);
        v_fromAddress     varchar2(200);
        v_recordCount     number(10);
        v_emailMessage    varchar2(28000);
        v_tempString      varchar2(100);
    begin
        
        dbms_output.put_line ('PreProcessStreetLightBilling started');
        
        -- Update the Connection Status for each Street Light associated with a billable account 
        -- where the Reconnect Date for the Street Light is less than or equal to the current date 
        -- and the Connection Status <>'Y'.
        update GIS.B$STREETLIGHT_N
        set CONNECTION_STATUS_C = 'CON'
        where 
            CONNECTION_STATUS_C <> 'CON'
            and nvl(TRUNC(CONNECT_D), TRUNC(sysdate) + 1) <= TRUNC(sysdate)
            and ACCOUNT_ID in (select ESI_LOCATION from STLT_ACCOUNT where BILLABLE = 'Y')
            and ltt_id = 0;
            
        dbms_output.put_line ('Connection Status records updated: ' ||SQL%ROWCOUNT);
            
        -- For each billable record in the STLT_ACCOUNT table, set the previous count of Street Lights 
        -- to the count that was previously sent to CC&B.
        update STLT_ACCOUNT
        set PREVIOUS_COUNT = CURRENT_COUNT
        where BILLABLE = 'Y';
        
        dbms_output.put_line ('Billable records updated: ' ||SQL%ROWCOUNT);
        
        -- Query the billable Street Lights and aggregate by ESI Location. 
        -- Set the current count of Street Lights for the ESI Location record in the STLT_ACCOUNT table to the aggregate count.
        update STLT_ACCOUNT sl
        set sl.CURRENT_COUNT = (select count(s.ACCOUNT_ID)
                                from GIS.B$STREETLIGHT_N s, GIS.B$COMMON_N c
                                where s.CONNECTION_STATUS_C = 'CON'
                                and sl.ESI_LOCATION = s.ACCOUNT_ID
                                and c.feature_state_c in ('INI', 'CLS', 'PPR', 'ABR', 'PPA', 'ABA')
                                and s.g3e_fid = c.g3e_fid
                                and s.ltt_id = 0
                                and c.ltt_id = 0)
        where exists (select 1 
                         from GIS.B$STREETLIGHT_N s
                         where s.ACCOUNT_ID = sl.ESI_LOCATION
                         and sl.BILLABLE = 'Y');
        
        -- Reset threshold status
        update STLT_ACCOUNT
        set THRESHOLD_STATE = 'SUCCESS'
        where THRESHOLD_STATE = 'ERROR';
        
        -- Calculate the percent of change between the current and previous counts of billable Street Lights. 
        -- Set the threshold status based on the percent of change versus the allowable percent of change.
        update STLT_ACCOUNT
        set THRESHOLD_STATE = 'ERROR'
        where 
            (CURRENT_COUNT = 0 or PREVIOUS_COUNT = 0) 
            and CURRENT_COUNT <> PREVIOUS_COUNT
            and BILLABLE = 'Y';
            
        dbms_output.put_line ('Zero count error records updated: ' ||SQL%ROWCOUNT);
        
        update STLT_ACCOUNT
        set THRESHOLD_STATE = 'ERROR'
        where
            CURRENT_COUNT <> 0
            and PREVIOUS_COUNT <> 0
            and CURRENT_COUNT <> PREVIOUS_COUNT
            and BILLABLE = 'Y'
            and ABS(((CURRENT_COUNT - PREVIOUS_COUNT) / CURRENT_COUNT * 100)) > THRESHOLD_PERCENT;
        
        dbms_output.put_line ('Threshold error records updated: ' ||SQL%ROWCOUNT);
        
        update STLT_ACCOUNT
        set THRESHOLD_STATE = 'SUCCESS'
        where 
            THRESHOLD_STATE <> 'ERROR'
            and BILLABLE = 'Y';
            
        commit;
        
        -- Process records in modificationlog table. 
        -- This is needed to show correct symbology in G/Tech if connection status has been changed by this procedure.
        G3E_ManageModLog.GTUpdateDeltasAll(1);
            
        -- Send an email if there are any records in the STLT_ACCOUNT table where the change in counts 
        -- for an ESI Location has exceeded the threshold. 
        -- The PERCENT_OF_CHANGE will be calculated using the CURRENT_COUNT and PREVIOUS_COUNT values.      
        select count(*) into v_recordCount from STLT_ACCOUNT 
        Where threshold_state = 'ERROR' and threshold_override = 'N';
        
        if (v_recordCount > 0) then
            -- Get the email metadata parameters
            select param_value into v_toAddress from SYS_GENERALPARAMETER 
            where subsystem_name = 'StreetlightBilling' and subsystem_component = 'ErrorLoggingMail' and param_name = 'ToEmailAddress';
            
            select param_value into v_fromAddress from SYS_GENERALPARAMETER 
            where subsystem_name = 'StreetlightBilling' and subsystem_component = 'ErrorLoggingMail' and param_name = 'FromEmailAddress';
            
            -- Build email body
            v_emailMessage := 'ESI Location  Description ID  Previous Count  Current Count  Threshold%  Change%';
            
            for i in curThresholdErrors
            loop
                v_tempString := chr(13) || lpad(i.esi_location, 12) || lpad(i.description_id, 16) || lpad(i.previous_count, 16)
                              || lpad(i.current_count, 15) || lpad(i.threshold_percent, 12) || lpad(i.percent_of_change, 9);
                v_emailMessage := v_emailMessage || v_tempString;
            end loop;
            
            dbms_output.put_line ('Email message: '||v_emailMessage);
            
            -- Set email package variables
            Send_EF_Email_PKG.emToAddress := v_toAddress;
            Send_EF_Email_PKG.emFromAddress := v_fromAddress;
            Send_EF_Email_PKG.emSubject := 'Street Light count threshold exceeded';
            Send_EF_Email_PKG.emMessage := v_emailMessage;
            Send_EF_Email_PKG.SendEmail;
        end if;

        dbms_output.put_line ('PreProcessStreetLightBilling ended');

    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.PreProcessStreetLightBilling: ' || SQLERRM);
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure PostProcessStreetLightBilling
    as
    begin
        
        dbms_output.put_line ('PostProcessStreetLightBilling started');
        
        -- Set the run date field to the current date for each record that has been sent to CC&B.
        update STLT_ACCOUNT
        set RUN_DATE = sysdate
        where 
            BILLABLE = 'Y' 
            and (THRESHOLD_STATE = 'SUCCESS' or THRESHOLD_OVERRIDE = 'Y');
            
        dbms_output.put_line ('RUN_DATE records updated: ' ||SQL%ROWCOUNT);
        
        -- Reset the threshold override field for each record in the STLT_ACCOUNT table.    
        update STLT_ACCOUNT
        set THRESHOLD_OVERRIDE = 'N'
        where THRESHOLD_OVERRIDE = 'Y';
        
        dbms_output.put_line ('THRESHOLD_OVERRIDE records updated: ' ||SQL%ROWCOUNT);
        
        commit;
        
        dbms_output.put_line ('PostProcessStreetLightBilling ended');

    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.PostProcessStreetLightBilling: ' || SQLERRM);
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure ValidateESILocation
    (
        p_ESI_Location                      in VARCHAR2,        -- ESI Location to check for validation
        p_G3E_FID                           in NUMBER,          -- G3E_FID to exclude from validation
        p_Existing_G3E_FID                  out NUMBER,         -- G3E_FID if one exists that has the same ESI Location
        p_Job_ID                            out VARCHAR2,       -- Job ID if one exists that contains another G3E_FID with the same ESI Location
        p_Status                            out NUMBER          -- 1 if duplicate ESI Location is not found, otherwise 0
    )
    as
        v_sqlStatement                      VARCHAR2(4000);
    begin
    
        dbms_output.put_line ('ValidateESILocation started');
        
        -- Check if the ESI Location exists on a Premise record by querying the GIS.B$PREMISE_N 
        -- using the input ESI Location (p_ESI_Location) and excluding the records for the input G3E_FID (p_G3E_FID).
        v_sqlStatement := 'select case when count(*) = 0 then 1 else 0 end case 
                         from GIS.B$PREMISE_N 
                         where premise_nbr = :b1 and g3e_fid <> :b2';
        execute immediate v_sqlStatement into p_Status using p_ESI_Location, p_G3E_FID;
        
        if (p_Status = 0) then
            v_sqlStatement := 'select distinct p.g3e_fid, j.g3e_identifier
                             from GIS.B$PREMISE_N p, GIS.LTT_IDENTIFIERS l, GIS.G3E_JOB j
                             where p.premise_nbr = :b1
                                and p.g3e_fid <> :b2
                                and p.ltt_Id = l.ltt_id(+)
                                and l.ltt_name = j.g3e_identifier(+)
                                and rownum = 1';
            execute immediate v_sqlStatement into p_Existing_G3E_FID, p_Job_ID using p_ESI_Location, p_G3E_FID;
        end if;
        
        dbms_output.put_line ('ValidateESILocation ended');
    
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.ValidateESILocation: ' || SQLERRM);
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure GetESILocationRecord
    (
        p_ESI_Location                      in  VARCHAR2,                       -- ESI Location to use for query
        p_Cursor                            out G3E_DBTYPES.G3E_REF_CURSOR      -- ESI Location cursor to return
    )
    as
    begin
        open p_Cursor for
            select 
                ESI_LOCATION,
                ADDRESS,
                HOUSE_NUMBER,
                FRACTIONAL_HOUSE_NUMBER,
                DIRECTION_INDICATOR,
                STREET_NAME,
                STREET_TYPE,
                DIRECTIONAL_IND_TRAILING,
                UNIT,
                ZIP_CODE,
                CITY,
                PREMISE_TYPE,
                RATE_CODE,
                SIC_CODE,
                COUNTY_CODE,
                INSIDE_CITY_LIMITS,
                CRITICAL_LOAD,
                CRITICAL_CUSTOMER,
                PREMISE_STATUS,
                MAJOR_CUSTOMER,
                DWELLING_TYPE
            from CIS_ESI_LOCATIONS
            Where 
                Esi_Location = p_ESI_Location
                And Service_Point_Type <> 'Street Light';
    
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.GetESILocationRecord: ' || SQLERRM);
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure UpdatePremise
    (
        p_ESI_Location                      in  VARCHAR2,       -- ESI Location to check for update
        p_Status                            out NUMBER          -- 1 if ESI Location is found, otherwise 0
    )
    as
        v_sqlStatement                      VARCHAR2(4000);
        v_configuration                     VARCHAR2(30);
    begin
    
        dbms_output.put_line ('UpdatePremise started');
        
        -- Check if the ESI Location exists on a Premise record by querying the GIS.B$PREMISE_N using the input ESI Location (p_ESI_Location).
        v_sqlStatement := 'select case when count(*) = 0 then 0 else 1 end case 
                         from GIS.B$PREMISE_N 
                         where premise_nbr = :b1';
        execute immediate v_sqlStatement into p_Status using p_ESI_Location;
        
        if (p_Status = 1) then
            
            -- Before updating data tables need to set the configuration or the product triggers will throw an error.
            v_sqlStatement := 'select g3e_value 
                         from G3E_GENERALPARAMETER_OPTABLE 
                         where g3e_name = ''AllConfigurations''';
            execute immediate v_sqlStatement into v_configuration;
            
            LTT_USER.SETCONFIGURATION(v_configuration);
            
            dbms_output.put_line ('Configuration: ' ||v_configuration);

            v_sqlStatement := 'UPDATE GIS.B$PREMISE_N p
                             SET (
                                    p.ADDRESS,
                                    p.HOUSE_NBR,
                                    p.HOUSE_FRACTION_NBR,
                                    p.DIR_LEADING_C,
                                    p.STREET_NM,
                                    p.STREET_TYPE_C,
                                    p.DIR_TRAILING_C,
                                    p.UNIT_NBR,
                                    p.ZIP_C,
                                    p.CITY_C,
                                    p.TYPE_C,
                                    p.RATE_CODE_C,
                                    p.SIC_C,
                                    p.COUNTY_C,
                                    p.INSIDE_CITY_LIMITS_YN,
                                    p.CRITICAL_LOAD_Q,
                                    p.CRITICAL_CUSTOMER_C,
                                    p.PREMISE_STATUS,
                                    p.MAJOR_CUSTOMER_C,
                                    p.DWELLING_TYPE_C
                                )  =  ((select
                                        c.ADDRESS,
                                        c.HOUSE_NUMBER,
                                        c.FRACTIONAL_HOUSE_NUMBER,
                                        c.DIRECTION_INDICATOR,
                                        c.STREET_NAME,
                                        c.STREET_TYPE,
                                        c.DIRECTIONAL_IND_TRAILING,
                                        c.UNIT,
                                        c.ZIP_CODE,
                                        c.CITY,
                                        c.PREMISE_TYPE,
                                        c.RATE_CODE,
                                        c.SIC_CODE,
                                        c.COUNTY_CODE,
                                        c.INSIDE_CITY_LIMITS,
                                        c.CRITICAL_LOAD,
                                        c.CRITICAL_CUSTOMER,
                                        c.PREMISE_STATUS,
                                        c.MAJOR_CUSTOMER,
                                        c.DWELLING_TYPE
                                             from CIS_ESI_LOCATIONS c
                                          where c.ESI_LOCATION = :b1))
                            where p.PREMISE_NBR = :b2';
            execute immediate v_sqlStatement using p_ESI_Location, p_ESI_Location;
        end if;
        
        dbms_output.put_line ('UpdatePremise ended');
    
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.UpdatePremise: ' || SQLERRM);
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public procedure, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure CreatePremiseCorrectionsReport
    as        
        v_sqlStatement        VARCHAR2(4000);      
        v_errorsExist         NUMBER(1);
        v_minErrorRecord      CIS_ESI_LOCATIONS_LOG.CIS_ESI_LOCATIONS_LOG_ID%TYPE;
        v_maxErrorRecord      CIS_ESI_LOCATIONS_LOG.CIS_ESI_LOCATIONS_LOG_ID%TYPE;
        v_toAddress           VARCHAR2(600);
        v_fromAddress         VARCHAR2(200);
        v_recordCount         NUMBER;
        v_emailMessage        CLOB;
        v_tempString          CLOB;
        
        cursor curRetiredErrors is
                select 
                    DISTINCT ESI_LOCATION, ERROR_MESSAGE
                from CIS_ESI_LOCATIONS_LOG
                Where 
                    cis_esi_locations_log_id between v_minErrorRecord and v_maxErrorRecord
                    and status = 'ERROR' and reported = 0
                    and upper(error_message) like 'RETIRED ESI LOCATION FOUND IN GIS%';
                    
        cursor curAddressParsingErrors is
                select 
                    DISTINCT ESI_LOCATION, ERROR_MESSAGE
                from CIS_ESI_LOCATIONS_LOG
                Where 
                    cis_esi_locations_log_id between v_minErrorRecord and v_maxErrorRecord
                    and status = 'ERROR' and reported = 0
                    and upper(error_message) like 'ERROR PARSING ADDRESS%';
                    
    begin    
        dbms_output.put_line ('CreatePremiseCorrectionsReport started');
        -- Query the CIS_ESI_LOCATIONS_LOG table to see if there are any unreported errors.
        v_sqlStatement := 'select case when count(*) = 0 then 0 else 1 end case 
                         from CIS_ESI_LOCATIONS_LOG 
                         where status = ''ERROR'' and reported = 0';
        execute immediate v_sqlStatement into v_errorsExist;
        
        dbms_output.put_line ('v_errorsExist: '||v_errorsExist);
        
        -- If the query returns any records then gather the errors and send an email.
        if (v_errorsExist = 1) then
            -- Get the unique record identifier for the errors. Will use these values to update the
            -- table after the errors have been successfully reported.
            v_sqlStatement := 'select min(CIS_ESI_LOCATIONS_LOG_ID), max(CIS_ESI_LOCATIONS_LOG_ID) 
                             from CIS_ESI_LOCATIONS_LOG 
                             where status = ''ERROR'' and reported = 0';
            execute immediate v_sqlStatement into v_minErrorRecord, v_maxErrorRecord;
            
            dbms_output.put_line ('CIS_ESI_LOCATIONS_LOG_ID range: '||v_minErrorRecord||' - '||v_maxErrorRecord);
            
            -- If there are any records from the query where the error is "ESI LOCATION NOT FOUND IN GIS", 
            -- then add a line to the email body with the format 
            -- "<count of records with this error> record(s) where the ESI Location does not exist in the GIS".
            v_sqlStatement := 'select count(*) 
                             from CIS_ESI_LOCATIONS_LOG 
                             where cis_esi_locations_log_id between :b1 and :b2 
                             and status = ''ERROR'' and reported = 0
                             and upper(error_message) like ''ESI LOCATION NOT FOUND IN GIS%''';
            execute immediate v_sqlStatement into v_recordCount using v_minErrorRecord, v_maxErrorRecord;
            
            if (v_recordCount > 0) then
                v_emailMessage := v_recordCount || ' record(s) where the ESI Location does not exist in the GIS.' ||chr(13);
            end if;
            
            -- If there are any records from the query where the error is "RETIRED ESI LOCATION FOUND IN GIS", 
            -- then add a line to the email body for each record with the format 
            -- "Retired ESI Location found in GIS - <ESI_LOCATION value>".
            for i in curRetiredErrors
            loop
                v_tempString := v_tempString || 'Retired ESI Location found in GIS - ' ||i.esi_location||chr(13);
            end loop;
            
            v_emailMessage := v_emailMessage || v_tempString;
            v_tempString := null;
            
            -- If there are any records from the query where the error is "ERROR PARSING ADDRESS", 
            -- then add a line to the email body for each record with the format 
            -- "Error parsing address for ESI Location - <ESI_LOCATION value>".
            for i in curAddressParsingErrors
            loop
                v_tempString := v_tempString || 'Error parsing address for ESI Location - ' ||i.esi_location||chr(13);
            end loop;
            
            v_emailMessage := v_emailMessage || v_tempString;
            dbms_output.put_line ('Email message: '||v_emailMessage);
            
            -- Get the email metadata parameters
            select param_value into v_toAddress from SYS_GENERALPARAMETER 
            where subsystem_name = 'PremiseCorrections' and subsystem_component = 'ErrorLoggingMail' and param_name = 'ToEmailAddress';
            
            select param_value into v_fromAddress from SYS_GENERALPARAMETER 
            where subsystem_name = 'PremiseCorrections' and subsystem_component = 'ErrorLoggingMail' and param_name = 'FromEmailAddress';
            
            -- Set email package variables
            Send_EF_Email_PKG.emToAddress := v_toAddress;
            Send_EF_Email_PKG.emFromAddress := v_fromAddress;
            Send_EF_Email_PKG.emSubject := 'Premise Correction Errors';
            Send_EF_Email_PKG.emMessage := v_emailMessage;
            Send_EF_Email_PKG.SendEmail;
            
            -- Update the CIS_ESI_LOCATIONS_LOG table setting the REPORTED field to 1 for those records that were included in the email.
            v_sqlStatement := 'update CIS_ESI_LOCATIONS_LOG set reported = 1 
                             where status = ''ERROR'' and cis_esi_locations_log_id between :b1 and :b2';
            execute immediate v_sqlStatement using v_minErrorRecord, v_maxErrorRecord;
            
            commit;
            
        end if;
        
        dbms_output.put_line ('CreatePremiseCorrectionsReport ended');
    
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.CreatePremiseCorrectionsReport: ' || SQLERRM);
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure validates and inserts a record into the SERVICE_ACTIVITY table. 
    -- This procedure will be called after a record is inserted into the STG_SERVICE_ACTIVITY table.
    -- The STG_SERVICE_ACTIVITY.STG_SERVICE_ACTIVITY_ID for the newly inserted record will be passed in to this procedure.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure InsertServiceActivityRecord
    (
        p_stg_service_activity_id           in  NUMBER,      -- STG_SERVICE_ACTIVITY_ID of the record to validate
        p_status                            out boolean,     -- Indicates success or failure
        p_ErrorMessage                      out VARCHAR2,    -- Error message if failure
        p_ErrorCode                         out VARCHAR2     -- Error code if failure
    )
    as
        v_sqlStatement                      VARCHAR2(4000);
        v_Address                           SERVICE_ACTIVITY.ADDRESS%TYPE;
        v_HouseNumber                       SERVICE_ACTIVITY.HOUSE_NO%TYPE;
        v_LeadingDirection                  SERVICE_ACTIVITY.ADDR_LEAD_DIR_IND%TYPE;
        v_StreetName                        SERVICE_ACTIVITY.STREET_NAME%TYPE;
        v_StreetType                        SERVICE_ACTIVITY.STREET_TYPE%TYPE;
        v_TrailingDirection                 SERVICE_ACTIVITY.ADDR_TRAIL_DIR_IND%TYPE;
        v_DwellingType                      SERVICE_ACTIVITY.DWELL_TYPE_C%TYPE;
        v_Unit                              SERVICE_ACTIVITY.UNIT_H%TYPE;
        v_ServiceOrderNo                    SERVICE_ACTIVITY.SERVICE_ORDER_NO%TYPE;
        v_EsiLocation                       SERVICE_ACTIVITY.ESI_LOCATION%TYPE;
        v_Town                              SERVICE_ACTIVITY.TOWN_NM%TYPE;
        v_ServiceInfoCode                   SERVICE_ACTIVITY.SERVICE_INFO_CODE%TYPE;
        v_OorU                              SERVICE_ACTIVITY.O_OR_U_CODE%TYPE;
        v_ServiceCenterCode                 SERVICE_ACTIVITY.SERVICE_CENTER_CODE%TYPE;
        v_StructureID                       SERVICE_ACTIVITY.STRUCTURE_ID%TYPE;
        v_Flnx                              SERVICE_ACTIVITY.FLNX_H%TYPE;
        v_Flny                              SERVICE_ACTIVITY.FLNY_H%TYPE;
        v_TrfCo                             SERVICE_ACTIVITY.TRF_CO_H%TYPE;
        v_TrfStructureID                    SERVICE_ACTIVITY.TRF_STRUCTURE_ID%TYPE;
        v_TrfFlnx                           SERVICE_ACTIVITY.TRF_FLNX_H%TYPE;
        v_TrfFlny                           SERVICE_ACTIVITY.TRF_FLNY_H%TYPE;
        v_Cu                                SERVICE_ACTIVITY.CU_ID%TYPE;
        v_MgmtActivityCode                  SERVICE_ACTIVITY.MGMT_ACTIVITY_CODE%TYPE;
        v_Status                            SERVICE_ACTIVITY.STATUS_C%TYPE;
        v_Msg                               SERVICE_ACTIVITY.MSG_T%TYPE;
        v_UserId                            SERVICE_ACTIVITY.USER_ID%TYPE;
        v_ExistPremGangbase                 SERVICE_ACTIVITY.EXIST_PREM_GANGBASE%TYPE;
        v_RemarksMobile                     SERVICE_ACTIVITY.REMARKS_MOBILE%TYPE;
        v_MeterLatitude                     SERVICE_ACTIVITY.METER_LATITUDE%TYPE;
        v_MeterLongitude                    SERVICE_ACTIVITY.METER_LONGITUDE%TYPE;
        
    pragma autonomous_transaction;
    begin
    
        dbms_output.put_line ('InsertServiceActivityRecord started');
        p_status := true;
        
        -- Get the record from the STG_SERVICE_ACTIVITY using the passed in STG_SERVICE_ACTIVITY_ID.
        v_sqlStatement := 'select service_order_no,esi_location,address,town_nm,service_info_code,o_or_u_code,service_center_code,
                                flnx_h,flny_h,trf_co_h,trf_flnx_h,trf_flny_h,cu_id,mgmt_activity_code,status_c,msg_t,dwell_type_c,
                                unit_h,user_id,exist_prem_gangbase,remarks_mobile,meter_latitude,meter_longitude
                         from STG_SERVICE_ACTIVITY 
                         where stg_service_activity_id = :b1';
        
        dbms_output.put_line ('Querying for record from STG_SERVICE_ACTIVITY where STG_SERVICE_ACTIVITY_ID = '||p_stg_service_activity_id);
        
        execute immediate v_sqlStatement into v_ServiceOrderNo,v_EsiLocation,v_Address,v_Town,v_ServiceInfoCode,v_OorU,v_ServiceCenterCode,
                                            v_Flnx,v_Flny,v_TrfCo,v_TrfFlnx,v_TrfFlny,v_Cu,v_MgmtActivityCode,v_Status,v_Msg,v_DwellingType,
                                            v_Unit,v_UserId,v_ExistPremGangbase,v_RemarksMobile,v_MeterLatitude,v_MeterLongitude 
                                         using p_stg_service_activity_id;
        
        begin
            -- Call the ParseAddress stored procedure to parse the ADDRESS.
            GISPKG_CCB_ESILOCATION.ParseAddress(v_Address, v_HouseNumber, v_LeadingDirection, v_StreetName, v_StreetType, 
                                                v_TrailingDirection, v_DwellingType, v_Unit);                                                
        exception
            when others then
                p_ErrorMessage :=  'Error Parsing Address - '||SQLERRM;
                p_ErrorCode := 'GISCFT02';
                p_status := false;
                rollback;
                return;
        end;
        
        -- Validate record
        if (v_EsiLocation = '0' or v_EsiLocation is null) then
            p_ErrorMessage := 'No ESI Location';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_HouseNumber = 0 or v_HouseNumber is null) then
            p_ErrorMessage := 'No House Number';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_StreetName is null) then
            p_ErrorMessage := 'No Street Name';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_Flnx = '0' or v_Flnx is null) then
            p_ErrorMessage := 'No flnx';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_Flny = '0' or v_Flny is null) then
            p_ErrorMessage := 'No flny';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_Cu = '0' or v_Cu is null) then
            p_ErrorMessage := 'No Cu id';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if ((v_ServiceInfoCode = 'GB' or v_ServiceInfoCode = 'MO') and (v_ExistPremGangbase = '0' or v_ExistPremGangbase is null)) then
            p_ErrorMessage := 'No Existing Premise';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_ServiceOrderNo = '0' or v_ServiceOrderNo is null) then
            p_ErrorMessage := 'Service Order# Null';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_MgmtActivityCode = '0' or v_MgmtActivityCode is null) then
            p_ErrorMessage := 'SI Activity  Null';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_OorU = '0' or v_OorU is null) then
            p_ErrorMessage := 'OH/UG  Null';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_TrfFlnx = '0' or v_TrfFlnx is null) then
            p_ErrorMessage := 'Service FLN X =Null';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_TrfFlny = '0' or v_TrfFlny is null) then
            p_ErrorMessage := 'Service FLN Y =Null';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        if (v_RemarksMobile is null) then
            p_ErrorMessage := 'Remarks: =Null';
            p_ErrorCode := 'GISCFT03';
            p_status := false;
            rollback;
            return;
        end if;
        
        -- Set structure id values
        v_StructureID := replace(v_Flnx, '-', '')||'-'||replace(v_Flny, '-', '');
        v_TrfStructureID := replace(v_TrfFlnx, '-', '')||'-'||replace(v_TrfFlny, '-', '');
        
        begin            

            v_sqlStatement := 'INSERT INTO SERVICE_ACTIVITY
                             (
                                ADDRESS,HOUSE_NO,ADDR_LEAD_DIR_IND,STREET_NAME,STREET_TYPE,ADDR_TRAIL_DIR_IND,DWELL_TYPE_C,UNIT_H,SERVICE_ORDER_NO,
                                ESI_LOCATION,TOWN_NM,SERVICE_INFO_CODE,O_OR_U_CODE,SERVICE_CENTER_CODE,STRUCTURE_ID,FLNX_H,FLNY_H,TRF_CO_H,
                                TRF_STRUCTURE_ID,TRF_FLNX_H,TRF_FLNY_H,CU_ID,MGMT_ACTIVITY_CODE,STATUS_C,MSG_T,USER_ID,EXIST_PREM_GANGBASE,
                                REMARKS_MOBILE,METER_LATITUDE,METER_LONGITUDE
                             )  
                             VALUES
                             (:b_Address,:b_HouseNumber,:b_LeadingDirection,:b_StreetName,:b_StreetType,:b_TrailingDirection,:b_DwellingType,
                              :b_Unit,:b_ServiceOrderNo,:b_EsiLocation,:b_Town,:b_ServiceInfoCode,:b_OorU,:b_ServiceCenterCode,:b_StructureID,
                              :b_Flnx,:b_Flny,:b_TrfCo,:b_TrfStructureID,:b_TrfFlnx,:b_TrfFlny,:b_Cu,:b_MgmtActivityCode,:b_Status,:b_Msg,
                              :b_UserId,:b_ExistPremGangbase,:b_RemarksMobile,:b_MeterLatitude,:b_MeterLongitude
                             )';
                             
            execute immediate v_sqlStatement 
                    using v_Address,v_HouseNumber,v_LeadingDirection,v_StreetName,v_StreetType,v_TrailingDirection,v_DwellingType,v_Unit,
                          v_ServiceOrderNo,v_EsiLocation,v_Town,v_ServiceInfoCode,v_OorU,v_ServiceCenterCode,v_StructureID,v_Flnx,v_Flny,
                          v_TrfCo,v_TrfStructureID,v_TrfFlnx,v_TrfFlny,v_Cu,v_MgmtActivityCode,v_Status,v_Msg,v_UserId,v_ExistPremGangbase,
                          v_RemarksMobile,v_MeterLatitude,v_MeterLongitude;
           
        exception
            when others then
            p_ErrorMessage :=  'Failed to add record to the SERVICE_ACTIVITY GIS table: '||SQLERRM;
            p_ErrorCode := 'GISCFT04';
            p_status := false;
            rollback;
            return;
        end;
        
        v_sqlStatement := 'update STG_SERVICE_ACTIVITY set gis_process_flag = ''Y'' where stg_service_activity_id = :b1';
        
        execute immediate v_sqlStatement using p_stg_service_activity_id;
        
        commit;
        
        dbms_output.put_line ('InsertServiceActivityRecord ended');
    
    exception
        when others then
            p_ErrorMessage := 'GISPKG_CCB_ESILOCATION.InsertServiceActivityRecord: ' || SQLERRM;
            p_ErrorCode := 'GISCFT04';
            p_status := false;
            rollback;
    end;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- This procedure inserts a record into the STG_SERVICE_ACTIVITY table. 
    -- This procedure will return the STG_SERVICE_ACTIVITY_ID for the newly inserted record.
    -- -----------------------------------------------------------------------------------------------------------------------------
    procedure InsertFieldActivityRecord
    (
      p_service_order_no	                in	varchar2,
      p_esi_location	                    in	varchar2,
      p_address	                            in	varchar2,
      p_town_nm	                            in	varchar2,
      p_service_info_code	                in	varchar2,
      p_trans_date	                        in	varchar2,
      p_o_or_u_code	                        in	varchar2,
      p_service_center_code	                in	varchar2,
      p_flnx_h	                            in	varchar2,
      p_flny_h	                            in	varchar2,
      p_trf_co_h	                        in	varchar2,
      p_TRF_FLNX_H	                        in	varchar2,
      p_TRF_FLNY_H	                        in	varchar2,
      p_cu_id	                            in	varchar2,
      p_mgmt_activity_code	                in	varchar2,
      p_gis_process_flag	                in	varchar2,
      p_status_c	                        in	varchar2,
      p_msg_t	                            in	varchar2,
      p_dwell_type_c	                    in	varchar2,
      p_unit_h	                            in	varchar2,
      p_user_id	                            in	varchar2,
      p_exist_prem_gangbase	                in	varchar2,
      p_remarks_mobile	                    in	varchar2,
      p_meter_latitude	                    in	varchar2,
      p_meter_longitude	                    in	varchar2,
      p_service_activity_id                 out number,
      p_status                              out boolean,     -- Indicates success or failure
      p_ErrorMessage                        out VARCHAR2,    -- Error message if failure
      p_ErrorCode                           out VARCHAR2     -- Error code if failure
    )
    as
        v_sqlStatement                      varchar2(4000);
    pragma autonomous_transaction;
    begin

        dbms_output.put_line ('InsertFieldActivityRecord started');
        p_status := true;
        v_sqlStatement := 'INSERT INTO STG_SERVICE_ACTIVITY (SERVICE_ORDER_NO,ESI_LOCATION,ADDRESS,TOWN_NM,SERVICE_INFO_CODE,TRANS_DATE,O_OR_U_CODE,
                       SERVICE_CENTER_CODE,FLNX_H,FLNY_H,TRF_CO_H,TRF_FLNX_H,TRF_FLNY_H,CU_ID,MGMT_ACTIVITY_CODE,GIS_PROCESS_FLAG,STATUS_C,MSG_T,
                       DWELL_TYPE_C,UNIT_H,USER_ID,EXIST_PREM_GANGBASE,REMARKS_MOBILE,METER_LATITUDE,METER_LONGITUDE) values (
                       :b_serviceOrder,:b_esiLocation,:b_fulladdress,:b_town,:b_srvcinfocode,:b_transdate,:b_ooru,:b_srvccenter,:b_flnx,:b_flny,
                       :b_trfco,:b_trfflnx,:b_trfflny,:b_cu,:b_mgmtcode,:b_pflag,:b_status,:b_msg,:b_dwelltype,:b_unit,:b_user,:b_gangbase,
                       :b_remarks,:b_lat,:b_long) returning (stg_service_activity_id) into :b_serviceID';
        execute immediate v_sqlStatement 
                using p_service_order_no,p_esi_location,p_address,p_town_nm,p_service_info_code,p_trans_date,p_o_or_u_code,p_service_center_code,
                      p_flnx_h,p_flny_h,p_trf_co_h,p_trf_flnx_h,p_trf_flny_h,p_cu_id,p_mgmt_activity_code,p_gis_process_flag,p_status_c,p_msg_t,
                      p_dwell_type_c,p_unit_h,p_user_id,p_exist_prem_gangbase,p_remarks_mobile,to_number(p_meter_latitude),to_number(p_meter_longitude)
                returning into p_service_activity_id;
      
        commit;
        dbms_output.put_line ('InsertFieldActivityRecord ended');

    exception
        when others then
            p_ErrorMessage := 'Failed to add record to the STG_SERVICE_ACTIVITY GIS table: ' || SQLERRM;
            p_ErrorCode := 'GISCFT01';
            p_status := false;
            rollback;
    end InsertFieldActivityRecord;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public function, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    function InsertSvcActivityRecordIFACE
    (
      p_service_order_no	                in	varchar2,
      p_esi_location	                    in	varchar2,
      p_address	                            in	varchar2,
      p_town_nm	                            in	varchar2,
      p_service_info_code	                in	varchar2,
      p_o_or_u_code	                        in	varchar2,
      p_service_center_code	                in	varchar2,
      p_flnx_h	                            in	varchar2,
      p_flny_h	                            in	varchar2,
      p_trf_co_h	                        in	varchar2,
      p_TRF_FLNX_H	                        in	VARCHAR2,
      p_TRF_FLNY_H	                        in	VARCHAR2,
      p_cu_id	                            in	varchar2,
      p_mgmt_activity_code	                in	varchar2,
      p_msg_t	                            in	varchar2,
      p_dwell_type_c	                    in	varchar2,
      p_unit_h	                            in	varchar2,
      p_user_id	                            in	varchar2,
      p_exist_prem_gangbase	                in	varchar2,
      p_remarks_mobile	                    in	varchar2,
      p_meter_latitude	                    in	varchar2,
      p_meter_longitude                     in	varchar2
    ) 
    return varchar2 
    as
        v_StgServiceActivityID              STG_SERVICE_ACTIVITY.STG_SERVICE_ACTIVITY_ID%TYPE;        
        v_status                            boolean;
        v_ErrorMessage                      VARCHAR2(4000);
        v_ErrorCode                         VARCHAR2(20);
    BEGIN

        v_status:= true;
        InsertFieldActivityRecord (p_service_order_no,p_esi_location,p_ADDRESS,p_TOWN_NM,p_service_info_code,sysdate,p_o_or_u_code,p_service_center_code,
                                   p_flnx_h,p_flny_h,p_trf_co_h,p_trf_flnx_h,p_trf_flny_h,p_cu_id,p_mgmt_activity_code,'N','QUEUED',p_msg_t,
                                   p_dwell_type_c,p_unit_h,p_user_id,p_exist_prem_gangbase,p_remarks_mobile,to_number(p_meter_latitude),to_number(p_meter_longitude),
                                   v_StgServiceActivityID, v_status, v_ErrorMessage, v_ErrorCode);
    
        if not (v_status) then
            return GISPKG_EF_UTIL.GenerateResultString('FAILURE', v_ErrorCode, v_ErrorMessage);
        end if;
            
        dbms_output.put_line ('v_StgServiceActivityID:'||v_stgserviceactivityid);

        begin
            
            InsertServiceActivityRecord(v_StgServiceActivityID, v_status, v_ErrorMessage, v_ErrorCode);
            if (v_status) then
                return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');
            else
                return GISPKG_EF_UTIL.GenerateResultString('FAILURE', v_ErrorCode, v_ErrorMessage);
            end if;
        
        exception
            when others then
                return gispkg_ef_util.generateresultstring('FAILURE', 'GISCFT03', sqlerrm);  
        end;
        
        return GISPKG_EF_UTIL.GenerateResultString('SUCCESS', '', '');
    
    exception
        when others then
            return gispkg_ef_util.generateresultstring('FAILURE', 'GISCFT01', 'Failed to add record to the STG_SERVICE_ACTIVITY GIS table:'||sqlerrm);
 
  end InsertSvcActivityRecordIFACE;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public function, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    function GetServiceStructureID
    (
       p_G3E_FID                            in NUMBER           -- G3E_FID of Service Point
    )
    return VARCHAR2                                             -- Structure ID of connected Service Line
    as
       v_sqlStatement                       VARCHAR2(4000);
       p_Structure_ID                       COMMON_N.STRUCTURE_ID%TYPE;
    begin
        -- Query the GIS database to determine the Structure ID for the Service Line connected to the Service Point
        v_sqlStatement := 'select nvl(commServiceLine.structure_id, ''0-0'')
        From GIS.B$CONNECTIVITY_N connServicePoint, 
             GIS.B$CONNECTIVITY_N connServiceLine, 
             GIS.B$COMMON_N commServiceLine
        Where 
             (connServicePoint.NODE_1_ID = connServiceLine.NODE_1_ID Or connServicePoint.NODE_1_ID = connServiceLine.NODE_2_ID)
             and connServiceLine.g3e_fno = 54
             and connServiceLine.G3E_FID = commServiceLine.G3E_FID
             and connServicePoint.LTT_ID = 0
             and connServiceLine.LTT_ID = 0
             and commServiceLine.LTT_ID = 0
             and connServicePoint.G3E_FID = :b2';

        execute immediate v_sqlStatement into p_Structure_ID using p_G3E_FID;

        return p_Structure_ID;
        
    exception
        when others then
            p_Structure_ID := 'ERROR';
            return p_Structure_ID;
    end GetServiceStructureID;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public function, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    function CheckIfPopulatedAndPosted
    (
       p_Table_Name                         in VARCHAR2,        -- Name of table to query
       p_Field_Name                         in VARCHAR2,        -- Column in table to query
       p_G3E_ID                             in NUMBER           -- G3E_ID to check in table
    )
    return NUMBER                                               -- 0 if query returns zero records, otherwise 1.
    as
       v_sqlStatement                       VARCHAR2(4000);
       p_Status                             NUMBER(1);
    begin
        -- Query the GIS database to determine if the passed in field name is populated on the passed in table name and the record is posted.
        v_sqlStatement := 'select case when count(*) = 0 then 0 else 1 end case 
                         from GIS.' ||p_Table_Name|| ' 
                         where ' ||p_Field_Name|| ' is not null and g3e_id = :b2 and ltt_id = 0';
        execute immediate v_sqlStatement into p_Status using p_G3E_ID;

        return p_Status;
        
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.CheckIfPopulatedAndPosted: ' || SQLERRM);
    end CheckIfPopulatedAndPosted;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public function, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    function ValidatePremise
    (
       p_ESI_Location                       in  VARCHAR2        -- ESI Location to check for validation
    )
    return NUMBER                                               -- 0 if query returns zero records, otherwise 1.
    as
       v_sqlStatement                       VARCHAR2(4000);
       p_Status                             NUMBER(1);
    begin
        -- Query the CIS_ESI_LOCATIONS table to determine if the passed in ESI Location exists for a Premise.
        v_sqlStatement := 'select case when count(*) = 0 then 0 else 1 end case 
                         from CIS_ESI_LOCATIONS 
                         where esi_location = :b1 and service_point_type <> ''Street Light''';
        execute immediate v_sqlStatement into p_Status using p_ESI_Location;

        return p_Status;
        
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.ValidatePremise: ' || SQLERRM);
    end ValidatePremise;
    
    -- -----------------------------------------------------------------------------------------------------------------------------
    -- Public function, see package specification
    -- -----------------------------------------------------------------------------------------------------------------------------
    function ValidateStreetLightAccount
    (
       p_ESI_Location                       in  VARCHAR2        -- ESI Location to check for validation
    )
    return NUMBER                                               -- 0 if query returns zero records, otherwise 1.
    as
       v_sqlStatement                       VARCHAR2(4000);
       p_Status                             NUMBER(1);
    begin
        -- Query the CIS_ESI_LOCATIONS table to determine if the passed in ESI Location exists for a Street Light.
        v_sqlStatement := 'select case when count(*) = 0 then 0 else 1 end case 
                         from CIS_ESI_LOCATIONS 
                         where esi_location = :b1 and service_point_type = ''Street Light''';
        execute immediate v_sqlStatement into p_Status using p_ESI_Location;

        return p_Status;
        
    exception
        when others then
            raise_application_error(-20001, 'GISPKG_CCB_ESILOCATION.ValidateStreetLightAccount: ' || SQLERRM);
    end ValidateStreetLightAccount;
    
end;
/

CREATE OR REPLACE PUBLIC SYNONYM GISPKG_CCB_ESILOCATION FOR GIS_ONC.GISPKG_CCB_ESILOCATION;

GRANT EXECUTE ON GISPKG_CCB_ESILOCATION TO DESIGNER;
GRANT EXECUTE ON GISPKG_CCB_ESILOCATION TO PRIV_INT_CCB;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
--exec adm_support.set_finish(?);
