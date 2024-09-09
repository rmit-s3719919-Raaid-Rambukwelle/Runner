using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference")]

    public Transform orientation;
    [SerializeField] Vector3 moveDir;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets vector from player inputs
        GetInput();

    }

    private void FixedUpdate()
    {
        if (PlayerController.instance.isIsometric && PlayerController.instance.canMove) isoMovement();

    }

    void GetInput()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    }


    void isoMovement()
    {
        // Rotation
        if (moveDir.magnitude <= 0) return;
        var iso = IsometricConversion(moveDir);
        var rot = Quaternion.LookRotation(iso);
        orientation.rotation = Quaternion.RotateTowards(orientation.rotation, rot, Time.deltaTime * PlayerController.instance.turnSpeed);

        // Movement
        if (moveDir.magnitude <= 0) return;
        rb.MovePosition(transform.position + iso * PlayerController.instance.moveSpeed * Time.deltaTime);

    }

    Vector3 IsometricConversion(Vector3 dir)
    {
        // Use matrix to rotate vector by 45 degrees
        Quaternion rotation = Quaternion.Euler(0, 45f, 0);
        Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
        return matrix.MultiplyPoint3x4(dir);
    }
}
