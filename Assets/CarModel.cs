using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RammingState
{
    RAM_READY,
    RAM_COOLDOWN
}
public class CarModel : MonoBehaviour {

    Rigidbody body;
    Material mat;
    
    public GameManager gameManager;

    RamSmokeController ramSmokeController;
    Bumper bumper;

	public float enginePower = 30f;
    public float defaultEnginePower = 30f;
    public float speedyEnginePower = 50f;

	public float turnPower = 9f;

	public float maxTurnSpeed = 5f;

    public int playerNumber = 0;

    float lavaHeight = -30f;
    float controlLossHeight = -1.0f;
    float respawnBoxRadius = 10.0f;
    Vector3 respawnAnchorPoint = new Vector3(-1, 5, 7);

    string accelerateAxis = "Accelerate0";
    string steerAxis = "Steer0";


	public float idleLimit = 10;
    public float idleTimer = 11;
    bool isIdle = true;

    int lastHitByPlayer = -1;
    float lastHitTimer = 0;
    float lastHitLimit = 15;

    bool isStagedForRespawning = true;

    float rammingSpeed = 30f;
    float ramTimer = -1f;
    float ramCooldown = 1.0f;

    float timeSinceForwardPressed = 0.0f;
    float oldTimeSinceForwardPressed = 1.0f;
    bool neutralLastFrame = true;
    float doubleTapLimit = 0.2f;
    bool doubleTap = false;

    public AudioClip ramClip;
    bool oldEngineOn = false;
    bool engineOn = false;
    float maxVolume = 1f;
    float minVolume = 0.05f;
    float fadeUpRate = 1f;
    float fadeDownRate = 2f;
    float currentVolume = 0;

    float groundLevel = 0.0f;
    float groundedRange = 0.5f;
    bool grounded = false;

    AudioSource engineSound;
    AudioSource ramSound;
    float pitchScaler = 0.75f;

    bool sliding = false;
    Vector3 slideDirection = new Vector3(1, 0, 0);

    private PowerupType activePower;
    private bool poweredUp = false;
    private float maxPowerupTimer = 5f;
    private float powerupTimer = 0f;

    float platformContactTimer = 10f;
    float platformContactForgiveness = 0.1f;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>() as Rigidbody;
        mat = GetComponent<Material>() as Material;

        gameManager = FindObjectOfType<GameManager>() as GameManager;

        engineSound = GetComponents<AudioSource>().GetValue(0) as AudioSource;
        ramSound = GetComponents<AudioSource>().GetValue(1) as AudioSource;

        ramSmokeController = GetComponentInChildren<RamSmokeController>() as RamSmokeController;

        engineSound.volume = 0.0f;
        engineSound.Play();

