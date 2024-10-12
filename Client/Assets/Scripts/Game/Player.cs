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

    private void Reset()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}