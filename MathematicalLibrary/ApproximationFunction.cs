using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections;
using Mathematics.Objects;
using System.Text.RegularExpressions;

namespace Mathematics
{
    namespace Approximation
    {
        [StructLayout(LayoutKind.Auto),Serializable]
        public struct Node:IEquatable<Node>,IComparable<Node>,IComparable
        {
            #region FIELD
            private readonly double _x;
            #endregion
            #region CONSTRUCTORS
            public Node(double x) => _x = x;
            public Node(Node obj) : this(obj._x) { }
            #endregion
            #region METHODS
            public bool Equals(Node other)
            {
                return _x.Equals(other._x);
            }

            public int CompareTo(Node other)
            {
                return _x.CompareTo(other._x);
            }

            int IComparable.CompareTo(object obj)
            {
                return (obj is Node) ? CompareTo((Node)obj) : -1;
            }

            public override bool Equals(object obj)
            {
                return (obj is Node) ? Equals((Node)obj) : false;
            }

            public override int GetHashCode()
            {
                return _x.GetHashCode();
            }

            public override string ToString()
            {
                return _x.ToString();
            }
            #endregion
            #region PROPERTI
            public double X => _x;
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Node Parse(string s)
            {
                s = s.Replace(" ", string.Empty).Replace(".", ",");
                switch (s)
                {
                    case "": return NaN;
                    case "{0}": return Zero;
                    case "{∞}": return Infinity;
                    case "{¿}": return NaN;
                    default:
                        {
                            return new Node(double.Parse(s));
                        }
                }                
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Node obj)
            {
                obj = new Node();
                try
                {
                    obj = Parse(s);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsInfinity(Node node)
            {
                return double.IsInfinity(node._x);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNaN(Node node)
            {
                return double.IsNaN(node._x);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZero(Node node)
            {
                return node._x == 0.0;
            }

            public static Node NaN => new Node(double.NaN);

            public static Node Infinity => new Node(double.PositiveInfinity);

            public static Node Zero => new Node();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(Node a, Node b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(Node a, Node b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator>(Node a, Node b)
            {
                return a._x > b._x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator<(Node a, Node b)
            {
                return a._x < b._x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator>=(Node a, Node b)
            {
                return a._x >= b._x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator<=(Node a, Node b)
            {
                return a._x <= b._x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Node operator++(Node a)
            {
                return new Node(a._x + 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Node operator--(Node a)
            {
                return new Node(a._x - 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Node(double x)
            {
                return new Node(x);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator double(Node n)
            {
                return n._x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Node(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Node a, Node b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Node Increment(Node a)
            {
                return new Node(a._x + 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Node Decrement(Node a)
            {
                return new Node(a._x - 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Compare(Node a, Node b)
            {
                return a > b;
            }
            #endregion
        }

        public sealed class SortedNodes:IEnumerable<Node>, IList<Node>, ICollection<Node>, IEquatable<SortedNodes>, IEnumerable, IList, ICollection
        {
            #region FIELD
            private List<Node> _nodes;
            private readonly Func<Node, double> _keySelector = (node) => node.X;
            #endregion
            #region CONSTRUCTORS
            public SortedNodes() => _nodes = new List<Node>();
            public SortedNodes(IEnumerable<Node> nodes) => _nodes = SortedNodesMethod(nodes);
            public SortedNodes(int capacity) => _nodes = new List<Node>(capacity);
            #endregion
            #region METHODS
            private List<Node> SortedNodesMethod(IEnumerable<Node> nodes)
            {
                return nodes.OrderBy(_keySelector).ToList();
            }

            public IEnumerator<Node> GetEnumerator()
            {
                return _nodes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int IndexOf(Node item)
            {
                return _nodes.IndexOf(item);
            }

            public void Insert(int index, Node item)
            {
                _nodes.Insert(index, item);
                _nodes = SortedNodesMethod(_nodes);
            }

            public void RemoveAt(int index)
            {
                _nodes.RemoveAt(index);
            }

            public void Add(Node item)
            {
                if(!_nodes.Contains(item))
                {
                    _nodes.Add(item);
                    _nodes = SortedNodesMethod(_nodes);
                }

                return;
            }

            public void AddRange(IEnumerable<Node> nodes)
            {
                _nodes.AddRange(nodes);
                _nodes = SortedNodesMethod(_nodes);
            }

            public void Clear()
            {
                _nodes.Clear();
            }

            public bool Contains(Node item)
            {
                return _nodes.Contains(item);
            }

            public void CopyTo(Node[] array, int arrayIndex)
            {
                _nodes.CopyTo(array, arrayIndex);
            }

            public bool Remove(Node item)
            {
                return _nodes.Remove(item);
            }

            int IList.Add(object value)
            {
                bool flag = (value is Node);

                if(flag)
                {
                    _nodes.Add((Node)value);
                    return _nodes.Count - 1;
                }
                else
                {
                    return -1;
                }
            }

            bool IList.Contains(object value)
            {
                return (value is Node) ? _nodes.Contains((Node)value) : false;
            }

            void IList.Clear()
            {
                _nodes.Clear();
            }

            int IList.IndexOf(object value)
            {
                bool flag = (value is Node);

                if (!flag) { return -1; }

                return _nodes.IndexOf((Node)value);
            }

            void IList.Insert(int index, object value)
            {
                _nodes.Insert(index, (Node)value);
            }

            void IList.Remove(object value)
            {
                _nodes.Remove((Node)value);
            }

            void IList.RemoveAt(int index)
            {
                _nodes.RemoveAt(index);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                _nodes.CopyTo((Node[])array, index);
            }

            public int Count => _nodes.Count;

            public bool IsReadOnly => false;

            bool IList.IsReadOnly => false;

            bool IList.IsFixedSize => false;

            int ICollection.Count => _nodes.Count;

            object ICollection.SyncRoot => false;

            bool ICollection.IsSynchronized => false;

            object IList.this[int index]
            {
                get => _nodes[index];
                set
                {
                    Node oldValue = _nodes[index];
                    bool flag = (value is Node);

                    if (!flag) { _nodes[index] = oldValue; return; }

                    _nodes[index] = (Node)value;
                    _nodes = SortedNodesMethod(_nodes);
                }
            }
            public Node this[int index]
            {
                get => _nodes[index];
                set
                {
                    _nodes[index] = value;
                    _nodes = SortedNodesMethod(_nodes);
                }
            }

            public bool Equals(SortedNodes other)
            {
                IStructuralEquatable se = _nodes.ToArray();

                return se.Equals(other._nodes.ToArray(), StructuralComparisons.StructuralEqualityComparer);
            }

            public override bool Equals(object obj)
            {
                return (obj is SortedNodes) ? Equals((SortedNodes)obj) : false;
            }

            public override int GetHashCode()
            {
                int hash = 17;

                foreach(var el in _nodes)
                {
                    hash *= (hash * 31) + el.GetHashCode();
                }

                return hash;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                foreach(var el in _nodes)
                {
                    builder.Append(el.X.ToString() + "\n");
                }

                return builder.ToString();
            }

            public Node[] ToArraySortedNodes()
            {
                return _nodes.ToArray();
            }

            public void Reverse(int startIndex, int count)
            {
                _nodes.Reverse(startIndex, count);
            }

            public void Reverse()
            {
                _nodes.Reverse();
            }

            public int RemoveAll(Predicate<Node> match)
            {
                return _nodes.RemoveAll(match);
            }

            public int LastIndexOf(Node node, int index, int count)
            {
                return _nodes.LastIndexOf(node, index, count);
            }

            public int LastIndexOf(Node node, int index)
            {
                return _nodes.LastIndexOf(node, index);
            }

            public int LastIndexOf(Node node)
            {
                return _nodes.LastIndexOf(node);
            }

            public int IndexOf(Node node, int index, int count)
            {
                return _nodes.IndexOf(node, index, count);
            }

            public int IndexOf(Node node, int index)
            {
                return _nodes.IndexOf(node, index);
            }

            public void ForEach(Action<Node> action)
            {
                _nodes.ForEach(action);
            }

            public int FindLastIndex(Predicate<Node> match)
            {
                return _nodes.FindLastIndex(match);
            }

            public int FindLastIndex(int startIndex, Predicate<Node> match)
            {
                return _nodes.FindLastIndex(startIndex, match);
            }

            public int FindLastIndex(int startIndex, int count, Predicate<Node> match)
            {
                return _nodes.FindLastIndex(startIndex, count, match);
            }

            public Node FindLast(Predicate<Node> match)
            {
                return _nodes.FindLast(match);
            }

            public int FindIndex(Predicate<Node> match)
            {
                return _nodes.FindIndex(match);
            }

            public int FindIndex(int startIndex, Predicate<Node> match)
            {
                return _nodes.FindIndex(startIndex, match);
            }

            public int FindIndex(int startIndex, int count, Predicate<Node> match)
            {
                return _nodes.FindIndex(startIndex, count, match);
            }

            public List<Node> FindAll(Predicate<Node> match)
            {
                return _nodes.FindAll(match);
            }

            public Node Find(Predicate<Node> match)
            {
                return _nodes.Find(match);
            }

            public bool Exists(Predicate<Node> match)
            {
                return _nodes.Exists(match);
            }
            #endregion
        }

        public enum TypeInterpolation
        {
            NewtonPolynomial, LagrangPolynomial, HermitPolynomial, SplineInterpolation 
        }

        /*public class Interpolation
        {
            #region FIELDS
            private SortedNodes _nodes;
            private TypeInterpolation _type;
            private Function _baseFunction;
            #endregion
            #region Constructors
            public Interpolation(TypeInterpolation type, IEnumerable<Node> nodes, Function baseFunction)
            {
                _type = type;
                _nodes = (SortedNodes)nodes;
                _baseFunction = baseFunction;
            }

            public Interpolation(IEnumerable<Node> nodes, Function baseFunction):this(TypeInterpolation.NewtonPolynomial, nodes, baseFunction) { }

            public Interpolation(TypeInterpolation type, IEnumerable<Point2D> nodes, Function baseFunction)
            {
                _type = type;
                _nodes = nodes.ToSortedNodes();
                _baseFunction = baseFunction;
            }

            public Interpolation(IEnumerable<Point2D> nodes, Function baseFunction):this(TypeInterpolation.NewtonPolynomial, nodes, baseFunction) { }
            #endregion
        }*/
    }
}
