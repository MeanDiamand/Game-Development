using UnityEngine;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField]
    private SceneData newScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            newScene.LoadScene();
        }
    }
}
