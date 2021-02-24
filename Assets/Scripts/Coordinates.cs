using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class Coordinates : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color blockedColor = Color.white;
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathColor = Color.red;

    private TextMeshPro label;
    private  Vector2Int coordinates = new Vector2Int();

    private GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
       
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying) // Only Execute in Edit Mode
        {
            DisplayCoordinates();
            UpdateName();
        }

        ColorCoordinates();
        ToggleLabels();
    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive(); // Set the current enabled state of the label to the opposite of the current active state.
        }
    }

    private void ColorCoordinates()
    {
        if (gridManager == null) { return; }
    

        Node node = gridManager.GetNode(coordinates);
        if (node == null) { return; }
   

        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
      
    }

    private void DisplayCoordinates()
    {
        if (gridManager == null) { return; }
     
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = coordinates.x + "," + coordinates.y;
    }

    private void UpdateName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
