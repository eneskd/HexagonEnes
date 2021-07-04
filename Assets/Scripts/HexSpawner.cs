using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using UnityEngine;

public class HexSpawner : MonoBehaviour
{
    public delegate void OnHexesSpawnedHandler();

    public static event OnHexesSpawnedHandler OnHexesSpawned;

    public static int bombSpawnScore = 1000;
    public static int bombsSpawned = 0;
    private static int bombsToSpawn => ScoreManager.Score / bombSpawnScore;
    


    public static void SpawnHexes(int[] list)
    {
        var maxY = HexGrid.Grid.GetLength(1);

        for (int i = 0; i < list.Length; i++)
        {
            for (int j = 0; j < list[i]; j++)
            {
                HexTile hexTile;
                if (bombsSpawned < bombsToSpawn)
                {
                    hexTile = HexGrid.I.CreateBombTile(i, maxY + j);
                    bombsSpawned++;
                }
                else
                {
                    hexTile = HexGrid.I.CreateHexTile(i, maxY + j);
                }
                hexTile.isChecked = false;
                hexTile.dropDistance = list[i];
                HexDropper.DropList.Add(hexTile);
            }
        }

        OnHexesSpawned?.Invoke();
    }

   
}