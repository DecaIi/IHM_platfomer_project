using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    [SerializeField] Text timerText;

    float startTime;

    void Start()
    {
        startTime = Time.time;
        timerText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.LevelTime = Time.time - startTime;
        timerText.text = ((GameManager.Instance.LevelTime)).ToString("0.0", new CultureInfo("en-US"));
    }
}
