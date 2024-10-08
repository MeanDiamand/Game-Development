using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField]
    public int newSceneId;

    [SerializeField]
    private bool autoSwitch;

    [SerializeField]
    private float switchDelay;

    private void Start()
    {
        if (autoSwitch)
        {
            Debug.Log($"Scene started. Waiting {switchDelay} seconds until new scene starts.");
            StartCoroutine(DelayedSceneLoad());
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LoadScene();
            PlayerEvents.GetInstance().Save(false);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(newSceneId, LoadSceneMode.Single);
    }

    private IEnumerator DelayedSceneLoad()
    {
        // Wait for the specified delay before loading the scene
        yield return new WaitForSeconds(switchDelay);

        LoadScene();
    }
}
