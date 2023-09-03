using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacters : MonoBehaviour
{
    [SerializeField] private GameObject character1;
    [SerializeField] private GameObject character2;
    private GameObject activeCharacter;

    void Start()
    {
        // Initialize the active character
        activeCharacter = character1;
        character2.SetActive(false);
    }

    void Update()
    {
        // Swap characters with Q and E keys
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCharacter(character1, character2);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCharacter(character2, character1);
        }
    }

    void SwitchCharacter(GameObject newCharacter, GameObject oldCharacter)
    {
        newCharacter.SetActive(true);
        oldCharacter.SetActive(false);
        activeCharacter = newCharacter;

        // Update the camera's target to the active character
        Camera.main.GetComponent<CameraFollow>().target = activeCharacter.transform;
    }
}
