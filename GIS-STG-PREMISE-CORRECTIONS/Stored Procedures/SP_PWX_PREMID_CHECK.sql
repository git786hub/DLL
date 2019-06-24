/*-----------------------------------------------------------------------------
Create Stored Procedure used for Informatica Replication, and to establish a link for PREM_ID
--Note:
--Create Stored Procedure GIS_STG.SP_PWX_PREMID_CHECK
-----------------------------------------------------------------------------*/   
create or replace PROCEDURE                  SP_PWX_PREMID_CHECK (
    p_per_id        CHAR,
    p_prem_id   OUT VARCHAR2)
AS    
BEGIN
    SELECT sp.prem_id
      Into P_Prem_Id
      FROM gis_stg.ci_acct_per         acct_per,
           gis_stg.ci_sa               sa,
           gis_stg.ci_sa_sp            sa_sp,
           gis_stg.ci_sp               sp,
           gis_stg.ci_prem             prem
     WHERE     acct_per.MAIN_CUST_SW = 'Y'
           AND acct_per.per_id = p_per_id
           AND sa.acct_id = acct_per.ACCT_ID
           AND sa_sp.sa_id = sa.sa_id
           AND sp.sp_id = sa_sp.sp_id
           AND sp.sp_type_cd IN ('METERED',
                                 'UNMTR-SL',
                                 'UNMTR-O',
                                 'UNMTR-GL')
           AND prem.prem_id = sp.PREM_ID
           AND prem.prem_type_cd NOT IN ('TEMP', 'TEST');
EXCEPTION
    WHEN NO_DATA_FOUND
    Then
        p_prem_id := '11';
END;   