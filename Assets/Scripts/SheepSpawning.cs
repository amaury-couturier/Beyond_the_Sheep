using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawning : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float distanceBehindPlayer;
    private PlayerMovement playerMovement;
    public bool playerActive = true;

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
                distanceBehindPlayer += 0.4f;
            }
            else if (sheepSpawned[i] && Input.GetKeyDown(KeyCode.Alpha1 + i) && !spawnedSheep[i].GetComponent<SheepMovement>().enabled)
            {
                Destroy(spawnedSheep[i]);
                sheepSpawned[i] = false;
                distanceBehindPlayer -= 0.4f;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (playerMovement.enabled)
            {
                if (!sheepSpawned[0] && !sheepSpawned[1] && !sheepSpawned[2])
                {
                    return;
                }

                else if (sheepSpawned[0])
                { 
                    SwitchToFirstSheep(0);
                }

                else if (!sheepSpawned[0] && sheepSpawned[1])
                { 
                    SwitchToFirstSheep(1);
                }

                else if (!sheepSpawned[0] && !sheepSpawned[1] && sheepSpawned[2])
                {
                    SwitchToFirstSheep(2);
                }
            }

            else if (spawnedSheep[0] && spawnedSheep[0].GetComponent<SheepMovement>().enabled)
            {
                if (sheepSpawned[1])
                {
                    SwitchToSheep(0, 1);
                }
                
                else if (sheepSpawned[2] && !sheepSpawned[1])
                {
                    SwitchToSheep(0, 2); 
                }

                else if (!sheepSpawned[2] && !sheepSpawned[1])
                {
                    SwitchToPlayer(0);
                }
            }

            else if (spawnedSheep[1] && spawnedSheep[1].GetComponent<SheepMovement>().enabled)
            {
                if (sheepSpawned[2])
                {
                    SwitchToSheep(1, 2);
                }
                
                else if (!sheepSpawned[2])
                {
                    SwitchToPlayer(1);
                }
            }

            else if (spawnedSheep[2] && spawnedSheep[2].GetComponent<SheepMovement>().enabled)
            {
                SwitchToPlayer(2);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (playerMovement.enabled)
            {
                if (!sheepSpawned[0] && !sheepSpawned[1] && !sheepSpawned[2])
                {
                    return;
                }

                else if (sheepSpawned[2])
                { 
                    SwitchToFirstSheep(2);
                }

                else if (!sheepSpawned[2] && sheepSpawned[1])
                {
                    SwitchToFirstSheep(1);
                }

                else if (sheepSpawned[0] && !sheepSpawned[1] && !sheepSpawned[2])
                { 
                    SwitchToFirstSheep(0);
                }
            }

            else if (spawnedSheep[2] && spawnedSheep[2].GetComponent<SheepMovement>().enabled)
            {
                if (sheepSpawned[1])
                {
                    SwitchToSheep(2, 1); 
                }
                
                else if (sheepSpawned[0] && !sheepSpawned[1])
                {
                    SwitchToSheep(2, 0);    
                }

                else if (!sheepSpawned[0] && !sheepSpawned[1])
                {
                    SwitchToPlayer(2);
                }
            }

            else if (spawnedSheep[1] && spawnedSheep[1].GetComponent<SheepMovement>().enabled)
            {
                if (sheepSpawned[0])
                {
                    SwitchToSheep(1, 0); 
                }
                
                else if (!sheepSpawned[0])
                {
                    SwitchToPlayer(1);
                }
            }

            else if (spawnedSheep[0] && spawnedSheep[0].GetComponent<SheepMovement>().enabled)
            {
                SwitchToPlayer(0);
            }
        }
    }

    private void SwitchToPlayer(int old)
    {
        playerActive = true;
        Camera.main.GetComponent<CameraFollow>().SetTarget(playerMovement.transform);
        spawnedSheep[old].GetComponent<SheepMovement>().enabled = false;
        playerMovement.enabled = true;
    }

    private void SwitchToSheep(int current, int newer)
    {
        playerActive = false;
        Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[newer].transform);
        spawnedSheep[current].GetComponent<SheepMovement>().enabled = false;
        spawnedSheep[newer].GetComponent<SheepMovement>().enabled = true;
    }

    private void SwitchToFirstSheep(int first)
    {
        playerActive = false;
        Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[first].transform);
        playerMovement.enabled = false;
        spawnedSheep[first].GetComponent<SheepMovement>().enabled = true; 
    }
}
