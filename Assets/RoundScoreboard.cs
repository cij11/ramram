using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoundScoreboard : MonoBehaviour {

    Text[] scoreText;
	// Use this for initialization
	void Start () {
        scoreText = new Text[4];

        for (int i = 0; i < 4; i++)
        {
            scoreText[i] = this.transform.GetChild(i + 1).GetComponent<Text>() as Text;
        }
    }

    public void SetScores(int[] scores)
    {
        for (int i = 0; i < 4; i ++)
        {
            scoreText[i].text = scores[i].ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
