using System;
using Mathematics.Objects;
using System.Runtime.CompilerServices;

namespace MathematicalLibrary
{
    namespace Functions
    {
        public static class ComplexFunctions
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Exp(Complex z) => new Complex(Math.Exp(z.Re) * Math.Cos(z.Im), Math.Exp(z.Re) * Math.Sin(z.Im));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Cos(Complex z) => (Exp(Complex.I * z) + Exp(-Complex.I * z)) / (2.0);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Sin(Complex z) => (Exp(Complex.I * z) - Exp(-Complex.I * z)) / (2.0 * Complex.I);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Pow(Complex z, double n) => new Complex(Math.Pow(z.Abs, n) * Math.Cos(n * z.Argument), Math.Pow(z.Abs, n) * Math.Sin(n * z.Argument));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Pow(Complex z, int n) => new Complex(Math.Pow(z.Abs, n) * Math.Cos(n * z.Argument), Math.Pow(z.Abs, n) * Math.Sin(n * z.Argument));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Sqrt(Complex z) => Pow(z, 0.5);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Sqr(Complex z) => z * z;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Tan(Complex z) => Sin(z) / Cos(z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Cot(Complex z) => Cos(z) / Sin(z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Ln(Complex z) => new Complex(Math.Log(z.Abs), z.Argument);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Cosh(Complex z) => ((Exp(z) + Exp(-z)) / (2.0));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Sinh(Complex z) => ((Exp(z) - Exp(-z)) / (2.0));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Tanh(Complex z) => Sinh(z) / Cosh(z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Coth(Complex z) => Cosh(z) / Sinh(z);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Arccos(Complex z) => -Complex.I * Ln(z + Complex.I * Sqrt(1.0 - z * z));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Arcsin(Complex z) => -Complex.I * Ln(Complex.I * z + Sqrt(1.0 - z * z));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Arcsh(Complex z) => Ln(z + Sqrt(z * z + 1.0));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Arcch(Complex z) => Ln(z + Sqrt(z * z - 1.0));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Arctanh(Complex z) => (0.5) * Ln((1.0 + z) / (1.0 - z));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Complex Arccoth(Complex z) => 0.5 * Ln((z + 1) / (z - 1));
        }
    }
}
