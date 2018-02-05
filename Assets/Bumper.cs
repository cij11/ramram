using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

	float reboundSpeed = 500f;
    float collisionPower = 50f;
    float defaultCollisionPower = 50f;
    float boostedCollisionPower = 200f;

	float minimumCollisionSpeed = 1000f;
    int playerNumber = 0;
	CarModel carModel;

    AudioSource collisionSound; 

	// Use this for initialization
	void Start () {
        carModel = this.transform.GetComponentInParent<CarModel>() as CarModel;
        playerNumber = carModel.GetPlayerNumber();

        collisionSound = GetComponent<AudioSource>() as AudioSource;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        CarModel otherCar = collider.GetComponent<CarModel>() as CarModel;

        if (otherCar != null)
        {

            //  Destroy(otherCar.gameObject);
            Vector3 vecToOtherCar = otherCar.transform.position - this.transform.position;
            vecToOtherCar.Normalize();


			carModel.PushCar (-carModel.transform.up, reboundSpeed);
			float carspeed = carModel.GetCarVelocity().magnitude;
			float bounceSpeed = carspeed * collisionPower;
			if (bounceSpeed < minimumCollisionSpeed) bounceSpeed = minimumCollisionSpeed;

			otherCar.PushCar(vecToOtherCar, bounceSpeed);
            otherCar.RegisterHitByPlayer(playerNumber);

            collisionSound.Play();
        }
    }

    public void setBoostedCollisionPower(bool isBoosted)
    {
       if (isBoosted)
        {
            this.collisionPower = this.boostedCollisionPower;
        }
       else
        {
            this.collisionPower = this.defaultCollisionPower;
        }
    }
}
