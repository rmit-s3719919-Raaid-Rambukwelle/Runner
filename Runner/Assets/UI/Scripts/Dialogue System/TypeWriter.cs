using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;
    // Start is called before the first frame update
    public Coroutine Run(string textToType,TMP_Text textLabel)
    {
       return StartCoroutine(routine: TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel) 
    {
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while(charIndex < textToType.Length) 
        {
            t += Time.deltaTime * typeWriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, min: 0, max: textToType.Length);

            textLabel.text = textToType.Substring(startIndex: 0, length: charIndex);

            yield return null;
        }

        textLabel.text = textToType;
    }
}
