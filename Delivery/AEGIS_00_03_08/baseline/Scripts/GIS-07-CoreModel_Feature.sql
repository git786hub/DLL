set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\CoreModel_Feature.log
--**************************************************************************************
-- SCRIPT NAME: CoreModel_Feature.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 01-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Update feature-level metadata
--**************************************************************************************


-- NOTE: Initially this script includes only values for G3E_REPLACE.  If additional feature-level
--    metadata is added to this script in the future, maintain a single update statement per
--    feature class and add the new column(s) to each one.


-- Initialize metadata

update G3E_FEATURE set G3E_REPLACE=0;

-- Update metadata

update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='AMS Collector';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='AMS Router';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Area Light';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Arrestor';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Autotransformer';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Capacitor';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='CES Battery';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Conduit';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='DA Fiber Modem';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='DA Radio';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Electronic Marker';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Fault Indicator';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Fuse Saver';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Guy';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Manhole';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Network Protector';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Pad';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Pole';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Conductor - OH';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Conductor - OH Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Conductor - UG';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Conductor - UG Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Enclosure';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Fuse - OH';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Fuse - OH Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Fuse - UG';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Fuse - UG Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Point of Delivery';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Pull Box';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Splice';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Splice - Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Switch - OH';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Switch - OH Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Switch - UG';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Switch - UG Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Primary Switch Gear';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Rack';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Recloser - OH';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Recloser - UG';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Remote Terminal Unit';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Riser';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Box';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Breaker';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Breaker - Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Conductor - OH';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Conductor - OH Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Conductor - UG';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Conductor - UG Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Enclosure';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Fuse';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Fuse - Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Splice - Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Secondary Switch Gear';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Service Line';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Street Light';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Street Light Control';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Street Light Standard';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Transformer - OH';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Transformer - OH Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Transformer - UG';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Transformer - UG Network';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Vault';
update G3E_FEATURE set G3E_REPLACE=1 where G3E_USERNAME='Voltage Regulator';


spool off;
