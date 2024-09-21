using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Switch : Interactable
{
    [Header("Switch variables")]
    public GameObject DoorObj;
    public bool locked = false;
    public string lockedMessage;
    public string requiredObj;
    public bool reactivateOnUse;
    Animator animator;

    bool open = false;
    bool active = true;
    

    public override void Interact()
    {
        if (!active) return;

        if (locked && !PlayerManager.current.SearchInventory(requiredObj))
        {
            PlayerManager.current.interactText.text = lockedMessage;
            return;
        }
        open = !open;
        animator.SetBool("Open", open);
        
        if (!reactivateOnUse) active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = DoorObj.GetComponent<Animator>();    
    }

}
