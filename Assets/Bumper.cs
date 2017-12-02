using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    float collisionPower = 1000f;

	// Use this for initialization
	void Start () {
		
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

            otherCar.PushCar(vecToOtherCar, collisionPower);
        }
    }
}
