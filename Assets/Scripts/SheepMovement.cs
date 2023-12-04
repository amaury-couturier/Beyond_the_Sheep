using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private PhysicsMaterial2D frictionMaterial;
    [SerializeField] private PhysicsMaterial2D nonFrictionMaterial;
    [SerializeField] private float raycastLength = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sheep Fall")]
    [SerializeField] private float originalGravity;
    [SerializeField] private float fallingGravity;

    [Header("Sheep Snapping")]
    public bool hasSnapped = false; 
    [SerializeField] private Transform snapCheck;
    private bool canMove = true; 
    [SerializeField] private float moveDisableTime = 0.4f; 
    private float moveDisableTimer = 0f;

    [Header("Checkpoints")]
    private Vector3 respawnPoint;
    [SerializeField] public float respawnThreshold = -10.0f;

    [Header("Componenets")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsSheep;
    public bool isFacingRight = true;

    private SheepSpawning sheepSpawning;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        sheepSpawning = player.GetComponent<SheepSpawning>();
    }

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
            isRunning = Mathf.Abs(inputHorizontal) > 0.0f;
        }

        if (IsGrounded())
        {
            hasSnapped = false;
        }

        if (!canMove)
        {
            moveDisableTimer -= Time.deltaTime;

            // If the timer has expired, re-enable movement
            if (moveDisableTimer <= 0f)
            {
                canMove = true;
                moveDisableTimer = 0f;
            }
        }

        if (transform.position.y <= respawnThreshold || Input.GetKeyDown(KeyCode.C))
        {
            sheepSpawning.SwitchBackToPlayerAfterDeath();
            for (int i = 0; i < sheepSpawning.spawnedSheep.Length; i++)
            {
                if (sheepSpawning.spawnedSheep[i] && sheepSpawning.spawnedSheep[i] != null && sheepSpawning.spawnedSheep[i].GetComponent<SheepMovement>().enabled)
                {
                    sheepSpawning.spawnedSheep[i].GetComponent<SheepMovement>().enabled = false;
                    sheepSpawning.DespawnLogic(i);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        else 
        {
            CheckForSheepBelow();

            FasterFallSpeed();

            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveSheep();
        }
    }

    //Methods
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void RespawnSheep()
    {
        gameObject.transform.position = respawnPoint;
        rb.velocity = Vector3.zero;
    }

    private void CheckForSheepBelow()
    {
        Vector2 rayOrigin = groundCheck.position;
        Vector2 rayDirection = Vector2.down;

        float rayDistance = 0.1f; 

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);

        if (hit.collider != null)
        {
            // Check if the hit object is not the same as the current sheep
            if (hit.collider.transform != transform)
            {
                if (hit.collider.CompareTag("Sheep") && !hasSnapped)
                {
                    Transform hitSheepTransform = hit.collider.transform;
                    Transform hitSnapCheck = hitSheepTransform.Find("Snap Check");

                    if (hitSnapCheck != null)
                    {
                        hasSnapped = true;
                        Vector2 newPosition = new Vector2(hitSnapCheck.position.x, transform.position.y);
                        transform.position = newPosition;
                        moveDisableTimer = moveDisableTime;
                        canMove = false; 
                    }
                }
            }
        }
    }   

    private void MoveSheep()
    {
        float targetVelocityX = inputHorizontal * SheepMaxSpeed;
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

    private void UpdateFrictionMaterial()
    {
        bool isLeftRaycastHit = Physics2D.Raycast(transform.position, Vector2.left, raycastLength, groundLayer | wallLayer);
        bool isRightRaycastHit = Physics2D.Raycast(transform.position, Vector2.right, raycastLength, groundLayer | wallLayer);

        if (isLeftRaycastHit || isRightRaycastHit)
        {
            SetFrictionMaterial(nonFrictionMaterial);
        }
        else
        {
            SetFrictionMaterial(frictionMaterial);
        }
    }

    private void SetFrictionMaterial(PhysicsMaterial2D newFrictionMaterial)
    {
        GetComponent<Collider2D>().sharedMaterial = newFrictionMaterial;
    }

    private void Flip()
    {
        if ((isFacingRight && inputHorizontal < 0f) || (!isFacingRight && inputHorizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
