using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    [Header("Runner")]
    public float maxTime;
    public float timer;

    [Header("Settings")]
    public bool running;
    public bool canMove;
    public bool thirdPerson;
    public float moveSpeed;

    [Header("Sensitivity")]
    public float sensX;
    public float sensY;
    public float tpSensX;
    public float tpSensY;


    [Header("Keybinds")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode grappleKey = KeyCode.Mouse1;
    public KeyCode transitionKey = KeyCode.Slash;
    public KeyCode inventoryKey = KeyCode.I;


    [Header("Interactables")]
    public float interactRange;
    public float grappleRange;
    public Interactable currentInteractable;

    [Header("Respawning")]
    public Transform currentRespawnPoint;

    [Header("Inventory")]
    public GameObject inventoryCanvas;
    public GameObject itemPrefab;
    public Inventory inventory;
    public Animator inventoryAni;
    bool inventoryOpen = false;

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    [SerializeField] private NPCAreaTrigger[] npcAreaTriggers;

    [Header("UI")]
    public TextMeshProUGUI interactText;
    public Animator timerUI;
    public TextMeshProUGUI timerText;
    bool updateUI = true;

    [Header("Runner Audio")]
    public Animator audioAni;


    PlayerMovement pm;

    void Start()
    {
        current = this;
        pm = GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        timerText.text = timer.ToString();

        //Stops player from moving if dialogue is open
        if (dialogueUI.IsOpen) return;

        if (Input.GetKeyDown(transitionKey))
        {
            thirdPerson = !thirdPerson;
            running = !running;
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryOpen = !inventoryOpen;
            inventoryAni.SetBool("Toggle", inventoryOpen);
        }

        //Interactions
        if (Input.GetKeyDown(interactKey))
        {
            if (currentInteractable != null)
            {
                if (currentInteractable.interactable)
                    currentInteractable.Interact(); 
            }
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

    public void AddItem(Item item)
    {
        inventory.items.Add(item);
        var newItem = Instantiate(itemPrefab, inventoryCanvas.transform, false);
        newItem.GetComponent<RawImage>().texture = item.itemImage;
    }

    public void UpdatePopupText(string input)
    {
        interactText.text = input;
    }

    public void SwapPerspective()
    {
        thirdPerson = !thirdPerson;
        running = !running;

        timerUI.CrossFade("Open", 1f);
        StartCoroutine(nameof(StartTimer));
    }

    public void Respawn()
    {  
        pm.Respawn();
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
    
    IEnumerator StartTimer()
    {
        timer = maxTime;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            if (timer <= 0f)
                break;
        }
    }

    IEnumerator HoldUI()
    {
        updateUI = false;
        yield return new WaitForSeconds(2f);
        updateUI = true;
    }    
}
