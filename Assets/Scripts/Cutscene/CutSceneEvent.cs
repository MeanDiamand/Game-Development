using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneEvent : MonoBehaviour
{
    public CutSceneDialogue DialogueScript;
    public static Boolean nextcs = false;
    [SerializeField]
    private string nextScene;

    private void Awake()
    {
        DialogueScript.StartDialogue();
    }

    public void skipDialogue()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void nextDialogue()
    {
        nextcs = true;
    }
}
