using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : PlayerControlHandler
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject dialogueBox;
    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypeWriter typeWriterEffect;

    protected override void Start()
    {
        base.Start();
        CloseDialogueBox();
        responseHandler = GetComponent<ResponseHandler>();
        typeWriterEffect = GetComponent<TypeWriter>();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        SequenceOneController sequenceController = FindObjectOfType<SequenceOneController>();
        if (sequenceController != null && sequenceController.isSequenceOneActive())
        {
            // Prevent dialogue if the opening sequence is active
            return;
        }

        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));

        DisablePlayerControls();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return new WaitUntil( () => (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(PlayerManager.current.interactKey)) );
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;

        EnablePlayerControls();

        if (TryGetComponent(out DialogueActivator dialogueActivator)) 
        {
            dialogueActivator.OnDialogueClose();
        }
    }

}