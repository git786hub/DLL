ALTER TABLE GT_PLOT_REDLINES
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_REDLINES CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_REDLINES
(
  RL_NO              NUMBER,
  GROUP_NO           NUMBER,
  RL_DATATYPE        VARCHAR2(30 BYTE),
  RL_COORDINATE_X1   NUMBER,
  RL_COORDINATE_Y1   NUMBER,
  RL_COORDINATE_X2   NUMBER,
  RL_COORDINATE_Y2   NUMBER,
  RL_STYLE_NUMBER    NUMBER,
  RL_TEXT_ALIGNMENT  NUMBER                     DEFAULT 0,
  RL_ROTATION        NUMBER                     DEFAULT 0,
  RL_TEXT            VARCHAR2(255 BYTE),
  RL_USERINPUT       NUMBER                     DEFAULT 0,
  RL_NAME            VARCHAR2(255 BYTE),
  RL_TEXT_FR         VARCHAR2(255 BYTE)
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


CREATE UNIQUE INDEX PK_GT_PLOT_REDLINES_RL_NO ON GT_PLOT_REDLINES
(RL_NO)
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


CREATE OR REPLACE TRIGGER M_T_BIUR_GT_PLOT_REDLINES
BEFORE INSERT OR UPDATE
ON GT_PLOT_REDLINES
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
    raise_application_error ( -20001, 'A reference to GT_PLOT_REDLINES.GROUP_NO=' || :NEW.GROUP_NO || ' does not exists in GT_PLOT_GROUPS_DRI.GROUP_NO');
  WHEN OTHERS THEN
    RAISE;
END M_T_BIUR_GT_PLOT_REDLINES;
/


DROP PUBLIC SYNONYM GT_PLOT_REDLINES;

CREATE PUBLIC SYNONYM GT_PLOT_REDLINES FOR GT_PLOT_REDLINES;


ALTER TABLE GT_PLOT_REDLINES ADD (
  CONSTRAINT PK_GT_PLOT_REDLINES_RL_NO
 PRIMARY KEY
 (RL_NO)
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

GRANT SELECT ON GT_PLOT_REDLINES TO DESIGNER;

GRANT SELECT ON GT_PLOT_REDLINES TO FINANCE;

GRANT SELECT ON GT_PLOT_REDLINES TO MARKETING;

GRANT SELECT ON GT_PLOT_REDLINES TO PUBLIC;


SET DEFINE OFF;
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001001, 40001, 'Redline Lines', 0, 0, 
    70, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001002, 40001, 'Redline Lines', 0, 7, 
    70, 7, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001003, 40001, 'Redline Lines', 0, 14, 
    70, 14, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001004, 40001, 'Redline Lines', 0, 21, 
    70, 21, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001005, 40001, 'Redline Lines', 0, 28, 
    70, 28, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001006, 40001, 'Redline Lines', 4, 32, 
    70, 32, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001007, 40001, 'Redline Lines', 4, 36, 
    70, 36, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001008, 40001, 'Redline Lines', 0, 40, 
    70, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001009, 40001, 'Redline Lines', 0, 0, 
    0, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001010, 40001, 'Redline Lines', 4, 28, 
    4, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001011, 40001, 'Redline Lines', 16, 32, 
    16, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001012, 40001, 'Redline Lines', 35, 7, 
    35, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001013, 40001, 'Redline Lines', 47, 32, 
    47, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001014, 40001, 'Redline Lines', 70, 0, 
    70, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001101, 40001, 'Redline Text', 0.1, 3.5, 
    NULL, NULL, 10005, 1, 0, 
    'SOURCE DOCUMENT INFORMATION', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001102, 40001, 'Redline Text', 0.5, 9, 
    NULL, NULL, 10001, 1, 0, 
    'Qualified Individual:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001103, 40001, 'Redline Text', 0.5, 16, 
    NULL, NULL, 10001, 1, 0, 
    'Welder / Fuser:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001104, 40001, 'Redline Text', 0.5, 23, 
    NULL, NULL, 10001, 1, 0, 
    'In-Service Date:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001105, 40001, 'Redline Text', 35.5, 9, 
    NULL, NULL, 10001, 1, 0, 
    'Pipeline Certificate No:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001106, 40001, 'Redline Text', 35.5, 16, 
    NULL, NULL, 10001, 1, 0, 
    'Ticket Number:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001107, 40001, 'Redline Text', 35.5, 23, 
    NULL, NULL, 10001, 1, 0, 
    'G-Tech Update By:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001108, 40001, 'Redline Text', 35.5, 26, 
    NULL, NULL, 10001, 1, 0, 
    'Date:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001109, 40001, 'Redline Text', 2, 39, 
    NULL, NULL, 10001, 1, 90, 
    'Testing', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001110, 40001, 'Redline Text', 4.5, 30, 
    NULL, NULL, 10001, 1, 0, 
    'Design Pressure:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001111, 40001, 'Redline Text', 35.5, 30, 
    NULL, NULL, 10001, 1, 0, 
    'Test Device:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001112, 40001, 'Redline Text', 4.5, 34, 
    NULL, NULL, 10001, 1, 0, 
    'Time On:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001113, 40001, 'Redline Text', 35.5, 34, 
    NULL, NULL, 10001, 1, 0, 
    'Pressure:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001114, 40001, 'Redline Text', 4.5, 38, 
    NULL, NULL, 10001, 1, 0, 
    'Time Off:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40001115, 40001, 'Redline Text', 35.5, 38, 
    NULL, NULL, 10001, 1, 0, 
    'Pressure:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002001, 40002, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002002, 40002, 'Redline Lines', 0, 7, 
    105, 7, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002003, 40002, 'Redline Lines', 0, 14, 
    105, 14, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002004, 40002, 'Redline Lines', 0, 21, 
    105, 21, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002005, 40002, 'Redline Lines', 0, 28, 
    105, 28, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002006, 40002, 'Redline Lines', 4, 32, 
    105, 32, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002007, 40002, 'Redline Lines', 4, 36, 
    105, 36, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002008, 40002, 'Redline Lines', 0, 40, 
    105, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002009, 40002, 'Redline Lines', 0, 0, 
    0, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002010, 40002, 'Redline Lines', 4, 28, 
    4, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002011, 40002, 'Redline Lines', 16, 32, 
    16, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002012, 40002, 'Redline Lines', 52.5, 7, 
    52.5, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002013, 40002, 'Redline Lines', 64.5, 32, 
    64.5, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002014, 40002, 'Redline Lines', 105, 0, 
    105, 40, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002101, 40002, 'Redline Text', 18, 3.5, 
    NULL, NULL, 10005, 1, 0, 
    'SOURCE DOCUMENT INFORMATION', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002102, 40002, 'Redline Text', 0.5, 9, 
    NULL, NULL, 10001, 1, 0, 
    'Qualified Individual:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002103, 40002, 'Redline Text', 0.5, 16, 
    NULL, NULL, 10001, 1, 0, 
    'Welder / Fuser:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002104, 40002, 'Redline Text', 0.5, 23, 
    NULL, NULL, 10001, 1, 0, 
    'In-Service Date:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002105, 40002, 'Redline Text', 53, 9, 
    NULL, NULL, 10001, 1, 0, 
    'Pipeline Certificate No:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002106, 40002, 'Redline Text', 53, 16, 
    NULL, NULL, 10001, 1, 0, 
    'Ticket Number:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002107, 40002, 'Redline Text', 53, 23, 
    NULL, NULL, 10001, 1, 0, 
    'G-Tech Update By:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002108, 40002, 'Redline Text', 53, 26, 
    NULL, NULL, 10001, 1, 0, 
    'Date:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002109, 40002, 'Redline Text', 2, 39, 
    NULL, NULL, 10001, 1, 90, 
    'Testing', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002110, 40002, 'Redline Text', 4.5, 30, 
    NULL, NULL, 10001, 1, 0, 
    'Design Pressure:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002111, 40002, 'Redline Text', 53, 30, 
    NULL, NULL, 10001, 1, 0, 
    'Test Device:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002112, 40002, 'Redline Text', 4.5, 34, 
    NULL, NULL, 10001, 1, 0, 
    'Time On:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002113, 40002, 'Redline Text', 53, 34, 
    NULL, NULL, 10001, 1, 0, 
    'Pressure:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002114, 40002, 'Redline Text', 4.5, 38, 
    NULL, NULL, 10001, 1, 0, 
    'Time Off:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (40002115, 40002, 'Redline Text', 53, 38, 
    NULL, NULL, 10001, 1, 0, 
    'Pressure:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007007, 20007, 'Redline Lines', 0, 54, 
    70, 54, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007008, 20007, 'Redline Lines', 0, 59, 
    70, 59, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007009, 20007, 'Redline Lines', 0, 64, 
    200.5, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007010, 20007, 'Redline Lines', 159, 55, 
    200.5, 55, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007011, 20007, 'Redline Lines', 0, 0, 
    0, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007012, 20007, 'Redline Lines', 70, 0, 
    70, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007013, 20007, 'Redline Lines', 159, 0, 
    159, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007014, 20007, 'Redline Lines', 8, 49, 
    8, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007015, 20007, 'Redline Lines', 13, 49, 
    13, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007016, 20007, 'Redline Lines', 20, 49, 
    20, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006106, 20006, 'Redline Text', 0.5, 45.25, 
    NULL, NULL, 10001, 1, 0, 
    'Title :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006100, 20006, 'Redline Text', 42, 3.5, 
    NULL, NULL, 10005, 1, 0, 
    'REVISIONS', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006101, 20006, 'Redline Text', 0.5, 8.5, 
    NULL, NULL, 10001, 1, 0, 
    'Date', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006102, 20006, 'Redline Text', 10, 8.5, 
    NULL, NULL, 10001, 1, 0, 
    'By', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006103, 20006, 'Redline Text', 14, 8.5, 
    NULL, NULL, 10001, 1, 0, 
    'App''d', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006104, 20006, 'Redline Text', 52, 8.5, 
    NULL, NULL, 10001, 1, 0, 
    'Remarks', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006105, 20006, 'Redline Text', 24, 38.5, 
    NULL, NULL, 10005, 1, 0, 
    'AS -', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006107, 20006, 'Redline Text', 13.5, 49.5, 
    NULL, NULL, 10005, 1, 0, 
    '[JOB_MODIFY_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006108, 20006, 'Redline Text', 0.5, 56, 
    NULL, NULL, 10001, 1, 0, 
    'Description :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006109, 20006, 'Redline Text', 21, 56, 
    NULL, NULL, 10000, 1, 0, 
    '[DESCRIPTION]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006110, 20006, 'Redline Text', 0.5, 95.5, 
    NULL, NULL, 10001, 1, 0, 
    'Project # :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006111, 20006, 'Redline Text', 2.5, 98, 
    NULL, NULL, 10000, 1, 0, 
    '[PROJECT_NUM]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006112, 20006, 'Redline Text', 35.5, 95.5, 
    NULL, NULL, 10001, 1, 0, 
    'CARS Ref # :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006113, 20006, 'Redline Text', 37.5, 98, 
    NULL, NULL, 10000, 1, 0, 
    '[CARS_NUMBER]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006114, 20006, 'Redline Text', 70.5, 95.5, 
    NULL, NULL, 10001, 1, 0, 
    'D.M.W.O. # :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006115, 20006, 'Redline Text', 72.5, 98, 
    NULL, NULL, 10000, 1, 0, 
    '[DMWO_NUMBER]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006123, 20006, 'Redline Text', 2.5, 113, 
    NULL, NULL, 10000, 1, 0, 
    '[USER_ID]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006124, 20006, 'Redline Text', 35.5, 110.5, 
    NULL, NULL, 10001, 1, 0, 
    'Date Drawn :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006125, 20006, 'Redline Text', 37.5, 113, 
    NULL, NULL, 10000, 1, 0, 
    '[SYSDATE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006126, 20006, 'Redline Text', 70.5, 110.5, 
    NULL, NULL, 10001, 1, 0, 
    'Corrosion:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006127, 20006, 'Redline Text', 0.5, 118, 
    NULL, NULL, 10001, 1, 0, 
    'Scale :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006128, 20006, 'Redline Text', 2.5, 120.5, 
    NULL, NULL, 10000, 1, 0, 
    '[SCALE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006116, 20006, 'Redline Text', 0.5, 103, 
    NULL, NULL, 10001, 1, 0, 
    'District :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006117, 20006, 'Redline Text', 2.5, 105.5, 
    NULL, NULL, 10000, 1, 0, 
    '[REGION_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006118, 20006, 'Redline Text', 35.5, 103, 
    NULL, NULL, 10001, 1, 0, 
    'Municipality :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006119, 20006, 'Redline Text', 37.5, 105.5, 
    NULL, NULL, 10000, 1, 0, 
    '[MUNICIPALITY_TYPE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006120, 20006, 'Redline Text', 37.5, 108, 
    NULL, NULL, 10000, 1, 0, 
    '[MUNICIPALITY_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006121, 20006, 'Redline Text', 70.5, 103, 
    NULL, NULL, 10001, 1, 0, 
    'Autherized:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006122, 20006, 'Redline Text', 0.5, 110.5, 
    NULL, NULL, 10001, 1, 0, 
    'Drawn By :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006129, 20006, 'Redline Text', 70.5, 118.5, 
    NULL, NULL, 10003, 1, 0, 
    'Drawing Number', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006130, 20006, 'Redline Text', 78, 122, 
    NULL, NULL, 10003, 1, 0, 
    '[SHEET_NUM]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006131, 20006, 'Redline Text', 84, 122, 
    NULL, NULL, 10003, 1, 0, 
    'OF', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006132, 20006, 'Redline Text', 92, 122, 
    NULL, NULL, 10003, 1, 0, 
    '[TOTAL_SHEETS]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001001, 20001, 'Redline Lines', 0, 7.5, 
    122, 7.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001002, 20001, 'Redline Lines', 0, 0, 
    182, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001003, 20001, 'Redline Lines', 182, 0, 
    182, 25.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001004, 20001, 'Redline Lines', 0, 23, 
    182, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001005, 20001, 'Redline Lines', 60, 15, 
    182, 15, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001006, 20001, 'Redline Lines', 182, 25.5, 
    0, 25.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001007, 20001, 'Redline Lines', 60, 0, 
    60, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001008, 20001, 'Redline Lines', 0, 25.5, 
    0, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001009, 20001, 'Redline Lines', 152, 15, 
    152, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001010, 20001, 'Redline Lines', 122, 0, 
    122, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001011, 20001, 'Redline Lines', 29, 0, 
    29, 7.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001012, 20001, 'Redline Text', 1, 1.5, 
    NULL, NULL, 5034, 1, 0, 
    'ISSUE DATE', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001013, 20001, 'Redline Text', 123, 1.5, 
    NULL, NULL, 5034, 1, 0, 
    'SIGNATURE', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001014, 20001, 'Redline Text', 61, 16.5, 
    NULL, NULL, 5034, 1, 0, 
    'CLLI', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001015, 20001, 'Redline Text', 153, 16.5, 
    NULL, NULL, 5034, 1, 0, 
    'PLAN', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001016, 20001, 'Redline Text', 61, 20, 
    NULL, NULL, 5035, 1, 0, 
    '[SWITCH_CENTRE_CLLI]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001017, 20001, 'Redline Text', 123, 12.3, 
    NULL, NULL, 5035, 1, 0, 
    '[ORIGINATOR]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001018, 20001, 'Redline Text', 61, 12.3, 
    NULL, NULL, 5035, 1, 0, 
    '[ROW_NUMBER]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001019, 20001, 'Redline Text', 90, 20, 
    NULL, NULL, 5035, 1, 0, 
    '[SWITCH_CENTRE_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001020, 20001, 'Redline Text', 153, 20, 
    NULL, NULL, 5036, 1, 0, 
    '[PLAN_ID]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001021, 20001, 'Redline Text', 123, 20, 
    NULL, NULL, 5036, 1, 0, 
    '[WORK_ORDER_ID]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001022, 20001, 'Redline Text', 123, 9, 
    NULL, NULL, 5034, 1, 0, 
    'ORIGINATOR', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001023, 20001, 'Redline Text', 30, 5, 
    NULL, NULL, 5034, 1, 0, 
    '[REISSUE_DATE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001024, 20001, 'Redline Text', 1, 5, 
    NULL, NULL, 5034, 1, 0, 
    '[ISSUE_DATE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001025, 20001, 'Redline Text', 80, 24, 
    NULL, NULL, 5034, 1, 0, 
    'Copyright Intergraph', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001026, 20001, 'Redline Text', 90, 16.5, 
    NULL, NULL, 5034, 1, 0, 
    'SC', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001027, 20001, 'Redline Text', 61, 9, 
    NULL, NULL, 5034, 1, 0, 
    'R/W', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001028, 20001, 'Redline Text', 30, 1.5, 
    NULL, NULL, 5034, 1, 0, 
    'REISSUE DATE', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001029, 20001, 'Redline Text', 61, 5, 
    NULL, NULL, 5035, 1, 0, 
    '[ASSOCIATE_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001030, 20001, 'Redline Text', 61, 1.5, 
    NULL, NULL, 5034, 1, 0, 
    'ASSOCIATE', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001031, 20001, 'Redline Text', 123, 16.5, 
    NULL, NULL, 5034, 1, 0, 
    'NETWORK#', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20001032, 20001, 'Redline Text', 1, 15, 
    NULL, NULL, 5034, 1, 0, 
    'ENGINEERING: This plan represents a Letter of Instruction.
The work will be completed according to cost/requirements
established in the Intergraph-Vendor contracts and conform
 to schedules determined by the Intergraph.', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006001, 20006, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006002, 20006, 'Redline Lines', 0, 7, 
    105, 7, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006003, 20006, 'Redline Lines', 0, 11, 
    105, 11, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006004, 20006, 'Redline Lines', 0, 15, 
    105, 15, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006005, 20006, 'Redline Lines', 0, 19, 
    105, 19, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006006, 20006, 'Redline Lines', 0, 23, 
    105, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006007, 20006, 'Redline Lines', 0, 35.5, 
    105, 35.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006008, 20006, 'Redline Lines', 0, 43.75, 
    105, 43.75, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006009, 20006, 'Redline Lines', 0, 54.5, 
    105, 54.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006010, 20006, 'Redline Lines', 0, 94, 
    105, 94, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006011, 20006, 'Redline Lines', 0, 101.5, 
    105, 101.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006012, 20006, 'Redline Lines', 0, 109, 
    105, 109, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006013, 20006, 'Redline Lines', 0, 116.5, 
    105, 116.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006014, 20006, 'Redline Lines', 0, 124, 
    105, 124, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006015, 20006, 'Redline Lines', 0, 0, 
    0, 124, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006016, 20006, 'Redline Lines', 9.5, 7, 
    9.5, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006017, 20006, 'Redline Lines', 13.5, 7, 
    13.5, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006018, 20006, 'Redline Lines', 20.75, 7, 
    20.75, 23, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006019, 20006, 'Redline Lines', 35, 94, 
    35, 124, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006020, 20006, 'Redline Lines', 70, 94, 
    70, 124, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20006021, 20006, 'Redline Lines', 105, 0, 
    105, 124, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20010001, 20010, 'Redline Lines', 0, 0, 
    0, 263.525, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20010002, 20010, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20010003, 20010, 'Redline Lines', 0, 263.525, 
    105, 263.525, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20010004, 20010, 'Redline Lines', 105, 0, 
    105, 263.525, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20011001, 20011, 'Redline Lines', 0, 0, 
    0, 415.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20011002, 20011, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20011003, 20011, 'Redline Lines', 0, 415.925, 
    105, 415.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20011004, 20011, 'Redline Lines', 105, 0, 
    105, 415.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20012001, 20012, 'Redline Lines', 0, 0, 
    0, 542.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20012002, 20012, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20012003, 20012, 'Redline Lines', 0, 542.925, 
    105, 542.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20012004, 20012, 'Redline Lines', 105, 0, 
    105, 542.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20013001, 20013, 'Redline Lines', 0, 0, 
    0, 847.725, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20013002, 20013, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20013003, 20013, 'Redline Lines', 0, 847.725, 
    105, 847.725, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20013004, 20013, 'Redline Lines', 105, 0, 
    105, 847.725, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20014001, 20014, 'Redline Lines', 0, 0, 
    0, 200.025, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20014002, 20014, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20014003, 20014, 'Redline Lines', 0, 200.025, 
    105, 200.025, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20014004, 20014, 'Redline Lines', 105, 0, 
    105, 200.025, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20015001, 20015, 'Redline Lines', 0, 0, 
    0, 415.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20015002, 20015, 'Redline Lines', 0, 0, 
    105, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20015003, 20015, 'Redline Lines', 0, 415.925, 
    105, 415.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20015004, 20015, 'Redline Lines', 105, 0, 
    105, 415.925, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007017, 20007, 'Redline Lines', 200.5, 0, 
    200.5, 64, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007100, 20007, 'Redline Text', 23, 46, 
    NULL, NULL, 10005, 1, 0, 
    'REVISIONS', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007101, 20007, 'Redline Text', 0.5, 50.5, 
    NULL, NULL, 10001, 1, 0, 
    'Date', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007102, 20007, 'Redline Text', 8.5, 50.5, 
    NULL, NULL, 10001, 1, 0, 
    'By', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007103, 20007, 'Redline Text', 13.5, 50.5, 
    NULL, NULL, 10001, 1, 0, 
    'App''d', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007104, 20007, 'Redline Text', 37.5, 50.5, 
    NULL, NULL, 10001, 1, 0, 
    'Remarks', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007105, 20007, 'Redline Text', 84, 14, 
    NULL, NULL, 10005, 1, 0, 
    'AS -', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007106, 20007, 'Redline Text', 70.5, 19.5, 
    NULL, NULL, 10001, 1, 0, 
    'Title :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007107, 20007, 'Redline Text', 79, 24.75, 
    NULL, NULL, 10005, 1, 0, 
    '[JOB_MODIFY_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007108, 20007, 'Redline Text', 70.5, 33, 
    NULL, NULL, 10001, 1, 0, 
    'Description :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007109, 20007, 'Redline Text', 84, 33, 
    NULL, NULL, 10000, 1, 0, 
    '[DESCRIPTION]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007110, 20007, 'Redline Text', 159.5, 1.5, 
    NULL, NULL, 10001, 1, 0, 
    'Project # :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007111, 20007, 'Redline Text', 173, 1.5, 
    NULL, NULL, 10000, 1, 0, 
    '[PROJECT_NUM]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007112, 20007, 'Redline Text', 159.5, 5.5, 
    NULL, NULL, 10001, 1, 0, 
    'CARS Ref # :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007113, 20007, 'Redline Text', 173, 5.5, 
    NULL, NULL, 10000, 1, 0, 
    '[CARS_NUMBER]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007114, 20007, 'Redline Text', 159.5, 9.5, 
    NULL, NULL, 10001, 1, 0, 
    'D.M.W.O. # :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007115, 20007, 'Redline Text', 173, 9.5, 
    NULL, NULL, 10000, 1, 0, 
    '[DMWO_NUMBER]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007116, 20007, 'Redline Text', 159.5, 13.5, 
    NULL, NULL, 10001, 1, 0, 
    'District :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007117, 20007, 'Redline Text', 173, 13.5, 
    NULL, NULL, 10000, 1, 0, 
    '[REGION_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007118, 20007, 'Redline Text', 159.5, 17.5, 
    NULL, NULL, 10001, 1, 0, 
    'Municipality :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007119, 20007, 'Redline Text', 173, 17.5, 
    NULL, NULL, 10000, 1, 0, 
    '[MUNICIPALITY_TYPE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007120, 20007, 'Redline Text', 173, 20, 
    NULL, NULL, 10000, 1, 0, 
    '[MUNICIPALITY_NAME]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007121, 20007, 'Redline Text', 159.5, 24, 
    NULL, NULL, 10001, 1, 0, 
    'Drawn By :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007122, 20007, 'Redline Text', 173, 24, 
    NULL, NULL, 10000, 1, 0, 
    '[USER_ID]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007123, 20007, 'Redline Text', 159.5, 28, 
    NULL, NULL, 10001, 1, 0, 
    'Date Drawn :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007124, 20007, 'Redline Text', 173, 28, 
    NULL, NULL, 10000, 1, 0, 
    '[SYSDATE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007125, 20007, 'Redline Text', 159.5, 32, 
    NULL, NULL, 10001, 1, 0, 
    'Scale :', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007126, 20007, 'Redline Text', 173, 32, 
    NULL, NULL, 10000, 1, 0, 
    '[SCALE]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007127, 20007, 'Redline Text', 159.5, 36, 
    NULL, NULL, 10001, 1, 0, 
    'Autherized:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007128, 20007, 'Redline Text', 159.5, 40, 
    NULL, NULL, 10001, 1, 0, 
    'Corrosion:', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007129, 20007, 'Redline Text', 159.5, 57, 
    NULL, NULL, 10003, 1, 0, 
    'Drawing Number', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007130, 20007, 'Redline Text', 172, 61, 
    NULL, NULL, 10003, 1, 0, 
    '[SHEET_NUM]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007131, 20007, 'Redline Text', 178, 61, 
    NULL, NULL, 10003, 1, 0, 
    'OF', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007132, 20007, 'Redline Text', 185, 61, 
    NULL, NULL, 10003, 1, 0, 
    '[TOTAL_SHEETS]', 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007001, 20007, 'Redline Lines', 0, 0, 
    200.5, 0, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007002, 20007, 'Redline Lines', 70, 11, 
    159, 11, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007003, 20007, 'Redline Lines', 70, 18, 
    159, 18, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007004, 20007, 'Redline Lines', 70, 31.5, 
    159, 31.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007005, 20007, 'Redline Lines', 0, 42.5, 
    70, 42.5, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
Insert into GT_PLOT_REDLINES
   (rl_no, group_no, rl_datatype, rl_coordinate_x1, rl_coordinate_y1, 
    rl_coordinate_x2, rl_coordinate_y2, rl_style_number, rl_text_alignment, rl_rotation, 
    rl_text, rl_userinput, rl_name, rl_text_fr)
 Values
   (20007006, 20007, 'Redline Lines', 0, 49, 
    70, 49, 27302111, 0, 0, 
    NULL, 0, NULL, NULL);
COMMIT;
