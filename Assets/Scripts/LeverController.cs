using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite offSprite;
    public Sprite onSprite;
    public bool leverState = false;
    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        spriteRenderer.sprite = offSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FlipLever()
    {
        leverState = !leverState;
        if(leverState)
        {
            spriteRenderer.sprite = onSprite; 
        }
        else
        {
            spriteRenderer.sprite = offSprite; 
        }
    }
}
