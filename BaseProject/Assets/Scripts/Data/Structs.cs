using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//struct for a vector 2 of ints
[Serializable]
public struct Vector2I 
{
	public int x;
	public int y;

	public Vector2I(int _x, int _y)
	{
		x = _x;
		y = _y;
	}

	public Vector2I(float _x, float _y)
	{
		x = (int)_x;
		y = (int)_y;
	}

	public Vector2I(Vector2I vec2i)
	{
		x = vec2i.x;
		y = vec2i.y;
	}

	public void Set(int _x, int _y)
	{
		x = _x;
		y = _y;
	}

	public static Vector2I Zero()
	{
		return new Vector2I (0, 0);
	}

	public static Vector2I absMinus (Vector2I a, Vector2I b)
	{
		return (new Vector2I (Mathf.Abs (a.x - b.x), Mathf.Abs (a.y - b.y)));
	}

	public static Vector2I absPlus (Vector2I a, Vector2I b)
	{
		return (new Vector2I (Mathf.Abs (a.x + b.x), Mathf.Abs (a.y + b.y)));
	}

	public static Vector2I operator - (Vector2I a, Vector2I b)
	{
		return (new Vector2I(a.x - b.x, a.y - b.y));
	}

	public static Vector2I operator + (Vector2I a, Vector2I b)
	{
		return (new Vector2I(a.x + b.x, a.y + b.y));
	}

	public static bool operator == (Vector2I a, Vector2I b)
	{
		return (a.x == b.x && a.y == b.y);
	}

	public static bool operator != (Vector2I a, Vector2I b)
	{
		return (!(a == b));
	}
}