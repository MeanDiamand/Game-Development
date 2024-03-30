using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningEvents : MonoBehaviour
{
    public Dialog dialogScript;

    private void Awake()
    {
        dialogScript.StartDialog();
    }

    public void startgame()
    {
        SceneManager.LoadScene("Menu");
    }
}
