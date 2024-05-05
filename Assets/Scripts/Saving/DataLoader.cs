using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private SwitchLevel switchLevel;

    void Start()
    {
        try
        {
            switchLevel.newSceneId = PlayerEvents.dataService.LoadData<int>("/scene_id");
        } catch { }

        player.LoadPlayer();
    }
}
