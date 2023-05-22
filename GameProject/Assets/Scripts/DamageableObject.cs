using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour
{
    private int health = 100;
    public bool targetable = false;
    public bool invulnerable = false;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    public Image healthBar;
    public GameObject healthUI;
    public Canvas gameInterface;
    public bool canMove = true;

    private static readonly int Defeated1 = Animator.StringToHash("Defeated");
    private static readonly int Hit1 = Animator.StringToHash("Hit");
    public float maxHealth = 100;


    public void SetupDamageableObject()
    {
        var ui = gameObject.GetComponentInChildren<Canvas>();
        if (ui is not null)
            gameInterface = ui;
        healthUI = gameInterface.transform.Find("HealthBar").gameObject;
        healthUI.SetActive(false);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int Health
    {
        set
        {
            health = value;
            if (Math.Abs(health - maxHealth) < 0.5)
                healthUI.SetActive(false);
            else
            {
                healthUI.SetActive(true);
            }
            if (health <= 0)
            {
                Defeated();
            }
        }
        get => health;
    }

    public void ReceiveHit(int damage, Vector2 knockBackForce)
    {
        if (!invulnerable)
        {
            Health -= damage;
        }
        animator.SetTrigger(Hit1);
        rb.AddForce(knockBackForce);
        UpdateHealthBar();
        canMove = true;
    }

    public void ReceiveHit(int damage)
    {
        canMove = false;
        Health -= damage;
        animator.SetTrigger(Hit1);
        canMove = true;
    }

    public void UpdateHealthBar()
    {
        if (healthBar is not null)
            healthBar.fillAmount = health / maxHealth;
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    private void Defeated()
    {
        animator.SetTrigger(Defeated1);
        canMove = false;
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}