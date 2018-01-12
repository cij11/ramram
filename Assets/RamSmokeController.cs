using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSmokeController : MonoBehaviour {

    ParticleSystem ramSmoke;
	// Use this for initialization
	void Start () {
        this.ramSmoke = GetComponent<ParticleSystem>() as ParticleSystem;
        this.ramSmoke.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateSmoke()
    {
        this.ramSmoke.Play();
        Invoke("DeactivateSmoke", 1);
    }

    private void DeactivateSmoke()
    {
        this.ramSmoke.Stop();
    }
}
