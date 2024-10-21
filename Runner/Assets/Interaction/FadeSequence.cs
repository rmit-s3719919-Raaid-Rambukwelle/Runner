using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FadeSequence : MonoBehaviour
{
    public Rigidbody rb;
    public RawImage fadeImage;
    public float duration;
    public TextMeshProUGUI uiText;
    public GameObject buttonObj;
    public GameObject crosshair;
    public string message;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade();
        }
    }

    public void StartFade()
    {
        StartCoroutine(nameof(FadeIn));
    }

    IEnumerator FadeIn()
    {
        Color imageColor = fadeImage.color;

        imageColor.a = 0f;
        fadeImage.color = imageColor;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Clamp01(elapsedTime / duration);
            fadeImage.color = imageColor;
            yield return null;
        }

        imageColor.a = 1f;

        uiText.text = message;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        buttonObj.SetActive(true);
        crosshair.SetActive(false);
        PlayerManager.current.canMove = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        PlayerManager.current.StopTimer();
        fadeImage.color = imageColor;
    }
}