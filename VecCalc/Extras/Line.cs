using System;

public class Line
{
    IVector anchor, direction;

    public Line(IVector anchor, IVector direction)
    {
        if (anchor.Count != direction.Count) throw new ArgumentException();
        this.anchor = anchor;
        this.direction = direction;
    }

    //public bool Contains(IVector v)
    //{
    //    return direction.ParallelTo(v.Subtract(anchor));
    //}

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