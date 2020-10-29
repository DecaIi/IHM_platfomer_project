using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public static string[] LevelScenes { get; private set; } = { "ControllerTuto", "KeyboardTuto", "SampleScene", "LVL2" };

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
        StartCoroutine(LoadScene(sceneName, keepGameScene, isGameScene));
        StartCoroutine(UnloadScenes(sceneName, keepGameScene, isGameScene));

        yield return null;
    }

    private IEnumerator UnloadScenes(string nextSceneName, bool keepGameScene = false, bool isGameScene = false)
    {
        if (keepGameScene && (currentGameScene != ""))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentGameScene));
        }

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

    private IEnumerator LoadScene(string sceneName, bool keepGameScene = false, bool isGameScene = false)
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

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
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
