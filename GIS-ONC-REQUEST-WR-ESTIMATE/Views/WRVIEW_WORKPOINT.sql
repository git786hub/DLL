DROP PUBLIC SYNONYM WRVIEW_WORKPOINT;
CREATE OR REPLACE VIEW GIS_ONC.WRVIEW_WORKPOINT
AS
	SELECT wp.WR_NBR AS WR_NBR,
				 wp.WP_NBR AS WP_NBR,
				 cu.WM_SEQ AS PS_SEQ_NBR,
				 DECODE(cu.WP_RELATED, 0, 'P', 'S') AS PS_TYPE_C,
				 DECODE(cu.WP_RELATED, 0, 0, QTY_LENGTH_Q) AS SPAN_LENGTH_FT,
				 DECODE(cu.WP_RELATED, 0, 0, wp.WP_NBR) AS PS_FROM_NBR,
				 cu.WP_RELATED AS PS_TO_NBR,
				 wp.STRUCTURE_ID AS STRUCTURE_ID_FROM,
				 NVL(swp.STRUCTURE_ID, 0) AS STRUCTURE_ID_TO,
				 (SELECT MAX(conn.VOLT_1_Q)
						FROM	 CONNECTIVITY_N conn
								 JOIN
									 COMMON_N comm
								 ON conn.G3E_FID = comm.G3E_FID
					 WHERE comm.STRUCTURE_ID = wp.STRUCTURE_ID)
					 AS OPERATING_VOLTAGE,
				 (SELECT MAX(LTRIM(conn.FEEDER_1_ID, conn.SSTA_C))
						FROM	 CONNECTIVITY_N conn
								 JOIN
									 COMMON_N comm
								 ON conn.G3E_FID = comm.G3E_FID
					 WHERE comm.STRUCTURE_ID = wp.STRUCTURE_ID)
					 AS FEEDER_NBR
		FROM WORKPOINT_N wp
				 JOIN WORKPOINT_CU_N cu
					 ON wp.G3E_FID = cu.G3E_FID
				 LEFT JOIN WORKPOINT_N swp
					 ON (wp.WR_NBR = swp.WR_NBR AND cu.WP_RELATED = swp.WP_NBR);

CREATE PUBLIC SYNONYM WRVIEW_WORKPOINT FOR GIS_ONC.WRVIEW_WORKPOINT;