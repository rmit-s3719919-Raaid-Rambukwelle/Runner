using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Slider progressBar;
    public GameObject transitionsContainer;
    private SceneTransition[] transitions;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    } 

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
        Debug.Log("Number of transitions found: " + transitions.Length);
    }

    public void LoadScene(string sceneName, string transitionName) 
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName) 
    {
        // Find the transition object
        SceneTransition transition = transitions.First(t => t.name == transitionName);

        // Start transition animation
        yield return transition.AnimateTransitionIn();

        // Load the scene asynchronously but prevent it from activating
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        // Show the progress bar
        progressBar.gameObject.SetActive(true);

        // Update the progress bar while loading
        while (sceneLoad.progress < 0.9f)
        {
            progressBar.value = sceneLoad.progress;
            yield return null;
        }

        // Set the progress bar to 100% and keep it there
        progressBar.value = 1f;

        // Now that the scene has finished loading (but is not yet activated),
        // activate the scene
        sceneLoad.allowSceneActivation = true;

        // Wait until the new scene is fully loaded and activated
        while (!sceneLoad.isDone)
        {
            yield return null;
        }

        // Hide the progress bar once the scene is fully activated
        progressBar.gameObject.SetActive(false);

        // Play the transition out animation after the scene has fully loaded and is active
        yield return transition.AnimateTransitionOut();
    }
}
