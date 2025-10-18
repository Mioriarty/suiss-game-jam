using System.Collections.Generic;
using UnityEngine;

public class AStarNodeGridCreator : MonoBehaviour
{
    [SerializeField] private GameObject node;
    [SerializeField] private Vector2 gridStartPosition;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float nodeSpacing;

    [SerializeField] private bool allowDiagonalConnections = true;

    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask onlyTargetLayer;
    [SerializeField] private bool drawGizmos = true;

    void OnEnable()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        AStarNode[][] grid = new AStarNode[gridWidth][];

        for (int x = 0; x < gridWidth; x++)
        {
            grid[x] = new AStarNode[gridHeight];

            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 nodePosition = new Vector2(
                    gridStartPosition.x + (x * nodeSpacing),
                    gridStartPosition.y + (y * nodeSpacing)
                );

                // Check if the space is not occupied
                Collider2D hitCollider = Physics2D.OverlapCircle(nodePosition, nodeSpacing / 2, obstacleLayer);
                if (hitCollider != null)
                    continue; // Skip this position if occupied

                hitCollider = Physics2D.OverlapCircle(nodePosition, nodeSpacing / 2, onlyTargetLayer);
                bool isOnlyTarget = hitCollider != null;

                GameObject newNode = Instantiate(node, nodePosition, Quaternion.identity, transform);
                newNode.transform.parent = transform;
                grid[x][y] = newNode.GetComponent<AStarNode>();
                grid[x][y].onlyTarget = isOnlyTarget;
            }
        }

        // Make all connections
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x][y] == null)
                    continue;

                List<AStarNode> neighbors = new List<AStarNode>();
                if (x > 0 && grid[x - 1][y] != null) // Left
                    neighbors.Add(grid[x - 1][y]);

                if (x < gridWidth - 1 && grid[x + 1][y] != null) // Right
                    neighbors.Add(grid[x + 1][y]);

                if (y > 0 && grid[x][y - 1] != null) // Down
                    neighbors.Add(grid[x][y - 1]);

                if (y < gridHeight - 1 && grid[x][y + 1] != null) // Up
                    neighbors.Add(grid[x][y + 1]);
                
                if (allowDiagonalConnections)
                {
                    if (x > 0 && y > 0 && grid[x - 1][y - 1] != null) // Down-Left
                        neighbors.Add(grid[x - 1][y - 1]);

                    if (x < gridWidth - 1 && y > 0 && grid[x + 1][y - 1] != null) // Down-Right
                        neighbors.Add(grid[x + 1][y - 1]);

                    if (x > 0 && y < gridHeight - 1 && grid[x - 1][y + 1] != null) // Up-Left
                        neighbors.Add(grid[x - 1][y + 1]);

                    if (x < gridWidth - 1 && y < gridHeight - 1 && grid[x + 1][y + 1] != null) // Up-Right
                        neighbors.Add(grid[x + 1][y + 1]);
                }

                grid[x][y].connections = neighbors.ToArray();
            }
        }
    }

    private void OnDrawGizmos()
    {

        // Only Draw if game is not running
        if (Application.isPlaying || !drawGizmos)
            return;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 nodePosition = new Vector2(
                    gridStartPosition.x + (x * nodeSpacing),
                    gridStartPosition.y + (y * nodeSpacing)
                );

                // Draw in red if occupied
                Collider2D hitCollider = Physics2D.OverlapCircle(nodePosition, nodeSpacing / 2, obstacleLayer | onlyTargetLayer);
                if (hitCollider != null)
                {
                    if((onlyTargetLayer.value & (1 << hitCollider.gameObject.layer)) > 0)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawWireSphere(nodePosition, nodeSpacing / 2);
            }
        }
    }
}
