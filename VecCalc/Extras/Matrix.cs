using System;

public class Matrix
{
    private IVector[] rows;

    #region Constructors
    public Matrix(params IVector[] rows)
        : this(true, rows) { }

    public Matrix(Matrix other)
        : this(false, other.rows) { }

    protected Matrix(bool check, IVector[] rows)
    {
        if (check)
        {
            for (int i = 1; i < rows.Length; i++)
                if (rows[i].Count != rows[0].Count)
                    throw new ArgumentException();
        }

        this.rows = new IVector[rows.Length];
        //it's okay to just copy, vectors are immutable
        Array.Copy(rows, this.rows, Height);
    }

    protected static Matrix FromRows(IVector[] rows)
    {
        //all row vectors are the same length, right?
        return new Matrix(false, rows);
    }

    protected static Matrix FromCols(IVector[] cols)
    {
        //all col vectors are same length, right?
        IVector[] rows = new IVector[cols[0].Count];
        for (int i = 0; i < rows.Length; i++)
        {
            double[] temp = new double[cols.Length];
            for (int j = 0; j < cols.Length; j++)
            {
                temp[j] = cols[j][i];
            }
            rows[i] = new Vector(temp);
        }
        return FromRows(rows);
    }
    #endregion

    #region Accessors
    public int Height
    {
        get { return rows.Length; }
    }

    public int Width
    {
        get { return (Height > 0 ? rows[0].Count : 0); }
    }

    public IVector this[int row]
    {
        get { return rows[row]; }
    }

    public double this[int row, int col]
    {
        get { return this[row][col]; }
    }

    public IVector Col(int col)
    {
        double[] temp = new double[Height];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = this[i][col];
        return new Vector(temp);
    }
    #endregion

    #region Methods - Matrix operations
    public Matrix Transpose()
    {
        return Matrix.FromCols(rows);
    }

    public Matrix MultiplyTo(Matrix other)
    {
        if (other.Height != this.Width) throw new ArgumentException();
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
        return Matrix.FromCols(tempCols);
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

    //TODO: make class to represent bases
    public SubSpace Kernel()
    {
        if (!IsReducedRowEchelon())
        {
            Matrix temp = new Matrix(this);
            temp.GaussJordanEliminate();
            return temp.Kernel();
        }
        //number of free vars = width - number of nonzero rows
        //solution vector has width rows
        //if column c of the matrix has no leading 1s, v[c] has free variables
        //else v[c] is a bound variable
        //basis will have 1 vector for each free variable
        //for vector from free variable a:
        //    component a is a 1
        //    components corresponding to free variables are 0
        //    components corresponding to bound variables:
        //        iterate through rows, row[a] is next open component

        bool[] hasLeading1 = new bool[Width];
        int boundVars = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (this[i][j] != 0)
                {
                    hasLeading1[j] = true;
                    boundVars++;
                    break;
                }
            }
        }

        IVector[] basis = new IVector[Width - boundVars];
        int colOfCurrentFree = 0;
        for (int i = 0; i < basis.Length; i++)
        {
            while (hasLeading1[colOfCurrentFree]) colOfCurrentFree++;
            double[] temp = new double[Width];
            int boundsSoFar = 0;
            for (int j = 0; j < temp.Length; j++)
            {
                if (j == colOfCurrentFree)
                    temp[j] = 1; //j == the col this free var comes from
                else if (!hasLeading1[j])
                    temp[j] = 0; //other free vars are being accounted for elsewhere
                else
                {
                    //we hafta get it from augmat
                    temp[j] = -this[boundsSoFar][colOfCurrentFree];
                    boundsSoFar++;
                }
            }
            basis[i] = new Vector(temp);
            colOfCurrentFree++;
        }

        return new SubSpace(basis);
    }

    public SubSpace Image()
    {
        Matrix working = this.Transpose();
        working.GaussJordanEliminate();

        int dim = 0;
        for (int i = 0; i < working.Height; i++)
        {
            if (!working[i].IsZero()) dim = i + 1;
        }
        IVector[] temp = new IVector[dim];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = working[i];
        }

        return new SubSpace(temp);
    }

    public int Rank()
    {
        if (IsReducedRowEchelon())
        {
            int nonZeroRows = 0;
            for (int i = 0; i < Height; i++)
            {
                if (!this[i].IsZero()) nonZeroRows++;
                else break;
            }
            //should be the same as Image().Dimension and Kernel().Dimension - math!
            return nonZeroRows;
        }
        else
        {
            Matrix temp = new Matrix(this);
            temp.GaussJordanEliminate();
            return temp.Rank();
        }
    }
    #endregion

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
        return FromRows(temp);
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
        return FromRows(temp);
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
        //so we know height >= width - 1
        for (int i = 0; i < augmat.Width - 1; i++)
            if (augmat[i][i] != 1) return true;
        return false;
    }

    public Flat SolveBasis()
    {
        if (IsInconsistent() || !HasFreeVariables()) return null;

        IVector[] temp = new IVector[augmat.Height];
        for (int i = 0; i < augmat.Height; i++)
        {
            double[] vec = new double[augmat.Width - 1];
            for (int j = 0; j < augmat.Width - 1; j++)
                vec[j] = augmat[i][j];
            temp[i] = new Vector(vec);
        }
        Matrix working = new Matrix(temp);

        return new Flat(working.Kernel(), augmat.Col(augmat.Width - 1));
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
        Matrix temp = (new Matrix(cols)).MultiplyTo(augmat);
        cols = null;
        temp.GaussJordanEliminate();
        return temp.Col(temp.Width - 1);
    }

    public static IVector Solve(Matrix a, IVector b)
    {
        return new LinearSystem(a, b).Solve();
    }
}