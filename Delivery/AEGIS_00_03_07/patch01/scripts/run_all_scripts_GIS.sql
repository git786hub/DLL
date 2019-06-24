--
-- Run All Scripts
-- v00.03.07 patchset 01
--
-- NOTE: RUN CONNECTED AS USER GIS
--

@@GIS-01-SyncModLogForChangedViews.sql
@@GIS-02-Capacitor_ControlStatusUsername_Update.sql
@@GIS-03-Update_ReadWriteControl_Type.sql
@@GIS-04-ValueListDatatypeChanges.sql
commit;
