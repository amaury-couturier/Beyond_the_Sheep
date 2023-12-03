using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep3Animation : MonoBehaviour
{
    private Animator sheep3Animator;
    private SheepMovement sheepMovement;

    public string currentState;
    public string sheep3Idle = "sheep3Idle";
    public string sheep3Walk = "sheep3Walk";

    void Start()
    {
        sheep3Animator = GetComponentInChildren<Animator>();
        sheepMovement = GetComponent<SheepMovement>();
    }

    void Update()
    {
        if (sheepMovement.isRunning && sheepMovement.IsGrounded())
        {
            PlayAnimation(sheep3Walk);
        }
        else if (!sheepMovement.isRunning && sheepMovement.IsGrounded() || sheepMovement.hasSnapped)
        {
            PlayAnimation(sheep3Idle);
        }
        
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        sheep3Animator.Play(newState);
        currentState = newState;
    }

    public void ResetToSheepIdleAnimation()
    {
        PlayAnimation(sheep3Idle);
    }
}
