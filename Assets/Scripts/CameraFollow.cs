using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float moveTime = 0.15f;
    //[SerializeField] private float changeDirectionSpeed = 0.5f;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float minX = -2f;
    [SerializeField] private float aheadDistance = 2.0f; 

    private Vector3 velocity = Vector3.zero;
    public Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        // Calculate the target position with clamping
        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(target.position.x, minX, Mathf.Infinity),
            Mathf.Clamp(target.position.y, minY, Mathf.Infinity),
            transform.position.z
        );

        // Calculate the camera's look-ahead position
        Vector3 lookAheadPosition = targetPosition + new Vector3(target.localScale.x * aheadDistance, 0, 0);
        

        // Smoothly follow the look-ahead position
        transform.position = Vector3.SmoothDamp(transform.position, lookAheadPosition, ref velocity, moveTime);
    }
}
