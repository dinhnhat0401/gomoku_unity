using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class GameBoard:MonoBehaviour
{
	private int _gridWidth = 15;
	//the grid number of cell horizontally
	private int _gridHeight = 15;
	//the grid number of cell vertically
	
	public CellState[,] arrayOfBlocks;
	
	void Start () 
	{
		arrayOfBlocks = new CellState[_gridWidth, _gridHeight];

		for (int i = 0; i < _gridWidth; i++) {
			for (int j = 0; j < _gridHeight; j++) {
				arrayOfBlocks[i, j] = CellState.None;
			}
		}
	}

	public IEnumerable<Point> EnumerateCells() 
	{
		for (int y = 0; y < _gridHeight; y ++) 
		{
			for (int x = 0; x < _gridWidth; x ++) 
			{
				yield return new Point(x, y);
			}
		}
	}

	public int getSize () 
	{
		return _gridWidth;
	}

	public void pushBlock (Point p, CellState color) {
		pushBlock(p.x, p.y, color);
	}

	public void pushBlock (int x, int y, CellState color) {
		arrayOfBlocks[x, y] = color;
	}

	public CellState getBlock (Point p) {
		return getBlock (p.x, p.y);
	}

	public CellState getBlock (int x, int y) {
		if (isOutOfBound(x, y)) {
			return CellState.None;
		} else {
			return arrayOfBlocks[x, y];
		}
	}

	public bool isOutOfBound (Point p) 
	{
		return isOutOfBound(p.x, p.y);
	}

	public bool isOutOfBound (int x, int y) {
		return x < 0 || y < 0 || x >= _gridWidth || y >= _gridHeight;
	}
}
