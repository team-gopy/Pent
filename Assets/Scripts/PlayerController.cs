using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool suppersed = false;
    
    private const float GROUNDED_RADIUS = .2f; // radius of a circle to check if the ground is within it 
    
    private bool grounded;
    private float groundedRecently;
    [SerializeField] private Transform groundCheck; // transform of an element in the bottom of the player
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundedRememberTime = 0.25f;
    
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool didDoubleJumped = false;
    [SerializeField] private bool doubleJumpEnabled = false;


    [Range(0, 30)] [SerializeField] private float movementMultiplier = 14;
    [Range(0, 30)] [SerializeField] private float inAirMovementMultiplier = 9;
    
    private Rigidbody2D rigidbody2D;

    private int keyCount = 0;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("ground");
    }

    private void HandleMovement()
    {
        
        // gets the x axis's velocity and add 1 * multiplier;
        float horizontalVelocity = grounded ? Input.GetAxisRaw("Horizontal") * movementMultiplier : Input.GetAxisRaw("Horizontal") * inAirMovementMultiplier;
        rigidbody2D.velocity = new Vector2(horizontalVelocity, rigidbody2D.velocity.y);

        if (Input.GetKey(KeyCode.Space) && groundedRecently > 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            grounded = false;
            groundedRecently = 0;
        }

        if (Input.GetKey(KeyCode.Space) && groundedRecently <= 0 && !didDoubleJumped && doubleJumpEnabled)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            didDoubleJumped = true;
        }
    }
    
    public void CollectKey()
    {
        keyCount++;
    }

    private void FixedUpdate()
    {
        if(suppersed) return;
        
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
        if(suppersed) return;
        
        groundedRecently -= Time.deltaTime;
        HandleMovement();
    }
    
}