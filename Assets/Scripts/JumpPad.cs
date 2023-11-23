using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounce;
    [SerializeField] private AudioSource jumpSoundEffect;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Be sure to change tag to sheep later on 
        if (collider.CompareTag("Player") || collider.CompareTag("Sheep"))
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                jumpSoundEffect.Play();
                rb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            }
        }
    }
}
