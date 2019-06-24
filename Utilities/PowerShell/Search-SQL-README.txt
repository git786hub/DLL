
*** Please do not change or alter code scricpt code ***

 This script is designed to search .sql files for SQL code such as "DROP TABLE" and .log files for warnings and errors. It can do both or either at once.


 Follow the below steps to execute the script:
 ----------------------------------------------


 STEP 1: CHECK YOUR SYSTEM AND CONFIGURATION REQUIREMENTS
 --------------------------------------------------------

   	* System/Configuration Requirements for running powershell scripts:
        	* 1 - Must be on Windows machine
	        * 2 - Must install Powershell Version 5.1 - https://www.microsoft.com/en-us/download/details.aspx?id=54616
        	* 3 - Must set EXecutionPolicy to 'Bypass' (might have to change registry key manually: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell)
	            * Run this command to set: 
			> Set-ExecutionPolicy Bypass -Force
		    * Run this command to check:
			> Get-ExecutionPolicy

 STEP 2: Update your PowerShell Profile
 --------------------------------------

	* Right-click on "profile_updater.ps1" and select "Run with PowerShell"
	* Check for success messages when complete
	* Exit

 STEP 3: OPEN PowerShell (NOT PowerShellISE)
 -----------------------------------------------

 STEP 4: SET YOUR PARAMETERS FOR THE SCRIPT AND EXECUTE
 ------------------------------------------------------
	
	* Syntax: 
		> Search-SQLLogs -InputDirectory <FULL DIRECTORY PATH> -SQLCode <SQL STRING TO SEARCH FOR> -ExportPath <Full path of export file>

	* Example:
	
		PS C:\Users\Josh.Mascorro.INTERNAL> Search-SQLLogs -InputDirectory C:\Users\Josh.Mascorro.INTERNAL\Downloads\315_metadata_refresh -SQLcode "DROP TABLE" -ExportPath C:\Users\Josh.Mascorro.INTERNAL\Documents\Oncor\Output-SQLLogs.txt
		
	
 STEP 4: OPEN OUTPUT FILE
 ------------------------
	
	* Open the output file in the location that you specified in the "-ExportPath" option.
    

 TIPS:
 -----

	1. To clear screen: "clear" or "cls"
	2. Use tab-completion. So if you want to use Search-SQLLogs, just type "Sear" then press tab.
	3. To break/cancel a command: "CTRL + C"