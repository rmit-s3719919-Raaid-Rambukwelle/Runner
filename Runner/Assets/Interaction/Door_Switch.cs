using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Switch : Interactable
{
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

        if (locked && !PlayerController.instance.SearchInventory(requiredObj))
        {
            Debug.Log(lockedMessage);
            return;
        }
        open = !open;
        //animator.SetBool("Open", open);
        DoorObj.gameObject.SetActive(false);

        if (!reactivateOnUse) active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator = DoorObj.GetComponent<Animator>();
        
    }

}
