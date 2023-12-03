using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawning : MonoBehaviour
{
    [SerializeField] private GameObject[] sheepPrefabs;
    [SerializeField] private float distanceBehindPlayer;
    [SerializeField] private float respawnThreshold = -6.0f;
    [SerializeField] private float distanceOffset = 0.4f;
    private PlayerMovement playerMovement;
    public bool playerActive = true;
    private GameObject player;

    public GameObject[] spawnedSheep = new GameObject[3];
    private bool[] sheepSpawned = new bool[3];
    
    private Vector3 spawnPosition;

    [SerializeField] private float raycastDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private AudioSource whistle;
    [SerializeField] private GameObject arrow;
    private Vector3 leftHitPoint;
    private Vector3 rightHitPoint;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        InputHandler();
        
        if (player.transform.position.y <= playerMovement.respawnThreshold)
        {
            SwitchBackToPlayerAfterDeath();
            DespawnAllSheep();
        }

        if (playerActive)
        {
            MoveArrowToSpot(playerMovement.transform.position);

            
        }
        else
        {
            for (int i = 0; i < spawnedSheep.Length; i++)
            {
                if (sheepSpawned[i] && spawnedSheep[i] != null && spawnedSheep[i].GetComponent<SheepMovement>().enabled)
                {
                    MoveArrowToSpot(spawnedSheep[i].transform.position);
                    break;
                }
            }
        }
    }

    private void InputHandler()
    {
        if (!sheepSpawned[0] && Input.GetKeyDown(KeyCode.Alpha1) && playerMovement.IsGrounded())
        {
            SpawnLogic(0, 0);
        }
        else if (sheepSpawned[0] && (Input.GetKeyDown(KeyCode.Alpha1) && spawnedSheep[0] != null || (spawnedSheep[0] && spawnedSheep[0].transform.position.y < respawnThreshold)) && !spawnedSheep[0].GetComponent<SheepMovement>().enabled)
        {
            DespawnLogic(0);
        }

        if (!sheepSpawned[1] && Input.GetKeyDown(KeyCode.Alpha2) && playerMovement.IsGrounded())
        {
            SpawnLogic(1, 1);
        }
        else if (sheepSpawned[1] && (Input.GetKeyDown(KeyCode.Alpha2) && spawnedSheep[1] != null || (spawnedSheep[1] != null && spawnedSheep[1].transform.position.y < respawnThreshold)) && !spawnedSheep[1].GetComponent<SheepMovement>().enabled)
        {
            DespawnLogic(1);
        }

        if (!sheepSpawned[2] && Input.GetKeyDown(KeyCode.Alpha3) && playerMovement.IsGrounded())
        {
            SpawnLogic(2, 2);
        }
        else if (sheepSpawned[2] && (Input.GetKeyDown(KeyCode.Alpha3) && spawnedSheep[2] != null || (spawnedSheep[2] != null && spawnedSheep[2].transform.position.y < respawnThreshold)) && !spawnedSheep[2].GetComponent<SheepMovement>().enabled)
        {
            DespawnLogic(2);
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
        MoveArrowToSpot(playerMovement.transform.position);
        spawnedSheep[old].GetComponent<SheepMovement>().enabled = false;
        playerMovement.enabled = true;
    }

    private void SwitchBackToPlayerAfterDeath()
    {
        playerActive = true;
        Camera.main.GetComponent<CameraFollow>().SetTarget(playerMovement.transform);
        MoveArrowToSpot(playerMovement.transform.position);

        // Find the currently active sheep and disable its movement
        for (int i = 0; i < spawnedSheep.Length; i++)
        {
            if (sheepSpawned[i] && spawnedSheep[i] != null && spawnedSheep[i].GetComponent<SheepMovement>().enabled)
            {
                spawnedSheep[i].GetComponent<SheepMovement>().enabled = false;
                break;
            }
        }

        playerMovement.enabled = true;
    }

    private void SwitchToSheep(int current, int newer)
    {
        playerActive = false;
        Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[newer].transform);
        MoveArrowToSpot(spawnedSheep[newer].transform.position);
        spawnedSheep[current].GetComponent<SheepMovement>().enabled = false;
        spawnedSheep[newer].GetComponent<SheepMovement>().enabled = true;
    }

    private void SwitchToFirstSheep(int first)
    {
        playerActive = false;
        Camera.main.GetComponent<CameraFollow>().SetTarget(spawnedSheep[first].transform);
        MoveArrowToSpot(spawnedSheep[first].transform.position);
        playerMovement.enabled = false;
        spawnedSheep[first].GetComponent<SheepMovement>().enabled = true; 
    }

    void SpawnLogic(int index, int prefabNum)
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, groundLayer | wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, groundLayer | wallLayer);

        if (hitRight.collider != null && playerMovement.isFacingRight)
        {
            // Spawn sheep behind the player when facing right and hitting a wall
            float xOffset = -distanceBehindPlayer;
            spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
        }
        else if (hitLeft.collider != null && !playerMovement.isFacingRight)
        {
            // Spawn sheep behind the player when facing left and hitting a wall
            float xOffset = distanceBehindPlayer;
            spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
        }
        else
        {
            // Spawn sheep in front of player when not hitting any walls
            float xOffset = (transform.localScale.x > 0) ? distanceBehindPlayer : -distanceBehindPlayer;
            spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
        }

        sheepSpawned[index] = true;
        spawnedSheep[index] = Instantiate(sheepPrefabs[prefabNum], spawnPosition, Quaternion.identity);
        distanceBehindPlayer += distanceOffset;
        whistle.Play();
    }

    void DespawnLogic(int index)
    {
        sheepSpawned[index] = false;
        Destroy(spawnedSheep[index]);
        distanceBehindPlayer -= distanceOffset;
        spawnedSheep[index] = null;
    }

    private void MoveArrowToSpot(Vector3 targetPosition)
    {
        targetPosition += new Vector3(-.5f, .25f, 0);
        float lerpSpeed = 15.0f;

        // Use Lerp in the Update method to gradually move the arrow towards the target position
        arrow.transform.position = Vector3.Lerp(arrow.transform.position, targetPosition, Time.deltaTime * lerpSpeed);
    }

    public void DespawnAllSheep()
    {
        for (int i = 0; i < spawnedSheep.Length; i++)
        {
            if (sheepSpawned[i] && spawnedSheep[i] != null)
            {
                DespawnLogic(i);
            }
        }
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