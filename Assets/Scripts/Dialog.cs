using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]

public class Dialog : MonoBehaviour
{
    //Fields
    public GameObject window;

    //Text component
    public TMP_Text dialogueText;

    //Dialog list
    public List<string> dialogues;

    //writing speed
    public float writingSpeed;

    //index on dialog
    private int index;

    //character index
    private int charIndex;

    //started boolean
    private Boolean started;

    //wait for next boolean
    private bool waitForNext;

    public void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    //Start dialog
    public void StartDialog()
    {
        if (started) return;

        //boolean to indicate that we have started
        started = true;
        //show the dialogbox
        ToggleWindow(true);
        //start with first dialogue
        GetDialog(0);
        
    }

    private void GetDialog(int i)
    {
        //start index at 0
        index = i;
        //reset the character index
        charIndex = 0;
        //clear the dialogue component text
        dialogueText.text = string.Empty;
        //start writing
        StartCoroutine(Writing());
    }

    //End dialog
    public void EndDialog()
    {
        //started is disable
        started = false;
        //disable wait for next
        waitForNext = false;
        //stop all Ienumerator
        StopAllCoroutines();
        //hide the dialogbox
        ToggleWindow(false);
    }

    //writing
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);
        string currentDialogue = dialogues[index];
        //Write the character
        dialogueText.text += currentDialogue[charIndex];
        //increase the character index
        charIndex++;
        //make sure to reach the end of the sentence
        if(charIndex < currentDialogue.Length) 
        {
            //wait for second
            yield return new WaitForSeconds(writingSpeed);
            //restartthe same process
            StartCoroutine(Writing());
        }
        else
        {
            //end the sentence and wait for the next one
            waitForNext = true;
        }
        
    }

    private void Update()
    {
        if (!started) return;

        if(waitForNext && Input.GetKeyDown(KeyCode.E)) 
        {
            waitForNext = false;
            index++;
            //check if we are in the scope of dialogues list
            if(index < dialogues.Count)
            {
                //if so fetch the next dialogue
                GetDialog(index);
            }
            else
            { 
                //if not, end the dialogue process  
                EndDialog();
            }
        }
    }
}
