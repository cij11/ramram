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

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>() as Rigidbody;
        mat = GetComponent<Material>() as Material;
	}
	
	// Update is called once per frame
	void Update () {

        //Check if the car is below limits, then respawn
        ManageRespawn();

        if (playerNumber == 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                body.AddForce(body.transform.up * enginePower);
            }

            if (Input.GetKey(KeyCode.S))
            {
                body.AddForce(body.transform.up * -enginePower);
            }

            //If player presses turn counter-clockwise
            if (Input.GetKey(KeyCode.D))
            {
                //Check that the current speed is greater than the maximum negative speed.
                //Do not let the car turn more negatively if it is already turning at its maximum negative speed.
                if (getTurnSpeed() > -maxTurnSpeed)
                {
                    body.AddTorque(body.transform.forward * -turnPower);
                }
            }

            //If the player presses turn clockwise
            if (Input.GetKey(KeyCode.A))
            {
                //Make the car spin clockwise if it is not already spinning at its maximum clockwise speed
                if (getTurnSpeed() < maxTurnSpeed)
                {
                    body.AddTorque(body.transform.forward * turnPower);
                }
            }
        }

        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                body.AddForce(body.transform.up * enginePower);
            }

            if (Input.GetKey(KeyCode.Keypad5))
            {
                body.AddForce(body.transform.up * -enginePower);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                if (getTurnSpeed() > -maxTurnSpeed)
                {
                    body.AddTorque(body.transform.forward * -turnPower);
                }
            }

            if (Input.GetKey(KeyCode.Keypad4))
            {
                if (getTurnSpeed() < maxTurnSpeed)
                {
                    body.AddTorque(body.transform.forward * turnPower);
                }
            }
        }

        
    }

    private void ManageRespawn()
    {
        if (body.transform.position.z < lavaHeight)
        {
            body.transform.position = new Vector3(0, 0, 5f);
         //   body.velocity = new Vector3(0f, 0f, 0f);
        }
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
