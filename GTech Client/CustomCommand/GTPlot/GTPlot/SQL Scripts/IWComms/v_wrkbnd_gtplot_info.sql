-- Create view used by GTPlot to retrieve all of the column info required to generate a list of available plot boundaries and
-- used to generate the redline text.
--
-- The view must expose the plot boundary feature's:
-- 
--  g3e_id
--  g3e_fno
--  g3e_fid
--  g3e_cno -Geo CNO of Plot Boundary Polygon
--  g3e_cid -Geo CID for Plot Boundary Polygon
--  g3e_detailid -Detail CNO for Plot Boundary Polygon (Optional)
--  g3e_cno_d -Detail CNO for Plot Boundary Polygon (Optional)
--  g3e_cid_d  -Detail CID for Plot Boundary Polygon (Optional)
--  plan_id
--  plot_type
--  plot_size
--  plot_scale
--  plot_orientation
--  job_name < used to link the feature to an active job.
--  [...] < Any attributes required to automatically display as automatic redline text.
--

DROP VIEW V_WRKBND_GTPLOT_INFO;

CREATE OR REPLACE FORCE VIEW V_WRKBND_GTPLOT_INFO
(
   G3E_ID,
   G3E_FNO,
   G3E_FID,
   G3E_CNO,
   G3E_CID,
   G3E_DETAILID,
   G3E_CNO_D,
   G3E_CID_D,
   PLAN_ID,
   PLOT_TYPE,
   PLOT_SIZE,
   PLOT_SCALE,
   PLOT_ORIENTATION,
   ORIGINATOR,
   PHONE,
   ORIGINATOR_INFO,
   ISSUE_DATE,
   REISSUE_DATE,
   ASSOCIATE_NAME,
   ASSOCIATE_PHONE,
   ASSOCIATE_INFO,
   ROW_NUMBER,
   JOB_ID,
   SWITCH_CENTRE_CLLI,
   G3E_IDENTIFIER,
   G3E_OWNER,
   G3E_STATUS,
   G3E_CREATION,
   G3E_POSTED,
   G3E_CLOSED,
   G3E_FIELDUSER,
   G3E_JOBCLASS,
   JOB_STATE,
   WORK_ORDER_ID,
   G3E_PROCESSINGSTATUS,
   G3E_POSTFLAG,
   SWITCH_CENTRE_NAME,
   EXCHANGE_NAME,
   "SYSDATE"
)
AS
   SELECT a.g3e_id, a.g3e_fno, a.g3e_fid, f.g3e_cno, f.g3e_cid, e.g3e_detailid, e.g3e_cno, e.g3e_cid, a.plan_id, a.plot_type, a.plot_size, a.plot_scale, a.plot_orientation, a.originator, a.phone, a.originator || ' / ' || a.phone "originator_info", TO_CHAR(a.issue_date, 'MON-DD-YYYY') "issue_date", TO_CHAR(a.reissue_date, 'MON-DD-YYYY') "reissue_date", a.associate_name, a.associate_phone, a.associate_name || ' / ' || a.associate_phone "associate_info", a.ROW_NUMBER, b.job_id, b.switch_centre_clli, c.g3e_identifier, c.g3e_owner, c.g3e_status, c.g3e_creation, c.g3e_posted, c.g3e_closed, c.g3e_fielduser, c.g3e_jobclass, c.job_state, c.work_order_id, c.g3e_processingstatus, c.g3e_postflag, d.switch_centre_name, d.exchange_name, TO_CHAR(SYSDATE, 'YYYY/MM/DD') "SYSDATE"
     FROM gc_wrkbnd a, gc_netelem b, g3e_job c, ref_exch d, dgc_wrkbnd_p e, gc_wrkbnd_p f
    WHERE b.g3e_fno = 1500 AND a.g3e_fid = b.g3e_fid AND b.sap_wrk_id = c.work_order_id AND a.g3e_fid = e.g3e_fid(+) AND a.g3e_fid = f.g3e_fid(+) AND a.plan_id IS NOT NULL AND a.plot_orientation IN ('L', 'P', 'Landscape', 'Portrait', 'ADHOC') AND (g3e_status IN ('Open', 'PartiallyPosted', 'Posted', 'Discarded') OR g3e_status IS NULL) AND b.switch_centre_clli = d.switch_centre_clli;


CREATE OR REPLACE PUBLIC SYNONYM V_WRKBND_GTPLOT_INFO FOR V_WRKBND_GTPLOT_INFO;


GRANT DELETE, INSERT, SELECT, UPDATE ON V_WRKBND_GTPLOT_INFO TO DESIGNER;

GRANT SELECT ON V_WRKBND_GTPLOT_INFO TO FINANCE;

GRANT SELECT ON V_WRKBND_GTPLOT_INFO TO MARKETING;

GRANT SELECT ON V_WRKBND_GTPLOT_INFO TO PUBLIC;
