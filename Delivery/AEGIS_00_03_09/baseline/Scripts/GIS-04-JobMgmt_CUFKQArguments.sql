set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\JobMgmt_CUFKQArguments.sql.log
--**************************************************************************************
--SCRIPT NAME: JobMgmt_CUFKQArguments.sql
--**************************************************************************************
-- AUTHOR			: Barry Scott
-- DATE				: 01-FEB-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Create arguments and update tab attributes for CU Selection FKQ.
--**************************************************************************************
-- Modified:
--  Pramod - Added update to disble FKQ for Substation Breaker and Substation Breaker Network feature class
--  03-AUG-2018, Rich Adase -- Added new information for network features
--                             Restored ability to select ancillary CUs for Substation Breaker and Substation Breaker Network
--  07-AUG-2018, Rich Adase -- Corrected Autotransformer to use MAUTOTRF instead of AUTOTRF
--                             Updated Primary Switch to use PRISWITCHOH and PRISWITCHUG (instead of PRISWITCH for both)
--  14-AUG-2018, Barry Scott -- Added new information for Conduit
--  17-AUG-2018, Rich Adase -- Added entries for Fuse Saver (ALM 757)
--  23-AUG-2018, Barry Scott -- Removed network categories from non-network conductors
--  30-AUG-2018, Rich Adase -- Temporarily disabled NPRISWITCHOH and NPRISWITCHUG until we can separate tabs
--**************************************************************************************

--Script is rerunnable.

/*
-- These can be removed once the script is run once.  The definition was implemented incorrectly...
update g3e_tabattribute set g3e_fkqrino=null,g3e_fkqarggroupno=null where g3e_tano=6120603;
delete from g3e_relationifaceargs where g3e_riarggroupno=6120603;
*/

begin create_sequence.create_metadata_sequence('G3E_RELATIONIFACEARGS','G3E_ARGROWNO','G3E_RELATIONIFACEARGS_SEQ');end;
/

-- Ancillary CUs
delete from g3e_relationifaceargs where g3e_riarggroupno=6120602;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6120602,'ANCILLARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6120602,'N',sysdate);

-- Unsure if these will be needed.  Leaving them in and commented for now but can be removed if/when it is deemed they're not needed.
--insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6120603,'XAMS,XAREALIGHT,XAUTOMATION,XAUTOTRF,XCAPBANK,XCOMOH,XCOMUG,XCONDUIT,XCONTCABLE,XDMP PROJECT,XEQUIPMENTRACK,XFAM,XGUYANCHOR,XMAJOROH,XMANHOLE,XMAUTOCAM,XMETER,XOHSERVICE,XOHTRF,XPAD,XPAM,XPPOD,XPRIFUSE,XPRIOHCAM,XPRIRISER,XPRISWGEAR,XPRISWITCH,XPRIUGCAM,XPRIUGCAM2,XREACTOR,XRECLOSER,XRESOH,XRESUG,XSECOHCAM,XSECRISER,XSECUGCAM,XSLSTAND,XSTREETLIGHT,XTRENCH,XUGSERVICE,XUGTRF,XVAULT,XVOLTREG',sysdate);

update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6120602
where g3e_tano in(select ta.g3e_tano from g3e_tabattribute ta
									join g3e_dialogtab dt on ta.g3e_dtno=dt.g3e_dtno
									where ta.g3e_ano=2203 and ta.g3e_readonly=0);


/*
This SQL produced the complete set of statements below (without the proper category values and the aggregate (2nd) argument set to N).
There are a few duplicates that have been removed but this generates the basic skeleton.

select distinct '-- '||f.g3e_username,'FNO: '||f.g3e_fno,'DNO: '||d.g3e_dno,'Dialog Type: '||d.g3e_type,'DTNO: '||d.g3e_dtno,'Attribute: '||a.g3e_username,'TANO:'||ta.g3e_tano||chr(10)||
'delete from g3e_relationifaceargs where g3e_riarggroupno='||ta.g3e_tano||';'||chr(10)||
'insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,'||ta.g3e_tano||',''PRIMARY'',sysdate);'||chr(10)||
'insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,'||ta.g3e_tano||',''N'',sysdate);'||chr(10)||
'insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,'||ta.g3e_tano||',''Category'',sysdate);'||chr(10)||
'update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username=''CU Selection FKQ''),g3e_fkqarggroupno='||ta.g3e_tano||' where g3e_tano='||ta.g3e_tano||';'||chr(10)
from g3e_feature f
join g3e_dialog d on f.g3e_fno=d.g3e_fno
join g3e_tabattribute ta on d.g3e_dtno=ta.g3e_dtno
join g3e_dialogtab dt on ta.g3e_dtno=dt.g3e_dtno
join g3e_attribute a on ta.g3e_ano=a.g3e_ano
where a.g3e_ano in(2102,2103)
and lower(f.g3e_username) not like '%fiber%'
and lower(dt.g3e_username) not like 'cu attributes'
and lower(dt.g3e_username) not like 'ancillary cu attributes'
and (g3e_type!='Review' and lower(g3e_type) not like '%fiber%')
and ta.g3e_readonly=0
order by 1
*/

-- Area Light	FNO: 61	DNO: 61201	Dialog Type: Edit	DTNO: 61201	Attribute: CU Code	"TANO:612011
delete from g3e_relationifaceargs where g3e_riarggroupno=612011;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,612011,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,612011,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,612011,'AREALIGHT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=612011 where g3e_tano=612011;

-- Area Light	FNO: 61	DNO: 61301	Dialog Type: Placement	DTNO: 61301	Attribute: CU Code	"TANO:613011
delete from g3e_relationifaceargs where g3e_riarggroupno=613011;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,613011,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,613011,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,613011,'AREALIGHT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=613011 where g3e_tano=613011;

