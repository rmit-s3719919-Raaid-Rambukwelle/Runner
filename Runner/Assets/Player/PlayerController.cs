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



    private void Start()
    {
        instance = this;
        interactables = new List<Interactable>();
    }






}
