using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour 
{
    [SerializeField]
    private PlayerController player;
    private void Start()
    {
        player.LoadPlayer();
    }

}
