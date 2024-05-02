using System.Collections;
using UnityEngine;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField]
    private SceneData newScene;

    [SerializeField]
    private bool autoSwitch;

    [SerializeField]
    private float switchDelay;

    [SerializeField]
    private Transform canvas;

    private void Start()
    {
        if (autoSwitch)
        {
            Debug.Log($"Scene started. Waiting {switchDelay} seconds until new scene starts.");
            StartCoroutine(DelayedSceneLoad());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            newScene.LoadScene();
        }
    }

    private IEnumerator DelayedSceneLoad()
    {
        // Wait for the specified delay before loading the scene
        yield return new WaitForSeconds(switchDelay);

        // Load the new scene
        newScene.LoadScene();

        canvas.gameObject.SetActive(true);
    }
}
