set serveroutput on size 999999;
set echo on;
spool gt_plot_Styles.log;
select 'Date: ' || TO_CHAR(new_time(sysdate,'GMT','PST'), 'DD-MON-YYYY HH:MI:SS') FROM DUAL;
select 'User: '|| user || ' on database ' || global_name, '  (term='||USERENV('TERMINAL')||')' as MYCONTEXT from   global_name;
--*****************************************************************************************
--*   SCRIPT NAME: gt_plot_Styles.sql
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
--*   Project Identifier: Gas
--*   
--*   Program Description: Implements style for the GTPlot solution
--*
--*****************************************************************************************

SET DEFINE OFF;

-- Add Generic Text Style
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10000, 'Text Style Arial (Black - 05.7)',         0, 'Arial', 0, 0, 0, 0, 05.7, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10001, 'Text Style Arial (Black - 05.7 - Bold)',  0, 'Arial', 1, 0, 0, 0, 05.7, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10002, 'Text Style Arial (Black - 08.5)',         0, 'Arial', 0, 0, 0, 0, 08.5, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10003, 'Text Style Arial (Black - 08.5 - Bold)',  0, 'Arial', 1, 0, 0, 0, 08.5, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10004, 'Text Style Arial (Black - 11.3)',         0, 'Arial', 0, 0, 0, 0, 11.3, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10005, 'Text Style Arial (Black - 11.3 - Bold)',  0, 'Arial', 1, 0, 0, 0, 11.3, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10006, 'Text Style Arial (Black - 14.2)',         0, 'Arial', 0, 0, 0, 0, 14.2, 0, 0, 1, 1, 1);
Insert into G3E_TEXTSTYLE (G3E_SNO, G3E_USERNAME, G3E_COLOR, G3E_FONTNAME, G3E_FONTBOLD, G3E_FONTITALIC, G3E_FONTSTRIKETHROUGH, G3E_FONTUNDERLINE, G3E_SIZE, G3E_TEXTBOX, G3E_TEXTBOXFRAME, G3E_PLOTREDLINE, G3E_JUSTIFICATION, G3E_STYLEUNITS) Values (
10007, 'Text Style Arial (Black - 14.2 - Bold)',  0, 'Arial', 1, 0, 0, 0, 14.2, 0, 0, 1, 1, 1);



--
-- Style actualy used by GTPlot
--

SET DEFINE OFF;
--
--SQL Statement which produced this data:
--  select * from g3e_linestyle where G3E_SNO in (select distinct RL_STYLE_NUMBER from GT_PLOT_REDLINES where RL_STYLE_NUMBER is not null)
--
Insert into G3E_LINESTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_width, g3e_offset, g3e_startsymbol, g3e_endsymbol, g3e_strokepattern, g3e_plotredline, g3e_editdate, g3e_styleunits, g3e_translucency)
 Values
   (27302111, 'Line Style 0 (Black - 1)', 0, 1, NULL, 
    NULL, NULL, NULL, 1, TO_DATE('01/04/2010 11:47:02', 'MM/DD/YYYY HH24:MI:SS'), 
    1, NULL);
COMMIT;



SET DEFINE OFF;
--
--SQL Statement which produced this data:
--  select * from g3e_textstyle where G3E_SNO in (select distinct RL_STYLE_NUMBER from GT_PLOT_REDLINES where RL_STYLE_NUMBER is not null)
--
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_editdate, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (10000, 'Text Style Arial (Black - 05.7)', 0, 'Arial', 0, 
    0, 0, 0, NULL, 5.7, 
    0, NULL, NULL, 0, NULL, 
    NULL, 1, 1, NULL, NULL, 
    NULL, NULL, NULL, TO_DATE('01/04/2010 11:46:52', 'MM/DD/YYYY HH24:MI:SS'), 1, 
    NULL, NULL, NULL, NULL, NULL);
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_editdate, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (10001, 'Text Style Arial (Black - 05.7 - Bold)', 0, 'Arial', 1, 
    0, 0, 0, NULL, 5.7, 
    0, NULL, NULL, 0, NULL, 
    NULL, 1, 1, NULL, NULL, 
    NULL, NULL, NULL, TO_DATE('01/04/2010 11:46:52', 'MM/DD/YYYY HH24:MI:SS'), 1, 
    NULL, NULL, NULL, NULL, NULL);
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_editdate, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (10003, 'Text Style Arial (Black - 08.5 - Bold)', 0, 'Arial', 1, 
    0, 0, 0, NULL, 8.5, 
    0, NULL, NULL, 0, NULL, 
    NULL, 1, 1, NULL, NULL, 
    NULL, NULL, NULL, TO_DATE('01/04/2010 11:46:52', 'MM/DD/YYYY HH24:MI:SS'), 1, 
    NULL, NULL, NULL, NULL, NULL);
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_editdate, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (10005, 'Text Style Arial (Black - 11.3 - Bold)', 0, 'Arial', 1, 
    0, 0, 0, NULL, 11.3, 
    0, NULL, NULL, 0, NULL, 
    NULL, 1, 1, NULL, NULL, 
    NULL, NULL, NULL, TO_DATE('01/04/2010 11:46:52', 'MM/DD/YYYY HH24:MI:SS'), 1, 
    NULL, NULL, NULL, NULL, NULL);
COMMIT;



SET DEFINE OFF;
--
--SQL Statement which produced this data:
--  select * from g3e_areastyle where G3E_SNO in (select distinct MF_STYLE_NO from GT_PLOT_MAPFRAME where MF_STYLE_NO is not null);
--
COMMIT;



SET DEFINE OFF;
--
--SQL Statement which produced this data:
--  select * from g3e_linestyle where G3E_SNO in (select distinct SHEET_INSET_STYLE_NO from GT_PLOT_DRAWINGINFO where SHEET_INSET_STYLE_NO is not null)
--
Insert into G3E_LINESTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_width, g3e_offset, g3e_startsymbol, g3e_endsymbol, g3e_strokepattern, g3e_plotredline, g3e_editdate, g3e_styleunits, g3e_translucency)
 Values
   (27302111, 'Line Style 0 (Black - 1)', 0, 1, NULL, 
    NULL, NULL, NULL, 1, TO_DATE('01/04/2010 11:47:02', 'MM/DD/YYYY HH24:MI:SS'), 
    1, NULL);
COMMIT;



spool off;



