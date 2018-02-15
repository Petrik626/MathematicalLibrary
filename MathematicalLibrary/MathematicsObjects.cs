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
                            string pattern1 = @"(?i)I";
                            string pattern2 = @"(?=[+-])";
                            if (!Regex.IsMatch(s, pattern1, RegexOptions.Compiled))
                            {
                                return new Complex(double.Parse(s), 0.0);
                            }
                            else
                            {
                                double re, im;
                                string rePart = string.Empty, imPart = string.Empty;

                                string[] target = Regex.Split(s, pattern2, RegexOptions.Compiled);
                                for (int i = 0; i < target.Length; i++)
                                {
                                    if (string.IsNullOrEmpty(target[i])) { continue; }

                                    if (Regex.IsMatch(target[i], pattern1, RegexOptions.Compiled))
                                    {
                                        imPart = Regex.Replace(target[i], pattern1, string.Empty, RegexOptions.Compiled);
                                    }
                                    else
                                    {
                                        rePart = target[i];
                                    }
                                }

                                re = double.Parse(rePart); im = double.Parse(imPart);
                                rePart = null; imPart = null;

                                return new Complex(re, im);
                            }
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
        public struct Point2D:IMathematicalObject, IArithmeticOperations, IComparisonOperations, IEquatable<Point2D>, IEnumerable<double>
        {
            #region FIELDS
            private double _x;
            private double _y;
            #endregion
            #region CONSTRUCTORS
            public Point2D(double x=0.0, double y=0.0)
            {
                _x = x;
                _y = y;
            }

            public Point2D(Point2D obj) : this(obj._x, obj._y) { }
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

            public string Show()
            {
                throw new NotImplementedException();
            }

            public IMathematicalObject Addition(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public IMathematicalObject Subtraction(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public IMathematicalObject Multiplication(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public IMathematicalObject Division(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool OperationIsEquality(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool OperationIsNotEquality(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool OperationIsMore(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool OperationIsLess(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool OperationIsMoreOrEqual(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }

            public bool OperationIsLessOrEqual(IMathematicalObject obj)
            {
                throw new NotImplementedException();
            }
            #endregion
        }

    }
}