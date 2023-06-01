using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47 : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 weaponDirection;
    private Camera mainCamera;
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private static readonly int Fire1 = Animator.StringToHash("fire");
    [SerializeField] private GameObject bullet;
    public int damage = 5;
    public float knockBackForce = 20;
    [SerializeField] private GameObject impactEffect;
    [SerializeField]private LineRenderer lineRenderer;

    private void Start()
    {
        Physics2D.queriesHitTriggers = false;
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        player = transform.parent.gameObject.GetComponent<PlayerController>();
        player.akWeapon = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.forward = new Vector3(-1, 0);
        player.weaponMode = true;
    }

    private void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        var mouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // transform.LookAt(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        direction = mouse - transform.position;
        if (direction.x > 0)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.left, direction);
        player.UpdateAnimatorDirection(direction.normalized);
    }

    public void Fire()
    {
        animator.SetTrigger(Fire1);
    }

    public void FireBullet()
    {
        StartCoroutine(FireBulletCoroutine());
    }

    public IEnumerator FireBulletCoroutine()
    {
        weaponDirection = new Vector2(direction.x, direction.y - 0.02f);
        
        var startPoint = (Vector2)transform.parent.position + weaponDirection.normalized / 5;
        startPoint.y -= 0.02f;
        var hitInfo = Physics2D.Raycast(startPoint, weaponDirection);
        if (hitInfo)
        {
            var enemy = hitInfo.transform.GetComponent<Enemy>();
            // Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            if (enemy is not null)
            {
                var knockDirection = ((Vector2)enemy.transform.position - hitInfo.point).normalized;
                enemy.ReceiveHit(damage, knockDirection * knockBackForce);
            }
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint + weaponDirection * 10);
        }

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }
}
