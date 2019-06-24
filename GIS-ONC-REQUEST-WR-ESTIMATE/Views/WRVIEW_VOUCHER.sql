DROP PUBLIC SYNONYM WRVIEW_VOUCHER;
CREATE OR REPLACE VIEW GIS_ONC.WRVIEW_VOUCHER
AS
	SELECT wp.WR_NBR AS WR_NBR,
				 wp.WP_NBR AS WP_NBR,
				 v.REQUEST_UID AS REQUEST_UID,
				 v.REQUEST_D AS REQUEST_D,
				 v.VOUCHER_C AS VOUCHER_C,
				 v.FERC_PRIME_ACCT AS FERC_PRIME_ACCT,
				 v.FERC_SUB_ACCT AS FERC_SUB_ACCT,
				 v.COST_COMPONENT_C AS COST_COMPONENT_C,
				 v.AMOUNT_USD AS AMOUNT_USD,
				 '[GIS] ' || SUBSTR(v.COMMENTS, 1, 234) AS COMMENTS
		FROM WORKPOINT_N wp JOIN VOUCHER_N v ON wp.G3E_FID = v.G3E_FID;
CREATE PUBLIC SYNONYM WRVIEW_VOUCHER FOR GIS_ONC.WRVIEW_VOUCHER;