using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Movingplatform : MonoBehaviour
{
    Playercontroler playercontroler; // player controler 
    // Transforms to act as start and end markers for the journey.
    public Vector2 startposition;
    public Vector2 endposition;
    // Time when the movement started.
    [SerializeField] float speed;
    [SerializeField] bool permaMovign = true;
    
    private float startTime;
    // Total distance between the markers.
    private float trajetlength;

    bool CanReVert =true; //if we can eange start and end point 

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        trajetlength = Vector3.Distance(startposition, endposition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playercontroler = other.gameObject.GetComponent<Playercontroler>();
        if (playercontroler != null)
        {
            playercontroler.TakeOver( Mathf.Sign(endposition.x-startposition.x)*speed);
        }
    }




    /** echange Start and end position
     */
    private void InvertionsartEnd()
    {
        Vector2 temp = Vector2.zero + startposition;
        startposition = endposition;
        endposition = temp;
        startTime = Time.time;
    }    
  
    // Update is called once per frame
    // Move to the target end position.
    void Update()
    {

        Vector2 curentpossition = transform.position; //implicit transform
        if (    permaMovign  
            &&  CanReVert   //cansel jum form start to end 
            &&  (curentpossition == startposition || curentpossition == endposition))
        {
            InvertionsartEnd();
            StartCoroutine(Wait(0.2f)); // wait 0.2s befor enney other inversion 
        }
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfDistance = distCovered / trajetlength; 

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startposition, endposition, fractionOfDistance);
     }

    IEnumerator Wait(float time)
    {
        CanReVert= false;
        yield return  new WaitForSeconds(time);
        CanReVert = true;
    }

}
