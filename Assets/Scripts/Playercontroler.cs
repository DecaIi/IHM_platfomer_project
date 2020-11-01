using System;
using System.Collections;
using UnityEngine;


public class Playercontroler : MonoBehaviour
{
    [Header("Movement")]
    
    [Header("GroundParameter")]
    [SerializeField] float movSpeedMax;                //The max movement speed when grounded
    [SerializeField] float movAccelMax;                //The maximum change in velocity the player can do on the ground. This determines how responsive the character will be when on the ground.
    [SerializeField] float movDeccelMax;               //the maximum change in velocity grounded when the player is free ( no imput comand ) 
    [SerializeField] float uTurnAccel;                 // accel to add when the player uturning
    [Header("airParameter")]
    [SerializeField] float airMovSpeedMax;             //The max movement speed when in the air
    [SerializeField] float airMovAccelMax;             //The maximum change in velocity the player can do in the air. This determines how responsive the character will be in the air.
    [SerializeField] float airMovDeccelMax;            //the maximum change in velocity grounded when player is "free" ( no imput command)
    [SerializeField] float fallingAccel;               //the acceleration when push down 
    [Header("Wall")] 
    [SerializeField] float wallGrabFallingAccel;              //wallmaxaccel on a wall
   
    [Header("Jump")]
    [SerializeField] float initialJumpAccel;            //The force applied to the player when starting to jump
    [SerializeField] Vector2 sideJumpAccel;
    [SerializeField] float jumpDelay;
    [SerializeField] float jumpSideDelay;
    [SerializeField] float disableControlDelay;
    [SerializeField] float wallJumpToleranceTime;

    [Header("Energie")]
    [SerializeField] float maximuEnergie;               //the maximumenergie for wall grab
    [SerializeField] float energieDecresePerTime;       //the energiedecrease when a wall is grab
    [SerializeField] float energieRecoverPerTime;       
    [Header("Dash")]
    [SerializeField] float initialDashAccel;            //The force applied to the player when starting to jump
    [SerializeField] float dashDelay;
    [SerializeField] float dashDuration;                //Duration of the dash 
    [SerializeField] float diagonalDashSuplementaryDelay;
    [SerializeField] float reductiondiage;
    [Header("Ground detection")]
    [SerializeField] float groundCastRadius;            //Radius of the circle when doing the circle cast to check for the ground
    [SerializeField] float groundDistanceDetection;     //Distance of the circle cast
    
    [Header("Faling parameter")]    
    [SerializeField] float gravityAccel;                //change in velovity (y) due to gravity 
    [SerializeField] float maxFallingSpeed;             // maximum speed the object can acces in y 

    [SerializeField] LayerMask platformLayer;

    ContactFilter2D filter;
    

    Rigidbody2D playerRigidbody;
    ContactHandler contactHanlder;

    FeedBackControler feedBackControler;
    
       
    float currentEnergie;
    float speedFactor = 1.0f;
    float suplementarytime;
    Vector2 currentVelocity;
    Vector2 direction;

    public Vector2 Velocity
    {
        get { return currentVelocity; }
        private set { currentVelocity = value; }
    }

