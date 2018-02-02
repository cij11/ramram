using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidTrigger : MonoBehaviour {

    public GameObject skidParentPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartSkid ()
    {
        CreateSkidParent();
    }
    
    public void StopSkid()
    {
        DetachSkidParent();
    }

    public void DetachSkidParent()
    {
        GameObject skidParent = this.transform.GetChild(0).gameObject;
        Destroy(skidParent, 10f);

        this.transform.DetachChildren();
    }

    public void CreateSkidParent()
    {
        GameObject skidParent = Instantiate(skidParentPrefab, this.transform);
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject collidedWith = collider.gameObject;
        if (collidedWith.tag == "platform")
        {
            StartSkid();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        GameObject collidedWith = collider.gameObject;
        if (collidedWith.tag == "platform")
        {
            StopSkid();
        }
    }
}
