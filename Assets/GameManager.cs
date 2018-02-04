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

    public int[] levelScores = { 0, 0, 0, 0 };
    public int[] tournamentScores = { 0, 0, 0, 0 };

    Canvas mainMenu;
    Canvas readyRoom;
    Canvas roundScores;
    Canvas roundOver;
    Canvas tournamentOver;

    private GameState gameState;
    // Use this for initialization
    void Start () {
        gameState = GameState.MAIN_MENU;

        mainMenu = this.transform.GetChild(0).GetComponent<Canvas>() as Canvas;
        readyRoom = this.transform.GetChild(1).GetComponent<Canvas>() as Canvas;
        roundScores = this.transform.GetChild(2).GetComponent<Canvas>() as Canvas;
        roundOver = this.transform.GetChild(3).GetComponent<Canvas>() as Canvas;
        tournamentOver = this.transform.GetChild(4).GetComponent<Canvas>() as Canvas;

        TransitionMainMenu();
    }

    void TransitionMainMenu()
    {
        HideAllCanvases();
        gameState = GameState.MAIN_MENU;
        mainMenu.enabled = true;
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

        HideAllCanvases();
        gameState = GameState.ROUND_PLAYING;
        roundScores.enabled = true;
    }

    void LoadRandomScene()
    {
        int nextScene = Random.Range(0, 4);
        string sceneName = "scene_" + nextScene.ToString();
        SceneManager.LoadScene(sceneName);
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
        roundScores.enabled = false;
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

}
