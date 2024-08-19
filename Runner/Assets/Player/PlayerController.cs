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


    private void Start()
    {
        instance = this;
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

    void GetClosestInteractable()
    {
        if (interactables.Count <= 0) return;

        closestInteractable = interactables[0];
        foreach(var interactable in interactables)
        {
            if (Vector3.Distance(transform.position, interactable.transform.position) < Vector3.Distance(transform.position, closestInteractable.transform.position))
            {
                closestInteractable = interactable;
            }
        } 
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
