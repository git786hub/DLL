
set echo on
set linesize 1000
set pagesize 300
set define off

whenever sqlerror exit -1
whenever oserror exit -1

spool c:\temp\PopulateCablePullTensionCommandSupportTables.log

--**************************************************************************************

--SCRIPT NAME: PopulateCablePullTensionCommandSupportTables.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\RPGABRYS
-- Date 	      	    : 14-JUL-2017
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.2.04
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Populate tables to support the Cable Pull Tension custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

INSERT INTO TOOLS_CPT_CABLECONFIGURATION (CONFIGURATION) VALUES ('Single');
INSERT INTO TOOLS_CPT_CABLECONFIGURATION (CONFIGURATION) VALUES ('Cradled');
INSERT INTO TOOLS_CPT_CABLECONFIGURATION (CONFIGURATION) VALUES ('Triangular');
INSERT INTO TOOLS_CPT_CABLECONFIGURATION (CONFIGURATION) VALUES ('Diamond');

INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#6 Al 600 V Duplex','600 V','Duplex','#6 Al','1-Phase',2,3,'S',0.3,0.11,1200,580,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#4 Al 600 V Duplex','600 V','Duplex','#4 Al','1-Phase',2,3,'S',0.35,0.14,1200,920,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#1/0 Al 600 V Triplex','600 V','Triplex','#1/0 Al','1-Phase',2,3,'S',0.52,0.41,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#4/0 Al 600 V Triplex','600 V','Triplex','#4/0 Al','1-Phase',2,3,'S',0.67,0.74,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('350 kcmil Al 600 V Triplex','600 V','Triplex','350 Al','1-Phase',2,3,'S',0.85,1.17,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('500 kcmil Al 600 V Triplex','600 V','Triplex','500 Al','1-Phase',2,2,'S',0.98,1.51,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#1/0 Al 600 V Quadruplex','600 V','Quadruplex','#1/0 Al','3-Phase',2,4,'S',0.52,0.57,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#4/0 Al 600 V Quadruplex','600 V','Quadruplex','#4/0 Al','3-Phase',2,4,'S',0.67,1.02,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('350 kcmil Al 600 V Quadruplex','600 V','Quadruplex','350 Al','3-Phase',2,4,'S',0.85,1.615,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('500 kcmil Al 600 V Quadruplex','600 V','Quadruplex','500 Al','3-Phase',2,4,'S',0.98,2.125,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1000 kcmil Al 600 V Quadruplex','600 V','Quadruplex','1000 Al','3-Phase',2,4,'S',1.34,4.155,1200,2000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('#4 Al 2 kV Duplex St. Light Cable','2 kV','Duplex','#4 Al','1-Phase',2,3,'S',0.37,0.15,1200,920,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#2 Al 15 kV XLPE','15 kV','XLPE','#2 Al','1-Phase',1,1,'N',0.9,0.371,1200,730,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#2 Al 15 kV XLPE','15 kV','XLPE','#2 Al','3-Phase',3,2,'N',0.9,1.113,750,1460,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#1/0 Al 15 kV XLPE','15 kV','XLPE','#1/0 Al','1-Phase',1,1,'N',0.98,0.511,1200,1160,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#1/0 Al 15 kV XLPE','15 kV','XLPE','#1/0 Al','3-Phase',3,2,'N',0.98,1.533,750,2320,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#4/0 Al 15 kV XLPE','15 kV','XLPE','#4/0 Al','1-Phase',1,1,'N',1.13,0.598,1200,2330,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#4/0 Al 15 kV XLPE','15 kV','XLPE','#4/0 Al','3-Phase',3,2,'N',1.13,1.794,750,4650,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Al 15 kV XLPE','15 kV','XLPE','500 Al','3-Phase',3,2,'N',1.49,3.621,750,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Al 15 kV XLPE','15 kV','XLPE','1000 Al','3-Phase',3,2,'N',1.75,5.88,750,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 15 kV XLPE','15 kV','XLPE','1000 Cu','3-Phase',3,2,'N',1.75,12.16,750,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#1/0 Al 15 kV TRXLPE w/ Jacket','15 kV','TRXLPE w/jkt','#1/0 Al','1-Phase',1,1,'S',1.06,0.672,2000,1160,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#1/0 Al 15 kV TRXLPE w/ Jacket','15 kV','TRXLPE w/jkt','#1/0 Al','3-Phase',3,2,'S',1.06,2.016,2000,2320,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 15 kV RD TRXLPE w/ Jacket','15 kV','TRXLPE w/jkt','1000 Cu','3-Phase',3,2,'S',1.7,12.02355,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 15 kV PILC w/ Jacket','15 kV','PILC w/jkt','1000 Cu','3-Phase',3,2,'S',1.87,17.25,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#2 Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','#2 Al','1-Phase',1,1,'N',1,0.487,2000,730,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#2 Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','#2 Al','3-Phase',3,2,'N',1,1.461,2000,1460,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#1/0 Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','#1/0 Al','1-Phase',1,1,'N',1.08,0.632,2000,1160,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#1/0 Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','#1/0 Al','3-Phase',3,2,'N',1.08,1.896,2000,2320,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#4/0 Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','#4/0 Al','1-Phase',1,1,'N',1.23,0.759,2000,2330,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#4/0 Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','#4/0 Al','3-Phase',3,2,'N',1.23,2.277,2000,4650,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','500 Al','3-Phase',3,2,'N',1.56,4.494,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Al 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','1000 Al','3-Phase',3,2,'N',1.91,7.15,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','1000 Cu','3-Phase',3,2,'N',1.91,13.43,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#1/0 Al 25 kV XLPE','25 kV','XLPE','#1/0 Al','1-Phase',1,1,'N',1.17,0.612,1200,1160,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#1/0 Al 25 kV XLPE','25 kV','XLPE','#1/0 Al','3-Phase',3,2,'N',1.17,1.836,750,2320,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#4/0 Al 25 kV XLPE','25 kV','XLPE','#4/0 Al','1-Phase',1,1,'N',1.34,0.943,1200,2330,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#4/0 Al 25 kV XLPE','25 kV','XLPE','#4/0 Al','3-Phase',3,2,'N',1.34,2.829,750,4650,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Al 25 kV XLPE','25 kV','XLPE','500 Al','3-Phase',3,2,'N',1.64,3.723,750,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Al 25 kV XLPE','25 kV','XLPE','1000 Al','3-Phase',3,2,'N',2.05,6.198,750,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 25 kV XLPE','25 kV','XLPE','1000 Cu','3-Phase',3,2,'N',2.05,12.48,750,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#1/0 Al 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','#1/0 Al','1-Phase',1,1,'S',1.23,0.814,2000,1160,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#1/0 Al 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','#1/0 Al','3-Phase',3,2,'S',1.23,2.442,2000,2320,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#4/0 Al 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','#4/0 Al','1-Phase',1,1,'S',1.41,1.052,2000,2330,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#4/0 Al 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','#4/0 Al','3-Phase',3,2,'S',1.41,3.156,2000,4650,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Al 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','500 Al','3-Phase',3,2,'N',1.82,4.623,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Al 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','1000 Al','3-Phase',3,2,'S',2.15,7.689,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','1000 Cu','3-Phase',3,2,'S',2.15,13.97,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Al 25 kV EPR w/ Jacket','25 kV','EPR w/jkt','1000 Al','3-Phase',3,2,'N',2.22,8.847,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-1000 kcmil Cu 25 kV EPR w/ Jacket','25 kV','EPR w/jkt','1000 Cu','3-Phase',3,2,'N',2.22,15.13,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('1-#1/0 Cu 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','#1/0 Cu','1-Phase',1,1,'S',1.06,2.67609687184662,2000,1160,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#1/0 Cu 25 kV TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','#1/0 Cu','3-Phase',3,2,'S',1.06,8.02829061553986,2000,2320,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#2/0 Cu 15 kV PILC w/ Jacket','15 kV','PILC w/jkt','#2/0 Cu','3-Phase',3,2,'N',1.2,5,2000,2930,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-250 kcmil Cu 15 kV PILC w/ Jacket','15 kV','PILC w/jkt','250 Cu','3-Phase',3,2,'S',1.31,7,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Cu 15 kV PILC w/ Jacket','15 kV','PILC w/jkt','500 Cu','3-Phase',3,2,'S',1.505,10.2,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-750 kcmil Cu 15 kV PILC w/ Jacket','15 kV','PILC w/jkt','750 Cu','3-Phase',3,2,'S',1.69,13.71,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Cu 15 kV XLPE w/ Jacket','15 kV','XLPE w/jkt','500 Cu','3-Phase',3,2,'N',1.52,7.1826,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Cu 25 kV RD TRXLPE w/ Jacket','25 kV','TRXLPE w/jkt','500 Cu','3-Phase',3,2,'S',1.55,7.47,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-#2/0 Cu 25 kV PILC w/ Jacket','25 kV','PILC w/jkt','#2/0 Cu','3-Phase',3,2,'N',1.37,6.28,2000,2930,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-250 kcmil Cu 25 kV PILC w/ Jacket','25 kV','PILC w/jkt','250 Cu','3-Phase',3,2,'N',1.44,8.04,2000,5000,150);
INSERT INTO TOOLS_CPT_CABLE (DESCRIPTION,VOLTAGE,TYPE,SIZE_MATERIAL,NUMBER_OF_PHASES,NUMBER_OF_CABLES,CONFIGURATION,NON_STD,OUTSIDE_DIAMETER,WEIGHT_PER_FOOT,MAX_SWBP,MAX_TENSION,SWBP_CHANGE_POINT) VALUES ('3-500 kcmil Cu 25 kV PILC w/ Jacket','25 kV','PILC w/jkt','500 Cu','3-Phase',3,2,'N',1.665,11.56,2000,5000,150);


