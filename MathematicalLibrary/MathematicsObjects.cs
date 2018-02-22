using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mathematics
{
    namespace Objects
    {
        public interface IMathematicalObject
        {
            string Show();
        }

        public interface IArithmeticOperations
        {
            IMathematicalObject Addition(IMathematicalObject obj);
            IMathematicalObject Subtraction(IMathematicalObject obj);
            IMathematicalObject Multiplication(IMathematicalObject obj);
            IMathematicalObject Division(IMathematicalObject obj);
        }

        public interface IComparisonOperations
        {
            bool OperationIsEquality(IMathematicalObject obj);
            bool OperationIsNotEquality(IMathematicalObject obj);
            bool OperationIsMore(IMathematicalObject obj); // Операция больше
            bool OperationIsLess(IMathematicalObject obj); // Операция меньше
            bool OperationIsMoreOrEqual(IMathematicalObject obj);
            bool OperationIsLessOrEqual(IMathematicalObject obj);
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public struct Complex : IMathematicalObject, IArithmeticOperations, IComparisonOperations, IEquatable<Complex>, IEnumerable<double>
        {
            #region FIELDS
            private readonly double _re;
            private readonly double _im;
            #endregion
            #region CONSTRUCTORS
            public Complex(double re = 0.0, double im = 0.0) { _re = re; _im = im; }
            public Complex(Complex obj) : this(obj._re, obj._im) { }
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsInfinity(Complex c)
            {
                return double.IsInfinity(c._re) || double.IsInfinity(c._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsRealPossitiveInfinity(Complex c)
            {
                return double.IsPositiveInfinity(c._re) && c._re == 0.0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsRealNegativeInfinity(Complex c)
            {
                return double.IsNegativeInfinity(c._re) && c._im == 0.0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNaN(Complex c)
            {
                return double.IsNaN(c._re) || double.IsNaN(c._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZero(Complex c)
            {
                return c._re == 0.0 && c._im == 0.0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex ConjugateComplexNumber(Complex c)
            {
                return new Complex(c._re, -c._im);
            }

            public static Complex I { get => new Complex(0.0, 1.0); }

            public static Complex Zero { get => new Complex(); }

            public static Complex Infinity { get => new Complex(double.PositiveInfinity, double.PositiveInfinity); }

            public static Complex RealPossitiveInfinity { get => new Complex(double.PositiveInfinity, 0.0); }

            public static Complex RealNegativeInfinity { get => new Complex(double.NegativeInfinity, 0.0); }

            public static Complex NaN { get => new Complex(double.NaN, double.NaN); }

            public static Complex Epsilon { get => new Complex(double.Epsilon, double.Epsilon); }

            public static Complex RealEpsilon { get => new Complex(double.Epsilon, 0.0); }

            public static Complex ImaginaryEpsilon { get => new Complex(0.0, double.Epsilon); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Parse(string s)
            {
                s = s.Replace(" ", string.Empty).Replace(".", ",");

                switch (s)
                {
                    case "0": return Zero;
                    case "∞": return Infinity;
                    case "¿": return NaN;
                    default:
                        {
                            string pattern = @"(([+-]?[\d]+([.,][\d]+)?)|([+-](?i)i?[\d]+([.,]\d+)?(?i)i?)?([+-](?i)i)?)";

                            if (!Regex.IsMatch(s, @"(?i)i", RegexOptions.Compiled))
                            {
                                return new Complex(double.Parse(s), 0.0);
                            }

                            string[] complexNumber = new string[2];
                            int i = 0;

                            foreach (Match el in Regex.Matches(s, pattern, RegexOptions.Compiled))
                            {
                                if (i > 1) { break; }

                                complexNumber[i] = el.Value;
                                i++;
                            }

                            complexNumber[1] = Regex.Replace(complexNumber[1], @"(?i)[i]", string.Empty, RegexOptions.Compiled);


                            if (Regex.IsMatch(complexNumber[1], @"\B([+-])\B"))
                            {
                                complexNumber[1] = complexNumber[1] + "1,0";
                            }

                            return complexNumber.Select(str => double.Parse(str)).ToArray<double>();

                        }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Complex c)
            {
                c = new Complex();
                try
                {
                    c = Parse(s);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            #endregion
            #region OPERATORS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator +(Complex a, Complex b)
            {
                return new Complex(a._re + b._re, a._im + b._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator -(Complex a, Complex b)
            {
                return new Complex(a._re - b._re, a._im - b._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator *(Complex a, Complex b)
            {
                return new Complex(a._re * b._re - a._im * b._im, a._im * b._re + a._re * b._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator /(Complex a, Complex b)
            {
                if ((IsZero(a) && IsZero(b)) || (IsInfinity(a) && IsInfinity(b)) || IsNaN(a) || IsNaN(b))
                {
                    return NaN;
                }
                else if (IsZero(a) || IsInfinity(b))
                {
                    return Zero;
                }
                else if (IsInfinity(a) || IsZero(b))
                {
                    return Infinity;
                }
                else
                {
                    return new Complex((a.Re * b.Re + a.Im * b.Im) / (b.Re * b.Re + b.Im * b.Im), (a.Im * b.Re - a.Re * b.Im) / (b.Re * b.Re + b.Im * b.Im));
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(Complex a, Complex b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator !=(Complex a, Complex b)
            {
                return !(a.Equals(b));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator +(Complex a, double b)
            {
                return new Complex(a._re + b, a._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator +(double a, Complex b)
            {
                return (b + a);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator -(Complex a, double b)
            {
                return new Complex(a._re - b, a._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator -(double a, Complex b)
            {
                return new Complex(a - b._re, -b._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator *(Complex a, double b)
            {
                return new Complex(a._re * b, a._im * b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator *(double a, Complex b)
            {
                return (b * a);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator /(double d, Complex a)
            {
                if ((d == 0.0 && IsZero(a)) || (double.IsInfinity(d) && IsInfinity(a)) || double.IsNaN(d) || IsNaN(a))
                {
                    return NaN;
                }
                else if (d == 0.0 || IsInfinity(a))
                {
                    return Zero;
                }
                else if (double.IsInfinity(d) || IsZero(a))
                {
                    return Infinity;
                }
                else
                {
                    return new Complex((d * a.Re) / (a.Re * a.Re + a.Im * a.Im), (-d * a.Im) / (a.Re * a.Re + a.Im * a.Im));
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator /(Complex a, double d)
            {
                if ((IsZero(a) && d == 0.0) || (IsInfinity(a) && double.IsInfinity(d)) || IsNaN(a) || double.IsNaN(d))
                {
                    return NaN;
                }
                else if (IsZero(a) || double.IsInfinity(d))
                {
                    return Zero;
                }
                else if (IsInfinity(a) || d == 0.0)
                {
                    return Infinity;
                }
                else
                {
                    return new Complex((a.Re * d) / (d * d), (a.Im * d) / (d * d));
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator ++(Complex a)
            {
                return new Complex(a._re + 1, a._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator --(Complex a)
            {
                return new Complex(a._re - 1, a._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator +(Complex a)
            {
                return new Complex(+a._re, +a._im);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex operator -(Complex a)
            {
                return new Complex(-a._re, -a._im);
            }
            #endregion
            #region METHODS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double GetArgument()
            {
                if ((Re > 0 && Im > 0) || (Re > 0 && Im < 0))
                {
                    return Math.Atan(Im / Re);
                }
                else if ((Re < 0 && Im > 0) || (Re < 0 && Im < 0))
                {
                    return Math.PI + Math.Atan(Im / Re);
                }
                else if (Im == 0 && Re > 0)
                {
                    return 0;
                }
                else if (Im == 0 && Re < 0)
                {
                    return Math.PI;
                }
                else if (Re == 0 && Im < 0)
                {
                    return 3 * Math.PI / 2;
                }
                else if (Re == 0 && Im > 0)
                {
                    return Math.PI / 2;
                }
                else
                {
                    return 0;
                }
            }

            public bool Equals(Complex other)
            {
                return ((_re == other._re) && (_im == other._im));
            }

            public override bool Equals(object obj)
            {
                return (obj is Complex) ? Equals((Complex)obj) : false;
            }

            public override int GetHashCode()
            {
                return _re.GetHashCode() ^ _im.GetHashCode();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override string ToString()
            {
                if (IsImaginaryPartEqualsToZero)
                {
                    return _re.ToString();
                }
                else if (IsInfinity(this))
                {
                    return "∞";
                }
                else if (IsZero(this))
                {
                    return "0";
                }
                else if (IsNaN(this))
                {
                    return "¿";

                }
                else
                {
                    string rePart = string.Empty;
                    string imPart = string.Empty;

                    if (Re != 0)
                    {
                        string reToS = Re.ToString();
                        if (reToS.Contains('E'))
                        {
                            if (Re > 0)
                            {
                                rePart = $"({reToS})";
                            }
                            else
                            {
                                rePart = $"-({reToS.Remove(0, 1)})";
                            }
                        }
                        else
                        {
                            rePart = reToS;
                        }
                    }

                    string imToS = Im.ToString();

                    if (imToS.Contains('E'))
                    {
                        if (Im > 0)
                        {
                            imPart = $"+({imToS})i";
                        }
                        else
                        {
                            imPart = $"-({imToS.Remove(0, 1)})i";
                        }
                    }
                    else
                    {
                        if (Im > 0 && Re != 0)
                        {
                            imPart = $"+{imToS}i";
                        }
                        else
                        {
                            imPart = $"{imToS}i";
                        }
                    }

                    return rePart + imPart;
                }
            }

            public IEnumerator<double> GetEnumerator()
            {
                yield return _re;
                yield return _im;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            string IMathematicalObject.Show()
            {
                return ToString();
            }

            IMathematicalObject IArithmeticOperations.Addition(IMathematicalObject obj)
            {
                return (obj is Complex) ? (this + (Complex)obj) : throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Complex");
            }

            IMathematicalObject IArithmeticOperations.Subtraction(IMathematicalObject obj)
            {
                return (obj is Complex) ? (this - (Complex)obj): throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Complex");
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                return (obj is Complex) ? (this * (Complex)obj): throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Complex");
            }

            IMathematicalObject IArithmeticOperations.Division(IMathematicalObject obj)
            {
                return (obj is Complex) ? (this / (Complex)obj): throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Complex");
            }

            bool IComparisonOperations.OperationIsEquality(IMathematicalObject obj)
            {
                return Equals(obj);
            } 

            bool IComparisonOperations.OperationIsNotEquality(IMathematicalObject obj)
            {
                return !Equals(obj);
            }

            bool IComparisonOperations.OperationIsMore(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsLess(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsMoreOrEqual(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsLessOrEqual(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }
            #endregion
            #region TYPE CONVERSIONS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Complex(double a)
            {
                return new Complex(a, 0.0);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Complex(double[] array)
            {
                try
                {
                    return new Complex(array[0], array[1]);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new ArgumentException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double(Complex a)
            {
                return a._re;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double[] (Complex a)
            {
                return new double[] { a._re, a._im };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Complex(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string(Complex a)
            {
                return a.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Complex(Tuple<double, double> tuple)
            {
                return new Complex(tuple.Item1, tuple.Item2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Tuple<double, double>(Complex z)
            {
                return new Tuple<double, double>(z._re, z._im);
            }
            #endregion
            #region PROPERTIES
            public double Re { get => _re; }
            public double Im { get => _im; }
            public double Abs { get => Math.Sqrt(_re * _re + _im * _im); }
            public double Argument { get => GetArgument(); }
            public double ArgumentDegrees { get => GetArgument() * 180.0 / Math.PI; }
            public Complex Conjugate { get => new Complex(_re, -_im); }
            public bool IsImaginaryPartEqualsToZero { get => _re != 0.0 && _im == 0.0; }
            #endregion
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public struct Point2D : IMathematicalObject, IArithmeticOperations, IComparisonOperations, IEquatable<Point2D>, IEnumerable<double>
        {
            #region FIELDS
            private readonly double _x;
            private readonly double _y;
            #endregion
            #region CONSTRUCTORS
            public Point2D(double x = 0.0, double y = 0.0)
            {
                _x = x;
                _y = y;
            }

            public Point2D(Point2D obj) : this(obj._x, obj._y) { }
            #endregion
            #region STATIC MEMBERS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsInfinity(Point2D p)
            {
                return double.IsInfinity(p._x) || double.IsInfinity(p._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsXPossitiveInfinity(Point2D p)
            {
                return double.IsPositiveInfinity(p._x) && p._y == 0.0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsYPossitiveInfinity(Point2D p)
            {
                return p._x == 0.0 && double.IsPositiveInfinity(p._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsXNegativeInfinity(Point2D p)
            {
                return double.IsNegativeInfinity(p._x) && p._y == 0.0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsYNegativeInfinity(Point2D p)
            {
                return p._x == 0.0 && double.IsNegativeInfinity(p._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNaN(Point2D p)
            {
                return double.IsNaN(p._x) || double.IsNaN(p._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZero(Point2D p)
            {
                return p._x == 0.0 && p._y == 0.0;
            }

            public static Point2D Zero { get => new Point2D(); }

            public static Point2D Infinity { get => new Point2D(double.PositiveInfinity, double.PositiveInfinity); }

            public static Point2D XPossitiveInfinity { get => new Point2D(double.PositiveInfinity, 0.0); }

            public static Point2D YPossitiveInfinity { get => new Point2D(0.0, double.PositiveInfinity); }

            public static Point2D XNegativeInfinity { get => new Point2D(double.NegativeInfinity, 0.0); }

            public static Point2D YNegativeInfinity { get => new Point2D(0.0, double.NegativeInfinity); }

            public static Point2D NaN { get => new Point2D(double.NaN, double.NaN); }

            public static Point2D Epsilon { get => new Point2D(double.Epsilon, double.Epsilon); }

            public static Point2D XEpsilon { get => new Point2D(double.Epsilon, 0.0); }

            public static Point2D YEpsilon { get => new Point2D(0.0, double.Epsilon); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D Parse(string s)
            {
                s = s.Replace(" ", string.Empty).Replace(".", ",");

                switch(s)
                {
                    case "" : return NaN;
                    case "{0}": return Zero;
                    case "{∞}": return Infinity;
                    case "{¿}": return NaN;
                    default:
                        {
                            string pattern = @"(([+-]?[\d]+([.,][\d]+)?);|([+-]?[\d]+([.,][\d]+)?))";
                            string[] coords = new string[2];
                            int i = 0;

                            foreach(Match m in Regex.Matches(s,pattern,RegexOptions.Compiled))
                            {
                                if (i > 1) { break; }
                                coords[i] = m.Value;
                                i++;
                            }

                            coords[0] = Regex.Replace(coords[0], ";", string.Empty, RegexOptions.Compiled);

                            if ((string.IsNullOrEmpty(coords[0]))&&(!string.IsNullOrEmpty(coords[1]))) { return new Point2D(0.0, double.Parse(coords[1])); }
                            else if (string.IsNullOrEmpty(coords[1])&&(!string.IsNullOrEmpty(coords[0]))) { return new Point2D(double.Parse(coords[0]), 0.0); }
                            return coords.Select(str => double.Parse(str)).ToArray<double>();
                        }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Point2D p)
            {
                p = new Point2D();
                try
                {
                    p = Parse(s);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(Point2D a, Point2D b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(Point2D a, Point2D b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator-(Point2D a, Point2D b)
            {
                return new Vector(b._x - a._x, b._y - a._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D operator -(Point2D a)
            {
                return new Point2D(-a._x, -a._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D operator +(Point2D a)
            {
                return new Point2D(+a._x, +a._y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D operator++(Point2D a)
            {
                return new Point2D(a._x + 1, a._y + 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D operator--(Point2D a)
            {
                return new Point2D(a._x - 1, a._y - 1);
            }
            #endregion
            #region TYPE CONVERSIONS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point2D(double[] coords)
            {
                try
                {
                    return new Point2D(coords[0], coords[1]);
                }
                catch
                {
                    throw new ArgumentException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double[](Point2D p)
            {
                return new double[] { p._x, p._y };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point2D(double a)
            {
                return new Point2D(a, 0.0);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double(Point2D p)
            {
                return p._x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point2D(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string(Point2D p)
            {
                return p.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point2D(Tuple<double,double> coords)
            {
                return new Point2D(coords.Item1, coords.Item2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Tuple<double, double>(Point2D p)
            {
                return new Tuple<double, double>(p._x, p._y);
            }
            #endregion
            #region METHODS

            public bool Equals(Point2D other)
            {
                return ((_x == other._x) && (_y == other._y));
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

            public override bool Equals(object obj)
            {
                return (obj is Point2D) ? Equals((Point2D)obj) : false;
            }

            public override int GetHashCode()
            {
                return _x.GetHashCode() ^ _y.GetHashCode();
            }

            public override string ToString()
            {
                return $"[X={_x};\tY={_y}]";
            }

            string IMathematicalObject.Show()
            {
                return ToString();
            }

            IMathematicalObject IArithmeticOperations.Addition(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            IMathematicalObject IArithmeticOperations.Subtraction(IMathematicalObject obj)
            {
                return (obj is Point2D) ? (this - (Point2D)obj) : throw new ArgumentException("Type {obj.GetType().Name} can not use arithmetic operations of type Point2D");
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            IMathematicalObject IArithmeticOperations.Division(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsEquality(IMathematicalObject obj)
            {
                return Equals(obj);
            }

            bool IComparisonOperations.OperationIsNotEquality(IMathematicalObject obj)
            {
                return !Equals(obj);
            }

            bool IComparisonOperations.OperationIsMore(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsLess(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsMoreOrEqual(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsLessOrEqual(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            public Point2D Offset(double x, double y)
            {
                return new Point2D(_x + x, _y + y);
            }
            #endregion
            #region PROPERTIES
            public double X { get => _x; }
            public double Y { get => _y; }
            #endregion
        }

        [StructLayout(LayoutKind.Auto),Serializable]
        public sealed class Vector:IMathematicalObject,IArithmeticOperations,IComparisonOperations,IEquatable<Vector>,IEnumerable<double>
        {
            #region FIELDS
            private readonly double[] _coords;
            private readonly int _dimensions;
            #endregion
            #region CONSTRUCTORS
            public Vector(params double[] coords)
            {
                if (coords.Length == 1) { throw new ArgumentException("The vector measurement can not be equal to unity"); }
                else if(coords == null) { _dimensions = 2; _coords = new double[] { 0.0, 0.0 }; return; }

                _dimensions = coords.Length;
                _coords = new double[_dimensions];
                _coords = coords.Select(n => n).ToArray<double>();
            }
            
            public Vector(Vector obj):this(obj._coords)
            {

            }

            public Vector(params int[] coords)
            {
                if (coords.Length == 1) { throw new ArgumentException("The vector measurement can not be equal to unity"); }
                else if (coords == null) { _dimensions = 2; _coords = new double[] { 0.0, 0.0 }; return; }

                _dimensions = coords.Length;
                _coords = new double[_dimensions];
                _coords = coords.Select(n => Convert.ToDouble(n)).ToArray<double>();
            }

            #endregion
            #region METHODS

            public IEnumerator<double> GetEnumerator()
            {
                return new Enumerator(this);
            }

            IMathematicalObject IArithmeticOperations.Addition(IMathematicalObject obj)
            {
                return (obj is Vector) ? (this + (Vector)obj) : throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Vector");
            }

            IMathematicalObject IArithmeticOperations.Division(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool Equals(Vector other)
            {
                IStructuralEquatable se = _coords;

                return _dimensions != other._dimensions ? false : se.Equals(other._coords, StructuralComparisons.StructuralEqualityComparer);
            }

            public override bool Equals(object obj)
            {
                return (obj is Vector) ? Equals((Vector)obj) : false;
            }

            public override int GetHashCode()
            {
                int hash = 17;

                for(int i=0; i<_coords.Length; i++)
                {
                    hash *= (hash * 31) + _coords[i].GetHashCode();
                }

                return hash;
            }

            public override string ToString()
            {
                return base.ToString();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this);
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                return (obj is Vector) ? Cross((Vector)obj) : throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Vector");
            }

            bool IComparisonOperations.OperationIsEquality(IMathematicalObject obj)
            {
                return Equals(obj);
            }

            bool IComparisonOperations.OperationIsLess(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsLessOrEqual(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsMore(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsMoreOrEqual(IMathematicalObject obj)
            {
                throw new NotSupportedException("This operation has not been supported by mathematical object");
            }

            bool IComparisonOperations.OperationIsNotEquality(IMathematicalObject obj)
            {
                return !Equals(obj);
            }

            string IMathematicalObject.Show()
            {
                return ToString();
            }

            IMathematicalObject IArithmeticOperations.Subtraction(IMathematicalObject obj)
            {
                return (obj is Vector) ? (this - (Vector)obj) : throw new ArgumentException($"Type {obj.GetType().Name} can not use arithmetic operations of type Vector");
            }

            public Vector Cross(Vector obj)//Векторное умножение 
            {
                if (_dimensions!=3 && obj._dimensions != 3) { throw new NotSupportedException("This operation has not been supported by mathematical object"); }

                return new Vector((_coords[1] * obj._coords[2] - _coords[2] * obj._coords[1]), -(_coords[0] * obj._coords[2] - _coords[2] * obj._coords[0]), (_coords[0] * obj._coords[1] - _coords[1] * obj._coords[0]));
            }
            #endregion
            #region STATIC MEMBERS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZero(Vector v)
            {
                return (v._dimensions == 2) && (v._coords[0] == 0.0) && (v._coords[1] == 0.0);
            }

            public static Vector Zero { get => new Vector(0.0, 0.0); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(Vector a, Vector b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(Vector a, Vector b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator+(Vector a, Vector b)
            {
                if (a._dimensions != b._dimensions) { throw new ArgumentException("Vectors dimensions are not equals"); }

                double[] coords = new double[a._dimensions];

                for(int i=0;i<a._coords.Length;i++)
                {
                    coords[i] = a._coords[i] + b._coords[i];
                }

                return new Vector(coords);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator-(Vector a, Vector b)
            {
                if (a._dimensions != b._dimensions) { throw new ArgumentException("Vectors dimensions are not equals"); }

                double[] coords = new double[a._dimensions];

                for (int i = 0; i < a._coords.Length; i++)
                {
                    coords[i] = a._coords[i] - b._coords[i];
                }

                return new Vector(coords);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double operator*(Vector a, Vector b)
            {
                if (a._dimensions != b._dimensions) { throw new ArgumentException("Vectors dimensions are not equals"); }
                else if (IsZero(a) || IsZero(b)) { return 0.0; }

                double result = 0.0;
                for(int i=0;i<a._dimensions;i++)
                {
                    result += (a._coords[i] * b._coords[i]);
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator*(double a, Vector b)
            {
                return b._coords.Select(n => a * n).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator*(Vector a, double b)
            {
                return b * a;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Cross(Vector a, Vector b)
            {
                return a.Cross(b);
            }
            #endregion
            #region NESTED TYPE
            private sealed class Enumerator:IEnumerator<double>
            {
                #region FIELDS
                private Vector obj;
                private int indexCoords;
                #endregion
                #region PROPERTIES
                public double Current
                {
                    get
                    {
                        if (indexCoords == -1) { throw new InvalidOperationException("Enumeration not started"); }

                        if (indexCoords == obj._coords.Length) { throw new InvalidOperationException("Past end of list"); }

                        return obj._coords[indexCoords];
                    }
                }

                object IEnumerator.Current => Current;
                #endregion
                #region CONSTRUCTOR
                public Enumerator(Vector collecion)
                {
                    obj = collecion;
                    indexCoords = -1;
                }
                #endregion
                #region METHODS
                void IDisposable.Dispose()
                {

                }

                public bool MoveNext()
                {
                    if (indexCoords >= obj._coords.Length - 1) { return false; }
                    return ++indexCoords < obj._coords.Length;
                }

                public void Reset()
                {
                    indexCoords = -1;
                }
                #endregion
            }
            #endregion
            #region TYPE CONVERSIONS
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Vector(double[] coords)
            {
                return new Vector(coords);
            }

            #endregion
        }

    }
}