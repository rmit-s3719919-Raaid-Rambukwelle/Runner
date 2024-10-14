using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public GameObject crosshair;
    public CinemachineBrain brain;
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineFreeLook npcCam;
    public Transform orientation;
    public Transform player;
    public Transform npc;

    [Header("Noise Profiles")]
    public NoiseSettings headBobProfile;
    public NoiseSettings shakeProfile;

    [Header("Screen Shake")]
    public float amp;
    public float freq;

    [Header("First Person")]
    public Transform cameraHolder;

    [Header("Third Person")]
    public GameObject gfx;
    public Rigidbody rb;
    public float rotationSpeed;
    float xStartSense;
    float yStartSense;

    bool shaking;
    float xRot, yRot = 180f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        thirdPersonCam.m_XAxis.m_MaxSpeed = PlayerManager.current.canMove ? PlayerManager.current.tpSensX : 0f;
        thirdPersonCam.m_YAxis.m_MaxSpeed = PlayerManager.current.canMove ? PlayerManager.current.tpSensY : 0f;

        if (PlayerManager.current.canMove)
        {
            crosshair.SetActive(!PlayerManager.current.thirdPerson);
            if (PlayerManager.current.thirdPerson)
            {
                StartCoroutine(switchGfx(PlayerManager.current.thirdPerson, 0f));
                firstPersonCam.Priority = 10;
                thirdPersonCam.Priority = 40;
                ThirdPersonCamera();
            }
            else
            {
                StartCoroutine(switchGfx(PlayerManager.current.thirdPerson, 0.25f));
                firstPersonCam.Priority = 40;
                thirdPersonCam.Priority = 10;
                FirstPersonCamera();
                if (!shaking && new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude > 0)
                {
                    firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
                    firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
                    firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = headBobProfile;
                }
                else
                {
                    firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
                    firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
                }
            }
        }
    }

    void FirstPersonCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * PlayerManager.current.sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * PlayerManager.current.sensY;

        yRot += mouseX;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -85f, 85f);

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
            gfx.transform.rotation = Quaternion.Lerp(gfx.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
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

    IEnumerator switchGfx(bool setter, float delay)
    {
        yield return new WaitForSeconds(delay);
        gfx.SetActive(setter);
    }

    public void ActivateScreenShake()
    {
        shaking = true;
        firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freq;
        firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = amp;
        firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = shakeProfile;

        thirdPersonCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freq;
        thirdPersonCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amp;

        thirdPersonCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freq;
        thirdPersonCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amp;

        thirdPersonCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freq;
        thirdPersonCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amp;
    }

    public void DeactivateScreenShake()
    {
        shaking = false;
        firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        firstPersonCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

        thirdPersonCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        thirdPersonCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

        thirdPersonCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        thirdPersonCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

        thirdPersonCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        thirdPersonCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    public void AdjustNpcCam(int prio)
    {
        npcCam.Priority = prio;
    }

    public void AdjustBlendSpeed(float speed)
    {
        brain.m_DefaultBlend.m_Time = speed;
    }

}
