using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KD_Tree
{
    public class KDTree<K, T, P> : IEnumerable<T>
        where K : IComparable
        where P : IComparable //implements Iterable<T> 
    {

        protected KDTreeNode<K, T, P> Root { get; private set; } = null;
        private int _numberOfKeys = 0;
        public int Count { get; private set; } = 0;

        /**
         * Konštruktor triedy. Nastaví sa hodnota atribútu numberOfKeys.
         *
         * @param numberOfKeys udáva koľko kľúčov bude štruktúra podporovať.
         */
        public KDTree(int numberOfKeys)
        {
            this._numberOfKeys = numberOfKeys;
        }

        /**
         * Metóda na vkladanie dát to štruktúry.
         *
         * @param keys
         *        pole kľúčov, na základe ktorých sú dáta v strome vložené na správne miesto.
         *
         * @param element
         *        dáta, ktoré majú byť v strome uložené.
         *
         * @return vráti null ak sa veľkosť poľa poskytnutých kľúčov líši od hodnnoty atribútu
         *         numberOfKeys. Ak je táto podmienka splnená, metóda vracia KDTreeNode, s
         *         novo vloženými dátami.
         */
        public bool Insert(K[] keys, T element, P primaryKey)
        {
            if (keys.Length != this._numberOfKeys)
                return false;

            if (this.Root == null)
            {
                this.Root = new KDTreeNode<K, T, P>(keys, element, primaryKey, 0);
                ++Count;
                return true;
            }


            int level = 0;
            KDTreeNode<K, T, P> node;
            KDTreeNode<K, T, P> parent = this.Root;
            int dimension = 0;


            while (true)
            {

                dimension = level++ % _numberOfKeys;
                if (keys[dimension].CompareTo(parent.Keys[dimension]) <= 0)
                {
                    if (parent.LeftSon == null)
                    {
                        node = new KDTreeNode<K, T, P>(keys, element, primaryKey, level);
                        node.Parent = parent;
                        parent.LeftSon = node;
                        ++Count;
                        return true;
                    }
                    else
                    {
                        parent = parent.LeftSon;
                    }
                }
                else
                {
                    if (parent.RightSon == null)
                    {
                        node = new KDTreeNode<K, T, P>(keys, element, primaryKey, level);
                        node.Parent = parent;
                        parent.RightSon = node;
                        ++Count;
                        return true;
                    }
                    else
                    {
                        parent = parent.RightSon;
                    }
                }
            }
        }


        /**
         * Verejná metóda pre vyhľadávanie dát na základe poskytnutého sekundárneho kľúča.
         *
         * @param keys
         *        obsahuje pole kľúčov na základe ktorého má byť prvok vyhľadaný.
         *
         * @return
         */
        public List<T> FindData(K[] keys)
        {
            return this.FindDataInRange(keys, keys);
        }

        /**
         * Metóda na intervalové vyhľadávanie. Vráti arraylist obsahujúci dáta, ktoré sa nachádzali
         * v danom rozsahu.
         *
         * @param lowerBound
         *        množina hodnôt, ktoré predstavujú spodnú hranicu pre jednotlivé kľúče v závislosti od dimenzie.
         *
         * @param upperBound
         *        množina hodnôt, ktoré predstavujú hornú hranicu pre jednotlivé kľúče v závislosti od dimenzie.
         *
         * @return array list obsahujúci dáta prvkov nachádzajúcích sa v zadanom rozsahu. V prípade ak bol
         *         zadaný nesprávny počet kľúčov je vrátená hodnota null.
         */
        public List<T> FindDataInRange(K[] lowerBound, K[] upperBound)
        {
            if (this._numberOfKeys != lowerBound.Length || this._numberOfKeys != upperBound.Length)
            {
                return null;
            }

            var result = new List<T>();
            if (this.Root == null)
            {
                return result;
            }
            int depth = 0;
            int dimension = depth % this._numberOfKeys;
            bool layInRange = false;
            Queue<KDTreeNode<K, T, P>> queueOfNodes = new Queue<KDTreeNode<K, T, P>>();
            queueOfNodes.Enqueue(this.Root);
            KDTreeNode<K, T, P> currentNode = null;
            KDTreeNode<K, T, P> tmpNode = null;

            // TODO:
            while (queueOfNodes.Count != 0)
            {
                currentNode = queueOfNodes.Dequeue();
                layInRange = true;
                dimension = currentNode.getLevel() % this._numberOfKeys;
                for (int keyNumber = 0; keyNumber < this._numberOfKeys; ++keyNumber)
                {
                    if (lowerBound[keyNumber].CompareTo(currentNode.Keys[keyNumber]) <= 0 &&
                            upperBound[keyNumber].CompareTo(currentNode.Keys[keyNumber]) >= 0)
                    {
                        continue;
                    }
                    else
                    {
                        layInRange = false;
                        break;
                    }
                }
                if (layInRange)
                {
                    result.Add(currentNode.Data);
                }


                if (lowerBound[dimension].CompareTo(currentNode.Keys[dimension]) <= 0)
                {
                    tmpNode = currentNode.LeftSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }

                if (currentNode.Keys[dimension].CompareTo(upperBound[dimension]) <= 0)
                {
                    tmpNode = currentNode.RightSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }
            }

            return result;
        }

        /**
         * Verejná metóda delete slúži na zmazanie prvku na základe poskytnutých sekundárnych kľúčov a primárneho
         * kľúča, ktorý slúži na presnú identifikáciu prvku.
         *
         * @param keys
         *        sekundárne kľúče, podľa ktorých sa vyhľadajú všetky KDTNode, ktoré majú rovnaký kľúč.
         *
         * @param primaryKey
         *        primárny kľúč, ktorý umožňuje presne identifikovať prvok, ktorý má byť zo zoznamu odstránený.
         *
         * @return vráti dáta vrcholu, ktorý bol odstránený. V prípade ak nebol nájdený žiaden vrchol alebo
         *         sa zadaný primárny kľúč nezhodoval s primárnym kľúčom žiadneho z najdených prvkov je
         *         vrátená hodnota null.
         */
        public T Delete(K[] keys, P primaryKey)
        {
            LinkedList<KDTreeNode<K, T, P>> result = this.Search(keys);

            if (result.Count == 0)
            {
                return default(T);
            }

            KDTreeNode<K, T, P> deleted = null;
            foreach (KDTreeNode<K, T, P> node in result)
            {
                if (node.PrimaryKey.Equals(primaryKey))
                {
                    deleted = node;
                    break;
                }
            }
            if (deleted == null)
                return default(T);

            T data = deleted.Data;
            this.Delete(deleted);

            return data;
        }

        /**
         * Metóda umožňuje vymazanie prvku, ktorý je poslaný do funkcie ako parameter. Pred mazaním
         * je preto potrebné zavolať metódu search a nájsť prvok, ktorý chceme vymazať.
         *
         * @param deletedNode
         *        obsahuje referenciu na vrchol stromu, ktorý sa má odstrániť.
         *
         * @return vracia hodnotu boolean v závislosti od toho, či sa podarilo prvok vymazať.
         */
        protected bool Delete(KDTreeNode<K, T, P> deletedNode)
        {
            // Inicializácia dočasných premenných, ktoré su počas behu používané.
            KDTreeNode<K, T, P> tmpNode = deletedNode;
            KDTreeNode<K, T, P> parentNode;
            LinkedList<KDTreeNode<K, T, P>> tmpList;
            while (true)
            {

                if (deletedNode.isLeaf())
                {
                    parentNode = deletedNode.Parent;
                    if (parentNode != null)
                    {
                        if (parentNode.LeftSon == deletedNode)
                        {
                            parentNode.LeftSon = null;
                        }
                        else if (parentNode.RightSon == deletedNode)
                        {
                            parentNode.RightSon = null;
                        }
                        deletedNode.Parent = null;
                    }
                    else
                    {
                        this.Root = null;
                    }
                    --Count;
                    return true;
                }

                tmpList = FindMaximal(deletedNode, true);
                int maximalDepth = 0;
                if (tmpList.Count != 0)
                {
                    tmpNode = tmpList.First.Value;
                    deletedNode.Keys = tmpNode.Keys;
                    deletedNode.Data = tmpNode.Data;
                    deletedNode.PrimaryKey =tmpNode.PrimaryKey;
                    deletedNode = tmpNode;
                }
                else
                {
                    tmpList = FindMaximal(deletedNode, false);
                    if (tmpList.Count != 0)
                    {
                        tmpNode = tmpList.First.Value;
                        deletedNode.Keys = tmpNode.Keys;
                        deletedNode.Data = tmpNode.Data;
                        deletedNode.PrimaryKey =tmpNode.PrimaryKey;
                        if (deletedNode.LeftSon == null)
                        {
                            deletedNode.LeftSon = deletedNode.RightSon;
                            deletedNode.RightSon = null;
                        }
                        deletedNode = tmpNode;
                    }
                }
            }
        }



        /**
         * Metóda na bodové vyhľadávanie.
         *
         * @param keys
         *        pole kľúčov, podľa ktorých budeme hľadať prvok.
         *
         * @return list nájdených vrcholov (pretože sú dovoloené aj duplicity). V prípade ak nebol vrchol
         *         nájdeny, je tento list prázdny.
         */
        private LinkedList<KDTreeNode<K, T, P>> Search(K[] keys)
        {
            return rangeSearch(keys, keys);
        }


        /**
         * Metóda na intervalové vyhľadávanie. Vráti linked list obsahujúci vrcholy, ktoré sa nachádzali
         * v danom rozsahu.
         *
         * @param lowerBound
         *        množina hodnôt, ktoré predstavujú spodnú hranicu pre jednotlivé kľúče v závislosti od dimenzie.
         *
         * @param upperBound
         *        množina hodnôt, ktoré predstavujú hornú hranicu pre jednotlivé kľúče v závislosti od dimenzie.
         *
         * @return linked-list obsahujúci prvky nachádzajúce sa v zadanom rozsahu. V prípade ak bol
         *         zadaný nesprávny počet kľúčov je vrátená hodnota null.
         */
        private LinkedList<KDTreeNode<K, T, P>> rangeSearch(K[] lowerBound, K[] upperBound)
        {
            if (this._numberOfKeys != lowerBound.Length || this._numberOfKeys != upperBound.Length)
            {
                return null;
            }

            LinkedList<KDTreeNode<K, T, P>> result = new LinkedList<KDTreeNode<K, T, P>>();
            if (this.Root == null)
            {
                return result;
            }

            int depth = 0;
            int dimension = depth % this._numberOfKeys;
            bool layInRange = false;
            Queue<KDTreeNode<K, T, P>> queueOfNodes = new Queue<KDTreeNode<K, T, P>>();
            queueOfNodes.Enqueue(this.Root);
            KDTreeNode<K, T, P> currentNode = null;
            KDTreeNode<K, T, P> tmpNode = null;

            while (queueOfNodes.Count != 0)
            {
                currentNode = queueOfNodes.Dequeue();
                layInRange = true;
                dimension = currentNode.getLevel() % this._numberOfKeys;
                for (int keyNumber = 0; keyNumber < this._numberOfKeys; ++keyNumber)
                {
                    if (lowerBound[keyNumber].CompareTo(currentNode.Keys[keyNumber]) <= 0 &&
                            upperBound[keyNumber].CompareTo(currentNode.Keys[keyNumber]) >= 0)
                    {
                        continue;
                    }
                    else
                    {
                        layInRange = false;
                        break;
                    }
                }
                if (layInRange)
                {
                    result.AddLast(currentNode);
                }

                if (lowerBound[dimension].CompareTo(currentNode.Keys[dimension]) <= 0)
                {
                    tmpNode = currentNode.LeftSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }
                if (currentNode.Keys[dimension].CompareTo(upperBound[dimension]) <= 0)
                {
                    tmpNode = currentNode.RightSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }
            }

            return result;
        }


        /**
         * Ide o pomocnú metódu, ktorá sa využíva pri mazaní prvkov zo stromu. Slúži na vyhľadanie maximálnej
         * hodnoty v niektorom z podstromov zadaného vrcholu.
         *
         * @param startingNode
         *        odkaz na vrchol, v ktorého podstromoch je hľadaná maximálna hodnota kľúča v závislosti od dimenzie.
         *
         * @param left
         *        parameter slúži na identifikáciu toho, v ktorom podstrome sa má maximum hľadať.
         *
         * @return
         */
        protected LinkedList<KDTreeNode<K, T, P>> FindMaximal(KDTreeNode<K, T, P> startingNode, bool left)
        {
            int actualLevel = startingNode.getLevel();
            int wantedDimension = actualLevel % _numberOfKeys;

            Queue<KDTreeNode<K, T, P>> queueOfNodes = new Queue<KDTreeNode<K, T, P>>();
            LinkedList<KDTreeNode<K, T, P>> result = new LinkedList<KDTreeNode<K, T, P>>();
            KDTreeNode<K, T, P> currentNode;

            if (left)
                currentNode = startingNode.LeftSon;
            else
            {
                currentNode = startingNode.RightSon;
            }

            if (currentNode == null)
            {
                return result;
            }
            K maximalFound = currentNode.Keys[wantedDimension];
            KDTreeNode<K, T, P> tmpNode;
            queueOfNodes.Enqueue(currentNode);

            while (queueOfNodes.Count != 0)
            {

                currentNode = queueOfNodes.Dequeue();
                actualLevel = currentNode.getLevel();
                if (currentNode.Keys[wantedDimension].CompareTo(maximalFound) > 0)
                {
                    maximalFound = currentNode.Keys[wantedDimension];
                    result.Clear();
                    result.AddLast(currentNode);
                }
                else if (currentNode.Keys[wantedDimension].CompareTo(maximalFound) == 0)
                {
                    result.AddLast(currentNode);
                }

                if (wantedDimension == actualLevel % _numberOfKeys)
                {
                    tmpNode = currentNode.RightSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }
                else
                {
                    tmpNode = currentNode.LeftSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                    tmpNode = currentNode.RightSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }
            }
            return result;
        }

        /**
         * Tato metóda sa v štruktúre nepoužíva. Umožňuje nájsť minimálnu hodnotu v podstrome poskytnutého vrcholu.
         *
         * @param startingNode
         *        odkaz na vrchol, v ktorého podstromoch je hľadaná minimalna hodnota kľúča v závislosti od dimenzie.
         *
         * @param left
         *        parameter slúži na identifikáciu toho, v ktorom podstrome sa má minimum hľadať.
         *
         * @return
         */
        protected LinkedList<KDTreeNode<K, T, P>> FindMinimal(KDTreeNode<K, T, P> startingNode, bool left)
        {
            int actualLevel = startingNode.getLevel();
            int wantedDimension = actualLevel % _numberOfKeys;

            Queue<KDTreeNode<K, T, P>> queueOfNodes = new Queue<KDTreeNode<K, T, P>>();
            LinkedList<KDTreeNode<K, T, P>> result = new LinkedList<KDTreeNode<K, T, P>>();
            KDTreeNode<K, T, P> currentNode = null;
            if (left)
            {
                currentNode = startingNode.LeftSon;
            }
            else
            {
                currentNode = startingNode.RightSon;
            }


            if (currentNode == null)
            {
                return result;
            }

            K minimalFound = currentNode.Keys[wantedDimension];
            KDTreeNode<K, T, P> tmpNode;
            queueOfNodes.Enqueue(currentNode);
            while (queueOfNodes.Count != 0)
            {

                currentNode = queueOfNodes.Dequeue();
                actualLevel = currentNode.getLevel();
                if (currentNode.Keys[wantedDimension].CompareTo(minimalFound) < 0)
                {
                    minimalFound = currentNode.Keys[wantedDimension];
                    result.Clear();
                    result.AddLast(currentNode);
                }
                else if (currentNode.Keys[wantedDimension].CompareTo(minimalFound) == 0)
                {
                    result.AddLast(currentNode);
                }

                if (wantedDimension == actualLevel % _numberOfKeys)
                {
                    tmpNode = currentNode.LeftSon;
                    if (tmpNode != null)
                    {
                        queueOfNodes.Enqueue(tmpNode);
                    }
                }
                else
                    tmpNode = currentNode.LeftSon;
                if (tmpNode != null)
                {
                    queueOfNodes.Enqueue(tmpNode);
                }
                tmpNode = currentNode.RightSon;
                if (tmpNode != null)
                {
                    queueOfNodes.Enqueue(tmpNode);
                }
            }
            return result;
        }


        /**
         * Ide o metódu, ktorá v LinkedListe vráti KDTreeNode stromu v poradí in-order.
         *
         * @return linked-list vrcholov podľa zoradenia in-order.
         */
        protected LinkedList<KDTreeNode<K, T, P>> InOrder()
        {
            LinkedList<KDTreeNode<K, T, P>> result = new LinkedList<KDTreeNode<K, T, P>>();
            if (this.Root == null)
            {
                return result;
            }

            Stack<KDTreeNode<K, T, P>> stack = new Stack<KDTreeNode<K, T, P>>();

            KDTreeNode<K, T, P> currentNode = this.Root;

            bool hotovo = false;
            while (!hotovo)
            {
                if (currentNode != null)
                {
                    stack.Push(currentNode);
                    currentNode = currentNode.LeftSon;
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        hotovo = true;
                    }
                    else
                    {
                        currentNode = stack.Pop();
                        result.AddLast(currentNode);
                        currentNode = currentNode.RightSon;
                    }
                }
            }
            return result;
        }

        /**
         * Ide o metódu, ktorá v LinkedListe vráti KDTreeNode stromu v poradí post-order.
         *
         * @return linked-list vrcholov podľa zoradenia post-order.
         */
        protected LinkedList<KDTreeNode<K, T, P>> PostOreder()
        {
            LinkedList<KDTreeNode<K, T, P>> result = new LinkedList<KDTreeNode<K, T, P>>();
            if (this.Root == null)
            {
                return result;
            }
            Stack<KDTreeNode<K, T, P>> stack = new Stack<KDTreeNode<K, T, P>>();
            KDTreeNode<K, T, P> currentNode;
            KDTreeNode<K, T, P> previousNode = null;
            stack.Push(this.Root);
            while (stack.Count != 0)
            {
                currentNode = stack.Peek();
                if (previousNode == null || currentNode == previousNode.LeftSon || currentNode == previousNode.RightSon)
                {
                    if (currentNode.LeftSon != null)
                    {
                        stack.Push(currentNode.LeftSon);
                    }
                    else if (currentNode.RightSon != null)
                    {
                        stack.Push(currentNode.RightSon);
                    }
                }
                else if (previousNode == currentNode.LeftSon)
                {
                    if (currentNode.RightSon != null)
                    {
                        stack.Push(currentNode.RightSon);
                    }
                }
                else
                {
                    stack.Pop();
                    result.AddLast(currentNode);
                }
                previousNode = currentNode;
            }
            return result;
        }

        protected LinkedList<KDTreeNode<K, T, P>> PreOrder()
        {
            LinkedList<KDTreeNode<K, T, P>> result = new LinkedList<KDTreeNode<K, T, P>>();
            if (this.Root == null)
            {
                return result;
            }
            Stack<KDTreeNode<K, T, P>> stack = new Stack<KDTreeNode<K, T, P>>();
            KDTreeNode<K, T, P> currentNode;
            KDTreeNode<K, T, P> tmpVrchol;
            stack.Push(this.Root);
            while (stack.Count != 0)
            {
                currentNode = stack.Pop();
                result.AddLast(currentNode);

                tmpVrchol = currentNode.RightSon;
                if (tmpVrchol != null)
                {
                    stack.Push(tmpVrchol);
                }
                tmpVrchol = currentNode.LeftSon;
                if (tmpVrchol != null)
                {
                    stack.Push(tmpVrchol);
                }
            }
            return result;
        }

        /**
         * Metóda ktorá prechádza vrcholy v poradí level order.
         *
         * @return linked list naplnený dátami vrcholov v poradí level-order.
         */
        public LinkedList<T> LevelOrder()
        {
            Queue<KDTreeNode<K, T, P>> queue = new Queue<KDTreeNode<K, T, P>>();
            LinkedList<T> result = new LinkedList<T>();

            KDTreeNode<K, T, P> currentNode;
            if (this.Root != null)
            {
                queue.Enqueue(this.Root);
            }

            while (queue.Count != 0)
            {
                currentNode = queue.Dequeue();
                if (currentNode != null)
                {
                    result.AddLast(currentNode.Data);
                    queue.Enqueue(currentNode.LeftSon);
                    queue.Enqueue(currentNode.RightSon);
                }
            }
            return result;
        }

        /**
         * Metodá vyčistí strom.
         */
        public void Clear()
        {
            this.Root = null;
            this.Count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new KDTInOrderIterator<K, T, P>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new KDTInOrderIterator<K, T, P>(this);
        }

        class KDTInOrderIterator<K, T, P> : IEnumerator<T>
        where K : IComparable
        where P : IComparable
        {
            KDTreeNode<K, T, P> next;

            public T Current
            {
                get
                {
                    KDTreeNode<K, T, P> current = next;

                    if (next.RightSon != null)
                    {
                        next = next.RightSon;
                        while (next.LeftSon != null)
                        {
                            next = next.LeftSon;
                        }
                        return current.Data;
                    }


                    while (true)
                    {
                        if (next.Parent == null)
                        {
                            next = null;
                            return current.Data;
                        }
                        if (next.Parent.LeftSon == next)
                        {
                            next = next.Parent;
                            return current.Data;
                        }
                        next = next.Parent;
                    }
                }
            }

            object IEnumerator.Current => next.Data;

            public KDTInOrderIterator(KDTree<K, T, P> tree)
            {
                next = tree.Root;
                if (next == null)
                {
                    return;
                }

                while (next.LeftSon != null)
                {
                    next = next.LeftSon;
                }
            }

            public bool MoveNext()
            {
                return next != null;
            }

            public void Reset()
            {
                next = null;
            }

            public void Dispose()
            {
            }
        }
    }




    /*
        class KDTLevelOrderIterator<K extends Comparable, T, P> implements Iterator<T> {
            Queue<KDTreeNode<K, T, P>> queue = new LinkedList<KDTreeNode<K, T, P>>();

        public KDTLevelOrderIterator(KDTree<K, T, P> tree)
        {
            KDTreeNode<K, T, P> root = tree.getRoot();
            if (root == null)
            {
                return;
            }
            queue.Add(root);
        }

        @Override
            public boolean hasNext()
        {
            return !queue.isEmpty();
        }

        @Override
            public T next()
        {
            KDTreeNode<K, T, P> current = queue.peek();
            KDTreeNode<K, T, P> tmp;
            if (current != null)
            {
                tmp = current.LeftSon;
                if (tmp != null)
                {
                    queue.Add(tmp);
                }
                tmp = current.RightSon;
                if (tmp != null)
                {
                    queue.Add(tmp);
                }
            }
            return current.Data;
        }
        }*/


    public class KDTreeNode<K, T, P>
        where K : IComparable
        where P : IComparable
    {
        private int level;
        public T Data { get; set; }
        public K[] Keys { get; set; }
        public P PrimaryKey { get; set; }
        public KDTreeNode<K, T, P> Parent { get; set; } = null;
        public KDTreeNode<K, T, P> LeftSon { get; set; } = null;
        public KDTreeNode<K, T, P> RightSon { get; set; } = null;

        public KDTreeNode(K[] keys, T data, P primaryKey, int level)
        {
            this.Data = data;
            this.Keys = keys;
            this.level = level;
            this.PrimaryKey = primaryKey;
        }

        public bool isLeaf()
        {
            return this.LeftSon == null && this.RightSon == null;
        }

        public bool isRoot()
        {
            return this.Parent == null;
        }

        public int getLevel()
        {
            return level;
        }

        public void setLevel(int level)
        {
            this.level = level;
        }

    }

}
