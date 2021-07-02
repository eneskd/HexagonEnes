using UnityEngine;

namespace HexagonalGrid
{

    public enum HexCornerDirections
    {
        E = 0,
        NE = 1,
        NW = 2,
        W = 3,
        SW = 4,
        SE = 5
    }

    public enum HexCornerHandiness
    {
        LeftHand = 0,
        RightHand = 1,
    }
    
    
    public class HexCorner : MonoBehaviour
    {
        public Hex[] hexes = new Hex[3];
        public HexCornerHandiness handiness;


        public void SetHexes(Hex hex)
        {
            hexes[0] = new Hex(hex.q, hex.r);
            for (int i = 0; i < 2; i++)
            {
                hexes[i + 1] = hexes[0].GetNeighbour(3 - i - (int) handiness);
            }
        }

    }
}