for /f "tokens=* delims=" %%a  in  ('%~dp0\xpath.bat  "C:\Mapfiles\%Config%\Paths\FileUNCPaths.xml" "/FileUNCPaths/PrepCCMPaths/Location"') do (
   set location=%%a
)
@echo %location%