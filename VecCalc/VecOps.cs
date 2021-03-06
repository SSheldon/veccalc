﻿using System;

public static class VecOps
{
    public static double Dot(this IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        double sum = 0;
        for (int i = 0; i < a.Count; i++)
        {
            sum += a[i] * b[i];
        }
        return sum;
    }

    public static double LengthSquared(this IVector v)
    {
        return v.Dot(v);
    }

    public static double Length(this IVector v)
    {
        return Math.Sqrt(v.LengthSquared());
    }

    public static double AngleBetween(this IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        return Math.Acos(a.Dot(b) / (a.Length() * b.Length()));
    }

    public static double DistanceSquared(this IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        double sum = 0;
        for (int i = 0; i < a.Count; i++)
        {
            sum += (a[i] - b[i]) * (a[i] - b[i]);
        }
        return sum;
        //return a.Subtract(b).LengthSquared();
    }

    public static double Distance(this IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        return Math.Sqrt(a.DistanceSquared(b));
    }

    public static double CrossMagnitudeSquared(this IVector a, IVector b)
    {
        if (a.Count != b.Count) throw new ArgumentException();
        return a.LengthSquared() * b.LengthSquared() - Dot(a, b) * Dot(a, b);
    }

    public static double CrossMagnitude(this IVector a, IVector b)
    {
        return Math.Sqrt(CrossMagnitudeSquared(a, b));
    }

    public static IVector Reflect(this IVector vector, IVector normal)
    {
        if (vector.Count != normal.Count) throw new ArgumentException();
        double dot = vector.Dot(normal);
        return vector.Subtract(normal.Multiply(2D * dot));
    }

    public static IVector Negate(this IVector v)
    {
        return v.Multiply(-1D);
    }

    public static IVector Normalize(this IVector v)
    {
        return v.Multiply(1D / v.Length());
    }

    public static IVector ProjectionOnto(this IVector a, IVector b)
    {
        return b.Multiply(a.Dot(b) / b.LengthSquared());
    }

    public static IVector OrthogonalComplementTo(this IVector a, IVector b)
    {
        return a.Subtract(a.ProjectionOnto(b));
    }

    public static bool IsZero(this IVector v)
    {
        for (int i = 0; i < v.Count; i++)
            if (v[i] != 0) return false;
        return true;
    }

    public static bool LinearlyIndependentOf(this IVector v, params IVector[] vecs)
    {
        IVector[] temp = new IVector[vecs.Length + 1];
        Array.Copy(vecs, temp, vecs.Length);
        temp[vecs.Length] = v;
        return VecOps.LinearlyIndependent(temp);
    }

    public static bool LinearlyIndependent(params IVector[] vecs)
    {
        //reduce and see if there are zero rows
        Matrix temp = new Matrix(vecs);
        temp.GaussJordanEliminate();
        return !temp[temp.Height - 1].IsZero();
    }
}