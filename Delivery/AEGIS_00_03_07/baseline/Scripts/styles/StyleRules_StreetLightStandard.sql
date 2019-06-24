set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_StreetLightStandard.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_StreetLightStandard.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Street Light Standard
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
    where G3E_RULE in ('Street Light Standard Symbol', 'Street Light Standard Symbol - OMS'
                      ,'Street Light Standard Proposed Symbol','Street Light Standard Active Permit Symbol'
                      ,'Street Light Standard Proposed Symbol - OMS','Street Light Standard Active Permit Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Standard Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Standard Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Standard Proposed Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Standard Active Permit Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Standard Proposed Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Street Light Standard Active Permit Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (114101,'Street Light Standard Symbol - PPI','AEGIS Structure',CHR(75),10158079,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (114102,'Street Light Standard Symbol','AEGIS Structure',CHR(75),6416383,12,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (114103,'Active Permit Symbol','AEGIS Structure',CHR(49),24319,15,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (114104,'Street Light Standard Proposed Symbol','AEGIS Structure',CHR(48),10158079,23,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11410101,114101,'Street Light Standard Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',1,114101,'Street Light Standard Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11410199,114101,'Street Light Standard Symbol','',99,114102,'Street Light Standard Symbol');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11414699,114146,'Street Light Standard Active Permit Symbol','',99,114103,'Active Permit Symbol');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11414799,114147,'Street Light Standard Proposed Symbol','',99,114104,'Street Light Standard Proposed Symbol');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11420101,114201,'Street Light Standard Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,114101,'Street Light Standard Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11420199,114201,'Street Light Standard Symbol - OMS','',99,114102,'Street Light Standard Symbol');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11424699,114246,'Street Light Standard Active Permit Symbol - OMS','',99,114103,'Active Permit Symbol');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11424799,114247,'Street Light Standard Proposed Symbol - OMS','',99,114104,'Street Light Standard Proposed Symbol');


spool off;
