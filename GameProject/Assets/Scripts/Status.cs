using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private Sprite speaking;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(string spriteName)
    {
        if (spriteName == "speaking")
            spriteRenderer.sprite = speaking;
        else
        {
            spriteRenderer.sprite = null;
        }
    }
}
