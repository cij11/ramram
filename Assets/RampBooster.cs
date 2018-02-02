using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBooster : MonoBehaviour {

    public float boostSpeed = 60.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider collider)
    {

        print("Ramp collision detected");

        Rigidbody otherBody = collider.attachedRigidbody;

        Vector3 rampDirection = this.transform.forward;

        float dotProduct = Vector3.Dot(rampDirection, otherBody.velocity);

        if (dotProduct > 1)
        {
            otherBody.velocity = rampDirection * boostSpeed;
        }
    }

}
