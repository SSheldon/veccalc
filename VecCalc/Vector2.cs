using System;

public struct Vector2 : IVector
{
    readonly double x, y;

    public Vector2(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public double this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return x;
                case 1: return y;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    public int Count
    {
        get { return 2; }
    }

    public double X
    {
        get { return this[0]; }
    }

    public double Y
    {
        get { return this[1]; }
    }

    public Vector2 Add(IVector v)
    {
        return new Vector2(x + v[0], y + v[1]);
    }

    public Vector2 Subtract(IVector v)
    {
        return new Vector2(x - v[0], y - v[1]);
    }

    public Vector2 Multiply(double c)
    {
        return new Vector2(x * c, y * c);
    }

    public static Vector2 One
    { get { return new Vector2(1, 1); } }

    public static Vector2 UnitX
    { get { return new Vector2(1, 0); } }

    public static Vector2 UnitY
    { get { return new Vector2(0, 1); } }

    public static Vector2 Zero
    { get { return new Vector2(0, 0); } }

    public double GetDirection()
    {
        return Math.Atan2(Y, X);
    }

    public static Vector2 Polar(double length, double direction)
    {
        return new Vector2(length * Math.Cos(direction), length * Math.Sin(direction));
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
        return "<" + x + "," + y + ">";
    }
}