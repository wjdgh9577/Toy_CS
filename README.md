# 개요
TCP/IP 기반의 실시간 서버 구현 토이 프로젝트

<br/>

# 목표
최대 8인이 참여하는 2D 플랫포머 멀티플레이어 게임 개발
1. TCP/IP 소켓 프로그래밍
1. 멀티 스레드 기반의 서버 로직
1. 간단한 서버 물리엔진
1. DB 연동
1. 로그인 인증 Web API 서버 + Redis 캐시 서버

![Diagram](https://github.com/user-attachments/assets/0a11a432-e602-4ca1-a962-cd52512e5187)

<br/>

# 진행 상황
1. ~~TCP/IP 채팅 서버 구현 및 테스트~~
1. ~~테스트 클라이언트 개발~~
1. ~~클라이언트/서버 공용 라이브러리 개발~~
1. ~~Google Protobuf 이식~~
1. ~~클라이언트/서버 공용 코드 생성 자동화~~
1. ~~아웃게임 로비 구현(방 목록 갱신, 빠른 입장, 방 생성)~~
1. ~~아웃게임 대기방 구현~~
1. 간단한 물리엔진 구현(중력, 충돌)
1. 인게임 구현
1. DB 연동
1. 클라이언트 회원가입/로그인 구현(Web API + Redis)

<br/>

# 개발 환경
Client : Unity 2022.3.4f1

Server : .NET 8.0 (C# 12)

CoreLibrary : .NET Standard 2.1 (C# 8.0)

Protocol : Google Protocol Buffers (proto3)

IDE : Visual Studio 2022