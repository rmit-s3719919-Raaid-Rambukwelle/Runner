using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement")]
    public bool isIsometric;
    public float moveSpeed;
    public float turnSpeed;

    [Header("Interaction")]
    public float interactRange;
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
        /*
        GetClosestInteractable();
        if (Input.GetKeyDown(interactKey))
        {
            if (interactables.Count <= 0)
            {
                Debug.Log("No Interactables in range");
                return;
            }
            
            closestInteractable.Interact();
        }
        */

        if (Input.GetMouseButtonDown(0))
        {
            if (currentInteractable == null) return;

            if (Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactRange)
                currentInteractable.Interact();
        }
    }

    public void DropItem(float itemSlot)
    {

    }

    private void OnDrawGizmos()
    {
        if (currentInteractable != null)
        {
            if (Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactRange)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, currentInteractable.transform.position);
        }
    }

}
