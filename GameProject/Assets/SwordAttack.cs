using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 0.2f;
    public float knockbackForce = 50f;
    public Vector2 rightForce = new Vector2(1, -0.9f);
    public Vector2 leftForce = new Vector2(-1, -0.9f);
    public LayerMask enemyLayers;
    public bool isAttackLeft;
    private Vector2 rightAttackOffset;
    private Vector2 leftAttackOffset;

    private void Start()
    {
        leftAttackOffset = new Vector2(0.1f, -0.05f);
        rightAttackOffset = new Vector2(-0.1f, -0.05f);
    }

    public void Attack()
    {
        if (isAttackLeft)
            transform.localPosition = leftAttackOffset;
        else
            transform.localPosition = rightAttackOffset;
        var hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.1f, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            TryToDealDamage(enemy);
        }
    }

    private void TryToDealDamage(Collider2D other)
    {
        //calculate direction between character and slime
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;
        var parentPosition = transform.parent.position;
        var direction = (Vector2)(enemy.transform.position - parentPosition).normalized;
        enemy.ReceiveHit(damage, direction * knockbackForce);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}