-- AMS Collector	FNO: 150	DNO: 15020	Dialog Type: Edit	DTNO: 150201	Attribute: CU Code	TANO:15020100
delete from g3e_relationifaceargs where g3e_riarggroupno=15020100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15020100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15020100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15020100,'COLLECTOR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15020100 where g3e_tano=15020100;

-- AMS Collector	FNO: 150	DNO: 15030	Dialog Type: AMS Collector	DTNO: 150301	Attribute: CU Code	TANO:15030100
delete from g3e_relationifaceargs where g3e_riarggroupno=15030100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15030100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15030100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15030100,'COLLECTOR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15030100 where g3e_tano=15030100;

-- AMS Router	FNO: 152	DNO: 15220	Dialog Type: Edit	DTNO: 152201	Attribute: CU Code	TANO:15220100
delete from g3e_relationifaceargs where g3e_riarggroupno=15220100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15220100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15220100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15220100,'ROUTER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15220100 where g3e_tano=15220100;

-- AMS Router	FNO: 152	DNO: 15230	Dialog Type: Placement	DTNO: 152301	Attribute: CU Code	TANO:15230100
delete from g3e_relationifaceargs where g3e_riarggroupno=15230100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15230100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15230100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15230100,'ROUTER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15230100 where g3e_tano=15230100;

-- Arrestor	FNO: 33	DNO: 3320	Dialog Type: Edit	DTNO: 33201	Attribute: CU Code	TANO:3320100
delete from g3e_relationifaceargs where g3e_riarggroupno=3320100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,3320100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,3320100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,3320100,'SURGEARR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=3320100 where g3e_tano=3320100;

-- Arrestor	FNO: 33	DNO: 3330	Dialog Type: Placement	DTNO: 33301	Attribute: CU Code	TANO:3330100
delete from g3e_relationifaceargs where g3e_riarggroupno=3330100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,3330100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,3330100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,3330100,'SURGEARR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=3330100 where g3e_tano=3330100;

-- Autotransformer	FNO: 34	DNO: 3420	Dialog Type: Edit	DTNO: 34202	Attribute: CU Code	TANO:3420200
delete from g3e_relationifaceargs where g3e_riarggroupno=3420200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,3420200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,3420200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,3420200,'MAUTOTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=3420200 where g3e_tano=3420200;

-- Autotransformer	FNO: 34	DNO: 3430	Dialog Type: Placement	DTNO: 34302	Attribute: CU Code	TANO:3430200
delete from g3e_relationifaceargs where g3e_riarggroupno=3430200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,3430200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,3430200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,3430200,'MAUTOTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=3430200 where g3e_tano=3430200;

-- Capacitor	FNO: 4	DNO: 420	Dialog Type: Edit	DTNO: 4201	Attribute: CU Code	TANO:510030289
delete from g3e_relationifaceargs where g3e_riarggroupno=510030289;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030289,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030289,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030289,'MCAPBANK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030289 where g3e_tano=510030289;

-- Capacitor	FNO: 4	DNO: 430	Dialog Type: Placement	DTNO: 4301	Attribute: CU Code	TANO:510030290
delete from g3e_relationifaceargs where g3e_riarggroupno=510030290;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030290,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030290,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030290,'MCAPBANK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030290 where g3e_tano=510030290;

-- Conduit	FNO: 104	DNO: 10420	Dialog Type: Edit	DTNO: 104201	Attribute: CU Code	TANO:10420104
delete from g3e_relationifaceargs where g3e_riarggroupno=10420104;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10420104,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10420104,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10420104,'CONDUIT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10420104 where g3e_tano=10420104;

-- Conduit	FNO: 104	DNO: 10430	Dialog Type: Placement	DTNO: 104301	Attribute: CU Code	TANO:10430104
delete from g3e_relationifaceargs where g3e_riarggroupno=10430104;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10430104,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10430104,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10430104,'CONDUIT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10430104 where g3e_tano=10430104;

-- DA Radio	FNO: 151	DNO: 15120	Dialog Type: Edit	DTNO: 151201	Attribute: CU Code	TANO:15120100
delete from g3e_relationifaceargs where g3e_riarggroupno=15120100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15120100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15120100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15120100,'DARADIO',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15120100 where g3e_tano=15120100;

-- DA Radio	FNO: 151	DNO: 15130	Dialog Type: Placement	DTNO: 151301	Attribute: CU Code	TANO:15130100
delete from g3e_relationifaceargs where g3e_riarggroupno=15130100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15130100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15130100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15130100,'DARADIO',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15130100 where g3e_tano=15130100;

-- Fault Indicator	FNO: 7	DNO: 720	Dialog Type: Edit	DTNO: 7201	Attribute: CU Code	TANO:720101
delete from g3e_relationifaceargs where g3e_riarggroupno=720101;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,720101,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,720101,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,720101,'FAULTIND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=720101 where g3e_tano=720101;

-- Fault Indicator	FNO: 7	DNO: 730	Dialog Type: Placement	DTNO: 7307	Attribute: CU Code	TANO:730701
delete from g3e_relationifaceargs where g3e_riarggroupno=730701;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,730701,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,730701,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,730701,'FAULTIND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=730701 where g3e_tano=730701;

-- Formation	FNO: 2400	DNO: 24020	Dialog Type: Edit	DTNO: 240201	Attribute: CU Code	TANO:24020001
delete from g3e_relationifaceargs where g3e_riarggroupno=24020001;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,24020001,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,24020001,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,24020001,'DUCTBANK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=24020001 where g3e_tano=24020001;

-- Formation	FNO: 2400	DNO: 24030	Dialog Type: Formation	DTNO: 240301	Attribute: CU Code	TANO:24030001
delete from g3e_relationifaceargs where g3e_riarggroupno=24030001;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,24030001,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,24030001,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,24030001,'DUCTBANK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=24030001 where g3e_tano=24030001;

