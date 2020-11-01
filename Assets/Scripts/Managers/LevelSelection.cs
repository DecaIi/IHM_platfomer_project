using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public void Menu()
    {
        GameManager.Instance.LoadMenu();
    }

    public void ControllerTuto()
    {
        GameManager.Instance.LoadGameScene(0);
    }

    public void KeyboardTuto()
    {
        GameManager.Instance.LoadGameScene(1);
    }

    public void Level1()
    {
        GameManager.Instance.LoadGameScene(2);
    }

    public void Level2()
    {
        GameManager.Instance.LoadGameScene(3);
    }

    public void Level3()
    {
        GameManager.Instance.LoadGameScene(4);
    }
}
