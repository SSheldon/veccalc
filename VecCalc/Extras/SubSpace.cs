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

    void Orthogonalize()
    {
        for (int i = 1; i < bases.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                bases[i] = bases[i].OrthogonalComplementTo(bases[j]);
            }
        }
    }

    void Normalize()
    {
        for (int i = 0; i < bases.Length; i++)
            bases[i] = bases[i].Normalize();
    }

    void OrthoNormalize()
    {
        Orthogonalize();
        Normalize();
    }

    bool Contains(IVector v)
    {
        //true if orthogonal component = 0
        //also true if the bases + v aren't linearly independent
        return !v.LinearlyIndependentOf(bases);
    }
}