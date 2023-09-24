using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSnap : MonoBehaviour
{
    private bool isSnapped = false;
    private Rigidbody2D rb;
    private SheepMovement sheepMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sheepMovement = GetComponent<SheepMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSnapped && collision.gameObject.CompareTag("Sheep"))
        {
            SnapToSheep(collision.gameObject);
        }
    }

    private void SnapToSheep(GameObject otherSheep)
    {
        // Snap to the top of the other sheep
        Vector2 snapPosition = otherSheep.transform.position + Vector3.up;
        rb.velocity = Vector2.zero;
        rb.position = snapPosition;

        // Disable movement in the SheepMovement script
        sheepMovement.DisableMovement();

        isSnapped = true;
    }

    private void Update()
    {
        if (isSnapped && Input.GetKeyDown(KeyCode.Tab))
        {
            // Re-enable the movement script when the Tab key is pressed
            sheepMovement.enabled = true;
            isSnapped = false;
        }
    }
}
