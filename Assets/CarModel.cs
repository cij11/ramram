using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModel : MonoBehaviour {

    Rigidbody body;
    Material mat;

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

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>() as Rigidbody;
        mat = GetComponent<Material>() as Material;
	}
	
	// Update is called once per frame
	void Update () {

        //Check if the car is below limits, then respawn
        ManageRespawn();

        accelerateAxis = "Accelerate" + playerNumber;
        steerAxis = "Steer" + playerNumber;

        ManageIdle(accelerateAxis, steerAxis);

        if (Input.GetAxis(accelerateAxis) > 0)
        {
            body.AddForce(body.transform.up * enginePower);
        }
        if (Input.GetAxis(accelerateAxis) < 0)
        {
            body.AddForce(body.transform.up * -enginePower);
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

    private void ManageRespawn()
    {
        if (body.transform.position.y < lavaHeight && !isIdle)
        {
            body.transform.position = new Vector3(0, 0, 5f);
         //   body.velocity = new Vector3(0f, 0f, 0f);
        }
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

        print(isIdle);
    }

    public void PushCar(Vector3 pushDirection, float pushPower)
    {
        body.AddForce(pushDirection * pushPower);
    }

    //Returns the rotational speed of the car
    float getTurnSpeed()
    {
        return body.angularVelocity.z;
    }



}
