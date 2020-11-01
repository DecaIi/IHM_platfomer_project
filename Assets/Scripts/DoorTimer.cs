using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]
public class DoorTimer : MonoBehaviour
{
    [SerializeField] float offsetTime;
    [SerializeField] float openTime;
    [SerializeField] float closeTime;

    bool timerRunning;


    Door door;

    void Awake()
    {
        door = GetComponent<Door>();
    }

    private void Start()
    {
        StartCoroutine(Timer(offsetTime));
    }

    private void Update()
    {
        if (!timerRunning)
        {
            if (door.IsOpen)
            {
                Debug.Log("Close Timer");
                door.SetDoorOpen(false);
                StartCoroutine(Timer(closeTime));
            }
            else
            {
                Debug.Log("Open Timer");
                door.SetDoorOpen(true);
                StartCoroutine(Timer(openTime));
            }
        }
    }

    private IEnumerator Timer(float timer)
    {
        Debug.Log("Timer started");
        timerRunning = true;
        yield return new WaitForSeconds(timer);
        timerRunning = false;
    }




}
