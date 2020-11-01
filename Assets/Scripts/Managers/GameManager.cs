using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static string baseScene = "BaseScene";
    private static string menuScene = "Menu";
    private static string levelsScene = "LevelSelection";
    private static string settingsScene = "Settings";
    private static string pauseScene = "Pause";
    private static string currentGameScene = "";
    private static string overlayScene = "Overlay";
    private static string levelFinishedScene = "LevelFinished";

    public static string[] LevelScenes { get; private set; } = { "ControllerTuto", "KeyboardTuto", "Level1", "Level2" };

    public float LevelTime { get; set; } = 0.0f;
    public int LevelStars { get; set; } = 0;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                throw new System.Exception("GameManager not instanciated. Hint: Start from the BaseScene to be able to perform scene changes.");
            }

            return instance;
        }
    }
    
    private void Awake()
    {
        instance = this;
        StartCoroutine(ChangeScene(menuScene));
    }

    public void LoadMenu()
    {
        StartCoroutine(ChangeScene(menuScene));
    }

    public void LoadLevels()
    {
        StartCoroutine(ChangeScene(levelsScene));
    }

    public void LoadGameScene(int gameSceneIndex)
    {
        Time.timeScale = 1;
        StartCoroutine(ChangeScene(LevelScenes[gameSceneIndex], false, true));
    }

    public void LoadGameScene(string gameSceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(ChangeScene(gameSceneName, false, true));
    }

    public void LoadLevelFinished()
    {
        Time.timeScale = 0;
        StartCoroutine(ChangeScene(levelFinishedScene, true));
    }

    public void LoadSettings()
    {
        StartCoroutine(ChangeScene(settingsScene, true));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        StartCoroutine(ChangeScene(pauseScene, true));
    }

    public void Resume()
    {
        StartCoroutine(ChangeScene(currentGameScene, true, true));
        Time.timeScale = 1;
    }

    private IEnumerator ChangeScene(string sceneName, bool keepGameScene = false, bool isGameScene = false)
    {
        if (!keepGameScene)
        {
            if (isGameScene)
            {
                currentGameScene = sceneName;
            }
            else
            {
                currentGameScene = "";
            }
        }

        StartCoroutine(LoadScene(sceneName));
        StartCoroutine(UnloadScenes(sceneName, keepGameScene, isGameScene));

        if (isGameScene)
        {
            StartCoroutine(LoadScene(overlayScene, false));
        }

        yield return null;
    }

    private IEnumerator UnloadScenes(string nextSceneName, bool keepGameScene = false, bool isGameScene = false)
    {
        List<AsyncOperation> operations = new List<AsyncOperation>();

        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            if (SceneManager.GetSceneAt(i).name == baseScene ||
                SceneManager.GetSceneAt(i).name == nextSceneName ||
                (keepGameScene && SceneManager.GetSceneAt(i).name == currentGameScene))
            {
                continue;
            }
            else
            {
                operations.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));
            }
        }

        foreach (AsyncOperation operation in operations)
        {
            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }

    private IEnumerator LoadScene(string sceneName, bool setNewSceneActive = true)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            AsyncOperation newSceneLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!newSceneLoad.isDone)
            {
                yield return null;
            }

            newSceneLoad.allowSceneActivation = true;
        }

        if (setNewSceneActive)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }

    private void Update()
    {
        if (currentGameScene != "")
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
            {
                Pause();
            }
        }
    }  
}
