using UnityEngine;
using System.Collections;

public class GomokuGameLogic : MonoBehaviour
{
	public bool fightWithAI;
	public Sprite white_block;
	public Sprite black_block;	
	public Sprite blank_block;

	public GameBoard _gameBoard;
	public GameState gameState;

	//the grid number of cell horizontally
	private int _gridWidth = 15;

	//the grid number of cell vertically
	private int _gridHeight = 15;
	private GomokuAI gomokuAI;
	private Stack moveObjectStack;
	private Stack movePositionStack;

	// blank_block prefab 
	public GameObject prefab;
	public GameObject gomokuAIPrefab;
	public GameObject[,] arrayObjects;
	public GameObject recentMoveWhite;
	public GameObject recentMoveBlack;

	public GameObject victoryPopup;
	public GameObject defeatedPopup;

		// Use this for initialization
		public GomokuGameLogic ()
		{

		}

		void Start ()
		{
			fightWithAI = GameConstants.aiMode;
				arrayObjects = new GameObject[_gridWidth, _gridHeight];
				gameState = GameState.Begin;
				for (int i = 0; i< _gridWidth; i++) {
						for (int j = 0; j < _gridHeight; j++) {
								GameObject go = GameObject.Instantiate (prefab, new Vector3 ((i - 7) * 0.835f, (j - 7) * 0.835f, 0), transform.rotation) as GameObject;
								go.GetComponent<BlockPosition> ().position = new Point (i, j);
								go.tag = "blank_block";
								arrayObjects [i, j] = go;
						}
				}

			GameObject gomokuAIGO = GameObject.Instantiate (gomokuAIPrefab, new Vector3 (0, 0, 0), transform.rotation) as GameObject;
			gomokuAI = gomokuAIGO.GetComponent<GomokuAI>();
			moveObjectStack = new Stack();
			movePositionStack = new Stack();
		}

	public void changeBlockSpriteAtLocation (GameObject clickedGO, Point pos)
	{
		switch (gameState) 
		{  
		case GameState.Begin:
		case GameState.Black_move:
			moveObjectStack.Push(clickedGO);
			movePositionStack.Push(pos);
			clickedGO.tag = "black_block";	
			clickedGO.GetComponent<SpriteRenderer> ().sprite = black_block;
			_gameBoard.pushBlock (pos, CellState.Black);
			recentMoveBlack.transform.position = clickedGO.transform.position;
					
			if (HavingVictoryAtPosition (pos, CellState.Black)) {
				gameState = GameState.End;
			} else {
				gameState = GameState.White_move;
			}	
			break;
				
		case GameState.White_move: 	
			moveObjectStack.Push(clickedGO);
			movePositionStack.Push(pos);
			clickedGO.tag = "white_block";
			clickedGO.GetComponent<SpriteRenderer> ().sprite = white_block;
			_gameBoard.pushBlock (pos, CellState.White);
			recentMoveWhite.transform.position = clickedGO.transform.position;
			
			if (HavingVictoryAtPosition (pos, CellState.White)) {
				gameState = GameState.End;
			} else {
				gameState = GameState.Black_move;
			}	
			break;
				
		case GameState.Restart:
			clickedGO.tag = "blank_block";
			clickedGO.GetComponent<SpriteRenderer> ().sprite = blank_block;
			_gameBoard.pushBlock (pos, CellState.None);
			break;

		case GameState.End:
		default: 
			break;
		}
	}

