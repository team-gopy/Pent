using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public AudioSource source;
    public AudioClip clip;
    public bool leverState = false;

    [SerializeField] private List<DimensionalGates> connectedGates;
    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !pressed)
        {
            gameObject.transform.position -= new Vector3 (0.0f,0.1f,0.0f);
            source.PlayOneShot(clip);
            OnPressurePadActivated();
            pressed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPressurePadActivated()
    {
        foreach(DimensionalGates gate in connectedGates)
        {
            gate.FlipGateColor();
        }
    }
}


