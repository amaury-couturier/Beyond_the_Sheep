using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float respawnThreshold = -6.0f;

    private Vector3 respawnPoint;

    void Start()
    {
        respawnPoint = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y <= respawnThreshold)
        {
            player.position = respawnPoint;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            Debug.Log("Checkpoint");
            respawnPoint = player.position;
        }
    }
}
