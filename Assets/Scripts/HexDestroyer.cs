using System.Collections;
using HexagonalGrid;
using UnityEngine;

public class HexDestroyer : MonoBehaviour
{

    public delegate void OnScoreChangeHandler(int amount);

    public static event OnScoreChangeHandler OnScoreChanged;
    public static float delay = 0.1f;
    
    
    public static HexDestroyer I => _i;
    private static HexDestroyer _i;




    
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
        
        HexCorner.OnMarkedFound += WaitAndDestroy;
    }

    public static void DestroyHexes()
    {
        SelectionManager.ClearSelection();
        var xSize = HexGrid.Grid.GetLength(0);
        var ySize = HexGrid.Grid.GetLength(1);
        var spawnList = new int[xSize];

        for (int i = 0; i < HexGrid.MarkedTiles.Count; i++)
        {
            var markedTile = HexGrid.MarkedTiles[i];
            var x = markedTile.hex.X;
            var y = markedTile.hex.Y;

            spawnList[x] += 1;

            for (int j = y; j < ySize; j++)
            {
                var hexTile = HexGrid.Grid[x, j];
                if (hexTile.isMarked) continue;

                if (!HexDropper.DropList.Contains(hexTile))
                {
                    HexDropper.DropList.Add(hexTile);
                }


                hexTile.isChecked = false;
                hexTile.dropDistance += 1;
            }
        }

        
        OnScoreChanged?.Invoke(HexGrid.MarkedTiles.Count * 5);
        
        for (var i = 0; i < HexGrid.MarkedTiles.Count; i++)
        {
            var markedTile = HexGrid.MarkedTiles[i];
            // print($"Destroying {markedTile.hex.X} {markedTile.hex.Y}");
            HexGrid.Grid[markedTile.hex.X, markedTile.hex.Y] = null;
            Destroy(markedTile.gameObject);
        }

        HexGrid.MarkedTiles.Clear();

        //foreach (var hexTile in HexDropper.DropList)
        //{
        //    var hex = hexTile.hex;
        //    Debug.Log($"{hex.q} {hex.r} wil drop {hexTile.dropDistance} tiles");
        //}
        //
        //string msg = "";
        //for (int i = 0; i < spawnList.Length; i++)
        //{
        //    msg += (spawnList[i].ToString() + " ");
        //}
        //print("Spawn List: " + msg);

        HexSpawner.SpawnHexes(spawnList);
    }

    public void WaitAndDestroy()
    {
        StartCoroutine(WaitForDestroyCoroutine());
    }

    private IEnumerator WaitForDestroyCoroutine()
    {
        GameLoop.CurrentState = GameState.Destroying;

        yield return new WaitForSeconds(delay);
        DestroyHexes();
    }
}