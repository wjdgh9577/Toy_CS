# CodeGenerator
Common 디렉토리의 'Protocol.proto'를 분석하여 클라이언트/서버의 PacketHandler의 공용 코드를 자동 생성

개별 구현부의 자동 생성은 추후 개발 고려

<br/>

# CoreLibrary

<br/>

# Server

<br/>

# TestClient
서버 통신 테스트용 콘솔 응용프로그램

자동으로 로컬호스트로 접속되며 커맨드로 조작 가능

## Specification
Command | Description | arg[0] | arg[1] | arg[2] | arg[3] | Response
|:-:|:-|:-|:-|:-|:-|:-|
Enter | 특정 방에 입장 | int:RoomId | string?:Password | - | - | bool:EnterOk
Leave | 특정 방에서 퇴장 | int:RoomId | - | - | - | bool:LeaveOk
Create | 방 생성 | int:RoomType | int:MaxAllowedToEnter | string:RoomTitle | string?:Password | bool:EnterOk
Quick | 무작위 랜덤 공개방 입장 | - | - | - | - | bool:EnterOk
Refresh | 로비 대기방 리스트 갱신 | - | - | - | - | class[]:RoomInfos
Ready | 대기방에서 준비상태 변경 | bool:Ready | - | - | - | class:AccountInfo
[No Command] | 현재 방 참가자에게 채팅 브로드캐스트 | string:Chat | - | - |  - | -