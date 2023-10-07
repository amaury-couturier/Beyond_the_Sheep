using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //[SerializeField] private Vector3 offset = new Vector3();
    [SerializeField] private float moveTime = .15f;
    private Vector3 velocity = Vector3.zero;

    public Transform target;
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z) /*+ offset*/;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, moveTime);
    }
}
