using System.Collections.Generic;
using Custom.PathFinding.Utilities;
using UnityEngine;

namespace Custom.PathFinding.Components
{
    public class PathFinding : MonoBehaviour
    {
        public Transform seeker, target;
        private Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                FindPath(seeker.position, target.position);
            }
        }

        void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            var startNode = grid.NodeFromWorldPoint(startPosition);
            var targetNode = grid.NodeFromWorldPoint(targetPosition);

            var openSet = new Heap<Node>(grid.MazSize);
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (var neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    var newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        int GetDistance(Node a, Node b)
        {
            var distanceX = Mathf.Abs(a.gridX - b.gridX);
            var distanceY = Mathf.Abs(a.gridY - b.gridY);
            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);

            return 14 * distanceX + 10 * (distanceY - distanceX);
        }

        void RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            grid.path = path;
        }
    }
}
