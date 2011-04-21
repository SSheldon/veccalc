using System;

public class SquareMatrix : Matrix
{
    #region Constructors
    public SquareMatrix(params IVector[] rows)
        : this(true, rows) { }

    public SquareMatrix(Matrix m)
        : base(m)
    {
        if (m.Height != m.Width)
            throw new ArgumentException();
    }

    private SquareMatrix(bool check, IVector[] rows)
        : base(false, rows)
    {
        if (check)
        {
            for (int i = 0; i < rows.Length; i++)
                if (rows[i].Count != rows.Length)
                    throw new ArgumentException();
        }
    }

    public static SquareMatrix Identity(int dimension)
    {
        IVector[] rows = new IVector[dimension];
        for (int i = 0; i < dimension; i++)
        {
            double[] temp = new double[dimension];
            temp[i] = 1;
            rows[i] = new Vector(temp);
        }
        return new SquareMatrix(false, rows);
    }
    #endregion

    #region Matrix Operations
    public SquareMatrix MultiplyTo(SquareMatrix other)
    {
        return new SquareMatrix(base.MultiplyTo(other));
    }

    public new SquareMatrix Transpose()
    {
        return new SquareMatrix(base.Transpose());
    }

    public SquareMatrix Inverse()
    {
        return null;
    }

    public double Determinant()
    {
        return 0;
    }

    public double Trace()
    {
        return 0;
    }
    #endregion

    #region Matrix Properties
    public bool Orthogonal()
    {
        return false;
    }

    public bool Symmetric()
    {
        for (int i = 0; i < Height; i++)
            if (!this[i].Equals(Col(i))) return false;
        return true;
    }

    public bool Invertible()
    {
        return false;
    }

    public bool Diagonal()
    {
        return false;
    }

    public bool UpperTriangular()
    {
        return false;
    }

    public bool LowerTriangular()
    {
        return false;
    }

    public bool Triangular()
    {
        return UpperTriangular() || LowerTriangular();
    }

    public bool SimilarTo(SquareMatrix other)
    {
        return false;
    }

    public bool Eigen(IVector v, double c)
    {
        return !v.IsZero() && this.MultiplyTo(v).Equals(v.Multiply(c));
    }
    #endregion
}