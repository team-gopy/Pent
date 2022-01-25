using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public bool completedLevel = false;

    private const float GROUNDED_RADIUS = .2f; // radius of a circle to check if the ground is within it 

    private bool grounded;
    private float groundedRecently;
    [SerializeField] private Transform groundCheck; // transform of an element in the bottom of the player
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundedRememberTime = 0.25f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool didDoubleJumped = false;
    public bool doubleJumpEnabled = false;


    [Range(0, 30)] [SerializeField] private float movementMultiplier = 14;
    [Range(0, 30)] [SerializeField] private float inAirMovementMultiplier = 9;



    private Rigidbody2D rigidbody2D;

    private int keyCount = 0;

    private Vector3 checkpoint;
    
    public bool isOnSwapPad = false;

    public AudioSource source;
    public AudioClip jumpSound;
    public AudioClip jump2Sound;

    private Image tutImage;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        checkpoint = transform.position;
        groundLayer = LayerMask.GetMask("ground");
        
        tutImage = GameObject.FindGameObjectWithTag("TutorialImage").GetComponent<Image>();
    }

    private void HandleMovement()
    {

        // gets the x axis's velocity and add 1 * multiplier;
        float horizontalVelocity = grounded
            ? Input.GetAxisRaw("Horizontal") * movementMultiplier
            : Input.GetAxisRaw("Horizontal") * inAirMovementMultiplier;
        rigidbody2D.velocity = new Vector2(horizontalVelocity, rigidbody2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && groundedRecently <= 0 && !didDoubleJumped && doubleJumpEnabled)
        {
            source.PlayOneShot(jump2Sound);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            didDoubleJumped = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && groundedRecently > 0)
        {
            source.PlayOneShot(jumpSound);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            grounded = false;
            groundedRecently = 0;
        }

    }

    public void PlayerCompletedLevel()
    {
        UseKey();
        completedLevel = true;
        Color fadedColor = GetComponent<SpriteRenderer>().color;
        fadedColor.a = 0.3f;
        GetComponent<SpriteRenderer>().color  = fadedColor;
    }
    public void CollectKey()
    {
        keyCount++;
    }

    void UseKey()
    {
        keyCount--;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    private void Die()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        GameController.Instance.levelController.FlashEffect(-0.4f);
        transform.position = checkpoint;
    }

    private void FixedUpdate()
    {
        if (completedLevel) return;

        grounded = false;

        var colliders = Physics2D.OverlapCircleAll(groundCheck.position, GROUNDED_RADIUS, groundLayer);
        foreach (var coll in colliders)
            if (coll.gameObject != gameObject)
            {
                grounded = true;
                groundedRecently = groundedRememberTime;
                didDoubleJumped = false;
            }
    }

    private void Update()
    {
        if (completedLevel) return;

        groundedRecently -= Time.deltaTime;
        HandleMovement();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "spikes")  Die();

        if (other.tag == "dimensional_pad")
        {
            isOnSwapPad = true;
        }
        
        if (other.tag == "checkpoint")
            checkpoint = other.transform.position;

        if(other.tag == "TutorialOff")
        {
            tutImage.GetComponent<TutorialText>().HideCurrentTutorial(other);
            
        }
        if(other.tag == "TutorialOn")
        {
            tutImage.GetComponent<TutorialText>().LoadNextTutorial(other);

        }
        if(other.tag == "TutorialOffB")
        {
            if(this.name == "Blue")
            {
                tutImage.GetComponent<TutorialText>().HideCurrentTutorial(other);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "dimensional_pad")
        {
            isOnSwapPad = false;
        }
    }

}