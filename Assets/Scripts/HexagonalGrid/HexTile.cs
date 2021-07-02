using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Hex hex;
    public int colorIndex;
   

    public void SetColor(int index, Color color)
    {
        colorIndex = index;
        GetComponent<SpriteRenderer>().color = color;
    }
    
    
    
    


}
