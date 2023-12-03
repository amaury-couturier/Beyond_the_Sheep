using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep2Animation : MonoBehaviour
{
    private Animator sheep2Animator;
    private SheepMovement sheepMovement;

    public string currentState;
    public string sheep2Idle = "sheep2Idle";
    public string sheep2Walk = "sheep2Walk";

    void Start()
    {
        sheep2Animator = GetComponentInChildren<Animator>();
        sheepMovement = GetComponent<SheepMovement>();
    }

    void Update()
    {
        if (sheepMovement.isRunning && sheepMovement.IsGrounded())
        {
            PlayAnimation(sheep2Walk);
        }
        else if (!sheepMovement.isRunning && sheepMovement.IsGrounded() || sheepMovement.hasSnapped)
        {
            PlayAnimation(sheep2Idle);
        }
        
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        sheep2Animator.Play(newState);
        currentState = newState;
    }

    public void ResetToSheepIdleAnimation()
    {
        PlayAnimation(sheep2Idle);
    }
}
