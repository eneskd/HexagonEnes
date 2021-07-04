using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using UnityEngine;

public enum GameState
{
    WaitingInput,
    Rotating,
    Destroying,
    Dropping,
    GameOver
}

public class GameLoop : MonoBehaviour
{
    public static GameState CurrentState = GameState.WaitingInput;
    [SerializeField] private GameObject endGamePanel;


    public static GameLoop I => _i;
    private static GameLoop _i;



    private void Awake()
    {
        if (_i != null && _i != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _i = this;
        }
    }

    public void EndGame()
    {
        CurrentState = GameState.GameOver;
        // Debug.Log("Game Over");
        endGamePanel.SetActive(true);
    }

    public void RestartGame()
    {
        endGamePanel.SetActive(false);
        ScoreManager.I.SetScore(0);
        HexSpawner.bombsSpawned = 0;
        HexGrid.I.FillGrid();
        CurrentState = GameState.WaitingInput;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}