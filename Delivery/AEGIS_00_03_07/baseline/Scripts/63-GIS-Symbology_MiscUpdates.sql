set echo on
set linesize 1000
set pagesize 300
set trimspool on

spool c:\temp\Symbology_MiscUpdates.log

--**************************************************************************************
-- SCRIPT NAME: Symbology_MiscUpdates.sql
--**************************************************************************************
-- AUTHOR			    : INGRNET\RRADASE
-- DATE				    : 16-JUL-2018
-- PRODUCT VERSION: 10.3.0
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC		: Misc. legend & symbology updates from SME symbology review (July 9 - 11)
--**************************************************************************************


-- Set all custom legend background colors to dark blue

  update G3E_LEGEND set G3E_BACKCOLOR = 10813440 where G3E_LNO between 9 and 13;


-- Change label styles from black to white for readability against new blue background

  update G3E_TEXTSTYLE 
  set G3E_COLOR = 16777215
  where G3E_COLOR = 0 
    and G3E_USERNAME not like 'Common%'
    and G3E_USERNAME not like 'GTPLOT%'
    and G3E_USERNAME not like 'General%'
    and G3E_USERNAME not like '%Black%'
    and G3E_USERNAME not like '%FONTSIZE%'
    and G3E_USERNAME not like '%JOB_STATE%'
    and G3E_USERNAME not like '%No Match%'
    and G3E_USERNAME not like 'Fiber%'
  ;


-- Eliminate use of Arial Black for label fonts, based on feedback gathered during conversion pilot testing

  update G3E_TEXTSTYLE 
  set G3E_FONTNAME = 'Arial'
  where G3E_FONTNAME = 'Arial Black' 
    and G3E_USERNAME not like 'Common%'
    and G3E_USERNAME not like 'GTPLOT%'
    and G3E_USERNAME not like 'General%'
    and G3E_USERNAME not like '%Black%'
    and G3E_USERNAME not like '%FONTSIZE%'
    and G3E_USERNAME not like '%JOB_STATE%'
    and G3E_USERNAME not like '%No Match%'
    and G3E_USERNAME not like 'Fiber%'
  ;


-- Fix style rule references for Secondary Conductor geo linear legend entries

  update G3E_LEGENDENTRY set G3E_SRNO = 53101 where G3E_LEGENDENTRY in ('V_SECCOND_L','V_SECCONDUG_L','V_SECCONDOHN_L','V_SECCONDUGN_L');


-- Make landbase display control entries non-locatable by default

  update G3E_LS_LEGDIST
  set G3E_LOCATABLE = 0
  where G3E_LENO in (
    select le.G3E_LENO
    from G3E_LEGENDENTRY le
    join G3E_LDC_LEGDIST ldc on le.G3E_LEGENDENTRY = ldc.G3E_LEGENDITEM
    where ldc.G3E_ORDINAL > (select G3E_ORDINAL from G3E_LDC_LEGDIST where G3E_LEGENDITEM = 'Landbase')
      and ldc.G3E_ORDINAL < (select G3E_ORDINAL from G3E_LDC_LEGDIST where G3E_LEGENDITEM = 'Fiber')
  )
  ;


-- Make certain display control entries not displayed by default

  update G3E_LS_LEGDIST set G3E_DISPLAYMODE = 0 where G3E_LENO in (select G3E_LENO from G3E_LEGENDENTRY where G3E_USERNAME like 'Recloser%Label Large');
  update G3E_LS_LEGDIST set G3E_DISPLAYMODE = 0 where G3E_LENO in (select G3E_LENO from G3E_LEGENDENTRY where G3E_LEGENDENTRY like 'V_PARCEL____S'); -- Parcel Centroid


-- Set roles for display control entries

  update G3E_LDC_LEGDIST set G3E_ROLE = 'EVERYONE' where G3E_LEAFINDICATOR = 0;

  update G3E_LDC_LEGDIST
  set G3E_ROLE = 'PRIV_MGMT_LAND'
  where G3E_ORDINAL >= (select G3E_ORDINAL from G3E_LDC_LEGDIST where G3E_LEGENDITEM = 'Archived/Suppressed')
    and G3E_ORDINAL < (select G3E_ORDINAL from G3E_LDC_LEGDIST where G3E_LEGENDITEM = 'Fiber')
    and G3E_LEAFINDICATOR = 0;
  ;

  update G3E_LDC_LEGDIST
  set G3E_ROLE = 'PRIV_MGMT_STLT'
  where G3E_LEGENDITEM = 'Street Light - Non Located'
  ;


spool off;
