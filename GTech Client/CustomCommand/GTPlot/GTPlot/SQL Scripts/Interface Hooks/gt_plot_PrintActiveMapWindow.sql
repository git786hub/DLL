set serveroutput on size 999999;
set echo on;
spool gt_plot_NewPlotWindow.log;
select 'Date: ' || TO_CHAR(new_time(sysdate,'GMT','PST'), 'DD-MON-YYYY HH:MI:SS') FROM DUAL;
select 'User: '|| user || ' on database ' || global_name, '  (term='||USERENV('TERMINAL')||')' as MYCONTEXT from   global_name;


--*****************************************************************************************
--*   SCRIPT NAME: gt_plot_NewPlotWindow.sql
--*****************************************************************************************     
--*                           
--*   Programmer: Paul Adams
--*
--*   Tracking Number: 
--*
--*   Product Version Number: 10.1
--*
--*   Release Number:
--*
--*   Project Identifier: IndustryWare
--*   
--*   Program Description: Implements the Print Active Map Window custom command.
--*
--*****************************************************************************************



-- Remove old Print Active Map Window... VBA command if exists.
Delete from G3E_CUSTOMCOMMAND where G3E_USERNAME='Print Active Map Window...';
Delete from G3E_CUSTOMCOMMAND where G3E_USERNAME='Print Active Map Window';

Delete from G3E_CUSTOMCOMMAND where G3E_USERNAME='Print Active Map Window Settings...';


