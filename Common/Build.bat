@ECHO off

"protoc-27.0-win64/protoc" -I=./ --csharp_out=./ ./Protocol.proto
IF ERRORLEVEL 1 PAUSE

dotnet build ../Server/Server.sln --configuration Debug
IF ERRORLEVEL 1 PAUSE

START /wait ../Server/CodeGenerator/bin/Debug/net8.0/CodeGenerator.exe ./Protocol.proto
IF ERRORLEVEL 1 PAUSE

SET CLIENT_PLUGIN_PATH="../Client/Assets/Plugins"
SET CLIENT_PACKET_PATH="../Client/Assets/Scripts/Network/Packet"

SET SERVER_PLUGIN_PATH="../Server/TestClient/bin/Debug/net8.0\CoreLibrary.dll"
SET SERVER_PACKET_PATH="../Server/Server/Packet"
SET SERVER_DATA_PATH="../Server/Server/Data/JsonData"

SET DUMMY_PACKET_PATH="../Server/TestClient/Packet"

XCOPY /Y %SERVER_PLUGIN_PATH% %CLIENT_PLUGIN_PATH%

XCOPY /Y Protocol.cs %CLIENT_PACKET_PATH% >nul
XCOPY /Y Protocol.cs %SERVER_PACKET_PATH% >nul
XCOPY /Y Protocol.cs %DUMMY_PACKET_PATH% >nul
DEL Protocol.cs

XCOPY /Y ClientPacketHandler.cs %CLIENT_PACKET_PATH% >nul
XCOPY /Y ClientPacketHandler.cs %DUMMY_PACKET_PATH% >nul
DEL ClientPacketHandler.cs

XCOPY /Y ServerPacketHandler.cs %SERVER_PACKET_PATH% >nul
DEL ServerPacketHandler.cs

XCOPY /I /E /Y "./Data" %SERVER_DATA_PATH% >nul