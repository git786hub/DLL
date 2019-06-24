ALTER TABLE GT_PLOT_OBJECTS
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_OBJECTS CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_OBJECTS
(
  OB_NO             NUMBER                          NULL,
  GROUP_NO          NUMBER                          NULL,
  OB_DATATYPE       VARCHAR2(30 BYTE)               NULL,
  OB_NAME           VARCHAR2(255 BYTE)              NULL,
  OB_FILE           VARCHAR2(255 BYTE)              NULL,
  OB_LINKFILE       NUMBER(1)                   DEFAULT 0                         NULL,
  OB_COORDINATE_X1  NUMBER                          NULL,
  OB_COORDINATE_Y1  NUMBER                          NULL,
  OB_COORDINATE_X2  NUMBER                          NULL,
  OB_COORDINATE_Y2  NUMBER                          NULL
)
TABLESPACE GTECH
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE UNIQUE INDEX PK_GT_PLOT_OBJECTS_OB_NO ON GT_PLOT_OBJECTS
(OB_NO)
LOGGING
TABLESPACE GTECH
NOPARALLEL;


CREATE OR REPLACE TRIGGER "M_T_BIUR_GT_PLOT_OBJECTS" 
BEFORE INSERT OR UPDATE
ON GT_PLOT_OBJECTS
REFERENCING NEW AS New OLD AS Old
FOR EACH ROW
DECLARE
  refValue            NUMBER;
  invalid_reference   EXCEPTION;
  CURSOR curReference (newValue NUMBER) IS
    SELECT GROUP_NO
    FROM GT_PLOT_GROUPS_DRI
    WHERE GROUP_NO = newValue;
BEGIN

  OPEN curReference(:NEW.GROUP_NO);
  FETCH curReference INTO refValue;
  IF curReference%NOTFOUND THEN
    RAISE invalid_reference;
  END IF;
  CLOSE curReference;

EXCEPTION
  WHEN invalid_reference THEN
    CLOSE curReference;
    raise_application_error ( -20001, 'A reference to GT_PLOT_OBJECTS.GROUP_NO=' || :NEW.GROUP_NO || ' does not exists in GT_PLOT_GROUPS_DRI.GROUP_NO');
  WHEN OTHERS THEN
    RAISE;
END M_T_BIUR_GT_PLOT_OBJECTS;
/
SHOW ERRORS;


ALTER TABLE GT_PLOT_OBJECTS ADD (
  CONSTRAINT PK_GT_PLOT_OBJECTS_OB_NO
 PRIMARY KEY
 (OB_NO)
    USING INDEX 
    TABLESPACE GTECH);

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_OBJECTS TO ADMINISTRATOR;

GRANT SELECT ON GT_PLOT_OBJECTS TO DESIGNER;

GRANT SELECT ON GT_PLOT_OBJECTS TO FINANCE;

GRANT SELECT ON GT_PLOT_OBJECTS TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_OBJECTS TO SUPERVISOR;


SET DEFINE OFF;
Insert into GT_PLOT_OBJECTS
   (OB_NO, GROUP_NO, OB_DATATYPE, OB_NAME, OB_FILE, 
    OB_LINKFILE, OB_COORDINATE_X1, OB_COORDINATE_Y1, OB_COORDINATE_X2, OB_COORDINATE_Y2)
 Values
   (10020007, 20007, 'Object', 'Portrait Ingr Logo', 'C:\Program Files\Intergraph\GTechnology\Program\gt_plot_Intergraph_logo_Asize.bmp', 
    0, 70.2, 0.1, NULL, NULL);
Insert into GT_PLOT_OBJECTS
   (OB_NO, GROUP_NO, OB_DATATYPE, OB_NAME, OB_FILE, 
    OB_LINKFILE, OB_COORDINATE_X1, OB_COORDINATE_Y1, OB_COORDINATE_X2, OB_COORDINATE_Y2)
 Values
   (10020006, 20006, 'Object', 'Landscape Ingr Logo', 'C:\Program Files\Intergraph\GTechnology\Program\gt_plot_Intergraph_logo.bmp', 
    0, 0.2, 23.1, NULL, NULL);
Insert into GT_PLOT_OBJECTS
   (OB_NO, GROUP_NO, OB_DATATYPE, OB_NAME, OB_FILE, 
    OB_LINKFILE, OB_COORDINATE_X1, OB_COORDINATE_Y1, OB_COORDINATE_X2, OB_COORDINATE_Y2)
 Values
   (10040006, 40006, 'Object', 'A-Size Landscape - CU Print - Excel', 'C:\Program Files\Intergraph\GTechnology\Program\gt_plot_CUPrint.csv', 
    0, 0, 0, NULL, NULL);
Insert into GT_PLOT_OBJECTS
   (OB_NO, GROUP_NO, OB_DATATYPE, OB_NAME, OB_FILE, 
    OB_LINKFILE, OB_COORDINATE_X1, OB_COORDINATE_Y1, OB_COORDINATE_X2, OB_COORDINATE_Y2)
 Values
   (10040005, 40005, 'Object', 'A-Size Portrait - CU Print - Excel', 'C:\Program Files\Intergraph\GTechnology\Program\gt_plot_CUPrint.csv', 
    0, 0, 0, NULL, NULL);
Insert into GT_PLOT_OBJECTS
   (OB_NO, GROUP_NO, OB_DATATYPE, OB_NAME, OB_FILE, 
    OB_LINKFILE, OB_COORDINATE_X1, OB_COORDINATE_Y1, OB_COORDINATE_X2, OB_COORDINATE_Y2)
 Values
   (10050005, 50005, 'Object', 'CP-Size Portrait - CU Print - Excel', 'C:\Program Files\Intergraph\GTechnology\Program\gt_plot_CUPrint.csv', 
    0, 0, 0, NULL, NULL);
Insert into GT_PLOT_OBJECTS
   (OB_NO, GROUP_NO, OB_DATATYPE, OB_NAME, OB_FILE, 
    OB_LINKFILE, OB_COORDINATE_X1, OB_COORDINATE_Y1, OB_COORDINATE_X2, OB_COORDINATE_Y2)
 Values
   (10050006, 50006, 'Object', 'CP-Size Landscape - CU Print - Excel', 'C:\Program Files\Intergraph\GTechnology\Program\gt_plot_CUPrint.csv', 
    0, 0, 0, NULL, NULL);
COMMIT;
