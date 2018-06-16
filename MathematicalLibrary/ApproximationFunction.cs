using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections;
using Mathematics.Objects;
using System.Text.RegularExpressions;

namespace Mathematics
{
    namespace Approximation
    {
        [StructLayout(LayoutKind.Auto), Serializable]
        public struct Node:IEquatable<Node>, IEnumerable<double>
        {
            #region FIELDS
            private readonly double _x;
            private readonly double _y;
            #endregion
            #region CONSTRUCTORS
            public Node(double x=0, double y=0)
            {
                _x = x;
                _y = y;
            }

            public Node(Node obj):this(obj._x, obj._y) { }
            #endregion
            #region METHODS
            public bool Equals(Node other)
            {
                return (_x == other._x) && (_y == other._y);
            }

            public IEnumerator<double> GetEnumerator()
            {
                yield return _x;
                yield return _y;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public override int GetHashCode()
            {
                return _x.GetHashCode() ^ _y.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return (obj is Node) ? Equals((Node)obj) : false;
            }

            public override string ToString()
            {
                return $"[X={_x};\tY={_y}]";
            }
            #endregion
            #region PROPERTIES
            public double X => _x;
            public double Y => _y;
            #endregion
            #region STATIC MEMBERS
            public static bool IsInfinity(Node node)
            {
                return double.IsInfinity(node.X) || double.IsInfinity(node.Y);
            }

            public static bool IsXPossitiveInfinity(Node node)
            {
                return double.IsPositiveInfinity(node._x) && node._y == 0.0;
            }

            public static bool IsYPossitiveInfinity(Node node)
            {
                return double.IsPositiveInfinity(node._y) && node._x == 0.0;
            }

            public static bool IsXNegativeInfinity(Node node)
            {
                return double.IsNegativeInfinity(node._x) && node._y == 0.0;
            }

            public static bool IsYNegativeInfinity(Node node)
            {
                return double.IsNegativeInfinity(node._y) && node._x == 0.0;
            }

            public static bool IsNaN(Node node)
            {
                return double.IsNaN(node._x) || double.IsNaN(node._y);
            }

            public static bool IsZero(Node node)
            {
                return node._x == 0.0 && node._y == 0.0;
            }

            public static Node Zero => new Node();

            public static Node Infinity => new Node(double.PositiveInfinity, double.PositiveInfinity);

            public static Node NaN => new Node(double.NaN, double.NaN);

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
                            string pattern = @"(([+-]?[\d]+([.,][\d]+)?);|([+-]?[\d]+([.,][\d]+)?))";
                            string[] coords = new string[2];
                            int i = 0;

                            foreach (Match m in Regex.Matches(s, pattern, RegexOptions.Compiled))
                            {
                                if (i > 1) { break; }
                                coords[i++] = m.Value;
                            }

                            coords[0] = Regex.Replace(coords[0], ";", string.Empty, RegexOptions.Compiled);

                            if ((string.IsNullOrEmpty(coords[0])) && (!string.IsNullOrEmpty(coords[1]))) { return new Node(0.0, double.Parse(coords[1])); }
                            else if (string.IsNullOrEmpty(coords[1]) && (!string.IsNullOrEmpty(coords[0]))) { return new Node(double.Parse(coords[0]), 0.0); }
                            return coords.Select(str => double.Parse(str)).ToArray<double>().ToNode();
                        }
                }
            }

            public static bool TryParse(string s, out Node node)
            {
                node = new Node();
                try
                {
                    node = Parse(s);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static implicit operator Node(Point2D obj)
            {
                return new Node(obj.X, obj.Y);
            }
            
            public static implicit operator Node(string s)
            {
                return Node.Parse(s);
            }
            #endregion
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public class SortedNodes:IEnumerable<Node>, IList<Node>, ICollection<Node>, IEquatable<SortedNodes>
        {
            #region FIELDS
            private List<Node> _nodes;
            private Func<Node, double> _keySelector = (node) => node.X;
            #endregion
            #region CONSTRUCTORS
            public SortedNodes() => _nodes = new List<Node>();
            public SortedNodes(IEnumerable<Node> nodes) => _nodes = SortedNodesMethod(nodes);
            public SortedNodes(int capacity) => _nodes = new List<Node>(capacity);
            #endregion;
            #region METHODS
            private List<Node> SortedNodesMethod(IEnumerable<Node> nodes)
            {
                return nodes.OrderBy(_keySelector).ToList();
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

            public int Count => _nodes.Count;

            public bool IsReadOnly => false;

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

            public IEnumerator<Node> GetEnumerator()
            {
                return _nodes.GetEnumerator();
            }

            public int IndexOf(Node item)
            {
                return _nodes.IndexOf(item);
            }

            public void Insert(int index, Node item)
            {
                _nodes.Insert(index, item);
            }

            public bool Remove(Node item)
            {
                return _nodes.Remove(item);
            }

            public void RemoveAt(int index)
            {
                _nodes.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Node[] ToArray()
            {
                return _nodes.ToArray();
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

            public bool Equals(SortedNodes other)
            {
                Node[] nodes = SortedNodesMethod(other._nodes).ToArray();

                IStructuralEquatable equatable = _nodes.ToArray();

                return equatable.Equals(nodes, StructuralComparisons.StructuralEqualityComparer);
            }

            public override bool Equals(object obj)
            {
                return (obj is SortedNodes) ? Equals((SortedNodes)obj): false;
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
                    builder.Append(el.ToString() + "\n");
                }

                return builder.ToString();
            }
            #endregion
            #region STATIC MEMBERS
            public static implicit operator SortedNodes(Node[] nodes)
            {
                return new SortedNodes(nodes);
            }
            #endregion
        }

        public enum TypeInterpolation
        {
            NewtonPolynomial, LagrangPolynomial, HermitPolynomial, SplineInterpolation 
        }

        public class Interpolation
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
        }
        internal static class Extensions
        {
            public static Node ToNode(this double[] array)
            {
                try
                {
                    return new Node(array[0], array[1]);
                }
                catch(IndexOutOfRangeException)
                {
                    return new Node();
                }
            }

            public static SortedNodes ToSortedNodes(this IEnumerable<Point2D> nodes)
            {
                return new SortedNodes(nodes.Select(p => new Node(p)));
            }
        }
    }
}
