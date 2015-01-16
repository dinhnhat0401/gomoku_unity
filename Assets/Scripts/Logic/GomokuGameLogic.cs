using UnityEngine;
using System.Collections;

public class GomokuGameLogic : MonoBehaviour
{

		public GameBoard _gameBoard;
		public GameState gameState;
		private int winPoint = 5;

		//the grid number of cell horizontally
		private int _gridWidth = 15;

		//the grid number of cell vertically
		private int _gridHeight = 15;

		// blank_block prefab 
		public GameObject prefab; 	

		// Use this for initialization
		public GomokuGameLogic ()
		{

		}

		void Start ()
		{
				gameState = GameState.Begin;
				for (int i = 0; i< _gridWidth; i++) {
						for (int j = 0; j < _gridHeight; j++) {
								GameObject go = GameObject.Instantiate (prefab, new Vector3 ((i - 7) * 0.835f, (j - 7) * 0.835f, 0), transform.rotation) as GameObject;
								go.GetComponent<BlockPosition> ().position = new Point (i, j);
								go.tag = "blank_block";
						}
				}

		}

		int countBlockInDirection (Point start, CellState color, int dx, int dy)
		{
				int result = 1;
				Point currentPoint = new Point (start.x + dx, start.y + dy);
				while (!_gameBoard.isOutOfBound(currentPoint)) {
						if (_gameBoard.getBlock (currentPoint) == color) {
								result ++;
						} else {
								break;
						}
						currentPoint = new Point (currentPoint.x + dx, currentPoint.y + dy);
				}
				return result;
		}

		int countBlockAtPosition (Point location, CellState color)
		{
				int result = -1;
				int[] counts = new int[4];
				counts [0] = countBlockInDirection (location, color, -1, 0) + countBlockInDirection (location, color, 1, 0);
				counts [1] = countBlockInDirection (location, color, 0, -1) + countBlockInDirection (location, color, 0, 1);
				counts [2] = countBlockInDirection (location, color, -1, 1) + countBlockInDirection (location, color, 1, -1);
				counts [3] = countBlockInDirection (location, color, -1, -1) + countBlockInDirection (location, color, 1, 1);

				for (int t = 0; t < 4; t++) {
						result = Mathf.Max (result, counts [t]);
				}
				return result;
		}

		public bool HavingVictoryAtPosition (Point location, CellState color)
		{
				if (_gameBoard.getBlock (location) == color && countBlockAtPosition (location, color) >= winPoint) {
						switch (color) {
						case CellState.White:
								{
										Debug.Log ("White win");
										break;
								}
						case CellState.Black:
								{
										Debug.Log ("Black win");
										break;
								}
						default:
								{
										Debug.Log ("UNDEFINED!!!");
										break;
								}
						}
						gameState = GameState.end;
						return true;
				}
				return false;
		}


}
