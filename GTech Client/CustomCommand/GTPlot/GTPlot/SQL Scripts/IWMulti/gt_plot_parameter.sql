ALTER TABLE GT_PLOT_PARAMETER
 DROP PRIMARY KEY CASCADE;

DROP TABLE GT_PLOT_PARAMETER CASCADE CONSTRAINTS;

CREATE TABLE GT_PLOT_PARAMETER
(
  PAR_NO           NUMBER(9) CONSTRAINT M_N_GTPLOT_PARAMETER_GPNO NOT NULL,
  PAR_NAME         VARCHAR2(80 BYTE) CONSTRAINT M_N_GTPLOT_PARAMETER_NAME NOT NULL,
  PAR_VALUE        VARCHAR2(80 BYTE) CONSTRAINT M_N_GTPLOT_PARAMETER_VALUE NOT NULL,
  PAR_DESCRIPTION  VARCHAR2(1024 BYTE)              NULL,
  PAR_EDITDATE     DATE CONSTRAINT M_N_GTPLOT_PARAMETER_EDITDATE NOT NULL
)
TABLESPACE GTECH
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE UNIQUE INDEX M_P_GT_PLOT_PARAMETER ON GT_PLOT_PARAMETER
(PAR_NO)
LOGGING
TABLESPACE GTECH
NOPARALLEL;


CREATE UNIQUE INDEX M_U_GTPLOT_PARAMETER_NAME ON GT_PLOT_PARAMETER
(PAR_NAME)
LOGGING
TABLESPACE GTECH
NOPARALLEL;


ALTER TABLE GT_PLOT_PARAMETER ADD (
  CONSTRAINT M_C_GTPLOT_PARAMETER_GPNO
 CHECK (PAR_NO > 0),
  CONSTRAINT M_P_GT_PLOT_PARAMETER
 PRIMARY KEY
 (PAR_NO)
    USING INDEX 
    TABLESPACE GTECH,
  CONSTRAINT M_U_GTPLOT_PARAMETER_NAME
 UNIQUE (PAR_NAME)
    USING INDEX 
    TABLESPACE GTECH);

GRANT SELECT ON GT_PLOT_PARAMETER TO FINANCE;

GRANT SELECT ON GT_PLOT_PARAMETER TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_PARAMETER TO PUBLIC;


SET DEFINE OFF;
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (1, 'GT_PLOT_DRAWINGINFO', 'GT_PLOT_DRAWINGINFO', 'This table defines each drawing type with each sheet definition.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (2, 'GT_PLOT_GROUPS_DRI', 'GT_PLOT_GROUPS_DRI', 'This table defines areas in the plot sheet for placement of the map frame and redlines.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (3, 'GT_PLOT_LEGEND_OVERRIDE_GROUPS', 'GT_PLOT_LEGEND_OVERRIDE_GROUPS', 'This table defines the overrides available in the New Plot Window.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (4, 'GT_PLOT_LEGEND_OVERRIDE_ITEMS', 'GT_PLOT_LEGEND_OVERRIDE_ITEMS', 'This table defines the legend items that are grouped together for each legend override.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (5, 'GT_PLOT_MAPFRAME', 'GT_PLOT_MAPFRAME', 'This table defines the legend and the map frame for the plot sheet to be used when creating the Plot Window.  Create unique Group_NO and MF_NO for multiple plot legends.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (6, 'GT_PLOT_OBJECTS', 'GT_PLOT_OBJECTS', 'This table defines OLE objects to be inserted when creating the Plot Window.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (7, 'GT_PLOT_REDLINES', 'GT_PLOT_REDLINES', 'This table defines the redlines for the titleblock, etc.  Units are mm.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (8, 'GT_PLOT_SHEETS', 'GT_PLOT_SHEETS', 'This table defines the different sheet sizes and their dimension and inset defining the plot border. There are 10 sheets defined (A size thru E size, each with a landscape and portrait orientation).', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (9, 'PlotBoundary_G3E_CNO', '20401', 'Plot Boundary main attribute non graphic G3E_FNO.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (10, 'PlotBoundary_G3E_FNO', '204', 'Plot Boundary G3E_FNO.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (11, 'PlotBoundaryAttribute_Name', 'NAME', 'Attribute name used to store the plot name.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (12, 'PlotBoundaryAttribute_Orientation', 'ORIENTATION', 'Attribute name used to store the orientation.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (13, 'PlotBoundaryAttribute_PageSize', 'PAPER_SIZE', 'Attribute name used to store the paper size.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (14, 'PlotBoundaryAttribute_Scale', 'SCALE', 'Attribute name used to store the scale.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (15, 'PlotBoundaryAttribute_Type', 'PRODUCT_TYPE', 'Attribute name used to store the product type.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (16, 'PlotBoundaryLinkedJobAttribute_FieldName', 'G3E_IDENTIFIER', 'Attribute field name in the Job table used to link the job to the plot boundary feature.  This is used to locate plot boundaries placed within a given Job.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (17, 'PlotBoundaryInfoView', 'V_PLOTBNDY_GTPLOT_INFO', 'Name of the view used by GTPlot to retrieve all of the column info required to generate a list of available plot boundaries and used to generate the redline text.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (18, 'PlotBoundaryStoreSingleCharacterForOrientation', 'YES', 'Stores only the first character for Orientation. example: "L" for "Landscape"', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (19, 'PlotBoundaryDataToUpper', 'Yes', 'Store the plot boundary feature data in upper case.', TO_DATE('07/15/2010 16:52:52', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (20, 'PlotBoundaryAvailableScales', '1:12000,1:5000,1:3600,1:2500,1:2000,1:1500,1:1250,1:1000,1:750,1:500,1:250', 'List of available scales to be used by the Plot Boundary placement interface.', TO_DATE('07/15/2010 16:52:53', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (21, 'JobComponentNumber', '34', 'Job Component Number (g3e_cno), To get the Job attributes for the plot redline texts', TO_DATE('07/15/2010 16:52:53', 'MM/DD/YYYY HH24:MI:SS'));
Insert into GT_PLOT_PARAMETER
   (PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION, PAR_EDITDATE)
 Values
   (22, 'CUPrintGroupNumbers', '40005,40006,50005,50006', 'Construction Print Group Numbers, seperated by comma which refers to the Object in GT_PLOT_OBJECTS for Excel sheet for CU print', TO_DATE('07/15/2010 16:52:53', 'MM/DD/YYYY HH24:MI:SS'));
COMMIT;
