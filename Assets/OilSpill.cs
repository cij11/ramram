using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : MonoBehaviour {
    float maxLifespan = 10f;
    float lifespanTimer = 0f;
	// Use this for initialization
	void Start () {
        lifespanTimer = maxLifespan;
	}
	
	// Update is called once per frame
	void Update () {
        lifespanTimer -= Time.deltaTime;
        if (lifespanTimer < 0)
        {
            Destroy(this.gameObject);
        }
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
