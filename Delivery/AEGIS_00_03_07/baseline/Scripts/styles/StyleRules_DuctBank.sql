set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_DuctBank.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_DuctBank.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 13-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Duct Bank
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
    where G3E_RULE in ('Duct Bank Linear', 'Duct Bank Linear - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Duct Bank Linear';
  delete from G3E_STYLERULE where G3E_RULE = 'Duct Bank Linear - OMS';

  for i in 1..arrSNO.COUNT loop
    delete from G3E_LINESTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;


-- Stroke patterns and symbols

delete from G3E_NORMALIZEDSTROKE where G3E_SPNO between 2200101 and 2200199;
delete from G3E_POINTSTYLE where G3E_SNO between 151 and 199;

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (151,'Stroke Symbol - Duct Bank PPI','AEGIS Structure',CHR(53),10158079,3,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (152,'Stroke Symbol - Duct Bank PPR','AEGIS Structure',CHR(53),14540253,1.5,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (153,'Stroke Symbol - Duct Bank OSR','AEGIS Structure',CHR(53),5921370,1.5,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (154,'Stroke Symbol - Duct Bank default','AEGIS Structure',CHR(53),8453982,1.5,0,0,0,null,0,1);

insert into G3E_NORMALIZEDSTROKE (G3E_SPNO,G3E_USERNAME,G3E_REPETITIONS,G3E_VISIBLEDASHES,G3E_DASHPATTERNADJUSTMENT,G3E_SEQUENCE_0,G3E_SEQUENCE_1,G3E_SEQUENCE_2,G3E_SEQUENCE_3,G3E_SEQUENCE_4,G3E_SEQUENCE_5,G3E_SEQUENCE_6,G3E_SEQUENCE_7,G3E_SEQUENCE_8,G3E_SEQUENCE_9,G3E_SYMBOLINDEX_0,G3E_STROKESYMBOL_0,G3E_SYMBOLPOSITION_0,G3E_SYMBOLROTATION_0,G3E_SYMBOLROTATIONRULE_0,G3E_SYMBOLOFFSET_0) values (2200101,'Duct Bank - PPI',0,1,'BOTH',-20,0,null,null,null,null,null,null,null,null,1,151,0.5,0,2,0);
insert into G3E_NORMALIZEDSTROKE (G3E_SPNO,G3E_USERNAME,G3E_REPETITIONS,G3E_VISIBLEDASHES,G3E_DASHPATTERNADJUSTMENT,G3E_SEQUENCE_0,G3E_SEQUENCE_1,G3E_SEQUENCE_2,G3E_SEQUENCE_3,G3E_SEQUENCE_4,G3E_SEQUENCE_5,G3E_SEQUENCE_6,G3E_SEQUENCE_7,G3E_SEQUENCE_8,G3E_SEQUENCE_9,G3E_SYMBOLINDEX_0,G3E_STROKESYMBOL_0,G3E_SYMBOLPOSITION_0,G3E_SYMBOLROTATION_0,G3E_SYMBOLROTATIONRULE_0,G3E_SYMBOLOFFSET_0) values (2200102,'Duct Bank - PPR',0,1,'BOTH',-20,0,null,null,null,null,null,null,null,null,1,152,0.5,0,2,0);
insert into G3E_NORMALIZEDSTROKE (G3E_SPNO,G3E_USERNAME,G3E_REPETITIONS,G3E_VISIBLEDASHES,G3E_DASHPATTERNADJUSTMENT,G3E_SEQUENCE_0,G3E_SEQUENCE_1,G3E_SEQUENCE_2,G3E_SEQUENCE_3,G3E_SEQUENCE_4,G3E_SEQUENCE_5,G3E_SEQUENCE_6,G3E_SEQUENCE_7,G3E_SEQUENCE_8,G3E_SEQUENCE_9,G3E_SYMBOLINDEX_0,G3E_STROKESYMBOL_0,G3E_SYMBOLPOSITION_0,G3E_SYMBOLROTATION_0,G3E_SYMBOLROTATIONRULE_0,G3E_SYMBOLOFFSET_0) values (2200103,'Duct Bank - OSR',0,1,'BOTH',-20,0,null,null,null,null,null,null,null,null,1,153,0.5,0,2,0);
insert into G3E_NORMALIZEDSTROKE (G3E_SPNO,G3E_USERNAME,G3E_REPETITIONS,G3E_VISIBLEDASHES,G3E_DASHPATTERNADJUSTMENT,G3E_SEQUENCE_0,G3E_SEQUENCE_1,G3E_SEQUENCE_2,G3E_SEQUENCE_3,G3E_SEQUENCE_4,G3E_SEQUENCE_5,G3E_SEQUENCE_6,G3E_SEQUENCE_7,G3E_SEQUENCE_8,G3E_SEQUENCE_9,G3E_SYMBOLINDEX_0,G3E_STROKESYMBOL_0,G3E_SYMBOLPOSITION_0,G3E_SYMBOLROTATION_0,G3E_SYMBOLROTATIONRULE_0,G3E_SYMBOLOFFSET_0) values (2200104,'Duct Bank - default',0,1,'BOTH',-20,0,null,null,null,null,null,null,null,null,1,154,0.5,0,2,0);


-- Line styles

insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (2200301,'Duct Bank - PPI',10158079,3.5,2200101,0,null,null,1,3);
insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (2200302,'Duct Bank - PPR',15658734,2.5,2200102,0,null,null,1,3);
insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (2200303,'Duct Bank - OSR',14540253,2.5,2200103,0,null,null,1,3);
insert into G3E_LINESTYLE(G3E_SNO,G3E_USERNAME,G3E_COLOR,G3E_WIDTH,G3E_STROKEPATTERN,G3E_OFFSET,G3E_STARTSYMBOL,G3E_ENDSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (2200399,'Duct Bank - default',8453982,2.5,2200104,0,null,null,1,3);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220010101,2200101,'Duct Bank Linear','FEATURE_STATE_C in (''PPI'',''ABI'')',1,2200301,'Duct Bank - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220010102,2200101,'Duct Bank Linear','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,2200302,'Duct Bank - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220010103,2200101,'Duct Bank Linear','FEATURE_STATE_C in (''OSR'',''OSA'')',3,2200303,'Duct Bank - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220010199,2200101,'Duct Bank Linear','',99,2200399,'Duct Bank - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220020101,2200201,'Duct Bank Linear - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,2200301,'Duct Bank - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220020102,2200201,'Duct Bank Linear - OMS','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,2200302,'Duct Bank - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220020103,2200201,'Duct Bank Linear - OMS','FEATURE_STATE_C in (''OSR'',''OSA'')',3,2200303,'Duct Bank - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (220020199,2200201,'Duct Bank Linear - OMS','',99,2200399,'Duct Bank - default');


spool off;
