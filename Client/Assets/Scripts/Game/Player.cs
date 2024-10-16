using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rigidbody;
    [SerializeField]
    CapsuleCollider2D _capsuleCollider;
    [SerializeField]
    SpriteRenderer _spriteRenderer;

    public GameRoomPlayerInfo PlayerInfo { get; set; }

    private void Reset()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PlayerInfo.Position = transform.position;

        if (PlayerInfo.IsDirty)
        {
            Managers.Instance.NetworkManager.Send(PacketHandler.C_SyncPlayer(PlayerInfo));
        }
    }

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }
}