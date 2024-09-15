using System.Collections;
using System.Linq;
using UnityEngine;

public class Shut : SceneTransition
{
    public Animator animator;

    private static readonly int IsClosingHash = Animator.StringToHash("isClosing");
    private static readonly int IsOpeningHash = Animator.StringToHash("isOpening");

    public override IEnumerator AnimateTransitionIn()
    {
        Debug.Log("Transition In Started");
        animator.SetBool(IsClosingHash, true);
        yield return new WaitForSeconds(1f); // Adjust the duration to match your animation length
        animator.SetBool(IsClosingHash, false);
        Debug.Log("Transition In Finished");
    }

    public override IEnumerator AnimateTransitionOut()
    {
        Debug.Log("Transition Out Started");
        animator.SetBool(IsOpeningHash, true);
        yield return new WaitForSeconds(1f); // Adjust the duration to match your animation length
        animator.SetBool(IsOpeningHash, false);
        Debug.Log("Transition Out Finished");
    }
}
