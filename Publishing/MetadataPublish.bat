pushd .

:: diff syntax here calls script using relative path before other variables have been set
call %~dp0\Env_Config\Environment_Config.bat

call %ConfigFolder%\Publishing\Support\MoveOldMetaPubLog.bat
call %ConfigFolder%\Publishing\Support\CreateMetaDataFolder.bat
pause
c:
cd \program files (x86)\intergraph\gtechnology\program

publishmetadatamapfiles %Config% %ConfigFolder%\Metadata\%ShortDateTime% %PubConnString%

@if %Logging%==1 pause

@rem if we are replacing (PubType = 1), then copy curr prod ccm over to prep in order to start with most updated version
if %PubType%==1 (
	copy %ConfigFolder%\CCM\ConnectionConfigurationMap.ccm %ConfigFolder%\CCM_Prep\ConnectionConfigurationMap.ccm
)

::remove lines that have "metadata" in them...we will replace these from the template
findstr /v "Metadata metadata" %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap.ccm > %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp.ccm

::append the metadata template lines to the bottom of the CCM
type %ConfigFolder%\Publishing\Support\CCM_Metadata_Template.CCM >> %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp.ccm

::perform string substitution to insert the config and the datetime
call %ConfigFolder%\Publishing\Support\BatchSubstitute.bat FoldDate %ShortDateTime% %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp.ccm > %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp1.ccm
call %ConfigFolder%\Publishing\Support\BatchSubstitute.bat ONCORCONFIG %Config% %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp1.ccm > %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp2.ccm

copy %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp2.ccm %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap.ccm

del %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp.ccm
del %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp1.ccm
del %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap_Temp2.ccm

if %PubType%==1 (
	copy %ConfigFolder%\CCM_PREP\ConnectionConfigurationMap.ccm %ConfigFolder%\CCM\ConnectionConfigurationMap.ccm
)

popd
pause
