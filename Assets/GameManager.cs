using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum GameState
{
    MAIN_MENU,
    READY_ROOM,
    ROUND_PLAYING,
    ROUND_OVER,
    TOURNAMENT_OVER
}
public class GameManager : MonoBehaviour {

    public bool loadRandomly = false;
    public int levelToLoad = 0;

    public int[] roundScores = { 0, 0, 0, 0 };
    public int[] tournamentScores = { 0, 0, 0, 0 };

    public int roundLimit = 2;
    public int tournamentLimit = 2;

    int roundWinner = -1;
    int tournamentWinner = -1;

    public bool[] playerJoined = { false, false, false, false };

    Canvas mainMenu;
    Canvas readyRoom;
    Canvas roundScoreBoard;
    Canvas roundOver;
    Canvas tournamentOver;

    RoundScoreboard roundScoreboardScript;
    RoundScoreboard roundOverScoreboardScript;

    Text roundOverText;
    Text tournamentOverText;

    private bool anyKeyLocked = false;

    private GameState gameState;

    // Use this for initialization
    void Start () {
        gameState = GameState.MAIN_MENU;

        mainMenu = this.transform.GetChild(0).GetComponent<Canvas>() as Canvas;
        readyRoom = this.transform.GetChild(1).GetComponent<Canvas>() as Canvas;

        roundScoreBoard = this.transform.GetChild(2).GetComponent<Canvas>() as Canvas;
        roundScoreboardScript = this.transform.GetChild(2).GetComponent<RoundScoreboard>() as RoundScoreboard;

        roundOver = this.transform.GetChild(3).GetComponent<Canvas>() as Canvas;
        roundOverText = this.transform.GetChild(3).GetChild(0).GetComponent<Text>() as Text;
        roundOverScoreboardScript = this.transform.GetChild(3).GetComponent<RoundScoreboard>() as RoundScoreboard;

        tournamentOver = this.transform.GetChild(4).GetComponent<Canvas>() as Canvas;
        tournamentOverText = this.transform.GetChild(4).GetChild(0).GetComponent<Text>() as Text;

        TransitionMainMenu();
    }

    void TransitionMainMenu()
    {
        HideAllCanvases();
        ResetTournamentScores();
        DeRegisterAllPlayers();
        this.tournamentWinner = -1;
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
        if (this.loadRandomly)
        {
            LoadRandomScene();
        }
        else
        {
            SceneManager.LoadScene("scene_" + this.levelToLoad.ToString());
        }

        ResetRoundScores();

        HideAllCanvases();
        gameState = GameState.ROUND_PLAYING;
        roundScoreBoard.enabled = true;
    }

    void LoadRandomScene()
    {
        int nextScene = Random.Range(0, 3);
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
        roundOver.enabled = true;
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
                    if(AnyKey())
                    {
                        TransitionReadyRoom();
                    }
                    break;
                }
            case GameState.READY_ROOM:
                {
                    if (AnyKey())
                        {
                        TransitionRoundPlaying();
                    }
                    break;
                }
            case GameState.ROUND_PLAYING:
                {
                    UpdateRoundScoreboard();

                    if (Input.GetKeyDown(KeyCode.Alpha7)) { ChangeScore(0, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha8)) { ChangeScore(1, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha9)) { ChangeScore(2, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha0)) { ChangeScore(3, 1); }
                    if (Input.GetKeyDown(KeyCode.Alpha3))

                    {
                        TransitionRoundOver();
                    }

                    break;
                }
            case GameState.ROUND_OVER:
                {
                    string roundOverMessage = "Player " + (this.roundWinner + 1).ToString() + " wins the Round!";
                    UpdateRoundOverScoreboard(roundOverMessage, this.roundWinner);

                    if (AnyKey())
                    {
                        if (this.tournamentWinner > -1)
                        {
                            TransitionTournamentOver();
                        } else
                        {
                            TransitionRoundPlaying();
                        }
                        
                    }
                    break;
                }
            case GameState.TOURNAMENT_OVER:
                {
                    string tournamentOverMessage = "Player " + (this.tournamentWinner + 1).ToString() + " wins the Tournament!";
                    UpdateRoundOverScoreboard(tournamentOverMessage, this.tournamentWinner);

                    if (AnyKey())
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

    void UpdateRoundOverScoreboard(string scoreboardMessage, int winner)
    {
        roundOverText.text = scoreboardMessage;

        switch (winner)
        {
            case (0): {
                    roundOverText.color = Color.red;
                    break;
                }
            case (1):
                {
                    roundOverText.color = Color.blue;
                    break;
                }
            case (2):
                {
                    roundOverText.color = Color.green;
                    break;
                }
            case (3):
                {
                    roundOverText.color = Color.yellow;
                    break;
                }
        }

        roundOverScoreboardScript.SetScores(this.tournamentScores);
    }

    void UpdateTournamentOverText()
    {
        tournamentOverText.text = "Tournament winner is " + (this.tournamentWinner + 1).ToString();
    }

    public void AddPoint(int playerNumber)
    {
        ChangeScore(playerNumber, 1);
    }

    public void SubtractPoint(int playerNumber)
    {
        ChangeScore(playerNumber, -1);
    }

    void ChangeScore(int player, int change)
    {
        if (player >= 0 && player <= 3)
        {
            this.roundScores[player] += change;
        }

        if (this.roundScores[player] >= this.roundLimit)
        {
            this.roundWinner = player;

            this.tournamentScores[player] += 1;
            if(this.tournamentScores[player] >= this.tournamentLimit)
            {
                this.tournamentWinner = player;
            }

            TransitionRoundOver();
        }
    }

    private bool AnyKey()
    {
        if (this.anyKeyLocked)
        {
            return false;
        }

        bool anyKey = false;

        for (int i = 0; i < 4; i ++ )
        {
            string accelerateAxis = "Accelerate" + i;
            string steerAxis = "Steer" + i;

            if(Input.GetAxis(accelerateAxis) != 0)
            {
                anyKey = true;
            }
            if (Input.GetAxis(steerAxis) != 0)
            {
                anyKey = true;
            }
        }

        if (anyKey)
        {
            this.anyKeyLocked = true;
            Invoke("UnlockAnyKey", 0.5f);
        }

        return anyKey;
    }

    private void UnlockAnyKey()
    {
        this.anyKeyLocked = false;
    }

    public void RegisterPlayer(int playerNumber)
    {
        playerJoined[playerNumber] = true;
    }

    private void DeRegisterAllPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            this.playerJoined[i] = false;
        }
    }
}
