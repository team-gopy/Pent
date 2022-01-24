using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CheckpointController : MonoBehaviour
{
    SpriteRenderer bulbB;
    SpriteRenderer bulbR;

    // Color onColor = new Color(1f,1f,57f/255f);
    private bool bulbOn = false;
    private bool lerpBlue = false;
    private bool lerpRed = false;

    Color blueColor;
    Color redColor;

    // Start is called before the first frame update
    void Start()
    {
        bulbB = gameObject.transform.Find("Bulb_B").GetComponent<SpriteRenderer>();
        bulbR = gameObject.transform.Find("Bulb_R").GetComponent<SpriteRenderer>();

    }
    
    // Update is called once per frame
    void Update()
    {
        if(lerpBlue)
        {
            bulbB.color = Color.Lerp(bulbB.color, blueColor, 0.005f);
            if(bulbB.color == blueColor)
            {
                lerpBlue = false;
            }
        }
        if(lerpRed)
        {
            bulbR.color = Color.Lerp(bulbR.color, redColor, 0.005f);            
            if(bulbR.color == redColor)
            {
                
                lerpRed = false;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(other.name == "Blue")
            {
                blueColor = other.GetComponent<SpriteRenderer>().color;
                lerpBlue = true;
                gameObject.transform.Find("BLight").GetComponent<Light2D>().enabled = true;
            }
            else
            {
                redColor = other.GetComponent<SpriteRenderer>().color;
                lerpRed = true;
                gameObject.transform.Find("RLight").GetComponent<Light2D>().enabled = true;
            }
        }
    }
}
