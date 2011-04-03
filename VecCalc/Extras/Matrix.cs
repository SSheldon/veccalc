using System;

public class Matrix
{
    private IVector[] rows;

    public Matrix(params IVector[] rows)
    {
        for (int i = 1; i < rows.Length; i++)
            if (rows[i].Count != rows[0].Count)
                throw new ArgumentException();
        this.rows = new IVector[rows.Length];
        for (int i = 0; i < Height; i++)
            this.rows[i] = rows[i]; //it's okay, vectors are immutable
    }

    #region Accessors
    public int Height
    {
        get { return rows.Length; }
    }

    public int Width
    {
        get { return rows[0].Count; }
    }

    public IVector this[int row]
    {
        get { return rows[row]; }
    }

    public IVector Col(int col)
    {
        double[] temp = new double[Height];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = rows[i][col];
        return new Vector(temp);
    }

    IVector[] Cols()
    {
        IVector[] cols = new IVector[Width];
        for (int c = 0; c < Width; c++)
            cols[c] = Col(c);
        return cols;
    }
    #endregion

    #region Methods - Matrix operations
    public Matrix Transpose()
    {
        return new Matrix(Cols());
    }

    public Matrix MultiplyBy(Matrix other)
    {
        return other.MultiplyTo(this);
    }

    public Matrix MultiplyTo(Matrix other)
    {
        IVector[] tempCols = new IVector[Width];
        for (int c = 0; c < other.Width; c++)
        {
            IVector col = other.Col(c);
            double[] tempVals = new double[this.Height];
            for (int r = 0; r < this.Height; r++)
            {
                tempVals[r] = this[r].Dot(col);
            }
            tempCols[c] = new Vector(tempVals);
        }
        return new Matrix(tempCols).Transpose();
    }

    public IVector MultiplyTo(IVector v)
    {
        if (v.Count != this.Width) throw new ArgumentException();
        double[] temp = new double[Height];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = this[i].Dot(v);
        return new Vector(temp);
    }
    #endregion

    #region Mutators
    public void Swap(int i, int j)
    {
        IVector temp = rows[i];
        rows[i] = rows[j];
        rows[j] = temp;
    }

    public void MultiplyRow(int i, double c)
    {
        rows[i] = rows[i].Multiply(c);
    }

    public void AddMultiple(int i, int other, double c)
    {
        rows[i] = rows[i].Add(rows[other].Multiply(c));
    }

    public void SubtractMultiple(int i, int other, double c)
    {
        rows[i] = rows[i].Subtract(rows[other].Multiply(c));
    }

    public void GaussianEliminate()
    {
        for (int i = 0, j = 0; i < Height && j < Width; j++, i++)
        {
            //Find pivot in column j, starting in row i:
            int maxi = i;
            for (int k = i + 1; k < Height; k++)
                if (Math.Abs(rows[k][j]) > Math.Abs(rows[maxi][j]))
                    maxi = k;
            if (rows[maxi][j] != 0.0)
            {
                Swap(i, maxi);
                //Now A[i,j] will contain the old value of A[maxi,j]
                MultiplyRow(i, 1.0 / this[i][j]);
                for (int u = i + 1; u < rows.Length; u++)
                    SubtractMultiple(u, i, this[u][j]);
                //Now A[u,j] will be 0, since A[u,j] - A[i,j] * A[u,j] = A[u,j] - 1 * A[u,j] = 0.
            }
        }
    }

    public void GaussJordanEliminate()
    {
        GaussianEliminate();
        //other stuff
        for (int i = Height - 1; i >= 0; i--)
        {
            int leading1 = 0;
            for (int j = 0; j < Width; j++)
            {
                if (this[i][j] != 0)
                {
                    leading1 = j;
                    break;
                }
            }

            for (int i2 = i - 1; i2 >= 0; i2--)
            {
                SubtractMultiple(i2, i, this[i2][leading1]);
            }
        }
    }
    #endregion

