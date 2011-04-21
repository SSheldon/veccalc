using System;

public class Flat
{
    protected SubSpace subspace;
    protected IVector anchor;

    public Flat(SubSpace subspace, IVector anchor)
    {
        if (anchor.Count != subspace.EuclideanSpaceDimension) throw new ArgumentException();
        this.subspace = subspace;
        this.anchor = anchor;
    }

    public bool Contains(IVector v)
    {
        return subspace.Contains(v.Subtract(anchor));
    }
}