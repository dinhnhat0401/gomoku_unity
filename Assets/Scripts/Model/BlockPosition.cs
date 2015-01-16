using UnityEngine;
using System.Collections;

public struct Point
{
	public int x, y;
	
	public Point(int px, int py)
	{
		x = px;
		y = py;
	}
}

public class BlockPosition : MonoBehaviour 
{
	public Point position;
}