-- Fuse Saver	FNO: 37	DNO: 37201	Dialog Type: Edit	DTNO: 37202	Attribute: CU Code	TANO:510030298
delete from g3e_relationifaceargs where g3e_riarggroupno=510030298;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030298,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030298,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030298,'FUSESAVER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030298 where g3e_tano=510030298;

-- Fuse Saver	FNO: 37	DNO: 37301	Dialog Type: Placement	DTNO: 37302	Attribute: CU Code	TANO:510030299
delete from g3e_relationifaceargs where g3e_riarggroupno=510030299;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030299,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030299,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030299,'FUSESAVER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030299 where g3e_tano=510030299;

-- Guy	FNO: 105	DNO: 10520	Dialog Type: Edit	DTNO: 105201	Attribute: CU Code	TANO:10520100
delete from g3e_relationifaceargs where g3e_riarggroupno=10520100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10520100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10520100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10520100,'MGUYANCHOR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10520100 where g3e_tano=10520100;

-- Guy	FNO: 105	DNO: 10530	Dialog Type: Placement	DTNO: 105301	Attribute: CU Code	TANO:10530100
delete from g3e_relationifaceargs where g3e_riarggroupno=10530100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10530100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10530100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10530100,'MGUYANCHOR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10530100 where g3e_tano=10530100;

-- Manhole	FNO: 106	DNO: 10620	Dialog Type: Edit	DTNO: 106201	Attribute: CU Code	TANO:10620101
delete from g3e_relationifaceargs where g3e_riarggroupno=10620101;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10620101,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10620101,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10620101,'MANHOLE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10620101 where g3e_tano=10620101;

-- Manhole	FNO: 106	DNO: 10630	Dialog Type: Placement	DTNO: 106301	Attribute: CU Code	TANO:10630101
delete from g3e_relationifaceargs where g3e_riarggroupno=10630101;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10630101,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10630101,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10630101,'MANHOLE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10630101 where g3e_tano=10630101;

-- Network Protector	FNO: 157	DNO: 15720	Dialog Type: Edit	DTNO: 157201	Attribute: CU Code	TANO:15720100
delete from g3e_relationifaceargs where g3e_riarggroupno=15720100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15720100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15720100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15720100,'NETWRKPROT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15720100 where g3e_tano=15720100;

-- Network Protector	FNO: 157	DNO: 15730	Dialog Type: Placement	DTNO: 157301	Attribute: CU Code	TANO:15730100
delete from g3e_relationifaceargs where g3e_riarggroupno=15730100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15730100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15730100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15730100,'NETWRKPROT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15730100 where g3e_tano=15730100;

-- Pad	FNO: 108	DNO: 10820	Dialog Type: Edit	DTNO: 108201	Attribute: CU Code	TANO:10820100
delete from g3e_relationifaceargs where g3e_riarggroupno=10820100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10820100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10820100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10820100,'PAD',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10820100 where g3e_tano=10820100;

-- Pad	FNO: 108	DNO: 10830	Dialog Type: Placement	DTNO: 108301	Attribute: CU Code	TANO:10830100
delete from g3e_relationifaceargs where g3e_riarggroupno=10830100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10830100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10830100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10830100,'PAD',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10830100 where g3e_tano=10830100;

-- Pole	FNO: 110	DNO: 11020	Dialog Type: Edit	DTNO: 110201	Attribute: CU Code	TANO:11020100
delete from g3e_relationifaceargs where g3e_riarggroupno=11020100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11020100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11020100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11020100,'MTUPOLE,MFOREIGNPOLE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11020100 where g3e_tano=11020100;

-- Pole	FNO: 110	DNO: 11030	Dialog Type: Placement	DTNO: 110301	Attribute: CU Code	TANO:11030100
delete from g3e_relationifaceargs where g3e_riarggroupno=11030100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11030100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11030100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11030100,'MTUPOLE,MFOREIGNPOLE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11030100 where g3e_tano=11030100;

-- Primary Conductor - OH	FNO: 8	DNO: 820	Dialog Type: Edit	DTNO: 8202	Attribute: CU Code	TANO:820210
delete from g3e_relationifaceargs where g3e_riarggroupno=820210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,820210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,820210,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,820210,'PRIOHCOND,PRIOHNEUT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=820210 where g3e_tano=820210;

-- Primary Conductor - OH	FNO: 8	DNO: 830	Dialog Type: Placement	DTNO: 8302	Attribute: CU Code	TANO:830210
delete from g3e_relationifaceargs where g3e_riarggroupno=830210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,830210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,830210,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,830210,'PRIOHCOND,PRIOHNEUT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=830210 where g3e_tano=830210;

-- Primary Conductor - OH Network	FNO: 84	DNO: 8201	Dialog Type: Edit	DTNO: 84202	Attribute: CU Code	TANO:8420210
delete from g3e_relationifaceargs where g3e_riarggroupno=8420210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,8420210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,8420210,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,8420210,'NPRIOHCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=8420210 where g3e_tano=8420210;

-- Primary Conductor - OH Network	FNO: 84	DNO: 8301	Dialog Type: Placement	DTNO: 84302	Attribute: CU Code	TANO:8430210
delete from g3e_relationifaceargs where g3e_riarggroupno=8430210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,8430210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,8430210,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,8430210,'NPRIOHCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=8430210 where g3e_tano=8430210;

-- Primary Conductor - UG	FNO: 9	DNO: 920	Dialog Type: Edit	DTNO: 9202	Attribute: CU Code	TANO:920210
delete from g3e_relationifaceargs where g3e_riarggroupno=920210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,920210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,920210,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,920210,'PRIUGCOND,PRIUGNEUT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=920210 where g3e_tano=920210;

-- Primary Conductor - UG	FNO: 9	DNO: 930	Dialog Type: Placement	DTNO: 9302	Attribute: CU Code	TANO:930210
delete from g3e_relationifaceargs where g3e_riarggroupno=930210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,930210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,930210,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,930210,'PRIUGCOND,PRIUGNEUT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=930210 where g3e_tano=930210;

