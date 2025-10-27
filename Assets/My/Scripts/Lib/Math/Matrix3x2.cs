// Parts of this code are from https://github.com/sharpdx/SharpDX/blob/master/Source/SharpDX.Mathematics/Matrix3x2.cs


using System;
using System.Globalization;
using System.Runtime.CompilerServices;

using UnityEngine;



namespace Lib.Math {



/// <summary>
/// Matrix 3x2 for projection from 2D to 3D and back.
/// </summary>
public struct Matrix3x2
{
	/// <summary>
	/// Element (1,1)
	/// </summary>
	public float M11;

	/// <summary>
	/// Element (1,2)
	/// </summary>
	public float M12;

	/// <summary>
	/// Element (2,1)
	/// </summary>
	public float M21;

	/// <summary>
	/// Element (2,2)
	/// </summary>
	public float M22;

	/// <summary>
	/// Element (3,1)
	/// </summary>
	public float M31;

	/// <summary>
	/// Element (3,2)
	/// </summary>
	public float M32;


	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="iBasisVector">Basis vector i (X axis) of 2D plane in 3D space.</param>
	/// <param name="jBasisVector">Basis vector j (Y axis) of 2D plane in 3D space.</param>
	public Matrix3x2(Vector3 iBasisVector, Vector3 jBasisVector)
	{
		M11 = iBasisVector.x;
		M21 = iBasisVector.y;
		M31 = iBasisVector.z;

		M12 = jBasisVector.x;
		M22 = jBasisVector.y;
		M32 = jBasisVector.z;
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="M11">The value to assign at row 1 column 1 of the matrix.</param>
	/// <param name="M12">The value to assign at row 1 column 2 of the matrix.</param>
	/// <param name="M21">The value to assign at row 2 column 1 of the matrix.</param>
	/// <param name="M22">The value to assign at row 2 column 2 of the matrix.</param>
	/// <param name="M31">The value to assign at row 3 column 1 of the matrix.</param>
	/// <param name="M32">The value to assign at row 3 column 2 of the matrix.</param>
	public Matrix3x2(float M11, float M12, float M21, float M22, float M31, float M32)
	{
		this.M11 = M11;
		this.M12 = M12;
		this.M21 = M21;
		this.M22 = M22;
		this.M31 = M31;
		this.M32 = M32;
	}


	/// <summary>
	/// Gets or sets the basis vector i (X axis) of 2D plane in 3D space.
	/// </summary>
	public Vector3 IBasisVector {
		get => new(M11, M21, M31);
		set {
			M11 = value.x;
			M21 = value.y;
			M31 = value.z;
		}
	}

	/// <summary>
	/// Gets or sets the basis vector j (Y axis) of 2D plane in 3D space.
	/// </summary>
	public Vector3 JBasisVector {
		get => new(M12, M22, M32);
		set {
			M12 = value.x;
			M22 = value.y;
			M32 = value.z;
		}
	}


	/// <summary>
	/// Projects a 2D point to 3D.
	/// </summary>
	/// <param name="point">The 2D point.</param>
	/// <returns>The projected 3D point.</returns>
	public readonly Vector3 ProjectPoint(Vector2 point)
		=> ProjectPoint(this, point);


	/// <summary>
	/// Projects a 2D point to 3D.
	/// </summary>
	/// <param name="matrix">The projection matrix.</param>
	/// <param name="point">The 2D point.</param>
	/// <returns>The projected 3D point.</returns>
	public static Vector3 ProjectPoint(Matrix3x2 matrix, Vector2 point)
	{
		Vector3 result;
		result.x = (point.x * matrix.M11) + (point.y * matrix.M12);
		result.y = (point.x * matrix.M21) + (point.y * matrix.M22);
		result.z = (point.x * matrix.M31) + (point.y * matrix.M32);
		return result;
	}


	/// <summary>
	/// Projects a 3D point to 2D plane defined by this matrix (two 3D basis vectors).
	/// </summary>
	/// <param name="point">The 3D point.</param>
	/// <returns>The projected 2D point.</returns>
	public readonly Vector2 BackProjectPoint(Vector3 point)
		=> BackProjectPoint(this, point);


	/// <summary>
	/// Projects a 3D point to 2D plane defined by a 3x2 matrix (two 3D basis vectors).
	/// </summary>
	/// <param name="matrix">The projection matrix.</param>
	/// <param name="point">The 3D point.</param>
	/// <returns>The projected 2D point.</returns>
	public static Vector2 BackProjectPoint(Matrix3x2 matrix, Vector3 point)
	{
		Vector2 result;
		result.x = (point.x * matrix.M11) + (point.y * matrix.M21) + (point.z * matrix.M31);
		result.y = (point.x * matrix.M12) + (point.y * matrix.M22) + (point.z * matrix.M32);
		return result;
	}


	/// <summary>
	/// Tests for equality between two objects.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	[MethodImpl((MethodImplOptions) 0x100)] // MethodImplOptions.AggressiveInlining
	public static bool operator ==(Matrix3x2 left, Matrix3x2 right)
	{
		return left.Equals(ref right);
	}

	/// <summary>
	/// Tests for inequality between two objects.
	/// </summary>
	/// <param name="left">The first value to compare.</param>
	/// <param name="right">The second value to compare.</param>
	/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	[MethodImpl((MethodImplOptions) 0x100)] // MethodImplOptions.AggressiveInlining
	public static bool operator !=(Matrix3x2 left, Matrix3x2 right)
	{
		return !left.Equals(ref right);
	}


	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		return string.Format(CultureInfo.CurrentCulture, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
			M11, M12, M21, M22, M31, M32);
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <param name="format">The format.</param>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public string ToString(string? format)
	{
		if (format == null)
			return ToString();

		return string.Format(format, CultureInfo.CurrentCulture,
			"[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
			M11.ToString(format, CultureInfo.CurrentCulture), M12.ToString(format, CultureInfo.CurrentCulture),
			M21.ToString(format, CultureInfo.CurrentCulture), M22.ToString(format, CultureInfo.CurrentCulture),
			M31.ToString(format, CultureInfo.CurrentCulture), M32.ToString(format, CultureInfo.CurrentCulture));
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <param name="formatProvider">The format provider.</param>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public string ToString(IFormatProvider formatProvider)
	{
		return string.Format(formatProvider, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
			M11.ToString(formatProvider), M12.ToString(formatProvider),
			M21.ToString(formatProvider), M22.ToString(formatProvider),
			M31.ToString(formatProvider), M32.ToString(formatProvider));
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <param name="format">The format.</param>
	/// <param name="formatProvider">The format provider.</param>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public string ToString(string? format, IFormatProvider formatProvider)
	{
		if (format == null)
			return ToString(formatProvider);

		return string.Format(format, formatProvider, "[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]",
			M11.ToString(format, formatProvider), M12.ToString(format, formatProvider),
			M21.ToString(format, formatProvider), M22.ToString(format, formatProvider),
			M31.ToString(format, formatProvider), M32.ToString(format, formatProvider));
	}


	/// <summary>
	/// Returns a hash code for this instance.
	/// </summary>
	/// <returns>
	/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
	/// </returns>
	public override int GetHashCode()
	{
		unchecked {
			var hashCode = M11.GetHashCode();
			hashCode = (hashCode * 397) ^ M12.GetHashCode();
			hashCode = (hashCode * 397) ^ M21.GetHashCode();
			hashCode = (hashCode * 397) ^ M22.GetHashCode();
			hashCode = (hashCode * 397) ^ M31.GetHashCode();
			hashCode = (hashCode * 397) ^ M32.GetHashCode();
			return hashCode;
		}
	}

	/// <summary>
	/// Determines whether the specified <see cref="Matrix3x2"/> is equal to this instance.
	/// </summary>
	/// <param name="other">The <see cref="Matrix3x2"/> to compare with this instance.</param>
	/// <returns>
	/// <c>true</c> if the specified <see cref="Matrix3x2"/> is equal to this instance; otherwise, <c>false</c>.
	/// </returns>
	public bool Equals(ref Matrix3x2 other)
	{
		return (Mathf.Approximately(other.M11, M11) &&
		        Mathf.Approximately(other.M12, M12) &&
		        Mathf.Approximately(other.M21, M21) &&
		        Mathf.Approximately(other.M22, M22) &&
		        Mathf.Approximately(other.M31, M31) &&
		        Mathf.Approximately(other.M32, M32));
	}

	/// <summary>
	/// Determines whether the specified <see cref="Matrix3x2"/> is equal to this instance.
	/// </summary>
	/// <param name="other">The <see cref="Matrix3x2"/> to compare with this instance.</param>
	/// <returns>
	/// <c>true</c> if the specified <see cref="Matrix3x2"/> is equal to this instance; otherwise, <c>false</c>.
	/// </returns>
	[MethodImpl((MethodImplOptions) 0x100)] // MethodImplOptions.AggressiveInlining
	public bool Equals(Matrix3x2 other)
	{
		return Equals(ref other);
	}

	/// <summary>
	/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
	/// </summary>
	/// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
	/// <returns>
	/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
	/// </returns>
	public override bool Equals(object? value)
	{
		if (!(value is Matrix3x2))
			return false;

		var strongValue = (Matrix3x2) value;
		return Equals(ref strongValue);
	}
}



}
