using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Playercontroler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movSpeed;                //The movement speed when grounded
    [SerializeField] float airMovSpeed;             //The movement speed when in the air
    [SerializeField] float movAccel;                //The maximum change in velocity the player can do on the ground. This determines how responsive the character will be when on the ground.
    [SerializeField] float airMovAccel;             //The maximum change in velocity the player can do in the air. This determines how responsive the character will be in the air.

    [Header("Jump")]
    [SerializeField] float initialJumpForce;        //The force applied to the player when starting to jump
    [SerializeField] float holdJumpForce;           //The force applied to the character when holding the jump button
    [SerializeField] float maxJumpTime;             //The maximum amount of time the player can hold the jump button

    [Header("Ground detection")]
    [SerializeField] float groundCastRadius;        //Radius of the circle when doing the circle cast to check for the ground
    [SerializeField] float groundCastDist;          //Distance of the circle cast

    [Header("Misc")]
    [SerializeField] float gravityMultiplier = 2.7f;

    //Rigidbody cache
    new Rigidbody2D rigidbody;

    bool isGrounded;

    void Start()
    {
        //Setup our rigidbody 
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // rigidbody.AddForce(gravityMultiplier * Physics2D.gravity * rigidbody.mass, ForceMode2D.Force); //Handel the gravity force 

        isGrounded = DoGroundCheck();

        if (Input.GetButtonDown("A"))
        {
            Jump();
        }
    }

    public void Move(Vector2 _dir)
    {
        Debug.Log(_dir);
        Vector2 velocity = rigidbody.velocity;
      

        Vector2 targetVelocity = _dir * (isGrounded ? movSpeed : airMovSpeed);
        
        //The change in velocity we need to perform to achieve our target velocity
        Vector2 velocityDelta = targetVelocity - velocity;

        //The maximum change in velocity we can do
        float maxDelta = isGrounded ? movAccel : airMovAccel;

        //Clamp the velocity delta to our maximum velocity change
        velocityDelta.x = Mathf.Clamp(velocityDelta.x, -maxDelta, maxDelta);

        //We don't want to move the character vertically
        velocityDelta.y = 0;
        Debug.Log(velocityDelta * rigidbody.mass);
        //Apply the velocity change to the character
        rigidbody.AddForce(velocityDelta * rigidbody.mass, ForceMode2D.Impulse);
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
        if (!isGrounded)
        {
            return;
        }

        rigidbody.AddForce(Vector3.up * initialJumpForce * rigidbody.mass, ForceMode2D.Impulse);

        StartCoroutine(JumpCoroutine());
    }

    IEnumerator JumpCoroutine() //Jump timer 
    {
        //true if the player is holding the Jump button down
        bool wantsToJump = true;

        //Counts for how long we've been jumping
        float jumpTimeCounter = 0;

        while (wantsToJump && jumpTimeCounter <= maxJumpTime)
        {
            jumpTimeCounter += Time.deltaTime;

            //check if the player still wants to jump
            wantsToJump = Input.GetButton("A");

            rigidbody.AddForce(Vector3.up * holdJumpForce * rigidbody.mass * maxJumpTime / jumpTimeCounter);

            yield return null;
        }
    }
}