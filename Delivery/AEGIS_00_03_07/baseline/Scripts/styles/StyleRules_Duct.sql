set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_Duct.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_Duct.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Duct
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
    where G3E_RULE in ('Duct From Symbol', 'Duct From Symbol - OMS'
                      ,'Duct To Symbol'  , 'Duct To Symbol - OMS'
                      ,'Duct From Geo Symbol', 'Duct From Detail Symbol'
                      ,'Duct To Geo Symbol'  , 'Duct To Detail Symbol');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Duct From Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Duct From Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Duct To Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Duct To Symbol - OMS';
  
  delete from G3E_STYLERULE where G3E_RULE = 'Duct From Geo Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Duct From Detail Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Duct To Geo Symbol'; -- obsolete
  delete from G3E_STYLERULE where G3E_RULE = 'Duct To Detail Symbol'; -- obsolete

  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (2300101,'Duct From Symbol - default','AEGIS Structure',CHR(50),8453982,6,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (2300102,'Duct To Symbol - default','AEGIS Structure',CHR(50),8453982,6,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (230010199,2300101,'Duct From Symbol','',99,2300101,'Duct From Symbol - default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (230010299,2300102,'Duct To Symbol','',99,2300102,'Duct To Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (230020199,2300201,'Duct From Symbol - OMS','',99,2300101,'Duct From Symbol - default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (230020299,2300202,'Duct To Symbol - OMS','',99,2300102,'Duct To Symbol - default');


spool off;
