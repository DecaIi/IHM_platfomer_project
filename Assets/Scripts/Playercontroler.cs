using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class Playercontroler : MonoBehaviour
{
    [Header("Movement")]
    
    [Header("GroundParameter")]
    [SerializeField] float movSpeedMax;                //The max movement speed when grounded
    [SerializeField] float movAccelMax;                //The maximum change in velocity the player can do on the ground. This determines how responsive the character will be when on the ground.
    [SerializeField] float movDeccelMax;               //the maximum change in velocity grounded when the player is free ( no imput comand ) 
    [SerializeField] float uTurnAccel;                  // accel to add when the player uturning
    [Header("airParameter")]
    [SerializeField] float airMovSpeedMax;             //The max movement speed when in the air
    [SerializeField] float airMovAccelMax;             //The maximum change in velocity the player can do in the air. This determines how responsive the character will be in the air.
    [SerializeField] float airMovDeccelMax;            //the maximum change in velocity grounded when player is "free" ( no imput command)
    [SerializeField] float fallingAccel;               //the acceleration when push down 
    [Header("Jump")]
    [SerializeField] float initialJumpAccel;        //The force applied to the player when starting to jump
    [SerializeField] float jumpDelay;
    [SerializeField] float jumpHeight;
    [Header("Ground detection")]
    [SerializeField] float groundCastRadius;                //Radius of the circle when doing the circle cast to check for the ground
    [SerializeField] float groundDistanceDetection;          //Distance of the circle cast
    
    [Header("Faling parameter")]    
    [SerializeField] float gravityAccel;            //change in velovity (y) due to gravity 
    [SerializeField] float maxFallingSpeed;         // maximum speed the object can acces in y 

    [SerializeField] LayerMask platformLayer;

    Collider2D playerCollider;
    
    Vector2 currentVelocity;

    public Vector2 Velocity
    {
        get { return currentVelocity; }
    }

    bool canJump = true;

    void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        currentVelocity = new Vector2(0,0);
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ApplyVelocity();
    }

    void ApplyGravity()
    {
        ComputeGravity();
    }

    void ApplyVelocity()
    {
        transform.position += new Vector3(currentVelocity.x, currentVelocity.y, 0) * Time.deltaTime;
    }

    public void Move(Vector2 _dir)
    {
        if (_dir.x == 0 && _dir.y == 0) //no movment imput 
        {
            float deceleration = IsGrounded() ? movDeccelMax : airMovDeccelMax;
            ComputeVelocity(0f, -deceleration, currentVelocity.x);
        }
        else
        {
            float maxAccel = (IsGrounded() ? movAccelMax : airMovAccelMax);
            float maxSpeed = (IsGrounded() ? movSpeedMax : airMovSpeedMax);
            ComputeVelocity(maxSpeed, maxAccel, _dir.x);
            //TODO : faire que ça desende plus vite quand on apuis vers le bas
            //TODO : faire un acceleration suplementaire pour les demis tours 
        }
    }

    /**
     * Computes velocity of the character.
     * \param targetVelocity    The absolute velocity the character has to reach.
     * \param acceleration      The maximum accelration of the character to reach the target velocity. If negative, the player will decelerate.
     * \param direction         The direction the character is moving, towards positive x if positive, towards negative x otherwise. 
     *                          Value has no effect.
     */
    void ComputeVelocity(float targetVelocity, float acceleration, float direction)
    {
        // Making sure the value of direction doesn't affect our calculations
        direction = Mathf.Sign(direction);

        float signedAcceleration = direction * acceleration;
        float signedTargetVelocity = direction * targetVelocity;

        float computedVelocity = currentVelocity.x + signedAcceleration * Time.deltaTime;

        // If the player accelrates or deceleartes but hasn't reached targetVelocity, we apply computedVelocity
        if ((signedAcceleration > 0 && computedVelocity < signedTargetVelocity) || (signedAcceleration < 0 && computedVelocity > signedTargetVelocity))
        {
            currentVelocity.x = computedVelocity;
        }
        // Else the player has already reached targetVelocity and we make sure it stays at that targetVelocity
        else
        {
            currentVelocity.x = signedTargetVelocity;
        }
    }

    bool ComputeJump(float targetHeight, float acceleration)
    {
        float computedVelocity = currentVelocity.y + acceleration * Time.deltaTime;

        Debug.Log(computedVelocity * Time.deltaTime);

        if (computedVelocity * Time.deltaTime < targetHeight)
        {
            currentVelocity.y = computedVelocity;
            return true;
        }

        return false;
    }

    void ComputeGravity()
    {
        float resultingVerticalVelocity = currentVelocity.y + gravityAccel * Time.deltaTime;

        RaycastHit2D bottomHit = BottomCollision(resultingVerticalVelocity);
        if (bottomHit.collider != null)
        {
            if (currentVelocity.y < 0)
            {
                currentVelocity.y = 0f;
            }
            transform.position += Vector3.down * (bottomHit.distance);
        }
        else
        {
            currentVelocity.y = resultingVerticalVelocity;
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, groundDistanceDetection, platformLayer);
        return raycastHit.collider != null;
    }

    RaycastHit2D BottomCollision(float verticalVelocity)
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, verticalVelocity, platformLayer);
        return raycastHit;
    }

    public void Jump() // jump if the player is grounder and start a timer for the jump
    {
        if (!IsGrounded() || !canJump)
        {
            return;
        }

        //StartCoroutine(JumpCoroutine());
    }

    IEnumerator JumpCoroutine()
    {
        canJump = false;

        while (ComputeJump(jumpHeight, initialJumpAccel)) {}
        yield return null;
    }

    IEnumerator JumpCoroutine2() //Jump timer 
    {
        //true if the player is holding the Jump button down
        canJump = false;

        //Counts for how long we've been jumping
        float jumpTimeCounter = 0;

        while (jumpTimeCounter <= jumpDelay)
        {
            ComputeJump(jumpHeight, initialJumpAccel);
            jumpTimeCounter += Time.deltaTime;

        }
        canJump = true; 
        yield return null;
        
    }
}