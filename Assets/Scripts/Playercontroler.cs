﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
public class Playercontroler : MonoBehaviour
{
    [Header("Movement")]
    
    [Header("GroundParameter")]
    [SerializeField] float movSpeedMax;                //The max movement speed when grounded
    [SerializeField] float movAccelMax;                //The maximum change in velocity the player can do on the ground. This determines how responsive the character will be when on the ground.
    [SerializeField] float movDeccelMax;               //the maximum change in velocity grounded when the player is free ( no imput comand ) 
    [Header("airParameter")]
    [SerializeField] float airMovSpeedMax;             //The max movement speed when in the air
    [SerializeField] float airMovAccelMax;             //The maximum change in velocity the player can do in the air. This determines how responsive the character will be in the air.
    [SerializeField] float airMovDeccelMax;            //the maximum change in velocity grounded when player is "free" ( no imput command)
    [SerializeField] float fallingAccel;               //the acceleration when push down 
    [Header("Jump")]
    [SerializeField] float initialJumpAccel;        //The force applied to the player when starting to jump
    [SerializeField] float jumpDelay;
    [Header("Ground detection")]
    [SerializeField] float groundCastRadius;        //Radius of the circle when doing the circle cast to check for the ground
    [SerializeField] float groundCastDist;//Distance of the circle cast
    
    [Header("Faling parameter")]    
    [SerializeField] float gravityAccel;            //change in velovity (y) due to gravity 
    [SerializeField] float maxFallingSpeed;         // maximum speed the object can acces in y 

    
    Vector2 curentVelocity;

    //Rigidbody cache
    new Rigidbody2D rigidbody;


    bool isGrounded;
    bool canJump = true;

    void Start()
    {
        isGrounded = false;
        curentVelocity = new Vector2(0,0);
        //Setup our rigidbody 
    }

    void Update()
    {
        // rigidbody.AddForce(gravityMultiplier * Physics2D.gravity * rigidbody.mass, ForceMode2D.Force); //Handel the gravity force 

        isGrounded = DoGroundCheck();

        if (Input.GetButtonDown("A"))
        {
            Jump();
        }
        ApplyGravity();
        ApplyVelocity();
    }

    void ApplyGravity()
    {
        if (isGrounded) 
        {
            if ( curentVelocity.y < 0)
                this.curentVelocity.y = 0;
            if (curentVelocity.y > 0)
                Debug.Log(curentVelocity);
            return;
        }
        this.curentVelocity.y += gravityAccel;
        //this.curentVelocity.y = Mathf.Clamp(curentVelocity.y, -maxFallingSpeed, 0);
    }
    void ApplyVelocity()
    {
        //Debug.Log(curentVelocity);
        transform.position += new Vector3(curentVelocity.x, curentVelocity.y, 0) * Time.deltaTime;
    }


    public void Move(Vector2 _dir)
    {
        //Debug.Log(_dir);

        if (_dir.x == 0 && _dir.y == 0) //no movment imput 
        {
            if (isGrounded)
            {
                
            }
            else
            {
                
            }
        }
        else 
        {
           Vector2 targetVelocity = _dir * (isGrounded ? movSpeedMax : airMovSpeedMax);
           //The change in velocity we need to perform to achieve our target velocity
           Vector2 targettAccel = targetVelocity - curentVelocity;
            //the maximum change in velocity we can do 

           float maxAccel = isGrounded ? movAccelMax : airMovAccelMax;
           //Clamp the velocity to our maximum velocity change
           targettAccel.x = Mathf.Clamp(targettAccel.x, -maxAccel, maxAccel);
           targettAccel.y = Mathf.Clamp(targettAccel.y, -fallingAccel, 0); //allow to fall fast but not to jump higer 
           curentVelocity += targettAccel *Time.deltaTime;
           //Debug.Log(targettAccel);
        }
    }

    bool DoGroundCheck()
    {
        return DoGroundCast() != Vector2.zero;
    }

    Vector2 DoGroundCast() //cast a ray in the botom way ad return the noral vector betwin what is hit and the player
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];
        if (Physics2D.CircleCast(transform.position, groundCastRadius, Vector3.down, new ContactFilter2D(), hits, groundCastDist) > 1)
        {
            return hits[1].normal;
        }
        return Vector2.zero;
    }

    void Jump() // jump if the player is grounder and start a timer for the jump
    {
        Debug.Log("canjum: " + canJump);
        Debug.Log("grounded:" + isGrounded);

        if (!isGrounded || !canJump)
        {
            return;
        }

        this.curentVelocity.y += initialJumpAccel ;

        StartCoroutine(JumpCoroutine());
    }

    IEnumerator JumpCoroutine() //Jump timer 
    {
        //true if the player is holding the Jump button down
        canJump = false;

        //Counts for how long we've been jumping
        float jumpTimeCounter = 0;

        while (jumpTimeCounter <= jumpDelay)
        {
            jumpTimeCounter += Time.deltaTime;

        }
        canJump = true; 
        yield return null;
        
    }
}