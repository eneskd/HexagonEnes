using System;
using System.Collections;
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

    public enum RotationDirection
    {
        CW = -1,
        CCW = 1
    }


    public class HexCorner : MonoBehaviour
    {
        public delegate void OnMarkedFoundHandler();
        public static event OnMarkedFoundHandler OnMarkedFound;
        
        public static float RotationSpeed = 4f;
        public static float Delay = 0.1f;

        public Hex[] hexes = new Hex[3];
        public HexCornerHandiness handiness;
        public int rotationCount = 0;

        private SpriteRenderer _spriteRenderer;
        private HexTile[,] _grid;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _grid = HexGrid.Grid;
        }


        public void SetHexes(Hex hex)
        {
            hexes[0] = new Hex(hex.q, hex.r);
            for (int i = 0; i < 2; i++)
            {
                hexes[i + 1] = hexes[i].GetNeighbour(2 * (i + 1) - (int) handiness);
            }

            if (handiness == HexCornerHandiness.LeftHand)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        public void Select(bool isSelected)
        {
            _spriteRenderer.enabled = isSelected;
            if (isSelected)
            {
                foreach (var hex in hexes)
                {
                    _grid[hex.X, hex.Y].transform.SetParent(this.transform);
                }
            }
            else
            {
                foreach (var hex in hexes)
                {
                    _grid[hex.X, hex.Y].transform.SetParent(HexGrid.I.transform);
                }
            }
        }

        public void StartRotation(RotationDirection direction)
        {
            GameLoop.CurrentState = GameState.Rotating;
            foreach (var hex in hexes)
            {
                _grid[hex.X, hex.Y].isChecked = false;
            }
            StartCoroutine(Rotate120(direction));
        }

        private void RotationEnd(RotationDirection direction)
        {
            StopAllCoroutines();
            for (int i = 0; i < hexes.Length; i++)
            {
                var moveDirection = (2 * i - ((int) direction - 1) / 2 + 2 - (int) handiness) % 6;
                _grid[hexes[i].X, hexes[i].Y].hex.MoveHex(moveDirection);
            }
            
            ReorderGrid(direction);
            
            if (HexGrid.CheckGrid())
            {
                rotationCount = 0;
                OnMarkedFound?.Invoke();
                return;
            }
            
            rotationCount++;
            
            if (rotationCount == 3)
            {
                rotationCount = 0;
                GameLoop.CurrentState = GameState.WaitingInput;
                return;
            }

            StartCoroutine(WaitThanStart(direction));
        }

        private void ReorderGrid(RotationDirection direction)
        {
            // Fix Grid references after rotation 
            SwitchGridLocation(hexes[0], hexes[1 + ((int) direction + 1) / 2]);
            SwitchGridLocation(hexes[1], hexes[2]);
        }

        private void SwitchGridLocation(Hex h1, Hex h2)
        {
            HexTile tile = _grid[h1.X, h1.Y];
            _grid[h1.X, h1.Y] = _grid[h2.X, h2.Y];
            _grid[h2.X, h2.Y] = tile;
        }


        private IEnumerator Rotate120(RotationDirection direction)
        {
            var initialRotation = transform.rotation;
            var targetAngle = (int) direction * 120 + initialRotation.eulerAngles.z;
            var target = Quaternion.Euler(0f, 0f, targetAngle);
            var ratio = 0f;

            while (ratio < 1)
            {
                ratio += RotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(initialRotation, target, ratio);
                yield return null;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
            RotationEnd(direction);
        }

        private IEnumerator WaitThanStart(RotationDirection direction)
        {
            yield return new WaitForSeconds(Delay);
            StartRotation(direction);
        }

    }
}