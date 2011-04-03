using System;

public struct Vector3 : IVector
{
    readonly double x, y, z;

    public Vector3(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3(Vector2 v, double z)
        : this(v.X, v.Y, z) { }

    public double this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return x;
                case 1: return y;
                case 2: return z;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    public int Count
    {
        get { return 3; }
    }

    public double X
    {
        get { return x; }
    }

    public double Y
    {
        get { return y; }
    }

    public double Z
    {
        get { return z; }
    }

    public static Vector3 Cylindrical(double radius, double azimuth, double height)
    {
        return new Vector3(radius * Math.Cos(azimuth), radius * Math.Sin(azimuth), height);
    }

    public static Vector3 Spherical(double radius, double azimuth, double zenith)
    {
        return new Vector3(radius * Math.Sin(zenith) * Math.Cos(azimuth), radius * Math.Sin(zenith) * Math.Sin(azimuth), radius * Math.Cos(zenith));
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
    }

    public static double TripleProduct(Vector3 a, Vector3 b, Vector3 c)
    {
        return a.Dot(Cross(b, c));
    }

    public Vector3 Add(IVector v)
    {
        return new Vector3(x + v[0], y + v[1], z + v[2]);
    }

    public Vector3 Subtract(IVector v)
    {
        return new Vector3(x - v[0], y - v[1], z - v[2]);
    }

    public Vector3 Multiply(double c)
    {
        return new Vector3(x * c, y * c, z * c);
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

    public override string ToString()
    {
        return "<" + x + "," + y + "," + z + ">";
    }
}