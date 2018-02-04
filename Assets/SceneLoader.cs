using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public bool startInMainMenu = true;
    public int first_scene = 1;

	// Use this for initialization
	void Start () {
        if (this.startInMainMenu)
        {
            SceneManager.LoadScene("menus");
        }
        else
        {
            SceneManager.LoadScene("scene_" + first_scene.ToString());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
