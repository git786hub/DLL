set serveroutput on size 999999;
set echo on;
spool gt_plot_WorkspacePlots.log;
select 'Date: ' || TO_CHAR(new_time(sysdate,'GMT','PST'), 'DD-MON-YYYY HH:MI:SS') FROM DUAL;
select 'User: '|| user || ' on database ' || global_name, '  (term='||USERENV('TERMINAL')||')' as MYCONTEXT from   global_name;


--*****************************************************************************************
--*   SCRIPT NAME: gt_plot_WorkspacePlots.sql
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
--*   Program Description: Implements Workspace Plots Custom Command
--*
--*****************************************************************************************



-- Remove old Workspace Plots VBA command if exists.
Delete from G3E_CUSTOMCOMMAND where G3E_USERNAME='Workspace Plots...';



-- WorkspacePlots
Insert into G3E_CUSTOMCOMMAND
   (G3E_CCNO, G3E_INTERFACE, G3E_USERNAME, G3E_DESCRIPTION, G3E_AUTHOR, G3E_COMMENTS, G3E_LARGEBITMAP, G3E_SMALLBITMAP, G3E_TOOLTIP, G3E_COMMANDCLASS, G3E_ENABLINGMASK, G3E_MODALITY, G3E_SELECTSETENABLINGMASK, G3E_MENUORDINAL)
 Values
   (G3E_CUSTOMCOMMAND_SEQ.NEXTVAL, 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTCustomCommands.WorkspacePlots', 'Workspace Plots...', 'Custom command used to manage Workspace Plots', 'Paul Adams', 'Intergraph Canada Ltd.', 0, 0, 'Workspace Plots...', 1, 0, 0, 0, 1);



COMMIT;



spool off;
