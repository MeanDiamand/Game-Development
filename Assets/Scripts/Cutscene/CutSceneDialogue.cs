using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class CutSceneDialogue : MonoBehaviour
{
    public GameObject window;
    public GameObject nextButton;
    public GameObject skipButton;
    public GameObject  next2Button;

    public TMP_Text dialogueText;
    public List<string> dialogues;
    public List<Sprite> dialogueImages; 
    public Image dialogueImage; 
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
        if (i < dialogueImages.Count)
        {
            dialogueImage.sprite = dialogueImages[i]; 
        }
        StartCoroutine(Writing());
    }

    public void EndDialogue()
    {
        started = false;
        waitForNext = false;
        StopAllCoroutines();
    }

    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);
        string currentDialogue = dialogues[index];
        dialogueText.text += currentDialogue[charIndex];
        charIndex++;

        if (charIndex < currentDialogue.Length)
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
        if (!started) return;

        if (waitForNext && CutSceneEvent.nextcs)
        {
            waitForNext = false;
            CutSceneEvent.nextcs = false;
            index++;

            if (index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                nextButton.SetActive(false);
                skipButton.SetActive(false);
                next2Button.SetActive(true);
                EndDialogue();
            }
        }
    }
}
