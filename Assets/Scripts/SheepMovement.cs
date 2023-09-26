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
    public bool isRunning = false;

    [Header("Sheep Fall")]
    [SerializeField] private float originalGravity;
    [SerializeField] private float fallingGravity;
    
    [Header("Componenets")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool isFacingRight = true;

    void Update()
    {

        //User input for horizontal movement
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(inputHorizontal) == 0)
        {
            isRunning = false;
        }
        else
        {
            isRunning =  Mathf.Abs(inputHorizontal) > 0.0f;;
        }

        FasterFallSpeed();

        Flip();
    
    }

    private void FixedUpdate()
    {
        MoveSheep();
    }

    //Methods
    
    public bool IsGrounded()
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

    public void DisableMovement()
    {
        // Disable input and movement
        inputHorizontal = 0f;
        rb.velocity = Vector2.zero;
        enabled = false; // Disable the entire SheepMovement script
    }
}