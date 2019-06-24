spool Update_SYS_GENPARAM_&SDATE..log

DECLARE
	--Replacment Vars
	PARAM_CBLE_TENS_FILENAME VARCHAR2(200);
    PARAM_DOC_MGMT_SP_URL varchar2(200);
    PARAM_EDF_HOST_NAME VARCHAR2(200);
    PARAM_GUY_TEMPLATE_PATH varchar(200);
    PARAM_ESI_ATTACHMENT_LOCN varchar(200);
    PARAM_SAG_TEMPLATE_PATH varchar(200);
    PARAM_SEC_CALC_REPORT_PATH varchar(200);
    PARAM_STRTLGHT_NO_MSLA_URL VARCHAR2(200); 	
	PARAM_STRTLGHT_MSLA_URL VARCHAR2(200);
    PARAM_STRTLGHT_VOLT_DROP_REPORT_PATH varchar(200);
	PARAM_WMIS_WRTBCK_USER VARCHAR2(20);
	PARAM_WMIS_WRTBCK_PASS VARCHAR2(20);
	PARAM_WMIS_WRTBCK_SID VARCHAR2(20);
    PARAM_EMAIL_FROM varchar(200);
    PARAM_EMAIL_TO varchar(200); 
    
    PROCEDURE MergeParam (p_subsystem_name VARCHAR2, p_subsystem_component VARCHAR2, p_param_name VARCHAR2, p_param_value VARCHAR2, p_param_desc VARCHAR2) IS
    begin
        merge into GIS_ONC.SYS_GENERALPARAMETER SGP
        using (
            select
                p_subsystem_name v_subsystem_name,
                p_subsystem_component v_subsystem_component,
                p_param_name v_param_name,
                p_param_value v_param_value,
                p_param_desc v_param_desc
            from
                dual
        ) val 
        on (
            (SGP.subsystem_name = v_subsystem_name)
            and (NVL(SGP.SUBSYSTEM_COMPONENT,'*') = NVL(v_subsystem_component,'*'))
            and (SGP.param_name = v_param_name)
        )
        when matched then 
            update set param_value = val.v_param_value, param_desc = val.v_param_desc
        when not matched then 
            insert (subsystem_name, subsystem_component, param_name, param_value, param_desc) values (v_subsystem_name, v_subsystem_component, v_param_name, v_param_value, v_param_desc);
    end MergeParam;

    FUNCTION EF_SERVICE_URL(v_PORT VARCHAR2) RETURN VARCHAR2
	IS
	BEGIN
		RETURN PARAM_EDF_HOST_NAME || ':' || v_PORT || '/interface';
	END EF_SERVICE_URL;
    
    FUNCTION DB_STRING(
			v_USER VARCHAR2,
            v_PASS VARCHAR2,
			v_SID VARCHAR2 )
		RETURN VARCHAR2
	IS
	BEGIN
		RETURN 'User Id=' || v_USER || ';Password=' || v_PASS || ';Data Source=' || v_SID;
    END DB_STRING;


  

