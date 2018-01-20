using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour {

    public int first_scene = 1;

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("scene_" + first_scene.ToString());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
