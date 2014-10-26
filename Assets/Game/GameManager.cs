using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	// Component handlers
	ScoreBoard scoreBoard;
	Parameters parameters;

	// Board dimension
	Board board;
	public Transform nxtPieceSpawnPoint;

	// Pieces & prefabs
	Piece curPiece = null, nxtPiece = null;
	public Transform pieceO;
	public Transform pieceT;
	public Transform pieceJ;
	public Transform pieceL;
	public Transform pieceS;
	public Transform pieceZ;
	public Transform pieceI;
	public Transform pieceR;
	List<Transform> pieces = new List<Transform> ();

	// Timer
	int level;
	int line;
	float downDelay;
	float downTimer;

	// Key map
	float inputDelay;
	float inputTimer;
	bool leftKeyDown, rightKeyDown, downKeyDown;

	void Start ()
	{
		scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<ScoreBoard> ();
		parameters = GameObject.Find ("Parameters").GetComponent<Parameters> ();
		board = GameObject.Find ("Board").GetComponent<Board> ();
		StartCoroutine (game ());
	}

	void Update ()
	{
		applyParameters ();

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			leftKeyDown = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			leftKeyDown = false;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			rightKeyDown = true;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			rightKeyDown = false;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			downKeyDown = true;
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			downKeyDown = false;
		}
	}
	
	IEnumerator game ()
	{
		// Initializing and start intro. sequence
		applyParameters ();
		initialize ();
		
		while (true) {
			createPiece ();
			inputTimer = 0;
			downTimer = 0;

			while (true) {
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					curPiece.rotate ();
				}

				inputTimer += Time.deltaTime;
				if (inputTimer >= inputDelay) {
					inputTimer = 0;
					if (leftKeyDown && curPiece.checkAvailable (-1, 0)) {
						curPiece.x--;
					}
					if (rightKeyDown && curPiece.checkAvailable (1, 0)) {
						curPiece.x++;
					}
					if (downKeyDown) {
						if (!moveDown ()) {
							break;
						}
					}
				}

				downTimer += Time.deltaTime;
				if (downTimer >= downDelay) {
					downTimer = 0;
					if (!moveDown ()) {
						break;
					}
				}

				curPiece.updatePosition ();
				yield return null;
			}
			
			parameters.score += curPiece.score;
			scoreBoard.setScore (parameters.score);
			if (!curPiece.apply ()) {
				break;
			}

			List<int> fullLine = board.findFullLines ();
			if (fullLine.Count > 0) {
				line += fullLine.Count;
				if (line >= 2 * level) {
					levelUp ();
				}
				parameters.score += Mathf.FloorToInt (100 * Mathf.Pow (2, fullLine.Count - 1));
				scoreBoard.setScore (parameters.score);
				yield return StartCoroutine (board.clearLine (fullLine));
			}
		}

		// Game Over sequence
		curPiece.hide ();
		nxtPiece.hide ();
		yield return StartCoroutine (board.gameover ());
	}

	void initialize ()
	{
		createPiece ();
		level = 1;
		line = 0;
		downDelay = downDelayByLevel (level);
		inputDelay = 0.1f;
		leftKeyDown = false;
		rightKeyDown = false;
		downKeyDown = false;
	}

	void levelUp ()
	{
		level++;
		downDelay = downDelayByLevel (level);
	}

	float downDelayByLevel (int lv)
	{
		return 0.6f * Mathf.Pow (0.9f, lv);
	}

	void createPiece ()
	{
		curPiece = nxtPiece;

		int r = Random.Range (0, pieces.Count);
		nxtPiece = (Instantiate (pieces[r]) as Transform).GetComponent<Piece> ();
		nxtPiece.initialize ();
		nxtPiece.transform.position = nxtPieceSpawnPoint.position;
	}

	bool moveDown ()
	{
		if (curPiece.checkAvailable (0, 1)) {
			curPiece.y++;
			return true;
		} else {
			curPiece.updatePosition ();
			return false;
		}
	}

	void applyParameters ()
	{
		if (!parameters.change) {
			return;
		}
		parameters.change = false;

		pieces.Clear ();
		if (parameters.pieceOT) {
			pieces.Add (pieceO);
			pieces.Add (pieceT);
		}
		if (parameters.pieceJL) {
			pieces.Add (pieceJ);
			pieces.Add (pieceL);
		}
		if (parameters.pieceSZ) {
			pieces.Add (pieceS);
			pieces.Add (pieceZ);
		}
		if (parameters.pieceI) {
			pieces.Add (pieceI);
		}
		if (parameters.pieceR) {
			pieces.Add (pieceR);
		}
	}
}
