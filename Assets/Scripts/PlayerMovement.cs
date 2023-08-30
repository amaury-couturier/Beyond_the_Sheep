using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    private float inputHorizontal;

    [Header("Player Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float originalGravity;
    [SerializeField] private float fallingGravity;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;
    private bool isJumping;

    [Header("Wall Jumping")]
    [SerializeField] private float wallSlidingSpeed;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2();
    private bool isWallSliding;
    private bool isWallJumping;

    [Header("Player Dash")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;
    
    [Header("Componenets")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private TrailRenderer tr;
    private bool isFacingRight = true;

    void Update()
    {
        //Simply return in case isDashing is true so the player is not allowed to move or jump while dahsing
        if(isDashing)
        {
            return;
        }

        //User input for horizontal movement
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        UpdateCoyoteTime();

        UpdateJumpBuffer();

        FasterFallSpeed();

        CheckDashInput();

        HandleJumping();

        HandleJumpHeight();
    
        WallSlide();
        
        WallJump();
    
        if(!isWallJumping)
        {
            Flip();
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

    //Methods
    //Checks to see if the Ground Check transform is overlapping with any "Ground" Layers, if it is, return true
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    //Checks to see if the Wall Check transform is overlapping with any "Wall" Layers, if it is, return true
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsWall);
    }

    //Use playerSpeed to move player left or right
    private void MovePlayer()
    {
        if (!isWallJumping)
        {
            float targetVelocityX = inputHorizontal * playerSpeed;
    
            // Apply acceleration
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, targetVelocityX, acceleration * Time.fixedDeltaTime), rb.velocity.y);

            // Apply deceleration when there's no input
            if (inputHorizontal == 0f)
            {
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, deceleration * Time.fixedDeltaTime), rb.velocity.y);
            }
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
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
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
        if((isFacingRight && inputHorizontal < 0f) || (!isFacingRight && inputHorizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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