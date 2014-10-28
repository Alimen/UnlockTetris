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
	public List<Transform> pieces = new List<Transform> ();

	// Timer
	int level;
	int line;
	float autoDownDelay;
	float autoDownTimer;

	// Key map & repeat timer
	const float startDelay = 0.12f;
	const float delayDecay = 0.8f;
	bool leftKeyDown, rightKeyDown, downKeyDown;
	float leftDelay, rightDelay, downDelay;
	float leftTimer, rightTimer, downTimer;
	bool leftFlag, rightFlag, downFlag;

	void Start ()
	{
		scoreBoard = GameObject.Find ("ScoreBoard").GetComponent<ScoreBoard> ();
		parameters = GameObject.Find ("Parameters").GetComponent<Parameters> ();
		board = GameObject.Find ("Board").GetComponent<Board> ();
		parameters.initialize ();
		StartCoroutine (game ());
	}

	void Update ()
	{
		// Left arrow
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			leftKeyDown = true;
			leftFlag = true;
			leftDelay = startDelay;
			leftTimer = 0;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			leftKeyDown = false;
		}
		
		// Right arrow
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			rightKeyDown = true;
			rightFlag = true;
			rightDelay = startDelay;
			rightTimer = 0;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			rightKeyDown = false;
		}
		
		// Down arrow
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			downKeyDown = true;
			downFlag = true;
			downDelay = startDelay;
			downTimer = 0;
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			downKeyDown = false;
		}
	}

	void checkKeyDelay ()
	{
		if (leftKeyDown) {
			leftTimer += Time.deltaTime;
			if (leftTimer > leftDelay) {
				leftFlag = true;
				leftTimer = 0;
				leftDelay *= delayDecay;
			}
		}
		if (rightKeyDown) {
			rightTimer += Time.deltaTime;
			if (rightTimer > rightDelay) {
				rightFlag = true;
				rightTimer = 0;
				rightDelay *= delayDecay;
			}
		}
		if (downKeyDown) {
			downTimer += Time.deltaTime;
			if (downTimer > downDelay) {
				downFlag = true;
				downTimer = 0;
				downDelay *= delayDecay;
			}
		}
	}
	
	IEnumerator game ()
	{
		// Initializing and start intro. sequence
		initialize ();
		
		while (true) {
			createPiece ();
			leftDelay = startDelay;
			rightDelay = startDelay;
			downDelay = startDelay;
			autoDownTimer = 0;

			while (true) {
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					curPiece.rotate ();
				}

				checkKeyDelay ();
				if (leftFlag) {
					leftFlag = false;
					if (curPiece.checkAvailable (-1, 0)) {
						curPiece.x--;
					}
				}
				if (rightFlag) {
					rightFlag = false;
					if (curPiece.checkAvailable (1, 0)) {
						curPiece.x++;
					}
				}
				if (downFlag) {
					downFlag = false;
					if (!moveDown ()) {
						break;
					}
				}

				autoDownTimer += Time.deltaTime;
				if (autoDownTimer >= autoDownDelay) {
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
		autoDownDelay = autoDownDelayByLevel (level);
		leftKeyDown = false;
		rightKeyDown = false;
		downKeyDown = false;
	}

	void levelUp ()
	{
		level++;
		autoDownDelay = autoDownDelayByLevel (level);
	}

	float autoDownDelayByLevel (int lv)
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
			autoDownTimer = 0;
			return true;
		} else {
			curPiece.updatePosition ();
			return false;
		}
	}
}
