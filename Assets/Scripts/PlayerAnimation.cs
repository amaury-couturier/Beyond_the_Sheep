using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerMovement playerMovement;

    public string currentState;
    public string playerIdle = "playerIdle";
    public string playerRun = "playerRun";
    public string playerJump = "playerJump";
    public string playerLedgeGrab = "playerLedgeGrab";
    public string playerWallJump = "playerWallJump";
    public string playerWallSlide = "playerWallSlide";
    public string playerDash = "playerDash";


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isRunning && playerMovement.IsGrounded())
        {
            PlayAnimation(playerRun);
        }
        else if (!playerMovement.isRunning && !playerMovement.isDashing && !playerMovement.isGrabbing && !playerMovement.isWallJumping && !playerMovement.isWallSliding && !playerMovement.isJumping)
        {
            PlayAnimation(playerIdle);
        }
        else if (playerMovement.isJumping)
        {
            PlayAnimation(playerJump);
        }
        else if (playerMovement.isDashing)
        {
            PlayAnimation(playerDash);
        }
        else if (playerMovement.isWallJumping)
        {
            PlayAnimation(playerWallJump);
        }
        else if (playerMovement.isWallSliding)
        {
            PlayAnimation(playerWallSlide);
        }
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        playerAnimator.Play(newState);
        currentState = newState;
    }
}
