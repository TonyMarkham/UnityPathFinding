namespace Custom.PathFinding.Utilities
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private T[] items;
        private int currentItemCount;
        public int Count => currentItemCount;

        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount += 1;
        }

        public T RemoveFirst()
        {
            var firstItem = items[0];
            currentItemCount -= 1;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);

            return firstItem;
        }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        void SortUp(T item)
        {
            var parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                var parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                    Swap(item, parentItem);
                else
                    break;
            }
        }

        void SortDown(T item)
        {
            while (true)
            {
                var leftChildIndex = item.HeapIndex * 2 + 1;
                var rightChildIndex = item.HeapIndex * 2 + 2;
                var swapindex = 0;

                if (leftChildIndex < currentItemCount)
                {
                    swapindex = leftChildIndex;

                    if (rightChildIndex < currentItemCount)
                    {
                        if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                        {
                            swapindex = rightChildIndex;
                        }
                    }

                    if (item.CompareTo(items[swapindex]) < 0)
                    {
                        Swap(item, items[swapindex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            var indexHolder = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = indexHolder;
        }
    }
}
