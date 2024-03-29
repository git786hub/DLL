set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_PrimaryConductorNode.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_PrimaryConductorNode.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Primary Conductor Node
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
    where G3E_RULE in ('Primary Conductor Node Symbol', 'Primary Conductor Node Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Primary Conductor Node Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Primary Conductor Node Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10701,'Primary Conductor Node Symbol - Change  KV1','AEGIS Misc',CHR(35),3956378,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10702,'Primary Conductor Node Symbol - Change KV2','AEGIS Misc',CHR(35),24285,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10703,'Primary Conductor Node Symbol - Change KV3','AEGIS Misc',CHR(35),39679,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10704,'Primary Conductor Node Symbol - Change KV4','AEGIS Misc',CHR(35),8453982,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10705,'Primary Conductor Node Symbol - Change KV5','AEGIS Misc',CHR(35),39424,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10706,'Primary Conductor Node Symbol - Change KV6','AEGIS Misc',CHR(35),19200,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10707,'Primary Conductor Node Symbol - Change default','AEGIS Misc',CHR(35),65535,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10708,'Primary Conductor Node Symbol - Dead End  KV1','AEGIS Misc',CHR(34),3956378,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10709,'Primary Conductor Node Symbol - Dead End KV2','AEGIS Misc',CHR(34),24285,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10710,'Primary Conductor Node Symbol - Dead End KV3','AEGIS Misc',CHR(34),39679,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10711,'Primary Conductor Node Symbol - Dead End KV4','AEGIS Misc',CHR(34),8453982,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10712,'Primary Conductor Node Symbol - Dead End KV5','AEGIS Misc',CHR(34),39424,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10713,'Primary Conductor Node Symbol - Dead End KV6','AEGIS Misc',CHR(34),19200,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10714,'Primary Conductor Node Symbol - Dead End default','AEGIS Misc',CHR(34),65535,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10715,'Primary Conductor Node Symbol - Jumpover  KV1','AEGIS Misc',CHR(36),3956378,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10716,'Primary Conductor Node Symbol - Jumpover KV2','AEGIS Misc',CHR(36),24285,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10717,'Primary Conductor Node Symbol - Jumpover KV3','AEGIS Misc',CHR(36),39679,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10718,'Primary Conductor Node Symbol - Jumpover KV4','AEGIS Misc',CHR(36),8453982,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10719,'Primary Conductor Node Symbol - Jumpover KV5','AEGIS Misc',CHR(36),39424,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10720,'Primary Conductor Node Symbol - Jumpover KV6','AEGIS Misc',CHR(36),19200,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10721,'Primary Conductor Node Symbol - Jumpover  default','AEGIS Misc',CHR(36),65535,18,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (10799,'Primary Conductor Node Symbol - default','AEGIS Misc',CHR(37),65535,12,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010101,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 4.1 and TYPE_C=''CHANGE''',1,10701,'Primary Conductor Node Symbol - Change  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010102,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 12.5 and TYPE_C=''CHANGE''',2,10702,'Primary Conductor Node Symbol - Change KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010103,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 13.2 and TYPE_C=''CHANGE''',3,10703,'Primary Conductor Node Symbol - Change KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010104,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 21.6 and TYPE_C=''CHANGE''',4,10704,'Primary Conductor Node Symbol - Change KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010105,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 24.9 and TYPE_C=''CHANGE''',5,10705,'Primary Conductor Node Symbol - Change KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010106,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 33 and TYPE_C=''CHANGE''',6,10706,'Primary Conductor Node Symbol - Change KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010107,10101,'Primary Conductor Node Symbol',' TYPE_C=''CHANGE''',7,10707,'Primary Conductor Node Symbol - Change default');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010108,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 4.1 and TYPE_C=''DEADEND''',8,10708,'Primary Conductor Node Symbol - Dead End  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010109,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 12.5 and TYPE_C=''DEADEND''',9,10709,'Primary Conductor Node Symbol - Dead End KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010110,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 13.2 and TYPE_C=''DEADEND''',10,10710,'Primary Conductor Node Symbol - Dead End KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010111,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 21.6 and TYPE_C=''DEADEND''',11,10711,'Primary Conductor Node Symbol - Dead End KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010112,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 24.9 and TYPE_C=''DEADEND''',12,10712,'Primary Conductor Node Symbol - Dead End KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010113,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 33 and TYPE_C=''DEADEND''',13,10713,'Primary Conductor Node Symbol - Dead End KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010114,10101,'Primary Conductor Node Symbol','TYPE_C=''DEADEND''',14,10714,'Primary Conductor Node Symbol - Dead End default');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010115,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 4.1 and TYPE_C=''JUMPOVER''',15,10715,'Primary Conductor Node Symbol - Jumpover  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010116,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 12.5 and TYPE_C=''JUMPOVER''',16,10716,'Primary Conductor Node Symbol - Jumpover KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010117,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 13.2 and TYPE_C=''JUMPOVER''',17,10717,'Primary Conductor Node Symbol - Jumpover KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010118,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 21.6 and TYPE_C=''JUMPOVER''',18,10718,'Primary Conductor Node Symbol - Jumpover KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010119,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 24.9 and TYPE_C=''JUMPOVER''',19,10719,'Primary Conductor Node Symbol - Jumpover KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010120,10101,'Primary Conductor Node Symbol','VOLT_1_Q = 33 and TYPE_C=''JUMPOVER''',20,10720,'Primary Conductor Node Symbol - Jumpover KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010121,10101,'Primary Conductor Node Symbol','TYPE_C=''JUMPOVER''',21,10721,'Primary Conductor Node Symbol - Jumpover  default');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1010199,10101,'Primary Conductor Node Symbol','',99,10799,'Primary Conductor Node Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020101,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 4.1 and TYPE_C=''CHANGE''',1,10701,'Primary Conductor Node Symbol - Change  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020102,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 12.5 and TYPE_C=''CHANGE''',2,10702,'Primary Conductor Node Symbol - Change KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020103,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 13.2 and TYPE_C=''CHANGE''',3,10703,'Primary Conductor Node Symbol - Change KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020104,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 21.6 and TYPE_C=''CHANGE''',4,10704,'Primary Conductor Node Symbol - Change KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020105,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 24.9 and TYPE_C=''CHANGE''',5,10705,'Primary Conductor Node Symbol - Change KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020106,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 33 and TYPE_C=''CHANGE''',6,10706,'Primary Conductor Node Symbol - Change KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020107,10201,'Primary Conductor Node Symbol - OMS',' TYPE_C=''CHANGE''',7,10707,'Primary Conductor Node Symbol - Change default');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020108,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 4.1 and TYPE_C=''DEADEND''',8,10708,'Primary Conductor Node Symbol - Dead End  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020109,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 12.5 and TYPE_C=''DEADEND''',9,10709,'Primary Conductor Node Symbol - Dead End KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020110,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 13.2 and TYPE_C=''DEADEND''',10,10710,'Primary Conductor Node Symbol - Dead End KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020111,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 21.6 and TYPE_C=''DEADEND''',11,10711,'Primary Conductor Node Symbol - Dead End KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020112,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 24.9 and TYPE_C=''DEADEND''',12,10712,'Primary Conductor Node Symbol - Dead End KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020113,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 33 and TYPE_C=''DEADEND''',13,10713,'Primary Conductor Node Symbol - Dead End KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020114,10201,'Primary Conductor Node Symbol - OMS','TYPE_C=''DEADEND''',14,10714,'Primary Conductor Node Symbol - Dead End default');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020115,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 4.1 and TYPE_C=''JUMPOVER''',15,10715,'Primary Conductor Node Symbol - Jumpover  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020116,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 12.5 and TYPE_C=''JUMPOVER''',16,10716,'Primary Conductor Node Symbol - Jumpover KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020117,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 13.2 and TYPE_C=''JUMPOVER''',17,10717,'Primary Conductor Node Symbol - Jumpover KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020118,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 21.6 and TYPE_C=''JUMPOVER''',18,10718,'Primary Conductor Node Symbol - Jumpover KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020119,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 24.9 and TYPE_C=''JUMPOVER''',19,10719,'Primary Conductor Node Symbol - Jumpover KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020120,10201,'Primary Conductor Node Symbol - OMS','VOLT_1_Q = 33 and TYPE_C=''JUMPOVER''',20,10720,'Primary Conductor Node Symbol - Jumpover KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020121,10201,'Primary Conductor Node Symbol - OMS','TYPE_C=''JUMPOVER''',21,10721,'Primary Conductor Node Symbol - Jumpover  default');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (1020199,10201,'Primary Conductor Node Symbol - OMS','',99,10799,'Primary Conductor Node Symbol - default');


spool off;
