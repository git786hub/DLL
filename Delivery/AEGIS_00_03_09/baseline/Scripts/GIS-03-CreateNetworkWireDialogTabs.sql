set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\CreateNetworkWireDialogTabs.sql.log
--**************************************************************************************
--SCRIPT NAME: JobMgmt_CUFKQArguments.sql
--**************************************************************************************
-- AUTHOR			: Barry Scott
-- DATE				: 06-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Create tabs for network conductors
--                : Sets the FKQ fields to null since those are created/set
--                : with the master script for CU Selection
--**************************************************************************************
-- Modified:
--**************************************************************************************

/*
FNO Username
--- -------------------------------
 8	 Primary Conductor - OH
84	Primary Conductor - OH Network

 9  Primary Conductor - UG
85	Primary Conductor - UG Network

53	Secondary Conductor - OH
96	Secondary Conductor - OH Network

63	Secondary Conductor - UG
97	Secondary Conductor - UG Network
*/

---------------------------------------------------------------------------------------------------------
-- This section ensures the script can be rerun

-- Primary Conductor - OH Network
update g3e_dialog set g3e_dtno=8102 where g3e_dno=8101 and g3e_dtno=84102; --Review
update g3e_dialog set g3e_dtno=8202 where g3e_dno=8201 and g3e_dtno=84202; --Edit
update g3e_dialog set g3e_dtno=8302 where g3e_dno=8301 and g3e_dtno=84302; --Placement

-- Primary Conductor - UG Network
update g3e_dialog set g3e_dtno=9102 where g3e_dno=9101 and g3e_dtno=85102; --Review
update g3e_dialog set g3e_dtno=9202 where g3e_dno=9201 and g3e_dtno=85202; --Edit
update g3e_dialog set g3e_dtno=9302 where g3e_dno=9301 and g3e_dtno=85302; --Placement

-- Secondary Conductor - OH Network
update g3e_dialog set g3e_dtno=53102 where g3e_dno=9610 and g3e_dtno=96102; --Review
update g3e_dialog set g3e_dtno=53202 where g3e_dno=9620 and g3e_dtno=96202; --Edit
update g3e_dialog set g3e_dtno=53302 where g3e_dno=9630 and g3e_dtno=96302; --Placement

-- Secondary Conductor - UG Network
update g3e_dialog set g3e_dtno=63102 where g3e_dno=97101 and g3e_dtno=97102; --Review
update g3e_dialog set g3e_dtno=63202 where g3e_dno=97201 and g3e_dtno=97202; --Edit
update g3e_dialog set g3e_dtno=63302 where g3e_dno=97301 and g3e_dtno=97302; --Placement

delete from g3e_tabattribute where g3e_dtno in(84102,84202,84302,85102,85202,85302,96102,96202,96302,97102,97202,97302);
delete from g3e_dialogtab where g3e_dtno in(84102,84202,84302,85102,85202,85302,96102,96202,96302,97102,97202,97302);

-- End of "revert" section
---------------------------------------------------------------------------------------------------------

-- These statements adn stay the same and not reverted if rerunning this script.
update g3e_dialogtab set g3e_username='Primary Conductor - OH Network Attributes' where g3e_dtno in(8104,8204,8304) and g3e_username!='Primary Conductor - OH Network Attributes';
update g3e_dialogtab set g3e_username='Primary Conductor - UG Network Attributes' where g3e_dtno in(85101,85201,85301)and g3e_username!='Primary Conductor - UG Network Attributes';
update g3e_dialogtab set g3e_username='Secondary Conductor - OH Network Attributes' where g3e_dtno in(53101,53201,53301)and g3e_username!='Secondary Conductor - OH Network Attributes';
update g3e_dialogtab set g3e_username='Secondary Conductor - UG Network Attributes' where g3e_dtno in(63101,63201,63301) and g3e_username!='Secondary Conductor - UG Network Attributes';


-- Primary Wire OH (Review)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	84102, -- g3e_dtno(not null)
	'Primary Wire - OH Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+7600000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+76000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=8102 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8410210,2102,10,null,1,null,0,84102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8410200,80202,20,319,1,0,0,84102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8410201,80203,30,320,1,0,0,84102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8410203,80205,40,null,1,null,0,84102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8410202,80201,90,30027,1,0,0,84102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8410211,8021000,100,269,1,null,0,84102,null,null,sysdate,0,0,null,null);



