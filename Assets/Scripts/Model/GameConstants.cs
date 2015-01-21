using UnityEngine;
using System.Collections;

// Where to declare game constants
public class GameConstants {
	public const int GAME_GRID_WIDTH = 15;
	public const int GAME_GRID_HEIGHT = 15;

	// block count needed to win game
	public const int GAME_WIN_POINT = 5;

	// fight with AI or human
	public static bool aiMode = false;

	// detemine user choose black or white block
	public static CellState userBlockColor;   
}
