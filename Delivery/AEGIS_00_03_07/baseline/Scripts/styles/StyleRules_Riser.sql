set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_Riser.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_Riser.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 09-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Riser
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
    where G3E_RULE in ('Riser Symbol', 'Riser Symbol - OMS', 'Riser Detail Symbol');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Riser Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Riser Symbol - OMS';
  
  delete from G3E_STYLERULE where G3E_RULE = 'Riser Detail Symbol'; -- obsolete
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (112101,'Riser Symbol Primary','AEGIS Structure',CHR(80),16777215,9,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (112102,'Riser Symbol Secondary','AEGIS Structure',CHR(80),16777215,6,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11210101,112101,'Riser Symbol','TYPE_C = ''PRI''',1,112101,'Riser Symbol Primary');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11210199,112101,'Riser Symbol','',99,112102,'Riser Symbol Secondary');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11220101,112201,'Riser Symbol - OMS','TYPE_C = ''PRI''',1,112101,'Riser Symbol Primary');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (11220199,112201,'Riser Symbol - OMS','',99,112102,'Riser Symbol Secondary');


spool off;
