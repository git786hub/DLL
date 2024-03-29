SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL SEC_NODE_CHANGE_JUMP_DELETE.LOG


ALTER TABLE B$SEC_COND_NODE_N DISABLE ALL TRIGGERS;
ALTER TABLE B$SEC_COND_NODE_S DISABLE ALL TRIGGERS;
ALTER TABLE B$SEC_COND_NODE_T DISABLE ALL TRIGGERS;
ALTER TABLE B$COMMON_N DISABLE ALL TRIGGERS;
ALTER TABLE B$CONNECTIVITY_N DISABLE ALL TRIGGERS;

DELETE FROM B$CONNECTIVITY_N
WHERE G3E_FNO = 162
AND G3E_FID NOT IN
(
    SELECT G3E_FID
    FROM B$SEC_COND_NODE_N
    WHERE TYPE_C = 'DEADEND'
) ;

DELETE FROM B$COMMON_N
WHERE G3E_FNO = 162
AND G3E_FID NOT IN
(
    SELECT G3E_FID
    FROM B$SEC_COND_NODE_N
    WHERE TYPE_C = 'DEADEND'
) ;

DELETE FROM B$SEC_COND_NODE_N
WHERE G3E_FID NOT IN 
(
    SELECT G3E_FID
    FROM B$SEC_COND_NODE_N
    WHERE TYPE_C = 'DEADEND'
) ;

DELETE FROM B$SEC_COND_NODE_S
WHERE G3E_FID NOT IN 
(
    SELECT G3E_FID
    FROM B$SEC_COND_NODE_N
    WHERE TYPE_C = 'DEADEND'
) ;

DELETE FROM B$SEC_COND_NODE_T
WHERE G3E_FID NOT IN 
(
    SELECT G3E_FID
    FROM B$SEC_COND_NODE_N
    WHERE TYPE_C = 'DEADEND'
) ;

DELETE FROM B$SEC_COND_NODE_S
WHERE G3E_CID > 1 ;

DELETE FROM B$SEC_COND_NODE_T
WHERE G3E_CID > 1 ;

DELETE FROM B$SEC_COND_NODE_N
WHERE G3E_CID > 1 ;

UPDATE B$SEC_COND_NODE_N
SET TYPE_C = 'DEADEND' 
WHERE TYPE_C <> 'DEADEND' ;

COMMIT;

ALTER TABLE B$SEC_COND_NODE_N ENABLE ALL TRIGGERS;
ALTER TABLE B$SEC_COND_NODE_S ENABLE ALL TRIGGERS;
ALTER TABLE B$SEC_COND_NODE_T ENABLE ALL TRIGGERS;
ALTER TABLE B$COMMON_N ENABLE ALL TRIGGERS;
ALTER TABLE B$CONNECTIVITY_N ENABLE ALL TRIGGERS;

SPOOL OFF;