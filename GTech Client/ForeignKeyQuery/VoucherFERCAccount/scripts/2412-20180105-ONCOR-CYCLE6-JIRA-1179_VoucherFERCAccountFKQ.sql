
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2412, '1179_VoucherFERCAccountFKQ');
spool c:\temp\2412-20180105-ONCOR-CYCLE6-JIRA-1179_VoucherFERCAccountFKQ.log
--**************************************************************************************
--SCRIPT NAME: 2412-20180105-ONCOR-CYCLE6-JIRA-1179_VoucherFERCAccountFKQ.sql
--**************************************************************************************
-- AUTHOR		: INGRNET\PNLELLA
-- DATE		: 05-JAN-2018
-- CYCLE		: 6

-- JIRA NUMBER	:
-- PRODUCT VERSION	: 10.2.04
-- PRJ IDENTIFIER	: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC	:
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

execute CREATE_SEQUENCE.CREATE_METADATA_SEQUENCES;

Insert into G3E_RELATIONINTERFACE (G3E_RINO,G3E_USERNAME,G3E_INTERFACE,G3E_TYPE,G3E_EDITDATE,G3E_DESCRIPTION) values (G3E_RELATIONIFACEARGS_SEQ.NEXTVAL,'Voucher FERC Account','fkqVoucherFERCAccount:GTechnology.Oncor.CustomAPI.fkqVoucherFERCAccount','Foreign Key Query',SYSDATE,'Updates the FERC Prime and Sub Account numbers  of vouchers based on the Prime Account Numbers and CU Activity codes');


Insert into g3e_relationargument (G3E_RANO,G3E_RINO,G3E_ARGUMENTORDINAL,G3E_ARGUMENTNAME,G3E_ARGUMENTDESCRIPTION,G3E_ARGUMENTTYPE,G3E_EDITDATE) values (G3E_RELATIONARGUMENT_SEQ.nextval,(select g3e_rino from G3E_RELATIONINTERFACE where g3e_username = 'Voucher FERC Account'),1,'AttributeName','Indicates FQR configured attribute',10,SYSDATE);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO)+1 from G3E_RELATIONIFACEARGS),1,'FERC_PRIME_ACCT',sysdate);

Insert into G3E_RELATIONIFACEARGS (G3E_ARGROWNO,G3E_RIARGGROUPNO,G3E_ARGUMENTORDINAL,G3E_VALUE,G3E_EDITDATE) values (G3E_RELATIONIFACEARGS_SEQ.nextval,(select max(G3E_RIARGGROUPNO)+1 from G3E_RELATIONIFACEARGS),1,'FERC_SUB_ACCT',sysdate);

--Work Point Attributes FI Update

UPDATE G3E_ATTRIBUTE SET G3E_FKQRINO=(SELECT G3E_RINO FROM G3E_RELATIONINTERFACE WHERE G3E_USERNAME='Voucher FERC Account'),G3E_FKQARGGROUPNO = (select G3E_RIARGGROUPNO from G3E_RELATIONIFACEARGS where g3e_value = 'FERC_SUB_ACCT') WHERE G3E_FIELD ='FERC_SUB_ACCT' AND G3E_USERNAME='FERC Sub Account' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');

UPDATE G3E_ATTRIBUTE SET G3E_FKQRINO=(SELECT G3E_RINO FROM G3E_RELATIONINTERFACE WHERE G3E_USERNAME='Voucher FERC Account'),G3E_FKQARGGROUPNO = (select G3E_RIARGGROUPNO from G3E_RELATIONIFACEARGS where g3e_value = 'FERC_PRIME_ACCT') WHERE G3E_FIELD ='FERC_PRIME_ACCT' AND G3E_USERNAME='FERC Prime Account' AND G3E_CNO=(SELECT G3E_CNO FROM G3E_COMPONENT WHERE G3E_USERNAME='Voucher Attributes');

commit;

--**************************************************************************************
-- End Script Body

--**************************************************************************************
spool off;
exec adm_support.set_finish(2412);

