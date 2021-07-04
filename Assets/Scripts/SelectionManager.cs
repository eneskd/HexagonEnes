using System.Collections;
using System.Collections.Generic;
using HexagonalGrid;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager I => _i;
    private static SelectionManager _i;

    public static HexCorner Selection;
    public static bool isSelectionActive = false;

    [SerializeField] private float selectionThreshold = 2f;


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


        PlayerInput.OnScreenClick += Select;
        PlayerInput.OnRotateCommand += Rotate;
    }

    private void Select(Vector3 position)
    {
        var closest = FindClosest(position, out bool isSelected);
        if (!isSelected) return;
        ClearSelection();
        isSelectionActive = true;
        Selection = closest;
        Selection.Select(true);
    }

    private void Rotate(RotationDirection direction)
    {
        if (isSelectionActive) {Selection.StartRotation(direction);}
    }

    private HexCorner FindClosest(Vector3 position, out bool isSelected)
    {
        var smallestDistance = Mathf.Infinity;
        HexCorner closestCorner = null;
        isSelected = false;
        foreach (var hexCorner in HexGrid.HexCorners)
        {
            var distance = ((Vector2) hexCorner.transform.position - (Vector2) position).magnitude;
            if (distance < selectionThreshold && distance < smallestDistance)
            {
                smallestDistance = distance;
                closestCorner = hexCorner;
                isSelected = true;
            }
        }

        return closestCorner;
    }

    public static void ClearSelection()
    {
        if (isSelectionActive)
        {
            Selection.Select(false);
            isSelectionActive = false;
            Selection = null;
        }
    }
}