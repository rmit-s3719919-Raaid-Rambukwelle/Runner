using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPause : MonoBehaviour
{
    public KeyCode interactKey;
    public GameObject dialogueBox;
    public string tutorialMsg;
    public TypeWriter tp;
    public TMP_Text textLabel;
    public float delay;
    public bool paused = false;
    public bool checking = false;
    public bool interactable = true;
    public bool buttonPressed = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactable)
        {
            Debug.LogWarning("HIT: " + interactKey);
            Time.timeScale = 0.05f;
            checking = true;
            StopAllCoroutines();
            StartCoroutine(nameof(WaitForDelay));
            dialogueBox.SetActive(true);
            tp.Run(tutorialMsg, textLabel);
        }
    }

    private void Update()
    {
        if (Input.GetKey(interactKey) && checking)
        {
            buttonPressed = true;
        }

        if (paused && buttonPressed && interactable)
        {
            Time.timeScale = 1;
            interactable = false;
            Debug.LogWarning("FINISHED: " + interactKey);
            dialogueBox.SetActive(false);
        }

    }

    IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(delay * 0.05f);
        paused = true;
    }
}
