# 개요
MMORPG 게임 서버에 대한 공부를 바탕으로 TCP/IP, 멀티 스레드 기반의 소켓 서버를 구현해보는 토이 프로젝트

<br/>

# 목표
1. 네트워크 개체간의 동기화 전략 R&D
1. 패킷 무결성 검증 전략 R&D (치팅 감지)
1. 그 외 구현해보고 싶은 기능 추가 구현 (로그인, 로비, 대기방, 채팅 등)

<br/>

# 핵심 구현 요소
1. [네트워크 개체간 동기화 전략](https://github.com/wjdgh9577/Toy_CS/blob/main/SyncStrategy.md)
1. [패킷 무결성 검증 전략](https://github.com/wjdgh9577/Toy_CS/blob/main/VerificationStrategy.md)

<br/>

# 기간
2024.08 ~ 진행중

</br>

# 개발 환경
Client : Unity 2022.3.4f1 -> Unity 6000.0.23f1

Server : .NET 8.0 (C# 12)

CoreLibrary : .NET Standard 2.1 (C# 8.0)

Protocol : Google Protocol Buffers (proto3)

IDE : Visual Studio 2022