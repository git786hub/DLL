
set echo on
set linesize 1000
set pagesize 300
set trimspool on

exec adm_support.set_start(2693, 'ONCOR_JIRA1925_SecondaryCondcutor_Dialog_fix');
spool c:\temp\2693-20180720-ONCOR_JIRA1925_SecondaryCondcutor_Dialog_fix.log
--**************************************************************************************
--SCRIPT NAME: 2693-20180720-ONCOR_JIRA1925_SecondaryCondcutor_Dialog_fix.sql
--**************************************************************************************
-- AUTHOR				: INGRNET\PVKURELL
-- DATE					: 20-JUL-2018
-- CYCLE				: 00.03.08
-- JIRA NUMBER			: 1925
-- PRODUCT VERSION		: 10.3
-- PRJ IDENTIFIER		: G/TECHNOLOGY - ONCOR
-- PROGRAM DESC			: create dialog for Secondary Conductor UG as per DFS to Fix.
--                        Current dialog tab showing as "Secondary Conductor - OH Attributes"
-- SOURCE DATABASE	:
--**************************************************************************************
-- Script Body
--**************************************************************************************

Insert into G3E_DIALOGTAB (G3E_DTNO,G3E_USERNAME,G3E_ORIENTATION) values (63301,'Secondary Conductor - UG Attributes','V');
Insert into G3E_DIALOGTAB (G3E_DTNO,G3E_USERNAME,G3E_ORIENTATION) values (63201,'Secondary Conductor - UG Attributes','V');
Insert into G3E_DIALOGTAB (G3E_DTNO,G3E_USERNAME,G3E_ORIENTATION) values (63101,'Secondary Conductor - UG Attributes','V');


Update G3E_DIALOG Set G3E_DTNO=63301 WHERE G3E_FNO=63 AND G3E_DTNO=53301;
Update G3E_DIALOG Set G3E_DTNO=63201 WHERE G3E_FNO=63 AND G3E_DTNO=53201;
Update G3E_DIALOG Set G3E_DTNO=63101 WHERE G3E_FNO=63 AND G3E_DTNO=53101;


Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (6310100,530101,10,293,1,63101);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (6310101,530102,20,null,1,63101);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (6320101,530102,20,null,0,63201);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (6320100,530101,10,293,0,63201);

Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (6330101,530102,20,null,0,63301);
Insert into G3E_TABATTRIBUTE (G3E_TANO,G3E_ANO,G3E_ORDINAL,G3E_PNO,G3E_READONLY,G3E_DTNO) values (6330100,530101,10,293,0,63301);

Update G3E_DIALOGTAB SET G3E_USERNAME='Secondary Wire Attributes' WHERE G3E_USERNAME='Secondary Wire - UG Attributes';

--**************************************************************************************
-- End Script Body
--**************************************************************************************
spool off;
exec adm_support.set_finish(2693);


