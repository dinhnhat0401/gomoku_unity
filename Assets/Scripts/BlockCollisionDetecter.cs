using UnityEngine;
using System.Collections;

public class BlockCollisionDetecter : MonoBehaviour {
	public Sprite white_block;
	public Sprite black_block;
	public GameObject gameBoard;

	private GameBoardScript gameBoardScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if (Input.GetButtonDown ("Fire1")) {
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null)
				{
					Debug.Log("object clicked: "+hit.collider.tag);
					onTouchChange(hit.collider.gameObject);
				}
			}
		}
#else 
	int nbTouches = Input.touchCount;
	if(nbTouches > 0)
	{
		Debug.Log("numb = "+nbTouches);
		for (int i = 0; i < nbTouches; i++)
		{
			Touch touch = Input.GetTouch(i);
			
			if(touch.phase == TouchPhase.Began)
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
					
				if(hit.collider != null)
				{
					Debug.Log("object clicked: "+hit.collider.tag);
				}
			}
				
		}
	}
#endif
	}

	void onTouchChange (GameObject clickedGO) {
		this.tag = "black_block";
		if (gameBoard == null) {
			gameBoard = GameObject.FindGameObjectWithTag("game_board");
		}
		if (gameBoard == null) {
			Debug.Log("CANNOT FIND OBJECT WITH TAG GAME_BOARD");
		} else {
			GameConstants.GameState game_state = gameBoard.GetComponent<GameBoardScript>().gameState;
			Debug.Log(game_state);
			switch (game_state) 
			{
				case GameConstants.GameState.Begin:
				case GameConstants.GameState.White_move: 
				{
					clickedGO.GetComponent<SpriteRenderer>().sprite = white_block;
					break;
				}
				case GameConstants.GameState.Black_move:
				{
					clickedGO.GetComponent<SpriteRenderer>().sprite = black_block;
					break;
				}
				default: 
				{
					break;
				}
			}
		}
	}
}
