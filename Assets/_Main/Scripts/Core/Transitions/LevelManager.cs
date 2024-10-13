using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject transitionsContainer; // Container for transition scripts

    private SceneTransition[] transitions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object when changing scenes
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }

    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        // Find the transition to use
        SceneTransition transition = System.Array.Find(transitions, t => t.name == transitionName);
        
        if (transition == null)
        {
            Debug.LogError($"Transition '{transitionName}' not found.");
            yield break;
        }

        // Start the transition in
        yield return transition.AnimateTransitionIn();

        // Load the scene asynchronously
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        sceneLoadOperation.allowSceneActivation = false; // Prevent scene from activating immediately

        // Wait until the scene is loaded
        while (!sceneLoadOperation.isDone)
        {
            // Here you could update a progress bar if you want
            if (sceneLoadOperation.progress >= 0.9f)
            {
                break; // Scene is ready to be activated
            }
            yield return null; // Wait until next frame
        }

        sceneLoadOperation.allowSceneActivation = true; // Activate the scene

        // Start the transition out
        yield return transition.AnimateTransitionOut();
    }
}

