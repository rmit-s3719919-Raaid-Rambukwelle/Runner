using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    public float amp;
    public float freq;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, amp * Mathf.Sin(Time.time * freq), transform.localPosition.z); 
    }
}
