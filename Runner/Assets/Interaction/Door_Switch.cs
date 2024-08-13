using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Switch : Interactable
{
    public GameObject DoorObj;
    Animator animator;
    bool open = false;

    public override void Interact()
    {
        open = !open;
        animator.SetBool("Open", open);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = DoorObj.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
