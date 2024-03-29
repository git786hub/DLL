SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL SUPER_NODE_TEMP_FIX.LOG

-- This change is being made because these three node values are connected
-- to more than 3000 features.

ALTER TABLE B$CONNECTIVITY_N DISABLE ALL TRIGGERS;

UPDATE B$CONNECTIVITY_N SET NODE_1_ID=0 WHERE NODE_1_ID=1611502932;
UPDATE B$CONNECTIVITY_N SET NODE_2_ID=0 WHERE NODE_2_ID=1611502932;

UPDATE B$CONNECTIVITY_N SET NODE_1_ID=0 WHERE NODE_1_ID=1591636420;
UPDATE B$CONNECTIVITY_N SET NODE_2_ID=0 WHERE NODE_2_ID=1591636420;

UPDATE B$CONNECTIVITY_N SET NODE_1_ID=0 WHERE NODE_1_ID=1651492981;
UPDATE B$CONNECTIVITY_N SET NODE_2_ID=0 WHERE NODE_2_ID=1651492981;

COMMIT;

ALTER TABLE B$CONNECTIVITY_N ENABLE ALL TRIGGERS;

SPOOL OFF
