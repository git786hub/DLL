set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\BizRules_Validation.log
--**************************************************************************************
-- SCRIPT NAME: BizRules_Validation.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 02-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Business rule validation rule table
--**************************************************************************************


--DROP TABLE "GIS"."WR_VALIDATION_RULE";
DROP TABLE "GIS_ONC"."WR_VALIDATION_RULE";
DROP PUBLIC SYNONYM "WR_VALIDATION_RULE";

CREATE TABLE "GIS_ONC"."WR_VALIDATION_RULE" (
  "RULE_ID"   VARCHAR2(10 BYTE)   NOT NULL ENABLE 
, "RULE_NM"   VARCHAR2(50 BYTE)   NOT NULL ENABLE 
, "RULE_MSG"  VARCHAR2(256 BYTE)  NOT NULL ENABLE 
, CONSTRAINT "WRVALIDATIONRULE_PK" PRIMARY KEY ("RULE_ID")
);

CREATE PUBLIC SYNONYM "WR_VALIDATION_RULE" FOR "GIS_ONC"."WR_VALIDATION_RULE";

GRANT SELECT ON WR_VALIDATION_RULE TO PRIV_EDIT;

Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('DRTG01','LOAD EXCEEDS DEVICE RATING',                        'The load for this device exceeds its rating.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN01','NOT CONNECTED: FEATURE STATE {0}',                  'Feature in {0} state must be connected.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN02','FEATURE CONNECTED TO ITSELF',                       'Feature may not be connected to itself.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN03','CONNECTED POINT FEATURES SHOULD SHARE OWNER',       'Connected point features should have the same owner.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN04','SERVICE LINE MUST CONNECT TO SERVICE POINT',        'Service Line must be connected to a Service Point.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN05','DEAD END MUST CONNECT TO ONLY ONE CONDUCTOR',       'Dead End must be connected to only one conductor.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN06','CONNECTIVITY REQUIRED FOR COMPANY STREET LIGHTS',   'Connectivity is required for company-owned Street Lights.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('EQAP01','ONLY WIRELESS ANTENNAS MAY BE ON TOP OF POLE',      'Only Wireless Antennas may be placed at the top of pole position.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PESI01','ESI LOCATION REQUIRED FOR PPOD PREMISES',           'ESI Location is required for premises at Service Points connected to Primary Points of Delivery.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('HGT01' ,'HEIGHT ATTRIBUTE NOT FORMATTED PROPERLY',           'Height attribute is not formatted properly (FT''IN").');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('OWN01' ,'SPAN FEATURE MUST HAVE TWO OWNERS',                 'Span features must have two different owners.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('OWNC01','OWNING COMPANY REQUIRED FOR FOREIGN FEATURE',       'Owning Company is required for foreign features.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHAG01','PHASE NOT PRESENT IN CONNECTED FEATURES',           'Feature contains a phase not present in connected features.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHAG02','NOT ALL PHASES CARRIED BETWEEN NODES',              'Not all phases are carried between nodes of the feature.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHAG03','SINGLE-PHASE DEVICES CONNECTED IN PARALLEL',        'Single-phase devices with the same phase connected in parallel.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHPS01','OVERHEAD CONDUCTOR MISSING PHASE POSITION VALUES',  'Overhead conductor is missing phase position values.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHPS02','PHASE POSITION ATTRIBUTES ARE NOT CONSISTENT',      'Phase Position attributes are not consistent with respect to one another.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PCND01','UG PRIMARY CONDUCTOR SHOULD HAVE PULL BOX',         'UG Primary Conductor longer than 800 feet should have a Primary Pull Box installed.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PCND02','OH SPAN LENGTH EXCEEDS RECOMMENDED MAXIMUM',        'OH span length exceeds recommended length {0} for {1} type.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PCND03','PRIMARY CONDUCTOR MAY HAVE ONE WIRE PER PHASE',     'Primary Conductor may have only one wire per phase.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PDEV01','1 AND 2 PHASE MAY NOT BE PROTECTED BY SUB BREAKER', 'Single and two-phase features may not be protected by a Substation Breaker.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PDEV02','RECLOSER SHOULD BE PROTECTED BY SUB BREAKER',       'Reclosers should be protected directly by a Substation Breaker or another Recloser.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('TRFV01','SECONDARY VOLTAGE SHOULD NOT EXCEED PRIMARY',       'Secondary voltage should not exceed primary voltage.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('ISO01' ,'FEATURE REQUIRES ISOLATION POINTS AT BOTH NODES',   'Feature must be connected to Isolation Points at both nodes.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('ISO02' ,'FEATURE REQUIRES ISOLATION POINT',                  'Feature must be connected to an Isolation Point.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('ISO03' ,'FEATURE REQUIRES TWO ELBOWS',                       'Feature must have two elbows.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('ISO04' ,'FEATURE REQUIRES THREE BYPASS POINTS',              'Feature must be connected to three Bypass Points.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('JM1','WORK POINT NOT FOUND AT {0}','Work Point not found for activity at {0}');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('JM2','CU ACTIVITY MISSING FROM WORK POINT {0}','CU Activity is missing from Work Point {0}');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('JM3','WORK POINT NUMBER MUST BE GREATER THAN ZERO','Work Point Number must be greater than zero.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('JM4','WORK POINT NUMBER MUST BE UNIQUE TO WR','Work Point Number must be unique with respect to the WR.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHAG04','1 OR 2 PHASE FEATURE CONNECTED TO 3 PHASE FEATURE','1 or 2 phase feature is connected to a 3-phase feature other than a protective device.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('PHAG05','DOWNSTREAM PHASE NOT FED FROM UPSTREAM','One or more phases connected downstream of the feature are not present at the upstream node.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('NETMGMT01','USER OWNING TO RESTRICTED MANHOLE - MANAGED','Only network designers may own features to restricted Manholes.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('NETMGMT02','USER OWNING TO RESTRICTED MANHOLE - UNMANAGED','Only network designers may own features to restricted Manholes.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('VOLT01','VOLTAGE DIFFERS BETWEEN CONNECTED FEATURES','Voltage differs between connected features.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('OWNC02','OWNING COMPANY INCORRECT FOR COMPANY FEATURE','Owning Company must be {0} for company-owned features.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('FEED01','FEEDER ATTRIBUTE DIFFERS FROM CONNECTED FEATURE','{0} value {1} differs from related feature value of {2}.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('FUSE01','FUSE NOT COORDINATED WITH UPSTREAM FUSES',           'Fuse must be at least two sizes smaller than upstream fuses.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('FUSE02','FUSE NOT COORDINATED WITH DOWNSTREAM FUSES',         'Fuse must be at least two sizes larger than downstream fuses.');
Insert into GIS_ONC.WR_VALIDATION_RULE (RULE_ID,RULE_NM,RULE_MSG) values ('CONN07','PROPOSED REMOVAL WILL CAUSE CONNECTIVITY BREAK','Removal of this feature will cause downstream features to be disconnected.');

COMMIT;

spool off;
