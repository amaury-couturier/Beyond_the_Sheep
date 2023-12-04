using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float playerBounce;
    [SerializeField] private float sheepBounce;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Be sure to change tag to sheep later on 
        if (collider.CompareTag("Player"))
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.up * playerBounce, ForceMode2D.Impulse);
            }
        }
        if (collider.CompareTag("Sheep"))
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.up * sheepBounce, ForceMode2D.Impulse);
            }
        }
    }
}
