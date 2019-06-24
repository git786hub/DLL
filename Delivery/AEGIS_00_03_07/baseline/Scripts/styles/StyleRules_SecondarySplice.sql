set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_SecondarySplice.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_SecondarySplice.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Secondary Splice
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
    where G3E_RULE in ('Secondary Splice Detail Symbol', 'Secondary Splice Detail Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Secondary Splice Detail Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Secondary Splice Detail Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (23101,'Secondary Splice Detail Symbol - I','AEGIS Misc',CHR(86),16777215,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (23102,'Secondary Splice Detail Symbol - Y','AEGIS Misc',CHR(87),16777215,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (23103,'Secondary Splice Detail Symbol - H','AEGIS Misc',CHR(88),16777215,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (23104,'Secondary Splice Detail Symbol - Crab 8','AEGIS Misc',CHR(89),16777215,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (23105,'Secondary Splice Detail Symbol - Crab 12','AEGIS Misc',CHR(90),16777215,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (23199,'Secondary Splice Detail Symbol - default','AEGIS Misc',CHR(86),16777215,12,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2310101,23101,'Secondary Splice Detail Symbol','SPLICE_C = ''I''',1,23101,'Secondary Splice Detail Symbol - I');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2310102,23101,'Secondary Splice Detail Symbol','SPLICE_C = ''Y''',2,23102,'Secondary Splice Detail Symbol - Y');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2310103,23101,'Secondary Splice Detail Symbol','SPLICE_C = ''H''',3,23103,'Secondary Splice Detail Symbol - H');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2310104,23101,'Secondary Splice Detail Symbol','SPLICE_C = ''Crab'' and Splice Bank Attributes.Connection Type = ''Crab 8''',4,23104,'Secondary Splice Detail Symbol - Crab 8');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2310105,23101,'Secondary Splice Detail Symbol','SPLICE_C = ''Crab'' and Splice Bank Attributes.Connection Type = ''Crab 12''',5,23105,'Secondary Splice Detail Symbol - Crab 12');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2310199,23101,'Secondary Splice Detail Symbol','',99,23199,'Secondary Splice Detail Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2340101,23401,'Secondary Splice Detail Symbol - OMS','SPLICE_C = ''I''',1,23101,'Secondary Splice Detail Symbol - I');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2340102,23401,'Secondary Splice Detail Symbol - OMS','SPLICE_C = ''Y''',2,23102,'Secondary Splice Detail Symbol - Y');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2340103,23401,'Secondary Splice Detail Symbol - OMS','SPLICE_C = ''H''',3,23103,'Secondary Splice Detail Symbol - H');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2340104,23401,'Secondary Splice Detail Symbol - OMS','SPLICE_C = ''Crab'' and Splice Bank Attributes.Connection Type = ''Crab 8''',4,23104,'Secondary Splice Detail Symbol - Crab 8');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2340105,23401,'Secondary Splice Detail Symbol - OMS','SPLICE_C = ''Crab'' and Splice Bank Attributes.Connection Type = ''Crab 12''',5,23105,'Secondary Splice Detail Symbol - Crab 12');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (2340199,23401,'Secondary Splice Detail Symbol - OMS','',99,23199,'Secondary Splice Detail Symbol - default');


spool off;