-- Primary Conductor - UG Network	FNO: 85	DNO: 9201	Dialog Type: Edit	DTNO: 85202	Attribute: CU Code	TANO:8520210
delete from g3e_relationifaceargs where g3e_riarggroupno=8520210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,8520210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,8520210,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,8520210,'NPRIUGCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=8520210 where g3e_tano=8520210;

-- Primary Conductor - UG Network	FNO: 85	DNO: 9301	Dialog Type: Placement	DTNO: 85302	Attribute: CU Code	TANO:8530210
delete from g3e_relationifaceargs where g3e_riarggroupno=8530210;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,8530210,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,8530210,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,8530210,'NPRIUGCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=8530210 where g3e_tano=8530210;

-- Primary Enclosure	FNO: 5	DNO: 520	Dialog Type: Edit	DTNO: 5204	Attribute: CU Code	TANO:520400
delete from g3e_relationifaceargs where g3e_riarggroupno=520400;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,520400,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,520400,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,520400,'PRIFEEDTHRU',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=520400 where g3e_tano=520400;

-- Primary Enclosure	FNO: 5	DNO: 530	Dialog Type: Placement	DTNO: 5301	Attribute: CU Code	TANO:530100
delete from g3e_relationifaceargs where g3e_riarggroupno=530100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,530100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,530100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,530100,'PRIFEEDTHRU',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=530100 where g3e_tano=530100;

-- Primary Fuse - OH	FNO: 11	DNO: 1120	Dialog Type: Edit	DTNO: 110204	Attribute: CU Code	TANO:11020404
delete from g3e_relationifaceargs where g3e_riarggroupno=11020404;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11020404,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11020404,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11020404,'PRIFUSEOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11020404 where g3e_tano=11020404;

-- Primary Fuse - OH	FNO: 11	DNO: 1130	Dialog Type: Placement	DTNO: 110304	Attribute: CU Code	TANO:11030404
delete from g3e_relationifaceargs where g3e_riarggroupno=11030404;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11030404,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11030404,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11030404,'PRIFUSEOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11030404 where g3e_tano=11030404;

-- Primary Fuse - OH Network	FNO: 87	DNO: 8702	Dialog Type: Edit	DTNO: 870204	Attribute: CU Code	TANO:87020404	
delete from g3e_relationifaceargs where g3e_riarggroupno=87020404;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,87020404,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,87020404,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,87020404,'NPRIFUSEOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=87020404 where g3e_tano=87020404;

-- Primary Fuse - OH Network	FNO: 87	DNO: 8703	Dialog Type: Placement	DTNO: 870304	Attribute: CU Code	TANO:87030404	
delete from g3e_relationifaceargs where g3e_riarggroupno=87030404;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,87030404,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,87030404,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,87030404,'NPRIFUSEOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=87030404 where g3e_tano=87030404;

-- Primary Fuse - UG	FNO: 38	DNO: 3802	Dialog Type: Edit	DTNO: 11201	Attribute: CU Code	TANO:110204
delete from g3e_relationifaceargs where g3e_riarggroupno=110204;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,110204,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,110204,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,110204,'PRIFUSEUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=110204 where g3e_tano=110204;

-- Primary Fuse - UG	FNO: 38	DNO: 3803	Dialog Type: Placement	DTNO: 11301	Attribute: CU Code	TANO:110304
delete from g3e_relationifaceargs where g3e_riarggroupno=110304;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,110304,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,110304,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,110304,'PRIFUSEUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=110304 where g3e_tano=110304;

-- Primary Fuse - UG Network	FNO: 88	DNO: 8802	Dialog Type: Edit	DTNO: 88201	Attribute: CU Code	TANO:8820107	
delete from g3e_relationifaceargs where g3e_riarggroupno=8820107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,8820107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,8820107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,8820107,'NPRIFUSEUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=8820107 where g3e_tano=8820107;

-- Primary Fuse - UG Network	FNO: 88	DNO: 8803	Dialog Type: Placement	DTNO: 88301	Attribute: CU Code	TANO:8830107	
delete from g3e_relationifaceargs where g3e_riarggroupno=8830107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,8830107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,8830107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,8830107,'NPRIFUSEUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=8830107 where g3e_tano=8830107;

-- Primary Point of Delivery	FNO: 12	DNO: 1220	Dialog Type: Edit	DTNO: 12201	Attribute: CU Code	TANO:1220100
delete from g3e_relationifaceargs where g3e_riarggroupno=1220100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1220100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1220100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1220100,'PPOD',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1220100 where g3e_tano=1220100;

-- Primary Point of Delivery	FNO: 12	DNO: 1230	Dialog Type: Placement	DTNO: 12301	Attribute: CU Code	TANO:1230100
delete from g3e_relationifaceargs where g3e_riarggroupno=1230100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1230100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1230100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1230100,'PPOD',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1230100 where g3e_tano=1230100;

-- Primary Pull Box	FNO: 109	DNO: 10920	Dialog Type: Edit	DTNO: 109201	Attribute: CU Code	TANO:10920100
delete from g3e_relationifaceargs where g3e_riarggroupno=10920100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10920100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10920100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10920100,'PRIBOX',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10920100 where g3e_tano=10920100;

-- Primary Pull Box	FNO: 109	DNO: 10930	Dialog Type: Placement	DTNO: 109301	Attribute: CU Code	TANO:10930100
delete from g3e_relationifaceargs where g3e_riarggroupno=10930100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,10930100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,10930100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,10930100,'PRIBOX',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=10930100 where g3e_tano=10930100;

-- Primary Splice	FNO: 21	DNO: 21201	Dialog Type: Edit	DTNO: 21202	Attribute: CU Code	TANO:2120200
delete from g3e_relationifaceargs where g3e_riarggroupno=2120200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,2120200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,2120200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,2120200,'PRISPLICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=2120200 where g3e_tano=2120200;

