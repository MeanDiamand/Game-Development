using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = PlayerController.transform;
    }

}
