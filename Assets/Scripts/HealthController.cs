using Assets.Interfaces;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [SerializeField]
    private DamagableCharacter character;

    // Update is called once per frame
    void Update()
    {
        // Assigning the emptyHeart into the array
        foreach(Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        // Adding the fullHeart into the array
        for(int i = 0; i < character.Health; i++) 
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
