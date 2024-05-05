using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialog dialogScript;
    private bool playerDetected;

    [SerializeField]
    private UIController uiController;

    private void Start()
    {
        Canvas canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();

        if (canvas != null)
        {
            Dialog dialogComponent = canvas.GetComponentInChildren<Dialog>();

            if (dialogComponent != null)
                dialogScript = dialogComponent;
            else
                Debug.LogError("No object with Dialog component found under Canvas.");
        }
        else
            Debug.LogError("No Canvas found in the scene.");

        uiController = GameObject.Find("Player").GetComponent<UIController>();
    }

    //detect trigger with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogScript.EndDialog();
            uiController.Show();
        }
    }
    //While detected if we interact start the dialogue
    private void Update()
    {
        if(playerDetected && Input.GetKeyDown(KeyCode.F)) 
        {
            dialogScript.StartDialog();
            uiController.Hide();
        }
    }
}
