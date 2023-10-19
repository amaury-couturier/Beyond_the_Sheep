using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("space"))
            LevelLoader.instance.LoadNextLevel();
    }
    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelLoader.instance.LoadNextLevel();
        }
    }*/
}
