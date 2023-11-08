using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAnimation : MonoBehaviour
{
    private Animator sheepAnimator;
    private SheepMovement sheepMovement;

    public string currentState;
    public string sheepIdle = "sheepIdle";
    public string sheepWalk = "sheepWalk";

    void Start()
    {
        sheepAnimator = GetComponentInChildren<Animator>();
        sheepMovement = GetComponent<SheepMovement>();
    }

    void Update()
    {
        if (sheepMovement.isRunning && sheepMovement.IsGrounded())
        {
            PlayAnimation(sheepWalk);
        }
        else if (!sheepMovement.isRunning && sheepMovement.IsGrounded() || sheepMovement.hasSnapped)
        {
            PlayAnimation(sheepIdle);
        }
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        sheepAnimator.Play(newState);
        currentState = newState;
    }

    public void ResetToSheepIdleAnimation()
    {
        PlayAnimation(sheepIdle);
    }
}
