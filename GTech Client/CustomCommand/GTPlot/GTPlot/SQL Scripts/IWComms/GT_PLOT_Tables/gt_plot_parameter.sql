ALTER TABLE GT_PLOT_PARAMETER
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_PARAMETER CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_PARAMETER
(
  PAR_NO           NUMBER(9) CONSTRAINT M_N_GTPLOT_PARAMETER_GPNO NOT NULL,
  PAR_NAME         VARCHAR2(80 BYTE) CONSTRAINT M_N_GTPLOT_PARAMETER_NAME NOT NULL,
  PAR_VALUE        VARCHAR2(80 BYTE) CONSTRAINT M_N_GTPLOT_PARAMETER_VALUE NOT NULL,
  PAR_DESCRIPTION  VARCHAR2(1024 BYTE),
  PAR_EDITDATE     DATE CONSTRAINT M_N_GTPLOT_PARAMETER_EDITDATE NOT NULL
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


CREATE UNIQUE INDEX M_P_GT_PLOT_PARAMETER ON GT_PLOT_PARAMETER
(PAR_NO)
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


CREATE UNIQUE INDEX M_U_GTPLOT_PARAMETER_NAME ON GT_PLOT_PARAMETER
(PAR_NAME)
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


CREATE OR REPLACE TRIGGER M_T_BIUR_GTPLOTPARAM_EDATE 
BEFORE INSERT OR UPDATE ON GT_PLOT_PARAMETER
FOR EACH ROW
BEGIN

  :NEW.PAR_EDITDATE := SYSDATE;
END;
/


CREATE OR REPLACE PUBLIC SYNONYM GT_PLOT_PARAMETER FOR GT_PLOT_PARAMETER;


ALTER TABLE GT_PLOT_PARAMETER ADD (
  CONSTRAINT M_C_GTPLOT_PARAMETER_GPNO
  CHECK (PAR_NO > 0)
  ENABLE VALIDATE,
  CONSTRAINT M_P_GT_PLOT_PARAMETER
  PRIMARY KEY
  (PAR_NO)
  USING INDEX M_P_GT_PLOT_PARAMETER
  ENABLE VALIDATE,
  CONSTRAINT M_U_GTPLOT_PARAMETER_NAME
  UNIQUE (PAR_NAME)
  USING INDEX M_U_GTPLOT_PARAMETER_NAME
  ENABLE VALIDATE);

GRANT SELECT ON GT_PLOT_PARAMETER TO FINANCE;

GRANT SELECT ON GT_PLOT_PARAMETER TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_PARAMETER TO PUBLIC;

GRANT SELECT ON GT_PLOT_PARAMETER TO SUPERVISOR2;

Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (1, 'GT_PLOT_DRAWINGINFO', 'GT_PLOT_DRAWINGINFO', 'This table defines each drawing type with each sheet definition.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (2, 'GT_PLOT_GROUPS_DRI', 'GT_PLOT_GROUPS_DRI', 'This table defines areas in the plot sheet for placement of the map frame and redlines.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (3, 'GT_PLOT_LEGEND_OVERRIDE_GROUPS', 'GT_PLOT_LEGEND_OVERRIDE_GROUPS', 'This table defines the overrides available in the New Plot Window.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (4, 'GT_PLOT_LEGEND_OVERRIDE_ITEMS', 'GT_PLOT_LEGEND_OVERRIDE_ITEMS', 'This table defines the legend items that are grouped together for each legend override.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (5, 'GT_PLOT_MAPFRAME', 'GT_PLOT_MAPFRAME', 'This table defines the legend and the map frame for the plot sheet to be used when creating the Plot Window.  Create unique Group_NO and MF_NO for multiple plot legends.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (6, 'GT_PLOT_OBJECTS', 'GT_PLOT_OBJECTS', 'This table defines OLE objects to be inserted when creating the Plot Window.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (7, 'GT_PLOT_REDLINES', 'GT_PLOT_REDLINES', 'This table defines the redlines for the titleblock, etc.  Units are mm.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (8, 'GT_PLOT_SHEETS', 'GT_PLOT_SHEETS', 'This table defines the different sheet sizes and their dimension and inset defining the plot border. There are 10 sheets defined (A size thru E size, each with a landscape and portrait orientation).', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (9, 'PlotBoundary_G3E_CNO', '1501', 'Plot Boundary main attribute non graphic G3E_FNO.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (10, 'PlotBoundary_G3E_FNO', '1500', 'Plot Boundary G3E_FNO.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (11, 'PlotBoundaryAttribute_Name', 'PLAN_ID', 'Attribute name used to store the plot name.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (12, 'PlotBoundaryAttribute_Orientation', 'PLOT_ORIENTATION', 'Attribute name used to store the orientation.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (13, 'PlotBoundaryAttribute_PageSize', 'PLOT_SIZE', 'Attribute name used to store the paper size.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (14, 'PlotBoundaryAttribute_Scale', 'PLOT_SCALE', 'Attribute name used to store the scale.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (15, 'PlotBoundaryAttribute_Type', 'PLOT_TYPE', 'Attribute name used to store the product type.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (16, 'PlotBoundaryLinkedJobAttribute_FieldName', 'WORK_ORDER_ID', 'Attribute field name in the Job table used to link the job to the plot boundary feature.  This is used to locate plot boundaries placed within a given Job.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (17, 'PlotBoundaryInfoView', 'V_WRKBND_GTPLOT_INFO', 'Name of the view used by GTPlot to retrieve all of the column info required to generate a list of available plot boundaries and used to generate the redline text.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (18, 'PlotBoundaryStoreSingleCharacterForOrientation', 'No', 'Stores only the first character for Orientation. example: "L" for "Landbase"', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (19, 'PlotBoundaryDataToUpper', 'No', 'Store the plot boundary feature data in upper case.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (20, 'PlotBoundaryAvailableScales', '1:5000,1:3600,1:2500,1:2000,1:1500,1:1250,1:1000,1:750,1:500,1:250', 'List of available scales to be used by the Plot Boundary placement interface.', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (21, 'PlotBoundaryDataStoreSizeOnly', 'Yes', 'Store the plot boundary feature data size only. "8.5x11" vs "A-Size (8.5x11)"', TO_DATE('04/05/2013 22:02:05', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (22, 'GT_PLOT_TMP_MULTI_TEXT', 'GT_PLOT_TMP_MULTI_TEXT', 'This temp table is used to store text for RedLine Multi Text display. This is populated using a Procedure and then selected from in order to retrieve the values to display. GT_PLOT_REDLINES.RL_DATATYPE=''Redline Multi Text''', TO_DATE('08/01/2014 22:03:47', 'MM/DD/YYYY HH24:MI:SS'));
COMMIT;
