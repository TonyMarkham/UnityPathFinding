using UnityEngine;

namespace Custom.PathFinding.Utilities
{
    [System.Serializable]
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX, gridY;
        public int gCost;
        public int hCost;
        public Node parent;
        public int fCost => gCost + hCost;

        private int heapIndex;

        public int HeapIndex
        {
            get => heapIndex;
            set => heapIndex = value;
        }

        public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int CompareTo(Node other)
        {
            var compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(other.hCost);

            return -compare;
        }
    }
}
