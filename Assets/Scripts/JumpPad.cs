using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounce;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Be sure to change tag to sheep later on 
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Sheep"))
        {
            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
}
