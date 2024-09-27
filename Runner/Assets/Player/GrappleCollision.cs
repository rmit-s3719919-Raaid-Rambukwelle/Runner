using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCollision : MonoBehaviour
{
    public PlayerMovement pm;

    private void OnCollisionEnter(Collision collision)
    {
        pm.GrappleCollide();
    }
}
