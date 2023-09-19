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
    private int activeSheepIndex = -1;

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
        }

        if (Input.GetKeyDown(KeyCode.Q) && activeSheepIndex != -1)
        {
            SwitchToPreviousSheep();
        }
        else if (Input.GetKeyDown(KeyCode.E) && activeSheepIndex != -1)
        {
            SwitchToNextSheep();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && activeSheepIndex == -1 && CountActiveSheep() > 0)
        {
            SwitchToFirstSheep();
        }
        else if (Input.GetKeyDown(KeyCode.E) && activeSheepIndex == -1 && CountActiveSheep() > 0)
        {
            SwitchToFirstSheep();
        }

        // Activate player controller and camera when the first or last spawned sheep is active
        if (activeSheepIndex == 0 && Input.GetKeyDown(KeyCode.E))
        {
            SwitchToPlayer();
        }
        else if (activeSheepIndex == 2 && Input.GetKeyDown(KeyCode.Q))
        {
            SwitchToPlayer();
        }
    }

    private int CountActiveSheep()
    {
        int count = 0;
        for (int i = 0; i < sheepSpawned.Length; i++)
        {
            if (sheepSpawned[i])
            {
                count++;
            }
        }
        return count;
    }

    private void SwitchToNextSheep()
    {
        int nextSheepIndex = (activeSheepIndex + 1) % 3; // Cycle through sheep
        while (!sheepSpawned[nextSheepIndex]) // Find the next active sheep
        {
            nextSheepIndex = (nextSheepIndex + 1) % 3;
        }

        SwitchToSheep(nextSheepIndex);
    }

    private void SwitchToPreviousSheep()
    {
        int previousSheepIndex = (activeSheepIndex - 1 + 3) % 3; // Cycle through sheep
        while (!sheepSpawned[previousSheepIndex]) // Find the previous active sheep
        {
            previousSheepIndex = (previousSheepIndex - 1 + 3) % 3;
        }

        SwitchToSheep(previousSheepIndex);
    }

    private void SwitchToFirstSheep()
    {
        for (int i = 0; i < sheepSpawned.Length; i++)
        {
            if (sheepSpawned[i])
            {
                SwitchToSheep(i);
                break;
            }
        }
    }

    private void SwitchToPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(playerMovement.transform);
        playerMovement.enabled = true;
        if (activeSheepIndex != -1)
        {
            spawnedSheep[activeSheepIndex].GetComponent<SheepMovement>().enabled = false;
            activeSheepIndex = -1; // Reset activeSheepIndex
        }
    }

    private void SwitchToSheep(int index)
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[index].transform);
        playerMovement.enabled = false;
        if (activeSheepIndex != -1)
        {
            spawnedSheep[activeSheepIndex].GetComponent<SheepMovement>().enabled = false;
        }
        spawnedSheep[index].GetComponent<SheepMovement>().enabled = true;
        activeSheepIndex = index;
    }
}
