set serveroutput on size 999999;
set echo on;
spool gt_plot_PlotBoudary.log;
select 'Date: ' || TO_CHAR(new_time(sysdate,'GMT','PST'), 'DD-MON-YYYY HH:MI:SS') FROM DUAL;
select 'User: '|| user || ' on database ' || global_name, '  (term='||USERENV('TERMINAL')||')' as MYCONTEXT from   global_name;


--*****************************************************************************************
--*   SCRIPT NAME: gt_plot_PlotBoudary.sql
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
--*   Program Description: Implements the Plot Boundary Feature GTPlacementTechnique for IPT polygon and SPT label
--*
--*****************************************************************************************



-- GTPlotBoundaryIPT
Insert into G3E_PLACEMENTINTERFACE (G3E_PINO, G3E_USERNAME, G3E_INTERFACE, G3E_TYPEMASK, G3E_SILENT, G3E_EDITDATE)
Values (G3E_PLACEMENTINTERFACE_SEQ.NEXTVAL, 'PlotBoundaryIPT', 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTPlacementTechnique.PlotBoundaryIPT', 256, 0, sysdate);

-- New PlotBoundaryDataStoreSizeOnly parameter added.
Insert into GT_PLOT_PARAMETER(PAR_NO, PAR_NAME, PAR_VALUE, PAR_DESCRIPTION)
 Values(21, 'PlotBoundaryDataStoreSizeOnly', 'Yes', 'Store the plot boundary feature data size only. "8.5x11" vs "A-Size (8.5x11)"');


-- GTPlotBoundaryLabelSPT
Insert into G3E_PLACEMENTINTERFACE (G3E_PINO, G3E_USERNAME, G3E_INTERFACE, G3E_TYPEMASK, G3E_SILENT, G3E_EDITDATE)
Values (G3E_PLACEMENTINTERFACE_SEQ.NEXTVAL, 'PlotBoundaryLabelSPT', 'Intergraph.GTechnology.GTPlot:Intergraph.GTechnology.GTPlot.GTPlacementTechnique.PlotBoundaryLabelSPT', 256, 1, sysdate);



--
-- Link the placement interfaces to a Geo and Detail plot boundary and Geo plot boundary label component.
--
-- The plot boundary feature must at least have the following require attributes:
-- 
--  plan_id
--  plot_type
--  plot_size
--  plot_scale
--  plot_orientation
--  job_name < used to link the feature to an active job.
--  [...] < Any attributes required to automaticly display as automatic redline text.
--
-- 
-- Add or Use a Plot Boundary Geo Component
-- Add or Use a Plot Boundary Detail Component (Optional)
-- Add alternate required Plotting Boundary Area (Det) component otherwise the FI wont update the attributes correctly (This might no longer be required.)
-- Add or Use a Large Plot Boundary Label Component (Optional) - to display the name of the plot boundary at large scales to be seen from a distance only.
--



-- Change the placement selection to use a placement configuration.
Update G3E_PLACEMENTSELECTION set G3E_FNO=NULL, G3E_PCNO=(Select max(G3E_PCNO)+1 from G3E_PLACEMENTCONFIGURATION) Where G3E_USERNAME ='Plotting Boundary';



-- Add component configurations and the new PlotBoundaryIPT and PlotBoundaryLabelSPT VB.NET references.
Insert into G3E_COMPCONFIGURATION (G3E_CCROWNO,G3E_CCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_REFRESH) Values (2040101, 20401, 'Work Plan Boundary', 204, 20402, 1, 0, (select G3E_PINO from G3E_PLACEMENTINTERFACE where G3E_USERNAME='PlotBoundaryIPT'), 1);
Insert into G3E_COMPCONFIGURATION (G3E_CCROWNO,G3E_CCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_REFRESH) Values (2040102, 20401, 'Work Plan Boundary', 204, 20404, 2, 0, (select G3E_PINO from G3E_PLACEMENTINTERFACE where G3E_USERNAME='PlotBoundaryLabelSPT'), 1);
Insert into G3E_COMPCONFIGURATION (G3E_CCROWNO,G3E_CCNO,G3E_USERNAME,G3E_FNO,G3E_CNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_REFRESH) Values (2040201, 20402, 'Work Plan Boundary', 204, 20405, 1, 0, (select G3E_PINO from G3E_PLACEMENTINTERFACE where G3E_USERNAME='PlotBoundaryIPT'), 1);



-- Link the component configuration to the placement selection
Insert into G3E_PLACEMENTCONFIGURATION (G3E_PCROWNO,G3E_PCNO,G3E_USERNAME,G3E_FNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_FEATUREINDEX,G3E_REFRESH,G3E_REQUIRED,G3E_REPEATING,G3E_CCNO,G3E_PLACECOMPONENT) Values (2040201, 1501, 'Plotting Boundary', 204, 1, 0, 7, 1, 1, 1, 0, 20401, 1);
Insert into G3E_PLACEMENTCONFIGURATION (G3E_PCROWNO,G3E_PCNO,G3E_USERNAME,G3E_FNO,G3E_ORDINAL,G3E_AUTOREPEAT,G3E_PINO,G3E_FEATUREINDEX,G3E_REFRESH,G3E_REQUIRED,G3E_REPEATING,G3E_CCNO,G3E_PLACECOMPONENT) Values (2040202, 1501, 'Plotting Boundary', 204, 2, 0, 7, 1, 1, 0, 0, 20402, 1);



-- Change the placement selection to use a placement configuration.
Update G3E_PLACEMENTSELECTION set G3E_FNO=NULL, G3E_PCNO=(Select G3E_PCNO from G3E_PLACEMENTCONFIGURATION Where G3E_USERNAME='Plotting Boundary') Where G3E_USERNAME ='Plotting Boundary';



--ROLLBACK;
COMMIT;



spool off;
