spool Truncate_Pending_Edits_&SDATE..log

DECLARE

begin
    execute immediate 'Truncate table gis.PENDINGEDITS';
end;
/
