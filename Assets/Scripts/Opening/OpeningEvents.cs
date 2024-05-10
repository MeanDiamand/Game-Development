using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningEvents : MonoBehaviour
{
    public OpDialogue opDialogueScript;
    public static Boolean next = false;

    private void Awake()
    {
        opDialogueScript.StartDialogue();
    }

    public void startgame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void nextDialogue()
    {
        next = true;
    }
}
