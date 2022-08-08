using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDataStructures
{
    /// <summary>
    /// Binary Search Tree that includes a key and a value.
    /// </summary>
    /// <typeparam name="K">An IComparable that represents the key to each value.</typeparam>
    /// <typeparam name="V">Represents the value.</typeparam>
    public class BinaryTree<K, V> : IEnumerable<V> where K : IComparable<K>
    {
        #region Node
        /// <summary>
        /// A nested class to construct a binary tree.
        /// </summary>
        private class BinaryNode
        {
            private K _key;
            private V _value;
            private BinaryNode left, right, father;
            public BinaryNode(K key, V data)
            {
                this._value = data;
                left = right = father = null;
                _key = key;
            }
            public V Value { get { return _value; } set { _value = value; } }
            public BinaryNode Left { get { return left; } set { left = value; } }
            public BinaryNode Right { get { return right; } set { right = value; } }
            public BinaryNode Father { get { return father; } set { father = value; } }
            public K Key { get { return _key; } set { _key = value; } }
        }
        #endregion

        private BinaryNode _root;
        public BinaryTree()
        {
            _root = null;
        }
        public bool IsEmpty { get { return _root == null; } }
        /// <summary>
        /// Add an item to the tree.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key, V value)
        {
            if (_root == null)
            {
                _root = new BinaryNode(key, value);
            }
            else
                Add(key, value, _root);
        }
        /// <summary>
        /// Recursive add function.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        private void Add(K key, V value, BinaryNode t)
        {
            if (key.CompareTo(t.Key) > 0)
            {

                if (t.Left == null)
                {
                    t.Left = new BinaryNode(key, value);
                    t.Left.Father = t;
                }
                else
                {
                    Add(key, value, t.Left);
                }
            }
            else if (key.CompareTo(t.Key) < 0)
            {
                if (t.Right == null)
                {
                    t.Right = new BinaryNode(key, value);
                    t.Right.Father = t;
                }
                else
                {
                    Add(key, value, t.Right);
                }
            }

        }

        #region Enumerators
        /// <summary>
        /// Implements the IEnumarable interface
        /// </summary>
        /// <returns></returns>
        public IEnumerator<V> GetEnumerator()
        {
            var stack = new Stack<BinaryNode>();
            var current = _root;
            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current.Right;
                }
                else
                {
                    current = stack.Pop();
                    yield return current.Value;
                    current = current.Left;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public void Remove(K key)
        {
            BinaryNode curr = FindNode(key);
            if (curr == null)
                return;
            if (curr.Father == null)
            {
                if (curr.Right != null)
                    _root = curr.Right;
                else if (curr.Left != null)
                    _root = curr.Left;
                else
                    _root = null;
                return;
            }
            if (curr.Left == null && curr.Right == null)
            {
                curr.Father.Left = null;
                curr.Father.Right = null;
                return;
            }
            if (curr.Left == null)
            {
                curr.Father.Right = curr.Right;
                return;
            }
            if (curr.Right == null)
            {
                curr.Father.Left = curr.Left;
                return;
            }
            var next = curr;
            while (next.Left != null)
            {
                next = next.Left;
            }
            if (next.Right != null)
            {
                next.Father.Left = next.Right;
            }
            if (next.Key.CompareTo(curr.Father.Key) > 0)
            {
                curr.Father.Left = next;
            }
            else
            {
                curr.Father.Right = next;
            }
            next.Right = curr.Right;
            next.Left = curr.Left;
            curr = null;

        }

        /// <summary>
        /// Recursivly finds a node by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private BinaryNode FindNode(K key)
        {
            return FindNode(key, _root);
        }

        private BinaryNode FindNode(K key, BinaryNode curr)
        {
            if (curr == null)
                return null;
            if (curr.Key.Equals(key))
                return curr;
            if (curr.Key.CompareTo(key) > 0)
                return FindNode(key, curr.Right);
            return FindNode(key, curr.Left);
        }
        /// <summary>
        /// Finds a value by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if a value was found, else false.</returns>
        public bool TryFind(K key, out V value)
        {
            return TryFind(key, _root, out value);
        }

        private bool TryFind(K key, BinaryNode curr, out V value)
        {
            if (curr == null)
            {
                value = default(V);
                return false;
            }
            if (curr.Key.Equals(key))
            {
                value = curr.Value;
                return true;
            }
            if (curr.Key.CompareTo(key) > 0)
                return TryFind(key, curr.Right, out value);
            return TryFind(key, curr.Left, out value);

        }
    }
}
