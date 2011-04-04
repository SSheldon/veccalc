using System;

public interface IVector : IEquatable<IVector>
{
    double this[int i]
    {
        get;
    }

    int Count
    {
        get;
    }

    IVector Add(IVector v);
    IVector Subtract(IVector v);
    IVector Multiply(double c);
}