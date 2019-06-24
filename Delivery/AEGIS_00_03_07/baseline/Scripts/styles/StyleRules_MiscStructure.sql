set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_MiscStructure.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_MiscStructure.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Miscellaneous Structure
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
    where G3E_RULE in ('Miscellaneous Structure Symbol', 'Miscellaneous Structure Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Miscellaneous Structure Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Miscellaneous Structure Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (107101,'Miscellaneous Structure - Mid Span','AEGIS Structure',CHR(83),6416383,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (107102,'Miscellaneous Structure - Street Light Point','AEGIS Structure',CHR(82),6416383,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (107103,'Miscellaneous Structure - default','AEGIS Structure',CHR(81),6416383,12,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (10710101,107101,'Miscellaneous Structure Symbol','TYPE_C = ''MS''',1,107101,'Miscellaneous Structure - Mid Span');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (10710102,107101,'Miscellaneous Structure Symbol','TYPE_C = ''SP''',2,107102,'Miscellaneous Structure - Street Light Point');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (10710199,107101,'Miscellaneous Structure Symbol','',99,107103,'Miscellaneous Structure - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (10720101,107201,'Miscellaneous Structure Symbol - OMS','TYPE_C = ''MS''',1,107101,'Miscellaneous Structure - Mid Span');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (10720102,107201,'Miscellaneous Structure Symbol - OMS','TYPE_C = ''SP''',2,107102,'Miscellaneous Structure - Street Light Point');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (10720199,107201,'Miscellaneous Structure Symbol - OMS','',99,107103,'Miscellaneous Structure - default');


spool off;
