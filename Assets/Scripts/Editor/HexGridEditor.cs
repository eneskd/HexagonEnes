using HexagonalGrid;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(HexGrid))]
    public class HexGridEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gridManager = target as HexGrid;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fill Grid"))
            {
                gridManager.FillGrid();
            }
            
            if (GUILayout.Button("Clear Grid"))
            {
                gridManager.ClearGrid();
            }

           
            
            if (GUILayout.Button("Snap"))
            {
                gridManager.Snap();
            }
            GUILayout.EndHorizontal();
            
           
            
            
        }
    }
}