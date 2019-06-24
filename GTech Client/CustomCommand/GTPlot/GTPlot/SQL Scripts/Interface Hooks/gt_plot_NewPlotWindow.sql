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
--*   Program Description: Implements the New Plot Window custom command.
--*
--*****************************************************************************************



-- Remove old New Plot Window... VBA command if exists.
Delete from G3E_CUSTOMCOMMAND where G3E_USERNAME='New Plot Window...';



-- GTNewPlotWindow
Insert into G3E_CUSTOMCOMMAND
   (G3E_CCNO, G3E_INTERFACE, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL)
 Values
   (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.NewPlotWindow', 'New Plot Window...', 'Custom command used to create new Plot Windows', 'Paul Adams', 'Intergraph Canada Ltd.', 0, 0, 'New Plot Window...', 1, 0, 0, 0, 1);



--ROLLBACK;
COMMIT;



spool off;
