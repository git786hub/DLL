set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\SyncModLogForChangedViews.log
--**************************************************************************************
-- SCRIPT NAME: SyncModLogForChangedViews.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 23-JUL-2018
-- PRODUCT VERSION	: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Sync modification log for latest changes to component views
--**************************************************************************************


execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCOND_L');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCOND_DL');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCONDUG_L');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCONDUG_DL');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCONDOHN_L');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCONDOHN_DL');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCONDUGN_L');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_SECCONDUGN_DL');

execute G3E_MANAGEMODLOG.SyncToComponentView('V_SERVICELINE_T');

execute G3E_MANAGEMODLOG.SyncToComponentView('V_STREETCTR_CL_T');
execute G3E_MANAGEMODLOG.SyncToComponentView('V_STREETCTR_ML_T');

spool off;
