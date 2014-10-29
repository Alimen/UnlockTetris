using UnityEngine;
using System.Collections;

public class GestureInput : MonoBehaviour
{
	// Manager handlers
	GameManager gameManager;

	// Screen dimensions
	float width;
	float height;

	// Mouse position
	const float moveThres = 2.0f;
	bool pressing = false;
	bool drag = false;
	Vector2 startPosition;

	// Key map
	bool leftKeyDown = false, rightKeyDown = false, downKeyDown = false, upKeyDown = false;
	bool leftKeyHold = false, rightKeyHold = false, downKeyHold = false;
	bool leftKeyUp = false, rightKeyUp = false, downKeyUp = false;

	void Start ()
	{
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		Vector3 newPosition = Camera.main.ScreenToWorldPoint (Vector3.zero);
		newPosition.z = transform.position.z;
		transform.position = newPosition;
		width = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0)).x - transform.position.x;
		height = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height)).y - transform.position.y;
	}

	public void updateKey ()
	{
		if (Input.GetMouseButtonDown (0)) {
			pressing = true;
			drag = false;
			startPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}

		if (!pressing) {
			return;
		}

		Vector2 curPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 delta = curPos - startPosition;
		if ((Mathf.Abs (delta.y) > Mathf.Abs (delta.x)) && ((-1) * delta.y >= moveThres)) {
			// Down
			drag = true;
			if (!downKeyHold) {
				if (leftKeyHold) {
					leftKeyHold = false;
					leftKeyUp = true;
				}
				if (rightKeyHold) {
					rightKeyHold = false;
					rightKeyUp = true;
				}
				downKeyDown = true;
				downKeyHold = true;
			}
		} else if (delta.x >= moveThres) {
			// Right
			drag = true;
			if (!rightKeyHold) {
				if (leftKeyHold) {
					leftKeyHold = false;
					leftKeyUp = true;
				}
				if (downKeyHold) {
					downKeyHold = false;
					downKeyUp = true;
				}
				rightKeyDown = true;
				rightKeyHold = true;
			}
		} else if ((-1) * delta.x >= moveThres) {
			// Left
			drag = true;
			if (!leftKeyHold) {
				if (rightKeyHold) {
					rightKeyHold = false;
					rightKeyUp = true;
				}
				if (downKeyHold) {
					downKeyHold = false;
					downKeyUp = true;
				}
				leftKeyDown = true;
				leftKeyHold = true;
			}
		} else {
			// Center
			if (leftKeyHold) {
				leftKeyHold = false;
				leftKeyUp = true;
			}
			if (rightKeyHold) {
				rightKeyHold = false;
				rightKeyUp = true;
			}
			if (downKeyHold) {
				downKeyHold = false;
				downKeyUp = true;
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			pressing = false;

			if (leftKeyHold) {
				leftKeyHold = false;
				leftKeyUp = true;
			}
			if (rightKeyHold) {
				rightKeyHold = false;
				rightKeyUp = true;
			}
			if (downKeyHold) {
				downKeyHold = false;
				downKeyUp = true;
			}

			if (!drag) {
				upKeyDown = true;
			}
		}
	}

	public bool getKeyDown (KeyCode k)
	{
		switch (k) {
		case KeyCode.LeftArrow:
			if (leftKeyDown) {
				leftKeyDown = false;
				return true;
			}
			break;

		case KeyCode.RightArrow:
			if (rightKeyDown) {
				rightKeyDown = false;
				return true;
			}
			break;

		case KeyCode.DownArrow:
			if (downKeyDown) {
				downKeyDown = false;
				return true;
			}
			break;

		case KeyCode.UpArrow:
			if (upKeyDown) {
				upKeyDown = false;
				return true;
			}
			break;
		}
		return false;
	}
	
	
	public bool getKeyUp (KeyCode k)
	{
		switch (k) {
		case KeyCode.LeftArrow:
			if (leftKeyUp) {
				leftKeyUp = false;
				return true;
			}
			break;
			
		case KeyCode.RightArrow:
			if (rightKeyUp) {
				rightKeyUp = false;
				return true;
			}
			break;
			
		case KeyCode.DownArrow:
			if (downKeyUp) {
				downKeyUp = false;
				return true;
			}
			break;
		}
		return false;
	}
}
