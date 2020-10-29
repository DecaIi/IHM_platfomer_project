using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalZone : MonoBehaviour
{
    [SerializeField] string nextLevel;

    private bool levelFinished = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!levelFinished)
        {
            levelFinished = true;
            GameManager.Instance.LoadGameScene(nextLevel);
        }
    }
}
