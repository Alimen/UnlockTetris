using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parameters : MonoBehaviour
{
	GameManager gameManager;

	public int score;
	public int itemBought;

	public bool keyRL;
	public bool keyUD;

	public bool pieceOT;
	public bool pieceJL;
	public bool pieceSZ;
	public bool pieceI;
	public bool pieceR;

	public bool highResolution;
	public bool thirdDimension;

	public Transform prefabPieceO;
	public Transform prefabPieceT;
	public Transform prefabPieceJ;
	public Transform prefabPieceL;
	public Transform prefabPieceS;
	public Transform prefabPieceZ;
	public Transform prefabPieceI;
	public Transform prefabPieceR;

	public void initialize ()
	{
		reset ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		applyParameters ();
	}

	public void reset ()
	{
		score = 0;
		itemBought = 0;

		keyRL = false;
		keyUD = false;

		pieceOT = true;
		pieceJL = false;
		pieceSZ = false;
		pieceI = false;
		pieceR = false;

		highResolution = false;
		thirdDimension = true;
	}
	
	
	public void applyParameters ()
	{
		List<Transform> pieces = gameManager.pieces;

		pieces.Clear ();
		if (pieceOT) {
			pieces.Add (prefabPieceO);
			pieces.Add (prefabPieceT);
		}
		if (pieceJL) {
			pieces.Add (prefabPieceJ);
			pieces.Add (prefabPieceL);
		}
		if (pieceSZ) {
			pieces.Add (prefabPieceS);
			pieces.Add (prefabPieceZ);
		}
		if (pieceI) {
			pieces.Add (prefabPieceI);
		}
		if (pieceR) {
			pieces.Add (prefabPieceR);
		}
	}
}
