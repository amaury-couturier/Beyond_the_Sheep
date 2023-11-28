using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float playerMaxSpeed = 5f;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float acceleration = 7.2f;
    private float targetVelocityX;
    private float currentVelocityX;
    private float inputHorizontal;
    private bool hasFlipped = false;
    public bool isRunning = false;

    [Header("Player Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float originalGravity = 4f;
    [SerializeField] private float fallingGravity = 4.8f;
    [SerializeField] private float coyoteTime = 0.08f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.08f;
    private float jumpBufferCounter;
    [SerializeField] private float airResistance = 2.5f;
    public bool isJumping;

    [Header("Wall Jumping")]
    [SerializeField] private float wallSlidingSpeed = 2f;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.5f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(12f, 14f);
    public bool isWallSliding;
    public bool isWallJumping;

    [Header("Player Dash")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    public bool isDashing;
    private bool canDash = true;

    [Header("Player Ledge Grab")]
    [SerializeField] private float redXOffset, redYOffset, redXSize, redYSize, greenXOffset, greenYOffset, greenXSize, greenYSize;
    private bool greenBox, redBox;
    public bool isGrabbing;
    
    [Header("Componenets")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsSheep;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private ParticleSystem dust;
    public bool isFacingRight = true;

    [Header("Checkpoints")]
    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private float respawnThreshold = -6.0f;

    void Start()
    {
        respawnPoint = transform.position;
    }

    void Update()
    {
        //Simply return in case isDashing is true so the player is not allowed to move or jump while dahsing
        if(isDashing)
        {
            return;
        }

        //User input for horizontal movement
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || isJumping || isDashing || isWallJumping || isGrabbing)
        {
            isRunning = false;
        }
        else
        {
            isRunning =  Mathf.Abs(inputHorizontal) > 0.0f;
        }

        UpdateCoyoteTime();

        UpdateJumpBuffer();

        FasterFallSpeed();

        CheckDashInput();

        HandleJumping();

        HandleJumpHeight();
    
        WallSlide();
        
        WallJump();

        LedgeGrab();
    
        if(!isWallJumping)
        {
            Flip();
        }

        if (transform.position.y <= respawnThreshold || Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            RespawnPlayer(); 
        }
    }

    private void FixedUpdate()
    {
        //Simply return in case isDashing is true so the player is not allowed to move or jump while dahsing
        if(isDashing)
        {
            return;
        }

        MovePlayer();
    }

    // Checkpoint methods
    private void RespawnPlayer()
    {
        gameObject.transform.position = respawnPoint;
        rb.velocity = Vector3.zero;
    }

    //Methods
    //Checks to see if the Ground Check transform is overlapping with any "Ground" Layers, if it is, return true
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround | whatIsSheep | whatIsWall);
    }

    //Checks to see if the Wall Check transform is overlapping with any "Wall" Layers, if it is, return true
    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsWall);
    }

    //Use playerSpeed to move player left or right
    private void MovePlayer()
    {
        if (!isWallJumping && !isGrabbing)
        {
            float targetVelocityX = inputHorizontal * playerMaxSpeed;

            // Apply acceleration/deceleration to movement
            targetVelocityX = inputHorizontal * playerMaxSpeed;
            float t = acceleration * Time.deltaTime;
            currentVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX, t);
            
            // Apply air resistance or linear drag when jumping
            if (!IsGrounded())
            {
                currentVelocityX *= 1f - airResistance * Time.deltaTime;
            }

            rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);
        }
    }

    //If player is grounded, update coyoteTimeCounter to coyoteTime, else decrease coyoteTimeCounter
    private void UpdateCoyoteTime()
    {
        coyoteTimeCounter = IsGrounded() ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
    }

    //If the space button is pressed down, update the jumpBufferCounter to jumpBufferTime, esle decrease jumpBufferCounter
    private void UpdateJumpBuffer()
    {
        jumpBufferCounter = Input.GetKeyDown(KeyCode.Space) ? jumpBufferTime : jumpBufferCounter - Time.deltaTime;
    }

    //Increase gravity when player is falling to make it feel more impactful
    private void FasterFallSpeed()
    {
        if (rb.velocity.y < 0 && !IsGrounded())
        {
            rb.gravityScale = fallingGravity;
        }
        else
        {
            rb.gravityScale = originalGravity;
        }
    }

    //If left shift is pressed and canDash is true, execute coroutine
    private void CheckDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void HandleJumping()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            CreateDust();
            StartCoroutine(JumpCooldown());
        }
    }

    //Variable jump height so that if the space key is released while jumping, the y velocity of the RgidiBody is halved
    private void HandleJumpHeight()
    {
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && inputHorizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        //Allows Player to still wall jump even if turned away from wall for a brief amount of time
        else
        {
            wallJumpingCounter -= Time.deltaTime;
            hasFlipped = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f && hasFlipped)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            
            if(transform.localScale.x != wallJumpingDirection)
            {
                Flip();
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        hasFlipped = false;

        if((isFacingRight && inputHorizontal < 0f) || (!isFacingRight && inputHorizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            CreateDust();
            hasFlipped = true;
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void LedgeGrab()
    {
        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y  + greenYOffset), new Vector2(greenXSize, greenYSize), 0f, (whatIsWall | whatIsGround));
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y  + redYOffset), new Vector2(redXSize, redYSize), 0f, (whatIsWall | whatIsGround));
        
        if (greenBox && !redBox && !isGrabbing && isJumping)
        {
            isGrabbing = true;
        }

        if (isGrabbing)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 0f;
            return;
        }
    }

    // Use for when animations are set up
    public void ChangePos()
    {
        transform.position = new Vector2(transform.position.x + (0.5f * transform.localScale.x), transform.position.y + 0.7f);
        rb.gravityScale = originalGravity;
        isGrabbing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y  + redYOffset), new Vector2(redXSize, redYSize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y  + greenYOffset), new Vector2(greenXSize, greenYSize));
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.6f);
        isJumping = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}