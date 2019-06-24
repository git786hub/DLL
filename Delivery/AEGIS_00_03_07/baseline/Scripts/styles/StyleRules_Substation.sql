set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_Substation.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_Substation.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Substation
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
    where G3E_RULE in ('Substation Symbol', 'Substation Symbol - OMS'
                      ,'Substation Point');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Substation Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Substation Symbol - OMS';

  delete from G3E_STYLERULE where G3E_RULE = 'Substation Point'; -- obsolete
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (115101,'Substation Symbol','AEGIS Structure',CHR(64),255,41,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11510199,115101,'Substation Symbol','',99,115101,'Substation Symbol');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11520199,115201,'Substation Symbol - OMS','',99,115101,'Substation Symbol');


spool off;
