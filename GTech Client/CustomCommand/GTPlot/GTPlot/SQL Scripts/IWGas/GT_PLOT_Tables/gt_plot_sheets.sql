ALTER TABLE GT_PLOT_SHEETS
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_SHEETS CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_SHEETS
(
  SHEET_ID           NUMBER,
  SHEET_NAME         VARCHAR2(255 BYTE),
  SHEET_HEIGHT       NUMBER,
  SHEET_WIDTH        NUMBER,
  SHEET_SIZE         VARCHAR2(30 BYTE),
  SHEET_ORIENTATION  VARCHAR2(30 BYTE)
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


CREATE UNIQUE INDEX PK_GT_PLOT_SHEETS_SHEET_ID ON GT_PLOT_SHEETS
(SHEET_ID)
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


DROP PUBLIC SYNONYM GT_PLOT_SHEETS;

CREATE PUBLIC SYNONYM GT_PLOT_SHEETS FOR GT_PLOT_SHEETS;


ALTER TABLE GT_PLOT_SHEETS ADD (
  CONSTRAINT PK_GT_PLOT_SHEETS_SHEET_ID
 PRIMARY KEY
 (SHEET_ID)
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

GRANT SELECT ON GT_PLOT_SHEETS TO DESIGNER;

GRANT SELECT ON GT_PLOT_SHEETS TO FINANCE;

GRANT SELECT ON GT_PLOT_SHEETS TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_SHEETS TO PUBLIC;


SET DEFINE OFF;
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (11, 'F', 762, 215.9, '8.5 x 30', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (12, 'F', 215.9, 762, '8.5 x 30', 
    'Landscape');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (13, 'G', 863.6, 431.8, '17 x 34', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (14, 'G', 431.8, 863.6, '17 x 34', 
    'Landscape');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (1, 'A', 279.4, 215.9, '8.5 x 11', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (2, 'A', 215.9, 279.4, '8.5 x 11', 
    'Landscape');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (3, 'B', 431.8, 279.4, '11 x 17', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (4, 'B', 279.4, 431.8, '11 x 17', 
    'Landscape');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (5, 'C', 558.8, 431.8, '17 x 22', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (6, 'C', 431.8, 558.8, '17 x 22', 
    'Landscape');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (7, 'D', 863.6, 558.8, '22 x 34', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (8, 'D', 558.8, 863.6, '22 x 34', 
    'Landscape');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (9, 'E', 1117.6, 863.6, '34 x 44', 
    'Portrait');
Insert into GT_PLOT_SHEETS
   (sheet_id, sheet_name, sheet_height, sheet_width, sheet_size, 
    sheet_orientation)
 Values
   (10, 'E', 863.6, 1117.6, '34 x 44', 
    'Landscape');
COMMIT;