-- Primary Splice	FNO: 21	DNO: 21301	Dialog Type: Placement	DTNO: 21302	Attribute: CU Code	TANO:2130200
delete from g3e_relationifaceargs where g3e_riarggroupno=2130200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,2130200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,2130200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,2130200,'PRISPLICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=2130200 where g3e_tano=2130200;

-- Primary Splice - Network	FNO: 22	DNO: 22201	Dialog Type: Edit	DTNO: 22202	Attribute: CU Code	TANO:2220200	
delete from g3e_relationifaceargs where g3e_riarggroupno=2220200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,2220200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,2220200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,2220200,'NPRISPLICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=2220200 where g3e_tano=2220200;

-- Primary Splice - Network	FNO: 22	DNO: 22301	Dialog Type: Placement	DTNO: 22302	Attribute: CU Code	TANO:2230200	
delete from g3e_relationifaceargs where g3e_riarggroupno=2230200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,2230200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,2230200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,2230200,'NPRISPLICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=2230200 where g3e_tano=2230200;

-- Primary Switch - OH	FNO: 13	DNO: 1320	Dialog Type: Edit	DTNO: 13202	Attribute: CU Code	TANO:1320200
delete from g3e_relationifaceargs where g3e_riarggroupno=1320200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1320200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1320200,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1320200,'PRISWITCHOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1320200 where g3e_tano=1320200;

-- Primary Switch - OH	FNO: 13	DNO: 1330	Dialog Type: Placement	DTNO: 13302	Attribute: CU Code	TANO:1330200
delete from g3e_relationifaceargs where g3e_riarggroupno=1330200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1330200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1330200,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1330200,'PRISWITCHOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1330200 where g3e_tano=1330200;

/* -- Primary Switch - OH and OH Network need to stop sharing the same TANO -- temporarily disabled selection for NPRISWITCHOH (Rich Adase, 30-AUG-2018)

-- Primary Switch - OH Network	FNO: 89	DNO: 3920	Dialog Type: Edit	DTNO: 13202	Attribute: CU Code	TANO:1320200	
delete from g3e_relationifaceargs where g3e_riarggroupno=1320200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1320200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1320200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1320200,'NPRISWITCHOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1320200 where g3e_tano=1320200;

-- Primary Switch - OH Network	FNO: 89	DNO: 3930	Dialog Type: Placement	DTNO: 13302	Attribute: CU Code	TANO:1330200	
delete from g3e_relationifaceargs where g3e_riarggroupno=1330200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1330200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1330200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1330200,'NPRISWITCHOH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1330200 where g3e_tano=1330200;

*/

-- Primary Switch - UG	FNO: 39	DNO: 3920	Dialog Type: Edit	DTNO: 13204	Attribute: CU Code	TANO:510030332
delete from g3e_relationifaceargs where g3e_riarggroupno=510030332;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030332,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030332,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030332,'PRISWITCHUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030332 where g3e_tano=510030332;

-- Primary Switch - UG	FNO: 39	DNO: 3930	Dialog Type: Placement	DTNO: 13304	Attribute: CU Code	TANO:510030346
delete from g3e_relationifaceargs where g3e_riarggroupno=510030346;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030346,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030346,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030346,'PRISWITCHUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030346 where g3e_tano=510030346;

/* -- Primary Switch - UG and UG Network need to stop sharing the same TANO -- temporarily disabled selection for NPRISWITCHUG (Rich Adase, 30-AUG-2018)

-- Primary Switch - UG Network	FNO: 90	DNO: 3920	Dialog Type: Edit	DTNO: 90202	Attribute: CU Code	TANO:9020201	
delete from g3e_relationifaceargs where g3e_riarggroupno=9020201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,9020201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,9020201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,9020201,'NPRISWITCHUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=9020201 where g3e_tano=9020201;

-- Primary Switch - UG Network	FNO: 90	DNO: 3930	Dialog Type: Placement	DTNO: 90302	Attribute: CU Code	TANO:9030201	
delete from g3e_relationifaceargs where g3e_riarggroupno=9030201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,9030201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,9030201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,9030201,'NPRISWITCHUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=9030201 where g3e_tano=9030201;

*/

-- Rack	FNO: 111	DNO: 11120	Dialog Type: Edit	DTNO: 111201	Attribute: CU Code	TANO:11120100
delete from g3e_relationifaceargs where g3e_riarggroupno=11120100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11120100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11120100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11120100,'EQUIPMENTRACK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11120100 where g3e_tano=11120100;

-- Rack	FNO: 111	DNO: 11130	Dialog Type: Placement	DTNO: 111301	Attribute: CU Code	TANO:11130100
delete from g3e_relationifaceargs where g3e_riarggroupno=11130100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11130100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11130100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11130100,'EQUIPMENTRACK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11130100 where g3e_tano=11130100;

-- Recloser - OH	FNO: 14	DNO: 1420	Dialog Type: Edit	DTNO: 14202	Attribute: CU Code	TANO:1420201
delete from g3e_relationifaceargs where g3e_riarggroupno=1420201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1420201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1420201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1420201,'MRECLOSEROH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1420201 where g3e_tano=1420201;

-- Recloser - OH	FNO: 14	DNO: 1430	Dialog Type: Placement	DTNO: 14302	Attribute: CU Code	TANO:1430201
delete from g3e_relationifaceargs where g3e_riarggroupno=1430201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1430201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1430201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1430201,'MRECLOSEROH',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1430201 where g3e_tano=1430201;

-- Recloser - UG	FNO: 15	DNO: 1520	Dialog Type: Edit	DTNO: 15202	Attribute: CU Code	TANO:1520202
delete from g3e_relationifaceargs where g3e_riarggroupno=1520202;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1520202,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1520202,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1520202,'MRECLOSERUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1520202 where g3e_tano=1520202;