-- Primary Wire OH (Edit)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	84202, -- g3e_dtno(not null)
	'Primary Wire - OH Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+7600000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+76000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=8202 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8420210,2102,10,null,0,null,0,84202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8420200,80202,20,319,0,null,0,84202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8420201,80203,30,320,0,null,0,84202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8420203,80205,40,null,1,null,0,84202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8420202,80201,90,30027,0,null,0,84202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8420211,8021000,100,269,0,null,0,84202,null,null,sysdate,0,0,null,null);


-- Primary Wire OH (Placement)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	84302, -- g3e_dtno(not null)
	'Primary Wire - OH Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+7600000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+76000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=8302 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8430210,2102,10,null,0,null,0,84302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8430200,80202,20,319,0,null,0,84302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8430201,80203,30,320,0,null,0,84302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8430203,80205,40,null,1,null,0,84302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8430202,80201,90,30027,0,null,0,84302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8430211,8021000,100,269,0,null,0,84302,null,null,sysdate,0,0,null,null);



-- Primary Wire UG (Review)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	85102, -- g3e_dtno(not null)
	'Primary Wire - UG Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+7600000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+76000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=9102 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8510210,2102,10,null,1,null,0,85102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8510200,90201,20,246,1,null,0,85102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8510201,90202,30,247,1,null,0,85102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8510202,90203,40,30027,1,0,0,85102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8510203,90204,50,319,1,0,0,85102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8510204,90205,60,320,1,0,0,85102,null,null,sysdate,0,0,null,null);



-- Primary Wire UG (Edit)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	85202, -- g3e_dtno(not null)
	'Primary Wire - UG Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+7600000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+76000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=9202 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8520210,2102,10,null,0,null,0,85202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8520200,90201,20,246,0,null,0,85202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8520201,90202,30,247,0,null,0,85202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8520202,90203,40,30027,0,null,0,85202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8520203,90204,50,319,0,null,0,85202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8520204,90205,60,320,0,null,0,85202,null,null,sysdate,0,0,null,null);


-- Primary Wire UG (Placement)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	85302, -- g3e_dtno(not null)
	'Primary Wire - UG Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+7600000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+76000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=9302 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8530210,2102,10,null,0,null,0,85302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8530200,90201,20,246,0,null,0,85302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8530201,90202,30,247,0,null,0,85302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8530202,90203,40,30027,0,null,0,85302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8530203,90204,50,319,0,null,0,85302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(8530204,90205,60,320,0,null,0,85302,null,null,sysdate,0,0,null,null);


-- Secondary Wire OH (Review)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	96102, -- g3e_dtno(not null)
	'Secondary Wire - OH Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+4300000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+43000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=53102 order by g3e_ordinal asc;
*/
insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9610200,2102,10,null,1,null,0,96102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9610201,530201,20,319,1,0,0,96102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9610202,530202,30,318,1,0,0,96102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620113,530205,40,null,1,null,0,96102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620114,530206,40,null,1,null,0,96102,null,null,sysdate,0,0,null,null);


-- Secondary Wire OH (Edit)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	96202, -- g3e_dtno(not null)
	'Secondary Wire - OH Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+4300000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+43000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=53202 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620200,2102,10,null,0,null,0,96202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620201,530201,20,319,0,null,0,96202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620202,530202,30,318,0,null,0,96202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620213,530205,40,null,0,null,0,96202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620214,530206,40,null,0,null,0,96202,null,null,sysdate,0,0,null,null);


-- Secondary Wire OH (Placement)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	96302, -- g3e_dtno(not null)
	'Secondary Wire - OH Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+4300000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+43000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=53302 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9630200,2102,10,null,0,null,0,96302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9630201,530201,20,319,0,null,0,96302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9630202,530202,30,318,0,null,0,96302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620313,530205,40,null,0,null,0,96302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9620314,530206,40,null,0,null,0,96302,null,null,sysdate,0,0,null,null);


-- Secondary Wire UG (Review)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	97102, -- g3e_dtno(not null)
	'Secondary Wire - UG Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);


/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+3400000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+34000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=63102 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9710200,2102,10,null,1,null,0,97102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9710201,530201,20,319,1,0,0,97102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9710202,530202,30,318,1,0,0,97102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720113,530205,40,null,1,null,0,97102,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720114,530206,50,null,1,null,0,97102,null,null,sysdate,0,0,null,null);



