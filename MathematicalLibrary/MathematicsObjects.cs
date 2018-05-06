using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static MathematicalLibrary.Functions.DoubleFunction;

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

                                complexNumber[i++] = el.Value;
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Complex a, Complex b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Add(Complex a, Complex b) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Add(Complex a, double b) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Add(double b, Complex a) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Subtract(Complex a, Complex b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Subtract(Complex a, double b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Subtract(double a, Complex b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Multiply(Complex a, Complex b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Multiply(Complex a, double b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Multiply(double b, Complex a) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Divide(Complex a, Complex b) => a / b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Divide(Complex a, double b) => a / b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Divide(double b, Complex a) => b / a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Increment(Complex a) => ++a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Decrement(Complex a) => --a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Plus(Complex a) => +a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Negate(Complex a) => -a;
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ToDouble()
            {
                return _re;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double[] ToArrayDouble()
            {
                return new double[] { _re, _im };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tuple<double,double> ToTupleDouble()
            {
                return new Tuple<double, double>(_re, _im);
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
                                coords[i++] = m.Value;
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Point2D a, Point2D b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Subtract(Point2D a, Point2D b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D Plus(Point2D a) => +a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D Negate(Point2D a) => -a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D Increment(Point2D a) => ++a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point2D Decrement(Point2D a) => --a;
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Point2D Offset(double x, double y)
            {
                return new Point2D(_x + x, _y + y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ToDouble()
            {
                return _x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double[] ToArrayDouble()
            {
                return new double[] { _x, _y };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tuple<double,double> ToTupleDouble()
            {
                return new Tuple<double, double>(_x, _y);
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

            public Vector(double c1, double c2):this(2)
            {
                _components[0] = c1; _components[1] = c2;
            }

            public Vector(double c1, double c2, double c3):this(3)
            {
                _components[0] = c1; _components[1] = c2; _components[2] = c3;
            }

            public Vector(double c1, double c2, double c3, double c4):this(4)
            {
                _components[0] = c1;_components[1] = c2;
                _components[2] = c3;_components[3] = c4;
            }

            public Vector(double c1, double c2, double c3, double c4, double c5):this(5)
            {
                _components[0] = c1; _components[1] = c2;
                _components[2] = c3;_components[3] = c4;
                _components[4] = c5;
            }

            public Vector(double c1, double c2, double c3, double c4, double c5, double c6):this(6)
            {
                _components[0] = c1;_components[1] = c2;
                _components[2] = c3;_components[3] = c4;
                _components[4] = c5;_components[5] = c6;
            }

            public Vector(double c1, double c2, double c3, double c4, double c5, double c6, double c7):this(7)
            {
                _components[0] = c1; _components[1] = c2;
                _components[2] = c3; _components[3] = c4;
                _components[4] = c5; _components[5] = c6;
                _components[6] = c7;
            }

            public Vector(double c1, double c2, double c3, double c4, double c5, double c6, double c7, double c8):this(8)
            {
                _components[0] = c1; _components[1] = c2;
                _components[2] = c3; _components[3] = c4;
                _components[4] = c5; _components[5] = c6;
                _components[6] = c7; _components[7] = c8;
            }

            public Vector(double c1, double c2, double c3, double c4, double c5, double c6, double c7, double c8, double c9):this(9)
            {
                _components[0] = c1; _components[1] = c2;
                _components[2] = c3; _components[3] = c4;
                _components[4] = c5; _components[5] = c6;
                _components[6] = c7; _components[7] = c8;
                _components[8] = c9;
            }

            public Vector(double c1, double c2, double c3, double c4, double c5, double c6, double c7, double c8, double c9, double c10):this(10)
            {
                _components[0] = c1; _components[1] = c2;
                _components[2] = c3; _components[3] = c4;
                _components[4] = c5; _components[5] = c6;
                _components[6] = c7; _components[7] = c8;
                _components[8] = c9; _components[9] = c10;
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
            public Vector Normalize(TypesVectorNormalization types)
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double[] ToArrayDouble()
            {
                return _components;
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
            public static Vector operator * (Vector a, Tensor b)
            {
                if (a._dimensions != b.Rank) { throw new ArithmeticException("Size of Tensor and vector are not equaling each other"); }

                Vector v = new Vector(b.Rank);

                double s;
                for (int i = 0; i < a._dimensions; i++)
                {
                    s = 0.0;
                    for (int j = 0; j < b.Rank; j++)
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
                return v.Normalize(types);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Vector a, Vector b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Add(Vector a, Vector b) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Subtract(Vector a, Vector b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double Multiply(Vector a, Vector b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Multiply(Vector a, double b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Multiply(double a, Vector b) => b * a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Multiply(Vector a, Matrix b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Multiply(Vector a, Tensor b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Divide(Vector a, double b) => a / b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Plus(Vector a) => +a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Negate(Vector a) => -a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Increment(Vector a) => ++a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Decrement(Vector a) => --a;
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
            [IndexerName("Component")]
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

                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfColumn; j++)
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
            private void SwapRowsInMatrix(int k, int j)
            {
                double[] tempRow = new double[_numberOfRow];

                for (int i = 0; i < _numberOfRow; i++)
                {
                    tempRow[i] = _components[k, i];
                    _components[k, i] = _components[j, i];
                    _components[j, i] = tempRow[i];
                }

                tempRow = null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int MaxIndexInRow(int k)
            {
                List<double> row = new List<double>();

                for (int i = 0; i < _numberOfRow; i++)
                {
                    row.Add(_components[k, i]);
                }

                return row.IndexOf(row.Max(n => Math.Abs(n)));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private double MaximumSumOfRowsNorm()
            {
                if (!IsSquare) { throw new NotSupportedException("This operation is not been supported for non square matrix"); }

                double[] sumRows = new double[_numberOfRow];
                double s;

                for (int i = 0; i < _numberOfRow; i++)
                {
                    s = 0.0;
                    for (int j = 0; j < _numberOfColumn; j++)
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

                for (int i = 0; i < _numberOfRow; i++)
                {
                    s = 0.0;
                    for (int j = 0; j < _numberOfColumn; j++)
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

                for (int i = 1; i < _numberOfRow; i++)
                {
                    if (list[i].Max(n => Math.Abs(n)) > max)
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

                for (int i = 0; i < _numberOfRow; i++)
                {
                    for (int j = 0; j < _numberOfRow; j++)
                    {
                        spherical += Math.Abs(Math.Pow(_components[i, j], 2.0));
                    }
                }

                return Math.Sqrt(spherical);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double Norm(TypesNormOfMatrix types)
            {
                switch (types)
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
                return (obj is Matrix) ? (this / (Matrix)obj) : throw new ArgumentException("Only matrix types allowed");
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Matrix Pow(int n)
            {
                Matrix temp = new Matrix(this);

                while ((n--) - 1 > 0)
                {
                    temp *= this;
                }

                return temp;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ConvertInUpperTriangularView()
            {
                double temp;
                int n = -1;

                for (int j = 0; j < _numberOfRow; j++)
                {
                    n = MaxIndexInRow(j);
                    if (n == j) { SwapRowsInMatrix(n, j); }
                    for (int i = j + 1; i < _numberOfRow; i++)
                    {
                        temp = _components[i, j] / _components[j, j];
                        for (int k = 0; k < _numberOfRow; k++)
                        {
                            _components[i, k] -= temp * _components[j, k];
                        }
                    }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double Determinant()
            {
                if (!IsSquare) { throw new NotSupportedException("The operation of the determinant can not be used in this object"); }
                Matrix temp = new Matrix(this);

                double det = 1.0;
                temp.ConvertInUpperTriangularView();

                for(int i=0; i<_numberOfRow; i++)
                {
                    det *= temp._components[i, i];
                }

                return det;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Rank()
            {
                Matrix temp = new Matrix(this);
                temp.ConvertInUpperTriangularView();

                int count = 0;
                for(int i=0; i<_numberOfRow; i++)
                {
                    for(int j=0; j<_numberOfColumn; j++)
                    {
                        if (temp._components[i, j] == 0.0) { count++; break; }
                    }
                }

                return count;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Matrix Inverse()
            {
                if (!IsSquare) { throw new NotSupportedException("The operation of the reverse can not be used in this object"); }

                Matrix result = new Matrix(_numberOfRow, _numberOfColumn);
                Matrix temp;
                double det = Determinant();
                double detReverse = ((1.0) / det);

                if (det == 0.0) { return this; }

                for(int i=0; i<_numberOfRow; i++)
                {
                    for(int j=0; j<_numberOfColumn;j++)
                    {
                        temp = Minor(i, j);
                        result._components[j, i] = Math.Pow(-1.0, i + j) * detReverse * temp.Determinant();
                    }
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double[,] ToSquareArrayDouble()
            {
                return _components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<List<double>> ToSqareListDouble()
            {
                return (List<List<double>>)this;
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
            public static Matrix Parse(string s)
            {
                double[,] components;
                try
                {
                    s = s.Replace(".", ",");
                    string pattern = @"([+-]?[\d]+([.,]\d+)?)|(?=(?i)[A-Z])";
                    string[] rows = Regex.Split(s, "\n", RegexOptions.Compiled);
                    int numberOfColumn = Regex.Matches(rows[0], " ", RegexOptions.Compiled).Count;
                    int numberOfRow = rows.Count();
                    components = new double[numberOfRow, numberOfColumn];
                    int j;

                    for (int i = 0; i < numberOfRow; i++)
                    {
                        j = 0;
                        foreach (Match match in Regex.Matches(rows[i], pattern, RegexOptions.Compiled))
                        {
                            components[i, j++] = double.Parse(match.Value);
                        }
                    }

                    return new Matrix(components);
                }
                catch(FormatException)
                {
                    components = new double[2, 2];
                    for(int i=0; i<2; i++)
                    {
                        for(int j=0; j<2; j++)
                        {
                            components[i, j] = 0.0;
                        }
                    }

                    return new Matrix(components);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Matrix obj)
            {
                obj = new Matrix();
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator /(Matrix a, Matrix b) => a * (b.Inverse());

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix operator /(Matrix a, double b) => a * (1.0 / b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double Determinant(Matrix obj) => obj.Determinant();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Transpose(Matrix obj) => obj.Transpose();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Reverse(Matrix obj) => obj.Inverse();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Rank(Matrix obj) => obj.Rank();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Pow(Matrix obj, int n) => obj.Pow(n);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Matrix a, Matrix b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Add(Matrix a, Matrix b) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Subtract(Matrix a, Matrix b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Multiply(Matrix a, Matrix b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Multiply(Matrix a, Vector b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Multiply(Matrix a, double b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Multiply(double a, Matrix b) => b * a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Divide(Matrix a, Matrix b) => a / b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Matrix Divide(Matrix a, double b) => a / b;
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
            public static implicit operator Matrix(string s)
            {
                return Parse(s);
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
        public sealed class Tensor : Matrix
        {
            #region FIELD
            private readonly int _rank;
            #endregion
            #region CONSTRUCTORS 
            public Tensor(int r) : base(r, r) => _rank = r;
            public Tensor(int r, double[,] components) : base(r, r, components) => _rank = r;
            public Tensor(double[,] components) : base(components) => _rank = components.GetLength(0);
            public Tensor(Tensor obj) : base(obj.Components) => _rank = obj.CountOfRow;
            public Tensor() : base() => _rank = 2;
            #endregion
            #region METHODS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public new Tensor Transpose() => new Tensor(base.Transpose().Components);

            public double Sp()
            {
                double s = 0.0;
                for(int i=0; i<_rank; i++)
                {
                    s += Components[i, i];
                }

                return s;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double FirstInvariant() => Sp();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double SecondInvariant()
            {
                Tensor q = this * this;
                return 0.5 * (Sp() * Sp() - q.Sp());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ThirdInvariant() => Determinant();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tensor Dev()
            {
                Tensor g = IdentityTensor(_rank);
                return this - g * (1.0 / 3.0) * Sp();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tensor IsotropicPart() => IdentityTensor(_rank) * (1.0 / 3.0) * Sp();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tensor SymmetricalPart() => 0.5 * (this + Transpose());

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tensor SkewSymmetricalPart() => 0.5 * (this - Transpose());

            public new void ConvertInUpperTriangularView() => throw new NotSupportedException("This operation has not been supported this type");

            public new Tensor Minor(int row, int column) => throw new NotSupportedException("This operation has not been supported this type");

            public new double Norm(TypesNormOfMatrix types) => throw new NotSupportedException("This operation has not been supported this type");

            #endregion
            #region STATIC MEMBERS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor IdentityTensor(int rank)
            {
                return new Tensor(Indentity(rank, rank).Components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public new static Tensor Parse(string s) => Matrix.Parse(s).Components;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Tensor t)
            {
                try
                {
                    t = Parse(s);
                    return true;
                }
                catch
                {
                    t = new Tensor();
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator * (Tensor a, Vector b)
            {
                if (a._rank != b.Measurement) { throw new ArithmeticException("Size of Tensor and vector are not equaling each other"); }
                double[] components = new double[a._rank];

                double s;
                for (int i = 0; i < a._rank; i++)
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
            public static Tensor operator * (Tensor a, Tensor b)
            {
                if (a._rank != b._rank) { throw new ArgumentException("The ranks of the tensors are not being equal"); }

                double[,] components = new double[a._rank, b._rank];
                double s;

                for (int i = 0; i < a._rank; i++)
                {
                    for (int j = 0; j < b._rank; j++)
                    {
                        s = 0.0;
                        for (int k = 0; k < a._rank; k++)
                        {
                            s += a[i, k] * b[k, j];
                        }
                        components[i, j] = s;
                    }
                }

                return new Tensor(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor operator*(Tensor t, double a)
            {
                double[,] tensor = new double[t._rank, t._rank];

                for(int i=0; i<t._rank; i++)
                {
                    for(int j=0; j<t._rank; j++)
                    {
                        tensor[i, j] = t[i, j] * a;
                    }
                }

                return new Tensor(tensor);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor operator *(double a, Tensor t) => t * a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor operator /(Tensor a, double b) => a * (1.0 / b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor operator - (Tensor a, Tensor b)
            {
                if (a._rank != b._rank) { throw new ArgumentException("The ranks of the tensors are not being equal"); }

                double[,] tensor = new double[a._rank, a._rank];

                for(int i=0; i<a._rank; i++)
                {
                    for(int j=0; j<a._rank; j++)
                    {
                        tensor[i, j] = a[i, j] - b[i, j];
                    }
                }

                return new Tensor(tensor);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor operator + (Tensor a, Tensor b)
            {
                if (a._rank != b._rank) { throw new ArgumentException("The ranks of the tensors are not being equal"); }

                double[,] tensor = new double[a._rank, a._rank];

                for (int i = 0; i < a._rank; i++)
                {
                    for (int j = 0; j < a._rank; j++)
                    {
                        tensor[i, j] = a[i, j] + b[i, j];
                    }
                }

                return new Tensor(tensor);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Tensor a, Tensor b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor Add(Tensor a, Tensor b) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor Subtract(Tensor a, Tensor b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor Multiply(Tensor a, Tensor b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Multiply(Tensor a, Vector b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor Multiply(Tensor a, double b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor Multiply(double a, Tensor b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Tensor Divide(Tensor a, double b) => a / b;
            #endregion
            #region PROPERTIES
            public new int Rank { get => _rank - 1; }
            public new bool IsSquare => true;
            #endregion
            #region TYPE CONVERSIONS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Tensor(double[,] components)
            {
                return new Tensor(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double[,](Tensor t)
            {
                return t.Components;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Tensor(List<List<double>> obj)
            {
                if (obj[0].Count != obj[1].Count) { throw new ArgumentException("The tensor can not be non-square"); }

                double[,] components = new double[obj[0].Count, obj[1].Count];

                for (int i = 0; i < components.GetLength(0); i++)
                {
                    for (int j = 0; j < components.GetLength(1); j++)
                    {
                        components[i, j] = obj[i][j];
                    }
                }

                return new Tensor(components);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator List<List<double>>(Tensor t)
            {
                List<List<double>> list = new List<List<double>>();
                List<double> rows;

                for (int i = 0; i < t._rank; i++)
                {
                    rows = new List<double>();
                    for (int j = 0; j < t._rank; j++)
                    {
                        rows.Add(t[i, j]);
                    }
                    list.Add(rows);
                    rows = null;
                }

                return list;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Tensor(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string(Tensor t)
            {
                return t.ToString();
            }
            #endregion
        }

        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class Point3D:IMathematicalObject, IArithmeticOperations, IComparisonOperations, IEquatable<Point3D>, IEnumerable<double>
        {
            #region FIELDS
            private readonly double _x;
            private readonly double _y;
            private readonly double _z;
            #endregion
            #region CONSTRUCTORS

            public Point3D(double x=0.0, double y=0.0, double z=0.0)
            {
                _x = x;
                _y = y;
                _z = z;
            }

            public Point3D(Point3D obj)
            {
                _x = obj._x;
                _y = obj._y;
                _z = obj._z;
            }
            #endregion
            #region METHODS

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
                return (obj is Point3D) ? (this - (Point3D)obj) : throw new ArgumentException("Only Point3D type alowed");
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

            public bool Equals(Point3D other)
            {
                return ((_x == other._x) && (_y == other._y) && (_z == other._z));
            }

            public override bool Equals(object obj)
            {
                return (obj is Point3D) ? Equals((Point3D)obj) : false;
            }

            public override int GetHashCode()
            {
                return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
            }

            public override string ToString()
            {
                return $"[X={_x};\tY={_y};\tZ={_z}]";
            }

            public IEnumerator<double> GetEnumerator()
            {
                yield return _x;
                yield return _y;
                yield return _z;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Point3D Offset(double dx, double dy, double dz)
            {
                return new Point3D(_x + dx, _y + dy, _z + dz);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double[] ToArrayDouble()
            {
                return new double[] { _x, _y, _z };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tuple<double,double,double> ToTupleDouble()
            {
                return new Tuple<double, double, double>(_x, _y, _z);
            }
            #endregion
            #region STATIC MEMBERS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsInfinity(Point3D p) => double.IsInfinity(p._x) || double.IsInfinity(p._y) || double.IsInfinity(p._z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsXPossitiveInfinity(Point3D p) => double.IsPositiveInfinity(p._x) && p._y == 0.0 && p._z == 0.0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsYPossitiveInfinity(Point3D p) => double.IsPositiveInfinity(p._y) && p._x == 0.0 && p._z == 0.0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZPossitiveInfinity(Point3D p) => double.IsPositiveInfinity(p._z) && p._x == 0.0 && p._y == 0.0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsXNegativeInfinity(Point3D p) => double.IsNegativeInfinity(p._x) && p._y == 0.0 && p._z == 0.0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsYNegativeInfinity(Point3D p) => double.IsNegativeInfinity(p._y) && p._x == 0.0 && p._z == 0.0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZNegativeInfinity(Point3D p) => double.IsNegativeInfinity(p._z) && p._x == 0.0 && p._y == 0.0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNaN(Point3D p) => double.IsNaN(p._x) || double.IsNaN(p._y) || double.IsNaN(p._z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsZero(Point3D p) => p._x == 0.0 && p._y == 0.0 && p._z == 0.0;

            public static Point3D XPossitiveInfinity { get => new Point3D(double.PositiveInfinity); }

            public static Point3D YPossitiveInfinity { get => new Point3D(0.0, double.PositiveInfinity); }

            public static Point3D ZPossitiveInfinity { get => new Point3D(0.0, 0.0, double.PositiveInfinity); }

            public static Point3D XNegativeInfinity { get => new Point3D(double.NegativeInfinity); }

            public static Point3D YNegativeInfinity { get => new Point3D(0.0, double.NegativeInfinity); }

            public static Point3D ZNegativeInfinity { get => new Point3D(0.0, 0.0, double.NegativeInfinity); }

            public static Point3D Zero { get => new Point3D(); }

            public static Point3D Infinity { get => new Point3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity); }

            public static Point3D NaN { get => new Point3D(double.NaN, double.NaN, double.NaN); }

            public static Point3D Epsilon { get => new Point3D(double.Epsilon, double.Epsilon, double.Epsilon); }

            public static Point3D XEpsilon { get => new Point3D(double.Epsilon); }

            public static Point3D YEpsilon { get => new Point3D(0.0, double.Epsilon); }

            public static Point3D ZEpsilon { get => new Point3D(0.0, 0.0, double.Epsilon); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D Parse(string s)
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
                            string[] coords = new string[3];
                            int i = 0;

                            foreach (Match m in Regex.Matches(s, pattern, RegexOptions.Compiled))
                            {
                                if (i > 2) { break; }
                                coords[i++] = m.Value;
                            }

                            coords[0] = Regex.Replace(coords[0], ";", string.Empty, RegexOptions.Compiled);
                            coords[1] = Regex.Replace(coords[1], ";", string.Empty, RegexOptions.Compiled);

                            if(!string.IsNullOrEmpty(coords[0]) && string.IsNullOrEmpty(coords[1]) && string.IsNullOrEmpty(coords[2]))
                            {
                                return new Point3D(double.Parse(coords[0]), 0.0, 0.0);
                            }
                            else if(string.IsNullOrEmpty(coords[0]) && !string.IsNullOrEmpty(coords[1]) && string.IsNullOrEmpty(coords[2]))
                            {
                                return new Point3D(0.0, double.Parse(coords[1]), 0.0);
                            }
                            else if(string.IsNullOrEmpty(coords[0]) && string.IsNullOrEmpty(coords[1]) &&! string.IsNullOrEmpty(coords[2]))
                            {
                                return new Point3D(0.0, 0.0, double.Parse(coords[2]));
                            }
                            else if(!string.IsNullOrEmpty(coords[0]) && !string.IsNullOrEmpty(coords[1]) && string.IsNullOrEmpty(coords[2]))
                            {
                                return new Point3D(double.Parse(coords[0]), double.Parse(coords[1]), 0.0);
                            }
                            else if(!string.IsNullOrEmpty(coords[0]) && string.IsNullOrEmpty(coords[1]) && !string.IsNullOrEmpty(coords[2]))
                            {
                                return new Point3D(double.Parse(coords[0]), 0.0, double.Parse(coords[2]));
                            }
                            else if(string.IsNullOrEmpty(coords[0]) && !string.IsNullOrEmpty(coords[1]) && !string.IsNullOrEmpty(coords[2]))
                            {
                                return new Point3D(0.0, double.Parse(coords[1]), double.Parse(coords[2]));
                            }

                            return coords.Select(n => double.Parse(n)).ToArray<double>();
                        }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Point3D p)
            {
                try
                {
                    p = Parse(s);
                    return true;
                }
                catch
                {
                    p = new Point3D();
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector operator - (Point3D a, Point3D b)
            {
                return new Vector(b._x - a._x, b._y - a._y, b._z - a._z);
            }        
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D operator++(Point3D a)
            {
                return new Point3D(a._x + 1.0, a._y + 1.0, a._z + 1.0);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D operator--(Point3D a)
            {
                return new Point3D(a._x - 1.0, a._y - 1.0, a._z - 1.0);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D operator+(Point3D a)
            {
                return new Point3D(+a._x, +a._y, +a._z);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D operator-(Point3D a)
            {
                return new Point3D(-a._x, -a._y, -a._z);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(Point3D a, Point3D b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(Point3D a, Point3D b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector Subtract(Point3D a, Point3D b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D Increment(Point3D a) => ++a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D Decrement(Point3D a) => --a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D Plus(Point3D a) => +a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point3D Negate(Point3D a) => -a;
            #endregion
            #region PROPERTIES
            public double X { get => _x; }
            public double Y { get => _y; }
            public double Z { get => _z; }
            #endregion
            #region TYPE CONVERSIONS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point3D(double[] coords)
            {
                try
                {
                    return new Point3D(coords[0], coords[1], coords[2]);
                }
                catch
                {
                    return new Point3D();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator double[](Point3D p)
            {
                return new double[3] { p._x, p._y, p._z };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point3D(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string(Point3D p)
            {
                return p.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Point3D(Tuple<double,double,double> tuple)
            {
                return new Point3D(tuple.Item1, tuple.Item2, tuple.Item3);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Tuple<double,double,double>(Point3D p)
            {
                return new Tuple<double, double, double>(p._x, p._y, p._z);
            }
            #endregion
        }
        
        public enum TypesNormOfQuaternion
        {
            EuclideanNorm, EuclideanSquaredNorm
        }

        public enum TypesQuaternionNormalization
        {
            EuclideanNormalization, EuclideanSquaredNormalization
        }

        [StructLayout(LayoutKind.Auto),Serializable]
        public sealed class Quaternion:IMathematicalObject,IArithmeticOperations,IComparisonOperations,IEquatable<Quaternion>,IEnumerable<double>
        {
            #region FIELDS
            private readonly double _x;
            private readonly double _y;
            private readonly double _z;
            private readonly double _w;
            #endregion
            #region CONSTRUCTORS
            public Quaternion(double x=0.0, double y=0.0, double z=0.0, double w=0.0)
            {
                _x = x;
                _y = y;
                _z = z;
                _w = w;
            }

            public Quaternion(Vector v, double anglePart)
            {
               try
                {
                    _x = v[0];
                    _y = v[1];
                    _z = v[2];
                    _w = anglePart;
                }
                catch
                {
                    _x = 0;
                    _y = 0;
                    _z = 0;
                    _w = anglePart;
                }
            }

            public Quaternion(double[] components)
            {
                try
                {
                    _x = components[0];
                    _y = components[1];
                    _z = components[2];
                    _w = components[3];
                }
                catch
                {
                    _x = 0.0;
                    _y = 0.0;
                    _z = 0.0;
                    _w = 0.0;
                }
            }

            public Quaternion(Complex z1, Complex z2)
            {
                _x = z1.Re;
                _y = z1.Im;
                _x = z2.Re;
                _w = z2.Im;
            }
            #endregion
            #region METHODS
            string IMathematicalObject.Show()
            {
                return ToString();
            }

            IMathematicalObject IArithmeticOperations.Addition(IMathematicalObject obj)
            {
                return (obj is Quaternion) ? (this + (Quaternion)obj) : throw new ArgumentException("Only quaternion type alowed");
            }

            IMathematicalObject IArithmeticOperations.Subtraction(IMathematicalObject obj)
            {
                return (obj is Quaternion) ? (this - (Quaternion)obj) : throw new ArgumentException("Only quaternion type alowed");
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                return (obj is Quaternion) ? (this * (Quaternion)obj) : throw new ArgumentException("Only quaternion type alowed");
            }

            IMathematicalObject IArithmeticOperations.Division(IMathematicalObject obj)
            {
                return (obj is Quaternion) ? (this / (Quaternion)obj) : throw new ArgumentException("Only quaternion type alowed");
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

            public bool Equals(Quaternion other)
            {
                return _x == other._x && _y == other._y && _z == other._z && _w == other._w;
            }

            public override bool Equals(object obj)
            {
                return (obj is Quaternion) ? Equals((Quaternion)obj) : false;
            }

            public override int GetHashCode()
            {
                return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode() ^ _w.GetHashCode();
            }

            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"X:{_x} ");
                stringBuilder.Append($"Y:{_y} ");
                stringBuilder.Append($"Z:{_z} ");
                stringBuilder.Append($"W:{_w}");
                return stringBuilder.ToString();
            }

            public IEnumerator<double> GetEnumerator()
            {
                yield return _x;
                yield return _y;
                yield return _z;
                yield return _w;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double Norm(TypesNormOfQuaternion types)
            {
                switch(types)
                {
                    case TypesNormOfQuaternion.EuclideanNorm: return Abs;
                    case TypesNormOfQuaternion.EuclideanSquaredNorm: return Abs * Abs;
                    default: throw new ArgumentException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Quaternion Normalize(TypesQuaternionNormalization types)
            {
                switch(types)
                {
                    case TypesQuaternionNormalization.EuclideanNormalization: return this / Norm(TypesNormOfQuaternion.EuclideanNorm);
                    case TypesQuaternionNormalization.EuclideanSquaredNormalization: return this / Norm(TypesNormOfQuaternion.EuclideanSquaredNorm);
                    default: throw new ArgumentException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Quaternion Inverse() => Conjugate / (Abs * Abs);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tuple<Complex, Complex> ToComplexNumbers()
            {
                Complex z1, z2;
                ToComplexNumbers(this, out z1, out z2);
                return new Tuple<Complex, Complex>(z1, z2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ScalarProduct(Quaternion q)=> _x * q._x + _y * q._y + _z * q._z + _w * q._w;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Matrix ToMatrix()
            {
                return this;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Tuple<double,double,double,double> ToTupleDouble()
            {
                return new Tuple<double, double, double, double>(_x, _y, _z, _w);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double[] ToArrayDouble()
            {
                return new double[] { _x, _y, _z, _w };
            }
            #endregion
            #region STATIC MEMBERS

            public static Quaternion Identity { get => new Quaternion(0, 0, 0, 1.0); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion ConjugateQuaternionNumber(Quaternion obj) => new Quaternion(obj._x, -obj._y, -obj._z, -obj._w);

            public static Quaternion XPossitiveInfinity { get => new Quaternion(double.PositiveInfinity); }

            public static Quaternion YPossitiveInfinity { get => new Quaternion(y:double.PositiveInfinity); }

            public static Quaternion ZPossitiveInfinity { get => new Quaternion(z: double.PositiveInfinity); }

            public static Quaternion WPossitiveInfinity { get => new Quaternion(w: double.PositiveInfinity); }

            public static Quaternion XNegativeInfinity { get => new Quaternion(double.NegativeInfinity); }

            public static Quaternion YNegativeInfinity { get => new Quaternion(y: double.NegativeInfinity); }

            public static Quaternion ZNegativeInfinity { get => new Quaternion(z: double.NegativeInfinity); }

            public static Quaternion WNegativeInfinity { get => new Quaternion(w: double.NegativeInfinity); }

            public static Quaternion Zero { get => new Quaternion(); }

            public static Quaternion NaN { get => new Quaternion(double.NaN, double.NaN, double.NaN, double.NaN); }

            public static Quaternion Infinity { get => new Quaternion(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity); }

            public static Quaternion Epsilon { get => new Quaternion(double.Epsilon, double.Epsilon, double.Epsilon, double.Epsilon); }

            public static Quaternion XEpsilon { get => new Quaternion(double.Epsilon); }

            public static Quaternion YEpsilon { get => new Quaternion(y: double.Epsilon); }

            public static Quaternion ZEpsilon { get => new Quaternion(z: double.Epsilon); }

            public static Quaternion WEpsilon { get => new Quaternion(w: double.Epsilon); }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Parse(string s)
            {
                switch(s)
                {
                    case "0": return Zero;
                    case "∞": return Infinity;
                    case "¿": return NaN;
                    default:
                        {
                            s = s.Replace(" ", string.Empty).Replace(".", ",");
                            string pattern = @"[+-]?(?i)[ijk]?[\d]+([.,]\d+)?(?i)[ijk]?";
                            string pattern2 = @"(?i)[ijk]";

                            Dictionary<string, double> quaternionKeysValues = new Dictionary<string, double>()
                            {
                                [string.Empty] = 0.0,
                                ["i"] = 0.0,
                                ["j"] = 0.0,
                                ["k"] = 0.0
                            };

                            MatchCollection collection = Regex.Matches(s, pattern, RegexOptions.Compiled);

                            foreach(Match match in collection)
                            {
                                if (!(match.Value.Contains("i") || match.Value.Contains("j") || match.Value.Contains("k")))
                                {
                                    quaternionKeysValues[string.Empty] = double.Parse(match.Value);
                                }
                                else if (match.Value.Contains("i"))
                                {
                                    quaternionKeysValues["i"] = double.Parse(Regex.Replace(match.Value, pattern2, string.Empty, RegexOptions.Compiled));
                                }
                                else if (match.Value.Contains("j"))
                                {
                                    quaternionKeysValues["j"] = double.Parse(Regex.Replace(match.Value, pattern2, string.Empty, RegexOptions.Compiled));
                                }
                                else if (match.Value.Contains("k"))
                                {
                                    quaternionKeysValues["k"] = double.Parse(Regex.Replace(match.Value, pattern2, string.Empty, RegexOptions.Compiled));
                                }
                            }

                            var quaternionValues = quaternionKeysValues.Values.ToArray();

                            return new Quaternion(quaternionValues);
                        }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryParse(string s, out Quaternion q)
            {
                try
                {
                    q = Parse(s);
                    return true;
                }
                catch
                {
                    q = new Quaternion();
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ToComplexNumbers(Quaternion q, out Complex z1, out Complex z2)
            {
                z1 = new Complex(q._x, q._y);
                z2 = new Complex(q._z, q._w);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ToMatrix(Quaternion q, out Matrix m) => m = q.ToMatrix();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Normalize(Quaternion a, TypesQuaternionNormalization types)
            {
                return a.Normalize(types);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static double ScalarProduct(Quaternion q1, Quaternion q2) => q1.ScalarProduct(q2);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Lerp(Quaternion q1, Quaternion q2, double amount)
            {
                Quaternion result;
                double amount1 = 1 - amount;
                double scalarProduct = ScalarProduct(q1, q2);

                if(scalarProduct >= 0.0)
                {
                    result = amount1 * q1 + amount * q2;
                }
                else
                {
                    result = amount1 * q1 - amount * q2;
                }

                return Normalize(result, TypesQuaternionNormalization.EuclideanNormalization);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(Quaternion q1, Quaternion q2, double amount, out Quaternion result) => result = Lerp(q1, q2, amount);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Slerp(Quaternion q1, Quaternion q2, double amount)
            {
                double scalarProduct = ScalarProduct(q1, q2);
                double amount2, amount3;
                bool flag = false;

                if(scalarProduct < 0.0)
                {
                    flag = true;
                    scalarProduct = -scalarProduct;
                }

                if(scalarProduct > 1.0)
                {
                    amount2 = 1 - amount;
                    amount3 = flag ? -amount : amount;
                }
                else
                {
                    double angle1 = Math.Acos(scalarProduct);
                    double angle2 = (1.0) / Math.Sin(angle1);

                    amount2 = Math.Sin((1 - amount) * angle1) * angle2;
                    amount3 = flag ? -Math.Sin(amount * angle1) * angle2 : Math.Sin(amount * angle1) * angle2;
                }

                return amount2 * q1 + amount3 * q2;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Slerp(Quaternion q1, Quaternion q2, double amount, out Quaternion result) => result = Slerp(q1, q2, amount);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator+(Quaternion a, Quaternion b)=> new Quaternion(a.Vector3D + b.Vector3D, a.W + b.W);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator +(Quaternion a, double d) => new Quaternion(a._x + d, a._y, a._z, a._w);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator +(double d, Quaternion a) => a + d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator -(Quaternion a, Quaternion b) => new Quaternion(a._x - b._x, a._y - b._y, a._z - b._z, a._w - b._w);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator -(Quaternion a, double d) => new Quaternion(a._x - d, a._y, a._z, a._w);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator -(double d, Quaternion a) => new Quaternion(d - a._x, -a._y, -a._z, -a._w);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator*(Quaternion a, Quaternion b) => new Quaternion(a.W * b.Vector3D + b.W * a.Vector3D + Vector.Cross(a.Vector3D, b.Vector3D), a.W * b.W - a.Vector3D * b.Vector3D);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator *(Quaternion a, double d) => new Quaternion(a._x * d, a._y * d, a._z * d, a._w * d);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator *(double d, Quaternion a) => a * d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator /(Quaternion q1, Quaternion q2)
            {
                double x = q1._x;
                double y = q1._y;
                double z = q1._z;
                double w = q1._w;
                double absSquared = q2.X * q2.X + q2.Y * q2.Y + q2.Z * q2.Z + q2.W * q2.W;
                double num2 = 1.0 / absSquared;
                double num3 = -q2.X * num2;
                double num4 = -q2.Y * num2;
                double num5 = -q2.Z * num2;
                double num6 = q2.W * num2;
                double num7 = y * num5 - z * num4;
                double num8 = z * num3 - x * num5;
                double num9 = x * num4 - y * num3;
                double num10 = x * num3 + y * num4 + z * num5;
                return new Quaternion(x * num6 + num3 * w + num7, y * num6 + num4 * w + num8, z * num6 + num5 * w + num9, w * num6 - num10);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator /(Quaternion a, double d) => a * (1.0 / d);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(Quaternion a, Quaternion b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator !=(Quaternion a, Quaternion b) => !a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator +(Quaternion a)
            {
                return new Quaternion(+a._x, +a._y, +a._z, +a._w);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator -(Quaternion a)
            {
                return new Quaternion(-a._x, -a._y, -a._z, -a._w);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator++(Quaternion a)
            {
                return new Quaternion(a._x + 1, a._y + 1, a._z + 1, a._w + 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion operator--(Quaternion a)
            {
                return new Quaternion(a._x - 1, a._y - 1, a._z - 1, a._w - 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Add(Quaternion a, Quaternion b) => a + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Add(Quaternion a, double d) => a + d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Add(double d, Quaternion a) => a + d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Subtract(Quaternion a, Quaternion b) => a - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Subtract(Quaternion a, double d) => a - d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Subtract(double d, Quaternion a) => d - a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Multiply(Quaternion a, Quaternion b) => a * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Multiply(Quaternion a, double d) => a * d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Multiply(double d, Quaternion a) => a * d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Divide(Quaternion q1, Quaternion q2) => q1 / q2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Divide(Quaternion q1, double d) => q1 / d;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Quaternion a, Quaternion b) => a.Equals(b);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Plus(Quaternion a) => +a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Negate(Quaternion a) => -a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Increment(Quaternion a) => ++a;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quaternion Decrement(Quaternion a) => --a;
            #endregion
            #region PROPERTIES
            public bool IsInfinity { get => double.IsInfinity(_x) || double.IsInfinity(_y) || double.IsInfinity(_z) || double.IsInfinity(_w); }
            public bool IsXPossitiveInfinity { get => double.IsPositiveInfinity(_x) && _y == 0.0 && _z == 0.0 && _z == 0.0; }
            public bool IsYPossitiveInfinity { get => double.IsPositiveInfinity(_y) && _x == 0.0 && _z == 0.0 && _w == 0.0; }
            public bool IsZPossitiveInfinity { get => double.IsPositiveInfinity(_z) && _x == 0.0 && _y == 0.0 && _w == 0.0; }
            public bool IsWPossitiveInfinity { get => double.IsPositiveInfinity(_w) && _x == 0.0 && _y == 0.0 && _z == 0.0; }
            public bool IsXNegativeInfinity { get => double.IsNegativeInfinity(_x) && _y == 0.0 && _z == 0.0 && _z == 0.0; }
            public bool IsYNegativeInfinity { get => double.IsNegativeInfinity(_y) && _x == 0.0 && _z == 0.0 && _w == 0.0; }
            public bool IsZNegativeInfinity { get => double.IsNegativeInfinity(_z) && _x == 0.0 && _y == 0.0 && _w == 0.0; }
            public bool IsWNegativeInfinity { get => double.IsNegativeInfinity(_w) && _x == 0.0 && _y == 0.0 && _z == 0.0; }
            public bool IsNaN { get => double.IsNaN(_x) || double.IsNaN(_y) || double.IsNaN(_z) || double.IsNaN(_w); }
            public bool IsZero { get => _x == 0.0 && _y == 0.0 && _z == 0.0 && _w == 0.0; }
            public bool IsIdentity { get => _x == 0.0 && _y == 0.0 && _z == 0.0 && _w == 1.0; }
            public double X { get => _x; }
            public double Y { get => _y; }
            public double Z { get => _z; }
            public double W { get => _w; }
            public Quaternion Conjugate { get => new Quaternion(_x, -_y, -_z, -_w); }
            public double Abs { get => Math.Sqrt(_x * _x + _y * _y + _z * _z + _w * _w); }
            public double AbsSquared { get => Abs * Abs; }
            public Vector Vector3D { get => new Vector(_x, _y, _z); }
            public double Scalar { get => _w; }
            #endregion
            #region TYPE CONVERSIONS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Matrix(Quaternion t)
            {
                Matrix quaternionMatrix = new Matrix(4, 4)
                {
                    Components = new double[4, 4]
                    {
                        { t._x,-t._y,-t._z,-t._w},
                        { t._y,t._x,-t._w,t._z},
                        { t._z,t._w,t._x,-t._y},
                        { t._w,-t._z,t._y,t._x}
                    }
                };

                return quaternionMatrix;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Quaternion(Matrix matrix)
            {
                try
                {
                    return new Quaternion(matrix[0, 0], -matrix[0, 1], -matrix[0, 2], -matrix[0, 3]);
                }
                catch
                {
                    return new Quaternion();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Quaternion(string s)
            {
                return Parse(s);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string(Quaternion t)
            {
                return t.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Quaternion(Tuple<double,double,double,double> tuple)
            {
                return new Quaternion(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Tuple<double,double,double,double>(Quaternion t)
            {
                return new Tuple<double, double, double, double>(t._x, t._y, t._z, t._w);
            }
            #endregion
        }

        [StructLayout(LayoutKind.Auto),Serializable]
        public sealed class Function:IMathematicalObject,IArithmeticOperations,IComparisonOperations,IEquatable<Function>
        {
            #region FIELDS
            private readonly Func<double, double> _expression;
            #endregion
            #region CONSTRUCTORS
            public Function()
            {
                _expression = default(Func<double, double>);
            }

            public Function(Func<double,double> expression=null)
            {
                _expression = expression ?? throw new ArgumentNullException("The Expression is null");
            }
            #endregion
            #region METHODS

            IMathematicalObject IArithmeticOperations.Addition(IMathematicalObject obj)
            {
                return (obj is Function) ? (this + (Function)obj) : throw new ArgumentException("Only function type alowed");
            }

            IMathematicalObject IArithmeticOperations.Division(IMathematicalObject obj)
            {
                return (obj is Function) ? (this / (Function)obj) : throw new ArgumentException("Only function type alowed");
            }

            IMathematicalObject IArithmeticOperations.Multiplication(IMathematicalObject obj)
            {
                return (obj is Function) ? (this * (Function)obj) : throw new ArgumentException("Only function type alowed");
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
                return (obj is Function) ? (this - (Function)obj) : throw new ArgumentException("Only function type alowed");
            }

            public bool Equals(Function other)
            {
                return GetHashCode() == other.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return (obj is Function) ? Equals((Function)obj) : false;
            }

            public override int GetHashCode()
            {
                int hash = 17;
                double a = -0.5, b = 0.5, h = (b - a) / 6.0;

                for(double x=a;x<=b;x+=h)
                {
                    hash *= (hash * 31) + _expression(x).GetHashCode();
                }

                return hash;
            }

            public override string ToString()
            {
                return $"{_expression.Method.Name}(x)";
            }

            public string ToString(double x)
            {
                return $"{(_expression.Method.Name)}(x):{ _expression.Invoke(x)}";
            }

            public double ToDouble(double x)
            {
                return _expression(x);
            }

            public Func<double,double> ToFuncDoubleDouble()
            {
                return _expression;
            }

            public double Invoke(double x)
            {
                return _expression.Invoke(x);
            }
            #endregion
            #region OPERATORS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator+(Function f1, Function f2)
            {
                return new Function((x) => f1._expression(x) + f2._expression(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator+(Function f, double b)
            {
                return new Function((x) => f._expression(x) + b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator +(double b, Function f)
            {
                return f + b;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator-(Function f1, Function f2)
            {
                return new Function((x) => f1._expression(x) - f2._expression(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator-(Function f, double b)
            {
                return new Function((x) => f._expression(x) - b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator-(double b, Function f)
            {
                return new Function((x) => b - f._expression(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator*(Function f1, Function f2)
            {
                return new Function((x) => f1._expression(x) * f2._expression(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator*(Function f, double b)
            {
                return new Function((x) => f._expression(x) * b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator*(double b, Function f)
            {
                return f * b;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator/(Function f1, Function f2)
            {
                return new Function((x) => f1._expression(x) / f2._expression(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator/(Function f, double b)
            {
                return new Function((x) => f._expression(x) / b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function operator/(double b, Function f)
            {
                return new Function((x) => b / f._expression(x));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(Function f1, Function f2)
            {
                return f1.Equals(f2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(Function f1, Function f2)
            {
                return !f1.Equals(f2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Add(Function f1, Function f2) => f1 + f2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Add(Function f, double b) => f + b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Add(double b, Function f) => b + f;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Subtract(Function f1, Function f2) => f1 - f2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Subtract(Function f, double b) => f - b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Subtract(double b, Function f) => b - f;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Multiply(Function f1, Function f2) => f1 * f2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Multiply(Function f, double b) => f * b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Multiply(double b, Function f) => b * f;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Divide(Function f1, Function f2) => f1 / f2;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Divide(Function f, double b) => f / b;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Function Divide(double b, Function f) => b / f;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(Function f1, Function f2) => f1 == f2;
            #endregion
            #region PROPERTIE
            public Func<double,double> Expression { get => _expression; }
            #endregion
            #region TYPE CONVERSIONS

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Function(Func<double,double> expression)
            {
                return new Function(expression);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Function(double d)
            {
                return new Function((x) => d);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Func<double, double>(Function func)
            {
                return func._expression;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator string(Function f)
            {
                return f.ToString();
            }
            #endregion
            #region STATIC MEMBERS

            public static double FindFirstDerivative(Function f, double x)
            {
                double step1 = 0.001, step2 = 0.0001, p = 2.0, g = step1 / step2;

                double res1 = (f.Invoke(x + step1) - f.Invoke(x - step1)) / (2.0 * step1);
                double res2 = (f.Invoke(x + step2) - f.Invoke(x - step2)) / (2.0 * step2);

                return res2 + (res2 - res1) / (Math.Pow(g, p) - 1);
            }

            public static double FindSecondDerivative(Function f, double x)
            {
                double step1 = 0.001, step2 = 0.0001, p = 2.0, g = step1 / step2;

                double res1 = (f.Invoke(x + step1) - 2 * f.Invoke(x) + f.Invoke(x - step1)) / (step1 * step1);
                double res2 = (f.Invoke(x + step2) - 2 * f.Invoke(x) + f.Invoke(x - step2)) / (step2 * step2);

                return res2 + (res2 - res1) / (Math.Pow(g, p) - 1);
            }

            public static double FindDerivative(Function f, double x, int order)
            {
                if (order < 0) { throw new ArithmeticException(); }
                switch(order)
                {
                    case 0: return f.Invoke(x);
                    case 1: return FindFirstDerivative(f, x);
                    case 2: return FindSecondDerivative(f, x);
                    default:
                        {
                            List<Point2D> points = new List<Point2D>();
                            double a = x - 0.001, b = x + 0.001, h = (b - a) / (order), sum = 0.0, p;
                            for(int i=0;i<=order;i++)
                            {
                                points.Add(new Point2D(a + i * h, f.Invoke(a + i * h)));
                            }

                            for(int i=0; i<points.Count; i++)
                            {
                                p = 1.0;
                                for(int j=0; j<points.Count; j++)
                                {
                                    if(j!=i)
                                    {
                                        p *= (points[i].X - points[j].X);
                                    }
                                }
                                sum += (points[i].Y / p);
                            }

                            return sum * Factorial(order);
                        }
                }
            }
            #endregion
        }
    }
}