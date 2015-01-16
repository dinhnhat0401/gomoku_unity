using UnityEngine;
using System.Collections;

public class GameBoardScript : MonoBehaviour
{
	public GameConstants.GameState gameState;

	public int _gridWidth;
	//the grid number of cell horizontally
	public int _gridHeight;
	//the grid number of cell vertically
	public GameObject[,] _arrayOfShapes;
	//The main array that contain all games tiles

	public GameObject prefab;

	// Use this for initialization
	void Start ()
	{
		gameState = GameConstants.GameState.Begin;
		_arrayOfShapes = new GameObject[_gridWidth, _gridHeight];
		for (int i = 0; i < _gridWidth; i++) {
			for (int j = 0; j < _gridHeight; j++) {
				GameObject go = GameObject.Instantiate(prefab, new Vector3( (i - 7) * 0.835f, (j - 7) * 0.835f, 0), transform.rotation) as GameObject;
				go.tag = "blank_block";
				_arrayOfShapes[i,j]= go;			
			}
		}
	}
	
//	// Update is called once per frame
//	void Update ()
//	{
//
//	}
}
