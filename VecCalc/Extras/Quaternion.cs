using System;

public struct Quaternion
{
    private readonly double a;
    private readonly Vector3 v;

    /// <summary>
    /// Constructs a Quaternion.
    /// </summary>
    /// <param name="real">The real component.</param>
    /// <param name="imaginary">The imaginary components.</param>
    public Quaternion(double real, Vector3 imaginary)
    {
        a = real;
        v = imaginary;
    }

    /// <summary>
    /// Returns the conjugate of this Quaternion.
    /// </summary>
    public Quaternion Conjugate()
    {
        return new Quaternion(a, v.Multiply(-1.0));
    }

    /// <summary>
    /// Adds two Quaternions.
    /// </summary>
    /// <param name="q1">The first Quaternion.</param>
    /// <param name="q2">The second Quaternion.</param>
    /// <returns>The sum of the Quaternions.</returns>
    public static Quaternion Add(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.a + q2.a, q1.v.Add(q2.v));
    }

    /// <summary>
    /// Multiplies two Quaternions.
    /// </summary>
    /// <param name="q1">The first Quaternion.</param>
    /// <param name="q2">The second Quaternion.</param>
    /// <returns>The product of the Quaternions.</returns>
    public static Quaternion Multiply(Quaternion q1, Quaternion q2)
    {
        double real = q1.a * q2.a - q1.v.Dot(q2.v);
        Vector3 imaginary = q2.v.Multiply(q1.a);
        imaginary = imaginary.Add(q1.v.Multiply(q2.a));
        imaginary = imaginary.Add(Vector3.Cross(q1.v, q2.v));
        return new Quaternion(real, imaginary);
    }

    /// <summary>
    /// Returns a Quaternion representing a rotation.
    /// </summary>
    /// <param name="axis">The axis to rotate around.</param>
    /// <param name="angle">The angle to rotate by.</param>
    /// <returns>A Quaternion representing the rotation.</returns>
    public static Quaternion Rotation(Vector3 axis, double angle)
    {
        double real = Math.Cos(angle / 2.0);
        Vector3 imaginary;
        //normalize first
        imaginary = axis.Multiply(1.0 / axis.Length());
        imaginary = imaginary.Multiply(Math.Sin(angle / 2.0));
        return new Quaternion(real, imaginary);
    }

    /// <summary>
    /// Performs a rotation specified by a Quaternion on a Vector3.
    /// </summary>
    /// <param name="v">The Vector3 to rotate.</param>
    /// <param name="q">The Quaternion representation of a rotation.</param>
    /// <returns>The rotated Vector3.</returns>
    public static Vector3 Rotate(Vector3 v, Quaternion q)
    {
        Quaternion result;
        result = Quaternion.Multiply(q, new Quaternion(0.0, v));
        result = Quaternion.Multiply(result, q.Conjugate());
        return result.v;
    }
}