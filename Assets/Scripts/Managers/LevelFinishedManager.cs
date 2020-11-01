using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinishedManager : MonoBehaviour
{
    [SerializeField] Text time;
    [SerializeField] Text stars;

    // Start is called before the first frame update
    void Start()
    {
        time.text = GameManager.Instance.LevelTime.ToString("0.0", new CultureInfo("en-US"));
        stars.text = GameManager.Instance.LevelStars.ToString();
    }

    public void Continue()
    {
        if (GameManager.Instance.NextLevel == "Menu")
        {
            GameManager.Instance.LoadMenu();
        }
        else
        {
            GameManager.Instance.LoadGameScene(GameManager.Instance.NextLevel);
        }
    }
}
