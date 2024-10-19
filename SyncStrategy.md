# 네트워크 개체간 동기화 전략
실시간 서버에서는 네트워크 개체간의 상태 변화를 추적하여 동기화하는 것이 중요하다.

이에 대해 다양한 동기화 전략을 고민해보고 테스트를 진행한다.

## 아이디어
아래 두가지 동기화 전략에 대해 고민해보았다.

1. 개체의 입력값을 기준으로 동기화
2. 개체의 상태를 기준으로 동기화

</br>

**개체의 입력값을 기준으로 동기화**할 경우
개체의 입력(ex. 방향키)이 주어졌을 경우에만 패킷 송신이 이루어지므로 트래픽 부하를 줄일 수 있고 패킷의 사이즈 자체도 줄일 수 있다.

단 게임의 기획에 따라 물리엔진이 필요하거나 복잡한 물리 연산을 수행해야 하는 경우 서버의 연산 부하가 증가한다.

예를 들어 클라이언트에서 방향키 조작으로 캐릭터를 이동시킨 경우, 서버에선 수신한 입력값과 맵 데이터를 기반으로 캐릭터의 위치를 계산하기 위해 클라이언트와 동일한 연산을 수행해야 한다.

</br>

**개체의 상태를 기준으로 동기화**할 경우
개체의 상태(ex. 위치) 변화를 감지할 때마다 패킷 송신이 이루어지므로 상대적으로 트래픽 부하가 늘어나고, 동기화할 상태가 많을수록 패킷의 사이즈 또한 증가한다.

그러나 트래픽 부하가 증가하는 만큼 동기화 정확성이 증가하고 서버측의 연산 부하가 감소하는 효과가 있다.

|기준|입력|상태|
|-|:-:|:-:|
|트래픽|↓|↑|
|클라이언트 연산 부하|↓|↑|
|서버 연산 부하|↑|↓|
|신뢰도|↓|↑|

</br>

본 프로젝트에서는 상태를 기준으로 동기화를 진행하여 복잡한 물리 연산은 클라이언트측에서 수행하고 서버측에서 이를 검증하는 방식을 채택했다.

## 세부 구현
### 동기화 상태 분류
클라이언트와 서버간에 동기화할 상태를 분류
- Transform
- Collider

동기화 전략에 대해 고민해보는 간단한 실험적인 프로젝트이기 때문에 실제 게임환경에 사용될 상태들(ex. 애니메이션)은 제외하였다.

다만 확장에 대해서는 열린 구조로 설계하도록 노력했다.

클라이언트측은 Unity의 'Transform', 'Collider' component를 사용하고 서버측은 별도로 자료구조를 정의했다.

Collider는 무결성 검증에 사용된다.

### 패턴 적용
클라이언트로부터 수신한 패킷을 단순히 broadcast하는 에코서버는 네트워크 부하 측면에서 비효율적이다.

따라서 서버는 일정한 규칙에 따라 수신한 패킷을 처리하고 변화분을 한 번에 응답해야 한다.

이는 아래와 같은 절차를 따른다.
1. 클라이언트로부터 상태 변화를 수신하면
2. 서버 내에 상태를 수정하고
3. 수정된 상태를 한 번에 broadcast한다.

상태 변화를 감지하기 위해 dirty flag 패턴을 적용하였다.

```cs
public CustomVector2 Position
{
    get => _position;
    private set
    {
        if (_position != value)
            IsDirty = true;
        _position = value;
    }
}
```

서버는 매 연산 주기에 game room 내부의 네트워크 개체에 대한 상태 변화를 추적하고 변화가 있을 경우에만 동기화 패킷을 broadcast한다.

```cs
public void RefreshGameRoom()
{
    if (Info.IsDirty)
    {
        Broadcast(PacketHandler.S_SyncPlayer(Info.GetProto().Players));
    }
}
```

### 클라이언트 상태 재조정
사용자의 UX를 해치지 않기 위해 local player와 remote player에 대해 상태 변화를 적용하는 로직이 약간 다르다.

전체 주기를 시간순으로 나열하면 아래와 같다.
1. Local player 상태 수정
2. 수정된 상태를 서버로 송신
3. 서버로부터 수신한 상태 적용

Local player와 remote player의 상태가 적용되는 시점이 다르기 때문에(RTT + 서버 연산 주기) 서버의 상태에 맞춰 재조정하는 로직이 추가되어야 한다.

대부분의 경우 약간의 네트워크 딜레이가 게임에 큰 영향을 미치지 않기 때문에 local player는 선반영된 상태를 유지하지만
서버에서 이상이 감지된 경우 서버와 상태를 동기화하는 로직을 구현했다.

```cs
foreach (var info in infos)
{
    if (info.accountInfo.Uuid == AccountInfo.Uuid) // local player
    {
        if (!info.IsValid) // 이상이 감지된 경우
            MyGameRoomPlayerInfo.Player.SetPosition(info.Position);
        continue;
    }

    var player = MyGameRoomInfo.players.Find(p => p.accountInfo.Uuid == info.accountInfo.Uuid).Player;
    player.SetPosition(info.Position);
}
```

### 자연스러운 이동
Remote player는 별도의 연산 없이 서버로부터 수신한 상태를 적용하기 때문에 네트워크가 지연됨에 따라 부자연스럽게 보이는 문제가 발생했다.

이를 해결하기 위해 상호작용이 일어나는 캐릭터의 본체와 모델을 분리하였다.

본체는 상태 변화를 즉시 반영하여 상호작용에 이상이 없도록 하고 모델은 서서히 반영하여 UX를 해치지 않도록 했다.

![gif](https://github.com/user-attachments/assets/8e440d01-79d6-4401-8a56-4b4fc480d1af)

![image](https://github.com/user-attachments/assets/7de19b10-0657-4975-887f-fbd48345e0d2)

## 추가 개선해볼 사항
현재까지 구현한 동기화 로직은 네트워크 딜레이에 취약하다.

TCP의 특성상 패킷의 전송은 보장되지만 시점에 대해선 신뢰도가 부족한다.

따라서 클라이언트와 서버의 시간대를 동기화하고 server time을 기준으로 동기화 전략을 전체적으로 수정해야 한다.