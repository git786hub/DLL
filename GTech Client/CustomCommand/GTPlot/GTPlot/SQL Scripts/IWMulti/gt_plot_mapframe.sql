ALTER TABLE GT_PLOT_MAPFRAME
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_MAPFRAME CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_MAPFRAME
(
  MF_NO                     NUMBER                  NULL,
  GROUP_NO                  NUMBER                  NULL,
  MF_DATATYPE               VARCHAR2(30 BYTE)       NULL,
  MF_NAME                   VARCHAR2(255 BYTE)      NULL,
  MF_GEO_LNO                NUMBER(5)               NULL,
  MF_DET_LNO                NUMBER(5)               NULL,
  MF_COORDINATE_X1          NUMBER                  NULL,
  MF_COORDINATE_Y1          NUMBER                  NULL,
  MF_COORDINATE_X2          NUMBER                  NULL,
  MF_COORDINATE_Y2          NUMBER                  NULL,
  MF_NORTHARROW_SIZE        NUMBER(5)               NULL,
  MF_NORTHARROW_SYMBOLFILE  VARCHAR2(255 BYTE)      NULL,
  MF_STYLE_NO               NUMBER                  NULL,
  MF_DISPLAY_FACTOR         NUMBER                  NULL
)
TABLESPACE GTECH
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE UNIQUE INDEX PK_GT_PLOT_MAPFRAME_MF_NO ON GT_PLOT_MAPFRAME
(MF_NO)
LOGGING
TABLESPACE GTECH
NOPARALLEL;


CREATE OR REPLACE TRIGGER "M_T_BIUR_GT_PLOT_MAPFRAME" 
BEFORE INSERT OR UPDATE
ON GT_PLOT_MAPFRAME
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
    raise_application_error ( -20001, 'A reference to GT_PLOT_MAPFRAME.GROUP_NO=' || :NEW.GROUP_NO || ' does not exists in GT_PLOT_GROUPS_DRI.GROUP_NO');
  WHEN OTHERS THEN
    RAISE;
END M_T_BIUR_GT_PLOT_MAPFRAME;
/
SHOW ERRORS;


ALTER TABLE GT_PLOT_MAPFRAME ADD (
  CONSTRAINT PK_GT_PLOT_MAPFRAME_MF_NO
 PRIMARY KEY
 (MF_NO)
    USING INDEX 
    TABLESPACE GTECH);

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_MAPFRAME TO ADMINISTRATOR;

GRANT SELECT ON GT_PLOT_MAPFRAME TO DESIGNER;

GRANT SELECT ON GT_PLOT_MAPFRAME TO FINANCE;

GRANT SELECT ON GT_PLOT_MAPFRAME TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_MAPFRAME TO SUPERVISOR;


