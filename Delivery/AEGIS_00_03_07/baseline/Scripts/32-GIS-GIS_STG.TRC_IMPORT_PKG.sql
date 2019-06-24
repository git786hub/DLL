/* Formatted on 6/8/2018 1:43:29 PM (QP5 v5.163.1008.3004) */
CREATE OR REPLACE PACKAGE GIS_STG.TRC_IMPORT_PKG
AS
	/******************************************************************************
		 NAME:			 TRC_IMPORT_PKG
		 PURPOSE:

		 REVISIONS:
		 Ver				Date				Author					 Description
		 ---------	----------	---------------  ------------------------------------
		 1.0				2/21/2018 		 reazzam			 1. Created this package.
	******************************************************************************/

	FUNCTION RefreshActivePermits
		RETURN VARCHAR2;

	PROCEDURE ImportTRCAttachments;
END TRC_IMPORT_PKG;
/


CREATE OR REPLACE PACKAGE BODY GIS_STG.TRC_IMPORT_PKG
AS
	PROCEDURE logSysError(vProcName VARCHAR2, vErrCode NUMBER, vErrMsg VARCHAR)
	IS
	BEGIN
		INSERT INTO Log_TRC_IMPORT(IMPORT_DATE_TIME,
															 IMPORT_SESSION_ID,
															 IMPORT_STATUS,
															 IMPORT_ERROR_MSG,
															 IMPORT_ERROR_TYPE)
		VALUES (SYSDATE,
						SYS_CONTEXT('USERENV', 'SID'),
						'ERROR',
						'Error in ' || vProcName || ' - ' || vErrCode || ':' || vErrMsg,
						'SYS_ERR');

		COMMIT;
	END;

	PROCEDURE NotFoundInField(vFid NUMBER, vStructId VARCHAR2)
	IS
		tmpCnt NUMBER := 0;
	BEGIN
		-- Equipment updates
		SELECT COUNT(*)
			INTO tmpCnt
			FROM GIS_STG.TRC_ALTERED_ATTACH_TMP
		 WHERE Record_type = 'E' AND Struct_Id = vStructId;

		IF tmpCnt > 0
		THEN
			-- find the not found in field attachments and log them
			UPDATE gis.ATTACH_EQPT_N
				 SET E_ATTACHMENT_STATUS = 'NotFdInFLD'
			 WHERE g3e_fid = vFid AND E_ATTACHMENT_STATUS NOT IN 'REMOVED'
				 AND g3e_cid NOT IN
							 (SELECT cid
									FROM TRC_ALTERED_ATTACH_TMP
								 WHERE record_type = 'E' AND Struct_ID = vStructId);

			FOR rec
				IN (SELECT *
							FROM ATTACH_EQPT_N
						 WHERE g3e_fid = vFid AND E_ATTACHMENT_STATUS NOT IN 'REMOVED'
							 AND g3e_cid NOT IN
										 (SELECT cid
												FROM TRC_ALTERED_ATTACH_TMP
											 WHERE record_type = 'E' AND Struct_ID = vStructId))
			LOOP
				INSERT INTO GIS_STG.LOG_TRC_IMPORT(RECORD_TYPE,
																					 STRUCTURE_ID,
																					 ATTACH_COMPANY,
																					 ATTACH_TYPE,
																					 PERMIT_NUMBER,
																					 ATTACH_HEIGHT_FT,
																					 ATTACH_POSITION,
																					 ATTACHMENT_STATUS,
																					 E_WEIGHT,
																					 E_BRACKET_ARM,
																					 IMPORT_SESSION_ID,
																					 IMPORT_STATUS,
																					 IMPORT_ERROR_MSG,
																					 IMPORT_ERROR_TYPE,
																					 G3E_FID)
				VALUES ('E',
								vStructId,
								REC.E_ATTACH_COMPANY,
								REC.E_ATTACH_TYPE_C,
								REC.E_PERMIT_NUMBER,
								REC.E_ATTACH_HEIGHT_FT,
								REC.E_ATTACH_POSITION_C,
								REC.E_ATTACHMENT_STATUS,
								REC.E_WEIGHT,
								REC.E_BRACKET_ARM,
								SYS_CONTEXT('USERENV', 'SID'),
								'ERROR',
								'Attachment not found in the field',
								'NOT_FOUND_IN_FLD',
								vFid);
			END LOOP;
		END IF;

		--	Wireline updates

		SELECT COUNT(*)
			INTO tmpCnt
			FROM GIS_STG.TRC_ALTERED_ATTACH_TMP
		 WHERE Record_type = 'W' AND Struct_Id = vStructId;

		IF tmpCnt > 0
		THEN
			-- find the not found in field attachments and log them
			UPDATE GIS.ATTACH_WIRELINE_N
				 SET w_ATTACHMENT_STATUS = 'NotFdInFLD'
			 WHERE g3e_fid = vFid AND w_ATTACHMENT_STATUS NOT IN 'REMOVED'
				 AND g3e_cid NOT IN
							 (SELECT cid
									FROM TRC_ALTERED_ATTACH_TMP
								 WHERE record_type = 'W' AND Struct_ID = vStructId);

			FOR rec
				IN (SELECT *
							FROM GIS.ATTACH_WIRELINE_N
						 WHERE g3e_fid = vFid AND W_ATTACHMENT_STATUS NOT IN 'REMOVED'
							 AND g3e_cid NOT IN
										 (SELECT cid
												FROM TRC_ALTERED_ATTACH_TMP
											 WHERE record_type = 'W' AND Struct_ID = vStructId))
			LOOP
				INSERT INTO GIS_STG.LOG_TRC_IMPORT(RECORD_TYPE,
																					 STRUCTURE_ID,
																					 ATTACH_COMPANY,
																					 ATTACH_TYPE,
																					 PERMIT_NUMBER,
																					 ATTACH_HEIGHT_FT,
																					 ATTACH_POSITION,
																					 ATTACHMENT_STATUS,
																					 C_MESSENGER,
																					 C_INIT_STR_TENSION,
																					 C_OUTSIDE_DIAM,
																					 IMPORT_SESSION_ID,
																					 IMPORT_STATUS,
																					 IMPORT_ERROR_MSG,
																					 IMPORT_ERROR_TYPE,
																					 G3E_FID)
				VALUES ('W',
								vStructId,
								REC.W_ATTACH_COMPANY,
								REC.W_ATTACH_TYPE,
								REC.W_PERMIT_NUMBER,
								REC.W_ATTACH_HEIGHT_FT,
								REC.W_ATTACH_POSITION,
								REC.W_ATTACHMENT_STATUS,
								REC.W_MESSENGER_C,
								REC.W_INIT_STR_TENSION,
								REC.W_OUTSIDE_DIAM,
								SYS_CONTEXT('USERENV', 'SID'),
								'ERROR',
								'Attachment not found in the field',
								'NOT_FOUND_IN_FLD',
								vFid);
			END LOOP;
		END IF;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('NotFoundInField', SQLCODE, SQLERRM);
			NULL;
	END;

	-- this procedure adds a row to the TRC_ALTERED_ATTACH_TMP tmp table.
	PROCEDURE addToModAttacments(vCID 				NUMBER,
															 vRecTyp			VARCHAR2,
															 vStructId		VARCHAR2)
	IS
	BEGIN
		INSERT INTO TRC_ALTERED_ATTACH_TMP(CID, RECORD_TYPE, STRUCT_ID)
		VALUES (vCID, vRecTyp, vStructId);
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('addToModAttacments', SQLCODE, SQLERRM);

			NULL;
	END;

	-- this function return the Company number with a company name input.
	FUNCTION LookupCoNum(CoName VARCHAR2)
		RETURN NUMBER
	IS
		tmpCodeNm NUMBER := 0;
	BEGIN
		--		Select vl_key into tmpCodeNm
		--				from GIS.VL_ATTACH_COMPANY
		--				where upper(VL_VALUE) = upper(CoName);
		SELECT vl_key
			INTO tmpCodeNm
			FROM GIS.VL_ATTACH_COMPANY
		 WHERE UPPER(TRC_CODE) = UPPER(CoName);

		RETURN tmpCodeNm;
	EXCEPTION
		WHEN NO_DATA_FOUND
		THEN
			tmpCodeNm := 0;
			RETURN tmpCodeNm;
		WHEN OTHERS
		THEN
			logSysError('LookupCoNum', SQLCODE, SQLERRM);
			tmpCodeNm := -1;
			RETURN tmpCodeNm;
	END;

	-- this function sends emails about error found while adding Permits to poles.
	PROCEDURE EmailPermitErrors(dupsInGIS 			 VARCHAR2,
															notFoundInGIS 	 VARCHAR2,
															run_id					 NUMBER)
	IS
		tmpMsg VARCHAR2(32000);
		tmpToAddrs VARCHAR2(4000);
		tmpFromAddr VARCHAR2(4000);
		tmpSubject VARCHAR2(4000);
		tmpDups VARCHAR2(32000);
		tmpNotFounds VARCHAR2(32000);
	BEGIN
		IF LENGTH(dupsInGIS) > 2 OR LENGTH(notFoundInGIS) > 2
		THEN
			IF LENGTH(dupsInGIS) > 2
			THEN
				tmpDups := REPLACE(dupsInGIS, '''', '');
				tmpDups := REPLACE(tmpDups, ',', '-');
				tmpDups := RTRIM(tmpDups, '-');
				--								dbms_output.put_line('tmpdups: ' || tmpToAddrs);
				tmpMsg :=
					tmpMsg ||
					'The each of the following Structure Ids reside on multiple structures in GIS: '
					|| tmpDups || '.' || CHR(10);
			END IF;

			IF LENGTH(notFoundInGIS) > 2
			THEN
				tmpNotFounds := REPLACE(notFoundInGIS, '''', '');
				tmpNotFounds := REPLACE(tmpNotFounds, ',', '-');
				tmpNotFounds := RTRIM(tmpNotFounds, '-');
				tmpMsg :=
					tmpMsg ||
					'The following Structure Ids were not found in the GIS system: '
					|| tmpNotFounds || '.';
			END IF;

			SELECT param_value
				INTO tmpFromAddr
				FROM sys_generalparameter
			 WHERE SUBSYSTEM_NAME = 'TRC_IMPORT'
				 AND SUBSYSTEM_COMPONENT = 'ErrorLoggingMail'
				 AND PARAM_NAME = 'AttachFromAddr';

			tmpSubject := 'TRC Active Permit Refresh Errors.';
			tmpToAddrs := 'DIS_HELP_DESK';

			--						dbms_output.put_line('To addr: ' || tmpToAddrs);
			--						dbms_output.put_line('From addr: ' || tmpFromAddr);
			--						dbms_output.put_line('Subject: ' || tmpSubject);
			--						dbms_output.put_line('Message: ' || tmpMsg);

			gis.SEND_EF_EMAIL_PKG.emToAddress := tmpToAddrs;
			gis.SEND_EF_EMAIL_PKG.emFromAddress := tmpFromAddr;
			gis.SEND_EF_EMAIL_PKG.emSubject := tmpSubject;
			gis.SEND_EF_EMAIL_PKG.emMessage := tmpMsg;
			GIS.SEND_EF_EMAIL_PKG.SENDEMAIL;
			tmpToAddrs := 'JOINT_USE_DIS_LIST';

			--						dbms_output.put_line('To addr 2: ' || tmpToAddrs);

			GIS.SEND_EF_EMAIL_PKG.SENDEMAIL;
		END IF;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('EmailPermitErrors', SQLCODE, SQLERRM);
			RAISE;
	END;

	-- this procedure logs the start of a process.
	PROCEDURE LogStartOfProcess(ProcessName VARCHAR2)
	IS
	BEGIN
		INSERT INTO GIS_STG.INTERFACE_LOG(LOG_DETAIL, PROCESS_RUN_ID, INSERT_DT)
		VALUES (
						 ProcessName || ' Process begins.',
						 SYS_CONTEXT('USERENV', 'SID'),
						 SYSDATE);

		COMMIT;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('LogStartOfProcess', SQLCODE, SQLERRM);
	END;


	-- This procedure logs the end of a process.
	PROCEDURE LogEndOfProcess(ProcessName VARCHAR2, hasErrs BOOLEAN)
	IS
		tmpMsg VARCHAR(500);
	BEGIN
		IF hasErrs = FALSE
		THEN
			tmpMsg := ' Completed Successfully with Errors.';
		ELSE
			tmpMsg := ' Completed Successfully.';
		END IF;

		INSERT INTO GIS_STG.INTERFACE_LOG(LOG_DETAIL, PROCESS_RUN_ID, INSERT_DT)
		VALUES (ProcessName || tmpMsg, SYS_CONTEXT('USERENV', 'SID'), SYSDATE);

		COMMIT;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('LogEndOfProcess', SQLCODE, SQLERRM);
	END;

	FUNCTION RefreshActivePermits
		RETURN VARCHAR2
	IS
		currRunId NUMBER;
		tmpReturnVal VARCHAR2(4000) := 'SUCCESS';
		structsNotFoundInGis VARCHAR2(32000);
		duplicatesInGis VARCHAR2(32000);
		tmpStructId VARCHAR2(16);
		tmpCntInGis NUMBER;
		tmpFids VARCHAR2(32000);
		tmpSuccess BOOLEAN := TRUE;
		vGisConfiguration VARCHAR2(30);
	BEGIN
		SELECT param_value
			INTO vGisConfiguration
			FROM sys_generalparameter
		 WHERE SUBSYSTEM_NAME = 'TRC_IMPORT'
			 AND SUBSYSTEM_COMPONENT = 'LttUserConfig'
			 AND PARAM_NAME = 'LttUserConfig';

		GIS.LTT_USER.SETCONFIGURATION(vGisConfiguration);

		-- get the last run_id
		SELECT MAX(PROCESS_RUN_ID) INTO currRunId FROM STG_TRC_ACTIVE_PERMITS;

		-- dbms_output.put_line('Run ID: ' || currRunId);

		-- Log Start of process
		LogStartOfProcess('Refresh Active Permits');

		UPDATE STG_TRC_ACTIVE_PERMITS
			 SET IMPORT_STATUS = 'PENDING'
		 WHERE PROCESS_RUN_ID = currRunId;

		-- check for missing Structures in GIS.
		FOR rec IN (SELECT STRUCTURE_ID
									FROM STG_TRC_ACTIVE_PERMITS
								 WHERE PROCESS_RUN_ID = currRunId)
		LOOP
			SELECT COUNT(*)
				INTO tmpCntInGis
				FROM common_n
			 WHERE structure_id = rec.STRUCTURE_ID AND g3e_fno = 110;

			-- dbms_output.put_line('Structure_id: ' || rec.STRUCTURE_ID || ' Count: '|| tmpCntInGis);
			-- Structures not found in GIS
			IF tmpCntInGis = 0
			THEN
				structsNotFoundInGis :=
					structsNotFoundInGis || '''' || rec.STRUCTURE_ID || ''',';

				UPDATE STG_TRC_ACTIVE_PERMITS
					 SET IMPORT_STATUS = 'ERROR',
							 IMPORT_ERROR_MSG = 'Structure Id not found in GIS'
				 WHERE STRUCTURE_ID = rec.STRUCTURE_ID;
			END IF;

			-- Duplicate Structures found in GIS
			IF tmpCntInGis > 1
			THEN
				duplicatesInGis :=
					duplicatesInGis || '''' || rec.STRUCTURE_ID || ''',';

				FOR rec2
					IN (SELECT G3E_FID
								FROM gis.common_n
							 WHERE structure_id = rec.STRUCTURE_ID AND g3e_fno = 110)
				LOOP
					tmpFids := tmpFids || rec2.G3E_FID || ',';
				END LOOP;

				-- update Stagging table records for Structures with duplicate Structure IDs in GIS.
				UPDATE GIS_STG.STG_TRC_ACTIVE_PERMITS
					 SET IMPORT_STATUS = 'ERROR',
							 IMPORT_ERROR_MSG =
								 'Duplicate Structure_Id''s found on these features: ' ||
								 RTRIM																										(
								 tmpFids,
								 ',')
				 WHERE structure_id = rec.STRUCTURE_ID;
			-- dbms_output.put_line('Set Import_Status to error: ' || rec.STRUCTURE_ID);
			END IF;
		END LOOP;

		COMMIT;

		-- Log Duplicate Structure Ids.

		IF LENGTH(duplicatesInGis) > 5
		THEN
			INSERT
				INTO GIS_STG.LOG_TRC_IMPORT(IMPORT_ERROR_TYPE,
																		IMPORT_ERROR_MSG,
																		IMPORT_SESSION_ID)
			VALUES (
							 'PERM_DUP_STRUCT_ID_GIS',

							 'These Structure_Id''s have duplicate Structure_Id''s in the GIS database:'
							 || RTRIM(duplicatesInGis, ','),
							 currRunId);
		END IF;

		IF LENGTH(structsNotFoundInGis) > 5
		THEN
			INSERT
				INTO GIS_STG.LOG_TRC_IMPORT(IMPORT_ERROR_TYPE,
																		IMPORT_ERROR_MSG,
																		IMPORT_SESSION_ID)
			VALUES (
							 'PERM_STRUCT_NOT_IN_GIS',

							 'These Structure_Id''s have were not found in the GIS database:'
							 || RTRIM(structsNotFoundInGis, ','),
							 currRunId);
		END IF;

		-- commit inserts and updates to the log table and
		COMMIT;

		--Truncate the GIS.TRC_ACTIVE_PERMITS;
		--execute immediate ('TRUNCATE TABLE GIS.TRC_ACTIVE_PERMITS drop storage');
		DELETE FROM GIS.TRC_ACTIVE_PERMITS;

		COMMIT;

		-- dbms_output.put_line('Truncated GIS.TRC_ACTIVE_PERMITS');

		-- Add new records to the gis.TRC_ACTIVE_PERMITS table.
		INSERT INTO gis.TRC_ACTIVE_PERMITS(STRUCTURE_ID, PERMIT_NUMBER)
			SELECT STRUCTURE_ID, PERMIT_NUMBER
				FROM GIS_STG.STG_TRC_ACTIVE_PERMITS
			 WHERE PROCESS_RUN_ID = currRunId AND IMPORT_STATUS <> 'ERROR';

		-- dbms_output.put_line('Did inserts to gis.TRC_ACTIVE_PERMITS');
		-- update the GIS_STG.STG_TRC_ACTIVE_PERMITS to show process was completed.
		UPDATE GIS_STG.STG_TRC_ACTIVE_PERMITS
			 SET import_status = 'ADDED'
		 WHERE PROCESS_RUN_ID = currRunId AND IMPORT_STATUS <> 'ERROR';

		UPDATE GIS_STG.STG_TRC_ACTIVE_PERMITS
			 SET IMPORT_DATE_TIME = SYSDATE
		 WHERE PROCESS_RUN_ID = currRunId;

		COMMIT;

		EmailPermitErrors(duplicatesInGis, structsNotFoundInGis, currRunId);

		IF LENGTH(duplicatesInGis) > 2 OR LENGTH(structsNotFoundInGis) > 2
		THEN
			tmpSuccess := FALSE;
		END IF;

		LogEndOfProcess('Refresh Active Permits', tmpSuccess);

		RETURN tmpReturnVal;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('RefreshActivePermits', SQLCODE, SQLERRM);
			RETURN tmpReturnVal;
			RAISE;
	END;

	-- This procedure validates the column data in the staging table for the given record
	PROCEDURE ValidateColumnData(attachRec GIS_STG.STG_TRC_ATTACHMENT%ROWTYPE)
	IS
		tmpMDError VARCHAR2(500);
		tmpIDError VARCHAR2(500);
		bMissingData BOOLEAN := FALSE;
		bIncorrectData BOOLEAN := FALSE;
		tmpValErr VARCHAR2(500);
	BEGIN
		-- Check for required Attributes.
		IF attachRec.RECORD_TYPE IS NULL
		THEN
			tmpMDError := tmpMDError || 'RECORD_TYPE,';
			bMissingData := TRUE;
		ELSE
			IF attachRec.RECORD_TYPE <> 'E' AND attachRec.RECORD_TYPE <> 'W'
			THEN
				tmpIDError := tmpIDError || 'RECORD_TYPE ,';
				bIncorrectData := TRUE;
			ELSE
				IF attachRec.record_type = 'E'
				THEN
					-- Attach_position test.
					IF attachRec.Attach_position IS NULL
					THEN
						tmpMDError := tmpMDError || 'Attach_Position,';
						bMissingData := TRUE;
					ELSE
						--												if upper(attachRec.Attach_position) <> 'TOP OF POLE' and upper(attachRec.Attach_position) <> 'POWER SPACE' and
						--													 upper(attachRec.Attach_position) <> 'COMMS SPACE' and upper(attachRec.Attach_position) <> 'BRACKET ARM'
						IF UPPER(attachRec.Attach_position) NOT IN
								 ('TOP OF POLE', 'POWER SPACE', 'COMMS SPACE', 'BRACKET ARM')
						THEN
							tmpIDError := tmpIDError || 'Attach_Position,';
							bIncorrectData := TRUE;
						END IF;
					END IF;

					-- Bracket_arm test.
					IF attachRec.ATTACH_POSITION = 'Bracket Arm'
				 AND attachRec.E_BRACKET_ARM IS NULL
					THEN
						tmpMDError := tmpMDError || 'E_BRACKET_ARM ,';
						bMissingData := TRUE;
					ELSE
						IF attachRec.ATTACH_POSITION = 'Bracket Arm'
					 AND UPPER(attachRec.E_BRACKET_ARM) NOT IN ('YES', 'NO', 'UNK')
						THEN
							tmpIDError := tmpIDError || 'E_BRACKET_ARM,';
							bIncorrectData := TRUE;
						END IF;
					END IF;
				END IF;
			END IF;
		END IF;

		IF attachRec.STRUCTURE_ID IS NULL
		THEN
			tmpMDError := tmpMDError || 'STRUCTURE_ID,';
			bMissingData := TRUE;
		END IF;

		IF attachRec.ATTACH_COMPANY IS NULL
		THEN
			tmpMDError := tmpMDError || 'ATTACH_COMPANY,';
			bMissingData := TRUE;
		END IF;

		IF attachRec.ATTACH_TYPE IS NULL
		THEN
			tmpMDError := tmpMDError || 'ATTACH_TYPE,';
			bMissingData := TRUE;
		END IF;

		IF attachRec.ATTACH_TYPE IS NULL
		THEN
			tmpMDError := tmpMDError || 'ATTACH_TYPE,';
			bMissingData := TRUE;
		END IF;

		IF attachRec.PERMIT_NUMBER IS NULL
		THEN
			tmpMDError := tmpMDError || 'PERMIT_NUMBER,';
			bMissingData := TRUE;
		END IF;

		IF attachRec.ATTACHMENT_STATUS IS NULL
		THEN
			tmpMDError := tmpMDError || 'ATTACHMENT_STATUS,';
			bMissingData := TRUE;
		ELSE
			--						if upper(attachRec.ATTACHMENT_STATUS) <> 'ACTIVE'
			--							 and upper(attachRec.ATTACHMENT_STATUS) <> 'ABANDONED'
			--							 and upper(attachRec.ATTACHMENT_STATUS) <> 'REMOVED'
			IF UPPER(attachRec.ATTACHMENT_STATUS) NOT IN
					 ('ACTIVE', 'ABANDONED', 'REMOVED')
			THEN
				tmpIDError := tmpIDError || 'ATTACHMENT_STATUS ,';
				bIncorrectData := TRUE;
			END IF;
		END IF;

		IF attachRec.INSPECTION_DATE IS NULL
		THEN
			tmpMDError := tmpMDError || 'INSPECTION_DATE,';
			bMissingData := TRUE;
		END IF;

		IF bMissingData = TRUE
		THEN
			tmpValErr :=
				'Required field//s missing data:' || RTRIM(tmpMDError, ',') || '.'
				|| CHR(10);
		END IF;

		IF bIncorrectData = TRUE
		THEN
			tmpValErr :=
				tmpValErr || 'Required field//s with invalid data:' || RTRIM(
				tmpIDError,
				',') || '.';
		END IF;

		-- log the errors
		IF bMissingData = TRUE OR bIncorrectData = TRUE
		THEN
			UPDATE GIS_STG.STG_TRC_ATTACHMENT
				 SET IMPORT_STATUS = 'ERROR',
						 IMPORT_ERROR_MSG = tmpValErr,
						 IMPORT_SESSION_ID = SYS_CONTEXT('USERENV', 'SID'),
						 IMPORT_ERROR_TYPE = 'INVALID_RECORD',
						 IMPORT_DATE_TIME = SYSDATE
			 WHERE STG_TRC_ATTACHMENT_ID = attachRec.STG_TRC_ATTACHMENT_ID;

			INSERT INTO GIS_STG.LOG_TRC_IMPORT(RECORD_TYPE,
																				 STRUCTURE_ID,
																				 ATTACH_COMPANY,
																				 ATTACH_TYPE,
																				 PERMIT_NUMBER,
																				 ATTACH_HEIGHT_FT,
																				 ATTACH_POSITION,
																				 ATTACHMENT_STATUS,
																				 C_MESSENGER,
																				 C_INIT_STR_TENSION,
																				 C_OUTSIDE_DIAM,
																				 E_WEIGHT,
																				 E_BRACKET_ARM,
																				 INSPECTION_DATE,
																				 IMPORT_DATE_TIME,
																				 IMPORT_SESSION_ID,
																				 IMPORT_STATUS,
																				 IMPORT_ERROR_MSG,
																				 IMPORT_ERROR_TYPE)
			VALUES (attachRec.RECORD_TYPE,
							attachRec.STRUCTURE_ID,
							attachRec.ATTACH_COMPANY,
							attachRec.ATTACH_TYPE,
							attachRec.PERMIT_NUMBER,
							attachRec.ATTACH_HEIGHT_FT,
							attachRec.ATTACH_POSITION,
							attachRec.ATTACHMENT_STATUS,
							attachRec.C_MESSENGER,
							attachRec.C_INIT_STR_TENSION,
							attachRec.C_OUTSIDE_DIAM,
							attachRec.E_WEIGHT,
							attachRec.E_BRACKET_ARM,
							attachRec.INSPECTION_DATE,
							SYSDATE,
							SYS_CONTEXT('USERENV', 'SID'),
							'ERROR',
							tmpValErr,
							'INVALID_RECORD');
		END IF;
	--commit;attachRec.C_INIT_STR_TENSION

	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('ValidateColumnData', SQLCODE, SQLERRM);
	END;

	FUNCTION BestMatch(RecordType 	 VARCHAR2,
										 stgRec 			 GIS_STG.STG_TRC_ATTACHMENT%ROWTYPE,
										 tmpFid 			 NUMBER)
		RETURN NUMBER
	IS
		TYPE arrAttEqpt IS VARRAY(20) OF gis.ATTACH_EQPT_N%ROWTYPE;

		TYPE arrAttWire IS VARRAY(20) OF gis.ATTACH_WIRELINE_N%ROWTYPE;

		tmpAttEqptLst arrAttEqpt;
		tmpAttWireLst arrAttWire;
		tmpCID NUMBER;
		tmpCompanyNo NUMBER;
		tmpMatchCnt NUMBER;
		tmpSavedCnt NUMBER := 0;
		aeRow GIS_STG.STG_TRC_ATTACHMENT%ROWTYPE;
	BEGIN
		tmpCompanyNo := LookupCoNum(stgRec.ATTACH_COMPANY);

		IF stgRec.RECORD_TYPE = 'E'
		THEN
			SELECT *
				BULK COLLECT INTO tmpAttEqptLst
				FROM ATTACH_EQPT_N
			 WHERE g3e_fid = tmpFid AND E_ATTACH_COMPANY = tmpCompanyNo;

			tmpMatchCnt := 0;

			FOR rIdx IN 1 .. tmpAttEqptLst.COUNT
			LOOP
				IF tmpAttEqptLst(rIdx).E_ATTACH_TYPE_C = stgRec.ATTACH_TYPE
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttEqptLst(rIdx).E_PERMIT_NUMBER = stgRec.PERMIT_NUMBER
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttEqptLst(rIdx).E_ATTACH_HEIGHT_FT = stgRec.ATTACH_HEIGHT_FT
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttEqptLst(rIdx).E_ATTACH_POSITION_C = stgRec.ATTACH_POSITION
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttEqptLst(rIdx).E_ATTACHMENT_STATUS = stgRec.ATTACHMENT_STATUS
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttEqptLst(rIdx).E_WEIGHT = stgRec.E_WEIGHT
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttEqptLst(rIdx).E_BRACKET_ARM = stgRec.E_BRACKET_ARM
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpMatchCnt > tmpSavedCnt
				THEN
					tmpSavedCnt := tmpMatchCnt;
					tmpCid := tmpAttEqptLst(rIdx).G3E_CID;
				END IF;

				tmpMatchCnt := 0;
			END LOOP;
		END IF;

		IF stgRec.RECORD_TYPE = 'W'
		THEN
			SELECT *
				BULK COLLECT INTO tmpAttWireLst
				FROM ATTACH_WIRELINE_N
			 WHERE g3e_fid = tmpFid AND W_ATTACH_COMPANY = tmpCompanyNo;

			tmpMatchCnt := 0;

			FOR rIdx IN 1 .. tmpAttWireLst.COUNT
			LOOP
				IF tmpAttWireLst(rIdx).W_ATTACH_TYPE = stgRec.ATTACH_TYPE
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_PERMIT_NUMBER = stgRec.PERMIT_NUMBER
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_ATTACH_HEIGHT_FT = stgRec.ATTACH_HEIGHT_FT
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_ATTACH_POSITION = stgRec.ATTACH_POSITION
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_ATTACHMENT_STATUS = stgRec.ATTACHMENT_STATUS
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_MESSENGER_C = stgRec.C_MESSENGER
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_INIT_STR_TENSION = stgRec.C_INIT_STR_TENSION
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpAttWireLst(rIdx).W_OUTSIDE_DIAM = stgRec.C_OUTSIDE_DIAM
				THEN
					tmpMatchCnt := tmpMatchCnt + 1;
				END IF;

				IF tmpMatchCnt > tmpSavedCnt
				THEN
					tmpSavedCnt := tmpMatchCnt;
					tmpCid := tmpAttWireLst(rIdx).G3E_CID;
				END IF;
			END LOOP;
		END IF;

		RETURN tmpCid;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('BestMatch', SQLCODE, SQLERRM);
			RETURN 0;
	END;

	FUNCTION InsertAttachment(tmpRec		GIS_STG.STG_TRC_ATTACHMENT%ROWTYPE,
														nFid			NUMBER)
		RETURN VARCHAR2
	IS
		tmpCid NUMBER;
		tmpAttDate DATE;
		tmpAbnDate DATE := NULL;
		tmpFno NUMBER;
		tmpStatus VARCHAR2(10);
		tmpCompanyNo NUMBER;
	BEGIN
		tmpCompanyNo := LookupCoNum(tmpRec.ATTACH_COMPANY);

		IF tmpRec.Record_Type = 'E'
		THEN
			BEGIN
				-- Get the next G3E_CID for the attachment to be added.
				SELECT MAX(g3e_cid), g3e_fno
					INTO tmpCid, tmpFno
					FROM gis.ATTACH_EQPT_N
				 WHERE g3e_fid = nFid
				GROUP BY g3e_fno;
			EXCEPTION
				WHEN NO_DATA_FOUND
				THEN
					tmpCID := 0;

					SELECT G3E_FNO
						INTO tmpFno
						FROM GIS.COMMON_N
					 WHERE g3e_fid = nFid;

					NULL;
				WHEN OTHERS
				THEN
					logSysError('AddUpdateAttachments',
											SQLCODE,
											'Current Structure_Id: ' || tmpRec.STRUCTURE_ID || ' '
											|| SQLERRM);
					NULL;
			END;

			tmpCid := tmpCid + 1;

			DBMS_OUTPUT.put_line(' next CID: ' || tmpCid);

			IF UPPER(tmpRec.ATTACHMENT_STATUS) = 'ACTIVE'
			THEN
				tmpAttDate := tmpRec.INSPECTION_DATE;
				tmpAbnDate := NULL;
			END IF;

			IF UPPER(tmpRec.ATTACHMENT_STATUS) = 'ABANDONED'
			THEN
				tmpAttDate := NULL;
				tmpAbnDate := tmpRec.INSPECTION_DATE;
			END IF;

			DBMS_OUTPUT.put_line(' FNO: ' || tmpFno);
			DBMS_OUTPUT.put_line(' CNO: 35');
			DBMS_OUTPUT.put_line(' FID: ' || nFid);

			INSERT INTO gis.ATTACH_EQPT_N(G3E_FNO,
																		G3E_CNO,
																		G3E_FID,
																		G3E_CID,
																		E_ATTACH_COMPANY,
																		E_ATTACH_TYPE_C,
																		E_PERMIT_NUMBER,
																		E_ATTACH_HEIGHT_FT,
																		E_ATTACH_POSITION_C,
																		E_ATTACHMENT_STATUS,
																		E_ATTACHED_D,
																		E_ABANDON_D,
																		E_ATTACH_SOURCE_C,
																		E_WEIGHT,
																		E_BRACKET_ARM)
			VALUES (tmpFno,
							35,
							nFid,
							tmpCid,
							tmpCompanyNo,
							tmpRec.ATTACH_TYPE,
							tmpRec.PERMIT_NUMBER,
							tmpRec.ATTACH_HEIGHT_FT,
							tmpRec.ATTACH_POSITION,
							tmpRec.ATTACHMENT_STATUS,
							tmpAttDate,
							tmpAbnDate,
							'Audit',
							tmpRec.E_WEIGHT,
							tmpRec.E_BRACKET_ARM);

			addToModAttacments(tmpCid, 'E', tmpRec.Structure_Id);
			tmpStatus := 'ADDED';
		END IF;

		IF tmpRec.Record_Type = 'W'
		THEN
			BEGIN
				--dbms_output.put_Line('AddUpdAtt - Add W Attachment');
				SELECT MAX(g3e_cid), g3e_fno
					INTO tmpCid, tmpFno
					FROM gis.ATTACH_WIRELINE_N
				 WHERE g3e_fid = nFid
				GROUP BY g3e_fno;
			EXCEPTION
				WHEN NO_DATA_FOUND
				THEN
					tmpCID := 0;

					SELECT G3E_FNO
						INTO tmpFno
						FROM GIS.COMMON_N
					 WHERE g3e_fid = nFid;

					NULL;
				WHEN OTHERS
				THEN
					logSysError('AddUpdateAttachments',
											SQLCODE,
											'Current Structure_Id: ' || tmpRec.STRUCTURE_ID || ' '
											|| SQLERRM);
					NULL;
			END;

			--dbms_output.put_Line('AddUpdAtt - tmpCid is: '||tmpCid);
			tmpCid := tmpCid + 1;

			IF UPPER(tmpRec.ATTACHMENT_STATUS) = 'ACTIVE'
			THEN
				tmpAttDate := tmpRec.INSPECTION_DATE;
				tmpAbnDate := NULL;
			END IF;

			IF UPPER(tmpRec.ATTACHMENT_STATUS) = 'ABANDONED'
			THEN
				tmpAttDate := NULL;
				tmpAbnDate := tmpRec.INSPECTION_DATE;
			END IF;

			INSERT INTO gis.ATTACH_WIRELINE_N(G3E_FNO,
																				G3E_CNO,
																				G3E_FID,
																				G3E_CID,
																				W_ATTACH_COMPANY,
																				W_ATTACH_TYPE,
																				W_PERMIT_NUMBER,
																				W_ATTACH_HEIGHT_FT,
																				W_ATTACH_POSITION,
																				W_ATTACHMENT_STATUS,
																				W_ATTACHED_D,
																				W_ABANDON_D,
																				W_ATTACH_SOURCE_C,
																				W_MESSENGER_C,
																				W_INIT_STR_TENSION,
																				W_OUTSIDE_DIAM)
			VALUES (tmpFno,
							34,
							nFid,
							tmpCid,
							tmpCompanyNo,
							tmpRec.ATTACH_TYPE,
							tmpRec.PERMIT_NUMBER,
							tmpRec.ATTACH_HEIGHT_FT,
							tmpRec.ATTACH_POSITION,
							tmpRec.ATTACHMENT_STATUS,
							tmpAttDate,
							tmpAbnDate,
							'Audit',
							tmpRec.C_MESSENGER,
							tmpRec.C_INIT_STR_TENSION,
							tmpRec.C_OUTSIDE_DIAM);

			tmpStatus := 'ADDED';
			addToModAttacments(tmpCid, 'W', tmpRec.Structure_Id);
		END IF;

		RETURN tmpStatus;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('InsertAttachment',
									SQLCODE,
									'Current Structure_Id: ' || tmpRec.STRUCTURE_ID || ' ' ||
									SQLERRM);
			NULL;
			RETURN NULL;
	END;

	FUNCTION UpdateAttachment(tmpRec		GIS_STG.STG_TRC_ATTACHMENT%ROWTYPE,
														nFid			NUMBER,
														nCid			NUMBER)
		RETURN VARCHAR2
	IS
		tmpCid NUMBER;
		tmpCompanyNo NUMBER;
		tmpStatus VARCHAR2(20);
		tmpAbnDate DATE := NULL;
		tmpRemDate DATE := NULL;
		tmpAttDate DATE;
	BEGIN
		tmpCid := nCid;

		IF UPPER(tmpRec.ATTACHMENT_STATUS) = 'REMOVED'
		THEN
			tmpRemDate := tmpRec.INSPECTION_DATE;
			tmpAbnDate := NULL;
		END IF;

		IF UPPER(tmpRec.ATTACHMENT_STATUS) = 'ABANDONED'
		THEN
			tmpRemDate := NULL;
			tmpAbnDate := tmpRec.INSPECTION_DATE;
		END IF;

		tmpCompanyNo := LookupCoNum(tmpRec.ATTACH_COMPANY);

		-- update Equipment attachment
		IF tmpRec.Record_type = 'E'
		THEN
			UPDATE gis.ATTACH_EQPT_N ae
				 SET E_ATTACH_TYPE_C = tmpRec.ATTACH_TYPE,
						 E_PERMIT_NUMBER = tmpRec.PERMIT_NUMBER,
						 E_ATTACH_HEIGHT_FT = tmpRec.ATTACH_HEIGHT_FT,
						 E_ATTACH_POSITION_C = tmpRec.ATTACH_POSITION,
						 E_ATTACHMENT_STATUS = tmpRec.ATTACHMENT_STATUS,
						 E_ATTACH_SOURCE_C = 'Audit',
						 E_WEIGHT = tmpRec.E_WEIGHT,
						 E_BRACKET_ARM = tmpRec.E_BRACKET_ARM,
						 E_ABANDON_D = tmpAbnDate,
						 E_REMOVED_D = tmpRemDate
			 WHERE ae.E_ATTACH_COMPANY = tmpCompanyNo
				 AND ae.g3e_cid = tmpCid
				 AND ae.g3e_fid = nFid;

			SELECT g3e_cid
				INTO tmpCid
				FROM gis.ATTACH_EQPT_N
			 WHERE E_ATTACH_COMPANY = tmpCompanyNo AND g3e_fid = nFid;

			addToModAttacments(tmpCid, 'E', tmpRec.Structure_Id);
			tmpStatus := 'UPDATED';
		END IF;

		-- update Wireline attachment
		IF tmpRec.Record_type = 'W'
		THEN
			UPDATE gis.ATTACH_WIRELINE_N
				 SET W_ATTACH_TYPE = tmpRec.ATTACH_TYPE,
						 W_PERMIT_NUMBER = tmpRec.PERMIT_NUMBER,
						 W_ATTACH_HEIGHT_FT = tmpRec.ATTACH_HEIGHT_FT,
						 W_ATTACH_POSITION = tmpRec.ATTACH_POSITION,
						 W_ATTACHMENT_STATUS = tmpRec.ATTACHMENT_STATUS,
						 W_ATTACH_SOURCE_C = 'Audit',
						 W_MESSENGER_C = tmpRec.C_MESSENGER,
						 W_INIT_STR_TENSION = tmpRec.C_INIT_STR_TENSION,
						 W_OUTSIDE_DIAM = tmpRec.C_OUTSIDE_DIAM,
						 W_ABANDON_D = tmpAbnDate,
						 W_REMOVED_D = tmpRemDate
			 WHERE W_ATTACH_COMPANY = tmpCompanyNo
				 AND g3e_cid = tmpCid
				 AND g3e_fid = nFid;

			tmpStatus := 'UPDATED';
			addToModAttacments(tmpCid, 'W', tmpRec.Structure_Id);
		END IF;

		RETURN tmpStatus;
	END;


	PROCEDURE AddUpdateAttachments(
		tmpRec		GIS_STG.STG_TRC_ATTACHMENT%ROWTYPE,
		nFid			NUMBER)
	IS
		tmpCnt NUMBER;
		tmpCnt2 NUMBER;
		tmpCnt3 NUMBER;
		tmpFoundCnt NUMBER;
		tmpCid NUMBER;
		tmpFno NUMBER;
		tmpCompanyNo NUMBER;
		tmpAttDate DATE;
		tmpAbnDate DATE := NULL;
		tmpRemDate DATE := NULL;
		tmpStatus VARCHAR2(10);
	BEGIN
		tmpCompanyNo := LookupCoNum(tmpRec.ATTACH_COMPANY);
		DBMS_OUTPUT.put_line('Company No for Company: ' || tmpRec.ATTACH_COMPANY
												 || ' is ' || tmpCompanyNo);
		DBMS_OUTPUT.put_line('  Record type is : ' || tmpRec.RECORD_TYPE);
		DBMS_OUTPUT.put_line('  G3E_FID : ' || nFid);
		DBMS_OUTPUT.put_line('  Structure Id : ' || tmpRec.STRUCTURE_ID);

		IF tmpCompanyNo NOT IN (0, -1)
		THEN
			IF tmpRec.RECORD_TYPE = 'E'
			THEN
				-- See how many Attachments are on the structure with the same
				--		attaching company.
				SELECT COUNT(*)
					INTO tmpCnt
					FROM gis.ATTACH_EQPT_N ae
				 WHERE ae.E_ATTACH_COMPANY = tmpCompanyNo AND ae.g3e_fid = nFid;

				-- If none, add attachment.
				CASE
					WHEN tmpCnt = 0
					THEN
						-- add Attachment
						tmpStatus := InsertAttachment(tmpRec, nFid);
					WHEN tmpCnt > 0
					THEN
						DBMS_OUTPUT.put_Line('tmpCnt > 0');

						-- see if a distinct attacment found by adding Attach_type to filter.
						SELECT COUNT(*)
							INTO tmpCnt2
							FROM gis.ATTACH_EQPT_N
						 WHERE E_ATTACH_COMPANY = tmpCompanyNo
							 AND g3e_fid = nFid
							 AND E_ATTACH_TYPE_C = tmpRec.ATTACH_TYPE;

						-- update the attachment.
						CASE
							WHEN tmpCnt2 = 0
							THEN
								tmpStatus := InsertAttachment(tmpRec, nFid);
							WHEN tmpCnt2 = 1
							THEN
								DBMS_OUTPUT.put_Line('tmpCnt2 = 1');

								SELECT COUNT(*)
									INTO tmpCnt3
									FROM gis.ATTACH_EQPT_N a
								 WHERE E_ATTACH_COMPANY = tmpCompanyNo
									 AND g3e_fid = nFid
									 AND E_ATTACH_TYPE_C = tmpRec.ATTACH_TYPE
									 AND E_ATTACH_POSITION_C = tmpRec.ATTACH_POSITION;

								IF tmpCnt3 = 0
								THEN
									tmpStatus := InsertAttachment(tmpRec, nFid);
								END IF;

								IF tmpCnt3 = 1
								THEN
									DBMS_OUTPUT.put_Line('tmpCnt3 = 1');

									SELECT g3e_cid
										INTO tmpCid
										FROM gis.ATTACH_EQPT_N
									 WHERE E_ATTACH_COMPANY = tmpCompanyNo
										 AND g3e_fid = nFid
										 AND E_ATTACH_TYPE_C = tmpRec.ATTACH_TYPE
										 AND E_ATTACH_POSITION_C = tmpRec.ATTACH_POSITION;

									tmpStatus := UpdateAttachment(tmpRec, nFid, tmpCid);
								END IF;
							WHEN tmpCnt2 > 1
							THEN
								DBMS_OUTPUT.put_Line('tmpCnt2 > 1');
								tmpCid := BestMatch('E', tmpRec, nFid);
								tmpStatus := updateAttachment(tmpRec, nFid, tmpCid);
						END CASE;
				END CASE;
			END IF;

			IF tmpRec.RECORD_TYPE = 'W'
			THEN
				SELECT COUNT(*)
					INTO tmpCnt
					FROM gis.ATTACH_WIRELINE_N
				 WHERE W_ATTACH_COMPANY = tmpCompanyNo AND g3e_fid = nFid;

				DBMS_OUTPUT.put_Line('tmpCnt =' || tmpCnt);

				CASE
					WHEN tmpCnt = 0
					THEN
						--add Attachment
						tmpStatus := InsertAttachment(tmpRec, nFid);
					WHEN tmpCnt > 0
					THEN
						DBMS_OUTPUT.put_Line('tmpCnt > 0');

						SELECT COUNT(*)
							INTO tmpCnt2
							FROM gis.ATTACH_WIRELINE_N
						 WHERE W_ATTACH_COMPANY = tmpCompanyNo
							 AND g3e_fid = nFid
							 AND W_ATTACH_TYPE = tmpRec.ATTACH_TYPE;

						DBMS_OUTPUT.put_Line('tmpCnt2 =' || tmpCnt2);

						-- update the attachment.
						CASE
							WHEN tmpCnt2 = 0
							THEN
								-- No Attachments found. Add an attachement.
								tmpStatus := InsertAttachment(tmpRec, nFid);
							WHEN tmpCnt2 = 1
							THEN
								DBMS_OUTPUT.put_Line('tmpCnt2 = 1');

								SELECT COUNT(*)
									INTO tmpCnt3
									FROM gis.ATTACH_WIRELINE_N a
								 WHERE W_ATTACH_COMPANY = tmpCompanyNo
									 AND g3e_fid = nFid
									 AND W_ATTACH_TYPE = tmpRec.ATTACH_TYPE
									 AND W_ATTACH_POSITION = tmpRec.ATTACH_POSITION;

								DBMS_OUTPUT.put_Line('tmpCnt3 =' || tmpCnt3);

								IF tmpCnt3 = 0
								THEN
									tmpStatus := InsertAttachment(tmpRec, nFid);
								END IF;

								IF tmpCnt3 = 1
								THEN
									DBMS_OUTPUT.put_Line('tmpCnt3 = 1');

									SELECT g3e_cid
										INTO tmpCid
										FROM gis.ATTACH_WIRELINE_N
									 WHERE W_ATTACH_COMPANY = tmpCompanyNo
										 AND g3e_fid = nFid
										 AND W_ATTACH_TYPE = tmpRec.ATTACH_TYPE
										 AND W_ATTACH_POSITION = tmpRec.ATTACH_POSITION;

									tmpStatus := UpdateAttachment(tmpRec, nFid, tmpCid);
								END IF;
							WHEN tmpCnt2 > 1
							THEN
								DBMS_OUTPUT.put_Line('tmpCnt2 > 1');
								DBMS_OUTPUT.put_Line('starting best match');
								tmpCid := BestMatch('W', tmpRec, nFid);
								DBMS_OUTPUT.put_line('after best match. tmpCid = ' || tmpCid);
								tmpStatus := updateAttachment(tmpRec, nFid, tmpCid);
						END CASE;
				END CASE;
			END IF;

			UPDATE gis_stg.STG_TRC_ATTACHMENT a
				 SET a.IMPORT_STATUS = tmpStatus
			 WHERE a.STG_TRC_ATTACHMENT_ID = tmpRec.STG_TRC_ATTACHMENT_ID;
		ELSE
			logSysError('AddUpdateAttachments',
									2000,
									'Company Number not found for ' || tmpRec.ATTACH_COMPANY);

			UPDATE gis_stg.STG_TRC_ATTACHMENT a
				 SET a.IMPORT_ERROR_MSG =
							 'Company Number not found for ' || tmpRec.ATTACH_COMPANY,
						 a.IMPORT_ERROR_TYPE = 'INVALID_RECORD',
						 a.IMPORT_STATUS = 'ERROR'
			 WHERE a.STG_TRC_ATTACHMENT_ID = tmpRec.STG_TRC_ATTACHMENT_ID;
		END IF;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('AddUpdateAttachments',
									SQLCODE,
									'Current Structure_Id: ' || tmpRec.STRUCTURE_ID || ' ' ||
									SQLERRM);
			NULL;
	END;

	FUNCTION getListForErrorType(vErrorType VARCHAR2, vSessionId NUMBER)
		RETURN VARCHAR2
	IS
		tmpList VARCHAR2(32000);
	BEGIN
		-- select sys_context('USERENV','SID') into tmpSessionId from dual;

		CASE
			WHEN vErrorType = 'SYS_ERROR'
			THEN
				FOR rec
					IN (SELECT IMPORT_ERROR_MSG
								FROM LOG_TRC_IMPORT
							 WHERE IMPORT_SESSION_ID = vSessionId
								 AND IMPORT_ERROR_TYPE = vErrorType
								 AND IMPORT_SESSION_ID = vSessionId)
				LOOP
					IF LENGTH(tmpList) < 31000
					THEN
						tmpList := tmpList || rec.IMPORT_ERROR_MSG || CHR(10);
					ELSE
						tmpList := tmpList || '...';
					END IF;
				END LOOP;
			WHEN vErrorType = 'TRC_IMP_UNK_ERROR'
			THEN
				FOR rec
					IN (SELECT IMPORT_ERROR_MSG
								FROM LOG_TRC_IMPORT
							 WHERE IMPORT_SESSION_ID = vSessionId
								 AND IMPORT_ERROR_TYPE = vErrorType
								 AND IMPORT_SESSION_ID = vSessionId)
				LOOP
					IF LENGTH(tmpList) < 31000
					THEN
						tmpList := tmpList || rec.IMPORT_ERROR_MSG || CHR(10);
					ELSE
						tmpList := tmpList || '...' || CHR(10);
					END IF;
				END LOOP;
			ELSE
				FOR rec
					IN (SELECT STRUCTURE_ID
								FROM LOG_TRC_IMPORT
							 WHERE IMPORT_SESSION_ID = vSessionId
								 AND IMPORT_ERROR_TYPE = vErrorType
								 AND IMPORT_SESSION_ID = vSessionId)
				LOOP
					tmpList := tmpList || rec.STRUCTURE_ID || ',';
				END LOOP;
		END CASE;

		RETURN RTRIM(tmpList, ',');
	EXCEPTION
		WHEN NO_DATA_FOUND
		THEN
			RETURN NULL;
		WHEN OTHERS
		THEN
			logSysError('getListForErrorType', SQLCODE, SQLERRM);
			RETURN NULL;
			RAISE;
	END;


	PROCEDURE EmailTRCAttachErrors(vSessionid NUMBER)
	IS
		lSysErr VARCHAR2(4000);
		lStrNotInGis VARCHAR2(4000);
		lDupStrInGis VARCHAR2(4000);
		lAttNotFndInFld VARCHAR2(4000);
		lInvRec VARCHAR2(4000);
		lUnkErr VARCHAR2(4000);
		tmpSubject VARCHAR2(100);
		tmpToAddrs VARCHAR2(400);
		tmpFromAddr VARCHAR2(100);
		tmpEMMsg VARCHAR2(32000);
	BEGIN
		-- get the listS

		lSysErr := getListForErrorType('SYS_ERR', vSessionid);
		lStrNotInGis := getListForErrorType('STRUCT_NOT_IN_GIS', vSessionid);
		lDupStrInGis := getListForErrorType('DUP_STRUCT_ID_GIS', vSessionid);
		lAttNotFndInFld :=
			getListForErrorType('ATCH_NOT_FOUND_IN_FLD', vSessionid);
		lInvRec := getListForErrorType('INVALID_RECORD', vSessionid);
		lUnkErr := getListForErrorType('TRC_IMP_UNK_ERROR', vSessionid);

		SELECT Param_Value
			INTO tmpFromAddr
			FROM sys_generalparameter
		 WHERE subsystem_name = 'TRC_IMPORT'
			 AND subsystem_component = 'ErrorLoggingMail'
			 AND param_name = 'AttachFromAddr';

		tmpSubject := 'TRC Import Attachment Errors.';

		-- email Joint Use distribution list
		IF lStrNotInGis IS NOT NULL
		OR	lDupStrInGis IS NOT NULL
		OR	lAttNotFndInFld IS NOT NULL
		OR	lInvRec IS NOT NULL
		OR	lUnkErr IS NOT NULL
		THEN
			tmpToAddrs := 'JOINT_USE_DIS_LIST';

			IF lStrNotInGis IS NOT NULL
			THEN
				tmpEMMsg :=
					'List of Structures not found in GIS: ' || CHR(10) || lStrNotInGis
					|| CHR(10);
			END IF;

			IF lDupStrInGis IS NOT NULL
			THEN
				tmpEMMsg :=
					tmpEMMsg || 'List of Structures in GIS that have duplicate Ids: '
					|| CHR(10) || lDupStrInGis || CHR(10);
			END IF;

			IF lAttNotFndInFld IS NOT NULL
			THEN
				tmpEMMsg :=
					tmpEMMsg ||
					'List of Structure Ids that were not found in the field: ' || CHR(
					10) || lAttNotFndInFld || CHR(10);
			END IF;

			IF lInvRec IS NOT NULL
			THEN
				tmpEMMsg :=
					tmpEMMsg ||
					'Unknown errors occuring during TRC Import Attachment Process: '
					|| CHR(10) || lInvRec || CHR(10);
			END IF;

			IF lUnkErr IS NOT NULL
			THEN
				tmpEMMsg :=
					tmpEMMsg ||
					'Unknown errors occuring during TRC Import Attachment Process: '
					|| CHR(10) || lUnkErr || CHR(10);
			END IF;

			gis.SEND_EF_EMAIL_PKG.emToAddress := tmpToAddrs;
			gis.SEND_EF_EMAIL_PKG.emFromAddress := tmpFromAddr;
			gis.SEND_EF_EMAIL_PKG.emSubject := tmpSubject;
			gis.SEND_EF_EMAIL_PKG.emMessage := tmpEMMsg;
			GIS.SEND_EF_EMAIL_PKG.SENDEMAIL;
		END IF;

		-- email DIS Help Desk distribution list
		IF lSysErr IS NOT NULL OR lInvRec IS NOT NULL OR lUnkErr IS NOT NULL
		THEN
			IF lSysErr IS NOT NULL
			THEN
				tmpEMMsg :=
					'System errors occuring during TRC Import Attachment Process: ' ||
					CHR
					(10) || lSysErr || CHR(10);
			END IF;

			IF lInvRec IS NOT NULL
			THEN
				tmpEMMsg :=
					tmpEMMsg ||
					'Unknown errors occuring during TRC Import Attachment Process: '
					|| CHR(10) || lInvRec || CHR(10);
			END IF;

			IF lUnkErr IS NOT NULL
			THEN
				tmpEMMsg :=
					tmpEMMsg ||
					'Unknown errors occuring during TRC Import Attachment Process: '
					|| CHR(10) || lUnkErr || CHR(10);
			END IF;

			tmpToAddrs := 'DIS_HELP_DESK';
			gis.SEND_EF_EMAIL_PKG.emToAddress := tmpToAddrs;
			gis.SEND_EF_EMAIL_PKG.emFromAddress := tmpFromAddr;
			gis.SEND_EF_EMAIL_PKG.emSubject := tmpSubject;
			gis.SEND_EF_EMAIL_PKG.emMessage := tmpEMMsg;
			GIS.SEND_EF_EMAIL_PKG.SENDEMAIL;
		END IF;

		-- email IT Support
		IF lSysErr IS NOT NULL
		THEN
			tmpEMMsg :=
				'System errors occuring during TRC Import Attachment Process: ' ||
				CHR 																															 (
				10) || lSysErr || CHR(10);

			tmpToAddrs := 'IT_SUPPORT';
			gis.SEND_EF_EMAIL_PKG.emToAddress := tmpToAddrs;
			gis.SEND_EF_EMAIL_PKG.emFromAddress := tmpFromAddr;
			gis.SEND_EF_EMAIL_PKG.emSubject := tmpSubject;
			gis.SEND_EF_EMAIL_PKG.emMessage := tmpEMMsg;
			GIS.SEND_EF_EMAIL_PKG.SENDEMAIL;
		END IF;
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('EmailTRCAttachErrors', SQLCODE, SQLERRM);
			NULL;
	END;

	PROCEDURE ImportTRCAttachments
	IS
		tmpSessionId NUMBER;
		tmpValErr VARCHAR2(500);
		structsNotFoundInGis VARCHAR2(32000);
		duplicatesInGis VARCHAR2(32000);
		tmpCntInGis NUMBER;
		tmpFid NUMBER;
		vGisConfiguration VARCHAR2(30);
		currStructId VARCHAR2(30);
	BEGIN
		SELECT param_value
			INTO vGisConfiguration
			FROM sys_generalparameter
		 WHERE SUBSYSTEM_NAME = 'TRC_IMPORT'
			 AND SUBSYSTEM_COMPONENT = 'LttUserConfig'
			 AND PARAM_NAME = 'LttUserConfig';

		GIS.LTT_USER.SETCONFIGURATION(vGisConfiguration);

		LogStartOfProcess('Import TRC Attachments');

		SELECT SYS_CONTEXT('USERENV', 'SID') INTO tmpSessionId FROM DUAL;

		FOR rec IN (SELECT *
									FROM GIS_STG.STG_TRC_ATTACHMENT
								 WHERE IMPORT_STATUS IS NULL)
		LOOP
			-- check record for required fields
			ValidateColumnData(rec);
		END LOOP;

		--		dbms_output.put_line('Validated attachment records' );

		UPDATE gis_stg.STG_TRC_ATTACHMENT
			 SET IMPORT_DATE_TIME = SYSDATE,
					 IMPORT_SESSION_ID = tmpSessionId,
					 IMPORT_STATUS = 'PENDING'
		 WHERE IMPORT_STATUS IS NULL;

		COMMIT;

		--		dbms_output.put_line('Set status to Pending' );
		FOR rec
			IN (SELECT DISTINCT STRUCTURE_ID
						FROM GIS_STG.STG_TRC_ATTACHMENT
					 WHERE import_status = 'PENDING'
						 AND import_Session_ID = tmpSessionId)
		LOOP
			--				dbms_output.put_line('Adding Attachments' );
			currStructId := rec.STRUCTURE_ID;

			SELECT COUNT(*)
				INTO tmpCntInGis
				FROM common_n
			 WHERE structure_id = rec.STRUCTURE_ID
				 AND g3e_fno IN (110, 114)
				 AND FEATURE_STATE_C IN ('PPI', 'INI', 'CLS', 'ABI', 'PPR', 'PPA');

			CASE																 -- see if Structure is found in GIS
				WHEN tmpCntInGis = 0
				THEN																						-- Structure not found
					UPDATE GIS_STG.STG_TRC_ATTACHMENT
						 SET IMPORT_STATUS = 'ERROR',
								 IMPORT_ERROR_TYPE = 'STRUCT_NOT_IN_GIS',
								 IMPORT_ERROR_MSG = 'Structure not found'
					 WHERE STRUCTURE_ID = rec.STRUCTURE_ID
						 AND import_Session_ID = tmpSessionId;

					INSERT INTO GIS_STG.LOG_TRC_IMPORT(RECORD_TYPE,
																						 STRUCTURE_ID,
																						 ATTACH_COMPANY,
																						 ATTACH_TYPE,
																						 PERMIT_NUMBER,
																						 ATTACH_HEIGHT_FT,
																						 ATTACH_POSITION,
																						 ATTACHMENT_STATUS,
																						 C_MESSENGER,
																						 C_INIT_STR_TENSION,
																						 C_OUTSIDE_DIAM,
																						 E_WEIGHT,
																						 E_BRACKET_ARM,
																						 INSPECTION_DATE,
																						 IMPORT_DATE_TIME,
																						 IMPORT_SESSION_ID,
																						 IMPORT_STATUS,
																						 IMPORT_ERROR_MSG,
																						 IMPORT_ERROR_TYPE)
						(SELECT RECORD_TYPE,
										STRUCTURE_ID,
										ATTACH_COMPANY,
										ATTACH_TYPE,
										PERMIT_NUMBER,
										ATTACH_HEIGHT_FT,
										ATTACH_POSITION,
										ATTACHMENT_STATUS,
										C_MESSENGER,
										C_INIT_STR_TENSION,
										C_OUTSIDE_DIAM,
										E_WEIGHT,
										E_BRACKET_ARM,
										INSPECTION_DATE,
										IMPORT_DATE_TIME,
										IMPORT_SESSION_ID,
										IMPORT_STATUS,
										IMPORT_ERROR_MSG,
										IMPORT_ERROR_TYPE
							 FROM GIS_STG.STG_TRC_ATTACHMENT
							WHERE STRUCTURE_ID = rec.STRUCTURE_ID
								AND IMPORT_SESSION_ID = tmpSessionId);
				WHEN tmpCntInGis > 1
				THEN									-- More than one Structure found with current ID
					UPDATE GIS_STG.STG_TRC_ATTACHMENT
						 SET IMPORT_STATUS = 'ERROR',
								 IMPORT_ERROR_TYPE = 'DUP_STRUCT_ID_GIS ',
								 IMPORT_ERROR_MSG = 'Duplicate Structures found in GIS'
					 WHERE STRUCTURE_ID = rec.STRUCTURE_ID
						 AND IMPORT_SESSION_ID = tmpSessionId;

					INSERT INTO GIS_STG.LOG_TRC_IMPORT(RECORD_TYPE,
																						 STRUCTURE_ID,
																						 ATTACH_COMPANY,
																						 ATTACH_TYPE,
																						 PERMIT_NUMBER,
																						 ATTACH_HEIGHT_FT,
																						 ATTACH_POSITION,
																						 ATTACHMENT_STATUS,
																						 C_MESSENGER,
																						 C_INIT_STR_TENSION,
																						 C_OUTSIDE_DIAM,
																						 E_WEIGHT,
																						 E_BRACKET_ARM,
																						 INSPECTION_DATE,
																						 IMPORT_DATE_TIME,
																						 IMPORT_SESSION_ID,
																						 IMPORT_STATUS,
																						 IMPORT_ERROR_MSG,
																						 IMPORT_ERROR_TYPE)
						(SELECT RECORD_TYPE,
										STRUCTURE_ID,
										ATTACH_COMPANY,
										ATTACH_TYPE,
										PERMIT_NUMBER,
										ATTACH_HEIGHT_FT,
										ATTACH_POSITION,
										ATTACHMENT_STATUS,
										C_MESSENGER,
										C_INIT_STR_TENSION,
										C_OUTSIDE_DIAM,
										E_WEIGHT,
										E_BRACKET_ARM,
										INSPECTION_DATE,
										IMPORT_DATE_TIME,
										IMPORT_SESSION_ID,
										IMPORT_STATUS,
										IMPORT_ERROR_MSG,
										IMPORT_ERROR_TYPE
							 FROM GIS_STG.STG_TRC_ATTACHMENT
							WHERE STRUCTURE_ID = rec.STRUCTURE_ID
								AND IMPORT_SESSION_ID = tmpSessionId);
				ELSE						 -- Only one sturcture found with current structure_ID
					SELECT g3e_fid
						INTO tmpFid
						FROM common_n
					 WHERE structure_id = rec.STRUCTURE_ID AND g3e_fno IN (110, 114);

					FOR rec2
						IN (SELECT *
									FROM GIS_STG.STG_TRC_ATTACHMENT
								 WHERE STRUCTURE_ID = rec.STRUCTURE_ID
									 AND IMPORT_SESSION_ID = tmpSessionId
									 AND IMPORT_STATUS = 'PENDING')
					LOOP
						-- Add or update the attactment of the current record in the Staging table.
						AddUpdateAttachments(rec2, tmpFid);
					END LOOP;

					NotFoundInField(tmpFid, rec.STRUCTURE_ID);

					-- Clear the list of attachments updated or added for this Structure.
					DELETE FROM TRC_ALTERED_ATTACH_TMP;
			END CASE;
		END LOOP;

		EmailTRCAttachErrors(tmpSessionId);
	EXCEPTION
		WHEN OTHERS
		THEN
			logSysError('ImportTRCAttachments',
									SQLCODE,
									'Current Structure_Id: ' || currStructId || ' ' || SQLERRM);

			RAISE;
	END;
END TRC_IMPORT_PKG;
/
