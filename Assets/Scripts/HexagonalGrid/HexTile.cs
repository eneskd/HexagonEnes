using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Hex hex;
    public int colorIndex;
    public bool isChecked = false;
    public bool isMarked = false;
    public int dropDistance = 0;

    public void SetColor(int index, Color color)
    {
        colorIndex = index;
        GetComponent<SpriteRenderer>().color = color;
        isChecked = false;
        isMarked = false;
    }

    public void Mark()
    {
        isMarked = true;
    }

    public void Check()
    {
        Check(0);
        isChecked = true;
    }

    private void Check(int direction)
    {
        while (true)
        {
            if (direction == 6) break;
            //if (hex.X == 0 || hex.Y == 0 || hex.X == HexGrid.Grid.GetLength(0) - 1 || hex.Y == HexGrid.Grid.GetLength(1) - 1) break; // Don't Check Edges


            var hex1 = hex.GetNeighbour(direction % 6);
            var hex2 = hex.GetNeighbour((direction + 1) % 6);
            direction += 1;


            var grid = HexGrid.Grid;
            if (hex1.X < 0 || hex1.X >= grid.GetLength(0) || hex1.Y < 0 || hex1.Y >= grid.GetLength(1)) continue;
            if (hex2.X < 0 || hex2.X >= grid.GetLength(0) || hex2.Y < 0 || hex2.Y >= grid.GetLength(1)) continue;

            var neighbourTile1 = grid[hex1.X, hex1.Y];
            var neighbourTile2 = grid[hex2.X, hex2.Y];
            if (neighbourTile1.colorIndex == colorIndex)
            {
                if (neighbourTile2.colorIndex == colorIndex)
                {
                    neighbourTile1.Mark();
                    neighbourTile2.Mark();
                    Mark();
                }
            }
        }
    }
}