using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class SceneTransition : MonoBehaviour
{
    public abstract IEnumerator AnimateTransitionIn();
    public abstract IEnumerator AnimateTransitionOut();

}
