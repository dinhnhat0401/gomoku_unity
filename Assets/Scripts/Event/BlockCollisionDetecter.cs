using UnityEngine;
using System.Collections;

public class BlockCollisionDetecter : MonoBehaviour
{
	public GameBoard gameBoardGO;

	private GomokuGameLogic gameLogic;
		// Use this for initialization
	void Start ()
	{
		if (gameLogic == null)
		{
			gameLogic = gameBoardGO.GetComponent<GomokuGameLogic>();
		}
	}
	
		// Update is called once per frame
		void Update ()
		{
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
//			Debug.Log("chay bao nhieu lan day ");
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit.collider != null)
			{
				if (hit.collider.gameObject != null && hit.collider.gameObject.tag == "blank_block") 
				{
					bool fightWithAI = gameLogic.fightWithAI;
					if (!fightWithAI || (fightWithAI && gameLogic.gameState == GameState.White_move)) 
					{
						onTouchChange(hit.collider.gameObject);
					}
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
											RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
						
											if(hit.collider != null)
											{
												if (hit.collider.gameObject != null && hit.collider.gameObject.tag == "blank_block") 
												{
													bool fightWithAI = gameLogic.fightWithAI;
													if (!fightWithAI || (fightWithAI && gameLogic.gameState == GameState.White_move)) 
													{
														onTouchChange(hit.collider.gameObject);
													}
												}
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
						GameState game_state = gameLogic.gameState;
						switch (game_state) {
						case GameState.White_move: 
								{
										Point pos = clickedGO.GetComponent<BlockPosition> ().position;
										gameLogic.changeBlockSpriteAtLocation(clickedGO, pos);
										break;
								}
						case GameState.Begin:
						case GameState.Black_move:
								{
										Point pos = clickedGO.GetComponent<BlockPosition> ().position;
										gameLogic.changeBlockSpriteAtLocation(clickedGO, pos);
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
