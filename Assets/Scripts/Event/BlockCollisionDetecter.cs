using UnityEngine;
using System.Collections;

public class BlockCollisionDetecter : MonoBehaviour
{
		public Sprite white_block;
		public Sprite black_block;
		public GameBoard gameBoardGO;

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("chay bao nhieu lan day ");
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null)
			{
				if (hit.collider.gameObject != null && hit.collider.gameObject.tag == "blank_block") 
				{
					//					Debug.Log("object clicked: "+Camera.main.ScreenToWorldPoint(Input.mousePosition));
					onTouchChange(hit.collider.gameObject);
				}
			}
		}
#else 
				if (Input.GetMouseButtonDown (0)) {
						int nbTouches = Input.touchCount;
						if (nbTouches > 0) {
								Debug.Log ("numb = " + nbTouches);
								for (int i = 0; i < nbTouches; i++) {
										Touch touch = Input.GetTouch (i);
					
										if (touch.phase == TouchPhase.Began) {
												RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (touch.position), Vector2.zero);
						
												if (hit.collider != null) {
														Debug.Log ("object clicked: " + hit.collider.tag);
												}
										}
					
								}
						}
				}
				#endif
		}
	
		void onTouchChange (GameObject clickedGO)
		{
				if (gameBoardGO == null) {
						Debug.Log ("CANNOT FIND OBJECT WITH TAG GAME_BOARD");
				} else {
						GomokuGameLogic gameLogic = gameBoardGO.GetComponent<GomokuGameLogic> ();
						GameState game_state = gameLogic.gameState;
						switch (game_state) {
						case GameState.Begin:
						case GameState.White_move: 
								{
										clickedGO.tag = "white_block";
										clickedGO.GetComponent<SpriteRenderer> ().sprite = white_block;
										Point pos = clickedGO.GetComponent<BlockPosition> ().position;
										gameBoardGO.pushBlock (pos, CellState.White);
										Debug.Log ("=====" + pos.x + "asdfasfsdf " + pos.y);
										if (gameLogic.HavingVictoryAtPosition (pos, CellState.White)) {
												gameLogic.gameState = GameState.end;
										} else {
												gameLogic.gameState = GameState.Black_move;
										}	
										break;
								}
						case GameState.Black_move:
								{
										clickedGO.tag = "black_block";	
										clickedGO.GetComponent<SpriteRenderer> ().sprite = black_block;
										Point pos = clickedGO.GetComponent<BlockPosition> ().position;
										gameBoardGO.pushBlock (pos, CellState.Black);
										if (gameLogic.HavingVictoryAtPosition (pos, CellState.Black)) {
												gameLogic.gameState = GameState.end;
										} else {
												gameLogic.gameState = GameState.White_move;
										}	
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
