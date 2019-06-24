
set echo on
set linesize 1000
set pagesize 300
set define off

spool c:\temp\AddSecondaryCalculatorMetadataParameters.log

--**************************************************************************************
--SCRIPT NAME: AddSecondaryCalculatorMetadataParameters.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET/RPGABRYS
-- Date 	      	    : 01-MAR-2018
-- Jira NUMBER	  	    :
-- PRODUCT VERSION  	: 10.03
-- PRJ IDENTIFIER   	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC     	: Add metadata parameters for Secondary Calculator custom command.
-- SOURCE DATABASE  	:
--**************************************************************************************
-- Script Body
--************************************************************************************** 

-- Remove implementation if exists so script can be reinstalled
DELETE FROM SYS_GENERALPARAMETER WHERE SUBSYSTEM_NAME = 'SecondaryCalculatorCC';

-- Add implementation
Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','Trace','TraceName','Secondary Calculation Trace','The G3E_TRACE.G3E_USERNAME of the trace to run.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','Report','ReportFileName','C:\\ReportTemplates\\DesignToolReports.xlsx','The full path to the report workbook template.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','Report','ReportName','Secondary Calculator Report','The name of the spreadsheet tab in the report workbook that contains the report.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','TransformerSizing','XfmrSizingFileName','C:\\Temp\\TransformerSizing\\Single Phase Transformer Sizing.xlsx','The full path to the Transformer Sizing workbook.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','Summer','3','Diversity factor multiplier for Summer. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','Winter','2','Diversity factor multiplier for Winter. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','CustomerCountHigh','31','Diversity factor high customer count value. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','CustomerSummerHigh','0.555','The Summer factor value to use when the customer count is greater than the diversity factor high customer count. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','CustomerSummerLow','0.545','The Summer factor value to use when the customer count is lower than the diversity factor high customer count. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','CustomerWinterHigh','0.680','The Winter factor value to use when the customer count is greater than the diversity factor high customer count. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','DiversityFactor','CustomerWinterLow','0.722','The Winter factor value to use when the customer count is lower than the diversity factor high customer count. Used in Load KVA calculation.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','VoltageThreshold','Transformer','2.8','The Transformer voltage drop percentage threshold to issue an error.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','VoltageThreshold','Secondary','5.8','The Secondary Conductor voltage drop percentage threshold to issue an error.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','VoltageThreshold','Service','6.8','The Service Line voltage drop percentage threshold to issue an error.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','FlickerThreshold','SecondaryLow','5','The Secondary Conductor flicker percentage threshold to issue a warning.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','FlickerThreshold','SecondaryHigh','6','The Secondary Conductor flicker percentage threshold to issue an error.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','FlickerThreshold','ServiceLow','5','The Service Line flicker percentage threshold to issue a warning.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','FlickerThreshold','ServiceHigh','6','The Service Line flicker percentage threshold to issue an error.');

Insert into SYS_GENERALPARAMETER (SUBSYSTEM_NAME,SUBSYSTEM_COMPONENT,PARAM_NAME,PARAM_VALUE,PARAM_DESC) values 
('SecondaryCalculatorCC','EnablingCondition','3PhaseVoltages','120/208Y 4W,240D 3W,277/480Y 4W,480D 3W','Comma separated list of valid voltages for 3-phase Transformer');

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;

