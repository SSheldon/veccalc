using System;

public class Matrix
{
    IVector[] rows;

    public Matrix(IVector[] rows)
    {
        for (int i = 1; i < rows.Length; i++)
            if (rows[i].Count != rows[0].Count)
                throw new ArgumentException();
        this.rows = rows; //do a deep copy, lazy ass
    }

    public int Height
    {
        get { return rows.Length; }
    }

    public int Width
    {
        get { return rows[0].Count; }
    }

    public IVector Row(int row)
    {
        return rows[row];
    }

    IVector Col(int col)
    {
        double[] temp = new double[Height];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = rows[i][col];
        return new Vector(temp);
    }

    Matrix Multiply(Matrix other)
    {
        IVector[] tempCols = new IVector[Width];
        for (int c = 0; c < other.Width; c++)
        {
            IVector col = other.Col(c);
            double[] tempVals = new double[this.Height];
            for (int r = 0; r < this.Height; r++)
            {
                tempVals[r] = Row(r).Dot(col);
            }
            tempCols[c] = new Vector(tempVals);
        }
        return new Matrix(tempCols).Transpose();
    }

    IVector Multiply(IVector v)
    {
        double[] temp = new double[Height];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = v.Dot(this.Row(i));
        return new Vector(temp);
    }

    Matrix Transpose()
    {
        IVector[] temp = new IVector[Width];
        for (int i = 0; i < temp.Length; i++)
            temp[i] = Col(i);
        return new Matrix(temp);
    }

    bool IsRowEchelon()
    {
        return true;
    }

    bool IsReducedRowEchelon()
    {
        return true;
    }
}

public class AugmmentedMatrix
{
    IVector[] rows;

    public AugmmentedMatrix(Matrix a, Matrix b)
    {
    }

    public void GaussianEliminate()
    {
        for (int i = 0, j = 0; i < rows.Length && j < rows[i].Count; j++, i++)
        {
            //Find pivot in column j, starting in row i:
            int maxi = i;
            for (int k = i + 1; k < rows.Length; k++)
                if (Math.Abs(rows[k][j]) > Math.Abs(rows[maxi][j]))
                    maxi = k;
            if (rows[maxi][j] != 0.0)
            {
                IVector temp = rows[i];
                rows[i] = rows[maxi];
                rows[maxi] = temp;
                //Now A[i,j] will contain the old value of A[maxi,j]
                rows[i].Multiply(1.0 / rows[i][j]);
                for (int u = i + 1; u < rows.Length; u++)
                    rows[u] = rows[u].Subtract(rows[i].Multiply(rows[u][j]));
                //Now A[u,j] will be 0, since A[u,j] - A[i,j] * A[u,j] = A[u,j] - 1 * A[u,j] = 0.
            }
        }
    }

    public void GaussJordanEliminate()
    {
        GaussianEliminate();
        //other stuff
    }
}