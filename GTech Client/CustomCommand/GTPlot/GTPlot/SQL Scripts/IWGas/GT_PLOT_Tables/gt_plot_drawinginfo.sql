ALTER TABLE GT_PLOT_DRAWINGINFO
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_DRAWINGINFO CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_DRAWINGINFO
(
  DRI_ID                NUMBER,
  SHEET_ID              NUMBER,
  DRI_TYPE              VARCHAR2(255 BYTE),
  DRI_NAME              VARCHAR2(255 BYTE),
  DRI_SCALES            VARCHAR2(255 BYTE),
  SHEET_INSET           NUMBER,
  SHEET_INSET_STYLE_NO  NUMBER
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


CREATE UNIQUE INDEX PK_GT_PLOT_DRAWINGINFO_DRI_ID ON GT_PLOT_DRAWINGINFO
(DRI_ID)
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


DROP PUBLIC SYNONYM GT_PLOT_DRAWINGINFO;

CREATE PUBLIC SYNONYM GT_PLOT_DRAWINGINFO FOR GT_PLOT_DRAWINGINFO;


ALTER TABLE GT_PLOT_DRAWINGINFO ADD (
  CONSTRAINT PK_GT_PLOT_DRAWINGINFO_DRI_ID
 PRIMARY KEY
 (DRI_ID)
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

GRANT SELECT ON GT_PLOT_DRAWINGINFO TO DESIGNER;

GRANT SELECT ON GT_PLOT_DRAWINGINFO TO FINANCE;

GRANT SELECT ON GT_PLOT_DRAWINGINFO TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_DRAWINGINFO TO PUBLIC;


SET DEFINE OFF;
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (10000, 1, NULL, 'A-Size Portrait', '1:250,1:500,1:750,1:1000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (20000, 2, NULL, 'A-Size Landscape', '1:250,1:500,1:750,1:1000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (30000, 3, NULL, 'B-Size Portrait', '1:500,1:750,1:1000,1:1250,1:1500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (40000, 4, NULL, 'B-Size Landscape', '1:500,1:750,1:1000,1:1250,1:1500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (50000, 5, NULL, 'C-Size Portrait', '1:500,1:750,1:1000,1:1250,1:1500,1:2000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (60000, 6, NULL, 'C-Size Landscape', '1:500,1:750,1:1000,1:1250,1:1500,1:2000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (70000, 7, NULL, 'D-Size Portrait', '1:750,1:1000,1:1250,1:1500,1:2000,1:2500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (80000, 8, NULL, 'D-Size Landscape', '1:750,1:1000,1:1250,1:1500,1:2000,1:2500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (90000, 9, NULL, 'E-Size Portrait', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (100000, 10, NULL, 'E-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (110000, 11, NULL, 'F-Size Portrait', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (120000, 12, NULL, 'F-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (130000, 13, NULL, 'G-Size Portrait', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (140000, 14, NULL, 'G-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (10001, 1, 'Test', 'A-Size Portrait', '1:250,1:500,1:750,1:1000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (20001, 2, 'Test', 'A-Size Landscape', '1:250,1:500,1:750,1:1000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (30001, 3, 'Test', 'B-Size Portrait', '1:500,1:750,1:1000,1:1250,1:1500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (40001, 4, 'Test', 'B-Size Landscape', '1:500,1:750,1:1000,1:1250,1:1500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (50001, 5, 'Test', 'C-Size Portrait', '1:500,1:750,1:1000,1:1250,1:1500,1:2000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (60001, 6, 'Test', 'C-Size Landscape', '1:500,1:750,1:1000,1:1250,1:1500,1:2000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (70001, 7, 'Test', 'D-Size Portrait', '1:750,1:1000,1:1250,1:1500,1:2000,1:2500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (80001, 8, 'Test', 'D-Size Landscape', '1:750,1:1000,1:1250,1:1500,1:2000,1:2500', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (90001, 9, 'Test', 'E-Size Portrait', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (100001, 10, 'Test', 'E-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (110001, 11, 'Test', 'F-Size Portrait', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (120001, 12, 'Test', 'F-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (130001, 13, 'Test', 'G-Size Portrait', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (140001, 14, 'Test', 'G-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    10, 27302111);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (10002, 1, 'Job Plot', 'A-Size Portrait', '1:250,1:500,1:750,1:1000', 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (20002, 2, 'Job Plot', 'A-Size Landscape', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (30002, 3, 'Job Plot', 'B-Size Portrait', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (40002, 4, 'Job Plot', 'B-Size Landscape', '1:500,1:750,1:1000,1:1250,1:1500', 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (50002, 5, 'Job Plot', 'C-Size Portrait', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (60002, 6, 'Job Plot', 'C-Size Landscape', '1:500,1:750,1:1000,1:1250,1:1500,1:2000', 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (70002, 7, 'Job Plot', 'D-Size Portrait', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (80002, 8, 'Job Plot', 'D-Size Landscape', '1:750,1:1000,1:1250,1:1500,1:2000,1:2500', 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (90002, 9, 'Job Plot', 'E-Size Portrait', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (100002, 10, 'Job Plot', 'E-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (110002, 11, 'Job Plot', 'F-Size Portrait', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (120002, 12, 'Job Plot', 'F-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (130002, 13, 'Job Plot', 'G-Size Portrait', NULL, 
    NULL, NULL);
Insert into GT_PLOT_DRAWINGINFO
   (dri_id, sheet_id, dri_type, dri_name, dri_scales, 
    sheet_inset, sheet_inset_style_no)
 Values
   (140002, 14, 'Job Plot', 'G-Size Landscape', '1:1000,1:1250,1:1500,1:2000,1:2500,1:3600,1:5000', 
    NULL, NULL);
COMMIT;
