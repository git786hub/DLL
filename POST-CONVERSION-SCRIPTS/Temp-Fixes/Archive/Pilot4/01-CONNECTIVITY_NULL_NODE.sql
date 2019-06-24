set echo on
set linesize 1000
set pagesize 300
set trimspool on
SPOOL CONNECTIVITY_NULL_NODE.LOG


alter table B$CONNECTIVITY_N DISABLE ALL TRIGGERS;
 
    --
    -- Update null nodes to 0
    --
    
      UPDATE B$CONNECTIVITY_N 
        SET NODE_1_ID = decode(NODE_1_ID,null,0,NODE_1_ID),
        NODE_2_ID = decode(NODE_2_ID,null,0,NODE_2_ID)
      WHERE NODE_1_ID IS NULL or NODE_2_ID IS NULL;
      
      
alter table B$CONNECTIVITY_N ENABLE ALL TRIGGERS;

commit;



spool off;