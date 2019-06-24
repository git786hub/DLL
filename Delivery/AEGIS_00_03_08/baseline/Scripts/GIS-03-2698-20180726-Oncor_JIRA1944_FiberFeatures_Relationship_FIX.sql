
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2698, 'Oncor_JIRA1944_FiberFeatures_Relationship_FIX');
spool c:\temp\2698-20180726-Oncor_JIRA1944_FiberFeatures_Relationship_FIX.log
--**************************************************************************************
--SCRIPT NAME: 2698-20180726-Oncor_JIRA1944_FiberFeatures_Relationship_FIX.sql
--**************************************************************************************
-- AUTHOR		        : INGRNET\PVKURELL
-- DATE		            : 26-JUL-2018
-- CYCLE	            : 00.03.08
-- JIRA NUMBER			: 1944
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: Remove fiber feature relationship with related feature detail components from relationship tables 
--                        as these are obsolete causing error at placement                      
-- SOURCE DATABASE		:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---**Fiber Building relationship with Fiber Duct,Fiber Inner Duct***
Delete FROM G3E_DIMRELATIONSHIP WHERE G3E_CONNECTEDFNO=14100;
Delete FROM G3E_OWNERSHIP WHERE G3E_OWNERFNO=14100 AND G3E_SOURCEFNO IN (14700,10300);

---**Fiber splice enclosure relationship with Fiber Duct,Fiber Inner Duct***
Delete from G3E_NODEEDGECONNECTIVITY where G3E_SOURCEFNO =11800 and G3E_CONNECTINGFNO in (4000,4100) AND G3E_CONNECTINGTRIGGERINGCNO IN (4011,4111);
Delete from G3E_NODEEDGECONNECTIVITY where G3E_CONNECTINGFNO =11800 and G3E_SOURCEFNO in (4000,4100) AND G3E_TRIGGERINGCNO IN (4011,4111);

Delete FROM G3E_OWNERSHIP WHERE G3E_OWNERFNO=11800 AND G3E_SOURCEFNO IN (12300,12500);
Delete from G3E_NODEORDCONNECTIVITY where G3E_SOURCEFNO =11800 and G3E_CONNECTINGFNO=7300 AND G3E_CONNECTINGTRIGGERINGCNO IN (7311,30020);
Delete from G3E_NODEORDCONNECTIVITY where G3E_CONNECTINGFNO =11800 and G3E_SOURCEFNO=7300 AND G3E_TRIGGERINGCNO IN (7311,30020);

---***FIBER CABLE***

Delete from G3E_NODEORDCONNECTIVITY where G3E_SOURCEFNO =7200 and G3E_CONNECTINGFNO IN (12000, 12100, 12400, 12000, 12100, 12400, 14700, 14700) AND G3E_CONNECTINGTRIGGERINGCNO IN (12021, 12121, 12421, 30029, 30031, 30038, 14721, 30033);
Delete from G3E_NODEORDCONNECTIVITY where G3E_CONNECTINGFNO  =7200 and G3E_SOURCEFNO IN (12000, 12100, 12400, 12000, 12100, 12400, 14700, 14700) AND G3E_TRIGGERINGCNO IN (12021, 12121, 12421, 30029, 30031, 30038, 14721, 30033);

---***Foreign communication cable***

Delete FROM G3E_MANYOWNERSHIP WHERE G3E_SOURCEFNO=7203 AND G3E_OWNERFNO IN (4000,4100) AND g3e_ownertriggeringcno IN (4011,4111);

---***Fiber Rack***

Delete from G3E_NODEEDGECONNECTIVITY where G3E_SOURCEFNO =15700 and G3E_CONNECTINGFNO=4100 AND G3E_CONNECTINGTRIGGERINGCNO=4111;
Delete from G3E_NODEEDGECONNECTIVITY where G3E_CONNECTINGFNO  =15700 and G3E_SOURCEFNO=4100 AND G3E_TRIGGERINGCNO=4111;

Delete FROM G3E_OWNERSHIP WHERE G3E_OWNERFNO=12400 AND G3E_SOURCEFNO =15700 AND g3e_ownertriggeringcno=12421;


--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2698);

