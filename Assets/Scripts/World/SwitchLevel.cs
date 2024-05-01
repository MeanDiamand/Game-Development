using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    public int sceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("Switching scene to: " + sceneIndex);
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
    }
}
