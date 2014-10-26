using UnityEngine;
using System.Collections;

public class Parameters : MonoBehaviour
{
	public int score;
	public int itemBought;
	public bool change;

	public bool keyRL;
	public bool keyUD;

	public bool pieceOT;
	public bool pieceJL;
	public bool pieceSZ;
	public bool pieceI;
	public bool pieceR;

	public bool highResolution;
	public bool thirdDimension;

	void Awake ()
	{
		reset ();
	}

	public void reset ()
	{
		score = 0;
		itemBought = 0;
		change = true;

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
}
