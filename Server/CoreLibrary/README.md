# 개요
클라이언트/서버 공용 라이브러리

## Job
멀티 스레드 환경에서 동시성 제어를 위한 일감(job)을 정의하고, 이를 다루는 API를 제공한다.

### JobSerializer
일감을 JobQueue에 담아 직렬화하여 순차적으로 처리한다.

동시성 제어가 필요한 클래스가 상속받아 사용된다.

일감의 처리는 매 프레임마다 순차적으로 진행된다.

### JobTimer
System.Timers.Timer 클래스를 활용한 커스텀 타이머

싱글톤으로 생성되어 프로세스 전역에서 사용되며 JobTimerHandler를 통해 접근할 수 있다.

내부적으로 풀링기법을 사용하여 메모리를 효율적으로 활용한다.

<br/>

## Log
런타임 로그 생성과 관련된 인터페이스를 제공한다.

로그 모듈을 추상화하여 제공하며 이는 클라이언트/서버 각각 별도로 기획에 따라 구현해야 한다.

LogHandler를 통해 API를 제공하며, 필요한 경우 모듈을 변경하여 사용할 수 있다.

### LogCode
로그의 종류를 나타내며 필요한 경우 추가하여 사용한다.

### LogType
로그의 중요도를 나타내며 None, Warning, Error 세 단계로 구성된다.

### LogKey
로그의 key를 지정하여 사용할 경우 추가하여 사용한다.

<br/>

## Network
TCP/IP 네트워크 통신에 필요한 API를 제공한다.

### NetworkHandler
Connector, Listener 소켓 초기화와 관련된 API를 제공한다.

### Session
세션을 추상화하여 제공하며 버퍼 사이즈를 지정할 수 있다.

기본적인 패킷 전송 API가 제공된다.

세션 고유 식별자(SUID)와 토큰(Token)을 필드로 갖는다.

토큰은 최초 연결시 서버에서 지급하며 이후 패킷의 헤더에 삽입되어 패킷 검증용으로 사용된다.

<패킷 구성>
![Packet](https://github.com/user-attachments/assets/b8eef1ea-ae64-4191-98b4-b2daa27fc860)

<br/>

## Utility
편의성 코드를 별도로 분류하여 관리한다.