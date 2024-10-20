using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePos : MonoBehaviour
{
    public LineRenderer lr;

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
    }
}
