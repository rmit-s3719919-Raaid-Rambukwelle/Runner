using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject dialogueBox;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypeWriter typeWriterEffect;

    private void Start()
    {
        CloseDialogueBox();
        responseHandler = GetComponent<ResponseHandler>();
        typeWriterEffect = GetComponent<TypeWriter>();
    }

    public void ShowDialogue (DialogueObject dialogueObject) 
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) 
    {

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++) 
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses) 
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        } else 
            {
            CloseDialogueBox();
            }

    }

    private void CloseDialogueBox() 
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
