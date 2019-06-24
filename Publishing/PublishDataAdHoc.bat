:: diff syntax here calls script using relative path before other variables have been set
call %~dp0\Env_Config\Environment_Config.bat

pushd .

call %ConfigFolder%\Publishing\Support\MoveOldPublishLog.bat
call %ConfigFolder%\Publishing\Support\CreateDataFolder.bat

c:
cd \Program Files (x86)\Intergraph\GTechnology\Program

PublishMapFiles %ConfigFolder%\Publishing\%Config%.UDB %PubType% %PubConnString%

popd

pause