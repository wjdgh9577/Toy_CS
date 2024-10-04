@ECHO off

SET CURRENT_DIR=%cd%

CALL Build.bat
IF ERRORLEVEL 1 PAUSE

CD /d %CURRENT_DIR%

SET SERVER_PROCESS_PATH=../Server/Server/bin/Debug/net8.0\Server.exe
SET DUMMY_PROCESS_PATH=../Server/TestClient/bin/Debug/net8.0\TestClient.exe

CD /d ../Server/Server/bin/Debug/net8.0
START Server.exe
::START %SERVER_PROCESS_PATH%
::START %DUMMY_PROCESS_PATH%
::START %DUMMY_PROCESS_PATH%