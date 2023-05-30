using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class DamageableObject : MonoBehaviour
{
    public UnityEvent eventOnDeath;
    private int health = 100;
    public bool targetable = false;
    public bool invulnerable = false;
    protected Animator animator;
    protected Animator effectAnimator;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    private Image healthBarFill;
    private GameObject healthUI;
    [SerializeField] private Canvas gameInterface;
    [SerializeField] private string deathAnimationEffect;
    public bool canMove = true;

    private static readonly int Defeated1 = Animator.StringToHash("Defeated");
    private static readonly int Hit1 = Animator.StringToHash("Hit");
    public float maxHealth = 100;
    private bool processingColor;
    public bool defeated;


    public void SetupDamageableObject()
    {
        if (gameInterface == null)
            throw new Exception("Should assign in inspector");
        healthUI = gameInterface.transform.Find("HealthBar").gameObject;
        healthUI.SetActive(false);
        healthBarFill = healthUI.transform.Find("HealthBarFill").GetComponent<Image>();
        animator = GetComponent<Animator>();
        effectAnimator = transform.Find("Effects").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected int Health
    {
        set
        {
            health = value;
            healthUI.SetActive(!(Math.Abs(health - maxHealth) < 0.5));
            if (health <= 0)
            {
                Defeated();
            }
        }
        get => health;
    }

    public void SetHealth(int healthMax)
    {
        health = healthMax;
        maxHealth = healthMax;
    }

    private IEnumerator ChangeSpriteRendererColor()
    {
        processingColor = true;
        var color = new Color(1, 0.2f, 0.2f, 1);
        var baseColor = spriteRenderer.color;
        var difColor = (color - baseColor) / 4;
        for (var i = 0; i < 3; i++)
        {
            spriteRenderer.color = color;
            color -= difColor;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.color = baseColor;

        processingColor = false;
    }

    public void ReceiveHit(int damage, Vector2 knockBackForce)
    {
        if (!invulnerable)
        {
            if (!processingColor)
                StartCoroutine(ChangeSpriteRendererColor());
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
        if (healthBarFill is not null)
            healthBarFill.fillAmount = health / maxHealth;
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
        defeated = true;
        animator.Play("defeated");
        effectAnimator.Play(deathAnimationEffect);
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
        eventOnDeath.Invoke();
    }
}