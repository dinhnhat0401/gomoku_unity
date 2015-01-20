using UnityEngine;
using System.Collections;

public class MenuClickEvent : MonoBehaviour {
	private GomokuGameLogic gameLogic;
	private GameObject gameBoardGO;
	private GameBoard gameBoard;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playWithAI() {
		Application.LoadLevel ("PlayScene"); 
		GameConstants.aiMode = true;
		GameConstants.userBlockColor = CellState.White;
	}

	public void playWithHuman() {
		Application.LoadLevel ("PlayScene");
		GameConstants.aiMode = false;
		GameConstants.userBlockColor = CellState.White;
	}

	public void backToMenu() {
		Application.LoadLevel("MenuScene");

	}
}
