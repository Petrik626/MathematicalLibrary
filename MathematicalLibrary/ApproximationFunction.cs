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
        }
    }
}
