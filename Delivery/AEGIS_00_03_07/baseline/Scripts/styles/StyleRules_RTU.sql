set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_RTU.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_RTU.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Remote Terminal Unit
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
    where G3E_RULE in ('Remote Terminal Unit Symbol', 'Remote Terminal Unit Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Remote Terminal Unit Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Remote Terminal Unit Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (160101,'Remote Terminal Unit Symbol - PPI','AEGIS Device',CHR(38),10158079,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (160102,'Remote Terminal Unit Symbol - PPR','AEGIS Device',CHR(38),14540253,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (160103,'Remote Terminal Unit Symbol - OSR','AEGIS Device',CHR(38),5921370,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (160199,'Remote Terminal Unit Symbol - default','AEGIS Device',CHR(38),39679,12,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16010101,160101,'Remote Terminal Unit Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',1,160101,'Remote Terminal Unit Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16010102,160101,'Remote Terminal Unit Symbol','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,160102,'Remote Terminal Unit Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16010103,160101,'Remote Terminal Unit Symbol','FEATURE_STATE_C in (''OSR'',''OSA'')',3,160103,'Remote Terminal Unit Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16010199,160101,'Remote Terminal Unit Symbol','',99,160199,'Remote Terminal Unit Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16020101,160201,'Remote Terminal Unit Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,160101,'Remote Terminal Unit Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16020102,160201,'Remote Terminal Unit Symbol - OMS','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,160102,'Remote Terminal Unit Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16020103,160201,'Remote Terminal Unit Symbol - OMS','FEATURE_STATE_C in (''OSR'',''OSA'')',3,160103,'Remote Terminal Unit Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (16020199,160201,'Remote Terminal Unit Symbol - OMS','',99,160199,'Remote Terminal Unit Symbol - default');


spool off;
