using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float inputHorizontal;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;
    [SerializeField] private float wallSlidingSpeed;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2();

    [SerializeField] private Rigidbody2D rb;

    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isJumping;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsWall;

    void Update()
    {
        //User input for horizontal movement
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        //Coyote time 
        if(IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        //Jump Buffer
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else 
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //Jumping
        //Apply full jumpFroce to the rigidbody on the y axis
        if(jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        //If the player is in the air but the space bar is released, half the rigibody vleocity
        //This allwos the player to have variable jump height
        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    
        //Wall Sliding
        WallSlide();
        //Wall Jumping
        WallJump();
    
        //Flip 
        if(!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        //Use playerSpeed to move player left or right
        if(!isWallJumping)
        {
            rb.velocity = new Vector2(inputHorizontal * playerSpeed, rb.velocity.y);
        }
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

    //
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
        //Allows Player to still wall jump even if turned away from wall for a brief amoutn of time
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
}