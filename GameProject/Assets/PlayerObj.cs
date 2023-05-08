using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObj : DamageableObject
{
    private void Start()
    {
        health = 10f;
        targetable = true;
        invulnerability = true;
    }
}