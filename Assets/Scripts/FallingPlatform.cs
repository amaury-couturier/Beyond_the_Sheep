using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1.0f;
    [SerializeField] private float destroyDelay = 2.0f;

    [SerializeField] private Rigidbody2D rb; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player")) || (collision.gameObject.CompareTag("Sheep")))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().isTrigger = true;
        Destroy(gameObject, destroyDelay);
    }
}
