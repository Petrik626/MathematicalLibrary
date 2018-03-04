using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

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
    
        public enum TypesNormOfVector
        {
            MaximumNorm, LNorm, EuclideanNorm
        }

        public enum TypesVectorNormalization
        {
            Maximum, LNormalization, Module
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class Vector : IMathematicalObject, IArithmeticOperations, IComparisonOperations, IEquatable<Vector>, IEnumerable<double>
        {
            #region FIELDS
            private readonly double[] _components;
            private readonly int _dimensions;
            #endregion
            #region CONSTRUCTORS
            public Vector()
            {
                _dimensions = 2;
                _components = new double[] { 0.0, 0.0 };
            }

            private Vector(int dimension)
            {
                _dimensions = dimension;
                _components = new double[_dimensions];
            }

            public Vector(int dimension, double[] coords)
            {
                _dimensions = dimension;
                _components = new double[_dimensions];
                for(int i=0; i<_dimensions; i++)
                {
                    _components[i] = coords[i];
                }
            }

            public Vector(params double[] coords)
            {
                if (coords.Length == 1 || coords.Length == 0) { throw new ArgumentException("The vector measurement can not be equal to unity"); }
                else if (coords == null) { new Vector(); return; }

                _dimensions = coords.Length;
                _components = new double[_dimensions];
                _components = coords.Select(n => n).ToArray<double>();
            }

            public Vector(Vector obj) : this(obj._components)
            {

            }

            public Vector(params int[] coords)
            {
                if (coords.Length == 1 || coords.Length == 0) { throw new ArgumentException("The vector measurement can not be equal to unity"); }
                else if (coords == null) { new Vector(); return; }

                _dimensions = coords.Length;
                _components = new double[_dimensions];
                _components = coords.Select(n => Convert.ToDouble(n)).ToArray<double>();
            }

            #endregion
            #region METHODS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double MaxNorm()
            {
                return _components.Max(x => Math.Abs(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double LNorm()
            {
                return _components.Sum((x) => Math.Abs(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double Norm(TypesNormOfVector types)
            {
                switch(types)
                {
                    case TypesNormOfVector.MaximumNorm: return MaxNorm();
                    case TypesNormOfVector.LNorm: return LNorm();
                    case TypesNormOfVector.EuclideanNorm: return Abs;
                    default: throw new ArgumentException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Vector Normalization(TypesVectorNormalization types)
            {
                double diveder = 1.0;
                switch (types)
                {
                    case TypesVectorNormalization.Maximum: diveder = MaxNorm(); return _components.Select(n => n / diveder).ToArray<double>();
                    case TypesVectorNormalization.LNormalization: diveder = LNorm(); return _components.Select(n => n / diveder).ToArray<double>();
                    case TypesVectorNormalization.Module: diveder = Abs; return _components.Select(n => n / diveder).ToArray<double>();
                    default: throw new ArgumentException();
                }
            }

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
                IStructuralEquatable se = _components;

                return _dimensions != other._dimensions ? false : se.Equals(other._components, StructuralComparisons.StructuralEqualityComparer);
            }

            public override bool Equals(object obj)
            {
                return (obj is Vector) ? Equals((Vector)obj) : false;
            }

            public override int GetHashCode()
            {
                int hash = 17;

                for (int i = 0; i < _components.Length; i++)
                {
                    hash *= (hash * 31) + _components[i].GetHashCode();
                }

                return hash;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                foreach(double el in _components)
                {
                    sb.AppendLine($"{el.ToString()}");
                }

                return sb.ToString();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this);
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                return (obj is Vector) ? Cross((Vector)obj) : (obj is Matrix) ? (this * (Matrix)obj) : throw new ArgumentException("Only vector or matrix types allowed");
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
                if (_dimensions != 3 && obj._dimensions != 3) { throw new NotSupportedException("This operation has not been supported by mathematical object"); }

                return new Vector((_components[1] * obj._components[2] - _components[2] * obj._components[1]), -(_components[0] * obj._components[2] - _components[2] * obj._components[0]), (_components[0] * obj._components[1] - _components[1] * obj._components[0]));
            }
            #endregion
            #region STATIC MEMBERS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZero(Vector v)
            {
                return v._components.Where(n => n == 0.0).Count() == v._dimensions;
            }

            public static Vector Zero { get => new Vector(0.0, 0.0); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(Vector a, Vector b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Parse(string s)
            {
                s = s.Replace(".", ",");
                if (Regex.IsMatch(s, @"\B[\d]+([.,]\d+)?\B|(?i)[A-Z]", RegexOptions.Compiled)) { throw new ArgumentException("This string is not converted in Vector"); }


                string[] coords = Regex.Split(s, @" ", RegexOptions.Compiled);
                return new Vector(coords.Select(n => double.Parse(n)).ToArray<double>());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Vector v)
            {
                try
                {
                    v = Parse(s);
                    return true;
                }
                catch
                {
                    v = new Vector();
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator !=(Vector a, Vector b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator +(Vector a, Vector b)
            {
                if (a._dimensions != b._dimensions) { throw new ArgumentException("Vectors dimensions are not equals"); }

                double[] coords = new double[a._dimensions];

                for (int i = 0; i < a._components.Length; i++)
                {
                    coords[i] = a._components[i] + b._components[i];
                }

                return new Vector(coords);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator -(Vector a, Vector b)
            {
                if (a._dimensions != b._dimensions) { throw new ArgumentException("Vectors dimensions are not equals"); }

                double[] coords = new double[a._dimensions];

                for (int i = 0; i < a._components.Length; i++)
                {
                    coords[i] = a._components[i] - b._components[i];
                }

                return new Vector(coords);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double operator *(Vector a, Vector b)
            {
                if (a._dimensions != b._dimensions) { throw new ArgumentException("Vectors dimensions are not equals"); }
                else if (IsZero(a) || IsZero(b)) { return 0.0; }

                double result = 0.0;
                for (int i = 0; i < a._dimensions; i++)
                {
                    result += (a._components[i] * b._components[i]);
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator*(Vector a, Matrix b)
            {
                if(a._dimensions != b.CountOfRow) { throw new ArithmeticException("Size of matrix and vector are not equaling each other"); }

                Vector v = new Vector(b.CountOfColumn);

                double s;
                for(int i=0; i<a._dimensions; i++)
                {
                    s = 0.0;
                    for(int j = 0; j<b.CountOfRow; j++)
                    {
                        s = s + a[j] * b[j, i];
                    }

                    v._components[i] = s;
                }

                return v;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator *(double a, Vector b)
            {
                return b._components.Select(n => a * n).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator *(Vector a, double b)
            {
                return b * a;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator/(Vector a, double b)
            {
                return a._components.Select(n => n / b).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Cross(Vector a, Vector b)
            {
                return a.Cross(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator-(Vector v)
            {
                return v._components.Select(n => -n).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator+(Vector v)
            {
                return v._components.Select(n => +n).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator++(Vector v)
            {
                return v._components.Select(n => n + 1).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator--(Vector v)
            {
                return v._components.Select(n => n - 1).ToArray<double>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Normalization(Vector v, TypesVectorNormalization types)
            {
                return v.Normalization(types);
            }
            #endregion
            #region NESTED TYPE
            private sealed class Enumerator : IEnumerator<double>
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

                        if (indexCoords == obj._components.Length) { throw new InvalidOperationException("Past end of list"); }

                        return obj._components[indexCoords];
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
                    if (indexCoords >= obj._components.Length - 1) { return false; }
                    return ++indexCoords < obj._components.Length;
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double[] (Vector v)
            {
                return v.ComponentsVector;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Vector(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string (Vector v)
            {
                return v.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Vector(int[] coords)
            {
                return new Vector(coords);
            }
            #endregion
            #region PROPERTIES
            public double Abs { get => Math.Sqrt(_components.Sum(n => n * n)); }
            public int Measurement { get => _dimensions; }
            public double[] ComponentsVector { get => _components; }
            public double this[int index]
            {
                get
                {
                    if(index<0 || index > _dimensions - 1) { throw new IndexOutOfRangeException(); }
                    return _components[index];
                }
                set
                {
                    if (index < 0 || index > _dimensions - 1) { throw new IndexOutOfRangeException(); }
                    _components[index] = value;
                }
            }
            #endregion
        }

        public enum TypesNormOfMatrix
        {
            MaximumSumOfRows, MaximumSumOfColumns, Maximum, Spherical
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public class Matrix : IMathematicalObject, IArithmeticOperations, IComparisonOperations, IEquatable<Matrix>
        {
            #region FIELDS
            private readonly int _numberOfRow;
            private readonly int _numberOfColumn;
            private readonly double[,] _components;
            #endregion
            #region CONSTRUCTORS
            public Matrix()
            {
                _numberOfRow = _numberOfColumn = 2;
                _components = new double[_numberOfRow, _numberOfColumn];

                for(int i=0; i<_numberOfRow; i++)
                {
                    for(int j=0; j<_numberOfColumn; j++)
                    {
                        _components[i, j] = 0.0;
                    }
                }
            }

            public Matrix(int row, int colums)
            {
                _numberOfRow = row;
                _numberOfColumn = colums;
                _components = new double[_numberOfRow, _numberOfColumn];

                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        _components[i, j] = 0.0;
                    }
                }
            }

            public Matrix(int row, int column, double[,] components)
            {
                _numberOfRow = row;
                _numberOfColumn = column;
                _components = new double[_numberOfRow, _numberOfColumn];

                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        _components[i, j] = components[i, j];
                    }
                }
            }

            public Matrix(double[,] components) : this(components.GetLength(0), components.GetLength(1), components) { }

            public Matrix(Matrix obj) : this(obj._components) { }
            #endregion
            #region METHODS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double MaximumSumOfRowsNorm()
            {
                if (!IsSquare) { throw new NotSupportedException("This operation is not been supported for non square matrix"); }

                double[] sumRows = new double[_numberOfRow];
                double s;

                for(int i=0; i<_numberOfRow; i++)
                {
                    s = 0.0;
                    for(int j=0; j<_numberOfColumn; j++)
                    {
                        s += Math.Abs(_components[i, j]);
                    }
                    sumRows[i] = s;
                }

                return sumRows.Max();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double MaximumSumOfColumnsNorm()
            {
                if (!IsSquare) { throw new NotSupportedException("This operation is not been supported for non square matrix"); }

                double[] sumColumns = new double[_numberOfColumn];
                double s;

                for(int i=0; i<_numberOfRow; i++)
                {
                    s = 0.0;
                    for(int j=0; j<_numberOfColumn; j++)
                    {
                        s += Math.Abs(_components[j, i]);
                    }
                    sumColumns[i] = s;
                }

                return sumColumns.Max();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double MaximumElementNorm()
            {
                if (!IsSquare) { throw new NotSupportedException("This operation is not been supported for non square matrix"); }

                List<List<double>> list = (List<List<double>>)this;
                double max = list[0].Max(n => Math.Abs(n));

                for(int i=1;i<_numberOfRow;i++)
                {
                    if(list[i].Max(n=>Math.Abs(n)) > max)
                    {
                        max = list[i].Max(n => Math.Abs(n));
                    }
                }

                return _numberOfRow * max;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double SphericalNorm()
            {
                if (!IsSquare) { throw new NotSupportedException("This operation is not been supported for non square matrix"); }

                double spherical = 0.0;

                for(int i=0;i<_numberOfRow;i++)
                {
                    for(int j=0; j<_numberOfRow; j++)
                    {
                        spherical += Math.Abs(Math.Pow(_components[i, j], 2.0));
                    }
                }

                return Math.Sqrt(spherical);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double Norm(TypesNormOfMatrix types)
            {
                switch(types)
                {
                    case TypesNormOfMatrix.MaximumSumOfRows: return MaximumSumOfRowsNorm();
                    case TypesNormOfMatrix.MaximumSumOfColumns: return MaximumSumOfColumnsNorm();
                    case TypesNormOfMatrix.Maximum: return MaximumElementNorm();
                    case TypesNormOfMatrix.Spherical: return SphericalNorm();
                    default: throw new ArgumentException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsSymmetricMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        if ((i != j) && (_components[i, j] != _components[j, i]))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsDiagonalMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        if ((i != j) && (_components[i, j] != 0.0))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsThreeDiagonalMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        if (i != j && i != j - 1 && i != j + 1 && _components[i, j] != 0.0) { return false; }
                    }
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsUpperTriangularMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (_components[i, j] != 0.0) { return false; }
                    }
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsLowerTriangularMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (_components[j, i] != 0.0) { return false; }
                    }
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsZeroMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        if (_components[i, j] != 0.0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool IsIdentityMatrix()
            {
                for (int i = 0; i < _numberOfRow; i++)
                {
                    if (_components[i, i] != 1.0) { return false; }
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        if (i == j) { continue; }
                        if (_components[i, j] != 0.0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            public bool Equals(Matrix other)
            {
                return ReferenceEquals(this, other);
            }

            public sealed override bool Equals(object obj)
            {
                return (obj is Matrix) ? Equals((Matrix)obj) : false;
            }

            public sealed override int GetHashCode()
            {
                return _components.GetHashCode();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sealed override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        sb.Append($"{_components[i, j]}\t");
                    }
                    sb.AppendLine();
                }

                return sb.ToString();
            }

            string IMathematicalObject.Show()
            {
                return ToString();
            }

            IMathematicalObject IArithmeticOperations.Addition(IMathematicalObject obj)
            {
                return (obj is Matrix) ? (this + (Matrix)obj) : throw new ArgumentException("Only matrix type allowed");
            }

            IMathematicalObject IArithmeticOperations.Subtraction(IMathematicalObject obj)
            {
                return (obj is Matrix) ? (this - (Matrix)obj) : throw new ArgumentException("Only matrix type allowed");
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                bool f1 = (obj is Matrix);
                bool f2 = (obj is Vector);

                return f1 ? (this * (Matrix)obj) : f2 ? (IMathematicalObject)(this * (Vector)obj) : throw new ArgumentException("Only matrix or vector types allowed");
            }

            IMathematicalObject IArithmeticOperations.Division(IMathematicalObject obj)
            {
                throw new NotImplementedException();
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Matrix Transpose()
            {
                double[,] components = new double[_numberOfColumn, _numberOfRow];

                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
                    {
                        components[i, j] = _components[j, i];
                    }

                }
                return components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Matrix Minor(int row, int column)
            {
                if ((row < 0) || (row > _numberOfRow - 1) || (column < 0) || (column > _numberOfColumn - 1)) { throw new ArgumentException("These arguments are bigger than numbers of row and column "); }
                double[,] components = new double[_numberOfRow - 1, _numberOfColumn - 1];
                int di = 0, dj = 0;

                for (int i = 0; i < _numberOfRow - 1; i++)
                {
                    if (i == row) { di = 1; }
                    for (int j = 0; j < _numberOfColumn - 1; j++)
                    {
                        if (j == column) { dj = 1; }
                        components[i, j] = _components[i + di, j + dj];
                    }
                    dj = 0;
                }

                return components;
            }
            #endregion
            #region PROPERTIES
            public int CountOfRow { get => _numberOfRow; }
            public int CountOfColumn { get => _numberOfColumn; }
            public double[,] Components
            {
                get => _components;
                set
                {
                    if (_components == null) { throw new NullReferenceException(); }
                    else if (_numberOfRow != value.GetLength(0) || _numberOfColumn != value.GetLength(1)) { throw new ArgumentException(); }

                    for (int i = 0; i < _numberOfRow; i++)
                    {
                        for (int j = 0; j < _numberOfColumn; j++)
                        {
                            _components[i, j] = value[i, j];
                        }
                    }
                }
            }

            public double this[int index1, int index2]
            {
                get
                {
                    if (index1 < 0 || index2 < 0 || index1 > _numberOfRow - 1 || index2 > _numberOfColumn - 1) { throw new IndexOutOfRangeException(); }
                    return _components[index1, index2];
                }
                set
                {
                    if (index1 < 0 || index2 < 0 || index1 > _numberOfRow - 1 || index2 > _numberOfColumn - 1) { throw new IndexOutOfRangeException(); }
                    _components[index1, index2] = value;
                }
            }

            public int AmountOfElements { get => _numberOfRow * _numberOfColumn; }
            public bool IsSquare { get => _numberOfRow == _numberOfColumn; }
            public bool IsSymmetric { get => IsSymmetricMatrix(); }
            public bool IsUpperTriangular { get => IsUpperTriangularMatrix(); }
            public bool IsLowerTriangular { get => IsLowerTriangularMatrix(); }
            public bool IsZero { get => IsZeroMatrix(); }
            public bool IsIdentity { get => IsIdentityMatrix(); }
            public bool IsDiagonal { get => IsDiagonalMatrix(); }
            public bool IsThreeDiagonal { get => IsThreeDiagonalMatrix(); }
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Indentity(int rowCount, int colCount)
            {
                if (rowCount < 0 || colCount < 0) { throw new ArgumentException("Matrix dimensions do not can to be negative"); }
                double[,] components = new double[rowCount, colCount];
                
                for(int i=0; i<rowCount; i++)
                {
                    components[i, i] = 1.0;
                    for(int j=0; j<colCount; j++)
                    {
                        if (i == j) { continue; }
                        components[i, j] = 0.0;
                    }
                }

                return components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Zero(int rowCount, int colCount)
            {
                if(rowCount<0 || colCount < 0) { throw new ArgumentException("Matrix dimensions do not can to be negative"); }

                return new double[rowCount, colCount];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double Norm(Matrix obj, TypesNormOfMatrix types)
            {
                return obj.Norm(types);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator*(Matrix a, Vector b)
            {
                if (a._numberOfColumn != b.Measurement) { throw new ArithmeticException("Size of matrix and vector are not equaling each other"); }
                double[] components = new double[a._numberOfRow];

                double s;
                for (int i = 0; i < a._numberOfRow; i++)
                {
                    s = 0.0;
                    for (int j = 0; j < b.Measurement; j++)
                    {
                        s = s + a[i, j] * b[j];
                    }
                    components[i] = s;
                }

                return components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator+(Matrix a, Matrix b)
            {
                if(a._numberOfRow != b._numberOfRow || a._numberOfColumn != b._numberOfColumn) { throw new ArgumentException("Sizes of arguments are not equal each other"); }

                double[,] components = new double[a._numberOfRow, a._numberOfColumn];

                for(int i=0;i<a._numberOfRow;i++)
                {
                    for(int j=0;j<a._numberOfColumn;j++)
                    {
                        components[i, j] = a._components[i, j] + b._components[i, j];
                    }
                }

                return new Matrix(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator-(Matrix a, Matrix b)
            {
                if (a._numberOfRow != b._numberOfRow || a._numberOfColumn != b._numberOfColumn) { throw new ArgumentException("Sizes of arguments are not equal each other"); }

                double[,] components = new double[a._numberOfRow, a._numberOfColumn];

                for(int i = 0; i < a._numberOfRow; i++)
                {
                    for(int j = 0; j < a._numberOfColumn; j++)
                    {
                        components[i, j] = a._components[i, j] - b._components[i, j];
                    }
                }

                return new Matrix(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator*(Matrix a, Matrix b)
            {
                if (a._numberOfRow != b._numberOfColumn) { throw new ArgumentException("Number of columns and rows are not equal each other in argument"); }

                double[,] components = new double[a._numberOfRow, b._numberOfColumn];
                double s;

                for(int i=0; i<components.GetLength(0);i++)
                {
                    for(int j=0; j<components.GetLength(1);j++)
                    {
                        s = 0.0;
                        for(int k=0; k<a.CountOfColumn; k++)
                        {
                            s += a._components[i, k] * b._components[k, j];
                        }
                        components[i, j] = s;
                    }
                }

                return new Matrix(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator == (Matrix a, Matrix b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator != (Matrix a, Matrix b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator*(double a, Matrix b)
            {
                double[,] components = new double[b._numberOfRow, b._numberOfColumn];

                for(int i=0;i<b._numberOfRow;i++)
                {
                    for(int j=0;j<b._numberOfColumn;j++)
                    {
                        components[i, j] = a * b._components[i, j];
                    }
                }

                return components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator*(Matrix a, double b)
            {
                return b * a;
            }
            #endregion
            #region TYPE CONVERSIONS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Matrix(double[,] components)
            {
                return new Matrix(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double[,](Matrix obj)
            {
                double[,] components = new double[obj._numberOfRow, obj._numberOfColumn];

                for(int i=0; i<obj._numberOfRow; i++)
                {
                    for(int j=0; j<obj._numberOfColumn; j++)
                    {
                        components[i, j] = obj._components[i, j];
                    }
                }

                return components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string (Matrix obj)
            {
                return obj.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Matrix(List<List<double>> obj)
            {
                double[,] components = new double[obj[0].Count, obj[1].Count];

                for(int i=0; i<components.GetLength(0); i++)
                {
                    for(int j=0; j<components.GetLength(1); j++)
                    {
                        components[i, j] = obj[i][j];
                    }
                }

                return components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator List<List<double>>(Matrix obj)
            {
                List<List<double>> list = new List<List<double>>();
                List<double> rows;

                for(int i=0; i<obj._numberOfRow; i++)
                {
                    rows = new List<double>();
                    for(int j=0; j<obj._numberOfColumn; j++)
                    {
                        rows.Add(obj[i, j]);
                    }
                    list.Add(rows);
                    rows = null;
                }

                return list;
            }
            #endregion
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class Tensor:Matrix
        {
            #region FIELD
            private int _rank;
            #endregion
            #region CONSTRUCTORS 
            public Tensor(int r):base(r,r)
            {
                _rank = r;
            }

            public Tensor(int r, double[,] components):base(r,r,components)
            {
                _rank = r;
            }

            public Tensor(double[,] components):base(components)
            {
                _rank = components.GetLength(0);
            }

            public Tensor(Tensor obj):base(obj.Components)
            {
                _rank = obj.CountOfRow;
            }

            public Tensor():base()
            {
                _rank = 2;
            }

            #endregion
        }
    }
}