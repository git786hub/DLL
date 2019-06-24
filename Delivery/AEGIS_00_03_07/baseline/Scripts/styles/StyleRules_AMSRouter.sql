set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_AMSRouter.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_AMSRouter.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for AMS Router
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
    where G3E_RULE in ('AMS Router Symbol', 'AMS Router Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'AMS Router Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'AMS Router Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (152101,'AMS Router Symbol - PPI','AEGIS Device',CHR(33),10158079,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (152102,'AMS Router Symbol - PPR','AEGIS Device',CHR(33),14540253,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (152103,'AMS Router Symbol - OSR','AEGIS Device',CHR(33),5921370,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (152199,'AMS Router Symbol - default','AEGIS Device',CHR(33),39679,12,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15210101,152101,'AMS Router Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',1,152101,'AMS Router Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15210102,152101,'AMS Router Symbol','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,152102,'AMS Router Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15210103,152101,'AMS Router Symbol','FEATURE_STATE_C in (''OSR'',''OSA'')',3,152103,'AMS Router Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15210199,152101,'AMS Router Symbol','',99,152199,'AMS Router Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15220101,152201,'AMS Router Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,152101,'AMS Router Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15220102,152201,'AMS Router Symbol - OMS','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,152102,'AMS Router Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15220103,152201,'AMS Router Symbol - OMS','FEATURE_STATE_C in (''OSR'',''OSA'')',3,152103,'AMS Router Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (15220199,152201,'AMS Router Symbol - OMS','',99,152199,'AMS Router Symbol - default');


spool off;
