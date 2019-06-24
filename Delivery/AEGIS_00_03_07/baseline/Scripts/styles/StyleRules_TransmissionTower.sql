set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_TransmissionTower.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_TransmissionTower.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Transmission Tower
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
    where G3E_RULE in ('Transmission Tower Symbol', 'Transmission Tower Symbol - OMS'
                      ,'Transmission Tower Proposed Symbol','Transmission Tower Proposed Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Transmission Tower Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Transmission Tower Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Transmission Tower Proposed Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Transmission Tower Proposed Symbol - OMS';

  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (116101,'Transmission Tower Symbol - PPI','AEGIS Structure',CHR(74),10158079,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (116102,'Transmission Tower Symbol - default','AEGIS Structure',CHR(74),6416383,12,0,0,0,null,0,1);

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (116103,'Transmission Tower Proposed Symbol','AEGIS Structure',CHR(48),10158079,23,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11610101,116101,'Transmission Tower Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',1,116101,'Transmission Tower Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11610199,116101,'Transmission Tower Symbol','',99,116102,'Transmission Tower Symbol - default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11614799,116147,'Transmission Tower Proposed Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',99,116103,'Transmission Tower Proposed Symbol');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11620101,116201,'Transmission Tower Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,116101,'Transmission Tower Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11620199,116201,'Transmission Tower Symbol - OMS','',99,116102,'Transmission Tower Symbol - default');

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11624699,116246,'Transmission Tower Proposed Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',99,116103,'Transmission Tower Proposed Symbol');


spool off;
