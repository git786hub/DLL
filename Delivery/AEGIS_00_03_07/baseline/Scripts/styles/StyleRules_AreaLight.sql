set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_AreaLight.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_AreaLight.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Area Light
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
    where G3E_RULE in ('Area Light Symbol'             , 'Area Light Symbol - OMS'
                      ,'Area Light Disconnected Symbol', 'Area Light Disconnected Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Area Light Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Area Light Disconnected Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Area Light Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Area Light Disconnected Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61101,'Area Light - Guard, LED - PPI','AEGIS Misc',CHR(110),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61102,'Area Light - Guard, LED - PPR','AEGIS Misc',CHR(110),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61103,'Area Light - Guard, LED - OSR','AEGIS Misc',CHR(110),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61104,'Area Light - Guard, LED','AEGIS Misc',CHR(110),16768256,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61105,'Area Light - Guard - PPI','AEGIS Misc',CHR(111),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61106,'Area Light - Guard - PPR','AEGIS Misc',CHR(111),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61107,'Area Light - Guard - OSR','AEGIS Misc',CHR(111),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61108,'Area Light - Guard','AEGIS Misc',CHR(111),16768256,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61109,'Area Light - Flood - PPI','AEGIS Misc',CHR(112),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61110,'Area Light - Flood - PPR','AEGIS Misc',CHR(112),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61111,'Area Light - Flood - OSR','AEGIS Misc',CHR(112),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61112,'Area Light - Flood','AEGIS Misc',CHR(112),16768256,24,1,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (61190,'Area Light Disconnected Symbol','AEGIS Misc',CHR(101),255,24,1,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110101,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE'' and FEATURE_STATE_C in (''PPI'',''ABI'')',1,61101,'Area Light - Guard, LED - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110102,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,61102,'Area Light - Guard, LED - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110103,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE'' and FEATURE_STATE_C in (''OSR'',''OSA'')',3,61103,'Area Light - Guard, LED - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110104,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE''',4,61104,'Area Light - Guard, LED');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110105,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and FEATURE_STATE_C in (''PPI'',''ABI'')',5,61105,'Area Light - Guard - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110106,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',6,61106,'Area Light - Guard - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110107,61101,'Area Light Symbol','LAMP_USE_C = ''G'' and FEATURE_STATE_C in (''OSR'',''OSA'')',7,61107,'Area Light - Guard - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110108,61101,'Area Light Symbol','LAMP_USE_C = ''G''',8,61108,'Area Light - Guard');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110109,61101,'Area Light Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',9,61109,'Area Light - Flood - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110110,61101,'Area Light Symbol','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',10,61110,'Area Light - Flood - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110111,61101,'Area Light Symbol','FEATURE_STATE_C in (''OSR'',''OSA'')',11,61111,'Area Light - Flood - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110199,61101,'Area Light Symbol','',99,61112,'Area Light - Flood');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6110499,61104,'Area Light Disconnected Symbol','',99,61190,'Area Light Disconnected Symbol');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120101,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE'' and FEATURE_STATE_C in (''PPI'',''ABI'')',1,61101,'Area Light - Guard, LED - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120102,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,61102,'Area Light - Guard, LED - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120103,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE'' and FEATURE_STATE_C in (''OSR'',''OSA'')',3,61103,'Area Light - Guard, LED - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120104,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and LAMP_TYPE_C = ''LE''',4,61104,'Area Light - Guard, LED');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120105,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and FEATURE_STATE_C in (''PPI'',''ABI'')',5,61105,'Area Light - Guard - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120106,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',6,61106,'Area Light - Guard - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120107,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G'' and FEATURE_STATE_C in (''OSR'',''OSA'')',7,61107,'Area Light - Guard - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120108,61201,'Area Light Symbol - OMS','LAMP_USE_C = ''G''',8,61108,'Area Light - Guard');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120109,61201,'Area Light Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',9,61109,'Area Light - Flood - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120110,61201,'Area Light Symbol - OMS','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',10,61110,'Area Light - Flood - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120111,61201,'Area Light Symbol - OMS','FEATURE_STATE_C in (''OSR'',''OSA'')',11,61111,'Area Light - Flood - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120199,61201,'Area Light Symbol - OMS','',99,61112,'Area Light - Flood');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (6120499,61204,'Area Light Disconnected Symbol - OMS','',99,61190,'Area Light Disconnected Symbol');


spool off;