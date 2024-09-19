using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Camera")]
    public Transform holder;
    float xRot;
    float yRot;
    [SerializeField] private GameObject[] ui;

    Vector3 moveDir;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        //checks to see if any ui element is active
        bool isAnyUIActive = false;
        foreach (var uiElement in ui) 
        {
            if (uiElement.activeSelf) 
            {
                isAnyUIActive = true;
                break;
            }
        }

        //unlocks cursor if ui element is active
        if (PlayerManager.current.DialogueUI.IsOpen || isAnyUIActive)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MoveCharacter();
            CameraMovement();
        }    
    }

    void GetInput()
    {
        moveDir = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * PlayerManager.current.sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * PlayerManager.current.sensY;
        yRot += mouseX;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, yRot, 0);
        holder.rotation = Quaternion.Euler(xRot, yRot, 0);
    }

    void MoveCharacter()
    {
        if (moveDir.magnitude > 0)
        {
            rb.AddForce(moveDir.normalized * PlayerManager.current.moveSpeed * 10f, ForceMode.Force);
        }

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > PlayerManager.current.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * PlayerManager.current.moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

}
