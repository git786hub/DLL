set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_Autotransformer.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_Autotransformer.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Autotransformer
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
    where G3E_RULE in ('Autotransformer Symbol', 'Autotransformer Symbol - OMS'
                      ,'Autotransformer Symbol  ', 'Autotransformer Symbol   - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Autotransformer Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Autotransformer Symbol - OMS';
  delete from G3E_STYLERULE where G3E_RULE = 'Autotransformer Symbol  ';
  delete from G3E_STYLERULE where G3E_RULE = 'Autotransformer Symbol   - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (34108,'Autotransformer Symbol - PPI','AEGIS Transformer',CHR(79),10158079,18,8,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (34109,'Autotransformer Symbol - PPR','AEGIS Transformer',CHR(79),14540253,18,8,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (34110,'Autotransformer Symbol - OSR','AEGIS Transformer',CHR(79),5921370,18,8,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (34111,'Autotransformer Symbol - default','AEGIS Transformer',CHR(79),16777215,18,8,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3410101,34101,'Autotransformer Symbol  ','FEATURE_STATE_C in (''PPI'',''ABI'')',1,34108,'Autotransformer Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3410102,34101,'Autotransformer Symbol  ','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,34109,'Autotransformer Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3410103,34101,'Autotransformer Symbol  ','FEATURE_STATE_C in (''OSR'',''OSA'')',3,34110,'Autotransformer Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3410199,34101,'Autotransformer Symbol  ','',99,34111,'Autotransformer Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3420101,34201,'Autotransformer Symbol   - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,34108,'Autotransformer Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3420102,34201,'Autotransformer Symbol   - OMS','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,34109,'Autotransformer Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3420103,34201,'Autotransformer Symbol   - OMS','FEATURE_STATE_C in (''OSR'',''OSA'')',3,34110,'Autotransformer Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3420199,34201,'Autotransformer Symbol   - OMS','',99,34111,'Autotransformer Symbol - default');


spool off;
