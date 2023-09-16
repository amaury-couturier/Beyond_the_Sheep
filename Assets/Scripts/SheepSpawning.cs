using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawning : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float distanceBehindPlayer;
    private PlayerMovement playerMovement;
    
    private GameObject[] spawnedSheep = new GameObject[3];
    private bool[] sheepSpawned = new bool[3];

    void Awake() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!sheepSpawned[i] && Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                float xOffset = (transform.localScale.x > 0) ? -distanceBehindPlayer : distanceBehindPlayer;
                Vector3 spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
                spawnedSheep[i] = Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
                sheepSpawned[i] = true;
            }

            else if (sheepSpawned[i] && Input.GetKeyDown(KeyCode.Alpha1 + i) && !spawnedSheep[i].GetComponent<SheepMovement>().enabled)
            {
                Destroy(spawnedSheep[i]);
                sheepSpawned[i] = false;
            }
            
            if (sheepSpawned[i] && Input.GetKeyDown(KeyCode.Q) && playerMovement.enabled)
            {
                Debug.Log("Called");

                Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[0].transform);
                
                spawnedSheep[0].GetComponent<SheepMovement>().enabled = true;

                if (spawnedSheep[1])
                    spawnedSheep[1].GetComponent<SheepMovement>().enabled = false;

                if (spawnedSheep[2])
                    spawnedSheep[2].GetComponent<SheepMovement>().enabled = false;

                playerMovement.enabled = false;
            }


            if (sheepSpawned[2] && Input.GetKeyDown(KeyCode.E) && playerMovement.enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[2].transform);

                if (spawnedSheep[0])
                    spawnedSheep[0].GetComponent<SheepMovement>().enabled = false;

                if (spawnedSheep[1])
                    spawnedSheep[1].GetComponent<SheepMovement>().enabled = false;

                spawnedSheep[2].GetComponent<SheepMovement>().enabled = true;

                playerMovement.enabled = false;
            }

            if (sheepSpawned[0] && Input.GetKeyDown(KeyCode.E) && !playerMovement.enabled && spawnedSheep[0].GetComponent<SheepMovement>().enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(playerMovement.transform);

                spawnedSheep[0].GetComponent<SheepMovement>().enabled = false;

                if (spawnedSheep[1])
                    spawnedSheep[1].GetComponent<SheepMovement>().enabled = false;
                    
                if (spawnedSheep[2])
                    spawnedSheep[2].GetComponent<SheepMovement>().enabled = false;

                playerMovement.enabled = true;
            }

            if (sheepSpawned[1] && sheepSpawned[0] && Input.GetKeyDown(KeyCode.Q) && !playerMovement.enabled && spawnedSheep[0].GetComponent<SheepMovement>().enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[1].transform);

                if (spawnedSheep[0])
                    spawnedSheep[0].GetComponent<SheepMovement>().enabled = false;

                spawnedSheep[1].GetComponent<SheepMovement>().enabled = true;
                    
                if (spawnedSheep[2])
                    spawnedSheep[2].GetComponent<SheepMovement>().enabled = false;

                playerMovement.enabled = false;
            }

            if (sheepSpawned[2] && sheepSpawned[1] && Input.GetKeyDown(KeyCode.Q) && !playerMovement.enabled && spawnedSheep[1].GetComponent<SheepMovement>().enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[2].transform);

                if (spawnedSheep[0])
                    spawnedSheep[0].GetComponent<SheepMovement>().enabled = false;

                if (spawnedSheep[1])
                    spawnedSheep[1].GetComponent<SheepMovement>().enabled = false;
                    
                spawnedSheep[2].GetComponent<SheepMovement>().enabled = true;

                playerMovement.enabled = false;
            }

            if (sheepSpawned[0] && sheepSpawned[1] && Input.GetKeyDown(KeyCode.E) && !playerMovement.enabled && spawnedSheep[1].GetComponent<SheepMovement>().enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[0].transform);

                spawnedSheep[0].GetComponent<SheepMovement>().enabled = true;

                if (spawnedSheep[1])
                    spawnedSheep[1].GetComponent<SheepMovement>().enabled = false;
                    
                if (spawnedSheep[2])
                    spawnedSheep[2].GetComponent<SheepMovement>().enabled = false;

                playerMovement.enabled = false;
            }

            if (sheepSpawned[1] && spawnedSheep[2] && Input.GetKeyDown(KeyCode.E) && !playerMovement.enabled && spawnedSheep[2].GetComponent<SheepMovement>().enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[1].transform);

                if (spawnedSheep[0])
                    spawnedSheep[0].GetComponent<SheepMovement>().enabled = false;

                spawnedSheep[1].GetComponent<SheepMovement>().enabled = true;
                    
                if (spawnedSheep[2])
                    spawnedSheep[2].GetComponent<SheepMovement>().enabled = false;

                playerMovement.enabled = false;
            }

            if (spawnedSheep[2] && Input.GetKeyDown(KeyCode.Q) && !playerMovement.enabled && spawnedSheep[2].GetComponent<SheepMovement>().enabled)
            {
                Camera.main.GetComponent<CameraFollow>().SetTarget(playerMovement.transform);

                if (spawnedSheep[0])
                    spawnedSheep[0].GetComponent<SheepMovement>().enabled = false;

                if (spawnedSheep[1])
                    spawnedSheep[1].GetComponent<SheepMovement>().enabled = false;
                    
                spawnedSheep[2].GetComponent<SheepMovement>().enabled = false;

                playerMovement.enabled = true;
            }
        }

        
    }
}
