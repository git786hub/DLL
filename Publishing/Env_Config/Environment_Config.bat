@REM CHANGE THESE TWO LINES IF COPIED TO NEW ENVIRONMENT
set PubConnString=username/password@DB_SID
set Config=ONCORTST1


@FOR /f %%a IN ('WMIC OS GET LocalDateTime ^| FIND "."') DO SET DTS=%%a
SET ShortDateTime=%DTS:~0,4%%DTS:~4,2%%DTS:~6,2%
SET LongDateTime=%DTS:~0,4%-%DTS:~4,2%-%DTS:~6,2%_%DTS:~8,2%-%DTS:~10,2%-%DTS:~12,2%

set ConfigFolder=\\enttstnas01\dis_ddcshare_dev\Mapfiles\%Config%
@REM Publish type - 0 WILL NOT copy prep ccm over ccm.  1 will copy prep ccm over ccm.
set PubType=0
@REM Logging - 0 WILL NOT pause/log, 1 will pause/log
set Logging=1

:: display the registry key value for FileUNCPath
call %ConfigFolder%\Publishing\Support\DisplayFUNC.bat
:: display the path inside the FileUNCPath to the CCM file
call %ConfigFolder%\Publishing\Support\DisplayCCMPath.bat
