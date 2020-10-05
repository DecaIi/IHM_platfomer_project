using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImputControler : MonoBehaviour
{
    [SerializeField] Playercontroler playercontroler;
    // Start is called before the first frame update
    void Start()
    {
       // Playercontroler playercontroler = gameObject.GetComponent<Playercontroler>();
    }
    bool MANNETE = false;

    Vector2 output;
    float leftHorizontal;
    float leftVertical;
    float padHorizontal;
    float padVertical;
    // Update is called once per frame
    void Update()
    {
        if (MANNETE) { 
        if (Input.GetButton("B"))
            {                
                //nothing yet 
            }
            //nothing yet 
            if (Input.GetButton("A"))
            {                
                //nothing yet 
            }
            if (Input.GetButton("X"))
            {                 
                //nothing yet 
            }
            if (Input.GetButton("Y"))
            {
                //nothing yet 
            }
            leftHorizontal = Input.GetAxis("LeftHorisontalAxis");
            leftVertical = Input.GetAxis("LeftVerticalAxis");
            padHorizontal = Input.GetAxis("DirectionalCroosHorisontal");
            padVertical = Input.GetAxis("DirectionalCroosVertical");
            output  = new Vector2(Mathf.Clamp(leftHorizontal + padHorizontal, -1, 1) , Mathf.Clamp(leftVertical + padVertical, -1, 1) );
        }else
        {
            output = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        playercontroler.Move(output);
    }

}

