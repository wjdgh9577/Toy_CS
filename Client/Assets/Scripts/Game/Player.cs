using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rigidbody;
    [SerializeField]
    CapsuleCollider2D _capsuleCollider;
    [SerializeField]
    SpriteRenderer _spriteRenderer;

    public GameRoomPlayerInfo PlayerInfo { get; set; }

    public bool IsLocalPlayer
        => PlayerInfo.accountInfo.Uuid == Managers.Instance.GameManager.AccountInfo.Uuid;

    private void Reset()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (!IsLocalPlayer)
        {
            Lerp(PlayerInfo.Position);
        }

        if (IsLocalPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetPosition(wPos);
            }
        }
    }

    private void LateUpdate()
    {
        PlayerInfo.Position = transform.position;

        if (IsLocalPlayer)
        {
            if (PlayerInfo.IsDirty)
            {
                Managers.Instance.NetworkManager.Send(PacketHandler.C_SyncPlayer(PlayerInfo));
            }
        }
    }

    public void SetPosition(Vector2 position)
    {
        var mPos = _spriteRenderer.transform.position;
        transform.position = new Vector3(position.x, position.y, 0);

        if (!IsLocalPlayer)
        {
            _spriteRenderer.transform.position = mPos;
            elapsedTime = 0f;
        }
    }

    float duration = 0.1f;
    float elapsedTime = 0f;

    void Lerp(Vector2 position)
    {
        elapsedTime += Time.deltaTime;

        var mPos = _spriteRenderer.transform.position;
        var t = Mathf.Clamp01(elapsedTime / duration);

        _spriteRenderer.transform.position = Vector2.Lerp(mPos, position, t);
    }
}