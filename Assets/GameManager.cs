using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    MAIN_MENU,
    READY_ROOM,
    ROUND_PLAYING,
    ROUND_OVER,
    TOURNAMENT_OVER
}
public class GameManager : MonoBehaviour {

    public int[] roundScores = { 0, 0, 0, 0 };
    public int[] tournamentScores = { 0, 0, 0, 0 };

    public int roundLimit = 2;
    public int tournamentLimit = 2;

    Canvas mainMenu;
    Canvas readyRoom;
    Canvas roundScoreBoard;
    Canvas roundOver;
    Canvas tournamentOver;

    RoundScoreboard roundScoreboardScript;

    private GameState gameState;
    // Use this for initialization
    void Start () {
        gameState = GameState.MAIN_MENU;

        mainMenu = this.transform.GetChild(0).GetComponent<Canvas>() as Canvas;
        readyRoom = this.transform.GetChild(1).GetComponent<Canvas>() as Canvas;

        roundScoreBoard = this.transform.GetChild(2).GetComponent<Canvas>() as Canvas;
        roundScoreboardScript = this.transform.GetChild(2).GetComponent<RoundScoreboard>() as RoundScoreboard;

        roundOver = this.transform.GetChild(3).GetComponent<Canvas>() as Canvas;
        tournamentOver = this.transform.GetChild(4).GetComponent<Canvas>() as Canvas;

        TransitionMainMenu();
    }

    void TransitionMainMenu()
    {
        HideAllCanvases();
        ResetTournamentScores();
        gameState = GameState.MAIN_MENU;
        mainMenu.enabled = true;
    }

    void ResetTournamentScores()
    {
         for (int i = 0; i < 4; i++)
         {
            this.tournamentScores[i] = 0;
         }
    }

    void TransitionReadyRoom()
    {
        HideAllCanvases();
        gameState = GameState.READY_ROOM;
        readyRoom.enabled = true;
    }

    void TransitionRoundPlaying()
    {
        LoadRandomScene();
        ResetRoundScores();

        HideAllCanvases();
        gameState = GameState.ROUND_PLAYING;
        roundScoreBoard.enabled = true;
    }

    void LoadRandomScene()
    {
        int nextScene = Random.Range(0, 4);
        string sceneName = "scene_" + nextScene.ToString();
        SceneManager.LoadScene(sceneName);
    }

    void ResetRoundScores()
    {
        for (int i = 0; i < 4; i ++)
        {
            this.roundScores[i] = 0;
        }
    }

    void TransitionRoundOver()
    {
        HideAllCanvases();
        gameState = GameState.ROUND_OVER;
        roundOver.enabled = true;
    }

    void TransitionTournamentOver()
    {
        HideAllCanvases();
        gameState = GameState.TOURNAMENT_OVER;
        tournamentOver.enabled = true;
    }

    void HideAllCanvases()
    {
        mainMenu.enabled = false;
        readyRoom.enabled = false;
        roundScoreBoard.enabled = false;
        roundOver.enabled = false;
        tournamentOver.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.gameState)
        {
            case GameState.MAIN_MENU:
                {
                    if(Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        TransitionReadyRoom();
                    }
                    break;
                }
            case GameState.READY_ROOM:
                {
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        TransitionRoundPlaying();
                    }
                    break;
                }
            case GameState.ROUND_PLAYING:
                {
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        TransitionRoundOver();
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha7)) { ChangeScore(0, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha8)) { ChangeScore(1, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha9)) { ChangeScore(2, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha0)) { ChangeScore(3, 1); }

                    UpdateRoundScoreboard();
                    break;
                }
            case GameState.ROUND_OVER:
                {
                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        TransitionTournamentOver();
                    }
                    break;
                }
            case GameState.TOURNAMENT_OVER:
                {
                    if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        TransitionMainMenu();
                    }
                    break;
                }
        }
    }

    void UpdateRoundScoreboard()
    {
        roundScoreboardScript.SetScores(this.roundScores);
    }

    void ChangeScore(int player, int change)
    {
        if (player >= 0 && player <= 3)
        {
            this.roundScores[player] += change;
        }

        if (this.roundScores[player] >= this.roundLimit)
        {
            this.tournamentScores[player] += 1;
            TransitionRoundOver();
        }
    }
}
