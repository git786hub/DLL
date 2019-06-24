set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_VirtualPoints.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_VirtualPoints.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Virtual Points
--**************************************************************************************
-- Modified:
--  17-JUL-2018, Rich Adase -- SME symbology review
--**************************************************************************************

alter trigger M_T_AUDR_G3E_STYLERULE_RULE disable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE disable;

declare
  TYPE numArray IS TABLE OF NUMBER INDEX BY PLS_INTEGER;
  arrSNO  numArray;
  i       PLS_INTEGER;
begin
  select distinct G3E_SNO
    bulk collect into arrSNO
    from G3E_STYLERULE 
    where G3E_RULE in ('Bypass Point Symbol'   , 'Bypass Point Symbol - OMS'
                      ,'Elbow Symbol'          , 'Elbow Symbol - OMS'
                      ,'Isolation Point Symbol', 'Isolation Point Symbol - OMS'
                      ,'Phase Connector Symbol', 'Phase Connector Symbol - OMS'
                      ,'Bypass Point Detail Symbol'
                      ,'Elbow Detail Symbol'
                      ,'Isolation Symbol'
                      ,'Isolation Detail Symbol'
                      ,'Isolation Point Detail Symbol'
                      ,'Phase Connector Detail Symbol');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Bypass Point Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Bypass Point Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Elbow Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Elbow Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Isolation Point Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Isolation Point Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Phase Connector Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Phase Connector Symbol - OMS';

  delete from G3E_STYLERULE where G3E_RULE = 'Bypass Point Detail Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Elbow Detail Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Isolation Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Isolation Detail Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Isolation Point Detail Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Phase Connector Detail Symbol'; -- obsolete
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;

  delete from G3E_POINTSTYLE where G3E_USERNAME like 'Bypass%'; -- obsolete
  delete from G3E_POINTSTYLE where G3E_USERNAME like 'Elbow%'; -- obsolete
  delete from G3E_POINTSTYLE where G3E_USERNAME like 'Isolation%'; -- obsolete
  delete from G3E_POINTSTYLE where G3E_USERNAME like 'Phase%'; -- obsolete

end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (40108,'Bypass Symbol - NO','AEGIS Misc',CHR(41),65280,3,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (40109,'Bypass Symbol - NC','AEGIS Misc',CHR(40),255,3,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (41201,'Elbow Symbol - NO','AEGIS Misc',CHR(41),65280,3,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (41202,'Elbow Symbol - NC','AEGIS Misc',CHR(40),255,3,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (6101,'Isolation Point Symbol - NO','AEGIS Misc',CHR(41),65280,3,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (6102,'Isolation Point Symbol - NC','AEGIS Misc',CHR(40),255,3,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (42101,'Phase Connector Symbol - NO','AEGIS Misc',CHR(41),65280,3,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (42102,'Phase Connector Symbol - NC','AEGIS Misc',CHR(40),255,3,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4010101,40101,'Bypass Point Symbol','STATUS_NORMAL_C=''OPEN''',1,40108,'Bypass Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4010199,40101,'Bypass Point Symbol','',99,40109,'Bypass Symbol - NC');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4110101,41101,'Elbow Symbol','STATUS_NORMAL_C=''OPEN''',1,41201,'Elbow Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4110199,41101,'Elbow Symbol','',99,41202,'Elbow Symbol - NC');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (610101,6101,'Isolation Point Symbol','STATUS_NORMAL_C=''OPEN''',1,6101,'Isolation Point Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (610199,6101,'Isolation Point Symbol','',99,6102,'Isolation Point Symbol - NC');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4210101,42101,'Phase Connector Symbol','STATUS_NORMAL_C=''OPEN''',1,42101,'Phase Connector Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4210102,42101,'Phase Connector Symbol','',2,42102,'Phase Connector Symbol - NC');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4020101,40201,'Bypass Point Symbol - OMS','STATUS_NORMAL_C=''OPEN''',1,40108,'Bypass Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4020199,40201,'Bypass Point Symbol - OMS','',99,40109,'Bypass Symbol - NC');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4120101,41201,'Elbow Symbol - OMS','STATUS_NORMAL_C=''OPEN''',1,41201,'Elbow Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4120199,41201,'Elbow Symbol - OMS','',99,41202,'Elbow Symbol - NC');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (620101,6201,'Isolation Point Symbol - OMS','STATUS_NORMAL_C=''OPEN''',1,6101,'Isolation Point Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (620199,6201,'Isolation Point Symbol - OMS','',99,6102,'Isolation Point Symbol - NC');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4220101,42201,'Phase Connector Symbol - OMS','STATUS_NORMAL_C=''OPEN''',1,42101,'Phase Connector Symbol - NO');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (4220102,42201,'Phase Connector Symbol - OMS','',2,42102,'Phase Connector Symbol - NC');


spool off;
