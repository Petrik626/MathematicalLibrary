using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Mathematics.Objects;
using static System.Math;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SystemEquations
{
    namespace LinearAlgebraicEquations
    {
        [StructLayout(LayoutKind.Auto), Serializable]
        public sealed class SystemLinearAlgebraicEquations:IEquatable<SystemLinearAlgebraicEquations>, ISerializable
        {
            #region FIELDS
            private readonly Vector _rightPart;
            private readonly Matrix _systemMatrix;
            #endregion
            #region CONSTRUCTORS
            public SystemLinearAlgebraicEquations(Matrix systemMatrix, Vector rightPart)
            {
                _systemMatrix = systemMatrix ?? new Matrix();
                _rightPart = rightPart ?? new Vector();
            }

            public SystemLinearAlgebraicEquations(double[,] systemMatrix, double[] rightPart)
            {
                _systemMatrix = systemMatrix ?? new Matrix();
                _rightPart = rightPart ?? new Vector();
            }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            private SystemLinearAlgebraicEquations(SerializationInfo info, StreamingContext context)
            {
                _rightPart = (Vector)info.GetValue("RightPart", typeof(Vector));
                _systemMatrix = (Matrix)info.GetValue("SystemMatrix", typeof(Matrix));
            }
            #endregion
            #region METHODS
            private Matrix CreateDeterminant(Matrix a, Vector v, int indexcolumn)
            {
                Matrix obj = new Matrix(a);
                for (int i = 0; i < a.CountOfColumn; i++)
                {
                    obj[i, indexcolumn] = v[i];
                }

                return obj;
            }

            private void SwapLines(int j, int k, Matrix m, Vector v)
            {
                double[] temp1 = new double[m.CountOfRow];
                double temp = 0.0;
                for (int i = 0; i < m.CountOfRow; i++)
                {
                    temp1[i] = m[k, i];
                    m[k, i] = m[j, i];
                    m[j, i] = temp1[i];
                    temp = v[j];
                    v[j] = v[k];
                    v[k] = temp;
                }
            }

            private int LineMaxNumber(int j, Matrix m)
            {
                int imax = j; double max = m[j, j];
                for (int k = j + 1; k < m.CountOfRow; k++)
                {
                    if (Abs(m[k, j]) > max) { max = m[k, j]; imax = k; }
                }

                return imax;
            }

            private void ConversionMatrixVector(Vector v, Matrix m)
            {
                double tmp;
                for (int j = 0; j < m.CountOfRow - 1; j++)
                {
                    int k = LineMaxNumber(j, m);
                    if (k != j) { SwapLines(k, j, m, v); }
                    for (int i = j + 1; i < m.CountOfRow; i++)
                    {
                        tmp = m[i, j] / m[j, j];
                        v[i] -= tmp * v[j];
                        for (int n = 0; n < m.CountOfRow; n++)
                        {
                            m[i, n] -= tmp * m[j, n];
                        }
                    }
                }
            }

            private Vector ReturnTrivialSolution(int dimension)
            {
                return new double[dimension];
            }

            public bool Equals(SystemLinearAlgebraicEquations other)
            {
                return ReferenceEquals(this, other);
            }

            public override bool Equals(object obj)
            {
                return (obj is SystemLinearAlgebraicEquations) ? Equals((SystemLinearAlgebraicEquations)obj) : false;
            }

            public override int GetHashCode()
            {
                return _systemMatrix.GetHashCode() ^ _rightPart.GetHashCode();
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Matrix of coefficients:\t" + _systemMatrix.ToString() + "\n");
                sb.Append("The right part of the system\t" + _rightPart.ToString() + "\n");

                return sb.ToString();
            }

            public Vector MethodCramerSolve()
            {
                if (!IsClosedSystem) { throw new InvalidOperationException(); }
                if (IsHomogeneousSystem) { return ReturnTrivialSolution(_rightPart.Measurement); }

                double det = DeterminantSystem;
                List<Matrix> matrixList = new List<Matrix>();
                List<double> detList;

                for (int i = 0; i < _systemMatrix.CountOfColumn; i++)
                {
                    matrixList.Add(CreateDeterminant(_systemMatrix, _rightPart, i));
                }

                detList = matrixList.Select(matrix => matrix.Determinant()).ToList();
                return new Vector(detList.Select(x => x / det).ToArray());
            }

            public Vector MethodGaussSolve()
            {
                Matrix tempMatrix = new Matrix(_systemMatrix);
                Vector tempVector = new Vector(_rightPart);
                Vector x = new Vector(_rightPart.Measurement);
                if (!IsClosedSystem) { throw new InvalidOperationException(); }
                if (IsHomogeneousSystem) { return ReturnTrivialSolution(_rightPart.Measurement); }

                if (!tempMatrix.IsUpperTriangular) { ConversionMatrixVector(tempVector, tempMatrix); }

                double sum = 0.0;
                for (int k = x.Measurement - 1; k >= 0; k--)
                {
                    for (int i = k + 1; i < tempVector.Measurement; i++)
                    {
                        sum += tempMatrix[k, i] * x[i];
                    }
                    x[k] = (1.0 / tempMatrix[k, k]) * (tempVector[k] - sum);
                    sum = 0;
                }

                return x;
            }

            public Vector MethodMatrixSolve()
            {
                if (IsDegenerateSystem) { throw new InvalidOperationException(); }
                return (_systemMatrix.Inverse()) * _rightPart;
            }

            public Vector MethodAlgebraicRunSolve()
            {
                if (!_systemMatrix.IsThreeDiagonal) { throw new InvalidOperationException(); }

                int N = _systemMatrix.CountOfRow - 1;
                double[] ksi = new double[N + 1];
                double[] etta = new double[N + 1];
                Vector resVector = new Vector(N + 1);

                double gamma = _systemMatrix[0, 0];
                ksi[0] = -_systemMatrix[0, 1] / gamma;
                etta[0] = _rightPart[0] / gamma;

                for (int i = 1; i < N; i++)
                {
                    gamma = _systemMatrix[i, i] + _systemMatrix[i, i - 1] * ksi[i - 1];
                    ksi[i] = -(_systemMatrix[i, i + 1]) / gamma;
                    etta[i] = (_rightPart[i] - _systemMatrix[i, i - 1] * etta[i - 1]) / gamma;
                }

                resVector[N] = (_rightPart[N] - _systemMatrix[N, N - 1] * etta[N - 1]) / (_systemMatrix[N, N] + _systemMatrix[N, N - 1] * ksi[N - 1]);
                for (int i = N - 1; i >= 0; i--)
                {
                    resVector[i] = ksi[i] * resVector[i + 1] + etta[i];
                }

                return resVector;
            }

            [SecurityPermission(SecurityAction.Demand,SerializationFormatter = true)]
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("RightPart", _rightPart, typeof(Vector));
                info.AddValue("SystemMatrix", _systemMatrix, typeof(Matrix));
            }
            #endregion
            #region STATIC MEMBERS
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator==(SystemLinearAlgebraicEquations a, SystemLinearAlgebraicEquations b)
            {
                return a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator!=(SystemLinearAlgebraicEquations a, SystemLinearAlgebraicEquations b)
            {
                return !a.Equals(b);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator SystemLinearAlgebraicEquations(Tuple<Matrix, Vector> system)
            {
                return new SystemLinearAlgebraicEquations(system.Item1, system.Item2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator Tuple<Matrix,Vector>(SystemLinearAlgebraicEquations system)
            {
                return new Tuple<Matrix, Vector>(system._systemMatrix, system._rightPart);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Equals(SystemLinearAlgebraicEquations a, SystemLinearAlgebraicEquations b)
            {
                return a.Equals(b);
            }
            #endregion
            #region PROPERTIES
            public Matrix SystemCoefficiensts => _systemMatrix;
            public Vector FreeMembers => _rightPart;
            public double DeterminantSystem => _systemMatrix.Determinant();
            public bool IsDegenerateSystem => _systemMatrix.Determinant() == 0.0;
            public bool IsClosedSystem => _systemMatrix.IsSquare;
            public bool IsHomogeneousSystem => _rightPart.Measurement == _rightPart.Where(x => x == 0.0).Count();
            public int NumberOfUnknowns => _systemMatrix.CountOfColumn;
            #endregion
        }
    }
}
