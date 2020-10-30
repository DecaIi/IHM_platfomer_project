using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
    {

        leftHorizontal = Input.GetAxis("LeftHorizontal");
        leftVertical = Input.GetAxis("LeftVertical");
        padHorizontal = Input.GetAxis("CrossHorizontal");
        padVertical = Input.GetAxis("CrossVertical");
        output  = new Vector2(Mathf.Clamp(leftHorizontal + padHorizontal, -1, 1) , Mathf.Clamp(leftVertical + padVertical, -1, 1) );
        
        if (Input.GetButtonDown("A")) //jum^p
        {
            playerControler.Jump();
        }
        if (Input.GetButton("B")) //dash
        {
            playerControler.Dash(output);
        }
        if (Input.GetButton("Y"))
        {
            //nothing yet 
        }
        if (Input.GetButtonUp("X") )        //when the buton is realeased stop grab 
        {
            playerControler.EndGrab();
        }
        if (Input.GetButtonDown("X") )  //when the buton is presed start to grab 
        {
            playerControler.StartGrab();
        }
        else                        // no movment alowed if dash
        {
            playerControler.Move(output);
        }

        playerControler.UpdateDirection(output);
        
    }

}

