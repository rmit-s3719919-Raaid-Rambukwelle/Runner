using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Settings")]
    public float followSpeed;
    public float transformSpeed;

    [Header("References")]
    public Transform isoTransform;
    public Transform tpTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.isIsometric)
        {
            transform.position = Vector3.Slerp(transform.position, isoTransform.position, Time.deltaTime * followSpeed);
            transform.rotation = isoTransform.rotation;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, tpTransform.position, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, tpTransform.rotation, Time.deltaTime * transformSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { ChangePerspective(); }
    }

    public void ChangePerspective()
    {
        PlayerController.instance.isIsometric = !PlayerController.instance.isIsometric;
        //Camera.main.orthographic = PlayerController.instance.isIsometric;
    }

    IEnumerator MoveCamera()
    {
        yield return null;
    }
}
