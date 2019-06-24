set echo on
set linesize 1000
set pagesize 300
set trimspool on
SPOOL PREMISE_NBR_UPDATE.LOG 

ALTER TABLE B$PREMISE_N DISABLE ALL TRIGGERS;


update B$PREMISE_N p 
  set p.PREMISE_NBR = LPAD(PREMISE_NBR,10,'0')
      where P.PREMISE_NBR <> '0';
 
commit;  

ALTER TABLE B$PREMISE_N ENABLE ALL TRIGGERS;

spool off;