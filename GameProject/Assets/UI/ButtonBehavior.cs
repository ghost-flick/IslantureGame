using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public Sprite newButtonImage;
    private Transform textPosition;
    private Button button;

    public void Start()
    {
        button = gameObject.GetComponent<Button>();
        textPosition = gameObject.GetComponentInChildren<Transform>();
    }

    public void ChangeButtonOnClick()
    {
        button.image.sprite = newButtonImage;
    }
}