-- PrintActiveMapWindow
Insert into G3E_CUSTOMCOMMAND
   (G3E_CCNO, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL, G3E_INTERFACE)
 Values
   (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL, 'Print Active Map Window', 'Custom command used to Print the Active Map Windows', 'Paul Adams', 'Intergraph Canada Ltd.', 0, 0, 'Print Active Map Window...', 1, 0, 0, 0, 1, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.PrintActiveMapWindow');


-- PrintActiveMapWindowSettings
Insert into G3E_CUSTOMCOMMAND
   (G3E_CCNO, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL, G3E_INTERFACE)
 Values
   (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL, 'Print Active Map Window Settings...', 'Custom command used to show the Print Active Map Windows Settings', 'Paul Adams', 'Intergraph Canada Ltd.', 0, 0, 'Print Active Map Window Settings...', 1, 0, 0, 0, 1, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.PrintActiveMapWindowSettings');



-- Add the following extra metadata entries to upgrade your existing metadata tables to support the PrintActiveMapWindow types.
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  10010,  1, 'A-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  20010,  2, 'A-Size Landscape', '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  30010,  3, 'B-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  40010,  4, 'B-Size Landscape', '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  50010,  5, 'C-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  60010,  6, 'C-Size Landscape', '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  70010,  7, 'D-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  80010,  8, 'D-Size Landscape', '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow',  90010,  9, 'E-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow', 100010, 10, 'E-Size Landscape', '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow', 110010, 11, 'F-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow', 120010, 12, 'F-Size Landscape', '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow', 130010, 13, 'G-Size Portrait',  '', 10, 20402001);
Insert into GT_PLOT_DRAWINGINFO (DRI_TYPE, DRI_ID, SHEET_ID, DRI_NAME, DRI_SCALES, SHEET_INSET, SHEET_INSET_STYLE_NO) Values('PrintActiveMapWindow', 140010, 14, 'G-Size Landscape', '', 10, 20402001);

-- Fix styles that are not Area Styles.
--update GT_PLOT_DRAWINGINFO set SHEET_INSET_STYLE_NO=20402001 where SHEET_INSET_STYLE_NO not in (select distinct(g3e_sno) from G3E_AREASTYLE);

-- Add extra small style
Insert into G3E_TEXTSTYLE(G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_FONTWEIGHT, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFILLCOLOR, G3E_TEXTBOXFILLTYPE, G3E_TEXTBOXFRAME, G3E_TEXTBOXFRAMECOLOR, G3E_TEXTBOXFRAMEWIDTH, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_LEFTTEXTBOXOFFSET, G3E_RIGHTTEXTBOXOFFSET, G3E_TOPTEXTBOXOFFSET, G3E_BOTTOMTEXTBOXOFFSET, G3E_STYLEUNITS, G3E_TEXTBOXFILLTRANSLUCENCY)
 Values(5033, 'Common Label Plot Extra Small', 0, 'Arial', 0, 0, 0, 0, 400, 5, 0, -3, 0, 1, 0, 2.834652, 1, 1, 0.24, 0.24, 0.24, 0.24, 1, 100);

-- These already exist in Multi Industry Ware Configurations
Insert into G3E_TEXTSTYLE(G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_FONTWEIGHT, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFILLCOLOR, G3E_TEXTBOXFILLTYPE, G3E_TEXTBOXFRAME, G3E_TEXTBOXFRAMECOLOR, G3E_TEXTBOXFRAMEWIDTH, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_LEFTTEXTBOXOFFSET, G3E_RIGHTTEXTBOXOFFSET, G3E_TOPTEXTBOXOFFSET, G3E_BOTTOMTEXTBOXOFFSET, G3E_STYLEUNITS, G3E_TEXTBOXFILLTRANSLUCENCY)
 Values(5034, 'Common Label Plot Small', 0, 'Arial', 0, 0, 0, 0, 400, 6, 0, -3, 0, 1, 0, 2.834652, 1, 1, 0.24, 0.24, 0.24, 0.24, 1, 100);
Insert into G3E_TEXTSTYLE(G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_FONTWEIGHT, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFILLCOLOR, G3E_TEXTBOXFILLTYPE, G3E_TEXTBOXFRAME, G3E_TEXTBOXFRAMECOLOR, G3E_TEXTBOXFRAMEWIDTH, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_LEFTTEXTBOXOFFSET, G3E_RIGHTTEXTBOXOFFSET, G3E_TOPTEXTBOXOFFSET, G3E_BOTTOMTEXTBOXOFFSET, G3E_STYLEUNITS)
 Values(5035, 'Common Label Plot Medium', 0, 'Arial', 0, 0, 0, 0, 400, 8, 1, -3, 0, 0, 16777215, 2, 1, 1, 0.32, 0.32, 0.32, 0.32, 1);
Insert into G3E_TEXTSTYLE(G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_FONTWEIGHT, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFILLCOLOR, G3E_TEXTBOXFILLTYPE, G3E_TEXTBOXFRAME, G3E_TEXTBOXFRAMECOLOR, G3E_TEXTBOXFRAMEWIDTH, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_LEFTTEXTBOXOFFSET, G3E_RIGHTTEXTBOXOFFSET, G3E_TOPTEXTBOXOFFSET, G3E_BOTTOMTEXTBOXOFFSET, G3E_STYLEUNITS, G3E_TEXTBOXFILLTRANSLUCENCY)
 Values(5036, 'Common Label Plot Large', 0, 'Arial', 0, 0, 0, 0, 400, 10, 0, -3, 0, 1, 0, 2.834652, 1, 1, 0.4, 0.4, 0.4, 0.4, 1, 100);



delete from GT_PLOT_GROUPS_DRI where GROUP_NAME like '% - PrintActiveMapWindow - Map Frame';

Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10001, 'A-Size Portrait  - PrintActiveMapWindow - Map Frame',  10010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10002, 'A-Size Landscape - PrintActiveMapWindow - Map Frame',  20010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10003, 'B-Size Portrait  - PrintActiveMapWindow - Map Frame',  30010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10004, 'B-Size Landscape - PrintActiveMapWindow - Map Frame',  40010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10005, 'C-Size Portrait  - PrintActiveMapWindow - Map Frame',  50010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10006, 'C-Size Landscape - PrintActiveMapWindow - Map Frame',  60010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10007, 'D-Size Portrait  - PrintActiveMapWindow - Map Frame',  70010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10008, 'D-Size Landscape - PrintActiveMapWindow - Map Frame',  80010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10009, 'E-Size Portrait  - PrintActiveMapWindow - Map Frame',  90010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10010, 'E-Size Landscape - PrintActiveMapWindow - Map Frame', 100010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10011, 'F-Size Portrait  - PrintActiveMapWindow - Map Frame', 110010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10012, 'F-Size Landscape - PrintActiveMapWindow - Map Frame', 120010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10013, 'G-Size Portrait  - PrintActiveMapWindow - Map Frame', 130010, 1, 0, 0);
Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(10014, 'G-Size Landscape - PrintActiveMapWindow - Map Frame', 140010, 1, 0, 0);


-- FUTURE --

--delete from GT_PLOT_GROUPS_DRI where GROUP_NAME like '% - PrintActiveMapWindow - Redline - %';
--
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'A-Size Portrait  - PrintActiveMapWindow - Redline - Date',  10010, 0, 23.9, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'A-Size Landscape - PrintActiveMapWindow - Redline - Date',  20010, 0, 87.4, 180.4);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'B-Size Portrait  - PrintActiveMapWindow - Redline - Date',  30010, 0, 87.4, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'B-Size Landscape - PrintActiveMapWindow - Redline - Date',  40010, 0, 239.8, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'C-Size Portrait  - PrintActiveMapWindow - Redline - Date',  50010, 0, 239.8, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'C-Size Landscape - PrintActiveMapWindow - Redline - Date',  60010, 0, 366.8, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'D-Size Portrait  - PrintActiveMapWindow - Redline - Date',  70010, 0, 366.8, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'D-Size Landscape - PrintActiveMapWindow - Redline - Date',  80010, 0, 671.6, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'E-Size Portrait  - PrintActiveMapWindow - Redline - Date',  90010, 0, 671.6, 1082.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'E-Size Landscape - PrintActiveMapWindow - Redline - Date', 100010, 0, 925.6, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'F-Size Portrait  - PrintActiveMapWindow - Redline - Date', 110010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'F-Size Landscape - PrintActiveMapWindow - Redline - Date', 120010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'G-Size Portrait  - PrintActiveMapWindow - Redline - Date', 130010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20002, 'G-Size Landscape - PrintActiveMapWindow - Redline - Date', 140010, 0, 100, 100);
--
---- Set each group offset based on page dimensions...
--select * from GT_PLOT_SHEETS s, GT_PLOT_DRAWINGINFO d, GT_PLOT_GROUPS_DRI g where group_no=20002 and d.DRI_ID=g.DRI_ID and s.SHEET_ID=d.SHEET_ID
--
--update GT_PLOT_GROUPS_DRI g set GROUP_OFFSET_Y=(select s.SHEET_HEIGHT-d.SHEET_INSET+1.5 from GT_PLOT_SHEETS s, GT_PLOT_DRAWINGINFO d where group_no=20002 and d.DRI_ID=g.DRI_ID and s.SHEET_ID=d.SHEET_ID)
--where group_no=20002;
--
--update GT_PLOT_GROUPS_DRI g set GROUP_OFFSET_X=(select d.SHEET_INSET from GT_PLOT_SHEETS s, GT_PLOT_DRAWINGINFO d where group_no=20002 and d.DRI_ID=g.DRI_ID and s.SHEET_ID=d.SHEET_ID)
--where group_no=20002;
--
--delete from GT_PLOT_REDLINES where GROUP_NO=20002;
--
--Insert into GT_PLOT_REDLINES(RL_NO, GROUP_NO, RL_DATATYPE, RL_COORDINATE_X1, RL_COORDINATE_Y1, RL_STYLE_NUMBER, RL_TEXT_ALIGNMENT, RL_ROTATION, RL_TEXT, RL_USERINPUT) Values(20002001, 20002, 'Redline Text', 0, 0, 5033, 1, 0, 'Printed - Date:', 0);
--Insert into GT_PLOT_REDLINES(RL_NO, GROUP_NO, RL_DATATYPE, RL_COORDINATE_X1, RL_COORDINATE_Y1, RL_STYLE_NUMBER, RL_TEXT_ALIGNMENT, RL_ROTATION, RL_TEXT, RL_USERINPUT) Values(20002002, 20002, 'Redline Text', 15, 0, 5034, 1, 0, '[SYSDATE]', 0);
--
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'A-Size Portrait  - PrintActiveMapWindow - Redline - Time',  10010, 0, 23.9, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'A-Size Landscape - PrintActiveMapWindow - Redline - Time',  20010, 0, 87.4, 180.4);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'B-Size Portrait  - PrintActiveMapWindow - Redline - Time',  30010, 0, 87.4, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'B-Size Landscape - PrintActiveMapWindow - Redline - Time',  40010, 0, 239.8, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'C-Size Portrait  - PrintActiveMapWindow - Redline - Time',  50010, 0, 239.8, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'C-Size Landscape - PrintActiveMapWindow - Redline - Time',  60010, 0, 366.8, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'D-Size Portrait  - PrintActiveMapWindow - Redline - Time',  70010, 0, 366.8, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'D-Size Landscape - PrintActiveMapWindow - Redline - Time',  80010, 0, 671.6, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'E-Size Portrait  - PrintActiveMapWindow - Redline - Time',  90010, 0, 671.6, 1082.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'E-Size Landscape - PrintActiveMapWindow - Redline - Time', 100010, 0, 925.6, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'F-Size Portrait  - PrintActiveMapWindow - Redline - Time', 110010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'F-Size Landscape - PrintActiveMapWindow - Redline - Time', 120010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'G-Size Portrait  - PrintActiveMapWindow - Redline - Time', 130010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20003, 'G-Size Landscape - PrintActiveMapWindow - Redline - Time', 140010, 0, 100, 100);
--
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'A-Size Portrait  - PrintActiveMapWindow - Redline - User',  10010, 0, 23.9, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'A-Size Landscape - PrintActiveMapWindow - Redline - User',  20010, 0, 87.4, 180.4);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'B-Size Portrait  - PrintActiveMapWindow - Redline - User',  30010, 0, 87.4, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'B-Size Landscape - PrintActiveMapWindow - Redline - User',  40010, 0, 239.8, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'C-Size Portrait  - PrintActiveMapWindow - Redline - User',  50010, 0, 239.8, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'C-Size Landscape - PrintActiveMapWindow - Redline - User',  60010, 0, 366.8, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'D-Size Portrait  - PrintActiveMapWindow - Redline - User',  70010, 0, 366.8, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'D-Size Landscape - PrintActiveMapWindow - Redline - User',  80010, 0, 671.6, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'E-Size Portrait  - PrintActiveMapWindow - Redline - User',  90010, 0, 671.6, 1082.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'E-Size Landscape - PrintActiveMapWindow - Redline - User', 100010, 0, 925.6, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'F-Size Portrait  - PrintActiveMapWindow - Redline - User', 110010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'F-Size Landscape - PrintActiveMapWindow - Redline - User', 120010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'G-Size Portrait  - PrintActiveMapWindow - Redline - User', 130010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20004, 'G-Size Landscape - PrintActiveMapWindow - Redline - User', 140010, 0, 100, 100);
--
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'A-Size Portrait  - PrintActiveMapWindow - Redline - Scale',  10010, 0, 23.9, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'A-Size Landscape - PrintActiveMapWindow - Redline - Scale',  20010, 0, 87.4, 180.4);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'B-Size Portrait  - PrintActiveMapWindow - Redline - Scale',  30010, 0, 87.4, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'B-Size Landscape - PrintActiveMapWindow - Redline - Scale',  40010, 0, 239.8, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'C-Size Portrait  - PrintActiveMapWindow - Redline - Scale',  50010, 0, 239.8, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'C-Size Landscape - PrintActiveMapWindow - Redline - Scale',  60010, 0, 366.8, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'D-Size Portrait  - PrintActiveMapWindow - Redline - Scale',  70010, 0, 366.8, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'D-Size Landscape - PrintActiveMapWindow - Redline - Scale',  80010, 0, 671.6, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'E-Size Portrait  - PrintActiveMapWindow - Redline - Scale',  90010, 0, 671.6, 1082.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'E-Size Landscape - PrintActiveMapWindow - Redline - Scale', 100010, 0, 925.6, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'F-Size Portrait  - PrintActiveMapWindow - Redline - Scale', 110010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'F-Size Landscape - PrintActiveMapWindow - Redline - Scale', 120010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'G-Size Portrait  - PrintActiveMapWindow - Redline - Scale', 130010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20005, 'G-Size Landscape - PrintActiveMapWindow - Redline - Scale', 140010, 0, 100, 100);
--
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'A-Size Portrait  - PrintActiveMapWindow - Redline - Name',  10010, 0, 23.9, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'A-Size Landscape - PrintActiveMapWindow - Redline - Name',  20010, 0, 87.4, 180.4);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'B-Size Portrait  - PrintActiveMapWindow - Redline - Name',  30010, 0, 87.4, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'B-Size Landscape - PrintActiveMapWindow - Redline - Name',  40010, 0, 239.8, 243.9);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'C-Size Portrait  - PrintActiveMapWindow - Redline - Name',  50010, 0, 239.8, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'C-Size Landscape - PrintActiveMapWindow - Redline - Name',  60010, 0, 366.8, 396.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'D-Size Portrait  - PrintActiveMapWindow - Redline - Name',  70010, 0, 366.8, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'D-Size Landscape - PrintActiveMapWindow - Redline - Name',  80010, 0, 671.6, 523.3);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'E-Size Portrait  - PrintActiveMapWindow - Redline - Name',  90010, 0, 671.6, 1082.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'E-Size Landscape - PrintActiveMapWindow - Redline - Name', 100010, 0, 925.6, 828.1);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'F-Size Portrait  - PrintActiveMapWindow - Redline - Name', 110010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'F-Size Landscape - PrintActiveMapWindow - Redline - Name', 120010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'G-Size Portrait  - PrintActiveMapWindow - Redline - Name', 130010, 0, 100, 100);
--Insert into GT_PLOT_GROUPS_DRI(GROUP_NO, GROUP_NAME, DRI_ID, USERPLACE, GROUP_OFFSET_X, GROUP_OFFSET_Y) Values(20006, 'G-Size Landscape - PrintActiveMapWindow - Redline - Name', 140010, 0, 100, 100);











--ROLLBACK;
COMMIT;



spool off;
