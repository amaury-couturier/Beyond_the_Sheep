using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    [Header("Sheep Movement")]
    [SerializeField] private float SheepMaxSpeed;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private float acceleration;
    private float targetVelocityX;
    private float currentVelocityX;
    private float inputHorizontal;

    [Header("Sheep Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float originalGravity;
    [SerializeField] private float fallingGravity;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;
    private bool isJumping;
    
    [Header("Componenets")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool isFacingRight = true;

    void Update()
    {

        //User input for horizontal movement
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        UpdateCoyoteTime();

        UpdateJumpBuffer();

        FasterFallSpeed();

        HandleJumping();

        HandleJumpHeight();
        
        Flip();
    
    }

    private void FixedUpdate()
    {
        MoveSheep();
    }

    //Methods
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void MoveSheep()
    {
        float targetVelocityX = inputHorizontal * SheepMaxSpeed;

        // Apply acceleration/deceleration to movement
        targetVelocityX = inputHorizontal * SheepMaxSpeed;
        float t = acceleration * Time.deltaTime;
        currentVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX, t);
        rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);
    }

    private void UpdateCoyoteTime()
    {
        coyoteTimeCounter = IsGrounded() ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
    }

    private void UpdateJumpBuffer()
    {
        jumpBufferCounter = Input.GetKeyDown(KeyCode.Space) ? jumpBufferTime : jumpBufferCounter - Time.deltaTime;
    }

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

    private void HandleJumping()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }
    }

    private void HandleJumpHeight()
    {
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
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


}