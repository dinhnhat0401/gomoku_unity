using UnityEngine;
using System.Collections;

public static class GameConstants {
	public enum CellState {
		None,
		White,
		Black
	}

	public enum GameState {
		Begin,
		White_move,
		Black_move,
		end
	}
}
