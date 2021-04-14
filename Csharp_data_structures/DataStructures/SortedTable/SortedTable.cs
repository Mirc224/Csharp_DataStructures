using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Csharp_data_structures.DataStructures.SortedTable
{
    public class SortedTable<K, T> : IEnumerable
        where K: IComparable
    {
        protected List<TableItem<K, T>> _list = new List<TableItem<K, T>>();
        public int Count { get => _list.Count; }

        public void Insert(K key, T data)
        {
            KeyValuePair<int, bool> result = IndexOfKey(key, 0, _list.Count);

            if (!result.Value)
            {
                TableItem<K, T> newItem = new TableItem<K, T>(key, data);
                _list.Insert(result.Key, newItem);
            }
            else
            {
                throw new Exception(this.GetType().ToString() + ": Insert - item already in table!");
            }
        }

        protected TableItem<K, T> FindTableItem(K key)
        {
            if(_list.Count == 0)
            {
                return null;
            }

            KeyValuePair<int, bool> result = IndexOfKey(key, 0, _list.Count);

            return result.Value ? _list[result.Key] : null;
        }

        public T Find(K key)
        {
            TableItem<K, T> result = FindTableItem(key);
            if (result != null)
            {
                return result.getData();
            }

            throw new Exception(this.GetType().ToString() + ": Find - item not found!");
        }

        public T Remove(K key)
        {
            TableItem<K, T> tableItem = FindTableItem(key);
            if (tableItem != null)
            {
                _list.Remove(tableItem);
                return tableItem.Data;
            }
            else
            {
                throw new Exception(this.GetType().ToString() + ": Remove - item not found!");
            }
        }

        public KeyValuePair<int, bool> IndexOfKey(K key, int indexStart, int indexEnd)
        {
            int indexSize = _list.Count;
            int pivot = -1;
            K keyAtPivot;
            while(true)
            {
                if (indexStart == indexSize)
                {
                    return new KeyValuePair<int, bool>(indexSize, false);
                }
                pivot = (indexStart + indexEnd) / 2;

                keyAtPivot = _list[pivot].Key;

                if (keyAtPivot.CompareTo(key) == 0)
                {
                    return new KeyValuePair<int, bool>(pivot, true);
                }
                else
                {

                    if (indexStart == indexEnd)
                    {
                        return new KeyValuePair<int, bool>((key.CompareTo(keyAtPivot) < 0 ? pivot : pivot + 1), false);
                    }
                    else
                    {
                        if (keyAtPivot.CompareTo(key) < 0)
                        {
                            indexStart = pivot + 1;
                        }
                        else
                        {
                            indexEnd = pivot;
                        }
                    }
                }
            }
        }

        public void CutOff(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= _list.Count || fromIndex > toIndex || toIndex > _list.Count)
                return;
            //List<TableItem<K,T>> removedItems = this.list.subList(fromIndex,toIndex + 1);
            List<TableItem<K, T>> newList = new List<TableItem<K, T>>(_list.GetRange(0, fromIndex));
            newList.AddRange(_list.GetRange(toIndex, _list.Count - toIndex));
            _list = newList;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SortedTableIterator<K, T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)new SortedTableIterator<K, T>(this);
        }

        protected class SortedTableIterator<K, T> : IEnumerator<T>
            where K: IComparable
        {
            private SortedTable<K,T> table;
            private int next = -1;
            public SortedTableIterator(SortedTable<K, T> table)
            {
                this.table = table;
            }

            public T Current => table._list[next].Data;

            object IEnumerator.Current => table._list[next].Data;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return ++next < table._list.Count;
            }

            public void Reset()
            {
                next = -1;
            }
        }


    protected class TableItem<K, T>
        where K : IComparable
        {
            public K Key { get; set; }
            public T Data { get; set; }

            public TableItem(K key, T data)
            {
                this.Key = key;
                this.Data = data;
            }

            public K getKey()
            {
                return Key;
            }

            public void setKey(K key)
            {
                this.Key = key;
            }

            public T getData()
            {
                return Data;
            }

            public void setData(T data)
            {
                this.Data = data;
            }
        }
    }

}
