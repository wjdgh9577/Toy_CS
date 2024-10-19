# 패킷 무결성 검증 전략
서버는 클라이언트로부터 수신한 네트워크 패킷을 무조건적으로 수용하는 것이 아니라 무결성 테스트를 통해 검증해야 한다.

이에 대해 다양한 무결성 검증 전략을 고민해보고 테스트를 진행한다.

## 아이디어
서버에서 패킷을 신뢰할 수 없는 경우에 대해 생각해보았다.

1. 패킷 변조
2. 게임 로직상 불가능한 요청
3. 오래된 패킷

</br>

**패킷 변조**는 최초 인증시 토큰을 부여하고 이를 패킷에 추가하는 방식으로 검증할 수 있을 것 같다.

이후 주기적으로 토큰을 갱신하여 보안을 높일 수 있을 것이다.

</br>

**게임 로직상 불가능한 요청**은 여러 케이스가 존재하지만 본 프로젝트에서는 '캐릭터가 맵을 뚫고 들어가는 상황'에 한해서만 고려했다.

이를 위해 서버는 사전에 맵 데이터를 추출하여 가지고 있어야 하고 캐릭터의 collider와의 연산을 통해 무결성을 검증해야 한다.

2D 타일맵으로 제작하고 collider를 구성하는 polygon의 vertex 데이터를 추출하여 연속된 지형별로 묶는다면 서버에서도 맵의 형태를 유추할 수 있을 것이다.

</br>

**오래된 패킷**에 대한 검증은 아직 고민중인 부분이고 구현체가 적용되지 않았다.

## 세부 구현
### 변조 패킷 검증
TCP 연결이 성공하면 세션과 token을 생성하고 패킷에 token을 넣어 전송한다.

![sequence diagram](https://github.com/user-attachments/assets/8cd0c1ff-6f87-4e06-9b70-b11fab772364)

Application level 패킷은 아래와 같이 설계하였다.

![packet](https://github.com/user-attachments/assets/19edbab1-9b61-45a2-8529-94cb593a671b)

이후 클라이언트와 서버는 패킷 수신시 검증 로직을 수행하고 검증되지 않은 패킷은 무시한다.

```cs
void HandleLogic(Action<SessionBase, IMessage> handler, SessionBase session, string token, IMessage message)
{
    if (session.Verify(token) == false)
    {
        LogHandler.LogError(LogCode.PACKET_INVALID_TOKEN, token);
        return;
    }
    handler.Invoke(session, message);
}
```

### 맵 데이터 추출
서버측에서 충돌 처리를 통하여 player의 상태 변화가 유효한지 검증하기 위해 우선 맵 데이터를 추출했다.

2D 타일맵의 'Composite Collider 2D' component로부터 polygon 정보를 추출할 수 있다.

```cs
static void ExtractTilemapCollider()
{
    // ...
    foreach (Map map in maps)
    {
        MapData mapData = new MapData(map.MapInfo.uniqueId);
        // 지형별 collider
        CompositeCollider2D[] compositeColliders = map.GetComponentsInChildren<CompositeCollider2D>();

        foreach (var compositeCollider in compositeColliders)
        {
            if (compositeCollider == null)
                continue;

            for (int i = 0; i < compositeCollider.pathCount; i++)
            {
                List<Vector2> path = new List<Vector2>();
                compositeCollider.GetPath(i, path); // vertex 추출

                // ...
            }
        }
        // ...
    }
    // ...
}
```

![map](https://github.com/user-attachments/assets/adf1f0c9-4dbc-4a5e-b00e-f2acd9d732b2)

맵 데이터를 Json 파일로 저장하고 서버가 실행될 때 preload하여 사용된다.

### 충돌 구현
맵의 polygon 정보와 캐릭터의 collider 정보를 통해 충돌 여부를 판단하는 로직을 구현했다.

player는 capsule collider를 가지며 아래와 같은 정보를 가진다.

![capsule collider](https://github.com/user-attachments/assets/8d68045a-4f60-47e9-9322-fe9eb70c2b16)

Capsule collider는 직사각형의 중심부와 상하단의 반원으로 구성되어 있다.

따라서 width/2는 반원의 radius로 볼 수 있다.

충돌 판정은 아래의 절차를 따른다.
1. 직사각형의 꼭지점이 각각 맵의 polygon 내부에 있는가?
2. 반원의 중심으로부터 맵의 polygon까지의 최단거리가 radius보다 작은가?
3. 위 과정을 모든 polygon에 대해 수행

#### 직사각형 충돌
Ray casting 알고리즘을 채택했다.

한 점으로부터 한 방향으로 반직선을 그었을 때 교차점의 개수가 홀수인 경우 도형의 내부라고 판단할 수 있다.

따라서 직사각형의 각 꼭지점에 대해 vector(1, 0) 방향으로의 교차점의 개수를 구하고 만약 결과값이 홀수라면 충돌로 판단했다.

#### 반원 충돌
최단거리 알고리즘을 채택했다.

한 점(반원의 중심)에서 두 점을 잇는 선분(polygon의 vertices)까지의 최단거리는 1) 직선까지의 수선의 길이거나, 2) 선분의 양 끝점 중 가까운 점까지의 거리다.

![distance](https://github.com/user-attachments/assets/fcdf0a19-3922-40ea-8b41-0d2669e48a39)

반원의 중심을 polygon에 내적한 결과가 양수이면서 선분의 길이보다 작으면 case 1, 음수이거나 선분의 길이보다 크면 case 2라고 볼 수 있다.

case 1의 경우 수선의 길이를 구하고 case 2의 경우 선분의 양 끝점까지의 거리를 각각 계산한 다음 최소값을 구한다.

> 점 $(x_1,y_1 )$과 직선 $ax+by+c=0$ 까지의 수선의 길이 d
>
> $d = \frac{|ax_1+by_1+c|}{\sqrt{a^2+b^2}}$

최단거리가 radius보다 작을 경우 충돌로 판단했다.

### 상태 롤백
클라이언트로부터 수신한 상태 변화가 비정상(충돌)이라고 판단될 경우 서버 내 상태를 갱신하지 않고 즉시 롤백 패킷을 송신한다.

이로 인해 game room 내의 다른 player들은 비정상적인 player의 상태 변화를 감지할 수 없으며, 또한 비정상적인 상태 변화가 게임의 로직을 해치지도 않는다.

비정상적인 행동이 감지된 클라이언트는 서버로부터 수신한 가장 최근의 유효한 위치로 즉시 이동된다.

![gif](https://github.com/user-attachments/assets/e1539016-4c7f-48e8-adfc-e338ae14e44a)

## 추가 개선해볼 사항
현재는 클라이언트로부터 발생하는 트래픽의 양이 압도적으로 많다.

상태 변화 감지 민감도를 조절하여 트래픽 부하를 줄일 필요가 있을 것 같다.