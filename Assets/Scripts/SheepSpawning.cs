using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawning : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float distanceBehindPlayer;
    private GameObject spawnedSheep; 
    public bool sheep1Spawned = false;

    void Update()
    {
        InputHandler();
    }

    private void InputHandler() 
    {
        if (!sheep1Spawned)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && transform.localScale.x > 0)
            {
                spawnedSheep = Instantiate(sheepPrefab, new Vector3(transform.position.x - distanceBehindPlayer, transform.position.y, 0), Quaternion.identity);
                sheep1Spawned = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && transform.localScale.x < 0)
            {
                spawnedSheep = Instantiate(sheepPrefab, new Vector3(transform.position.x + distanceBehindPlayer, transform.position.y, 0), Quaternion.identity);
                sheep1Spawned = true;
            }
        }

        else if (sheep1Spawned && spawnedSheep != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Destroy(spawnedSheep); // Destroy the specific instance
                sheep1Spawned = false;
            }
        }
    }
}
