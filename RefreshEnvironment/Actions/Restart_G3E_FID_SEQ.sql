spool RESTART_G3E_FID_SEQ_&SDATE..log

SET TERMOUT OFF

DECLARE
    nMaxCount NUMBER;
    nCount NUMBER;
    nNewFID NUMBER;
begin
    nMaxCount:=0;
    for cur in (select table_name from user_tables where table_name like 'B$%') LOOP
        execute immediate ( 'select max(g3e_fid)  from ' ||  cur.table_name) into nCount;
        if (nCount>nMaxCount) then
            nMaxCount := nCount;
            dbms_output.put_line('NEW max fid - ' || nMaxCount || ' found in ' || cur.table_name);
        end if;
    end loop;
    dbms_output.put_line('Currently Used Max FID is ' || nMaxCount);
    nNewFID := nMaxCount + 1;
    EXECUTE immediate('ALTER SEQUENCE gis.g3e_fid_seq RESTART START WITH ' || nNewFID); 
    dbms_output.put_line('G3E_FID_SEQ has been restarted to begin with ' || nNewFID);
end;
/
