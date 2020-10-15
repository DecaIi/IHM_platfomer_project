using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImputControler : MonoBehaviour
{
    [SerializeField] Playercontroler playerControler;

    void Start()
    {
       // Playercontroler playercontroler = gameObject.GetComponent<Playercontroler>();
    }

    Vector2 output;
    float leftHorizontal;
    float leftVertical;
    float padHorizontal;
    float padVertical;
    bool dash;
    void Update()
    {
        dash = false;
        if (Input.GetButton("A")) //jum^p
        {
            playerControler.Jump(); 
        }
        if (Input.GetButton("B")) //dash
        {
            dash = true;
        }
        if (Input.GetButton("X"))
        {                 
                //nothing yet 
        }
        if (Input.GetButton("Y"))
        {
                //nothing yet 
        }

        leftHorizontal = Input.GetAxis("LeftHorizontal");
        leftVertical = Input.GetAxis("LeftVertical");
        padHorizontal = Input.GetAxis("CrossHorizontal");
        padVertical = Input.GetAxis("CrossVertical");

        output  = new Vector2(Mathf.Clamp(leftHorizontal + padHorizontal, -1, 1) , Mathf.Clamp(leftVertical + padVertical, -1, 1) );
        if (dash) //in case of dash
        {
            playerControler.Dash(output);
            return;
        }
        playerControler.Move(output);
    }

}

