using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawning : MonoBehaviour
{
    [SerializeField] private GameObject sheep;
    [SerializeField] private float distanceBehindPlayer;

    void Update()
    {
        InputHandler();
    }

    private void InputHandler() 
    {
        if(Input.GetKeyDown(KeyCode.Q) && transform.localScale.x > 0)
        {
            Instantiate(sheep, new Vector3(transform.position.x - distanceBehindPlayer, transform.position.y, 0), Quaternion.identity);
        }
        else if(Input.GetKeyDown(KeyCode.Q) && transform.localScale.x < 0)
        {
            Instantiate(sheep, new Vector3(transform.position.x + distanceBehindPlayer, transform.position.y, 0), Quaternion.identity);
        }
    }
}
