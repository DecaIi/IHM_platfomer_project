using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] SceneAsset mainMenu;
    [SerializeField] SceneAsset settingsMenu;
    [SerializeField] SceneAsset game;
    [SerializeField] SceneAsset pauseMenu;

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
        StartCoroutine(LoadScene(mainMenu));
    }

    public void LoadMenu()
    {
        UnloadScenes();
        StartCoroutine(LoadScene(mainMenu));
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        UnloadScenes();
        StartCoroutine(LoadScene(game));
    }

    public void LoadSettings()
    {
        UnloadScenesExceptGame();
        StartCoroutine(LoadScene(settingsMenu));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UnloadScenesExceptGame();
        StartCoroutine(LoadScene(pauseMenu));
    }

    public void Resume()
    {
        UnloadScenesExceptGame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(game.name));
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
            if (SceneManager.GetSceneAt(maxSceneIndex).name != game.name)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(maxSceneIndex));
            }
        }
    }

    private IEnumerator LoadScene(SceneAsset scene)
    {
        SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
        yield return null; // Wait for the next frame
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    
     
}
