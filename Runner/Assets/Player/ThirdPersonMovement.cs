using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public GameObject col;
    public bool limitVelocity = true;

    [Header("Movement")]
    public float moveSpeed;
    public float dashSpeed;
    public float crouchSpeed;
    public float slideSpeed;
    float speed;

    [Header("Crouching")]
    public float crouchYScale;
    float startYScale;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    float slideTimer;
    bool sliding;

    [Header("Dashing")]
    public float dashForce;
    public float dashLimit;
    public float dashCooldown;
    bool dashing;
    [SerializeField] float dashCount = 3;
    [SerializeField] bool readyToDash = true;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMulti;
    bool readyToJump = true;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundDrag;
    bool grounded;


    Vector3 moveDir;
    Rigidbody rb;

    public MoveState moveState;
    public enum MoveState
    {
        Moving,
        Dashing,
        Sliding,
        Crouching,
        Idle,
        Air
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        startYScale = col.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        GetInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;
    }

    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.velocity.magnitude > speed)
                rb.velocity = rb.velocity.normalized * speed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

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
        if (sliding)
            SlidingMovement();
    }
    void StateHandler()
    {
        if (sliding)
        {
            moveState = MoveState.Sliding;
            speed = slideSpeed;
        }
        else if (Input.GetKey(PlayerManager.current.crouchKey) && !sliding)
        {
            moveState = MoveState.Crouching;
            speed = crouchSpeed;
        }
        else if (dashing)
        {
            moveState = MoveState.Dashing;
            speed = dashSpeed;
        }
        else if (moveDir.magnitude > 0 && grounded)
        {
            moveState = MoveState.Moving;
            speed = moveSpeed;
        }
        else if (moveDir.magnitude == 0 && grounded)
        {
            moveState = MoveState.Idle;
        }
        else
        {
            moveState = MoveState.Air;
        }
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
            dashing = true;
            Dash();
            Invoke(nameof(ResetDash), 0.3f);
        }

        if (Input.GetKeyDown(PlayerManager.current.crouchKey))
        {
            col.transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            if (!sliding)
                StartSlide();
        }

        if (Input.GetKeyUp(PlayerManager.current.crouchKey))
        {
            col.transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            if (sliding)
                StopSlide();
        }
    }

    void StartSlide()
    {
        sliding = true;
        slideTimer = maxSlideTime;
    }

    void StopSlide()
    {
        sliding = false;
    }

    void SlidingMovement()
    {
        rb.AddForce(moveDir.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
            StopSlide();
    }

    void MoveCharacter()
    {
        if (OnSlope())
        {
            rb.AddForce(GetSlopeDirection() * speed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
            rb.AddForce(moveDir.normalized * speed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDir.normalized * speed * 10f * airMulti, ForceMode.Force);
        rb.useGravity = !OnSlope();


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
            rb.AddForce((moveDir + transform.up * 0.05f) * dashForce, ForceMode.Force);
        else
            rb.AddForce((orientation.forward + transform.up * 0.05f) * dashForce, ForceMode.Force);
        dashCount--;
    }

    void ResetDash()
    {
        dashing = false;
        readyToDash = true;
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

}
