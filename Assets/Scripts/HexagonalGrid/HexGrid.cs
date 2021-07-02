using System.Collections.Generic;
using UnityEngine;

namespace HexagonalGrid
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize = new Vector2Int(8, 9);
        [SerializeField] private GameObject tileObject;
        [SerializeField] private GameObject cornerObject;

        
        public List<Color> colors = new List<Color>();
        public static HexTile[,] Grid;
        
        public static HexGrid I => _i;
        private static HexGrid _i;
            
            

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


        public void FillGrid()
        {
            ClearGrid();
            Grid = new HexTile[gridSize.x, gridSize.y];
            Hex.Origin = transform.position;
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    Grid[i, j] = CreateHexTile(i, j);
                }
            }
            
            PlaceCorners();
        }

        public void ClearGrid()
        {
            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }

            for (int i = 0; i < children.Count; i++)
            {
                DestroyImmediate(children[i]);
            }

            Grid = null;
        }

        public void Snap()
        {
            transform.position = transform.position.Round();
        }

        private HexTile CreateHexTile(int x, int y)
        {
            var tileObj = Instantiate(tileObject, transform);
            var hexTile = tileObj.GetComponent<HexTile>();
            hexTile.hex = Hex.FromXY(x, y);
            var r = Random.Range(0, colors.Count);
            hexTile.SetColor(r, colors[r]);
            var position = hexTile.hex.GetPosition();
            hexTile.transform.position = position;
            return hexTile;
        }

        private void PlaceCorners()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    if ( i < gridSize.x - 1 && j < gridSize.y -1)
                    {
                        CreateHexCorner(Grid[i, j].hex, HexCornerHandiness.RightHand);
                    }
                    if ( i > 0 && j < gridSize.y -1)
                    {
                        CreateHexCorner(Grid[i, j].hex, HexCornerHandiness.LeftHand);
                    }
                }
            }
        }

        private HexCorner CreateHexCorner(Hex hex, HexCornerHandiness hand)
        {
            var cornerObj = Instantiate(cornerObject, transform);
            cornerObj.transform.position = hex.GetCornerPosition(2 -  (int) hand);
            var corner = cornerObj.GetComponent<HexCorner>();
            corner.handiness = hand;
            corner.SetHexes(hex);

            return corner;
        }
        

        
    }
}