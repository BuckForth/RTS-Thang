using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInt2
{
	public int x;
	public int y;
	private Vector2 vec;

	public VectorInt2 ()			
	{
		x = 0;
		y = 0;
		vec = new Vector2 (0f, 0f);
	}

	public VectorInt2 (int xx, int yy)	
	{
		x = xx;
		y = yy;
		vec = new Vector2 ((float)xx, (float)yy);
	}

	public VectorInt2 (Vector2 vector)
	{
		vec = new Vector2 (vector.x, vector.y);
		x = (int)vec.x;
		y = (int)vec.y;
	}

	public static VectorInt2 operator - (VectorInt2 v1, VectorInt2 v2)
	{
		return new VectorInt2 (v1.x - v2.x, v1.y - v2.y);
	}

	public static VectorInt2 operator + (VectorInt2 v1, VectorInt2 v2)
	{
		return new VectorInt2 (v1.x + v2.x, v1.y + v2.y);
	}

	public static VectorInt2 operator * (VectorInt2 v1, VectorInt2 v2)
	{
		return new VectorInt2 (v1.x * v2.x, v1.y * v2.y);
	}

	public static VectorInt2 operator / (VectorInt2 v1, VectorInt2 v2)
	{
		return new VectorInt2 (v1.x / v2.x, v1.y / v2.y);
	}

	public bool Equals (VectorInt2 v1)
	{
		return (v1.x == x && v1.y == y);
	}
}

public class VectorInt3
{
	public int x;
	public int y;
	public int z;
	private Vector3 vec;

	public VectorInt3 ()			
	{
		x = 0;
		y = 0;
		z = 0;
		vec = new Vector3 (0f, 0f);
	}

	public VectorInt3 (int xx, int yy)	
	{
		x = xx;
		y = yy;
		z = 0;
		vec = new Vector3 ((float)xx, (float)yy);
	}

	public VectorInt3 (int xx, int yy, int zz)	
	{
		x = xx;
		y = yy;
		z = zz;
		vec = new Vector3 ((float)xx, (float)yy, (float)zz);
	}

	public VectorInt3 (Vector2 vector)
	{
		vec = new Vector3 (vector.x, vector.y);
		x = (int)vec.x;
		y = (int)vec.y;
		z = 0;
	}

	public VectorInt3 (Vector3 vector)
	{
		vec = new Vector3 (vector.x, vector.y, vector.z);
		x = (int)vec.x;
		y = (int)vec.y;
		z = (int)vec.z;
	}

	public static VectorInt3 operator - (VectorInt3 v1, VectorInt3 v2)
	{
		return new VectorInt3 (v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
	}

	public static VectorInt3 operator + (VectorInt3 v1, VectorInt3 v2)
	{
		return new VectorInt3 (v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
	}

	public static VectorInt3 operator * (VectorInt3 v1, VectorInt3 v2)
	{
		return new VectorInt3 (v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	public static VectorInt3 operator / (VectorInt3 v1, VectorInt3 v2)
	{
		return new VectorInt3 (v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
	}

	public bool Equals (VectorInt3 v1)
	{
		return (v1.x == x && v1.y == y && v1.z == z);
	}
}