-- Recloser - UG	FNO: 15	DNO: 1530	Dialog Type: Placement	DTNO: 15302	Attribute: CU Code	TANO:1530202
delete from g3e_relationifaceargs where g3e_riarggroupno=1530202;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,1530202,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,1530202,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,1530202,'MRECLOSERUG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=1530202 where g3e_tano=1530202;

-- Riser	FNO: 112	DNO: 11220	Dialog Type: Edit	DTNO: 112201	Attribute: CU Code	TANO:11220100
delete from g3e_relationifaceargs where g3e_riarggroupno=11220100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11220100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11220100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11220100,'PRIRISER,SECRISER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11220100 where g3e_tano=11220100;

-- Riser	FNO: 112	DNO: 11230	Dialog Type: Placement	DTNO: 112301	Attribute: CU Code	TANO:11230100
delete from g3e_relationifaceargs where g3e_riarggroupno=11230100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11230100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11230100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11230100,'PRIRISER,SECRISER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11230100 where g3e_tano=11230100;

-- Riser	FNO: 112	DNO: 11240	Dialog Type: Riser - Primary	DTNO: 112401	Attribute: CU Code	TANO:510030358
delete from g3e_relationifaceargs where g3e_riarggroupno=510030358;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030358,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030358,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030358,'PRIRISER,SECRISER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030358 where g3e_tano=510030358;

-- Riser	FNO: 112	DNO: 11250	Dialog Type: Riser - Secondary	DTNO: 112501	Attribute: CU Code	TANO:510030359
delete from g3e_relationifaceargs where g3e_riarggroupno=510030359;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,510030359,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,510030359,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,510030359,'PRIRISER,SECRISER',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=510030359 where g3e_tano=510030359;

-- Secondary Box	FNO: 113	DNO: 11320	Dialog Type: Edit	DTNO: 113201	Attribute: CU Code	TANO:11320100
delete from g3e_relationifaceargs where g3e_riarggroupno=11320100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11320100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11320100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11320100,'SECBOX',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11320100 where g3e_tano=11320100;

-- Secondary Box	FNO: 113	DNO: 11330	Dialog Type: Placement	DTNO: 113301	Attribute: CU Code	TANO:11330100
delete from g3e_relationifaceargs where g3e_riarggroupno=11330100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11330100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11330100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11330100,'SECBOX',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11330100 where g3e_tano=11330100;

-- Secondary Breaker - Network	FNO: 94	DNO: 9420	Dialog Type: Edit	DTNO: 154201	Attribute: CU Code	TANO:15420107	
delete from g3e_relationifaceargs where g3e_riarggroupno=15420107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15420107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15420107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15420107,'NSECBREAK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15420107 where g3e_tano=15420107;

-- Secondary Breaker - Network	FNO: 94	DNO: 9430	Dialog Type: Placement	DTNO: 154301	Attribute: CU Code	TANO:15410107	
delete from g3e_relationifaceargs where g3e_riarggroupno=15410107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15410107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15410107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15410107,'NSECBREAK',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15410107 where g3e_tano=15410107;

-- Secondary Conductor - OH	FNO: 53	DNO: 5320	Dialog Type: Edit	DTNO: 53202	Attribute: CU Code	TANO:5320200
delete from g3e_relationifaceargs where g3e_riarggroupno=5320200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5320200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5320200,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5320200,'SECOHCOND,SLOHCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5320200 where g3e_tano=5320200;

-- Secondary Conductor - OH	FNO: 53	DNO: 5330	Dialog Type: Placement	DTNO: 53302	Attribute: CU Code	TANO:5330200
delete from g3e_relationifaceargs where g3e_riarggroupno=5330200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5330200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5330200,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5330200,'SECOHCOND,SLOHCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5330200 where g3e_tano=5330200;

-- Secondary Conductor - OH Network	FNO: 96	DNO: 9620	Dialog Type: Edit	DTNO: 96202	Attribute: CU Code	TANO:9620200
delete from g3e_relationifaceargs where g3e_riarggroupno=9620200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,9620200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,9620200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,9620200,'NSECOHCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=9620200 where g3e_tano=9620200;

-- Secondary Conductor - OH Network	FNO: 96	DNO: 9630	Dialog Type: Placement	DTNO: 96302	Attribute: CU Code	TANO:9630200
delete from g3e_relationifaceargs where g3e_riarggroupno=9630200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,9630200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,9630200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,9630200,'NSECOHCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=9630200 where g3e_tano=9630200;

-- Secondary Conductor - UG	FNO: 63	DNO: 6320	Dialog Type: Edit	DTNO: 63202	Attribute: CU Code	TANO:6320200	
delete from g3e_relationifaceargs where g3e_riarggroupno=6320200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6320200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6320200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6320200,'SECUGCOND,SLUGCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6320200 where g3e_tano=6320200;

-- Secondary Conductor - UG	FNO: 63	DNO: 6330	Dialog Type: Placement	DTNO: 63302	Attribute: CU Code	TANO:6330200	
delete from g3e_relationifaceargs where g3e_riarggroupno=6330200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6330200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6330200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6330200,'SECUGCOND,SLUGCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6330200 where g3e_tano=6330200;

-- Secondary Conductor - UG Network	FNO: 97	DNO: 97201	Dialog Type: Edit	DTNO: 97202	Attribute: CU Code	TANO:9720200
delete from g3e_relationifaceargs where g3e_riarggroupno=9720200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,9720200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,9720200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,9720200,'NSECUGCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=9720200 where g3e_tano=9720200;

-- Secondary Conductor - UG Network	FNO: 97	DNO: 97301	Dialog Type: Placement	DTNO: 97302	Attribute: CU Code	TANO:9730200
delete from g3e_relationifaceargs where g3e_riarggroupno=9730200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,9730200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,9730200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,9730200,'NSECUGCOND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=9730200 where g3e_tano=9730200;

