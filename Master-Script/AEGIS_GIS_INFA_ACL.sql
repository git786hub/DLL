/*
  EdgeFrontier Web Service Access - Create the Access Control List (ACL), assign EdgeFrontier host, and grant connect priviledge
	
	Services/Port:
		GIS_CreateUpdateJob		8010
		GIS_UpdateWriteBackStatus		8011
		GIS_RequestBatch		8012
		GIS_CreateFieldTransaction		8013
		WMIS_SendBatchResults		8030
		WMIS_UpdateStatus		8031
		WMIS_WriteBack		8032
		NJUNS_CheckTicketStatus		8050
		NJUNS_SubmitTicket		8051
		NJUNS_MemberCodeUpdate		8052
		GIS_DEISTransaction		8060
		DEIS_GISTransactionResults		8061
		SendEmail	 	8089
*/

DECLARE
	-- Varchar array type
  TYPE VARCHAR2_LIST_TYPE IS	TABLE OF VARCHAR2(30);
	--Enter the network host to which this access control list will be assigned.
	EDGE_FRONTIER_HOST_VAR VARCHAR2(100);-- := 'odcdiswdapp07.build.corp.oncor.com';
	--Enter the user accounts or roles being granted or denied permissions. Must include at least one.
	PRINCIPAL_NAME_LIST VARCHAR2_LIST_TYPE := VARCHAR2_LIST_TYPE('GIS','GIS_INFA','GIS_ONC','GIS_STG');
BEGIN
	EDGE_FRONTIER_HOST_VAR := '&EDGEFRONTIER_HOST_NAME';
	IF PRINCIPAL_NAME_LIST.FIRST IS NOT NULL THEN		
		-- Grant privileges to the principals in PRINCIPAL_NAME_LIST
		FOR I IN PRINCIPAL_NAME_LIST.FIRST..PRINCIPAL_NAME_LIST.LAST
		LOOP
				DBMS_NETWORK_ACL_ADMIN.append_host_ace (
					host       => EDGE_FRONTIER_HOST_VAR, 					
					ace        => xs$ace_type(privilege_list => xs$name_list('connect','resolve','http'),
																		principal_name => PRINCIPAL_NAME_LIST(i),
																		principal_type => xs_acl.ptype_db)); 
		END LOOP;
	END IF;
	COMMIT;
END;

/

--Use the following queries for verification
--SELECT HOST,
--       LOWER_PORT,
--       UPPER_PORT,
--       ACL,
--       ACLID,
--       ACL_OWNER
--FROM   dba_host_acls
--ORDER BY host;
--SELECT host,
--       lower_port,
--       upper_port,
--       ace_order,
--       TO_CHAR(start_date, 'DD-MON-YYYY') AS start_date,
--       TO_CHAR(end_date, 'DD-MON-YYYY') AS end_date,
--       grant_type,
--       inverted_principal,
--       principal,
--       principal_type,
--       privilege
--FROM   dba_host_aces
--ORDER BY host, ace_order;