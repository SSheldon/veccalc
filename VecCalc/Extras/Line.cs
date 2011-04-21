using System;

public class Line : Flat
{
    public Line(IVector anchor, IVector direction)
        : base(new SubSpace(new IVector[] { direction }), anchor) { }

    private IVector direction
    {
        get { return subspace.Basis(0); }
    }

    public IVector Point(double t)
    {
        return anchor.Add(direction.Multiply(t));
    }

    public double DistanceTo(IVector point)
    {
        if (point.Count != anchor.Count) throw new ArgumentException();
        return direction.CrossMagnitude(point.Subtract(anchor)) / direction.Length();
    }
}