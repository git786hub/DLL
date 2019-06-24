set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_StreetLight.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_StreetLight.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Street Light
--**************************************************************************************
-- Modified:
--  03-MAR-2018, Rich Adase -- Fixed rule filters to use single quotes and abbreviated values
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
    where G3E_RULE in ('Street Light Symbol', 'Street Light Symbol - OMS'
                      ,'Street Light Customer Owned Symbol', 'Street Light Customer Owned Symbol - OMS'
                      ,'Street Light Disconnected Symbol', 'Street Light Disconnected Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Customer Owned Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Customer Owned Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Disconnected Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Disconnected Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56101,'Street Light - High Pressure Sodium - PPI','AEGIS Misc',CHR(104),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56102,'Street Light - High Pressure Sodium - PPR','AEGIS Misc',CHR(104),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56103,'Street Light - High Pressure Sodium - OSR','AEGIS Misc',CHR(104),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56104,'Street Light - High Pressure Sodium Custormer','AEGIS Misc',CHR(104),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56105,'Street Light - High Pressure Sodium','AEGIS Misc',CHR(104),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56106,'Street Light - Mercury Vapor - PPI','AEGIS Misc',CHR(105),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56107,'Street Light - Mercury Vapor - PPR','AEGIS Misc',CHR(105),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56108,'Street Light - Mercury Vapor - OSR','AEGIS Misc',CHR(105),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56109,'Street Light - Mercury Vapor Custormer','AEGIS Misc',CHR(105),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56110,'Street Light - Mercury Vapor','AEGIS Misc',CHR(105),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56111,'Street Light - Fluorescent - PPI','AEGIS Misc',CHR(106),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56112,'Street Light - Fluorescent - PPR','AEGIS Misc',CHR(106),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56113,'Street Light - Fluorescent - OSR','AEGIS Misc',CHR(106),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56114,'Street Light - Fluorescent Custormer','AEGIS Misc',CHR(106),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56115,'Street Light - Fluorescent','AEGIS Misc',CHR(106),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56116,'Street Light - Metal Halide - PPI','AEGIS Misc',CHR(107),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56117,'Street Light - Metal Halide - PPR','AEGIS Misc',CHR(107),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56118,'Street Light - Metal Halide - OSR','AEGIS Misc',CHR(107),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56119,'Street Light - Metal Halide Custormer','AEGIS Misc',CHR(107),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56120,'Street Light - Metal Halide','AEGIS Misc',CHR(107),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56121,'Street Light - SV - PPI','AEGIS Misc',CHR(104),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56122,'Street Light - SV - PPR','AEGIS Misc',CHR(104),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56123,'Street Light - SV - OSR','AEGIS Misc',CHR(104),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56124,'Street Light - SV Custormer','AEGIS Misc',CHR(104),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56125,'Street Light - SV','AEGIS Misc',CHR(104),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56126,'Street Light - Aviation Obstruction - PPI','AEGIS Misc',CHR(109),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56127,'Street Light - Aviation Obstruction - PPR','AEGIS Misc',CHR(109),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56128,'Street Light - Aviation Obstruction - OSR','AEGIS Misc',CHR(109),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56129,'Street Light - Aviation Obstruction Custormer','AEGIS Misc',CHR(109),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56130,'Street Light - Aviation Obstruction','AEGIS Misc',CHR(109),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56131,'Street Light - LED - PPI','AEGIS Misc',CHR(108),10158079,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56132,'Street Light - LED - PPR','AEGIS Misc',CHR(108),14540253,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56133,'Street Light - LED - OSR','AEGIS Misc',CHR(108),5921370,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56134,'Street Light - LED Custormer','AEGIS Misc',CHR(108),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56135,'Street Light - LED','AEGIS Misc',CHR(108),14516479,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56136,'Street Light - Default Custormer','AEGIS Misc',CHR(108),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56137,'Street Light - Default','AEGIS Misc',CHR(103),14516479,24,1,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56138,'Street Light Customer Owned Symbol - Schedule C','AEGIS Misc',CHR(99),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56139,'Street Light Customer Owned Symbol - Schedule D','AEGIS Misc',CHR(100),12844988,24,1,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56140,'Street Light Customer Owned Symbol - default','AEGIS Misc',CHR(98),12844988,24,1,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (56141,'Street Light Disconnect Symbol','AEGIS Misc',CHR(101),255,24,1,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610101,56101,'Street Light Symbol','LAMP_TYPE_C= ''HP'' and FEATURE_STATE_C in (''PPI'',''ABI'')',1,56101,'Street Light - High Pressure Sodium - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610102,56101,'Street Light Symbol','LAMP_TYPE_C= ''HP'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,56102,'Street Light - High Pressure Sodium - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610103,56101,'Street Light Symbol','LAMP_TYPE_C= ''HP'' and FEATURE_STATE_C in (''OSR'',''OSA'')',3,56103,'Street Light - High Pressure Sodium - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610104,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''HP''',4,56104,'Street Light - High Pressure Sodium Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610105,56101,'Street Light Symbol','LAMP_TYPE_C= ''HP''',5,56105,'Street Light - High Pressure Sodium');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610106,56101,'Street Light Symbol','LAMP_TYPE_C= ''MV'' and FEATURE_STATE_C in (''PPI'',''ABI'')',6,56106,'Street Light - Mercury Vapor - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610107,56101,'Street Light Symbol','LAMP_TYPE_C= ''MV'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',7,56107,'Street Light - Mercury Vapor - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610108,56101,'Street Light Symbol','LAMP_TYPE_C= ''MV'' and FEATURE_STATE_C in (''OSR'',''OSA'')',8,56108,'Street Light - Mercury Vapor - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610109,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''MV''',9,56109,'Street Light - Mercury Vapor Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610110,56101,'Street Light Symbol','LAMP_TYPE_C= ''MV''',10,56110,'Street Light - Mercury Vapor');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610111,56101,'Street Light Symbol','LAMP_TYPE_C= ''FL'' and FEATURE_STATE_C in (''PPI'',''ABI'')',11,56111,'Street Light - Fluorescent - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610112,56101,'Street Light Symbol','LAMP_TYPE_C= ''FL'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',12,56112,'Street Light - Fluorescent - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610113,56101,'Street Light Symbol','LAMP_TYPE_C= ''FL'' and FEATURE_STATE_C in (''OSR'',''OSA'')',13,56113,'Street Light - Fluorescent - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610114,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''FL''',14,56114,'Street Light - Fluorescent Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610115,56101,'Street Light Symbol','LAMP_TYPE_C= ''FL''',15,56115,'Street Light - Fluorescent');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610116,56101,'Street Light Symbol','LAMP_TYPE_C= ''MH'' and FEATURE_STATE_C in (''PPI'',''ABI'')',16,56116,'Street Light - Metal Halide - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610117,56101,'Street Light Symbol','LAMP_TYPE_C= ''MH'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',17,56117,'Street Light - Metal Halide - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610118,56101,'Street Light Symbol','LAMP_TYPE_C= ''MH'' and FEATURE_STATE_C in (''OSR'',''OSA'')',18,56118,'Street Light - Metal Halide - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610119,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''MH''',19,56119,'Street Light - Metal Halide Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610120,56101,'Street Light Symbol','LAMP_TYPE_C= ''MH''',20,56120,'Street Light - Metal Halide');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610121,56101,'Street Light Symbol','LAMP_TYPE_C= ''SV'' and FEATURE_STATE_C in (''PPI'',''ABI'')',21,56121,'Street Light - SV - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610122,56101,'Street Light Symbol','LAMP_TYPE_C= ''SV'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',22,56122,'Street Light - SV - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610123,56101,'Street Light Symbol','LAMP_TYPE_C= ''SV'' and FEATURE_STATE_C in (''OSR'',''OSA'')',23,56123,'Street Light - SV - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610124,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''SV''',24,56124,'Street Light - SV Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610125,56101,'Street Light Symbol','LAMP_TYPE_C= ''SV''',25,56125,'Street Light - SV');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610126,56101,'Street Light Symbol','LAMP_TYPE_C= ''AV'' and FEATURE_STATE_C in (''PPI'',''ABI'')',26,56126,'Street Light - Aviation Obstruction - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610127,56101,'Street Light Symbol','LAMP_TYPE_C= ''AV'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',27,56127,'Street Light - Aviation Obstruction - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610128,56101,'Street Light Symbol','LAMP_TYPE_C= ''AV'' and FEATURE_STATE_C in (''OSR'',''OSA'')',28,56128,'Street Light - Aviation Obstruction - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610129,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''AV''',29,56129,'Street Light - Aviation Obstruction Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610130,56101,'Street Light Symbol','LAMP_TYPE_C= ''AV''',30,56130,'Street Light - Aviation Obstruction');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610131,56101,'Street Light Symbol','LAMP_TYPE_C= ''LE'' and FEATURE_STATE_C in (''PPI'',''ABI'')',31,56131,'Street Light - LED - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610132,56101,'Street Light Symbol','LAMP_TYPE_C= ''LE'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',32,56132,'Street Light - LED - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610133,56101,'Street Light Symbol','LAMP_TYPE_C= ''LE'' and FEATURE_STATE_C in (''OSR'',''OSA'')',33,56133,'Street Light - LED - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610134,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''LE''',34,56134,'Street Light - LED Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610135,56101,'Street Light Symbol','LAMP_TYPE_C= ''LE''',35,56135,'Street Light - LED');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610136,56101,'Street Light Symbol','OWNED_TYPE_C != ''COMPANY''',36,56136,'Street Light - Default Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610199,56101,'Street Light Symbol','',99,56137,'Street Light - Default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610301,56103,'Street Light Customer Owned Symbol','RATE_SCHEDULE_C =''C''',1,56138,'Street Light Customer Owned Symbol - Schedule C');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610302,56103,'Street Light Customer Owned Symbol','RATE_SCHEDULE_C =''D''',2,56139,'Street Light Customer Owned Symbol - Schedule D');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610399,56103,'Street Light Customer Owned Symbol','',99,56140,'Street Light Customer Owned Symbol - default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5610499,56104,'Street Light Disconnected Symbol','',99,56141,'Street Light Disconnect Symbol');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620101,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''HP'' and FEATURE_STATE_C in (''PPI'',''ABI'')',1,56101,'Street Light - High Pressure Sodium - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620102,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''HP'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,56102,'Street Light - High Pressure Sodium - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620103,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''HP'' and FEATURE_STATE_C in (''OSR'',''OSA'')',3,56103,'Street Light - High Pressure Sodium - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620104,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''HP''',4,56104,'Street Light - High Pressure Sodium Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620105,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''HP''',5,56105,'Street Light - High Pressure Sodium');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620106,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MV'' and FEATURE_STATE_C in (''PPI'',''ABI'')',6,56106,'Street Light - Mercury Vapor - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620107,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MV'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',7,56107,'Street Light - Mercury Vapor - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620108,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MV'' and FEATURE_STATE_C in (''OSR'',''OSA'')',8,56108,'Street Light - Mercury Vapor - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620109,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''MV''',9,56109,'Street Light - Mercury Vapor Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620110,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MV''',10,56110,'Street Light - Mercury Vapor');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620111,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''FL'' and FEATURE_STATE_C in (''PPI'',''ABI'')',11,56111,'Street Light - Fluorescent - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620112,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''FL'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',12,56112,'Street Light - Fluorescent - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620113,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''FL'' and FEATURE_STATE_C in (''OSR'',''OSA'')',13,56113,'Street Light - Fluorescent - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620114,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''FL''',14,56114,'Street Light - Fluorescent Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620115,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''FL''',15,56115,'Street Light - Fluorescent');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620116,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MH'' and FEATURE_STATE_C in (''PPI'',''ABI'')',16,56116,'Street Light - Metal Halide - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620117,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MH'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',17,56117,'Street Light - Metal Halide - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620118,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MH'' and FEATURE_STATE_C in (''OSR'',''OSA'')',18,56118,'Street Light - Metal Halide - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620119,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''MH''',19,56119,'Street Light - Metal Halide Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620120,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''MH''',20,56120,'Street Light - Metal Halide');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620121,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''SV'' and FEATURE_STATE_C in (''PPI'',''ABI'')',21,56121,'Street Light - SV - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620122,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''SV'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',22,56122,'Street Light - SV - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620123,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''SV'' and FEATURE_STATE_C in (''OSR'',''OSA'')',23,56123,'Street Light - SV - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620124,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''SV''',24,56124,'Street Light - SV Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620125,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''SV''',25,56125,'Street Light - SV');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620126,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''AV'' and FEATURE_STATE_C in (''PPI'',''ABI'')',26,56126,'Street Light - Aviation Obstruction - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620127,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''AV'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',27,56127,'Street Light - Aviation Obstruction - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620128,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''AV'' and FEATURE_STATE_C in (''OSR'',''OSA'')',28,56128,'Street Light - Aviation Obstruction - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620129,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''AV''',29,56129,'Street Light - Aviation Obstruction Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620130,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''AV''',30,56130,'Street Light - Aviation Obstruction');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620131,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''LE'' and FEATURE_STATE_C in (''PPI'',''ABI'')',31,56131,'Street Light - LED - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620132,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''LE'' and FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',32,56132,'Street Light - LED - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620133,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''LE'' and FEATURE_STATE_C in (''OSR'',''OSA'')',33,56133,'Street Light - LED - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620134,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY'' and LAMP_TYPE_C= ''LE''',34,56134,'Street Light - LED Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620135,56201,'Street Light Symbol - OMS','LAMP_TYPE_C= ''LE''',35,56135,'Street Light - LED');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620136,56201,'Street Light Symbol - OMS','OWNED_TYPE_C != ''COMPANY''',36,56136,'Street Light - Default Custormer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620199,56201,'Street Light Symbol - OMS','',99,56137,'Street Light - Default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620301,56203,'Street Light Customer Owned Symbol - OMS','RATE_SCHEDULE_C =''C''',1,56138,'Street Light Customer Owned Symbol - Schedule C');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620302,56203,'Street Light Customer Owned Symbol - OMS','RATE_SCHEDULE_C =''D''',2,56139,'Street Light Customer Owned Symbol - Schedule D');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620399,56203,'Street Light Customer Owned Symbol - OMS','',99,56140,'Street Light Customer Owned Symbol - default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5620499,56204,'Street Light Disconnected Symbol - OMS','',99,56141,'Street Light Disconnect Symbol');


spool off;