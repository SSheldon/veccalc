using System;

public class Vector : IVector
{
    double[] components;
    readonly int count;
    public int Count
    {
        get { return count; }
    }

    public double this[int i]
    {
        get { return components[i]; }
    }

    public Vector(double[] components)
    {
        count = components.Length;
        this.components = components;
    }

    #region INSTANCE METHODS
    public Vector Add(IVector v)
    {
        return Add(this, v);
    }

    public Vector Subtract(IVector v)
    {
        return Subtract(this, v);
    }

    public Vector Multiply(double c)
    {
        return Multiply(this, c);
    }

    IVector IVector.Add(IVector v)
    {
        return Add(v);
    }

    IVector IVector.Subtract(IVector v)
    {
        return Subtract(v);
    }

    IVector IVector.Multiply(double c)
    {
        return Multiply(c);
    }
    #endregion

    #region STATIC METHODS
    public static Vector Add(IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        double[] components = new double[a.Count];
        for (int i = 0; i < components.Length; i++)
        {
            components[i] = a[i] + b[i];
        }
        return new Vector(components);
    }

    public static Vector Subtract(IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        double[] components = new double[a.Count];
        for (int i = 0; i < components.Length; i++)
        {
            components[i] = a[i] - b[i];
        }
        return new Vector(components);
    }

    public static Vector Multiply(IVector v, double c)
    {
        double[] components = new double[v.Count];
        for (int i = 0; i < components.Length; i++)
        {
            components[i] = v[i] * c;
        }
        return new Vector(components);

    }
    #endregion

    #region OPERATORS
    public static Vector operator +(Vector a, Vector b)
    {
        return a.Add(b);
    }

    public static Vector operator -(Vector a, Vector b)
    {
        return a.Subtract(b);
    }
    #endregion
}