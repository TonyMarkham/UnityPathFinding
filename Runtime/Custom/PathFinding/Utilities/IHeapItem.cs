using System;

namespace Custom.PathFinding.Utilities
{
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}