-- Secondary Enclosure	FNO: 120	DNO: 12020	Dialog Type: Edit	DTNO: 120201	Attribute: CU Code	TANO:12020100
delete from g3e_relationifaceargs where g3e_riarggroupno=12020100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,12020100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,12020100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,12020100,'SECCONNENCL',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=12020100 where g3e_tano=12020100;

-- Secondary Enclosure	FNO: 120	DNO: 12030	Dialog Type: Placement	DTNO: 120301	Attribute: CU Code	TANO:12030100
delete from g3e_relationifaceargs where g3e_riarggroupno=12030100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,12030100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,12030100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,12030100,'SECCONNENCL',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=12030100 where g3e_tano=12030100;

-- Secondary Fuse - Network	FNO: 86	DNO: 8620	Dialog Type: Edit	DTNO: 161201	Attribute: CU Code	TANO:16120104	
delete from g3e_relationifaceargs where g3e_riarggroupno=16120104;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,16120104,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,16120104,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,16120104,'NSECFUSE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=16120104 where g3e_tano=16120104;

-- Secondary Fuse - Network	FNO: 86	DNO: 8630	Dialog Type: Placement	DTNO: 161301	Attribute: CU Code	TANO:16130104	
delete from g3e_relationifaceargs where g3e_riarggroupno=16130104;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,16130104,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,16130104,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,16130104,'NSECFUSE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=16130104 where g3e_tano=16130104;

-- Secondary Splice - Network	FNO: 23	DNO: 23201	Dialog Type: Edit	DTNO: 23202	Attribute: CU Code	TANO:2320200	
delete from g3e_relationifaceargs where g3e_riarggroupno=2320200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,2320200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,2320200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,2320200,'NSECSPLICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=2320200 where g3e_tano=2320200;

-- Secondary Splice - Network	FNO: 23	DNO: 23301	Dialog Type: Placement	DTNO: 23302	Attribute: CU Code	TANO:2330200	
delete from g3e_relationifaceargs where g3e_riarggroupno=2330200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,2330200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,2330200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,2330200,'NSECSPLICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=2330200 where g3e_tano=2330200;

-- Secondary Switch Gear	FNO: 153	DNO: 15320	Dialog Type: Edit	DTNO: 153201	Attribute: CU Code	TANO:15320100
delete from g3e_relationifaceargs where g3e_riarggroupno=15320100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15320100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15320100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15320100,'SPNWSWGEAR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15320100 where g3e_tano=15320100;

-- Secondary Switch Gear	FNO: 153	DNO: 15330	Dialog Type: Placement	DTNO: 153301	Attribute: CU Code	TANO:15330100
delete from g3e_relationifaceargs where g3e_riarggroupno=15330100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,15330100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,15330100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,15330100,'SPNWSWGEAR',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=15330100 where g3e_tano=15330100;

-- Service Line	FNO: 54	DNO: 5420	Dialog Type: Edit	DTNO: 54201	Attribute: CU Code	TANO:5420100
delete from g3e_relationifaceargs where g3e_riarggroupno=5420100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5420100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5420100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5420100,'OHSERVICE,UGSERVICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5420100 where g3e_tano=5420100;

-- Service Line	FNO: 54	DNO: 5430	Dialog Type: Placement	DTNO: 54301	Attribute: CU Code	TANO:5430100
delete from g3e_relationifaceargs where g3e_riarggroupno=5430100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5430100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5430100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5430100,'OHSERVICE,UGSERVICE',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5430100 where g3e_tano=5430100;

-- Street Light	FNO: 56	DNO: 5620	Dialog Type: Edit	DTNO: 56201	Attribute: CU Code	TANO:5620107
delete from g3e_relationifaceargs where g3e_riarggroupno=5620107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5620107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5620107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5620107,'STREETLIGHT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5620107 where g3e_tano=5620107;

-- Street Light	FNO: 56	DNO: 5630	Dialog Type: Placement	DTNO: 56301	Attribute: CU Code	TANO:5630107
delete from g3e_relationifaceargs where g3e_riarggroupno=5630107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5630107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5630107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5630107,'STREETLIGHT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5630107 where g3e_tano=5630107;

-- Street Light	FNO: 56	DNO: 5640	Dialog Type: Street Light + Standard	DTNO: 56301	Attribute: CU Code	TANO:5630107
delete from g3e_relationifaceargs where g3e_riarggroupno=5630107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5630107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5630107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5630107,'STREETLIGHT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5630107 where g3e_tano=5630107;

-- Street Light	FNO: 56	DNO: 5660	Dialog Type: Street Light + Misc Structure	DTNO: 56301	Attribute: CU Code	TANO:5630107
delete from g3e_relationifaceargs where g3e_riarggroupno=5630107;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5630107,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5630107,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5630107,'STREETLIGHT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5630107 where g3e_tano=5630107;

-- Street Light Control	FNO: 58	DNO: 5820	Dialog Type: Edit	DTNO: 58201	Attribute: CU Code	TANO:5820100
delete from g3e_relationifaceargs where g3e_riarggroupno=5820100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5820100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5820100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5820100,'SLCONTROL',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5820100 where g3e_tano=5820100;

-- Street Light Control	FNO: 58	DNO: 5830	Dialog Type: Placement	DTNO: 58301	Attribute: CU Code	TANO:5830100
delete from g3e_relationifaceargs where g3e_riarggroupno=5830100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5830100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5830100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5830100,'SLCONTROL',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5830100 where g3e_tano=5830100;

-- Street Light Standard	FNO: 114	DNO: 11420	Dialog Type: Edit	DTNO: 114201	Attribute: CU Code	TANO:11420100
delete from g3e_relationifaceargs where g3e_riarggroupno=11420100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11420100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11420100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11420100,'SLSTAND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11420100 where g3e_tano=11420100;

