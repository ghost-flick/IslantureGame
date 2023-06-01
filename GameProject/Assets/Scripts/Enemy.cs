using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageableObject
{
    protected Transform player;
    public int damage = 1;
    public float knockBackForce = 1f;
    public float moveSpeed = 50f;
}