using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public bool useThirdPerson;

    [Header("References")]
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineFreeLook thirdPersonCam;

    [Header("Shake Values")]
    public float shakeDistance;
    public float shakeSpeed;

    public void EnableShake()
    {
        if (useThirdPerson)
        {

        }
        else
        {

        }
    }

    public void DisableShake()
    {
        if (useThirdPerson)
        {

        }
        else
        {

        }
    }
}
