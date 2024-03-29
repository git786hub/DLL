set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\StyleRules_FuseSaver.log
--**************************************************************************************
-- SCRIPT NAME: StyleRules_FuseSaver.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 10-MAR-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Styles and style rules for Fuse Saver
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
    where G3E_RULE in ('Fuse Saver Symbol', 'Fuse Saver Symbol - OMS');
  
  delete from G3E_STYLERULE where G3E_RULE = 'Fuse Saver Symbol';
  delete from G3E_STYLERULE where G3E_RULE = 'Fuse Saver Symbol - OMS';
  
  for i in 1..arrSNO.COUNT loop
    delete from G3E_POINTSTYLE where G3E_SNO = arrSNO(i);
    delete from G3E_STYLE where G3E_SNO = arrSNO(i);
  end loop;
end;
/

alter trigger M_T_AUDR_G3E_STYLERULE_RULE enable;
alter trigger M_T_AUD_G3E_STYLERULE_RULE enable;

-- Point styles

insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37108,'Fuse Saver Symbol - PPI','AEGIS Device',CHR(75),10158079,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37109,'Fuse Saver Symbol - PPR','AEGIS Device',CHR(75),14540253,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37110,'Fuse Saver Symbol - OSR','AEGIS Device',CHR(75),5921370,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37111,'Fuse Saver Symbol -  KV1','AEGIS Device',CHR(75),3956378,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37112,'Fuse Saver Symbol - KV2','AEGIS Device',CHR(75),24285,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37113,'Fuse Saver Symbol - KV3','AEGIS Device',CHR(75),39679,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37114,'Fuse Saver Symbol - KV4','AEGIS Device',CHR(75),8453982,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37115,'Fuse Saver Symbol - KV5','AEGIS Device',CHR(75),39424,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37116,'Fuse Saver Symbol - KV6','AEGIS Device',CHR(75),19200,12,0,0,0,null,0,1);
insert into G3E_POINTSTYLE(G3E_SNO,G3E_USERNAME,G3E_FONTNAME,G3E_SYMBOL,G3E_COLOR,G3E_SIZE,G3E_ALIGNMENT,G3E_ROTATION,G3E_USEMASK,G3E_MASKSYMBOL,G3E_PLOTREDLINE,G3E_STYLEUNITS) values (37199,'Fuse Saver Symbol - default','AEGIS Device',CHR(75),65535,12,0,0,0,null,0,1);


-- Style rules - GIS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710101,37101,'Fuse Saver Symbol','FEATURE_STATE_C in (''PPI'',''ABI'')',1,37108,'Fuse Saver Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710102,37101,'Fuse Saver Symbol','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,37109,'Fuse Saver Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710103,37101,'Fuse Saver Symbol','FEATURE_STATE_C in (''OSR'',''OSA'')',3,37110,'Fuse Saver Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710104,37101,'Fuse Saver Symbol','VOLT_1_Q  = 4.1',4,37111,'Fuse Saver Symbol -  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710105,37101,'Fuse Saver Symbol','VOLT_1_Q = 12.5',5,37112,'Fuse Saver Symbol - KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710106,37101,'Fuse Saver Symbol','VOLT_1_Q = 13.2',6,37113,'Fuse Saver Symbol - KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710107,37101,'Fuse Saver Symbol','VOLT_1_Q = 21.6',7,37114,'Fuse Saver Symbol - KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710108,37101,'Fuse Saver Symbol','VOLT_1_Q = 24.9',8,37115,'Fuse Saver Symbol - KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710109,37101,'Fuse Saver Symbol','VOLT_1_Q = 33',9,37116,'Fuse Saver Symbol - KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3710199,37101,'Fuse Saver Symbol','',99,37199,'Fuse Saver Symbol - default');


-- Style rules - OMS

insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720101,37201,'Fuse Saver Symbol - OMS','FEATURE_STATE_C in (''PPI'',''ABI'')',1,37108,'Fuse Saver Symbol - PPI');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720102,37201,'Fuse Saver Symbol - OMS','FEATURE_STATE_C in (''PPR'',''ABR'',''PPA'',''ABA'')',2,37109,'Fuse Saver Symbol - PPR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720103,37201,'Fuse Saver Symbol - OMS','FEATURE_STATE_C in (''OSR'',''OSA'')',3,37110,'Fuse Saver Symbol - OSR');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720104,37201,'Fuse Saver Symbol - OMS','VOLT_1_Q  = 4.1',4,37111,'Fuse Saver Symbol -  KV1');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720105,37201,'Fuse Saver Symbol - OMS','VOLT_1_Q = 12.5',5,37112,'Fuse Saver Symbol - KV2');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720106,37201,'Fuse Saver Symbol - OMS','VOLT_1_Q = 13.2',6,37113,'Fuse Saver Symbol - KV3');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720107,37201,'Fuse Saver Symbol - OMS','VOLT_1_Q = 21.6',7,37114,'Fuse Saver Symbol - KV4');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720108,37201,'Fuse Saver Symbol - OMS','VOLT_1_Q = 24.9',8,37115,'Fuse Saver Symbol - KV5');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720109,37201,'Fuse Saver Symbol - OMS','VOLT_1_Q = 33',9,37116,'Fuse Saver Symbol - KV6');
insert into G3E_STYLERULE(G3E_SRROWNO,G3E_SRNO,G3E_RULE,G3E_FILTER,G3E_FILTERORDINAL,G3E_SNO,G3E_DESCRIPTION) values (3720199,37201,'Fuse Saver Symbol - OMS','',99,37199,'Fuse Saver Symbol - default');


spool off;
