DROP TABLE GT_PLOT_TMP_MULTI_TEXT CASCADE CONSTRAINTS;

CREATE GLOBAL TEMPORARY TABLE GT_PLOT_TMP_MULTI_TEXT
(
  NOTES  VARCHAR2(2048 BYTE)
)
ON COMMIT DELETE ROWS
NOCACHE;


CREATE OR REPLACE PUBLIC SYNONYM GT_PLOT_TMP_MULTI_TEXT FOR GT_PLOT_TMP_MULTI_TEXT;


GRANT SELECT ON GT_PLOT_TMP_MULTI_TEXT TO FINANCE;

GRANT SELECT ON GT_PLOT_TMP_MULTI_TEXT TO FULLPUBLISH;

GRANT SELECT ON GT_PLOT_TMP_MULTI_TEXT TO MARKETING;

GRANT DELETE, INSERT, SELECT, UPDATE ON GT_PLOT_TMP_MULTI_TEXT TO PUBLIC;

GRANT SELECT ON GT_PLOT_TMP_MULTI_TEXT TO SELECT_GCOMM_TABS_VIEWS;
