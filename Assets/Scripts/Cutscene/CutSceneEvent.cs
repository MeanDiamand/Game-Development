using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneEvent : MonoBehaviour
{
    public CutSceneDialogue DialogueScript;
    public static Boolean nextcs = false;
    private static string scene = "";

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

    public static void retrieveSceneId(int id)
    {
        if(id == 1)
        {
            scene = "SampleScene";
        }
        else if (id == 2)
        {
            scene = "";
        }
    }
}
