set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\TemporaryStreetLightAccountValueListUpdates.sql.log
--**************************************************************************************
-- SCRIPT NAME: TemporaryStreetLightAccountValueListUpdates.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\CBSCOTT
-- DATE				    : 17-JUL-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Insert value list data to support Street Light Account Editor
--                  that has incomplete or invalid defaulted converted data.
--**************************************************************************************

delete from stlt_owner_vl where owner_code='UNK';

insert into stlt_owner_vl
columns(owner_code,owner_name)
values('UNK','Unknown');

delete from stlt_desc_vl where description_id=0;

insert into stlt_desc_vl
columns(description_id,description,msla_date)
values(0,'Unknown',null);

delete from vl_stlt_lum_style where vl_key='UNK';

insert into vl_stlt_lum_style
columns(vl_key,vl_value,unused,ordinal)
values('UNK','Unknown',0,5);

commit;

spool off;