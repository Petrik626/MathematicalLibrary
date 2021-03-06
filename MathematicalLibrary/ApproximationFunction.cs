﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections;
using Mathematics.Objects;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Mathematics
{
    namespace Approximation
    {
        [StructLayout(LayoutKind.Auto), Serializable]
        public struct Node:IEquatable<Node>,IComparable<Node>,IComparable,ISerializable
        {
            #region FIELD
            private readonly double _x;
            #endregion
            #region CONSTRUCTORS
            public Node(double x) => _x = x;
            public Node(Node obj) : this(obj._x) { }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            private Node(SerializationInfo info, StreamingContext context)
            {
                _x = info.GetDouble("Value");
            }
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

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Value", _x);
            }
            #endregion
            #region PROPERTI
            public double Value => _x;
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

        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class SortedNodes:IEnumerable<Node>, IList<Node>, ICollection<Node>, IEquatable<SortedNodes>, IEnumerable, IList, ICollection, ISerializable
        {
            #region FIELDS
            private List<Node> _nodes;
            private readonly Func<Node, double> _keySelector = (node) => node.Value;
            #endregion
            #region CONSTRUCTORS
            public SortedNodes() => _nodes = new List<Node>();
            public SortedNodes(IEnumerable<Node> nodes) => _nodes = SortedNodesMethod(nodes);
            public SortedNodes(int capacity) => _nodes = new List<Node>(capacity);

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            private SortedNodes(SerializationInfo info, StreamingContext context)
            {
                _nodes = (List<Node>)info.GetValue("Nodes", typeof(List<Node>));
            }
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
                    builder.Append(el.Value.ToString() + "\n");
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

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Nodes", _nodes, typeof(List<Node>));
            }
            #endregion
            #region PROPERTIES
            public int Count => _nodes.Count;

            public int Capacity => _nodes.Capacity;

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

            [IndexerName("Node")]
            public Node this[int index]
            {
                get => _nodes[index];
                set
                {
                    _nodes[index] = value;
                    _nodes = SortedNodesMethod(_nodes);
                }
            }
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(SortedNodes a, SortedNodes b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(SortedNodes a, SortedNodes b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator SortedNodes(List<Node> nodes)
            {
                return new SortedNodes(nodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator SortedNodes(Node[] nodes)
            {
                return new SortedNodes(nodes);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator List<Node>(SortedNodes nodes)
            {
                return nodes._nodes;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Node[](SortedNodes nodes)
            {
                return nodes._nodes.ToArray();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(SortedNodes a, SortedNodes b)
            {
                return a.Equals(b);
            }
            #endregion
        }

        public enum TypeInterpolation
        {
            NewtonPolynomial, LagrangePolynomial, HermitPolynomial, CubicSplineInterpolation 
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class TypeInterpolationChangedEventArgs:EventArgs
        {
            #region FIELDS
            private readonly TypeInterpolation _oldType;
            private readonly TypeInterpolation _newType;
            #endregion
            #region CONSTRUCTOR
            public TypeInterpolationChangedEventArgs(TypeInterpolation oldType, TypeInterpolation newType)
            {
                _oldType = oldType;
                _newType = newType;
            }
            #endregion
            #region PROPERTIES
            public TypeInterpolation OldType => _oldType;
            public TypeInterpolation NewType => _newType;
            #endregion
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public abstract class BaseApproximation:IEquatable<BaseApproximation>, ISerializable
        {
            #region FIELDS
            private SortedNodes _nodes;
            private readonly Function _baseFunction;
            private Point2D[] _basePoints;
            private readonly int _amountPoints;
            #endregion
            #region CONSTRUCTORS
            public BaseApproximation(SortedNodes nodes, Function baseFunction)
            {
                _nodes = nodes;
                _baseFunction = baseFunction;
                _basePoints = GetBasePoints(_nodes, _baseFunction);
                _amountPoints = _nodes.Count;
            }

            public BaseApproximation(double startPoint, double endPoint, double step, Function baseFunction)
            {
                _baseFunction = baseFunction;
                _basePoints = GetBasePoints(startPoint, endPoint, step, _baseFunction);
                _nodes = _basePoints.Select(p => new Node(p.X)).ToList();
                _amountPoints = _nodes.Count;
            }

            public BaseApproximation(double startPoints, double endPoints, int countPoints, Function baseFunction)
            {
                _baseFunction = baseFunction;
                _basePoints = GetBasePoints(startPoints, endPoints, countPoints, _baseFunction);
                _nodes = _basePoints.Select(p => new Node(p.X)).ToList();
                _amountPoints = _nodes.Count;
            }

            public BaseApproximation(IEnumerable<Point2D> points)
            {
                _baseFunction = new Function();
                _basePoints = points.ToArray();
                _nodes = _basePoints.Select(p => new Node(p.X)).ToList();
                _amountPoints = _nodes.Count;
            }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            protected BaseApproximation(SerializationInfo info, StreamingContext context)
            {
                _nodes = (SortedNodes)info.GetValue("Nodes", typeof(SortedNodes));
                _baseFunction = (Function)info.GetValue("Function", typeof(Function));
                _basePoints = (Point2D[])info.GetValue("Points", typeof(Point2D[]));
                _amountPoints = info.GetInt32("AmountPoints");
            }
            #endregion
            #region METHODS
            private Point2D[] GetBasePoints(SortedNodes nodes, Function function)
            {
                Point2D[] points = new Point2D[nodes.Count];

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new Point2D(nodes[i], function.Invoke(nodes[i]));
                }

                return points;
            }

            private Point2D[] GetBasePoints(double startPoints, double endPoints, double step, Function f)
            {
                List<Point2D> points = new List<Point2D>();

                for (double x = startPoints; x <= endPoints; x += step)
                {
                    points.Add(new Point2D(x, f.Invoke(x)));
                }

                return points.ToArray();
            }

            private Point2D[] GetBasePoints(double startPoints, double endPoints, int countPoints, Function f)
            {
                double step = (endPoints - startPoints) / (countPoints - 1);
                List<Point2D> points = new List<Point2D>(countPoints);

                for (double x = startPoints; x <= endPoints; x += step)
                {
                    points.Add(new Point2D(x, f.Invoke(x)));
                }

                return points.ToArray();
            }

            public abstract double Calculate(Node node);

            bool IEquatable<BaseApproximation>.Equals(BaseApproximation other)
            {
                return _baseFunction.Equals(other._baseFunction) && _nodes.Equals(other._nodes);
            }

            public override bool Equals(object obj)
            {
                BaseApproximation baseApp = obj as BaseApproximation;
                if (baseApp == null) { return false; }
                IEquatable<BaseApproximation> equatable = this;

                return equatable.Equals(baseApp);
            }

            public override int GetHashCode()
            {
                int n = _baseFunction?.GetHashCode() ?? 1;
                return _nodes.GetHashCode() ^ n.GetHashCode();
            }

            public override string ToString()
            {
                string nameFunction = string.Empty;
                StringBuilder builder = new StringBuilder();
                builder.Append("Number of nodes:" + "\t" + _nodes.Count.ToString());
                builder.AppendLine();

                nameFunction = _baseFunction.Expression?.ToString() ?? "Undefined";
                builder.Append("Function name:" + "\t" + nameFunction);

                return builder.ToString();
            }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Nodes", _nodes, typeof(SortedNodes));
                info.AddValue("Function", _baseFunction, typeof(Function));
                info.AddValue("Points", _basePoints, typeof(Point2D[]));
                info.AddValue("AmountPoints", _amountPoints);
            }
            #endregion
            #region PROPERTIES
            public Function InterpolationFunction { get => _baseFunction; }
            public SortedNodes Nodes { get => _nodes; set => _nodes = value; }
            public Point2D[] InterpolationPoints { get => _basePoints; set => _basePoints = value; }
            public int AmountPoints => _amountPoints;
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(BaseApproximation a, BaseApproximation b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(BaseApproximation a, BaseApproximation b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(BaseApproximation a, BaseApproximation b)
            {
                return a.Equals(b);
            }
            #endregion
        }

        [StructLayout(LayoutKind.Auto)]
        internal struct SplineSegment:IEquatable<SplineSegment>
        {
            #region FIELDS
            private double _a;
            private double _b;
            private double _c;
            private double _d;
            private double _value;
            #endregion
            #region CONSTRUCTOR
            public SplineSegment(double a, double b, double c, double d, double value)
            {
                _a = a;
                _b = b;
                _c = c;
                _d = d;
                _value = value;
            }
            #endregion
            #region PROPERTIES
            public double A { get => _a; set => _a = value; }
            public double B { get => _b; set => _b = value; }
            public double C { get => _c; set => _c = value; }
            public double D { get => _d; set => _d = value; }
            public double Value { get => _value; set => _value = value; }
            #endregion
            #region METHODS
            public bool Equals(SplineSegment other)
            {
                return (_a == other._a) && (_b == other._b) && (_c == other._c) && (_d == other._d) && (_value == other._value);
            }

            public override bool Equals(object obj)
            {
                return (obj is SplineSegment) ? Equals((SplineSegment)obj) : false;
            }

            public override int GetHashCode()
            {
                return _a.GetHashCode() ^ _b.GetHashCode() ^ _c.GetHashCode() ^ _d.GetHashCode() ^ _value.GetHashCode();
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("A: " + _a.ToString() + "\n");
                builder.Append("B: " + _b.ToString() + "\n");
                builder.Append("C: " + _c.ToString() + "\n");
                builder.Append("D: " + _d.ToString() + "\n");
                builder.Append("Value: " + _value.ToString() + "\n");

                return builder.ToString();
            }
            #endregion
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class Interpolation:BaseApproximation, IEquatable<Interpolation>, ISerializable
        {
            #region FIELD
            private TypeInterpolation _type;
            private SplineSegment[] _segments;
            #endregion
            #region CONSTRUCTORS
            public Interpolation(TypeInterpolation type, SortedNodes nodes, Function baseFunction):base(nodes,baseFunction)
            {
                _type = type;
                SplineBuilt();
            }

            public Interpolation(TypeInterpolation type, double startPoint, double endPoints, double step, Function baseFunction):base(startPoint, endPoints, step, baseFunction)
            {
                _type = type;
                SplineBuilt();
            }

            public Interpolation(TypeInterpolation type, double startPoint, double endPoint, int countPoints, Function baseFunction):base(startPoint, endPoint, countPoints, baseFunction)
            {
                _type = type;
                SplineBuilt();
            }

            public Interpolation(TypeInterpolation type, IEnumerable<Point2D> points):base(points)
            {
                _type = type;
                SplineBuilt();
            }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            private Interpolation(SerializationInfo info, StreamingContext context):base(info, context)
            {
                _type = (TypeInterpolation)info.GetValue("Type", typeof(TypeInterpolation));
            }
            #endregion
            #region METHODS
            public bool Equals(Interpolation other)
            {
                return base.Equals(other) && _type == other._type;
            }

            public override bool Equals(object obj)
            {
                return (obj is Interpolation) ? Equals((Interpolation)obj) : false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode() ^ _type.GetHashCode();
            }

            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder(base.ToString());
                stringBuilder.AppendLine();
                stringBuilder.Append("Type interpolation:" + "\t" + _type.ToString());

                return stringBuilder.ToString();
            }

            private double Lagrange(double x)
            {
                double sum = 0.0;
                double p;

                for (int i = 0; i < InterpolationPoints.Length; i++)
                {
                    p = 1.0;
                    for (int j = 0; j < InterpolationPoints.Length; j++)
                    {
                        if (j != i)
                        {
                            p *= (x - InterpolationPoints[j].X) / (InterpolationPoints[i].X - InterpolationPoints[j].X);
                        }
                    }
                    sum += InterpolationPoints[i].Y * p;
                }

                return sum;
            }

            private double GetDividedDifference(int numberDifference)
            {
                double sum = 0.0, p;

                for (int i = 0; i <= numberDifference; i++)
                {
                    p = 1.0;
                    for (int j = 0; j <= numberDifference; j++)
                    {
                        if (i != j)
                        {
                            p *= (InterpolationPoints[i].X - InterpolationPoints[j].X);
                        }
                    }
                    sum += (InterpolationPoints[i].Y / p);
                }

                return sum;
            }

            private List<double> GetDividedDifferences()
            {
                List<double> dividedDifferences = new List<double>();

                for (int i = 1; i < InterpolationPoints.Length; i++)
                {
                    dividedDifferences.Add(GetDividedDifference(i));
                }

                return dividedDifferences;
            }

            private double Newton(double x)
            {
                double res = InterpolationPoints[0].Y, p, sum = 0.0;
                List<double> differences = GetDividedDifferences();

                for (int i = 1; i <= differences.Count; i++)
                {
                    p = 1.0;
                    for (int j = 0; j <= i - 1; j++)
                    {
                        p *= (x - InterpolationPoints[j].X);
                    }
                    sum += (differences[i - 1] * p);
                }

                return res + sum;
            }

            private double Hermit(double x)
            {
                return 0.0;
            }

            private void SplineBuilt()
            {
                int amountPoints = InterpolationPoints.Length;
                _segments = new SplineSegment[amountPoints];
                double[] alpha = new double[amountPoints - 1];
                double[] betta = new double[amountPoints - 1];
                double step1, step2;
                double a, b, c, f, z;

                for (int i=0; i<_segments.Length; i++)
                {
                    _segments[i].Value = InterpolationPoints[i].X;
                    _segments[i].A = InterpolationPoints[i].Y;
                }

                _segments[0].C = _segments[amountPoints - 1].C = 0.0;

                alpha[0] = betta[0] = 0.0;

                for(int i = 1; i< amountPoints - 1; i++)
                {
                    step1 = InterpolationPoints[i].X - InterpolationPoints[i - 1].X;
                    step2 = InterpolationPoints[i + 1].X - InterpolationPoints[i].X;

                    a = step1;
                    c = 2.0 * (step1 + step2);
                    b = step2;
                    f = 6.0 * (((InterpolationPoints[i + 1].Y - InterpolationPoints[i].Y) / step2) - ((InterpolationPoints[i].Y - InterpolationPoints[i - 1].Y) / step1));
                    z = a * alpha[i - 1] + c;

                    alpha[i] = -b / z;
                    betta[i] = (f - a * betta[i - 1]) / z;
                }

                for(int i = amountPoints - 2; i > 0; i--)
                {
                    _segments[i].C = alpha[i] * _segments[i + 1].C + betta[i];
                }

                for(int i = amountPoints - 1; i > 0; i--)
                {
                    step1 = InterpolationPoints[i].X - InterpolationPoints[i - 1].X;

                    _segments[i].D = (_segments[i].C - _segments[i - 1].C) / step1;
                    _segments[i].B = step1 * (2.0 * _segments[i].C + _segments[i - 1].C) / 6.0 + (InterpolationPoints[i].Y - InterpolationPoints[i - 1].Y) / step1;
                }
            }

            private double Spline(double x)
            {
                SplineSegment segment;
                int amountSegments = _segments.Length;
                double dx;

                if (x <= _segments[0].Value)
                {
                    segment = _segments[0];
                }
                else if (x >= _segments[amountSegments - 1].Value)
                {
                    segment = _segments[amountSegments - 1];
                }
                else
                {
                    int i = 0, j = amountSegments - 1;
                    int k;
                    while(i + 1 < j)
                    {
                        k = i + (j - i) / 2;

                        if (x <= _segments[k].Value)
                        {
                            j = k;
                        }
                        else
                        {
                            i = k;
                        }
                    }

                    segment = _segments[j];
                }

                dx = x - segment.Value;
                return segment.A + (segment.B + (segment.C / 2.0 + segment.D * dx / 6.0) * dx) * dx;
            }

            public override double Calculate(Node node)
            {
                double res = 0.0;
                switch (_type)
                {
                    case TypeInterpolation.NewtonPolynomial: res = Newton(node.Value); break;
                    case TypeInterpolation.LagrangePolynomial: res = Lagrange(node.Value); break;
                    case TypeInterpolation.HermitPolynomial: res = Hermit(node.Value); break;
                    case TypeInterpolation.CubicSplineInterpolation: res = Spline(node.Value); break;
                    default: throw new ArgumentException();
                }

                return res;
            }

            private void OnTypeInterpolationChanged(TypeInterpolationChangedEventArgs e)
            {
                TypeInterpolationChanged?.Invoke(this, e);
            }

            public override sealed void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
                info.AddValue("Type", _type, typeof(TypeInterpolation));
            }

            [OnDeserialized]
            private void OnDeserialized(StreamingContext context)
            {
                SplineBuilt();
            }
            #endregion
            #region PROPERTIE
            public TypeInterpolation InteprolationType
            {
                get => _type;
                set
                {
                    TypeInterpolation oldType = InteprolationType;
                    if (oldType == value) { return; }

                    _type = value;
                    OnTypeInterpolationChanged(new TypeInterpolationChangedEventArgs(oldType, value));
                }
            }
            #endregion
            #region EVENT
            public event EventHandler<TypeInterpolationChangedEventArgs> TypeInterpolationChanged;
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(Interpolation a, Interpolation b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(Interpolation a, Interpolation b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Interpolation a, Interpolation b)
            {
                return a.Equals(b);
            }
            #endregion
        }
    }
}
