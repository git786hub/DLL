:: diff syntax here calls script using relative path before other variables have been set
call %~dp0\Env_Config\Environment_Config.bat

pushd .

c:
cd \Program Files (x86)\Intergraph\GTechnology\Program

call %ConfigFolder%\Publishing\Support\MoveOldPublishLog.bat
call %ConfigFolder%\Publishing\Support\CreateDataFolder.bat

copy %ConfigFolder%\Publishing\UDB_Templates\GIS_electric_template.udb %ConfigFolder%\Publishing\%Config%.UDB

call %ConfigFolder%\Publishing\Support\BatchSubstitute.bat FoldDate %ShortDateTime% %ConfigFolder%\Publishing\%Config%.UDB > %ConfigFolder%\Publishing\%Config%_temp.UDB
call %ConfigFolder%\Publishing\Support\BatchSubstitute.bat ONCORCONFIG %Config% %ConfigFolder%\Publishing\%Config%_temp.UDB > %ConfigFolder%\Publishing\%Config%_temp1.UDB

copy %ConfigFolder%\Publishing\%Config%_temp1.UDB %ConfigFolder%\Publishing\%Config%.UDB
del %ConfigFolder%\Publishing\%Config%_temp.UDB
del %ConfigFolder%\Publishing\%Config%_temp1.UDB

PublishMapFiles %ConfigFolder%\Publishing\%Config%.UDB %PubType% %PubConnString%

popd

pause


