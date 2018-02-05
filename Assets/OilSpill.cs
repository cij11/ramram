using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : MonoBehaviour {
    float maxLifespan = 10f;
    float lifespanTimer = 0f;

    public bool isGlue = false;

    public bool hasLifespan = true;
	// Use this for initialization
	void Start () {
        lifespanTimer = maxLifespan;
	}
	
	// Update is called once per frame
	void Update () {
        ManageLifespan();

	}

    void ManageLifespan()
    {
        if (hasLifespan)
        {
            lifespanTimer -= Time.deltaTime;
            if (lifespanTimer < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        CarModel otherCar = collider.GetComponent<CarModel>() as CarModel;

        if (otherCar != null)
        {
            if (!isGlue)
            {
                otherCar.SetSliding(true);
            }
            else
            {
                otherCar.SetStuck(true);
            }
        }
    }
}
