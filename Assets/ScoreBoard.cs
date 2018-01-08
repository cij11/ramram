using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

    int[] scores = new int[] { 0, 0, 0, 0 };
    public Text[] scoreBoards;

    public RoundManager roundManager;

    public int scoreLimit = 15;

	// Use this for initialization
	void Start () {
        roundManager = FindObjectOfType<RoundManager>() as RoundManager;

        this.scoreBoards = new Text[4];
        this.scoreBoards[0] = GameObject.FindGameObjectWithTag("player0score").GetComponent<Text>() as Text;
        this.scoreBoards[1] = GameObject.FindGameObjectWithTag("player1score").GetComponent<Text>() as Text;
        this.scoreBoards[2] = GameObject.FindGameObjectWithTag("player2score").GetComponent<Text>() as Text;
        this.scoreBoards[3] = GameObject.FindGameObjectWithTag("player3score").GetComponent<Text>() as Text;
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
        if (roundManager.IsGamePlaying())
        {
            scores[player] = scores[player] + 1;
            if(scores[player] >= this.scoreLimit)
            {
                NotifyRoundManagerOfVictory(player);
            }
        }
    }

    public void SubtractPoint(int player)
    {
        if (roundManager.IsGamePlaying())
        {
            scores[player] = scores[player] - 1;
        }
    }

    private void NotifyRoundManagerOfVictory(int winningPlayer)
    {
        roundManager.SetWinningPlayer(winningPlayer);
    }
}
