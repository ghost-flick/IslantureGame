using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float speed = 1f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int knockBackForce = 10;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField]private LayerMask enemyLayer;
    private static readonly int Gotcha = Animator.StringToHash("Gotcha");
    
    public float lifetime = 5.0f;
    // Start is called before the first frame update
       
    public void Awake ()
    {
        Destroy(this.gameObject, lifetime);
    }

    public void Setup(Vector2 direction)
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed; 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
            return;
        var player = collision.gameObject.GetComponent<PlayerController>();
        rb.velocity = Vector2.zero;
        animator.SetTrigger(Gotcha);
        if (player is not null)
        {
            var fireBallPos = transform.position;
            var dir = (Vector2)(player.transform.position - fireBallPos).normalized;
            player.ReceiveHit(damage, dir * knockBackForce);
        }
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
