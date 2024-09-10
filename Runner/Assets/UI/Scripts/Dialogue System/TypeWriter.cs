using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
        {
            new Punctuation(punctuations: new HashSet<char>(){'.', '!', '?'}, waitTime: 0.2f),
            new Punctuation(punctuations: new HashSet<char>(){',', ';', ':'}, waitTime: 0.1f),

        };

    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(routine: TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            int lastCharacterIndex = charIndex;

            t += Time.deltaTime * typeWriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, min: 0, max: textToType.Length);

            for (int i = lastCharacterIndex; i < charIndex; i++)
            {
                bool isLast = i == textToType.Length - 1;


                textLabel.text = textToType.Substring(startIndex: 0, length: i + 1);

                if (IsPunctuation(textToType[i], out float waitTime) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }

        textLabel.text = textToType;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}