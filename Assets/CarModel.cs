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
    
    public ScoreBoard scoreBoard;

    float enginePower = 30f;
    float turnPower = 9f;

    float maxTurnSpeed = 5f;

    public int playerNumber = 0;

    float lavaHeight = -30f;

    string accelerateAxis = "Accelerate0";
    string steerAxis = "Steer0";


    float idleTimer = 0;
    float idleLimit = 10;
    bool isIdle = false;

    int lastHitByPlayer = -1;
    float lastHitTimer = 0;
    float lastHitLimit = 15;

    bool isStagedForRespawning = false;

    float rammingSpeed = 40f;
    float ramTimer = -1f;
    float ramCooldown = 5.0f;

    float timeSinceForwardPressed = 0.0f;
    float timeSinceForwardNeutral = 0.0f;
    float doubleTapLimit = 0.2f;


    
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>() as Rigidbody;
        mat = GetComponent<Material>() as Material;

        scoreBoard = FindObjectOfType<ScoreBoard>() as ScoreBoard;
	}
	
	// Update is called once per frame
	void Update () {

        //Check if the car is below limits, then respawn
        ManageRespawn();

        accelerateAxis = "Accelerate" + playerNumber;
        steerAxis = "Steer" + playerNumber;

        ManageIdle(accelerateAxis, steerAxis);
        ManageLastHits();
        UpdateDoubleTapTimers();
        ManageRamming();

            if (Input.GetAxis(accelerateAxis) > 0)
            {
               timeSinceForwardPressed = 0.0f;
                body.AddForce(body.transform.up * enginePower);
            }
            if (Input.GetAxis(accelerateAxis) < 0)
            {
                body.AddForce(body.transform.up * -enginePower);
            }

        if (Input.GetAxis(accelerateAxis) <= 0)
        {
            timeSinceForwardNeutral = 0.0f;
        }

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

    private void ManageRamming()
    {
        bool doubleTapDetected = checkDoubleTaps();

        if (doubleTapDetected)
        {
            if (ramTimer < 0)
            {
                RamCar();
                ramTimer = ramCooldown;
            }
        }

        if (ramTimer > 0)
        {
            ramTimer -= Time.deltaTime;
        }
    }

    void UpdateDoubleTapTimers()
    {
        timeSinceForwardNeutral += Time.deltaTime;
        timeSinceForwardPressed += Time.deltaTime;
    }

    bool checkDoubleTaps()
    {
        if (Input.GetAxis(accelerateAxis) > 0)
        {
            if (timeSinceForwardNeutral < doubleTapLimit)
            {
                if ( (timeSinceForwardPressed < doubleTapLimit) && (timeSinceForwardPressed > timeSinceForwardNeutral ) ) {
                    return true;
                }
            }
        }
        return false;
    }

    private void RamCar()
    {
        body.velocity = body.transform.up * rammingSpeed;
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
            body.transform.position = new Vector3(0, 0, -100f);
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
            scoreBoard.SubtractPoint(playerNumber);
        }
        else
        {
            scoreBoard.AddPoint(lastHitByPlayer);
        }
    }

    private void RespawnCar()
    {
        body.transform.position = new Vector3(0, 0, 5f);
        isStagedForRespawning = false;
    }

    private void ManageIdle(string accelerateAxis, string steerAxis)
    {
        if ((Input.GetAxis(accelerateAxis) != 0) || Input.GetAxis(steerAxis) != 0)
        {
            idleTimer = 0;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }

        isIdle = (idleTimer > idleLimit);
    }

    private void ManageLastHits()
    {
        lastHitTimer += Time.deltaTime;
        if (lastHitTimer >= lastHitLimit)
        {
            lastHitTimer = lastHitLimit;
            lastHitByPlayer = -1;
        }

        print("Player: " + playerNumber + " last hit by player: " + lastHitByPlayer);
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

}
