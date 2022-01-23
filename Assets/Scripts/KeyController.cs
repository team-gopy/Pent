using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class KeyController : MonoBehaviour
{
    [SerializeField] private GameObject keyLight;
    [Range(0, 1)] [SerializeField] private float lightIntensity = 0.15f;
    
    public AudioSource source;
    public AudioClip clip;

    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    
    public bool blueKey;
    private bool collected;
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController collectingPlayer = other.gameObject.GetComponent<PlayerController>();
            collectingPlayer.CollectKey();
            source.PlayOneShot(clip);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            keyLight.GetComponent<Light2D>().intensity = 0;
            collected = true;

        }
    }

    // Use this for initialization
    void Start () 
    {
        source = GetComponent<AudioSource>();
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        UpdateKeys(0);
    }
     
    public void UpdateKeys(int dimension)
    {
        if(collected) return;
        
        if(dimension == 0)
        {
            if(blueKey)
            {
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider2D>().enabled = true;
                keyLight.GetComponent<Light2D>().intensity = lightIntensity;
            }
            else
            {
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                keyLight.GetComponent<Light2D>().intensity = 0;
            }
        }
        else
        {
            if(blueKey)
            {
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                keyLight.GetComponent<Light2D>().intensity = 0;
            }
            else
            {
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider2D>().enabled = true;
                keyLight.GetComponent<Light2D>().intensity = lightIntensity;
            }
        }
    }
    // Update is called once per frame
    void Update () 
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }
}
