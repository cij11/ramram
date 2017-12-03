using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    int[] scores = new int[] { 0, 0, 0, 0 };
    public TextMesh[] scoreBoards;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; i++)
        {
            scoreBoards[i].text = "" + scores[i];
        }
	}

    public void SetPlayerScore(int player, int score)
    {
        scores[player] = score;
    }

    public void AddPoint(int player)
    {
        scores[player] = scores[player] + 1;
    }

    public void SubtractPoint(int player)
    {
        scores[player] = scores[player] - 1;
    }
}
