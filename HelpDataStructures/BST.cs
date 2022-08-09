using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDataStructures
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        #region Node
        /// <summary>
        /// A nested class to construct a binary tree.
        /// </summary>
        private class BinaryNode
        {
            private T _data;
            private BinaryNode left, right, father;
            public BinaryNode(T data)
            {
                
                left = right = father = null;
                Data = data;
            }
            public BinaryNode Left { get { return left; } set { left = value; } }
            public BinaryNode Right { get { return right; } set { right = value; } }
            public BinaryNode Father { get { return father; } set { father = value; } }
            public T Data { get { return _data; } set { _data = value; } }
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
        public void Add(T data)
        {
            if (_root == null)
            {
                _root = new BinaryNode(data);
            }
            else
                Add(data, _root);
        }
        /// <summary>
        /// Recursive add function.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        private void Add(T data, BinaryNode t)
        {
            if (data.CompareTo(t.Data) > 0)
            {

                if (t.Left == null)
                {
                    t.Left = new BinaryNode(data);
                    t.Left.Father = t;
                }
                else
                {
                    Add(data, t.Left);
                }
            }
            else if (data.CompareTo(t.Data) < 0)
            {
                if (t.Right == null)
                {
                    t.Right = new BinaryNode(data);
                    t.Right.Father = t;
                }
                else
                {
                    Add(data, t.Right);
                }
            }

        }

        #region Enumerators
        /// <summary>
        /// Implements the IEnumarable interface
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
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
                    yield return current.Data;
                    current = current.Left;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public void Remove(T data)
        {
            BinaryNode curr = FindNode(data);
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
            if (next.Data.CompareTo(curr.Father.Data) > 0)
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
        /// Recursivly finds a node by its data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private BinaryNode FindNode(T data)
        {
            return FindNode(data, _root);
        }

        private BinaryNode FindNode(T data, BinaryNode curr)
        {
            if (curr == null)
                return null;
            if (curr.Data.Equals(data))
                return curr;
            if (curr.Data.CompareTo(data) > 0)
                return FindNode(data, curr.Right);
            return FindNode(data, curr.Left);
        }
        /// <summary>
        /// Finds a value by its data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if a value was found, else false.</returns>
        public bool TryFind(T data, out T value)
        {
            return TryFind(data, _root, out value);
        }

        private bool TryFind(T key, BinaryNode curr, out T value)
        {
            if (curr == null)
            {
                value = default(T);
                return false;
            }
            if (curr.Data.Equals(key))
            {
                value = curr.Data;
                return true;
            }
            if (curr.Data.CompareTo(key) > 0)
                return TryFind(key, curr.Right, out value);
            return TryFind(key, curr.Left, out value);

        }
    }
}

