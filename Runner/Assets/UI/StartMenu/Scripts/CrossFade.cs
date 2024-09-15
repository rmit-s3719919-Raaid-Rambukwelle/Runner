using System.Collections;
using UnityEngine;

public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;

    public override IEnumerator AnimateTransitionIn()
    {
        Debug.Log("Transition In Started");
        crossFade.alpha = 0f;
        yield return StartCoroutine(FadeCanvasGroup(crossFade, 0f, 1f, 1f)); // Fade in over 1 second
        Debug.Log("Transition In Finished");

    }

    public override IEnumerator AnimateTransitionOut()
    {
        Debug.Log("Transition Out Started");
        yield return StartCoroutine(FadeCanvasGroup(crossFade, 1f, 0f, 1f)); // Fade out over 1 second
        Debug.Log("Transition Out Finished");

    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        // Set the initial alpha value
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            // Increment time
            elapsed += Time.deltaTime;

            // Calculate and update the alpha value
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);

            yield return null; // Wait until the next frame
        }

        // Ensure the final alpha value is set
        canvasGroup.alpha = endAlpha;
    }
}
