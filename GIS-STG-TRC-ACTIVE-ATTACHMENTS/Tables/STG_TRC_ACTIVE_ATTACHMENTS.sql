DROP TABLE GIS_STG.STG_TRC_ACTIVE_ATTACHMENTS;
CREATE TABLE GIS_STG.STG_TRC_ACTIVE_ATTACHMENTS
(
    STG_TRC_ATTACHMENT_ID NUMBER GENERATED ALWAYS AS IDENTITY,
    PROCESS_DT            date,
    PROCESS_RUN_ID        number,
    RECORD_TYPE           varchar2(1),
    STRUCTURE_ID          varchar2(16),
    ATTACH_COMPANY        varchar2(10),
    ATTACH_TYPE           varchar2(30),
    PERMIT_NUMBER         varchar2(10),
    ATTACH_HEIGHT_FT      varchar2(7),
    ATTACH_POSITION       varchar2(20),
    ATTACHMENT_STATUS     varchar2(10),
    C_MESSENGER           varchar2(10),
    C_INIT_STR_TENSION    number(5),
    C_OUTSIDE_DIAM        number(5),
    E_WEIGHT              number(5),
    E_BRACKET_ARM         varchar2(3),
    INSPECTION_DATE       timestamp(6),
    AUD_CREATE_USR_ID     varchar2(30) default user,
    AUD_MOD_USR_ID        varchar2(30),
    AUD_CREATE_TS         timestamp(6) default sysdate,
    AUD_MOD_TS            timestamp(6),
    IMPORT_DATE_TIME      date,
    IMPORT_SESSION_ID     number(5),
    IMPORT_STATUS         varchar2(10),
    IMPORT_ERROR_MSG      varchar2(500),
    IMPORT_ERROR_TYPE     varchar2(20)
);
ALTER TABLE GIS_STG.STG_TRC_ACTIVE_ATTACHMENTS ADD PRIMARY KEY (STG_TRC_ATTACHMENT_ID);