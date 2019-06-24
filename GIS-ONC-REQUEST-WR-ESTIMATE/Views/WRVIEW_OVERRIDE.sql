CREATE OR REPLACE FORCE EDITIONABLE VIEW GIS_ONC.WRVIEW_OVERRIDE (
    WR_NBR,
    RULE_ID,
    RULE_NM,
    G3E_FNO,
    G3E_FID,
    STRUCTURE_ID,
    FEATURE_NAME,
    STRUCTURE_FID,
    ERROR_MSG,
    OVERRIDE_UID,
    OVERRIDE_COMMENTS,
    OVERRIDE_D
) AS
    SELECT
        vo.g3e_identifier      AS wr_nbr,
        vo.rule_id             AS rule_id,
        vo.rule_nm             AS rule_nm,
        vo.g3e_fno             AS g3e_fno,
        vo.g3e_fid             AS g3e_fid,
        vo.structure_id        AS structure_id,
        g3.g3e_username        AS feature_name,
        (
            SELECT
                MAX(comm.g3e_fid)
            FROM
                common_n comm
            WHERE
                comm.structure_id = vo.structure_id
        ) AS structure_fid,
        vo.error_msg           AS error_msg,
        vo.override_uid        AS override_uid,
        vo.override_comments   AS override_comments,
        vo.override_d          AS override_d
    FROM
        gis_onc.wr_validation_override vo,
        gis.g3e_feature g3
    WHERE
        g3.g3e_fno = vo.g3e_fno;