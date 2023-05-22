using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObj : DamageableObject
{
    private void Start()
    {
        maxHealth = 100;
        Health = 100;
        targetable = true;
        invulnerable = false;
    }
}