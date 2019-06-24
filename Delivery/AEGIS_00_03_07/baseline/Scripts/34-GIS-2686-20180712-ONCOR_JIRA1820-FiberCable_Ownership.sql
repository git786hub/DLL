
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2686, 'ONCOR_JIRA1820-FiberCable_Ownership');
spool c:\temp\.log
--**************************************************************************************
--SCRIPT NAME: 2686-20180712-ONCOR_JIRA1820-FiberCable_Ownership.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 12-JUL-2018
-- CYCLE				: 00.03.07
-- JIRA NUMBER	  		: 1820
-- PRODUCT VERSION	    : 10.3
-- PRJ IDENTIFIER	    : G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	        : Configure Owns relationship with Fiber Cable as per DFS
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

---**Fiber Cable with Fiber Building Ownership relationship***
Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Fiber Building'),1,'OWNER1_ID',null,(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Building'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);

Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Fiber Building'),1,'OWNER1_ID',null,(select g3e_primarydetailcno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarydetailcno from g3e_feature where g3e_username='Fiber Building'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


---***Fiber Cable ownership relationship with Vault***

Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Vault'),1,'OWNER1_ID',null,(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarygeographiccno from g3e_feature where g3e_username='Vault'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Vault'),1,'OWNER1_ID',null,(select g3e_primarydetailcno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarydetailcno from g3e_feature where g3e_username='Vault'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


---***Fiber Cable ownership relationship with Manhole***

Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Manhole'),1,'OWNER1_ID',null,(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarygeographiccno from g3e_feature where g3e_username='Manhole'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Manhole'),1,'OWNER1_ID',null,(select g3e_primarydetailcno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarydetailcno from g3e_feature where g3e_username='Manhole'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


---***Fiber Cable ownership relationship with Transmission Tower***

Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Transmission Tower'),1,'OWNER1_ID',null,(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarygeographiccno from g3e_feature where g3e_username='Transmission Tower'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


---***Fiber Cable ownership relationship with Pole***
Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Pole'),1,'OWNER1_ID',null,(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarygeographiccno from g3e_feature where g3e_username='Pole'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);

---***Fiber Cable ownership relationship with Primary Enclosure***
Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_fno from g3e_feature where g3e_username='Primary Enclosure'),1,'OWNER1_ID',null,(select g3e_primarygeographiccno from g3e_feature where g3e_username='Fiber Cable'),(select g3e_primarygeographiccno from g3e_feature where g3e_username='Primary Enclosure'),0,3,1,'OWNER2_ID',null,10,'P4',0,0,sysdate);


---**Fiber Rack with Room ownership relationship***
Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Rack'),(select g3e_fno from g3e_feature where g3e_username='Room'),1,'OWNER1_ID',null,(select g3e_primarydetailcno from g3e_feature where g3e_username='Fiber Rack'),(select g3e_primarydetailcno from g3e_feature where g3e_username='Room'),0,3,1,null,null,10,'P4',0,0,sysdate);



Insert into G3E_OWNERSHIP(G3E_ONO,G3E_SOURCEFNO,G3E_OWNERFNO,G3E_SOURCECNO,G3E_OWNERFIELD,G3E_OWNERCNOFIELD,G3E_TRIGGERINGCNO,G3E_OWNERTRIGGERINGCNO,G3E_ENFORCEMENT,G3E_DELETE,G3E_BREAKLINEAR,G3E_SECONDOWNERFIELD,G3E_SECONDOWNERCNOFIELD,G3E_MAXOFFSET,G3E_OFFSETVALIDATIONPRIORITY,G3E_BREAKTOLERANCE,G3E_ALLOWINTERIORRELATION,G3E_EDITDATE) values ((select max(g3e_ono)+1 from G3E_OWNERSHIP),(select g3e_fno from g3e_feature where g3e_username='Fiber Rack'),(select g3e_fno from g3e_feature where g3e_username='Vault'),1,'OWNER1_ID',null,(select g3e_primarydetailcno from g3e_feature where g3e_username='Fiber Rack'),(select g3e_primarydetailcno from g3e_feature where g3e_username='Vault'),0,3,1,null,null,10,'P4',0,0,sysdate);


---***Update DetailPlacement***
Update G3e_featureComponent set g3e_required=1,g3e_detailplacementcno=7211 where G3E_FNO=7200 AND G3E_CNO=7210;
Update G3e_featureComponent set g3e_detailplacementcno=7231 where G3E_FNO=7200 AND G3E_CNO=7230;


Update G3e_featureComponent set g3e_required=1,g3e_detailplacementcno=11821 where G3E_FNO=11800 AND G3E_CNO=11820;
Update G3e_featureComponent set  g3e_required=1,g3e_detailplacementcno=11831 where G3E_FNO=11800 AND G3E_CNO=11830;

COMMIT;

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2686);

