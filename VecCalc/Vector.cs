using System;

public class Vector : IVector
{
    readonly double[] components;

    public int Count
    {
        get { return components.Length; }
    }

    public double this[int i]
    {
        get { return components[i]; }
    }

    public Vector(double[] components)
    {
        this.components = new double[components.Length];
        for (int i = 0; i < Count; i++)
            this.components[i] = components[i];
    }

    #region Instance methods
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

    #region Static methods
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

    public static Vector Zero(int count)
    {
        return new Vector(new double[count]);
    }
    #endregion

    #region Operators
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