		public CellState getOpponentColor (CellState currentColor)
		{
				switch (currentColor) {
				case CellState.Black:
						return CellState.White;
				case CellState.White:
						return CellState.Black;
				default:
						return CellState.None;
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

	public int countBlockAtPosition (Point location, CellState color)
	{
		int result = -1;
		int[] counts = new int[4];
		counts [0] = countBlockInDirection (location, color, -1, 0) + countBlockInDirection (location, color, 1, 0) - 1;
		counts [1] = countBlockInDirection (location, color, 0, -1) + countBlockInDirection (location, color, 0, 1) - 1;
		counts [2] = countBlockInDirection (location, color, -1, 1) + countBlockInDirection (location, color, 1, -1) - 1;
		counts [3] = countBlockInDirection (location, color, -1, -1) + countBlockInDirection (location, color, 1, 1) - 1;
		for (int t = 0; t < 4; t++) {
			result = Mathf.Max (result, counts [t]);
		}
		return result;
	}

	public bool HavingVictoryAtPosition (Point location, CellState color)
	{
		int result = countBlockAtPosition (location, color);
		if ((_gameBoard.getBlock (location) == color) && (result >= GameConstants.GAME_WIN_POINT)) {
			if (color == GameConstants.userBlockColor) {
				StartCoroutine("playVictoryAnimation");
			} else {
				StartCoroutine("playDefeatedAnimation");
			}
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
			gameState = GameState.End;
			return true;
		}
		return false;
	}

	public void NewGame () 
	{
		recentMoveWhite.transform.position = new Vector3(int.MaxValue, 0, 0);
		recentMoveBlack.transform.position = new Vector3(int.MaxValue, 0, 0);

		moveObjectStack.Clear();
		movePositionStack.Clear();

		gameState = GameState.Restart;
		for (int i = 0; i< _gridWidth; i++) {
			for (int j = 0; j < _gridHeight; j++) {
				changeBlockSpriteAtLocation(arrayObjects[i, j], new Point(i, j));
			}
		}

		gameState = GameState.Begin;
		if (fightWithAI) 
		{
			// p v c
			gomokuAI.RestartGame();
		} else {
			// p v p
		}
	}

	public IEnumerator playVictoryAnimation() {
		victoryPopup.SetActive(true);
		yield return new WaitForSeconds(3.5f);
		victoryPopup.SetActive(false);
	}

	public IEnumerator playDefeatedAnimation() {
		defeatedPopup.SetActive(true);
		yield return new WaitForSeconds(3.5f);
		defeatedPopup.SetActive(false);
	}

	public void undoMove() {
		if (GameConstants.userBlockColor == CellState.Black) {
			if (gameState == GameState.Black_move) {
				if (moveObjectStack.Count >= 4) {
					GameObject pop1 = moveObjectStack.Pop() as GameObject;
					Point pos1 = (Point)movePositionStack.Pop();
					pop1.tag = "blank_block";	
					pop1.GetComponent<SpriteRenderer> ().sprite = blank_block;
					_gameBoard.removeBlock (pos1);
					
					GameObject pop2 = moveObjectStack.Pop() as GameObject;
					Point pos2 = (Point)movePositionStack.Pop();
					pop2.tag = "blank_block";	
					pop2.GetComponent<SpriteRenderer> ().sprite = blank_block;
					_gameBoard.removeBlock (pos2);

					GameObject pop3 = moveObjectStack.Pop() as GameObject;
					recentMoveWhite.transform.position = pop3.transform.position;

					GameObject pop4 = moveObjectStack.Peek() as GameObject;
					recentMoveBlack.transform.position = pop4.transform.position;

					moveObjectStack.Push(pop3);
				}
			}
		} else {
			if (gameState == GameState.White_move) {
				if (moveObjectStack.Count >= 4) {
					GameObject pop1 = moveObjectStack.Pop() as GameObject;
					Point pos1 = (Point)movePositionStack.Pop();
					pop1.tag = "blank_block";	
					pop1.GetComponent<SpriteRenderer> ().sprite = blank_block;
					_gameBoard.removeBlock (pos1);
					
					GameObject pop2 = moveObjectStack.Pop() as GameObject;
					Point pos2 = (Point)movePositionStack.Pop();
					pop2.tag = "blank_block";	
					pop2.GetComponent<SpriteRenderer> ().sprite = blank_block;
					_gameBoard.removeBlock (pos2);
					
					GameObject pop3 = moveObjectStack.Pop() as GameObject;
					recentMoveWhite.transform.position = pop3.transform.position;
					
					GameObject pop4 = moveObjectStack.Peek() as GameObject;
					recentMoveBlack.transform.position = pop4.transform.position;

					moveObjectStack.Push(pop3);
				}
			}
		}
	}
}
