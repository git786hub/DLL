set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_DOCNote.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_DOCNote.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for DOC Note
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
    where G3E_RULE in ('DOC Note Symbol', 'DOC Note Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'DOC Note Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'DOC Note Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219101,'DOC Note Type COMMS','AEGIS Misc',CHR(48),14548736,1200,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219102,'DOC Note Type MILITARY ','AEGIS Misc',CHR(49),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219103,'DOC Note Type AIRPORT ','AEGIS Misc',CHR(50),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219104,'DOC Note Type WTR/SWR ','AEGIS Misc',CHR(51),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219105,'DOC Note Type SCH/UNV ','AEGIS Misc',CHR(52),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219106,'DOC Note Type POLICE ','AEGIS Misc',CHR(53),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219107,'DOC Note Type FIRE ','AEGIS Misc',CHR(54),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219108,'DOC Note Type CITY HALL ','AEGIS Misc',CHR(55),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219109,'DOC Note Type NURSING ','AEGIS Misc',CHR(56),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219110,'DOC Note Type HOSPITAL ','AEGIS Misc',CHR(57),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219111,'DOC Note Type 8 PSB ','AEGIS Misc',CHR(58),39679,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219112,'DOC Note Type 24 PSB ','AEGIS Misc',CHR(59),39679,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219113,'DOC Note Type NGS ','AEGIS Misc',CHR(60),14548736,120,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (219199,'DOC Note Symbol Default','AEGIS Misc',CHR(61),14548736,120,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910101,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''COMMS''',1,219101,'DOC Note Type COMMS');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910102,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''MILITARY''',2,219102,'DOC Note Type MILITARY ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910103,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''AIRPORT''',3,219103,'DOC Note Type AIRPORT ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910104,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''WTR/SWR''',4,219104,'DOC Note Type WTR/SWR ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910105,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''SCH/UNV''',5,219105,'DOC Note Type SCH/UNV ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910106,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''POLICE''',6,219106,'DOC Note Type POLICE ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910107,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''FIRE''',7,219107,'DOC Note Type FIRE ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910108,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''CITY HALL''',8,219108,'DOC Note Type CITY HALL ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910109,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''NURSING''',9,219109,'DOC Note Type NURSING ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910110,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''HOSPITAL''',10,219110,'DOC Note Type HOSPITAL ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910111,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''8 PSB''',11,219111,'DOC Note Type 8 PSB ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910112,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''24 PSB''',12,219112,'DOC Note Type 24 PSB ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910113,219101,'DOC Note Symbol','DOC_NOTE_TYPE=''NGS''',13,219113,'DOC Note Type NGS ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21910199,219101,'DOC Note Symbol','',99,219199,'DOC Note Symbol Default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920101,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''COMMS''',1,219101,'DOC Note Type COMMS');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920102,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''MILITARY''',2,219102,'DOC Note Type MILITARY ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920103,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''AIRPORT''',3,219103,'DOC Note Type AIRPORT ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920104,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''WTR/SWR''',4,219104,'DOC Note Type WTR/SWR ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920105,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''SCH/UNV''',5,219105,'DOC Note Type SCH/UNV ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920106,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''POLICE''',6,219106,'DOC Note Type POLICE ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920107,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''FIRE''',7,219107,'DOC Note Type FIRE ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920108,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''CITY HALL''',8,219108,'DOC Note Type CITY HALL ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920109,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''NURSING''',9,219109,'DOC Note Type NURSING ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920110,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''HOSPITAL''',10,219110,'DOC Note Type HOSPITAL ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920111,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''8 PSB''',11,219111,'DOC Note Type 8 PSB ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920112,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''24 PSB''',12,219112,'DOC Note Type 24 PSB ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920113,219201,'DOC Note Symbol - OMS','DOC_NOTE_TYPE=''NGS''',13,219113,'DOC Note Type NGS ');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (21920199,219201,'DOC Note Symbol - OMS','',99,219199,'DOC Note Symbol Default');


spool off;
