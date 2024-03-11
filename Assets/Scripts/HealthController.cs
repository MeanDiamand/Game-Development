using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int health = 5;
    public bool isHitable = true;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


    // Update is called once per frame
    void Update()
    {
        // Assigning the emptyHeart into the array
        foreach(Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        // Adding the fullHeart into the array
        for(int i = 0; i < health; i++) 
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
