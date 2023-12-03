using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerText;
    private SheepSpawning sheepSpawning;
    private string sheep1Active = "Sheep 1";
    private string sheep2Active = "Sheep 2";
    private string sheep3Active = "Sheep 3";
    private string playerIsActive = "Player";
    private int currentControlledPlayer;

    void Start()
    {
        sheepSpawning = FindObjectOfType<SheepSpawning>();
    }

    void Update()
    {
        if (sheepSpawning.playerActive)
        {
            playerText.text = playerIsActive;
        }
        else
        {
            // Check if each sheep is not null before accessing its components
            if (sheepSpawning.spawnedSheep[0] != null && sheepSpawning.spawnedSheep[0].GetComponent<SheepMovement>().enabled)
            {
                playerText.text = sheep1Active;
            }
            else if (sheepSpawning.spawnedSheep[1] != null && sheepSpawning.spawnedSheep[1].GetComponent<SheepMovement>().enabled)
            {
                playerText.text = sheep2Active;
            }
            else if (sheepSpawning.spawnedSheep[2] != null && sheepSpawning.spawnedSheep[2].GetComponent<SheepMovement>().enabled)
            {
                playerText.text = sheep3Active;
            }
        }
    }
}
