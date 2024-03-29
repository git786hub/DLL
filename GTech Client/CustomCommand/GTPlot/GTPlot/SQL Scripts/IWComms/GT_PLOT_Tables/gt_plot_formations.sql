ALTER TABLE GT_PLOT_FORMATIONS
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_FORMATIONS CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_FORMATIONS
(
  FRM_ID           NUMBER                       NOT NULL,
  FRM_NAME         VARCHAR2(80 BYTE)            NOT NULL,
  FRM_NAME_FR      VARCHAR2(80 BYTE)            NOT NULL,
  FRM_VIEW         VARCHAR2(30 BYTE)            NOT NULL,
  FRM_DETAIL       VARCHAR2(30 BYTE)            NOT NULL,
  FRM_DET_LNO      NUMBER(5)                    NOT NULL,
  FRM_FILTER       VARCHAR2(1024 BYTE),
  FRM_DESCRIPTION  VARCHAR2(1024 BYTE),
  FRM_EDITDATE     DATE                         NOT NULL
)
TABLESPACE GC_DATA_PRODUCT
PCTUSED    0
PCTFREE    10
INITRANS   1
MAXTRANS   255
STORAGE    (
            INITIAL          128K
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

COMMENT ON COLUMN GT_PLOT_FORMATIONS.FRM_NAME IS 'Name of item having formations in detail.';



CREATE UNIQUE INDEX PK_GT_PLOT_FORMATIONS_FRM_ID ON GT_PLOT_FORMATIONS
(FRM_ID)
LOGGING
TABLESPACE GC_DATA_PRODUCT
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


CREATE OR REPLACE TRIGGER M_T_BIUR_GTPLOTFORMA_EDATE 
BEFORE INSERT OR UPDATE ON GT_PLOT_FORMATIONS
FOR EACH ROW
BEGIN

  :NEW.FRM_EDITDATE := SYSDATE;
END;
/


CREATE OR REPLACE PUBLIC SYNONYM GT_PLOT_FORMATIONS FOR GT_PLOT_FORMATIONS;


ALTER TABLE GT_PLOT_FORMATIONS ADD (
  CONSTRAINT PK_GT_PLOT_FORMATIONS_FRM_ID
  PRIMARY KEY
  (FRM_ID)
  USING INDEX PK_GT_PLOT_FORMATIONS_FRM_ID
  ENABLE VALIDATE);

GRANT SELECT ON GT_PLOT_FORMATIONS TO FINANCE;

GRANT SELECT ON GT_PLOT_FORMATIONS TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_FORMATIONS TO PUBLIC;

GRANT SELECT ON GT_PLOT_FORMATIONS TO SUPERVISOR2;

Insert into GT_PLOT_FORMATIONS
   (FRM_ID, FRM_NAME, FRM_NAME_FR, FRM_VIEW, FRM_DETAIL, 
    FRM_DET_LNO, FRM_FILTER, FRM_DESCRIPTION, FRM_EDITDATE)
 Values
   (1, 'Handhole', 'Puits de Tirage', 'VGC_MANHL_S', 'GC_DETAIL', 
    2, 'FEATURE_TYPE=''HH''', 'Search within details of Handhole type Manholes', TO_DATE('04/05/2013 22:53:42', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_FORMATIONS
   (FRM_ID, FRM_NAME, FRM_NAME_FR, FRM_VIEW, FRM_DETAIL, 
    FRM_DET_LNO, FRM_FILTER, FRM_DESCRIPTION, FRM_EDITDATE)
 Values
   (2, 'Pedestal', 'Pi�destal', 'VGC_CLOSURE_S', 'GC_DETAIL', 
    2, NULL, 'Search within details of Pedestals', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_FORMATIONS
   (FRM_ID, FRM_NAME, FRM_NAME_FR, FRM_VIEW, FRM_DETAIL, 
    FRM_DET_LNO, FRM_FILTER, FRM_DESCRIPTION, FRM_EDITDATE)
 Values
   (3, 'Passage Connector', 'Connecteur Conduit', 'VGC_PSGCON_S', 'GC_DETAIL', 
    2, NULL, 'Search within details of Passage Connector', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
COMMIT;
