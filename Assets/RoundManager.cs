﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RoundState
{
    WAITING_FOR_PLAYERS, START_COUNTDOWN, PLAYING, ENDING
}

public class RoundManager : MonoBehaviour {
    Text roundText;
        
    RoundState roundState = RoundState.WAITING_FOR_PLAYERS;
    int minNumPlayers = 2;

    float startCountdownTimer = 0;
    float maxStartCountdown = 5;

    float endingTimer = 0;
    float maxEndingCountdown = 5;

    public bool[] playerJoined = { false, false, false, false };

    int winningPlayer = -1;

    int scene_to_load = 0;

    public bool loadRandomScene = true;
    int numScenes = 2;

	// Use this for initialization
	void Start () {
        roundText = this.GetComponentInChildren<Text>() as Text;
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (this.roundState)
        {
            case RoundState.WAITING_FOR_PLAYERS:
                {
                    if (NumPlayers() >= minNumPlayers)
                    {
                        roundState = RoundState.START_COUNTDOWN;
                        this.startCountdownTimer = this.maxStartCountdown;

                    }
                    roundText.text = "Press Accelerate to join Game!";
                    break;
                }
            case RoundState.START_COUNTDOWN:
                {
                    this.startCountdownTimer -= Time.deltaTime;

                    if (this.startCountdownTimer <= 0)
                    {
                        roundState = RoundState.PLAYING;
                        this.startCountdownTimer = 0;
                    }
                    roundText.text = "Game Starts in: " + Math.Round(this.startCountdownTimer).ToString() + "...";
                    break;
                }
            case RoundState.PLAYING:
                {
                    roundText.text = "";
                    break;
                }
            case RoundState.ENDING:
                {
                    String winningColor = "Blue";

                    switch(this.winningPlayer)
                    {
                        case 0:
                            winningColor = "Red";
                            break;
                        case 1:
                            winningColor = "Blue";
                            break;
                        case 2:
                            winningColor = "Green";
                            break;
                        case 3:
                            winningColor = "Yellow";
                            break;
                    }
                    roundText.text = "Winner: Player " + winningColor + "!\nNext round in " + Math.Round(this.endingTimer).ToString() + " Seconds...";
                    this.endingTimer -= Time.deltaTime;
                    if (this.endingTimer <= 0)
                    {
                        StartRound();
                    }
                    break;
                }
        }
	}

    void StartRound()
    {
        for (int i = 0; i < 4; i++)
        {
            this.playerJoined[i] = false;
        }

        this.roundState = RoundState.WAITING_FOR_PLAYERS;


        int nextScene = scene_to_load;
        if (loadRandomScene)
        {
            nextScene = UnityEngine.Random.Range((int)0, numScenes);

        }
        SceneManager.LoadScene("scene_" + nextScene.ToString());
    }

    void EndRound()
    {

    }

    public void RegisterPlayer(int playerNumber)
    {
        if (!playerJoined[playerNumber])
        {
            playerJoined[playerNumber] = true;
        }
    }

    private int NumPlayers()
    {
        int numPlayers = 0;
        for (int i = 0; i < 4; i++)
        {
            if (playerJoined[i])
            {
                numPlayers++;
            }
        }
        return numPlayers;
    }

    public bool IsGamePlaying()
    {
        return (this.roundState == RoundState.PLAYING);
    }

    public bool IsPlayerPlayering(int playerNumber)
    {
        return playerJoined[playerNumber];
    }

    public void SetWinningPlayer(int winningPlayer)
    {
        this.winningPlayer = winningPlayer;
        this.roundState = RoundState.ENDING;
        this.endingTimer = this.maxEndingCountdown;
    }
}
