using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSmokeController : MonoBehaviour {

    ParticleSystem ramSmoke;

    float smokeTimer = 0f;
    float maxSmokeTimer = 1f;
	// Use this for initialization
	void Start () {
        this.ramSmoke = GetComponent<ParticleSystem>() as ParticleSystem;
        this.ramSmoke.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		if (smokeTimer > 0)
        {
            smokeTimer -= Time.deltaTime;
            if (smokeTimer < 0)
            {
                DeactivateSmoke();
            }
        }
	}

    public void ActivateSmoke(float smokeDuration)
    {
        this.ramSmoke.Play();
        if (smokeTimer < smokeDuration)
        {
            smokeTimer = smokeDuration;
        }
    }

    private void DeactivateSmoke()
    {
        this.ramSmoke.Stop();
    }
}
