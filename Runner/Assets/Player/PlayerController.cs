using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Dialogue system
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }


    public static PlayerController instance;

    [Header("Movement")]
    public bool canMove;
    public bool isIsometric;
    public float moveSpeed;
    public float turnSpeed;

    [Header("Interaction")]
    public float interactRange;
    public float grappleRange;
    public List<Interactable> interactables;

    [Header("Keybinds")]
    public KeyCode interactKey;
    Interactable closestInteractable;
    public Interactable currentInteractable;

    [Header("Inventory")]
    public float currentInventoryWeight = 0;
    public float maxInventoryWeight;

    public List<Item> items;


    private void Start()
    {
        instance = this;
        items = new List<Item>();
        interactables = new List<Interactable>();
    }


    private void Update()
    {
        //Stops player from moving if dialogue is open
        if (dialogueUI.IsOpen) return;

        //Dialogue system
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(playerController: this);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentInteractable == null) return;

            if (Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactRange)
                currentInteractable.Interact();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (currentInteractable == null) return;

            if (Vector3.Distance(transform.position, currentInteractable.transform.position) <= grappleRange)
                currentInteractable.Interact();
        }
    }

    public void DropItem(float itemSlot)
    {

    }

    public bool SearchInventory(string input)
    {
        foreach (Item item in items)
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

}
