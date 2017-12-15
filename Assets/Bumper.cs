using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

	float reboundSpeed = 1000f;
    float collisionPower = 50f;
	float minimumCollisionSpeed = 1000f;
    int playerNumber = 0;
	CarModel carModel;
	// Use this for initialization
	void Start () {
        carModel = this.transform.GetComponentInParent<CarModel>() as CarModel;
        playerNumber = carModel.GetPlayerNumber();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        CarModel otherCar = collider.GetComponent<CarModel>() as CarModel;

        if (otherCar != null)
        {
            print("Collision Detected");

            //  Destroy(otherCar.gameObject);
            Vector3 vecToOtherCar = otherCar.transform.position - this.transform.position;
            vecToOtherCar.Normalize();

			carModel.PushCar (-vecToOtherCar, reboundSpeed);
			float carspeed = carModel.GetCarVelocity().magnitude;
			float bounceSpeed = carspeed * collisionPower;
			if (bounceSpeed < minimumCollisionSpeed) bounceSpeed = minimumCollisionSpeed;

			otherCar.PushCar(vecToOtherCar, bounceSpeed);
            otherCar.RegisterHitByPlayer(playerNumber);
        }
    }
}
