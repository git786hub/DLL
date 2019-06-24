ALTER TABLE GT_PLOT_OBJECTS
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_OBJECTS CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_OBJECTS
(
  OB_NO             NUMBER,
  GROUP_NO          NUMBER,
  OB_DATATYPE       VARCHAR2(30 BYTE),
  OB_NAME           VARCHAR2(255 BYTE),
  OB_FILE           VARCHAR2(255 BYTE),
  OB_COORDINATE_X1  NUMBER,
  OB_COORDINATE_Y1  NUMBER,
  OB_COORDINATE_X2  NUMBER,
  OB_COORDINATE_Y2  NUMBER,
  OB_LINKFILE       NUMBER(1)                   DEFAULT 0
)
TABLESPACE UGAS
PCTUSED    0
PCTFREE    10
INITRANS   1
MAXTRANS   255
STORAGE    (
            INITIAL          64K
            MINEXTENTS       1
            MAXEXTENTS       UNLIMITED
            PCTINCREASE      0
            BUFFER_POOL      DEFAULT
           )
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE UNIQUE INDEX PK_GT_PLOT_OBJECTS_OB_NO ON GT_PLOT_OBJECTS
(OB_NO)
LOGGING
TABLESPACE UGAS
PCTFREE    10
INITRANS   2
MAXTRANS   255
STORAGE    (
            INITIAL          64K
            MINEXTENTS       1
            MAXEXTENTS       UNLIMITED
            PCTINCREASE      0
            BUFFER_POOL      DEFAULT
           )
NOPARALLEL;


CREATE OR REPLACE TRIGGER M_T_BIUR_GT_PLOT_OBJECTS
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


DROP PUBLIC SYNONYM GT_PLOT_OBJECTS;

CREATE PUBLIC SYNONYM GT_PLOT_OBJECTS FOR GT_PLOT_OBJECTS;


ALTER TABLE GT_PLOT_OBJECTS ADD (
  CONSTRAINT PK_GT_PLOT_OBJECTS_OB_NO
 PRIMARY KEY
 (OB_NO)
    USING INDEX 
    TABLESPACE UGAS
    PCTFREE    10
    INITRANS   2
    MAXTRANS   255
    STORAGE    (
                INITIAL          64K
                MINEXTENTS       1
                MAXEXTENTS       UNLIMITED
                PCTINCREASE      0
               ));

GRANT SELECT ON GT_PLOT_OBJECTS TO DESIGNER;

GRANT SELECT ON GT_PLOT_OBJECTS TO FINANCE;

GRANT SELECT ON GT_PLOT_OBJECTS TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_OBJECTS TO PUBLIC;


SET DEFINE OFF;
Insert into GT_PLOT_OBJECTS
   (ob_no, group_no, ob_datatype, ob_name, ob_file, 
    ob_coordinate_x1, ob_coordinate_y1, ob_coordinate_x2, ob_coordinate_y2, ob_linkfile)
 Values
   (10020007, 20007, 'Object', 'Portrait Union Gas Logo', '\\TORCIT\Mapfiles\OTHER\gt_plot_UnionGasLogoAsize.bmp', 
    70.2, 0.1, NULL, NULL, 0);
Insert into GT_PLOT_OBJECTS
   (ob_no, group_no, ob_datatype, ob_name, ob_file, 
    ob_coordinate_x1, ob_coordinate_y1, ob_coordinate_x2, ob_coordinate_y2, ob_linkfile)
 Values
   (10020006, 20006, 'Object', 'Landscape Union Gas Logo', '\\TORCIT\Mapfiles\OTHER\gt_plot_UnionGasLogo.bmp', 
    0.2, 23.1, NULL, NULL, 0);
COMMIT;
