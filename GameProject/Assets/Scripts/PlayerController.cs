using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : PlayerObj
{
    public UnityEvent actions;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    protected SwordAttack swordAttack;
    protected Interaction interaction;
    public float lastXInput;
    public float lastYInput;
    public float lastXWeapon;
    public float lastYWeapon;
    private Vector2 movementInput;
    private readonly List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private GameObject gameMenu;
    [SerializeField] private GameObject fireWeapon;
    public Ak47 akWeapon;
    private int regenTimeThreshold = 5;
    private int regenCount = 5;
    public bool weaponMode;

    public static PlayerController Instance;
    private static readonly int Attack = Animator.StringToHash("swordAttack");
    private static readonly int XInput = Animator.StringToHash("XInput");
    private static readonly int YInput = Animator.StringToHash("YInput");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        swordAttack = GetComponentInChildren<SwordAttack>();
        interaction = GetComponentInChildren<Interaction>();
        SetupDamageableObject();
        SetHealth(100);
        gameMenu = gameInterface.transform.Find("GameMenu").gameObject;
        targetable = true;
        invulnerable = false;
        StartCoroutine(SecondsCaller());
    }

    private IEnumerator SecondsCaller()
    {
        while (!defeated)
        {
            yield return new WaitForSeconds(1);
            if (Time.time - lastDamagedTime > regenTimeThreshold)
            {
                Regenerate();
            }
        }
    }

    private void Regenerate()
    {
        Health += Math.Min(regenCount, maxHealth - Health);
        if (Health < maxHealth)
            effectAnimator.Play("regeneration");
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        // <param name="contactFilter">.</param>
        // <param name="results">Array to receive results.</param>
        // <param name="distance">Maximum distance over which to cast the Collider(s).</param>
        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero)
        {
            lastXInput = movementInput.x;
            lastYInput = movementInput.y;
            var success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(lastXInput, 0));
            }

            if (!success)
            {
                success = TryMove((new Vector2(0, lastYInput)));
            }

            animator.SetBool(IsMoving, success);
            if (!weaponMode)
                UpdateAnimatorDirection();
        }
        else
        {
            animator.SetBool(IsMoving, false);
        }
    }

    public void UpdateAnimatorDirection()
    {
        animator.SetFloat(XInput, lastXInput);
        animator.SetFloat(YInput, lastYInput);
    }

    public void UpdateAnimatorDirection(Vector2 dir)
    {
        animator.SetFloat(XInput, dir.x);
        animator.SetFloat(YInput, dir.y);
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return false;
        var count = rb.Cast(
            direction, // Vector representing the direction to cast each Collider2D shape
            movementFilter, // Filter results defined by the contact filter
            castCollisions, // List of collisions to store the found collisions into 
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // the amount to cast
        rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));

        return count == 0;
    }

    void OnMove(InputValue movementValue)
    {
        if (!GameStateController.NormalMode)
            return;
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        if (!GameStateController.NormalMode)
            return;
        if (!weaponMode)
            animator.SetTrigger(Attack);
        else
        {
            akWeapon.Fire();
        }
    }

    void OnInteract()
    {
        if (GameStateController.NormalMode)
            interaction.Interact(lastXInput, lastYInput);
        else
            actions.Invoke();
    }

    void OnMenu()
    {
        TogglePause();
    }

    private void OpenGameMenu(bool flag)
    {
        gameMenu.gameObject.SetActive(flag);
    }

    private void TogglePause()
    {
        if (Math.Abs(Time.timeScale - 1) < 0.1)
        {
            OpenGameMenu(true);
            PauseGame();
        }
        else
        {
            OpenGameMenu(false);
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void EnterDialog()
    {
        animator.SetBool(IsMoving, false);
    }

    void SwordAttack()
    {
        LockMovement();
        swordAttack.Attack(lastXInput, lastYInput);
    }

    void EndSwordAttack()
    {
        UnlockMovement();
    }

    public void TakeLegendaryWeapon()
    {
        animator.Play("weapon_idle");
        var weapon = Instantiate(fireWeapon, transform, false);
        weapon.transform.localPosition = new Vector3(0, -0.04f);
    }
}