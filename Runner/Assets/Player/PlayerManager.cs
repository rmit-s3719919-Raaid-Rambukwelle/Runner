using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;


    [Header("Keybinds")]
    public KeyCode interactKey;
    public KeyCode jumpKey;
    public KeyCode dashKey;


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

    [Header("UI")]
    public TextMeshProUGUI interactText;
    bool updateUI = true;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Stops player from moving if dialogue is open
        if (dialogueUI.IsOpen) return;



        //Update UI
        if (updateUI)
        {
            if (Interactable != null)
            {
                interactText.text = "E to talk";
            }
            else if (currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactRange && currentInteractable.showText && currentInteractable.interactable && !currentInteractable.canGrapple)
            {
                interactText.text = currentInteractable.textToShow + " " + currentInteractable.name;
            }
            else if (currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.transform.position) <= grappleRange && currentInteractable.showText && currentInteractable.interactable && currentInteractable.canGrapple)
            {
                interactText.text = currentInteractable.textToShow + " " + currentInteractable.name;
            }
            else
                interactText.text = "";
        }

        //Interactions
        if (Input.GetKeyDown(interactKey))
        {
            if (Interactable != null)
            {
                Interactable.Interact(playerManager: this);
            }
            else if (currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactRange && currentInteractable.interactable)
            {
                currentInteractable.Interact();
            }
            StartCoroutine(HoldUI());
        }

        if (Input.GetMouseButton(1) && currentInteractable != null && Vector3.Distance(transform.position, currentInteractable.transform.position) <= grappleRange && currentInteractable.interactable)
        {
            currentInteractable.Interact();
        }
        */
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
