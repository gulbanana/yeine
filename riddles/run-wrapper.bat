@echo off & setlocal enabledelayedexpansion

for /f "usebackqdelims=" %%i in (wrapper-commands-windows.json) do set commands=!commands!%%i
echo on
java -jar match-wrapper-1.4.0.jar "%commands%"