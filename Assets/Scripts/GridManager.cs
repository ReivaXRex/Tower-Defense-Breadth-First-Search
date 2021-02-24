using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Tooltip("World Grid Size should match the UnityEditor Snap Settings.")]
    [SerializeField] private int unityGridSize = 10;

    public int UnityGridSize { get { return unityGridSize; } }

    [SerializeField] private Vector2Int gridSize; // Specify grid size

    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }

    /// <summary>
    /// Return a specified node from within the grid Dictionary.
    /// </summary>
    /// <param name="coordinates">
    /// Key for the Dictionary.
    /// </param>
    /// <returns>
    /// Node from Dictionary.
    /// </returns>
    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    /// <summary>
    /// Block a specific Node.
    /// </summary>
    /// <param name="coordinates">
    /// Coordinates of the Node.
    /// </param>
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    /// <summary>
    /// Reset the status of a Node.
    /// </summary>
    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }

    /// <summary>
    /// Convert a World Position into a Coordinate.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    }

    /// <summary>
    /// Convert a Coordinate into a World Position.
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        return position;
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}