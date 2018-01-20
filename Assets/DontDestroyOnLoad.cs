using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        print("DDOL Loaded");
    }
}
