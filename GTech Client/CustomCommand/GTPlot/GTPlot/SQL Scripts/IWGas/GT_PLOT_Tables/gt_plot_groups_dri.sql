ALTER TABLE GT_PLOT_GROUPS_DRI
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_GROUPS_DRI CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_GROUPS_DRI
(
  GROUP_NO        NUMBER,
  GROUP_NAME      VARCHAR2(255 BYTE),
  DRI_ID          NUMBER,
  USERPLACE       NUMBER                        DEFAULT 0,
  GROUP_OFFSET_X  NUMBER,
  GROUP_OFFSET_Y  NUMBER
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


CREATE UNIQUE INDEX PK_GT_PLOT_GROUPS_DRI ON GT_PLOT_GROUPS_DRI
(DRI_ID, GROUP_NO)
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


DROP PUBLIC SYNONYM GT_PLOT_GROUPS_DRI;

CREATE PUBLIC SYNONYM GT_PLOT_GROUPS_DRI FOR GT_PLOT_GROUPS_DRI;


ALTER TABLE GT_PLOT_GROUPS_DRI ADD (
  CONSTRAINT PK_GT_PLOT_GROUPS_DRI
 PRIMARY KEY
 (DRI_ID, GROUP_NO)
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

ALTER TABLE GT_PLOT_GROUPS_DRI ADD (
  CONSTRAINT FK_GT_PLOT_GROUPS_DRI_DRI_ID 
 FOREIGN KEY (DRI_ID) 
 REFERENCES GT_PLOT_DRAWINGINFO (DRI_ID));

GRANT SELECT ON GT_PLOT_GROUPS_DRI TO DESIGNER;

GRANT SELECT ON GT_PLOT_GROUPS_DRI TO FINANCE;

GRANT SELECT ON GT_PLOT_GROUPS_DRI TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_GROUPS_DRI TO PUBLIC;


SET DEFINE OFF;
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'A-Size Portrait  - Job Plot - Redline - Source Document Info', 10002, 0, 7.5, 
    206);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'B-Size Portrait  - Job Plot - Redline - Source Document Info', 30002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'C-Size Portrait  - Job Plot - Redline - Source Document Info', 50002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'D-Size Portrait  - Job Plot - Redline - Source Document Info', 70002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'E-Size Portrait  - Job Plot - Redline - Source Document Info', 90002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'F-Size Portrait  - Job Plot - Redline - Source Document Info', 110002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40001, 'G-Size Portrait  - Job Plot - Redline - Source Document Info', 130002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'A-Size Landscape - Job Plot - Redline - Source Document Info', 20002, 0, 0, 
    60);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'B-Size Landscape - Job Plot - Redline - Source Document Info', 40002, 0, 314.325, 
    107.4625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'C-Size Landscape - Job Plot - Redline - Source Document Info', 60002, 0, 447.675, 
    258.925);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'D-Size Landscape - Job Plot - Redline - Source Document Info', 80002, 0, 752.475, 
    386.8625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'E-Size Landscape - Job Plot - Redline - Source Document Info', 100002, 0, 1006.475, 
    691.6625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'F-Size Landscape - Job Plot - Redline - Source Document Info', 120002, 0, 650.875, 
    43.9625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (40002, 'G-Size Landscape - Job Plot - Redline - Source Document Info', 140002, 0, 752.475, 
    259.8625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20010, 'B-Size Landscape - Job Plot - Redline - Drawing Info Frame', 40002, 0, 314.325, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10001, 'A-Size Portrait  - Map Frame', 10000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10002, 'A-Size Landscape - Map Frame', 20000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10003, 'B-Size Portrait  - Map Frame', 30000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10004, 'B-Size Landscape - Map Frame', 40000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10005, 'C-Size Portrait  - Map Frame', 50000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10006, 'C-Size Landscape - Map Frame', 60000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10007, 'D-Size Portrait  - Map Frame', 70000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10008, 'D-Size Landscape - Map Frame', 80000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10009, 'E-Size Portrait  - Map Frame', 90000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10010, 'E-Size Landscape - Map Frame', 100000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10011, 'F-Size Portrait  - Map Frame', 110000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10012, 'F-Size Landscape - Map Frame', 120000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10013, 'G-Size Portrait  - Map Frame', 130000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10014, 'G-Size Landscape - Map Frame', 140000, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10101, 'A-Size Portrait  - Test - Map Frame', 10001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10102, 'A-Size Landscape - Test - Map Frame', 20001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10103, 'B-Size Portrait  - Test - Map Frame', 30001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10104, 'B-Size Landscape - Test - Map Frame', 40001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10105, 'C-Size Portrait  - Test - Map Frame', 50001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10106, 'C-Size Landscape - Test - Map Frame', 60001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10107, 'D-Size Portrait  - Test - Map Frame', 70001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10108, 'D-Size Landscape - Test - Map Frame', 80001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10109, 'E-Size Portrait  - Test - Map Frame', 90001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10110, 'E-Size Landscape - Test - Map Frame', 100001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10111, 'F-Size Portrait  - Test - Map Frame', 110001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10112, 'F-Size Landscape - Test - Map Frame', 120001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10113, 'G-Size Portrait  - Test - Map Frame', 130001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (10114, 'G-Size Landscape - Test - Map Frame', 140001, 1, 0, 
    0);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'A-Size Landscape - Test - Redline - Drawing Info', 20001, 0, 87.4, 
    180.4);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'A-Size Portrait  - Test - Redline - Drawing Info', 10001, 0, 23.9, 
    243.9);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'B-Size Landscape - Test - Redline - Drawing Info', 40001, 0, 239.8, 
    243.9);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'B-Size Portrait  - Test - Redline - Drawing Info', 30001, 0, 87.4, 
    396.3);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'C-Size Landscape - Test - Redline - Drawing Info', 60001, 0, 366.8, 
    396.3);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'C-Size Portrait  - Test - Redline - Drawing Info', 50001, 0, 239.8, 
    523.3);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'D-Size Landscape - Test - Redline - Drawing Info', 80001, 0, 671.6, 
    523.3);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'D-Size Portrait  - Test - Redline - Drawing Info', 70001, 0, 366.8, 
    828.1);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'E-Size Landscape - Test - Redline - Drawing Info', 100001, 0, 925.6, 
    828.1);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'E-Size Portrait  - Test - Redline - Drawing Info', 90001, 0, 671.6, 
    1082.1);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'F-Size Landscape - Test - Redline - Drawing Info', 120001, 0, 100, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'F-Size Portrait  - Test - Redline - Drawing Info', 110001, 0, 100, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'G-Size Landscape - Test - Redline - Drawing Info', 140001, 0, 100, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'G-Size Portrait  - Test - Redline - Drawing Info', 130001, 0, 100, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20101, 'A-Size Portrait  - Job Plot - Map Frame', 10002, 1, 7.5, 
    7.5);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20102, 'A-Size Landscape - Job Plot - Map Frame', 20002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20103, 'B-Size Portrait  - Job Plot - Map Frame', 30002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20104, 'B-Size Landscape - Job Plot - Map Frame', 40002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20105, 'C-Size Portrait  - Job Plot - Map Frame', 50002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20106, 'C-Size Landscape - Job Plot - Map Frame', 60002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20107, 'D-Size Portrait  - Job Plot - Map Frame', 70002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20108, 'D-Size Landscape - Job Plot - Map Frame', 80002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20109, 'E-Size Portrait  - Job Plot - Map Frame', 90002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20110, 'E-Size Landscape - Job Plot - Map Frame', 100002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20111, 'F-Size Portrait  - Job Plot - Map Frame', 110002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20112, 'F-Size Landscape - Job Plot - Map Frame', 120002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20113, 'G-Size Portrait  - Job Plot - Map Frame', 130002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20114, 'G-Size Landscape - Job Plot - Map Frame', 140002, 1, 7.9375, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'A-Size Landscape - Job Plot - Redline - Drawing Info', 20002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20007, 'A-Size Portrait  - Job Plot - Redline - Drawing Info', 10002, 0, 7.5, 
    206);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20006, 'B-Size Landscape - Job Plot - Redline - Drawing Info', 40002, 0, 314.325, 
    147.4625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'B-Size Portrait  - Job Plot - Redline - Drawing Info', 30002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20006, 'C-Size Landscape - Job Plot - Redline - Drawing Info', 60002, 0, 447.675, 
    298.925);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'C-Size Portrait  - Job Plot - Redline - Drawing Info', 50002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20006, 'D-Size Landscape - Job Plot - Redline - Drawing Info', 80002, 0, 752.475, 
    426.8625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'D-Size Portrait  - Job Plot - Redline - Drawing Info', 70002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20006, 'E-Size Landscape - Job Plot - Redline - Drawing Info', 100002, 0, 1006.475, 
    731.6625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'E-Size Portrait  - Job Plot - Redline - Drawing Info', 90002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20006, 'F-Size Landscape - Job Plot - Redline - Drawing Info', 120002, 0, 650.875, 
    83.9625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'F-Size Portrait  - Job Plot - Redline - Drawing Info', 110002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20006, 'G-Size Landscape - Job Plot - Redline - Drawing Info', 140002, 0, 752.475, 
    299.8625);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20001, 'G-Size Portrait  - Job Plot - Redline - Drawing Info', 130002, 0, 0, 
    100);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (30001, 'B-Size Landscape - Job Plot - Map Frame Keymap', 40002, 0, 316.325, 
    9.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (30001, 'C-Size Landscape - Job Plot - Map Frame Keymap', 60002, 0, 449.675, 
    9.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (30001, 'D-Size Landscape - Job Plot - Map Frame Keymap', 80002, 0, 754.475, 
    9.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (30001, 'E-Size Landscape - Job Plot - Map Frame Keymap', 100002, 0, 1008.475, 
    9.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (30001, 'G-Size Landscape - Job Plot - Map Frame Keymap', 140002, 0, 754.475, 
    9.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20011, 'C-Size Landscape - Job Plot - Redline - Drawing Info Frame', 60002, 0, 447.675, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20012, 'D-Size Landscape - Job Plot - Redline - Drawing Info Frame', 80002, 0, 752.475, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20013, 'E-Size Landscape - Job Plot - Redline - Drawing Info Frame', 100002, 0, 1006.475, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20014, 'F-Size Landscape - Job Plot - Redline - Drawing Info Frame', 120002, 0, 650.875, 
    7.9375);
Insert into GT_PLOT_GROUPS_DRI
   (group_no, group_name, dri_id, userplace, group_offset_x, 
    group_offset_y)
 Values
   (20015, 'G-Size Landscape - Job Plot - Redline - Drawing Info Frame', 140002, 0, 752.475, 
    7.9375);
COMMIT;
