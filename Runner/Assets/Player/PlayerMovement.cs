using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debug")]
    public float DebugSpeed;
    public float DebugDesiredMoveSpeed;
    public bool allowMovementDebug;
    public GameObject testPrefab;
    public MovementState state;

    [Header("Speed Values")]
    public float walkSpeed;
    public float jogSpeed;
    public float runnerSpeed;
    public float slideSpeed;
    public float slopeSlideSpeed;
    public float crouchSpeed;
    public float wallRunSpeed;
    public float climbSpeed;
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
    public float airDrag;

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

    [Header("Climbing")]
    public LayerMask whatIsClimbingWall;
    public Transform feetPos;
    public float climbLiftSpeed;
    public float maxClimbTime;
    float climbTimer;
    bool climbing;
    public float climbDetectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    float wallLookAngle;
    RaycastHit frontWallHit;
    bool wallFront;

    [Header("Wall Running")]
    public bool useGravity;
    public float gravityCounterForce;
    public LayerMask whatIsWall;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallCheckDistance;
    public float minJumpHeight;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float exitWallTime;

    [Header("Grappling")]
    public LineRenderer lr;
    public Transform cameraPoint;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootY;
    public bool freeze;
    Vector3 grapplePoint;
    public float grappleCD;
    float grappleCDTimer;
    bool grappling;
    RaycastHit grappleHit;

    [Header("Grapple Animation")]
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    Spring spring;
    public AnimationCurve affectCurve;

    bool exitingWall;
    float exitWallTimer;
    float wallRunTimer;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallLeft;
    bool wallRight;
    bool wallrunning;

    [Header("Conditions")]
    public bool grounded;
    public bool readyToJump;
    public bool sliding;
    public bool canSlide;
    public bool activeGrapple;

    [Header("References")]
    public Transform orientation;
    public GameObject col;
    public Animator ani;
    public PlayerCamera cam;

    [Header("UI")]
    [SerializeField] private GameObject[] ui;

    float hInput;
    float vInput;
    Vector3 moveDir;
    Rigidbody rb;

    public enum MovementState
    {
        freeze,
        walking,
        crouching,
        sliding,
        wallRun,
        climbing,
        air,
        idle
    }

    private void Start()
    {
        spring = new Spring();
        spring.SetTarget(0);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        canSlide = true;
        startYScale = transform.localScale.y;
        if (allowMovementDebug)
            ani.SetTrigger("AllowMovement");
    }

    private void Update()
    {
        PlayerManager.current.moveSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        DebugSpeed = rb.velocity.magnitude;
        DebugDesiredMoveSpeed = desiredMoveSpeed;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (PlayerManager.current.canMove)
        {
            GetInput();
            SpeedControl();
            SideWallCheck();
            FrontWallCheck();
            WallRunHandler();
            ClimbingHandler();
            StateHandler();
        }

        if (grounded && !activeGrapple)
        {
            rb.drag = groundDrag;
            if (Mathf.Abs(cam.firstPersonCam.m_Lens.Dutch) > 0 && PlayerManager.current.running) cam.DoTilt(0f);
        }
        else
            rb.drag = airDrag;

        ani.SetFloat("Velocity", rb.velocity.magnitude);
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

        if (PlayerManager.current.DialogueUI.IsOpen || isAnyUIActive)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

            if (!PlayerManager.current.running)
            {
                // State 1: Scavenger
                MovePlayer();
            }
            else
            {
                // State 2: Runner
                MovePlayer();

                if (sliding)                
                    SlidingMovement();                

                if (wallrunning)                
                    WallRunningMovement();                

                if (climbing)
                    ClimbingMovement();
            }
        
    }

    void GetInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (PlayerManager.current.running)
        {
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

            if (Input.GetKeyDown(PlayerManager.current.grappleKey))
            {
                StartGrapple();
            }

            if (grappleCDTimer > 0) grappleCDTimer -= Time.deltaTime;
        }
    }

    void StateHandler()
    {
        // State 1: Scavenger (only walking)
        if (!PlayerManager.current.running) 
        {
            // walking or idle
            if (moveDir.magnitude > 0 && grounded) // Walking
            {
                state = MovementState.walking;
                if (Input.GetKey(PlayerManager.current.dashKey))
                    desiredMoveSpeed = jogSpeed;
                else
                    desiredMoveSpeed = walkSpeed;
            }
            else // idle
            {
                state = MovementState.idle;
                desiredMoveSpeed = 0f;
            }

            // disable advanced movement
            sliding = false;
            wallrunning = false;

            moveSpeed = desiredMoveSpeed;
        }
        else // State 2: Runner (full movement)
        {
            if (freeze)
            {
                state = MovementState.freeze;                
                rb.velocity *= 0.98f;
            }
            else if (climbing)
            {
                state = MovementState.climbing;
                desiredMoveSpeed = climbSpeed;
            }
            else if (wallrunning)
            {
                state = MovementState.wallRun;
                desiredMoveSpeed = wallRunSpeed;
            }
            else if (sliding) // sliding
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
            else if (grounded && moveDir.magnitude > 0) // walking
            {
                state = MovementState.walking;
                desiredMoveSpeed = runnerSpeed;
            }
            else if (!grounded) // jumping
            {
                state = MovementState.air;
                desiredMoveSpeed = walkSpeed;
            }
            else // idle
            {
                state = MovementState.idle;
                desiredMoveSpeed = 0f;
            }

            // Keep the movement speed consistent
            if (desiredMoveSpeed < moveSpeed && moveSpeed != 0f && Mathf.Abs(lastDesiredMoveSpeed - desiredMoveSpeed) > 10f)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothLerpMovement());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;
        }
    }

    void MovePlayer()
    {
        //if (activeGrapple) return;
        if (!PlayerManager.current.canMove) return;

        moveDir = orientation.forward * vInput + orientation.right * hInput;
        //Debug.Log("MovePlayer called. hInput: " + hInput + " vInput: " + vInput + " moveDir: " + moveDir);


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

        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        if (activeGrapple) return;

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
            if (slopeHit.transform.CompareTag("Player")) return false;

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

    void SideWallCheck()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void WallRunHandler()
    {
        if ((wallLeft || wallRight) && Input.GetAxisRaw("Vertical") > 0f && AboveGround() && !exitingWall)
        {
            if (!wallrunning)
                StartWallRun();
            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(PlayerManager.current.jumpKey))
                WallJump();
        }
        else if (exitingWall)
        {
            cam.DoTilt(0f);
            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;

            if (wallrunning)
                StopWallRun();
        }
        else
        {
            if (wallrunning)
            {
                cam.DoTilt(0f);
                StopWallRun();
            }
        }
    }

    void StartWallRun()
    {
        wallrunning = true;
        wallRunTimer = maxWallRunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);
    }

    void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(wallLeft && moveDir.x > 0) && !(wallRight && moveDir.x < 0))
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);

        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    void StopWallRun()
    {
        wallrunning = false;
    }

    void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    void JumpToPosition(Vector3 targetPos,  float t)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPos, t);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    Vector3 velocityToSet;

    void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }
    
    void ResetRestrictions()
    {
        activeGrapple = false;
    }

    public void GrappleCollide()
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        if (!grappling)
        {
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.update(Time.deltaTime);

        var up = Quaternion.LookRotation(grapplePoint - gunTip.position).normalized * Vector3.up;

        lr.SetPosition(0, gunTip.position);

        for (int i = 1; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTip.position, grapplePoint, delta) + offset);
        }
    }

    bool enableMovementOnNextTouch;

    void StartGrapple()
    {
        if (grappleCDTimer > 0) return;

        grappling = true;
        freeze = true;

        

        if (Physics.Raycast(cameraPoint.position, cameraPoint.forward, out grappleHit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = grappleHit.point;
            Invoke(nameof(GrappleMovement), grappleDelayTime);
            //Debug.Log("Hit Grapple Point: + " + grappleHit.collider.name);
        }
        else
        {
            grapplePoint = cameraPoint.position + cameraPoint.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
            //Instantiate(testPrefab, grapplePoint, Quaternion.identity);
            //Debug.Log("Missed Grapple Point: + " + grappleHit);
        }

        lr.enabled = true;
    }

    void GrappleMovement()
    {
        freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelYPos + overshootY;

        if (grapplePointRelYPos < 0) highestPointOnArc = overshootY;

        JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 0.5f);
    }

    void StopGrapple()
    {
        freeze = false;
        grappling = false;

        grappleCDTimer = grappleCD;

        lr.enabled = false;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float g = Physics.gravity.y;
        float dy = endPoint.y - startPoint.y;
        Vector3 dxz = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 vy = Vector3.up * MathF.Sqrt(-2 * g * trajectoryHeight);
        Vector3 vxz = dxz / (MathF.Sqrt(-2 * trajectoryHeight / g) + MathF.Sqrt(2 * (dy - trajectoryHeight) / g));

        return vy + vxz;
    }

    void FrontWallCheck()
    {
        wallFront = Physics.SphereCast(feetPos.position, sphereCastRadius, orientation.forward, out frontWallHit, climbDetectionLength, whatIsClimbingWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
        if (grounded) climbTimer = maxClimbTime;
    }

    void ClimbingHandler()
    {
        if (wallFront && Input.GetKey(KeyCode.W) && Input.GetKey(PlayerManager.current.jumpKey) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }
        else
        {
            if (climbing) StopClimbing();
        }    
    }

    void StartClimbing()
    {
        climbing = true;
    }

    void ClimbingMovement()
    {
        rb.velocity = new Vector3(0f, climbLiftSpeed, 0f);
    }

    void StopClimbing()
    {
        climbing = false;
    }


    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        transform.position = PlayerManager.current.currentRespawnPoint.position;
    }
}
