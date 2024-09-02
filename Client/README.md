# 개요
2D 플랫포머 멀티플레이어 게임 클라이언트 애플리케이션

![Diagram](https://github.com/user-attachments/assets/cf99c502-45c3-4c2c-b098-7f3648bf6708)

<br/>

## Authentication
계정 생성과 로그인에 관한 부분을 담당한다.

Authentication Server(Web API)과 HTTPS 연결을 시도하고, 결과를 콜백 메서드로 반환한다.(미구현)

<br/>

## Data
컨텐츠 전반에 사용되는 데이터 타입을 정의하고 확장 기능을 제공한다.

<br/>

## Editor
에디터용 개발 도구를 제공한다. (현재는 테스트 클라이언트만 제공)

<br/>

## Log
CoreLibrary의 LogModule을 구현한다.

LogCode가 Console일 경우 에디터에 로그를 출력하며, 그 외의 경우 별도의 파일에 저장한다.(미구현)

<br/>

## Network
네트워크와 관련된 전반적인 기능들을 지원한다.

### Packet
애플리케이션 레벨의 프로토콜을 정의하고, 이를 다루는 API를 제공한다.

ClientPacketHandler.cs와 Protocol.cs는 각각 Code Generator와 Google Protobuf에 의해 자동 생성된다.

PacketHandler는 ClientPacketHandler.cs, PacketHandler_Recv.cs, PacketHandler_Send.cs 세 파일로 분리 관리되며, 새로운 프로토콜에 대해 PacketHandler_Recv, PacketHandler_Send는 직접 구현해야 한다.

PacketHandler_Recv의 HandleLogic에서 패킷 검증을 수행한다.
단 첫 Connect 패킷은 서버로부터 Token을 전달받기 때문에 검증을 수행하지 않는다.

PacketHandler_Send에서만 패킷을 생성하여 전체 코드의 유지보수성을 높인다.

### Session
ServerSession을 구현한다.

서버와 연결될 경우 첫 Connect 패킷으로 Token을 전달받으며, 이는 이후 패킷 검증에 사용된다.

서버와 일정 주기로 Ping을 주고받으며, 일정 시간 응답이 없으면 세션은 만료된다.

<br/>

## Scene
씬이 로드될 때 씬을 초기화하는 스크립트를 정의한다.

씬 로드는 비동기로 진행되며 결과를 콜백 메서드로 반환한다.

<br/>

## UI
각 씬별 UI Canvas와 UI Item을 정의한다.

UIManager에서 UI 요소와 관련된 API를 제공하며, 내부적으로 풀링기법을 활용하여 메모리를 효율적으로 사용한다.

UI는 씬에 귀속되지 않고 Managers 산하에서 관리된다.