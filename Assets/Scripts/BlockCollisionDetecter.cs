using UnityEngine;
using System.Collections;

public class BlockCollisionDetecter : MonoBehaviour {
	public Transform white_block;
	public Transform black_block;
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
//					Debug.Log("object clicked: "+hit.collider.tag);
					onTouchChange();
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

	void onTouchChange () {
		this.tag = "black_block";
		if (gameBoard == null) {
			gameBoard = GameObject.FindGameObjectWithTag("game_board");
		}
		if (gameBoard == null) {
			Debug.Log("CANNOT FIND OBJECT WITH TAG GAME_BOARD");
		} else {
			int game_state = -1;
			game_state = gameBoard.GetComponent<GameBoardScript>().gameState;
			Debug.Log(game_state);
			switch (game_state) 
			{
				case GameConstants.GAME_STATE_BEGIN:
				case GameConstants.GAME_STATE_WHITE_MOVE: 
				{
					break;
				}
				case GameConstants.GAME_STATE_BLACK_MOVE:
				{
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
