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
    bool paused = false;
    bool interactable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactable)
        {
            Time.timeScale = 0.05f;
            interactable = false;
            StartCoroutine(nameof(WaitForDelay));
            dialogueBox.SetActive(true);
            tp.Run(tutorialMsg, textLabel);
        }
    }

    private void Update()
    {
        if (paused && Input.GetKey(interactKey))
        {
            Time.timeScale = 1;
            dialogueBox.SetActive(false);
        }

    }

    IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(delay / 0.05f);
        paused = true;
    }
}
