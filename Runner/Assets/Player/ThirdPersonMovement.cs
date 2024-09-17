using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public bool limitVelocity = true;

    [Header("Dashing")]
    public float dashForce;
    public float dashLimit;
    public float dashCooldown;
    [SerializeField] float dashCount = 3;
    [SerializeField] bool readyToDash = true;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMulti;
    bool readyToJump = true;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundDrag;
    bool grounded;


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
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        GetInput();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;
    }

    private void FixedUpdate()
    {
        /*
        if (PlayerManager.current.DialogueUI.IsOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }    
        */
        MoveCharacter();
    }

    void GetInput()
    {
        moveDir = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(PlayerManager.current.jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(PlayerManager.current.dashKey) && dashCount > 0 && readyToDash)
        {
            readyToDash = false;
            Dash();
            Invoke(nameof(ResetDash), 0.3f);
        }
    }


    void MoveCharacter()
    {
        if (grounded)
            rb.AddForce(moveDir.normalized * PlayerManager.current.moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDir.normalized * PlayerManager.current.moveSpeed * 10f * airMulti, ForceMode.Force);


        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > PlayerManager.current.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * PlayerManager.current.moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce((transform.up + moveDir.normalized) * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    void Dash()
    {
        if (moveDir.magnitude > 0)
            rb.AddForce((moveDir + transform.up * 0.05f) * dashForce, ForceMode.Impulse);
        else
            rb.AddForce((orientation.forward + transform.up * 0.05f) * dashForce, ForceMode.Impulse);
        dashCount--;
    }

    void ResetDash()
    {
        readyToDash = true;
    }

}
