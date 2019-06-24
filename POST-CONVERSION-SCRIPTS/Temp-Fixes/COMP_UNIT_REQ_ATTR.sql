SET ECHO ON
SET LINESIZE 1000
SET PAGESIZE 300
SET TRIMSPOOL ON
SPOOL COMP_UNIT_REQ_ATTR.LOG

ALTER TABLE GIS.B$COMP_UNIT_N DISABLE ALL TRIGGERS;


UPDATE GIS.B$COMP_UNIT_N CU
  SET CU.WR_ID = DECODE (CU.WR_ID,NULL,'UNK',CU.WR_ID)
  WHERE WR_ID IS NULL;

UPDATE GIS.B$COMP_UNIT_N CU
  SET CU.QTY_LENGTH_Q    = DECODE (CU.QTY_LENGTH_Q,NULL,80,CU.QTY_LENGTH_Q)
  WHERE CU.QTY_LENGTH_Q IS NULL
    AND CU.G3E_FNO       = 54
    AND Cu.G3e_Cno = 21;

COMMIT;    

ALTER TABLE GIS.B$COMP_UNIT_N ENABLE ALL TRIGGERS;

SPOOL OFF;



