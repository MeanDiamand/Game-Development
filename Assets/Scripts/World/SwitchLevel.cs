using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;
    [SerializeField]
    private Vector2 newCoordinates;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("Switching scene to: " + sceneIndex);
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
            print("Teleporting: " + newCoordinates);
            PlayerEvents.GetInstance().Teleport(newCoordinates);
        }
    }
}
