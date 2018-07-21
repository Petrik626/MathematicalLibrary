using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics.Objects;

namespace SystemEquations
{
    namespace LinearAlgebraicEquations
    {
        public sealed class SystemLinearAlgebraicEquations:IEquatable<SystemLinearAlgebraicEquations>
        {
            #region FIELDS
            private readonly Vector _rightPart;
            private readonly Matrix _systemMatrix;
            private readonly Vector _solvingSystem;
            #endregion

            #region CONSTRUCTORS
            public SystemLinearAlgebraicEquations(Matrix systemMatrix, Vector rightPart)
            {
                _systemMatrix = systemMatrix ?? new Matrix();
                _rightPart = rightPart ?? new Vector();

                _solvingSystem = new Vector(_rightPart.Measurement);
            }

            public SystemLinearAlgebraicEquations(double[,] systemMatrix, double[] rightPart)
            {
                _systemMatrix = systemMatrix ?? new Matrix();
                _rightPart = rightPart ?? new Vector();

                _solvingSystem = new Vector(_solvingSystem.Measurement);
            }
            #endregion

            #region METHODS
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
            #endregion

            #region STATIC MEMBERS
            public static bool operator==(SystemLinearAlgebraicEquations a, SystemLinearAlgebraicEquations b)
            {
                return a.Equals(b);
            }

            public static bool operator!=(SystemLinearAlgebraicEquations a, SystemLinearAlgebraicEquations b)
            {
                return !a.Equals(b);
            }

            public static implicit operator SystemLinearAlgebraicEquations(Tuple<Matrix, Vector> system)
            {
                return new SystemLinearAlgebraicEquations(system.Item1, system.Item2);
            }

            public static explicit operator Tuple<Matrix,Vector>(SystemLinearAlgebraicEquations system)
            {
                return new Tuple<Matrix, Vector>(system._systemMatrix, system._rightPart);
            }

            public static bool Equals(SystemLinearAlgebraicEquations a, SystemLinearAlgebraicEquations b)
            {
                return a.Equals(b);
            }
            #endregion
        }
    }
}
