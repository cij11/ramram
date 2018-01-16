using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : MonoBehaviour {

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
            otherCar.SetSliding(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        CarModel otherCar = collider.GetComponent<CarModel>() as CarModel;

        if (otherCar != null)
        {
            otherCar.SetSliding(false);
        }
    }
}
