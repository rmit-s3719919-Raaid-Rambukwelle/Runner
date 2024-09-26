using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;


    [Header("Keybinds")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Interactables")]
    public float interactRange;
    public float grappleRange;
    public Interactable currentInteractable;

    [Header("Inventory")]
    public Inventory inventory;

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    [SerializeField] private NPCAreaTrigger[] npcAreaTriggers;

    [Header("UI")]
    public TextMeshProUGUI interactText;
    bool updateUI = true;

    void Start()
    {
        current = this;
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {        
        //Stops player from moving if dialogue is open
        if (dialogueUI.IsOpen) return;

        //Update UI
        if (updateUI)
        {
            bool canTalk = false;

            foreach (var trigger in npcAreaTriggers) 
            {
                if (Interactable != null && trigger.isNPCinTriggerZone()) 
                {
                    canTalk = true;
                    break;
                }
            }

            if (canTalk)
            {
                interactText.text = "E to talk";
            }
            else
                {
                    interactText.text = ""; 
                }
        }

        //Interactions
        if (Input.GetKeyDown(interactKey))
        {

            foreach (var trigger in npcAreaTriggers) 
            {
                if (Interactable != null && trigger.isNPCinTriggerZone()) 
                {
                    Interactable.Interact(playerManager: this);
                    break;
                }
            }

        }

        if (Input.GetMouseButton(1) && currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.transform.position) <= grappleRange && currentInteractable.interactable)
        {
            currentInteractable.Interact();
        }
        StartCoroutine(HoldUI());
    }

    public bool SearchInventory(string input)
    {
        foreach (Item item in inventory.items)
        {
            if (item.itemID == input)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (currentInteractable != null)
        {
            if (Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactRange)
                Gizmos.color = Color.green;
            else if (Vector3.Distance(transform.position, currentInteractable.transform.position) <= grappleRange)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, currentInteractable.transform.position);
        }
    }
    
    IEnumerator HoldUI()
    {
        updateUI = false;
        yield return new WaitForSeconds(2f);
        updateUI = true;
    }    
}
