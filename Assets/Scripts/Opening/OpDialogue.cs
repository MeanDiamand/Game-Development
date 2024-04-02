using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]

public class OpDialogue : MonoBehaviour
{
    public GameObject window;
    public GameObject nextButton;
    public GameObject skipButton;
    public GameObject startButton;
    public GameObject title;

    public TMP_Text dialogueText;
    public List<string> dialogues;
    public float writingSpeed;
    private int index;
    private int charIndex;
    public Boolean started;
    private bool waitForNext;

    public void StartDialogue()
    {
        if (started) return;

        started = true;
        window.SetActive(true);
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
        index = i;
        charIndex = 0;
        dialogueText.text = string.Empty;
        StartCoroutine(Writing());
    }

    public void EndDialogue()
    {
        started = false;
        waitForNext = false;
        StopAllCoroutines();
        window.SetActive(false);
    }

    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);
        string currentDialogue = dialogues[index];
        dialogueText.text += currentDialogue[charIndex];
        charIndex++;

        if(charIndex < currentDialogue.Length)
        {
            yield return new WaitForSeconds(writingSpeed);
            StartCoroutine(Writing());
        }
        else
        {
            waitForNext = true;
        }
    }

    private void Update()
    {
        if(!started) return;

        if(waitForNext && OpeningEvents.next)
        {
            waitForNext = false;
            OpeningEvents.next = false;
            index++;

            if(index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                nextButton.SetActive(false);
                skipButton.SetActive(false);
                title.SetActive(true);
                startButton.SetActive(true);
                EndDialogue();
            }
        }
    }
}
