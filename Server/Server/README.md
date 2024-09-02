# 개요
2D 플랫포머 멀티플레이어 게임 서버 애플리케이션

![Diagram](https://github.com/user-attachments/assets/621d3650-9988-4746-9482-4f279afcab9e)

<br/>

## Content
게임의 컨텐츠를 구성하는 부분을 정의한다.

### RoomBase, RoomInfo
Room은 RoomBase를 상속받아 구현하며 각각의 RoomInfo를 갖는다.

필요한 경우 RoomInfo를 상속받은 고유의 Info를 구현할 수 있다.

RoomBase와 RoomInfo는 Session 출입시 정보를 갱신하고, 기본적인 조작 API를 제공한다.

### RoomManager
Room과 관련된 모든 관리를 담당한다.

Room의 핵심 로직은 RoomManager를 통해서만 접근 가능하기에 멀티 스레드 환경에서도 동시성 제어가 보장된다.

<br/>

## Log
CoreLibrary의 LogModule을 구현한다.

LogCode가 Console일 경우 콘솔 애플리케이션에 로그를 출력하며, 그 외의 경우 DB에 로그를 저장한다.(미구현)

<br/>

## Packet
애플리케이션 레벨의 프로토콜을 정의하고, 이를 다루는 API를 제공한다.

ServerPacketHandler.cs와 Protocol.cs는 각각 Code Generator와 Google Protobuf에 의해 자동 생성된다.

PacketHandler는 ServerPacketHandler.cs, PacketHandler_Recv.cs, PacketHandler_Send.cs 세 파일로 분리 관리되며, 새로운 프로토콜에 대해 PacketHandler_Recv, PacketHandler_Send는 직접 구현해야 한다.

PacketHandler_Recv의 HandleLogic에서 패킷 검증을 수행한다.

PacketHandler_Send에서만 패킷을 생성하여 전체 코드의 유지보수성을 높인다.

<br/>

## Session
ClientSession을 구현하고 이를 관리하는 SessionManager를 정의한다.

세션이 발급될 때 세션 고유 식별자(SUID)와 토큰(Token)이 지급된다.

SUID는 SessionManager가 세션을 식별하는데 사용되고 Token은 패킷 헤더에 삽입되어 검증용으로 사용된다.

클라이언트와의 연결이 확인되면 일정 주기로 Ping을 주고받으며, 일정 시간 응답이 없으면 세션은 만료된다.