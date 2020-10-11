using System.Collections.Generic;
using Custom.PathFinding.Utilities;
using UnityEngine;

namespace Custom.PathFinding.Components
{
    public class Grid : MonoBehaviour
    {
        public bool onlyDisplayPathGizmos;
        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        private Node[,] grid;

        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        public int MazSize => gridSizeX * gridSizeY;

        private void Start()
        {
            nodeDiameter = nodeRadius * 2f;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            var worldBottomLeft =
                transform.position - Vector3.right * gridSizeX / 2f - Vector3.forward * gridSizeY / 2f;
            for (var x = 0; x < gridSizeX; x++)
            {
                for (var y = 0; y < gridSizeX; y++)
                {
                    var worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * nodeDiameter + nodeRadius);
                    var walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public IEnumerable<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    var checkX = node.gridX + x;
                    var checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                        neighbours.Add(grid[checkX, checkY]);
                }
            }

            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            var percentX = Mathf.Clamp01((worldPosition.x + gridSizeX / 2f) / gridSizeX);
            var percentY = Mathf.Clamp01((worldPosition.z + gridSizeY / 2f) / gridSizeY);

            var x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            var y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            return grid[x, y];
        }

        public List<Node> path;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (onlyDisplayPathGizmos)
            {
                if (path != null)
                {
                    foreach (var node in path)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                    }
                }
            }
            else
            {
                if (path != null)
                {
                    foreach (var node in grid)
                    {
                        Gizmos.color = node.walkable ? Color.white : Color.red;
                        if (path != null && path.Contains(node))
                            Gizmos.color = Color.black;
                        Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                    }
                }
            }
        }
    }
}
