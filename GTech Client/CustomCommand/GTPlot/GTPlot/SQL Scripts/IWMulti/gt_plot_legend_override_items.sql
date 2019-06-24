ALTER TABLE GT_PLOT_LEGEND_OVERRIDE_ITEMS
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_LEGEND_OVERRIDE_ITEMS CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_LEGEND_OVERRIDE_ITEMS
(
  LO_ITEM_NO                NUMBER                  NULL,
  LO_GROUP_ID               NUMBER                  NULL,
  LO_LEGENDITEM             VARCHAR2(80 BYTE)       NULL,
  LO_DISPLAYMODE            NUMBER(1)           DEFAULT 1                         NULL,
  LO_FILTER                 VARCHAR2(240 BYTE)      NULL,
  LO_DISPLAYMODE_UNCHECKED  NUMBER(1)           DEFAULT 1                         NULL,
  LO_FILTER_UNCHECKED       VARCHAR2(240 BYTE)      NULL
)
TABLESPACE GTECH
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE INDEX PK_GT_PLOT_LEG_OVR_ITMS_GRP_ID ON GT_PLOT_LEGEND_OVERRIDE_ITEMS
(LO_GROUP_ID)
LOGGING
TABLESPACE GTECH
NOPARALLEL;


CREATE UNIQUE INDEX PK_GT_PLOT_LEG_OVR_ITMS_ITM_NO ON GT_PLOT_LEGEND_OVERRIDE_ITEMS
(LO_ITEM_NO)
LOGGING
TABLESPACE GTECH
NOPARALLEL;


CREATE OR REPLACE TRIGGER "M_T_BIUR_LEGEND_OVERRIDE_ITEMS" 
BEFORE INSERT OR UPDATE
ON GT_PLOT_LEGEND_OVERRIDE_ITEMS
REFERENCING NEW AS NEW OLD AS OLD
FOR EACH ROW
DECLARE
  refValue                      NUMBER;
  refValueMF_LNO                GT_PLOT_LEGEND_OVERRIDE_GROUPS.MF_LNO%TYPE;
  refValuedisplaycontroltable   G3E_LEGEND.G3E_DISPLAYCONTROLTABLE%TYPE;
  refValueLegendItem            GT_PLOT_LEGEND_OVERRIDE_ITEMS.LO_LEGENDITEM%TYPE;

  invalid_reference             EXCEPTION;
  invalid_referenceDC           EXCEPTION;
  invalid_referenceLI           EXCEPTION;

  CURSOR curReference (newValue NUMBER) IS
    SELECT LO_GROUP_ID, MF_LNO
    FROM GT_PLOT_LEGEND_OVERRIDE_GROUPS
    WHERE LO_GROUP_ID = newValue;

  CURSOR curReferenceDC (newValue NUMBER) IS
    SELECT g3e_displaycontroltable
      FROM g3e_legend
     WHERE g3e_lno = newValue;

  TYPE modcur IS REF CURSOR;
  modcv modcur;
  sqlStr VARCHAR2(2048);

  vUtlFileDir VARCHAR2(50);
  File        UTL_FILE.FILE_TYPE;

BEGIN

  OPEN curReference(:NEW.LO_GROUP_ID);
  FETCH curReference INTO refValue, refValueMF_LNO;
  IF curReference%NOTFOUND THEN
    RAISE invalid_reference;
  END IF;
  CLOSE curReference;

  OPEN curReferenceDC(refValueMF_LNO);
  FETCH curReferenceDC INTO refValuedisplaycontroltable;
  IF curReferenceDC%NOTFOUND THEN
    RAISE invalid_referenceDC;
  END IF;
  CLOSE curReferenceDC;

  sqlStr := 'select g3e_legenditem from ' || refValuedisplaycontroltable || ' WHERE g3e_leafindicator = 1 AND g3e_legenditem=''' || :NEW.LO_LEGENDITEM || '''';

  -- Generate a log.
  --select value into vUtlFileDir from v$parameter where name='utl_file_dir';
  --File := UTL_FILE.FOPEN(vUtlFileDir,'Sql.log','w');
  --UTL_FILE.PUT_LINE(File,'Cursor: ' || sqlStr);
  --UTL_FILE.FCLOSE(File);

  OPEN modcv FOR sqlstr;
  FETCH modcv INTO refValueLegendItem;
  IF modcv%NOTFOUND THEN
    RAISE invalid_referenceLI;
  END IF;
  CLOSE modcv;

EXCEPTION
  WHEN invalid_reference THEN
    CLOSE curReference;
    raise_application_error ( -20001, 'A reference to LO_GROUP_ID=' || :NEW.LO_GROUP_ID || ' does not exists in GT_PLOT_LEGEND_OVERRIDE_GROUPS.LO_GROUP_ID');
  WHEN invalid_referenceDC THEN
    CLOSE curReferenceDC;
    raise_application_error ( -20002, 'Unable to locate G3E_LEGEND.G3E_LNO=' || refValueMF_LNO);
  WHEN invalid_referenceLI THEN
    CLOSE modcv;
    raise_application_error ( -20002, 'Unable to locate the LO_LEGENDITEM =' || :NEW.LO_LEGENDITEM || ' in the referenced ' || refValuedisplaycontroltable || ' legend.');
  WHEN OTHERS THEN
    RAISE;
END M_T_BIUR_LEGEND_OVERRIDE_ITEMS;
/
SHOW ERRORS;


ALTER TABLE GT_PLOT_LEGEND_OVERRIDE_ITEMS ADD (
  CONSTRAINT PK_GT_PLOT_LEG_ORI_LO_ITM_NO
 PRIMARY KEY
 (LO_ITEM_NO));

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_LEGEND_OVERRIDE_ITEMS TO ADMINISTRATOR;

GRANT SELECT ON GT_PLOT_LEGEND_OVERRIDE_ITEMS TO DESIGNER;

GRANT SELECT ON GT_PLOT_LEGEND_OVERRIDE_ITEMS TO FINANCE;

GRANT SELECT ON GT_PLOT_LEGEND_OVERRIDE_ITEMS TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_LEGEND_OVERRIDE_ITEMS TO SUPERVISOR;