BEGIN
    PARAM_CBLE_TENS_FILENAME := '&CBLE_TENS_FILENAME';
    PARAM_DOC_MGMT_SP_URL := '&DOC_MGMT_SP_URL';    
    PARAM_EDF_HOST_NAME := '&EDF_HOST_NAME';
    PARAM_GUY_TEMPLATE_PATH := '&GUY_TEMPLATE_PATH';
    PARAM_ESI_ATTACHMENT_LOCN := '&ESI_ATTACHMENT_LOCN';
    PARAM_SAG_TEMPLATE_PATH := '&SAG_TEMPLATE_PATH'; 
    PARAM_SEC_CALC_REPORT_PATH := '&SEC_CALC_REPORT_PATH';
	PARAM_STRTLGHT_MSLA_URL := '&STRTLGHT_MSLA_URL';
    PARAM_STRTLGHT_NO_MSLA_URL := '&STRTLGHT_NO_MSLA_URL';
    PARAM_STRTLGHT_VOLT_DROP_REPORT_PATH :=  '&STRTLGHT_VOLT_DROP_REPORT_PATH';
    PARAM_WMIS_WRTBCK_USER := '&WMIS_WRTBCK_USER';
    PARAM_WMIS_WRTBCK_PASS := '&WMIS_WRTBCK_PASS';
    PARAM_WMIS_WRTBCK_SID := '&WMIS_WRTBCK_SID';
    PARAM_EMAIL_FROM := '&EMAIL_FROM';
    PARAM_EMAIL_TO := '&EMAIL_TO';


        MergeParam('CablePullTensionCC', 'Report', 'ReportFileName', PARAM_CBLE_TENS_FILENAME, 'The full path to the report workbook template.'); 

        MergeParam('Doc_Management', 'GT_SharePoint', 'SP_URL', PARAM_DOC_MGMT_SP_URL, 'SharePoint site URL'); 
    
        MergeParam('EdgeFrontier','GIS_CreateFieldTransaction','EF_URL', EF_SERVICE_URL('8013'),'Url to the EdgeFrontier GIS_CreateFieldTransaction System');
        MergeParam('EdgeFrontier','GIS_CreateUpdateJob','EF_URL', EF_SERVICE_URL('8010'),'Url to the EdgeFrontier GIS_CreateUpdateJob System');
        MergeParam('EdgeFrontier','GIS_RequestBatch','EF_URL', EF_SERVICE_URL('8012'),'Url to the EdgeFrontier GIS_RequestBatch System');
        MergeParam('EdgeFrontier','GIS_UpdateWritebackStatus','EF_URL', EF_SERVICE_URL('8011'),'Url to the EdgeFrontier GIS_UpdateWritebackStatus System');
        MergeParam('EdgeFrontier','NJUNS_CheckTicketStatus','EF_URL', EF_SERVICE_URL('8050'),'Url to the EdgeFrontier NJUNS_CheckTicketStatus System');
        MergeParam('EdgeFrontier','NJUNS_MemberCodeUpdate','EF_URL', EF_SERVICE_URL('8052'),'Url to the EdgeFrontier NJUNS_MemberCodeUpdate System');
        MergeParam('EdgeFrontier','NJUNS_SubmitTicket','EF_URL', EF_SERVICE_URL('8051'),'Url to the EdgeFrontier NJUNS_SubmitTicket System');
        MergeParam('EdgeFrontier','SendEmail','EF_URL', EF_SERVICE_URL('8089'),'Url to the EdgeFrontier SendEmail System');
        MergeParam('EdgeFrontier','WMIS_SendBatchResults','EF_URL', EF_SERVICE_URL('8030'),'Url to the EdgeFrontier WMIS_SendBatchResults System');
        MergeParam('EdgeFrontier','WMIS_UpdateStatus','EF_URL', EF_SERVICE_URL('8031'),'Url to the EdgeFrontier WMIS_UpdateStatus System');
        MergeParam('EdgeFrontier','WMIS_WriteBack','EF_URL', EF_SERVICE_URL('8032'),'Url to the EdgeFrontier WMIS_WriteBack System');

        MergeParam('GISAUTO_DEIS','URL','DEIS_RESPONSE_WEB_ADDRESS', EF_SERVICE_URL('8061'),'The EdgeFrontier system to call to send the DEIS transaction result.');

        MergeParam('Guying','Template','WorkbookPath',PARAM_GUY_TEMPLATE_PATH,'The full path to the tool workbook template.');

        MergeParam('REQUEST_ESI_LOCATION_CC',null,'ATTACHMENT_LOCATION',PARAM_ESI_ATTACHMENT_LOCN, 'Location where the Request ESI Location custom command will create the spreadsheet which will be sent to the Call Center using Edge Frontier.');

        MergeParam('SEND_EMAIL',null,'EF_URL', EF_SERVICE_URL('8089'),'Url to the EdgeFrontier SendEmail System');       
 
        MergeParam('Sag Clearance','Template','WorkbookPath', PARAM_SAG_TEMPLATE_PATH,'The full path to the tool workbook template.');  

        MergeParam('SecondaryCalculatorCC','Report','ReportFileName', PARAM_SEC_CALC_REPORT_PATH,'The full path to the report workbook template.');  

        MergeParam('StreetLightAgreementForms','StreetLightAgreementFormsWithMSLA','StreetLightWithMSLA_URL',PARAM_STRTLGHT_MSLA_URL,'Street Light Supplemental Agreement Forms With MSLA');
        MergeParam('StreetLightAgreementForms','StreetLightAgreementFormsWithOutMSLA','StreetLightWithWithOutMSLA_URL',PARAM_STRTLGHT_NO_MSLA_URL,'Street Light Supplemental Agreement Forms WithOut MSLA');

        MergeParam('StreetLightVoltageDropCC',null,'ReportFileName', PARAM_STRTLGHT_VOLT_DROP_REPORT_PATH,'The full path to the report workbook template.');  

        MergeParam('WMIS','WMIS_UpdateStatus','EF_URL', EF_SERVICE_URL('8031'),'Url to the Edgefrontier WMIS_UpdateStatus System');
        MergeParam('WMIS','WMIS_WRITEBACK','ConnectionString',DB_STRING(PARAM_WMIS_WRTBCK_USER,PARAM_WMIS_WRTBCK_PASS,PARAM_WMIS_WRTBCK_SID),'Connection string for writeback polling.');        
        MergeParam('WMIS','WMIS_WRITERBACK','EF_URL', EF_SERVICE_URL('8032'),'Url to the Edgefrontier WMIS_WRITEBACK System');

        MergeParam('GISAUTO_WMIS','ERRORREPORTING','EmailFromAddress',PARAM_EMAIL_FROM ,'EMail from address for WMIS Batch Processing errors.');
        MergeParam('MaximoWO','ErrorLoggingMail','FromEmailAddr',PARAM_EMAIL_FROM ,'The "From" email address for the Maximo Interface');
        MergeParam('MaximoWO','ErrorLoggingMail','NotificationEmailAddr',PARAM_EMAIL_TO ,'The "To" email address for the Maximo Interface');
        MergeParam('PremiseCorrections','ErrorLoggingMail','FromEmailAddress',PARAM_EMAIL_FROM ,'The "From" for the "Premise Correction Errors" email.');
        MergeParam('PremiseCorrections','ErrorLoggingMail','ToEmailAddress',PARAM_EMAIL_TO ,'The "To" for the "Premise Correction Errors" email.');
        MergeParam('REQUEST_ESI_LOCATION_CC',null,'TO_ADDRESS',PARAM_EMAIL_TO ,'The "To" recipient of the "GIS ESI Location Request" email.');
        MergeParam('StreetlightBilling','ErrorLoggingMail','FromEmailAddress',PARAM_EMAIL_FROM ,'The "From" for the "Street Light count threshold exceeded" email.');
        MergeParam('StreetlightBilling','ErrorLoggingMail','ToEmailAddress',PARAM_EMAIL_TO ,'The "To" for the "Street Light count threshold exceeded" email.');
        
        COMMIT;
end;
/
