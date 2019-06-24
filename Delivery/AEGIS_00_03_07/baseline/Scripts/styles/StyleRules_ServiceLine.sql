set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_ServiceLine.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_ServiceLine.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 06-JUL-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Service Line
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
    where G3E_RULE in ('Service Line Linear', 'Service Line Linear - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Service Line Linear';
  delete from G3E_STYLERULE where G3E_RULE = 'Service Line Linear - OMS';

  for i in 1..arrSNO.COUNT loop
    delete from G3E_LINESTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;


delete from G3E_LINESTYLE where G3E_SNO between 54301 and 54305;


-- Line styles

insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (54301,'Service Line Linear - Material',10158079,3.5,200,0,null,null,1,3);
insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (54302,'Service Line Linear - Associated',39679,1.75,200,0,null,null,1,3);
insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (54303,'Service Line Linear - Customer',12844988,1.75,200,0,null,null,1,3);
insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (54304,'Service Line Linear - default',14548736,1.75,200,0,null,null,1,3);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5410101,54101,'Service Line Linear','FEATURE_STATE_C != ''CLS'' and PLACEMENT_TYPE_C = ''MATERIAL''',1,54301,'Service Line Linear - Material');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5410102,54101,'Service Line Linear','FEATURE_STATE_C != ''CLS'' and PLACEMENT_TYPE_C = ''ASSOCIATED''',2,54302,'Service Line Linear - Associated');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5410103,54101,'Service Line Linear','OWNED_TYPE_C = ''CUSTOMER''',3,54303,'Service Line Linear - Customer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5410199,54101,'Service Line Linear','',99,54304,'Service Line Linear - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5420101,54201,'Service Line Linear - OMS','FEATURE_STATE_C != ''CLS'' and PLACEMENT_TYPE_C = ''MATERIAL''',1,54301,'Service Line Linear - Material');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5420102,54201,'Service Line Linear - OMS','FEATURE_STATE_C != ''CLS'' and PLACEMENT_TYPE_C = ''ASSOCIATED''',2,54302,'Service Line Linear - Associated');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5420103,54201,'Service Line Linear - OMS','OWNED_TYPE_C = ''CUSTOMER''',3,54303,'Service Line Linear - Customer');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (5420199,54201,'Service Line Linear - OMS','',99,54304,'Service Line Linear - default');


spool off;
