using System;

public class SubSpace
{
    IVector[] bases;

    public SubSpace(IVector[] bases)
    {
        this.bases = bases;
    }

    public int Dimension
    {
        get { return bases.Length; }
    }

    public int EuclideanSpaceDimension
    {
        get { return Basis(0).Count; }
    }

    public IVector Basis(int index)
    {
        return bases[index];
    }
}