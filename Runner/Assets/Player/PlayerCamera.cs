using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineFreeLook thirdPersonCam;
    public Transform orientation;
    public Transform player;

    [Header("First Person")]
    public Transform cameraHolder;

    [Header("Third Person")]
    public Transform gfx;
    public Rigidbody rb;
    public float rotationSpeed;

    float xRot, yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (PlayerManager.current.thirdPerson)
        {
            firstPersonCam.Priority = 10;
            thirdPersonCam.Priority = 20;
            ThirdPersonCamera();
        }
        else
        {
            firstPersonCam.Priority = 20;
            thirdPersonCam.Priority = 10;
            FirstPersonCamera();
        }


    }

    void FirstPersonCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * PlayerManager.current.sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * PlayerManager.current.sensY;

        yRot += mouseX;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -60f, 60f);

        cameraHolder.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);

    }

    void ThirdPersonCamera()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDir, Vector3.up);
            gfx.rotation = Quaternion.Lerp(gfx.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void DoTilt(float endValue)
    {
        StopCoroutine(nameof(LerpTilt));
        StartCoroutine(LerpTilt(endValue));
    }

    IEnumerator LerpTilt(float endValue)
    {
        float t = 0;
        float d = Mathf.Abs(firstPersonCam.m_Lens.Dutch - endValue);
        float s = firstPersonCam.m_Lens.Dutch;

        while (t < d)
        {
            firstPersonCam.m_Lens.Dutch = Mathf.Lerp(s, endValue, t / d);
            t += Time.deltaTime * 40f;
            yield return null;
        }

        firstPersonCam.m_Lens.Dutch = endValue;
    }

}