SET DEFINE OFF;
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010001, 10001, 'Map Frame', 'A-Size Portrait', 1, 
    1, 10, 10, 205.9, 269.4, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010002, 10002, 'Map Frame', 'A-Size Landscape', 1, 
    1, 10, 10, 269.4, 205.9, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010003, 10003, 'Map Frame', 'B-Size Portrait', 1, 
    1, 10, 10, 269.4, 421.8, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010004, 10004, 'Map Frame', 'B-Size Landscape', 1, 
    1, 10, 10, 421.8, 269.4, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010005, 10005, 'Map Frame', 'C-Size Portrait', 1, 
    1, 10, 10, 421.8, 548.8, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010006, 10006, 'Map Frame', 'C-Size Landscape', 1, 
    1, 10, 10, 548.8, 421.8, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010007, 10007, 'Map Frame', 'D-Size Portrait', 1, 
    1, 10, 10, 548.8, 853.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010008, 10008, 'Map Frame', 'D-Size Landscape', 1, 
    1, 10, 10, 853.6, 548.8, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010009, 10009, 'Map Frame', 'E-Size Portrait', 1, 
    1, 10, 10, 853.6, 1107.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010010, 10010, 'Map Frame', 'E-Size Landscape', 1, 
    1, 10, 10, 1107.6, 853.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010011, 10011, 'Map Frame', 'F-Size Portrait', 1, 
    1, 10, 10, 205.9, 752, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010012, 10012, 'Map Frame', 'F-Size Landscape', 1, 
    1, 10, 10, 752, 205.9, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010013, 10013, 'Map Frame', 'G-Size Portrait', 1, 
    1, 10, 10, 421.8, 853.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010014, 10014, 'Map Frame', 'G-Size Landscape', 1, 
    1, 10, 10, 853.6, 421.8, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010101, 10101, 'Map Frame', 'A-Size Portrait (Test)', 1, 
    1, 10, 10, 205.9, 243.9, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010102, 10102, 'Map Frame', 'A-Size Landscape (Test)', 1, 
    1, 10, 10, 269.4, 180.4, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010103, 10103, 'Map Frame', 'B-Size Portrait (Test)', 1, 
    1, 10, 10, 269.4, 396.3, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010104, 10104, 'Map Frame', 'B-Size Landscape (Test)', 1, 
    1, 10, 10, 298.45, 243.9, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010105, 10105, 'Map Frame', 'C-Size Portrait (Test)', 1, 
    1, 10, 10, 421.8, 523.3, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010106, 10106, 'Map Frame', 'C-Size Landscape (Test)', 1, 
    1, 10, 10, 548.8, 396.3, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010107, 10107, 'Map Frame', 'D-Size Portrait (Test)', 1, 
    1, 10, 10, 548.8, 828.1, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010108, 10108, 'Map Frame', 'D-Size Landscape (Test)', 1, 
    1, 10, 10, 853.6, 523.3, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010109, 10109, 'Map Frame', 'E-Size Portrait (Test)', 1, 
    1, 10, 10, 853.6, 1082.1, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010110, 10110, 'Map Frame', 'E-Size Landscape (Test)', 1, 
    1, 10, 10, 1107.6, 828.1, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010111, 10111, 'Map Frame', 'F-Size Portrait (Test)', 1, 
    1, 10, 10, 200.025, 635, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010112, 10112, 'Map Frame', 'F-Size Landscape (Test)', 1, 
    1, 10, 10, 635, 200.025, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010113, 10113, 'Map Frame', 'G-Size Portrait (Test)', 1, 
    1, 10, 10, 415.925, 736.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10010114, 10114, 'Map Frame', 'G-Size Landscape (Test)', 1, 
    1, 10, 10, 736.6, 415.925, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020101, 20101, 'Map Frame', 'A-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 200.5, 198, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020102, 20102, 'Map Frame', 'A-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 187.325, 187.325, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020103, 20103, 'Map Frame', 'B-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 263.525, 263.525, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020104, 20104, 'Map Frame', 'B-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 298.45, 263.525, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020105, 20105, 'Map Frame', 'C-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 415.925, 431.8, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020106, 20106, 'Map Frame', 'C-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 431.8, 415.925, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020107, 20107, 'Map Frame', 'D-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 542.925, 736.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020108, 20108, 'Map Frame', 'D-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 736.6, 542.925, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020109, 20109, 'Map Frame', 'E-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 847.725, 990.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020110, 20110, 'Map Frame', 'E-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 990.6, 847.725, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020111, 20111, 'Map Frame', 'F-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 200.025, 635, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020112, 20112, 'Map Frame', 'F-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 635, 200.025, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020113, 20113, 'Map Frame', 'G-Size Portrait (Job Plot)', 1, 
    1, 0, 0, 415.925, 736.6, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10020114, 20114, 'Map Frame', 'G-Size Landscape (Job Plot)', 1, 
    1, 0, 0, 736.6, 415.925, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10030001, 30001, 'Key Map', 'Keymap (Job Plot)', 1, 
    1, 0, 0, 101, 76, 
    30, NULL, 27302135, -2.5);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10040001, 40001, 'Map Frame', 'A-Size Portrait (CU Print)', 1, 
    1, 5, 5, 125.5, 248.9, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10040002, 40002, 'Map Frame', 'A-Size Landscape (CU Print)', 1, 
    1, 10, 10, 184.7, 180.4, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10050001, 50001, 'Map Frame', 'CP-Size Portrait (CU Print)', 1, 
    1, 5, 5, 125.5, 325.1, 
    30, 'arrow1.wmf', NULL, NULL);
Insert into GT_PLOT_MAPFRAME
   (MF_NO, GROUP_NO, MF_DATATYPE, MF_NAME, MF_GEO_LNO, 
    MF_DET_LNO, MF_COORDINATE_X1, MF_COORDINATE_Y1, MF_COORDINATE_X2, MF_COORDINATE_Y2, 
    MF_NORTHARROW_SIZE, MF_NORTHARROW_SYMBOLFILE, MF_STYLE_NO, MF_DISPLAY_FACTOR)
 Values
   (10050002, 50002, 'Map Frame', 'CP-Size Landscape (CU Print)', 1, 
    1, 10, 10, 260.4, 180.4, 
    30, 'arrow1.wmf', NULL, NULL);
COMMIT;
