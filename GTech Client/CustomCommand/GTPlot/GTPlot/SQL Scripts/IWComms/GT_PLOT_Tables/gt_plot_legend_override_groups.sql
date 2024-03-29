DROP TABLE GT_PLOT_LEGEND_OVERRIDE_GROUPS CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_LEGEND_OVERRIDE_GROUPS
(
  LO_GROUP_ID             NUMBER,
  LO_USERNAME             VARCHAR2(255 BYTE),
  DRI_TYPE                VARCHAR2(255 BYTE),
  MF_LNO                  NUMBER(5),
  LO_USER_OPTION          NUMBER                DEFAULT 0,
  LO_USER_OPTION_DEFAULT  NUMBER                DEFAULT 0,
  LO_EDITDATE             DATE                  NOT NULL
)
TABLESPACE GC_DATA_PRODUCT
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


CREATE INDEX PK_GT_PLOT_LO_GRPS_LO_GRP_ID ON GT_PLOT_LEGEND_OVERRIDE_GROUPS
(LO_GROUP_ID)
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


CREATE OR REPLACE TRIGGER M_T_BIUR_GT_PLOT_LEG_OR_GRPS 
BEFORE INSERT OR UPDATE
ON GT_PLOT_LEGEND_OVERRIDE_GROUPS
REFERENCING NEW AS New OLD AS Old
FOR EACH ROW
DECLARE
  refValueDRI_TYPE    GT_PLOT_DRAWINGINFO.DRI_TYPE%TYPE;

  refValue                      NUMBER;
  refValueMF_LNO                GT_PLOT_LEGEND_OVERRIDE_GROUPS.MF_LNO%TYPE;

  invalid_reference             EXCEPTION;
  invalid_reference_2           EXCEPTION;

  CURSOR curReference (newValue GT_PLOT_DRAWINGINFO.DRI_TYPE%TYPE) IS
    SELECT DRI_TYPE
    FROM GT_PLOT_DRAWINGINFO
    WHERE DRI_TYPE = newValue;

  CURSOR curReference_2 (newValue GT_PLOT_DRAWINGINFO.DRI_TYPE%TYPE, newValue2 NUMBER) IS
    SELECT distinct(d.dri_type)
      FROM GT_PLOT_drawinginfo d, GT_PLOT_groups_dri g, GT_PLOT_mapframe m
     WHERE d.dri_type = newValue
       AND d.dri_id = g.dri_id
       AND g.group_no = m.group_no
       AND (mf_geo_lno = newValue2 OR mf_det_lno = newValue2);

 BEGIN

  OPEN curReference(:NEW.DRI_TYPE);
  FETCH curReference INTO refValueDRI_TYPE;
  IF curReference%NOTFOUND THEN
    RAISE invalid_reference;
  END IF;
  CLOSE curReference;

  OPEN curReference_2(:NEW.DRI_TYPE, :NEW.MF_LNO);
  FETCH curReference_2 INTO refValueDRI_TYPE;
  IF curReference_2%NOTFOUND THEN
    RAISE invalid_reference_2;
  END IF;
  CLOSE curReference_2;

EXCEPTION
  WHEN invalid_reference THEN
    CLOSE curReference;
    raise_application_error ( -20001, 'A reference to DRI_TYPE=' || :NEW.DRI_TYPE || ' does not exists in GT_PLOT_DRAWINGINFO.DRI_TYPE');

  WHEN invalid_reference_2 THEN
    CLOSE curReference_2;
    raise_application_error ( -20002, 'Invalid DRI_TYPE=' || :NEW.DRI_TYPE || ' and MF_LNO=' || :NEW.MF_LNO || ' reference.  A match cannot be found for the given GT_PLOT_DRAWINGINFO.DRI_TYPE and GT_PLOT_MAPFRAME.MF_GEO_LNO or MF_DET_LNO combination through the GT_PLOT_GROUPS_DRI table.');

  WHEN OTHERS THEN
    RAISE;
END M_T_BIUR_GT_PLOT_LEG_OR_GRPS;
/


CREATE OR REPLACE TRIGGER M_T_BIUR_GTPLOTLGOVG_EDATE 
BEFORE INSERT OR UPDATE ON GT_PLOT_LEGEND_OVERRIDE_GROUPS
FOR EACH ROW
BEGIN

  :NEW.LO_EDITDATE := SYSDATE;
END;
/


CREATE OR REPLACE PUBLIC SYNONYM GT_PLOT_LEGEND_OVERRIDE_GROUPS FOR GT_PLOT_LEGEND_OVERRIDE_GROUPS;


GRANT SELECT ON GT_PLOT_LEGEND_OVERRIDE_GROUPS TO FINANCE;

GRANT SELECT ON GT_PLOT_LEGEND_OVERRIDE_GROUPS TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_LEGEND_OVERRIDE_GROUPS TO PUBLIC;