    #region Methods - Matrix properties
    public bool IsRowEchelon()
    {
        int leading = -1;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (this[i][j] != 0)
                {
                    if (j <= leading) return false;
                    else
                    {
                        leading = j;
                        break;
                    }
                }
            }
        }
        return true;
    }

    public bool IsReducedRowEchelon()
    {
        int leading = -1;
        for (int i = 0; i < Height; i++)
        {
            bool foundLeading1 = false;
            for (int j = 0; j < Width; j++)
            {
                if (this[i][j] != 0)
                {
                    if (this[i][j] != 1) return false;
                    else
                    {
                        //found a 1! burn her!
                        if (foundLeading1 || j <= leading) return false;
                        else
                        {
                            leading = j;
                            foundLeading1 = true;
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool LinearlyIndependent()
    {
        //reduce and see if there are zero rows
        Matrix temp = new Matrix(this.rows);
        temp.GaussJordanEliminate();
        return !temp[temp.Height - 1].IsZero();
    }
    #endregion

    //TODO: make class to represent bases
    //get Kernel
    //get Image
    //get rank

    public override string ToString()
    {
        string temp = "";
        if (Height > 0) temp = this[0].ToString();
        for (int i = 1; i < Height; i++)
            temp += "\n" + this[i].ToString();
        return temp;
    }
}

public class AugmmentedMatrix : Matrix
{
    int w1;

    public AugmmentedMatrix(Matrix a, Matrix b)
        : base(Combine(a, b))
    {
        w1 = a.Width;
    }

    static IVector[] Combine(Matrix a, Matrix b)
    {
        int w1 = a.Width;
        int w2 = b.Width;
        IVector[] temp = new IVector[a.Height];
        for (int i = 0; i < temp.Length; i++)
        {
            double[] vec = new double[w1 + w2];
            for (int j = 0; j < vec.Length; j++)
                vec[j] = (j < w1 ? a[i][j] : b[i][j - w1]);
            temp[i] = new Vector(vec);
        }
        return temp;
    }

    Matrix MatrixA()
    {
        IVector[] temp = new IVector[Height];
        for (int i = 0; i < Height; i++)
        {
            double[] vec = new double[w1];
            for (int j = 0; j < w1; j++)
                vec[j] = this[i][j];
            temp[i] = new Vector(vec);
        }
        return new Matrix(temp);
    }

    Matrix MatrixB()
    {
        int w2 = Width - w1;
        IVector[] temp = new IVector[Height];
        for (int i = 0; i < Height; i++)
        {
            double[] vec = new double[w2];
            for (int j = 0; j < w2; j++)
                vec[j] = this[i][w1 + j];
            temp[i] = new Vector(vec);
        }
        return new Matrix(temp);
    }
}

public class LinearSystem
{
    Matrix augmat;

    public LinearSystem(Matrix a, IVector b)
    {
        int w1 = a.Width;
        IVector[] temp = new IVector[a.Height];
        for (int i = 0; i < temp.Length; i++)
        {
            double[] vec = new double[w1 + 1];
            for (int j = 0; j < w1; j++)
                vec[j] = a[i][j];
            vec[w1] = b[i];
            temp[i] = new Vector(vec);
        }

        augmat = new Matrix(temp);
        augmat.GaussJordanEliminate();
    }

    public bool IsInconsistent()
    {
        for (int i = augmat.Height - 1; i >= 0; i--)
        {
            //find last nonzero coefficient
            int lastNonZero = -1;
            for (int j = augmat.Width - 2; j >= 0; j--)
                if (augmat[i][j] != 0) lastNonZero = j;

            if (lastNonZero == -1)
            {
                //preceding row was all 0s, so if last entry isn't 0, inconsistent
                if (augmat[i][augmat.Width - 1] != 0) return true;
            }
            else if (lastNonZero >= 0) break; //once a row has more than 0s, all above will too
        }
        return false;
    }

    public bool HasFreeVariables()
    {
        //must be if there are less equations than variables
        if (augmat.Height < augmat.Width - 1) return true;
        for (int i = 0; i < augmat.Width - 1; i++)
            if (augmat[i][i] != 1) return false;
        return true;
    }

    public IVector Solve()
    {
        if (IsInconsistent() || HasFreeVariables()) return null;
        else return augmat.Col(augmat.Width - 1);
    }

    public IVector LeastSquaresSolve()
    {
        IVector[] cols = new IVector[augmat.Width - 1];
        for (int i = 0; i < cols.Length; i++)
            cols[i] = augmat.Col(i);
        Matrix temp = augmat.MultiplyBy(new Matrix(cols));
        cols = null;
        temp.GaussJordanEliminate();
        return temp.Col(temp.Width - 1);
    }

    public static IVector Solve(Matrix a, IVector b)
    {
        return new LinearSystem(a, b).Solve();
    }
}