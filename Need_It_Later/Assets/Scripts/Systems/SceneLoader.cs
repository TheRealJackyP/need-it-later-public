using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public UnityEvent<SceneLoader, string> OnSceneLoadComplete = new();
    public UnityEvent<SceneLoader, string> OnSceneUnloadComplete = new();
    public GameObject LoadingScreen;
    public Coroutine LoadInstance;
    public List<string> LoadableScenes;

    private void Start()
    {
        SceneManager.sceneLoaded += DisableLoadingScreen;
    }

    public void DisableLoadingScreen(Scene scene, LoadSceneMode mode)
    {
        if (LoadableScenes.Contains(scene.name))
        {
            LoadingScreen.SetActive(false);
        }
    }

    public void DoSceneLoad(int sceneIndex)
    {
        if (LoadInstance != null) return;
        LoadInstance = StartCoroutine(ExecuteSceneLoad(sceneIndex));
    }

    public void DoSceneUnload(int sceneIndex)
    {
        if (LoadInstance != null) return;
        LoadInstance = StartCoroutine(ExecuteSceneUnload(sceneIndex));
    }

    public void DoSceneReset(int sceneIndex)
    {
        if (LoadInstance != null) return;
        LoadInstance = StartCoroutine(ExecuteSceneReset(sceneIndex));
    }

    public IEnumerator ExecuteSceneLoad(int sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        OnSceneLoadComplete.Invoke(this, SceneManager.GetSceneByBuildIndex(sceneIndex).name);
    }
    
    public IEnumerator ExecuteSceneUnload(int sceneIndex)
    {
        yield return SceneManager.UnloadSceneAsync(sceneIndex);
        OnSceneUnloadComplete.Invoke(this, SceneManager.GetSceneByBuildIndex(sceneIndex).name);
    }
    
    public IEnumerator ExecuteSceneReset(int sceneIndex)
    {
        var currentScene  = SceneManager.GetActiveScene();
        yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        yield return SceneManager.UnloadSceneAsync(currentScene);
        OnSceneLoadComplete.Invoke(this, SceneManager.GetSceneByBuildIndex(sceneIndex).name);
    }
}