    bool canMove = true;
    bool canJumpBottom = true;
    bool canJumpLeft = true;
    bool canJumpRight = true;
    bool canDash = true;
    bool canJum = true;
    bool isGrabing;
    bool startGrabing;
    
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        contactHanlder = GetComponent<ContactHandler>();
        feedBackControler = GetComponent<FeedBackControler>();
        filter = new ContactFilter2D()
        {
            layerMask = platformLayer,
            useLayerMask = true
        };
        isGrabing = false;
        currentVelocity = new Vector2(0, 0);
        direction = new Vector2(0, 0);
        currentEnergie = maximuEnergie;
    }

    void FixedUpdate()
    {
        ComputeGravity();
        Move();
        HandelGrab();
        HandleCollisions();
        ApplySpeedFactor();
        ApplyVelocity();
    }
  
    void ApplyVelocity()
    {
        transform.position += new Vector3(currentVelocity.x, currentVelocity.y, 0) * Time.deltaTime;
    }

    public void Move()
    {
        Vector2 _dir = direction;
        if (!canMove)
        {
            return;
        }

        if (_dir.x == 0 && _dir.y == 0) //no movment imput 
        {
            float deceleration = contactHanlder.Contacts.Bottom ? movDeccelMax : airMovDeccelMax;
            ComputeVelocity(new Vector2(0f, 0f), new Vector2(-deceleration, 0), new Vector2(currentVelocity.x, 0));
           
        }
        else
        {
            float maxAccel = (contactHanlder.Contacts.Bottom ? movAccelMax : airMovAccelMax);
            float maxSpeed = (contactHanlder.Contacts.Bottom ? movSpeedMax : airMovSpeedMax);
            if (!contactHanlder.Contacts.Bottom && _dir.y < 0)
            { //can fall aster only is press y down side and on the air 
                ComputeVelocity(new Vector2(maxSpeed, float.PositiveInfinity), new Vector2(maxAccel, fallingAccel), _dir);
                return;
            }       
            ComputeVelocity(new Vector2(maxSpeed,0), new Vector2(maxAccel,0), new Vector2(_dir.x , 0 ));  //only x movment
            
        }
    }
    /** Function to call when we chant the player to grab wall 
     */
    public void StartGrab()
    {
        isGrabing = true;
        startGrabing = true;
    }
    /** Function to call when we want the player stop grabing the wall 
     */
    public void EndGrab()
    {
        isGrabing = false;
    }

    public void RecoverEnergie()
    {
        float energieIncreced = currentEnergie + energieRecoverPerTime * Time.deltaTime;
        currentEnergie = energieIncreced < maximuEnergie ? energieIncreced : maximuEnergie;
    }
    public void DecreceEnergie()
    {
        float energieDecreced = currentEnergie - energieDecresePerTime * Time.deltaTime;
        currentEnergie = energieDecreced > 0 ? energieDecreced : 0;
    }

    /** Handel the grab complicated by the fact that gravity comme from this update when grab comme frome imputcontroler update 
     */ 
    public void HandelGrab()
    {
        if (isGrabing &&        // want to grab
            currentEnergie > energieDecresePerTime * Time.deltaTime    // had enouth energie 
            && (contactHanlder.Contacts.Left || contactHanlder.Contacts.Right)  //is close to a wall
            //&& !contactHanlder.Contacts.Bottom                                    // is't close to ground 
            ) //wall grab 
        {
            DecreceEnergie();
            //Debug.Log("Eneegie decrease :" + currentEnergie);
            if (startGrabing)
            {
                currentVelocity.y = 0;
                startGrabing = false;
            }
            else
            {
                currentVelocity.y += gravityAccel * Time.deltaTime; //compensate gravity
            }
            ComputeVelocity(new Vector2(0, wallGrabFallingAccel), new Vector2(0, wallGrabFallingAccel), new Vector2(0, direction.y)); //the player can slide along the wall           
        }
        else
        {
            if (contactHanlder.Contacts.Bottom)     // only recover energi if grounded 
            {
                //Debug.Log("Eneegie Recover:  " + currentEnergie);
                RecoverEnergie();
            }
            if (!(contactHanlder.Contacts.Left || contactHanlder.Contacts.Right)) //betwin 2 wall and holding grab
            {
                startGrabing = true;
            }
        }
        if(currentEnergie < maximuEnergie / 3)
        {
            lowEnergiFeedback();
        }
    }
    private void lowEnergiFeedback()
    {
        feedBackControler.Clignote(Color.red, Color.green, 1);
    }

    /**
     * Computes the horizontal velocity of the character.
     * \param targetVelocity    The absolute velocity the character has to reach.
     * \param acceleration      The maximum accelration of the character to reach the target velocity. If negative, the player will decelerate.
     * \param direction         The direction the character is moving, towards positive x if positive, towards negative x otherwise. 
     *                          Value has no effect.
     */
    void ComputeVelocity(Vector2 targetVelocity, Vector2 acceleration, Vector2 direction)
    {
        // Making sure the value of direction doesn't affect our calculations
        Vector2 normedDirection = new Vector2(Math.Sign(direction.x), Math.Sign(direction.y));
       
        Vector2 signedAcceleration = normedDirection * acceleration;
        Vector2 signedTargetVelocity = normedDirection * targetVelocity;

        Vector2 computedVelocity = currentVelocity + signedAcceleration * Time.deltaTime;
        //handel X velocity
      
        if(normedDirection.x != 0) { //modifie volocity only if we have an order on this direction
            // If the player accelrates or deceleartes but hasn't reached targetVelocity, we apply computedVelocity
            if ((signedAcceleration.x > 0 && computedVelocity.x < signedTargetVelocity.x) || (signedAcceleration.x < 0 && computedVelocity.x > signedTargetVelocity.x))
            {
                currentVelocity.x = computedVelocity.x;
            }
            // Else the player has already reached targetVelocity and we make sure it stays at that targetVelocity
            else
            {
                currentVelocity.x = signedTargetVelocity.x;
            }
        }
        //Handel Y Velocity
        
        if(normedDirection.y != 0 ) {  //modifie volocity only if we have an order on this direction
            if ((signedAcceleration.y > 0 && computedVelocity.y < signedTargetVelocity.y) || (signedAcceleration.y < 0 && computedVelocity.y > signedTargetVelocity.y))
            {
                currentVelocity.y = computedVelocity.y;
            }
            // Else the player has already reached targetVelocity and we make sure it stays at that targetVelocity
            else
            {
                currentVelocity.y = signedTargetVelocity.y;
            }
        }
    }

    /**
     * Add the effect of gravity to current velocity.
     */
    void ComputeGravity()
    {
        currentVelocity += Vector2.down * gravityAccel * Time.deltaTime;
    }

    /**
     * Handle collisions and snaps the player to walls and platforms.
     */
    void HandleCollisions()
    {
        RaycastHit2D[] raycastHits = new RaycastHit2D[4];
        int num = playerRigidbody.Cast(currentVelocity.normalized, filter, raycastHits, currentVelocity.magnitude * speedFactor * Time.deltaTime);

        for(int i = 0; i < num; ++i)
        {
            if (Vector2.Dot(currentVelocity, raycastHits[i].normal) < 0)
            {
                transform.position += new Vector3(currentVelocity.normalized.x, currentVelocity.normalized.y) * raycastHits[i].distance;
                currentVelocity -= raycastHits[i].normal * Vector2.Dot(currentVelocity, raycastHits[i].normal);
            }
        }
    }

    public void Jump() // jump if the player is grounder and start a timer for the jump
    {
        if (!canJum)
            return;
        if (contactHanlder.Contacts.Bottom && canJumpBottom)
        {
            canJumpBottom = false;
            currentVelocity += Vector2.up * initialJumpAccel;
            StartCoroutine(JumpCoroutineBottom());
            feedBackControler.PlayJumpSound();
        }
        else if (contactHanlder.Contacts.Left && canJumpLeft)
        {
            canJumpLeft = false;
            Debug.Log("prevelo : " + currentVelocity);
            
            Debug.Log("postvelo:" + currentVelocity);
            StartCoroutine(JumpCoroutineLeft());
            feedBackControler.PlayJumpSound();
        }
        else if(contactHanlder.Contacts.Right && canJumpRight)
        {
            canJumpRight = false;
            StartCoroutine(JumpCoroutineRight());
            feedBackControler.PlayJumpSound();
        } 
    }

    IEnumerator JumpCoroutineBottom() //Jump timer 
    {
        //Counts for how long we've been jumping
        yield return new WaitForSeconds(jumpDelay); // wait jumpDelay second 
        canJumpBottom = true;
        yield return null;
    }

    IEnumerator JumpCoroutineLeft() //Jump timer 
    {
        canJumpLeft = false;
        float t = Time.time;
        while (Time.time - t < wallJumpToleranceTime && direction == Vector2.zero)
        {
            yield return null;
        }
        t = Time.time - t;
        currentVelocity = currentVelocity.x * Vector2.right + new Vector2((0.2f + direction.x) * sideJumpAccel.x, sideJumpAccel.y);

        canMove = false;
        //Counts for how long we've been jumping
        yield return new WaitForSeconds(disableControlDelay - t); // wait jumpDelay second 
        canMove = true;

        yield return new WaitForSeconds(jumpSideDelay - disableControlDelay - t);
        canJumpLeft = true;
        yield return null;
    }

    IEnumerator JumpCoroutineRight() //Jump timer 
    {
        canJumpRight = false;
        float t = Time.time;
        while (Time.time - t < wallJumpToleranceTime && direction == Vector2.zero)
        {
            yield return null;
        }
        t = Time.time - t;
        currentVelocity = currentVelocity.x * Vector2.right + new Vector2((-0.2f + direction.x) * sideJumpAccel.x, sideJumpAccel.y);

        canMove = false;
        //Counts for how long we've been jumping
        yield return new WaitForSeconds(disableControlDelay - t); // wait jumpDelay second 
        canMove = true;

        yield return new WaitForSeconds(jumpSideDelay - disableControlDelay - t);
        canJumpRight = true;
        yield return null;
    }

    public void Dash(Vector2 _dir)
    {
        if (!canDash || _dir.x == 0 && _dir.y == 0)  // if dir x and dir y ar equal to 0 it's not realy a dash 
        {
            //Debug.Log("Cant dash");
            return;
        }
        else
        {
            canDash = false;
            if ( currentVelocity.y <0 && _dir.y > 0) // we want to dash a she same higth when faling and we dont 
            {
                currentVelocity.y = 0; 
            }
            Vector2 diagonalreduction = _dir.y != 0 ? new Vector2(reductiondiage, 1) : new Vector2(1, 1); // angle on diagonal dash to go to far 

            currentVelocity +=  _dir * initialDashAccel * diagonalreduction ;
            suplementarytime = _dir.y != 0 && _dir.x != 0 ? suplementarytime = diagonalDashSuplementaryDelay: 0; // prevent weard angle on diagonal dash
            
            StartCoroutine(Dashduration(dashDuration + suplementarytime ));
            dashFeedback();
            StartCoroutine(DashRecoverCoroutine());

            
           // StartCoroutine(JumpCoroutine());
        }
    }
    /** Function to call to generat feedback for the dash 
     * 
     */
    void dashFeedback()
    {
        feedBackControler.PlayDashSound();
        feedBackControler.ActivateTrail(1);
        feedBackControler.CameraSharke();
        feedBackControler.InstanciateDashPrefabOnPosition(transform.position + new Vector3(0, transform.localScale.y, 0) / 2);
        feedBackControler.ChangeToBlue();
    }
    bool isRecovering = false;
    public void Respawn(Vector2 position,float incapacityTime)
    {
        
        this.transform.position = position;
        this.currentVelocity = Vector2.zero;
        StartCoroutine(Preventmoving(incapacityTime));
        FeedBackRespawn(incapacityTime); 
    }

    void FeedBackRespawn(float duration)
    {
        feedBackControler.CameraSharke(1);
        feedBackControler.Clignote(Color.red, Color.white,duration);
    }
    IEnumerator Preventmoving(float duration)
    {
        canMove = false;
        canJum = false;
        canDash = false;
        yield return new WaitForSeconds(duration);
        //feedBackControler.ChangeToRed();
        canDash = true;
        canJum = true;
        canMove = true;
    }




    /**
     * to call when speed is unlimited 
     */
    IEnumerator Dashduration(float duration )
    {
        movSpeedMax += initialDashAccel;            //prevent from clamping the sped for the dash 
        airMovSpeedMax += initialDashAccel;
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
        movSpeedMax -= initialDashAccel;            //prevent from clamping the sped for the dash 
        airMovSpeedMax -= initialDashAccel;
    }

    IEnumerator DashRecoverCoroutine()
    {
        yield return new WaitForSeconds(dashDelay); // wait for dashdelaysecond
        yield return new WaitWhile(() => !contactHanlder.Contacts.Bottom); // wait until contact bottom = true 
        canDash = true;
        feedBackControler.ChangeToRed();
        //canJump = true;
        yield return null;
    }

    /** make the player bounce but not working ;
     */
    public void Bouncing()
    {
        if(contactHanlder.Contacts.Bottom || contactHanlder.Contacts.Top)
        {
            this.currentVelocity.y = -currentVelocity.y;
        }
        if (contactHanlder.Contacts.Left || contactHanlder.Contacts.Right)
        {
            this.currentVelocity.x = -currentVelocity.x;
        }
    }
    /** Take over ... [not working]
     * 
     */
    public void TakeOver(float xSpeed )
    {

        Debug.LogWarning("Take over");
        if (contactHanlder.Contacts.Bottom)
        {
            Debug.LogWarning("take over + contact " + xSpeed);
            Debug.LogWarning("prev"+currentVelocity);
            this.currentVelocity.x += xSpeed * 10;
            Debug.LogWarning("post" + currentVelocity);
        }
    }


    public void UpdateDirection(Vector2 _dir)
    {
        direction = _dir;
    }

    public void SlowDown(float speedFactor)
    {
        this.speedFactor = speedFactor;
    }

    public void ApplySpeedFactor()
    {
        currentVelocity *= speedFactor;
    }

}