using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        StartCoroutine(LoadScene("Menu"));
    }

    public void LoadMenu()
    {
        UnloadScenes();
        StartCoroutine(LoadScene("Menu"));
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        UnloadScenes();
        StartCoroutine(LoadScene("LVL2"));
    }

    public void LoadSettings()
    {
        UnloadScenesExceptGame();
        StartCoroutine(LoadScene("Settings"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UnloadScenesExceptGame();
        StartCoroutine(LoadScene("Pause"));
    }

    public void Resume()
    {
        UnloadScenesExceptGame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LVL2"));
        Time.timeScale = 1;
    }

    private void UnloadScenes()
    {
        for (int maxSceneIndex = SceneManager.sceneCount - 1; maxSceneIndex > 0; --maxSceneIndex)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(maxSceneIndex));
        }
    }

    private void UnloadScenesExceptGame()
    {
        for (int maxSceneIndex = SceneManager.sceneCount - 1; maxSceneIndex > 0; --maxSceneIndex)
        {
            if (SceneManager.GetSceneAt(maxSceneIndex).name != "LVL2")
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(maxSceneIndex));
            }
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        yield return null; // Wait for the next frame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LVL2")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }  
}
