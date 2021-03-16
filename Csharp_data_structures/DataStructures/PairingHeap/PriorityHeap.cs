using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp_data_structures.DataStructures.PairingHeap
{
    class PriorityHeap<K, P, V>
        where K : IComparable
        where P : IComparable
    {
        public K Peek { get => _root != null ? _root.Key : default(K); }
        private PriorityHeapNode<K, P, V> _root = null;
        private int _numberOfNodes = 0;
        public int Count { get => _numberOfNodes; }

        public void Insert(K key, P priority, V value)
        {
            var newNode = new PriorityHeapNode<K, P, V>(key, priority,  value);
            ++_numberOfNodes;
            if (_root == null)
            {
                _root = newNode;
                return;
            }
            _root = Pair(_root, newNode);

        }

        private PriorityHeapNode<K, P, V> Pair(PriorityHeapNode<K, P, V> nodeOne, PriorityHeapNode<K, P, V> nodeTwo)
        {
            if (nodeOne.Key.CompareTo(nodeTwo.Key) == 0)
            {
                if (nodeOne.Priority.CompareTo(nodeTwo.Priority) > 0)
                {
                    var leftSon = nodeOne.LeftSon;
                    nodeOne.LeftSon = nodeTwo;
                    nodeTwo.RightSon = leftSon;
                    return nodeOne;
                }
                else
                {
                    var leftSon = nodeTwo.LeftSon;
                    nodeTwo.LeftSon = nodeOne;
                    nodeOne.RightSon = leftSon;
                    return nodeTwo;
                }
            }
            else if (nodeOne.Key.CompareTo(nodeTwo.Key) < 0)
            {
                var leftSon = nodeOne.LeftSon;
                nodeOne.LeftSon = nodeTwo;
                nodeTwo.RightSon = leftSon;
                return nodeOne;
            }
            else
            {
                var leftSon = nodeTwo.LeftSon;
                nodeTwo.LeftSon = nodeOne;
                nodeOne.RightSon = leftSon;
                return nodeTwo;
            }
        }

        public V GetMin()
        {
            var minimalNode = _root;
            var front = new Queue<PriorityHeapNode<K, P, V>>();
            PriorityHeapNode<K, P, V> prevNode = null;
            var tmpNode1 = _root.LeftSon;
            while (tmpNode1 != null)
            {
                front.Enqueue(tmpNode1);
                prevNode = tmpNode1;
                tmpNode1 = tmpNode1.RightSon;
                prevNode.RightSon = null;
            }

            if (front.Count == 0)
                _root = null;
            else
            {
                PriorityHeapNode<K, P, V> tmpNode2 = null;
                while (front.Count > 1)
                {
                    tmpNode1 = front.Dequeue();
                    tmpNode2 = front.Dequeue();
                    front.Enqueue(Pair(tmpNode1, tmpNode2));
                }
                _root = front.Dequeue();
            }
            --_numberOfNodes;
            return minimalNode.Value;
        }

        public void Reset()
        {
            _numberOfNodes = 0;
            _root = null;
        }




        private class PriorityHeapNode<K, P, V>
        where K : IComparable
        where P : IComparable

        {
            public K Key { get; set; }
            public V Value { get; set; }
            public P Priority { get; set; }
            public PriorityHeapNode<K, P, V> LeftSon { get; set; }
            public PriorityHeapNode<K, P, V> RightSon { get; set; }
            public PriorityHeapNode(K key, P priority, V value)
            {
                Key = key;
                Value = value;
                Priority = priority;
            }
        }
    }
}