INSERT INTO TOOLS_CPT_DUCTTRENCH (DESCRIPTION, INSIDE_DIAMETER, NOMINAL_INSIDE_DIAMETER, STD_BEND_RADIUS, MIN_CLEARANCE) VALUES ('1"', 1.049,1,1.46,0.25);
INSERT INTO TOOLS_CPT_DUCTTRENCH (DESCRIPTION, INSIDE_DIAMETER, NOMINAL_INSIDE_DIAMETER, STD_BEND_RADIUS, MIN_CLEARANCE) VALUES ('2"', 2.067,2,1.91,0.5);
INSERT INTO TOOLS_CPT_DUCTTRENCH (DESCRIPTION, INSIDE_DIAMETER, NOMINAL_INSIDE_DIAMETER, STD_BEND_RADIUS, MIN_CLEARANCE) VALUES ('3"', 3.068,3,1.87,0.5);
INSERT INTO TOOLS_CPT_DUCTTRENCH (DESCRIPTION, INSIDE_DIAMETER, NOMINAL_INSIDE_DIAMETER, STD_BEND_RADIUS, MIN_CLEARANCE) VALUES ('4"', 4.026,4,1.83,0.5);
INSERT INTO TOOLS_CPT_DUCTTRENCH (DESCRIPTION, INSIDE_DIAMETER, NOMINAL_INSIDE_DIAMETER, STD_BEND_RADIUS, MIN_CLEARANCE) VALUES ('5"', 5.047,5,2.75,0.5);
INSERT INTO TOOLS_CPT_DUCTTRENCH (DESCRIPTION, INSIDE_DIAMETER, NOMINAL_INSIDE_DIAMETER, STD_BEND_RADIUS, MIN_CLEARANCE) VALUES ('6"', 6.261,6,2.75,0.5);


INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (15,2,3560);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (22,2,3560);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (28,2,3560);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (35,2,3560);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (42,2,3560);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (16,4,1860);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (23,4,1860);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (29,4,1860);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (36,4,1860);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (43,4,1860);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (18,4,2480);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (31,4,2480);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (38,4,2480);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (45,4,2480);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (19,6,2210);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (32,6,2210);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (39,6,2210);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (46,6,2210);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (20,6,1160);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (33,6,1160);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (40,6,1160);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (47,6,1160);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (49,6,1160);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (21,6,640);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (24,6,640);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (25,6,640);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (34,6,640);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (41,6,640);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (48,6,640);
INSERT INTO TOOLS_CPT_VALIDATION (CABLE_ID,DUCTTRENCH_ID,MAX_DISTANCE) VALUES (50,6,640);

--**************************************************************************************
-- End Script Body
--**************************************************************************************