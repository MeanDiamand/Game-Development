using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneEvent : MonoBehaviour
{
    public CutSceneDialogue DialogueScript;
    public static Boolean nextcs = false;
    [SerializeField]
    private string scene;

    private void Awake()
    {
        DialogueScript.StartDialogue();
    }

    public void skipDialogue()
    {
        SceneManager.LoadScene(scene);
    }

    public void nextDialogue()
    {
        nextcs = true;
    }
}
