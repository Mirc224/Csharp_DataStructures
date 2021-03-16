using System;
using System.Collections.Generic;
using System.Text;


namespace Csharp_data_structures.DataStructures.PairingHeap
{
    class PairingHeap<K, V> where K : IComparable
    {
        public K Peek { get => _root != null ? _root.Key : default(K); }
        private PairingHeapNode<K, V> _root = null;
        private int _numberOfNodes = 0;
        public int Count { get => _numberOfNodes; }

        public void Insert(K key, V value)
        {
            var newNode = new PairingHeapNode<K, V>(key, value);
            ++_numberOfNodes;
            if (_root == null)
            {
                _root = newNode;
                return;
            }
            _root = Pair(_root, newNode);

        }

        private PairingHeapNode<K, V> Pair(PairingHeapNode<K, V> nodeOne, PairingHeapNode<K, V> nodeTwo)
        {
            if (nodeOne.Key.CompareTo(nodeTwo.Key) < 0)
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
            var front = new Queue<PairingHeapNode<K, V>>();
            PairingHeapNode<K, V> prevNode = null;
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
                PairingHeapNode<K, V> tmpNode2 = null;
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

        private class PairingHeapNode<K, V> where K : IComparable
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public PairingHeapNode<K, V> LeftSon { get; set; }
            public PairingHeapNode<K, V> RightSon { get; set; }

            public PairingHeapNode(K key, V value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
