set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\AddIdentityToSTLT_BOUNDARY.sql.log
--**************************************************************************************
--SCRIPT NAME: JobMgmt_CUFKQArguments.sql
--**************************************************************************************
-- AUTHOR			: Barry Scott
-- DATE				: 17-AUG-2018
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Make STLT_BOUNDARY.BND_CLASS an IDENTITY column.
--                : Design calls for this column to auto-generate the IDs.
--                : Note: This process involves removing the foreign key constraint from
--                :       STLT_ACCOUNT, creating a temporary boundary table, recreating
--                :       STLT_BOUNDARY, updating STLT_ACCOUNT with the new BND_CLASS
--                :       values, and dropping the extra objects.
--**************************************************************************************
-- Modified:
--**************************************************************************************

-- Drop the FK constraint on STLT_ACCOUNT to allow manipulating the reference table
alter table gis_onc.stlt_account drop constraint fk_stlt_account_boundary_class;

-- Create a temporary table and add/populate a column containing the existing class IDs
create table gis_onc.stlt_boundary_bak as select * from gis_onc.stlt_boundary;

alter table gis_onc.stlt_boundary_bak add bnd_class_orig number(5);

update gis_onc.stlt_boundary_bak set bnd_class_orig=bnd_class;

-- Recreate STLT_BOUNDARY using an IDENTITY column
-- plus an additional column to track the original BND_CLASS values
drop public synonym stlt_boundary;

drop table gis_onc.stlt_boundary;

create table gis_onc.stlt_boundary
(
	bnd_class number(5) generated always as identity,
	bnd_fno number(5)	not null,
	bnd_type_ano number(10),		
	bnd_type varchar2(80),		
	bnd_id_ano number(10)	not null,
	bnd_class_orig number(5)
);

alter table gis_onc.stlt_boundary add constraint pk_stlt_boundary primary key(bnd_class);
alter table gis_onc.stlt_boundary add constraint fk_stlt_boundary_bnd_fno foreign key(bnd_fno) references gis.g3e_feature(g3e_fno);
alter table gis_onc.stlt_boundary add constraint fk_stlt_boundary_bnd_type_ano foreign key(bnd_type_ano) references gis.g3e_attribute(g3e_ano);
alter table gis_onc.stlt_boundary add constraint fk_stlt_boundary_bnd_id_ano foreign key(bnd_id_ano) references gis.g3e_attribute(g3e_ano);

create public synonym stlt_boundary for gis_onc.stlt_boundary;

-- In case the master grant script is not run...
grant select,insert,update,delete on gis_onc.stlt_boundary to priv_mgmt_stlt;

-- Populate the new table with the values from the backup table.
-- BND_CLASS_ORIG will contain the original BND_CLASS values.
-- These will be used to update STLT_ACCOUNT with the new BND_CLASS values.
insert into gis_onc.stlt_boundary
columns(bnd_fno,bnd_type_ano,bnd_type,bnd_id_ano,bnd_class_orig)
select bnd_fno,bnd_type_ano,bnd_type,bnd_id_ano,bnd_class from gis_onc.stlt_boundary_bak;

-- Update SLT_ACCOUNT with the new BOUNDARY_CLASS values
update gis_onc.stlt_account
set boundary_class=(select bnd_class from gis_onc.stlt_boundary where gis_onc.stlt_boundary.bnd_class_orig=gis_onc.stlt_account.boundary_class);

commit;

-- Drop the extra objects.
alter table gis_onc.stlt_boundary drop column bnd_class_orig;
drop table gis_onc.stlt_boundary_bak;

-- Add the constraint back that was dropped.
alter table gis_onc.stlt_account add constraint fk_stlt_account_boundary_class foreign key (boundary_class) references gis_onc.stlt_boundary(bnd_class);

spool off;