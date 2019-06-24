@echo off
setlocal ENABLEEXTENSIONS

set KEY_NAME="HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Intergraph\GFramme\02.00\Path"
set VALUE_NAME=FileUNCPath

FOR /F "usebackq skip=2 tokens=1-4" %%A IN (`REG QUERY %KEY_NAME% /v %VALUE_NAME% 2^>nul`) DO (
    set ValueName=%%A
    set ValueType=%%B
    set ValueValue=%%C %%D
)

if defined ValueName (
	@echo FileUNCPath registry key currently set to: %ValueValue%

) else (
    @echo "%KEY_NAME:"=%\%VALUE_NAME%" not found.
)
echo on