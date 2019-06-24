--
-- Run All Scripts
-- v00.03.07 patchset 02
--
-- NOTE: RUN CONNECTED AS USER GIS
--

@@GIS-01-GISPKG_EF_UTIL.sql
@@GIS-02-PostJobCustomCommand.sql
@@GIS-03-Setting_G3E_COPY_To_1-MacroCUFields.sql
@@GIS-04-UpdateG3ECopyForCUDefaultAttributes.sql
commit;
