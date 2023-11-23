using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueMusic : MonoBehaviour
{
    void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Music");

        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 2)
        {
            Debug.Log("called");
            Destroy(this.gameObject);
        }
    }
}
