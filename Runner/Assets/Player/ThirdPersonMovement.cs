using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Debug")]
    public float DebugSpeed;
    public float DebugDesiredMoveSpeed;
    public MovementState state;

    [Header("Speed Values")]
    public float walkSpeed;
    public float slideSpeed;
    public float slopeSlideSpeed;
    public float crouchSpeed;
    float moveSpeed;

    [Header("Movement Multipliers")]
    public float lerpMultiplier;
    public float airLerpMultiplier;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMulti;

    [Header("Scale Values")]
    public float crouchYScale;
    float startYScale;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    public float slideCooldown;
    [SerializeField] float slideTimer;

    [Header("Ground Check")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;

    [Header("Slope Handler")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [Header("Conditions")]
    public bool grounded;
    public bool readyToJump;
    public bool sliding;
    public bool canSlide;

    [Header("References")]
    public Transform orientation;
    public GameObject col;
    public Animator ani;

    [Header("UI")]
    [SerializeField] private GameObject[] ui;

    float hInput;
    float vInput;
    Vector3 moveDir;
    Rigidbody rb;


    public enum MovementState
    {
        walking,
        crouching,
        sliding,
        air,
        idle
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        canSlide = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        DebugSpeed = rb.velocity.magnitude;
        DebugDesiredMoveSpeed = desiredMoveSpeed;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        GetInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        ani.SetFloat("Velocity", moveDir.magnitude);
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

            MovePlayer();
            if (sliding)
                SlidingMovement();
        }

    }

    void GetInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(PlayerManager.current.jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(PlayerManager.current.crouchKey))
        {
            col.transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            if (moveDir.magnitude > 0f && !sliding && grounded && canSlide)
            {
                StartSlide();
            }
        }

        if (Input.GetKeyUp(PlayerManager.current.crouchKey))
        {
            col.transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            if (sliding)
            {
                StopSlide();
            }
        }

    }

    void StateHandler()
    {
        if (sliding) // Sliding
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slopeSlideSpeed;
            else
                desiredMoveSpeed = slideSpeed;

        }
        else if (Input.GetKey(PlayerManager.current.crouchKey) && !sliding && grounded) // Crouching
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded && !sliding && moveDir.magnitude > 0) // Walking
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else if (!grounded) // Jumping
        {
            state = MovementState.air;
        }
        else // Idle
        {
            state = MovementState.idle;

        }


        if (desiredMoveSpeed < moveSpeed && moveSpeed != 0f && Mathf.Abs(lastDesiredMoveSpeed - desiredMoveSpeed) > 10f)
        {
            Debug.Log(Mathf.Abs(lastDesiredMoveSpeed - desiredMoveSpeed));
            StopAllCoroutines();
            StartCoroutine(SmoothLerpMovement());
        }
        else
            moveSpeed = desiredMoveSpeed;

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }


    void MovePlayer()
    {
        moveDir = orientation.forward * vInput + orientation.right * hInput;
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDir(moveDir) * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMulti, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.8f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDir(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, slopeHit.normal).normalized;
    }

    IEnumerator SmoothLerpMovement()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;
        float multi;

        while (time < difference)
        {
            if (!grounded)
                multi = airLerpMultiplier;
            else
                multi = lerpMultiplier;

            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, (time / difference) * multi);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;
            yield return null;
        }
        moveSpeed = desiredMoveSpeed;

    }

    void StartSlide()
    {
        sliding = true;
        canSlide = false;
        slideTimer = maxSlideTime;
    }

    void StopSlide()
    {
        sliding = false;
        Invoke("StartSlideCooldown", slideCooldown);
    }

    void StartSlideCooldown()
    {
        canSlide = true;
    }

    void SlidingMovement()
    {
        Vector3 inputDir = orientation.forward * vInput + orientation.right * hInput;

        if (!OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
            if (!grounded)
                slideTimer = maxSlideTime;
        }
        else
        {
            rb.AddForce(GetSlopeMoveDir(inputDir) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            StopSlide();

        }
    }

}
