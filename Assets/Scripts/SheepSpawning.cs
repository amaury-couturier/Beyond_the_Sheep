using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawning : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float distanceBehindPlayer;
    [SerializeField] private float respawnThreshold = -6.0f;
    [SerializeField] private float distanceOffset = 0.4f;
    private PlayerMovement playerMovement;
    public bool playerActive = true;

    public GameObject[] spawnedSheep = new GameObject[3];
    private bool[] sheepSpawned = new bool[3];
    
    private Vector3 spawnPosition;

    [SerializeField] private float raycastDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Vector3 leftHitPoint;
    private Vector3 rightHitPoint;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    void Update()
    {
        InputHandler();
        //CheckSpawnOnGroundOrWall();
    }

    private void InputHandler()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, groundLayer | wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, groundLayer | wallLayer);
        
        for (int i = 0; i < 3; i++)
        {
            if (!sheepSpawned[i] && Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (hitRight.collider != null && playerMovement.isFacingRight)
                {
                    // Spawn sheep behind the player when facing right and htting wall
                    float xOffset = -distanceBehindPlayer;
                    spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
                }
                else if (hitLeft.collider != null && !playerMovement.isFacingRight)
                {
                    // Spawn sheep behind the player when facing left and htting wall
                    float xOffset = distanceBehindPlayer;
                    spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
                }
                else
                {
                    // Spawn sheep in front of player when not hittitng any walls
                    float xOffset = (transform.localScale.x > 0) ? distanceBehindPlayer : -distanceBehindPlayer;
                    spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
                }
                
                spawnedSheep[i] = Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
                sheepSpawned[i] = true;
                distanceBehindPlayer += distanceOffset;
            }
            else if (sheepSpawned[i] && (Input.GetKeyDown(KeyCode.Alpha1 + i) || (spawnedSheep[i] != null && spawnedSheep[i].transform.position.y < respawnThreshold)) && !spawnedSheep[i].GetComponent<SheepMovement>().enabled)
            {
                // Destroy the spawned sheep and reset the distanceBehindPlayer
                Destroy(spawnedSheep[i]);
                sheepSpawned[i] = false;
                distanceBehindPlayer -= distanceOffset;
            }
        }
    
        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.E))
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Q))
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

    private void OnDrawGizmos()
    {
         Gizmos.color = Color.red;

        // Simulate raycasts for Gizmos drawing
        Vector3 simulatedLeftHitPoint = transform.position + Vector3.left * raycastDistance;
        Vector3 simulatedRightHitPoint = transform.position + Vector3.right * raycastDistance;

        Gizmos.DrawLine(transform.position, simulatedLeftHitPoint);
        Gizmos.DrawLine(transform.position, simulatedRightHitPoint);
    }
}
