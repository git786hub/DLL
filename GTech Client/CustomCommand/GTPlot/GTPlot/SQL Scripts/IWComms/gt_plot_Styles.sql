set serveroutput on size 999999;
set echo on;
spool gt_plot_Styles.log;
select 'Date: ' || TO_CHAR(new_time(sysdate,'GMT','PST'), 'DD-MON-YYYY HH:MI:SS') FROM DUAL;
select 'User: '|| user || ' on database ' || global_name, '  (term='||USERENV('TERMINAL')||')' as MYCONTEXT from   global_name;
--*****************************************************************************************
--*   SCRIPT NAME: GT_PLOT_Styles.sql
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
--*   Project Identifier: Comms
--*   
--*   Program Description: Implements style for the GTPlot solution
--*
--*****************************************************************************************



SET DEFINE OFF;
--
--SQL Statement which produced this data:
--  select * from g3e_linestyle where G3E_SNO in (select distinct RL_STYLE_NUMBER from GT_PLOT_REDLINES where RL_STYLE_NUMBER is not null)
--
Insert into G3E_LINESTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_width, g3e_offset, g3e_startsymbol, g3e_endsymbol, g3e_strokepattern, g3e_plotredline, g3e_editdate, g3e_styleunits, g3e_translucency)
 Values
   (5010, 'Common Line', 0, 0.75, 0, 
    NULL, NULL, 100, 1, TO_DATE('09/27/2007 15:42:26', 'MM/DD/YYYY HH24:MI:SS'), 
    1, NULL);
COMMIT;



SET DEFINE OFF;
--
--SQL Statement which produced this data:
--  select * from g3e_textstyle where G3E_SNO in (select distinct RL_STYLE_NUMBER from GT_PLOT_REDLINES where RL_STYLE_NUMBER is not null)
--
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_editdate, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (5034, 'Common Label Plot Small', 0, 'Arial', 0, 
    0, 0, 0, 400, 6, 
    0, -3, 0, 1, 0, 
    2.834652, 1, TO_DATE('04/26/2010 16:47:38', 'MM/DD/YYYY HH24:MI:SS'), 1, NULL, 
    0.24, 0.24, 0.24, 0.24, 1, 
    NULL, 100, NULL, NULL, NULL);
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_editdate, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (5035, 'Common Label Plot Medium', 0, 'Arial', 0, 
    0, 0, 0, 400, 8, 
    1, -3, 0, 0, 16777215, 
    2, 1, TO_DATE('04/26/2010 16:47:38', 'MM/DD/YYYY HH24:MI:SS'), 1, NULL, 
    0.32, 0.32, 0.32, 0.32, 1, 
    NULL, NULL, NULL, NULL, NULL);
Insert into G3E_TEXTSTYLE
   (g3e_sno, g3e_username, g3e_color, g3e_fontname, g3e_fontbold, g3e_fontitalic, g3e_fontstrikethrough, g3e_fontunderline, g3e_fontweight, g3e_size, g3e_textbox, g3e_textboxfillcolor, g3e_textboxfilltype, g3e_textboxframe, g3e_textboxframecolor, g3e_textboxframewidth, g3e_plotredline, g3e_editdate, g3e_justification, g3e_underlinesno, g3e_lefttextboxoffset, g3e_righttextboxoffset, g3e_toptextboxoffset, g3e_bottomtextboxoffset, g3e_styleunits, g3e_translucency, g3e_textboxfilltranslucency, g3e_textboxhatchtranslucency, g3e_textboxframetranslucency, g3e_textboxhatchcolor)
 Values
   (5036, 'Common Label Plot Large', 0, 'Arial', 0, 
    0, 0, 0, 400, 10, 
    0, -3, 0, 1, 0, 
    2.834652, 1, TO_DATE('04/26/2010 16:47:38', 'MM/DD/YYYY HH24:MI:SS'), 1, NULL, 
    0.4, 0.4, 0.4, 0.4, 1, 
    NULL, 100, NULL, NULL, NULL);
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
   (5011, 'Common Line Thick', 0, 2.25, 0, 
    NULL, NULL, 100, 1, TO_DATE('09/27/2007 15:42:26', 'MM/DD/YYYY HH24:MI:SS'), 
    1, NULL);
COMMIT;



spool off;