        bumper = GetComponentInChildren<Bumper>() as Bumper;
    }
	
	// Update is called once per frame
	void Update () {

        //Check if the car is below limits, then respawn
        ManageRespawn();

        CheckGrounded();

        accelerateAxis = "Accelerate" + playerNumber;
        steerAxis = "Steer" + playerNumber;

        ManageIdle(accelerateAxis, steerAxis);
        ManageJoinGame();
        ManageLastHits();
        UpdateDoubleTapTimers();
        ManageRamming();
        ManageTurning();
        ManageSounds();

        ManagePowerupTimers();

        if (this.grounded && !this.sliding)
        {
            ManageAcceleration();
        }

        if(this.sliding)
        {
            body.AddForce(this.slideDirection * enginePower);
        }
    }

    void CheckGrounded()
    {
        this.platformContactTimer += Time.deltaTime;

        this.grounded = false;
        if (this.platformContactTimer < this.platformContactForgiveness)
        {
            this.grounded = true;
        }
    }

    void ManageSounds()
    {
        if (Input.GetAxis(accelerateAxis) > 0 && !this.oldEngineOn)
        {
            
            this.oldEngineOn = true;
        }

        if(! (Input.GetAxis(accelerateAxis) > 0) && this.oldEngineOn)
        {
         //   engineSound.Stop();
            this.oldEngineOn = false;
        }

        if(this.oldEngineOn)
        {
            if(this.currentVolume < this.maxVolume)
            {
                this.currentVolume += Time.deltaTime * this.fadeDownRate;
            }
        }

        if(!this.oldEngineOn)
        {
            if (this.currentVolume > this.minVolume)
            {
                this.currentVolume -= Time.deltaTime * this.fadeUpRate;
            }
        }

        int maxPitch = 2;
        engineSound.volume = this.currentVolume;
        if (this.currentVolume > 1f/maxPitch)
        {
            engineSound.pitch = this.currentVolume * maxPitch * pitchScaler;
        }
        else
        {
            engineSound.pitch = 1.0f * pitchScaler;
        }
    }

    void ManageAcceleration()
    {
        if (Input.GetAxis(accelerateAxis) > 0)
        {
            //Accelerate care forwards
            body.AddForce(body.transform.up * enginePower);
        }

        if (Input.GetAxis(accelerateAxis) < 0)
        {
            body.AddForce(body.transform.up * -enginePower);
        }
    }
    void ManageTurning()
    {
        //If player presses turn counter-clockwise
        if (Input.GetAxis(steerAxis) > 0)
        {
            //Check that the current speed is greater than the maximum negative speed.
            //Do not let the car turn more negatively if it is already turning at its maximum negative speed.
            if (getTurnSpeed() < maxTurnSpeed)
            {
                body.AddTorque(body.transform.forward * turnPower);
            }
        }

        //If the player presses turn clockwise
        if (Input.GetAxis(steerAxis) < 0)
        {
            //Make the car spin clockwise if it is not already spinning at its maximum clockwise speed
            if (getTurnSpeed() > -maxTurnSpeed)
            {
                body.AddTorque(body.transform.forward * -turnPower);
            }
        }
    }

    private void RegisterTap()
    {
        oldTimeSinceForwardPressed = timeSinceForwardPressed;
        timeSinceForwardPressed = 0.0f;

        if (oldTimeSinceForwardPressed - timeSinceForwardPressed < doubleTapLimit)
        {
            doubleTap = true;
        }
    }

    private void ManageRamming()
    {
        if (Input.GetAxis(accelerateAxis) > 0)
        {
            if (neutralLastFrame)
            {
                RegisterTap();
            }
        }

            if (doubleTap)
        {
            //reset the double tap
            doubleTap = false;

            if (ramTimer < 0)
            {
                RamCar();
                ramSound.Play();
                ramTimer = ramCooldown;
            }
        }

        if (ramTimer > 0)
        {
            ramTimer -= Time.deltaTime;
        }

        neutralLastFrame = (Input.GetAxis(accelerateAxis) <= 0);
    }

    void UpdateDoubleTapTimers()
    {
        oldTimeSinceForwardPressed += Time.deltaTime;
        timeSinceForwardPressed += Time.deltaTime;
    }

    private void RamCar()
    {
        body.velocity = body.transform.up * rammingSpeed;
        ramSmokeController.ActivateSmoke(1f);
    }

    private void ManageRespawn()
    {
        if (!isStagedForRespawning) {
            if (body.transform.position.y < lavaHeight)
            {
                UpdateScores();
                isStagedForRespawning = true;
            }
        }
        else
        {
            body.transform.position = new Vector3(this.playerNumber * 2, 0, -100f);
            body.velocity = new Vector3(0, 0, 0);
            body.angularVelocity = new Vector3(0, 0, 0);

            if (!isIdle)
            {
                RespawnCar();
            }
        }
    }

    private void UpdateScores()
    {
        if (lastHitByPlayer == -1)
        { //If this is a suicide
            gameManager.SubtractPoint(playerNumber);
        }
        else
        {
            gameManager.AddPoint(lastHitByPlayer);
        }
    }

    private void RespawnCar()
    {
        Vector3 randomVector = new Vector3(Random.Range(-respawnBoxRadius, respawnBoxRadius), 0.0f, Random.Range(-respawnBoxRadius, respawnBoxRadius));
        body.transform.position = respawnAnchorPoint + randomVector;
        isStagedForRespawning = false;
    }

    private void ManageIdle(string accelerateAxis, string steerAxis)
    {
        if ((Input.GetAxis(accelerateAxis) != 0) || Input.GetAxis(steerAxis) != 0)
        {
            //idleTimer = 0;
            isIdle = false;
        }
   //     else
   //    {
   //         idleTimer += Time.deltaTime;
   //     }

        //No longer go idle during a game. Opt into game at beginning instead.
      //  isIdle = (idleTimer > idleLimit);
    }

    private void ManageJoinGame()
    {
        if(Input.GetAxis(steerAxis) != 0 || Input.GetAxis(accelerateAxis) != 0)
        {
            gameManager.RegisterPlayer(this.playerNumber);
        }
    }

    private void ManageLastHits()
    {
        lastHitTimer += Time.deltaTime;
        if (lastHitTimer >= lastHitLimit)
        {
            lastHitTimer = lastHitLimit;
            lastHitByPlayer = -1;
        }
    }

    public void PushCar(Vector3 pushDirection, float pushPower)
    {
        body.AddForce(pushDirection * pushPower);
    }

    public void RegisterHitByPlayer(int hittingPlayer)
    {
        lastHitByPlayer = hittingPlayer;
        lastHitTimer = 0;
    }

    //Returns the rotational speed of the car
    float getTurnSpeed()
    {
        return body.angularVelocity.z;
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }
	public Vector3 GetCarVelocity()
	{
		return body.velocity;
	}

    public void SetSliding (bool isSliding) {
        this.sliding = isSliding;

        Vector3 currentDirection = body.velocity;
        if(currentDirection.magnitude < 0.01f)
        {
            currentDirection = new Vector3(1f, 0f, 1f);
        }

            this.slideDirection = currentDirection.normalized;



    }

    public void ApplyPowerup(PowerupType powerup)
    {
        RemovePowerups();
        this.powerupTimer = this.maxPowerupTimer;

        switch (powerup)
        {
            case PowerupType.SPEEDY:
                {
                    this.enginePower = this.speedyEnginePower;
                    ramSmokeController.ActivateSmoke(maxPowerupTimer);

                }
                break;
            case PowerupType.WIDE:
                {
                    SetGraphicAndBumperScale(2);
                }
                break;
            case PowerupType.SUPERRAM:
                {
                    bumper.setBoostedCollisionPower(true);
                    SetGraphicHeight(2);
                }
                break;
        }
    }

    private void RemovePowerups()
    {
        this.enginePower = this.defaultEnginePower;
        SetGraphicAndBumperScale(1);
        bumper.setBoostedCollisionPower(false);
        SetGraphicHeight(1);
    }

    private void SetGraphicAndBumperScale(float scale)
    {
        Transform car_full = this.transform.GetChild(0);
        car_full.transform.localScale = new Vector3(scale, 1f, 1f);

        Transform bumper = this.transform.GetChild(1);
        SphereCollider bumperCollider = bumper.GetComponent<SphereCollider>() as SphereCollider;
        bumperCollider.radius = 0.8f * scale;
    }

    private void SetGraphicHeight(float scale)
    {
        Transform car_full = this.transform.GetChild(0);
        car_full.transform.localScale = new Vector3(1f, scale, 1f);
    }

    private void ManagePowerupTimers()
    {
        if (this.powerupTimer > 0)
        {
            this.powerupTimer -= Time.deltaTime;

            if (this.powerupTimer < 0)
            {
                RemovePowerups();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject hit = collision.gameObject;
        //Is this a platform?
        if (hit.tag == "platform")
        {
            // Is this the flat upper surface of the platform?
            bool flatSurface = false;

            foreach (ContactPoint contact in collision.contacts)
            {
                print("Platform point = " + contact.point.y);
                print("Body point = " + body.transform.position.y);

                if (contact.point.y < body.transform.position.y - 0.1f)
                {
                    flatSurface = true;
                }
            }

            if (flatSurface) {
                platformContactTimer = 0.0f;
            }
        }
    }
}
