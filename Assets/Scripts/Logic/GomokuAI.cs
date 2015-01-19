using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GomokuAI: MonoBehaviour {


	public Sprite black_block;

	private GomokuGameLogic gameLogic;
	private GameObject[,] arrayObjects;
	private GameObject gameBoardGO;
	private GameBoard gameBoard;

	delegate IComparable EstimateFunction(Point location, CellState color);

	void Start () {
		if (gameBoard == null) 
		{
			gameBoardGO = GameObject.FindGameObjectWithTag("game_board") as GameObject;
			gameBoard = gameBoardGO.GetComponent<GameBoard>();
		}

		if (gameLogic == null) 
		{
			gameLogic = gameBoardGO.GetComponent<GomokuGameLogic> ();
		}

		arrayObjects = gameLogic.arrayObjects;
		GameObject clickedGO = arrayObjects[7, 7];

		Point pos = clickedGO.GetComponent<BlockPosition> ().position;
		gameLogic.changeBlockSpriteAtLocation(clickedGO, pos);
	}
	
	void Update () {

		if(gameLogic.gameState == GameState.Black_move) {
			Point bestPos = SelectBestPosition(CellState.Black);
			gameLogic.changeBlockSpriteAtLocation(arrayObjects[bestPos.x, bestPos.y], bestPos);
		}
	}

	public Point SelectBestPosition(CellState color) 
	{
		var candidates = FilterCellsStageThree(color);
		
		if(candidates.Count == 0)
			return new Point(1,1);
		
		if(candidates.Count == 1)
			return candidates[0];

		for (int i = 0; i< candidates.Count; i++) 
		{
			if(gameLogic.countBlockAtPosition(candidates[i], CellState.Black) >= GameConstants.GAME_WIN_POINT) 
			{
				return candidates[i];
			}
		}

		System.Random random = new System.Random();
		int index = random.Next(0, candidates.Count - 1);
		return candidates[index];
	}

	List<Point>	FilterCellsCore(IEnumerable<Point> source, EstimateFunction estimator, CellState color) {
		var result = new List<Point>();
		IComparable bestEstimate = null;

		foreach(Point location in source) {
			if(gameBoard.getBlock(location) != CellState.None)
				continue;
			var estimate = estimator(location, color);
			
			int compareResult = estimate.CompareTo(bestEstimate);
			if(compareResult < 0)
				continue;
			if(compareResult > 0) {
				result.Clear();
				bestEstimate = estimate;
			}
			result.Add(location);
		}
		
		return result;
	}

	internal IComparable EstimateForStageOne(Point location, CellState color) 
	{
		GomokuGameLogic gameLogic = gameBoard.GetComponent<GomokuGameLogic>();
		int selfScore = 1 + gameLogic.countBlockAtPosition(location, color);
		CellState opponentColor = gameLogic.getOpponentColor(color);
		int opponentScore = 1 + gameLogic.countBlockAtPosition(location, opponentColor);

		if(selfScore > GameConstants.GAME_WIN_POINT)
		{
			selfScore = int.MaxValue;
		}
		return Math.Max(selfScore, opponentScore);
	}

	internal IComparable EstimateForStageOne(int x, int y, CellState color) 
	{
		return EstimateForStageOne(new Point(x, y), color);
	}

	internal IComparable EstimateForStageTwo(Point location, CellState color)
	{
		GomokuGameLogic gameLogic = gameBoard.GetComponent<GomokuGameLogic>();
		int cx = location.x;
		int cy = location.y;

		int selfCount = 0;
		int opponentCount = 0;

		for (int x = cx - 1; x <= cx + 1; x++) 
		{
			for (int y = cy - 1; y <= cy + 1; y ++)
			{
				if (gameBoard.getBlock(x, y) == color) selfCount ++;
				if (gameBoard.getBlock(x, y) == gameLogic.getOpponentColor(color)) opponentCount ++;
			}
		}
		return 2 * selfCount + opponentCount;
	}

	internal IComparable EstimateForStageTwo(int x, int y, CellState color) 
	{
		return EstimateForStageTwo(new Point(x, y), color);
	}

	internal IComparable EstimateForStageThree(Point location, CellState color)
	{
		var dx = location.x - gameBoard.getSize() / 2;
		var dy = location.y - gameBoard.getSize() / 2;
		return -Math.Sqrt(dx * dx + dy * dy);
	}

	internal IComparable EstimateForStageThree(int x, int y, CellState color) {
		return EstimateForStageThree(new Point(x, y), color);
	}

	internal List<Point> FilterCellsStageOne(CellState color) 
	{
		return FilterCellsCore(gameBoard.EnumerateCells(), EstimateForStageOne, color);
	}

	List<Point> FilterCellsStageTwo(CellState color) 
	{
		return FilterCellsCore(FilterCellsStageOne(color), EstimateForStageTwo, color);
	}

	List<Point> FilterCellsStageThree(CellState color)
	{
		return FilterCellsCore(FilterCellsStageTwo(color), EstimateForStageThree, color);
	}
}
