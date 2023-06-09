using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SwordAttack : MonoBehaviour
{
    public int damage = 20;
    public float knockbackForce = 50f;
    public LayerMask enemyLayers;
    private Vector2 rightAttackOffset = new Vector2(0.1f, -0.07f);
    private Vector2 leftAttackOffset = new Vector2(-0.1f, -0.07f);
    private Vector2 downAttackOffset = new Vector2(0, -0.1f);
    private Vector2 topAttackOffset = new Vector2(0, 0.05f);
    public void Attack(float dirX, float dirY)
    {
        transform.localPosition = dirY switch
        {
            >= 0.8f => topAttackOffset,
            <= -0.8f => downAttackOffset,
            _ => dirX >= 0 ? rightAttackOffset : leftAttackOffset
        };
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