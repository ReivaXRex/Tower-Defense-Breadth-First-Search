using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int currentCoordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(currentCoordinates);
        return BuildPath();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        // Loop through all four directions...
        foreach (Vector2Int direction in directions) 
        {
            // Calculate the coordinates of the Node in that direction from the currentSearchNode and store it within a variable.
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            // Check if the neighbor's coordinates exist in the grid...
            if (grid.ContainsKey(neighborCoords))
            {   
                // If it does, add it to the neighbors List.
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            // If the reached Dictionary doesn't already contain this Node & if it's walkable..
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                // Add the neighbor Node to the reached Dictionary & frontier Queue.
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        // Add the coordinates that are being passed in into the frontier Queue
        frontier.Enqueue(grid[coordinates]);
        // Add the coordinates passed in along with the associated Node from the grid to the reached Dictionary.
        reached.Add(coordinates, grid[coordinates]);

        // While there's still Node's to explore
        // IsRunning provides a way to break of the loop if there's still node's to search but the destination has been found already.
        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();

            // If the Destination has been found..
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                // Break out of the loop using the isRunning flag.
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        // Setup the currentNode for Search, from the destination backwards.
        List<Node> path = new List<Node>();

        Node currentNode = destinationNode;
        
        path.Add(currentNode);
        currentNode.isPath = true;

        // While there are still connected Nodes to explore move back throught the 'search tree'...
        while (currentNode.connectedTo != null)
        {
            // Set the currentNode to one step down the path.
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        // Reverse the path since it was searched in reverse from the destination.
        path.Reverse();

        return path;
    }

    /// <summary>
    /// Check whether placing a Tower will block a Node.
    /// </summary>
    /// <param name="coordinates">
    /// Identify a specific Node by it's coordinates.
    /// </param>
    /// <returns>
    /// False if not blocking the path. If the pathfinding is unable to find a route then returns false.
    /// </returns>
    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