-- Secondary Wire UG (Edit)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	97202, -- g3e_dtno(not null)
	'Secondary Wire - UG Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+3400000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+34000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=63202 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720200,2102,10,null,0,null,0,97202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720201,530201,20,319,0,null,0,97202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720202,530202,30,318,0,null,0,97202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720213,530205,40,null,0,null,0,97202,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720214,530206,50,null,0,null,0,97202,null,null,sysdate,0,0,null,null);

-- Secondary Wire UG (Placement)
insert into g3e_dialogtab
columns(g3e_dtno,g3e_username,g3e_orientation,g3e_editdate,g3e_widthintwips,g3e_supportinterface,g3e_localecomment,g3e_orderbykey)
values(
	97302, -- g3e_dtno(not null)
	'Secondary Wire - UG Network Attributes', -- g3e_username(not null)
	'H', -- g3e_orientation
	sysdate, -- g3e_editdate(not null)
	null, -- g3e_widthintwips
	null, -- g3e_supportinterface
	null, -- g3e_localecomment
	null -- g3e_orderbykey
);

/*
select 'insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values('||to_number(g3e_tano+3400000)||','||g3e_ano||','||g3e_ordinal||','||nvl(to_char(g3e_pno),'null')||','||g3e_readonly||','||nvl(to_char(g3e_default),'null')||','||g3e_refresh||','||to_number(g3e_dtno+34000)||','||nvl(g3e_fieldvaluefilter,'null')||','||nvl(to_char(g3e_traversalano),'null')||',sysdate,'||g3e_triggerpopulate||','||g3e_usefiltertotranslatekey||',null,null);'||chr(10)
from g3e_tabattribute where g3e_dtno=63302 order by g3e_ordinal asc;
*/

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9730200,2102,10,null,0,null,0,97302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9730201,530201,20,319,0,null,0,97302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9730202,530202,30,318,0,null,0,97302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720313,530205,40,null,0,null,0,97302,null,null,sysdate,0,0,null,null);

insert into g3e_tabattribute
columns(g3e_tano,g3e_ano,g3e_ordinal,g3e_pno,g3e_readonly,g3e_default,g3e_refresh,g3e_dtno,g3e_fieldvaluefilter,g3e_traversalano,g3e_editdate,g3e_triggerpopulate,g3e_usefiltertotranslatekey,g3e_fkqrino,g3e_fkqarggroupno)
values(9720314,530206,50,null,0,null,0,97302,null,null,sysdate,0,0,null,null);


-- Switch the non-network tabs for network tabs in the pertinent dialogs

-- Primary Conductor - OH Network
update g3e_dialog set g3e_dtno=84102 where g3e_dno=8101 and g3e_dtno=8102; --Review
update g3e_dialog set g3e_dtno=84202 where g3e_dno=8201 and g3e_dtno=8202; --Edit
update g3e_dialog set g3e_dtno=84302 where g3e_dno=8301 and g3e_dtno=8302; --Placement

-- Primary Conductor - UG Network
update g3e_dialog set g3e_dtno=85102 where g3e_dno=9101 and g3e_dtno=9102; --Review
update g3e_dialog set g3e_dtno=85202 where g3e_dno=9201 and g3e_dtno=9202; --Edit
update g3e_dialog set g3e_dtno=85302 where g3e_dno=9301 and g3e_dtno=9302; --Placement

-- Secondary Conductor - OH Network
update g3e_dialog set g3e_dtno=96102 where g3e_dno=9610 and g3e_dtno=53102; --Review
update g3e_dialog set g3e_dtno=96202 where g3e_dno=9620 and g3e_dtno=53202; --Edit
update g3e_dialog set g3e_dtno=96302 where g3e_dno=9630 and g3e_dtno=53302; --Placement

-- Secondary Conductor - UG Network
update g3e_dialog set g3e_dtno=97102 where g3e_dno=97101 and g3e_dtno=63102; --Review
update g3e_dialog set g3e_dtno=97202 where g3e_dno=97201 and g3e_dtno=63202; --Edit
update g3e_dialog set g3e_dtno=97302 where g3e_dno=97301 and g3e_dtno=63302; --Placement


commit;
spool off;