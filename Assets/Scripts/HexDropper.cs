using System;
using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using UnityEngine;

public class HexDropper : MonoBehaviour
{
    public static List<HexTile> DropList = new List<HexTile>();
    public float dropSpeed = 5f;

    public delegate void OnActionCompletedHandler();

    public static event OnActionCompletedHandler OnActionCompleted;

    private void Awake()
    {
        HexSpawner.OnHexesSpawned += RearrangeGrid;
    }

    private void Update()
    {
        if (GameLoop.CurrentState != GameState.Dropping) return;

        for (var i = 0; i < DropList.Count; i++)
        {
            var hexTile = DropList[i];
            if (hexTile.hex.GetPosition().y < hexTile.transform.position.y)
            {
                hexTile.transform.position += Vector3.down * (dropSpeed * Time.deltaTime);
            }
            else
            {
                hexTile.transform.position = hexTile.hex.GetPosition();
                DropList.Remove(hexTile);
            }
        }

        if (DropList.Count == 0)
        {
            if (HexGrid.CheckGrid())
            {
                HexDestroyer.I.WaitAndDestroy();
            }
            else
            {
                GameLoop.CurrentState = GameState.WaitingInput;
                OnActionCompleted?.Invoke();
            }
            
        }
    }
    
    public static void RearrangeGrid()
    {
        foreach (var hexTile in DropList)
        {
            MoveDown(hexTile);
        }

        GameLoop.CurrentState = GameState.Dropping;
    }

    private static void MoveDown(HexTile hexTile)
    {
        var hex = hexTile.hex;
        hex.MoveHex(HexNeighbourDirection.S, hexTile.dropDistance);
        hexTile.isChecked = false;
        hexTile.dropDistance = 0;
        HexGrid.Grid[hex.X, hex.Y] = hexTile;
        
    }
}