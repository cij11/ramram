using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroyExplosion", 2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DestroyExplosion()
    {
        Destroy(this.gameObject);
    }
}
