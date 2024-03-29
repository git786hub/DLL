spool Restart_CORRELATION_ID_SEQ_&SDATE..log

UNDEFINE CORR_ID_SEQ

ACCEPT CORR_ID_SEQ NUMBER PROMPT 'Enter the new GIS_STG.CORRELATION_ID_SEQ value:'

DECLARE
    PARAM_CORR_ID_SEQ NUMBER;
BEGIN
    PARAM_CORR_ID_SEQ := '&CORR_ID_SEQ';
    EXECUTE IMMEDIATE ( 'ALTER SEQUENCE GIS_STG.CORRELATION_ID_SEQ RESTART START WITH ' || PARAM_CORR_ID_SEQ); 
    DBMS_OUTPUT.PUT_LINE('CORRELATION_ID_SEQ has been restarted to begin with ' || PARAM_CORR_ID_SEQ);
end;
/
