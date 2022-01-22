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


    [Range(0, 0.5f)] [SerializeField] private float movementMultiplier = 0.069f;
    [Range(0, 1)] [SerializeField] private float horizontalDampingWhenStopping = 0.5f;
    [Range(0, 1)] [SerializeField] private float horizontalDampingWhenTurning = .05f;
    [Range(0, 1)] [SerializeField] private float horizontalDampingBasic = .05f;
    
    private Rigidbody2D rigidbody2D;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("ground");
    }

    private void HandleMovement()
    {
        
        // gets the x axis's velocity and add 1 * multiplier;
        float horizontalVelocity = rigidbody2D.velocity.x;
        horizontalVelocity += Input.GetAxisRaw("Horizontal") * movementMultiplier ;

        // if left or right are not pressed that means the player wanna stop
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.deltaTime * 10f);
        else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(horizontalVelocity))
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.deltaTime * 10f);
        else
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingBasic, Time.deltaTime * 10f);
        
        // setting the new vector as the velocity
        rigidbody2D.velocity = new Vector2(horizontalVelocity, rigidbody2D.velocity.y);
        
        if (Input.GetKey(KeyCode.Space) && groundedRecently > 0)
        {
            rigidbody2D.velocity = Vector2.up * jumpForce;
            grounded = false;
            groundedRecently = 0;
        }

        if (Input.GetKey(KeyCode.Space) && !grounded && !didDoubleJumped && doubleJumpEnabled)
        {
            rigidbody2D.velocity = Vector2.up * jumpForce;
            didDoubleJumped = true;
        }
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

    // Okkio: Shift between players (Dimensional Shift)
    private void DimensionalShift()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Pressing Shift");
        }
    }

    private void Update()
    {
        if(suppersed) return;
        
        groundedRecently -= Time.deltaTime;
        HandleMovement();
    }
}