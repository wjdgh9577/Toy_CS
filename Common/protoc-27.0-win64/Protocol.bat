@echo off
protoc -I=./ --csharp_out=./ ./Protocol.proto
IF ERRORLEVEL 1 PAUSE

START ../../Server/CodeGenerator/bin/Debug/net8.0/CodeGenerator.exe ./Protocol.proto

set CLIENT_PACKET_PATH="../../Client/Assets/Scripts/Packet"
set SERVER_PACKET_PATH="../../Server/Server/Packet"
set DUMMY_PACKET_PATH="../../Server/TestClient/Packet"

:proto
if exist "Protocol.cs" (
	:: XCOPY /Y Protocol.cs %CLIENT_PACKET_PATH% >nul
	XCOPY /Y Protocol.cs %SERVER_PACKET_PATH% >nul
	XCOPY /Y Protocol.cs %DUMMY_PACKET_PATH% >nul
	DEL Protocol.cs
) else (
	echo 颇老 积己吝...
	timeout /t 1 /nobreak >nul
	goto :proto
)

:client_packet
if exist "ClientPacketManager.cs" (
:: 	XCOPY /Y ClientPacketManager.cs %CLIENT_PACKET_PATH% >nul
	XCOPY /Y ClientPacketManager.cs %DUMMY_PACKET_PATH% >nul
	DEL ClientPacketManager.cs
) else (
echo 颇老 积己吝...
 	timeout /t 1 /nobreak >nul
 	goto :client_packet
)
:server_packet
if exist "ServerPacketManager.cs" (
	XCOPY /Y ServerPacketManager.cs %SERVER_PACKET_PATH% >nul
	DEL ServerPacketManager.cs
) else (
	echo 颇老 积己吝...
	timeout /t 1 /nobreak >nul
	goto :server_packet
)