-- Street Light Standard	FNO: 114	DNO: 11430	Dialog Type: Placement	DTNO: 114301	Attribute: CU Code	TANO:11430100
delete from g3e_relationifaceargs where g3e_riarggroupno=11430100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11430100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11430100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11430100,'SLSTAND',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11430100 where g3e_tano=11430100;

-- Transformer - OH	FNO: 59	DNO: 5920	Dialog Type: Edit	DTNO: 59202	Attribute: CU Code	TANO:5920201
delete from g3e_relationifaceargs where g3e_riarggroupno=5920201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5920201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5920201,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5920201,'MOHTRF,NOHTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5920201 where g3e_tano=5920201;

-- Transformer - OH	FNO: 59	DNO: 5930	Dialog Type: Placement	DTNO: 59302	Attribute: CU Code	TANO:5930200
delete from g3e_relationifaceargs where g3e_riarggroupno=5930200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5930200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5930200,'Y',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5930200,'MOHTRF,NOHTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5930200 where g3e_tano=5930200;

/* -- Transformer - OH and OH Network need to stop sharing the same TANO -- temporarily added NOHTRF category to the shared list (Rich Adase, 03-AUG-2018)

-- Transformer - OH Network	FNO: 98	DNO: 98201	Dialog Type: Edit	DTNO: 59202	Attribute: CU Code	TANO:5920201	
delete from g3e_relationifaceargs where g3e_riarggroupno=5920201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5920201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5920201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5920201,'NOHTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5920201 where g3e_tano=5920201;

-- Transformer - OH Network	FNO: 98	DNO: 98301	Dialog Type: Placement	DTNO: 59302	Attribute: CU Code	TANO:5930200	
delete from g3e_relationifaceargs where g3e_riarggroupno=5930200;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,5930200,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,5930200,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,5930200,'NOHTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=5930200 where g3e_tano=5930200;

*/

-- Transformer - UG	FNO: 60	DNO: 6020	Dialog Type: Edit	DTNO: 60202	Attribute: CU Code	TANO:6020201
delete from g3e_relationifaceargs where g3e_riarggroupno=6020201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6020201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6020201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6020201,'MUGTRF,NUGTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6020201 where g3e_tano=6020201;

-- Transformer - UG	FNO: 60	DNO: 6030	Dialog Type: Placement	DTNO: 60302	Attribute: CU Code	TANO:6030201
delete from g3e_relationifaceargs where g3e_riarggroupno=6030201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6030201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6030201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6030201,'MUGTRF,NUGTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6030201 where g3e_tano=6030201;

/* -- Transformer - UG and UG Network need to stop sharing the same TANO -- temporarily added NUGTRF category to the shared list (Rich Adase, 03-AUG-2018)

-- Transformer - UG Network	FNO: 99	DNO: 99201	Dialog Type: Edit	DTNO: 60202	Attribute: CU Code	TANO:6020201	
delete from g3e_relationifaceargs where g3e_riarggroupno=6020201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6020201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6020201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6020201,'NUGTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6020201 where g3e_tano=6020201;

-- Transformer - UG Network	FNO: 99	DNO: 99301	Dialog Type: Placement	DTNO: 60302	Attribute: CU Code	TANO:6030201	
delete from g3e_relationifaceargs where g3e_riarggroupno=6030201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,6030201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,6030201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,6030201,'NUGTRF',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=6030201 where g3e_tano=6030201;

*/

-- Vault	FNO: 117	DNO: 11720	Dialog Type: Edit	DTNO: 117201	Attribute: CU Code	TANO:11720101
delete from g3e_relationifaceargs where g3e_riarggroupno=11720101;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11720101,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11720101,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11720101,'VAULT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11720101 where g3e_tano=11720101;

-- Vault	FNO: 117	DNO: 11720	Dialog Type: Edit	DTNO: 118201	Attribute: CU Code	TANO:11820100
delete from g3e_relationifaceargs where g3e_riarggroupno=11820100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11820100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11820100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11820100,'VAULT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11820100 where g3e_tano=11820100;

-- Vault	FNO: 117	DNO: 11720	Dialog Type: Placement	DTNO: 118301	Attribute: CU Code	TANO:11830100
delete from g3e_relationifaceargs where g3e_riarggroupno=11830100;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11830100,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11830100,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11830100,'VAULT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11830100 where g3e_tano=11830100;

-- Vault	FNO: 117	DNO: 11730	Dialog Type: Placement	DTNO: 117301	Attribute: CU Code	TANO:11730101
delete from g3e_relationifaceargs where g3e_riarggroupno=11730101;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,11730101,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,11730101,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,11730101,'VAULT',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=11730101 where g3e_tano=11730101;

-- Voltage Regulator	FNO: 36	DNO: 3620	Dialog Type: Edit	DTNO: 36202	Attribute: CU Code	TANO:3620201
delete from g3e_relationifaceargs where g3e_riarggroupno=3620201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,3620201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,3620201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,3620201,'MVOLTREG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=3620201 where g3e_tano=3620201;

-- Voltage Regulator	FNO: 36	DNO: 3630	Dialog Type: Placement	DTNO: 36302	Attribute: CU Code	TANO:3630201
delete from g3e_relationifaceargs where g3e_riarggroupno=3630201;
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,1,3630201,'PRIMARY',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,2,3630201,'N',sysdate);
insert into g3e_relationifaceargs colums(g3e_argrowno,g3e_argumentordinal,g3e_riarggroupno,g3e_value,g3e_editdate) values(g3e_relationifaceargs_seq.nextval,3,3630201,'MVOLTREG',sysdate);
update g3e_tabattribute set g3e_fkqrino=(select g3e_rino from g3e_relationinterface where g3e_username='CU Selection FKQ'),g3e_fkqarggroupno=3630201 where g3e_tano=3630201;

commit;

spool off;
