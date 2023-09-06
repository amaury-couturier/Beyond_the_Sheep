using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacter : SheepSpawning
{
    [SerializeField] private Transform character;
    [SerializeField] private List<Transform> possibleCharacters;
    [SerializeField] private int whichCharacter;
    [SerializeField] private CameraFollow cam;

    void Start()
    {
        if(character == null && possibleCharacters.Count >= 1)
        {
            character = possibleCharacters[0];
        }

        Swap();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && sheep1Spawned)
        {
            if(whichCharacter == 0) 
            {
                whichCharacter = possibleCharacters.Count - 1;
            }
            else
            {
                whichCharacter -= 1;
            }

            Swap();
        }
        else if(Input.GetKeyDown(KeyCode.E) && sheep1Spawned)
        {
            if(whichCharacter == possibleCharacters.Count - 1) 
            {
                whichCharacter = 0;
            }
            else
            {
                whichCharacter += 1;
            }

            Swap();
        }
    }

    void Swap()
    {
        character = possibleCharacters[whichCharacter];
        character.GetComponent<PlayerMovement>().enabled = true;

        for(int i = 0; i < possibleCharacters.Count; i++)
        {
            if(possibleCharacters[i] != character)
            {
                possibleCharacters[i].GetComponent<PlayerMovement>().enabled = false;
            }
        }
        
        if (cam != null)
        {
            cam.target = character;
        }
    }
}
