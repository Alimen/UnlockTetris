using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
	// Board
	protected Board board;

	// Bricks
	public Transform brick;
	protected Color color = new Color ();
	protected List<Brick> bricks = new List<Brick> ();

	// Dimensions
	public int x, y;
	public int score;
	protected int width;
	protected int direction = 0;
	protected List<int> content = new List<int> ();

	public virtual void initialize (bool isHighRes)
	{
		board = GameObject.Find ("Board").GetComponent<Board> ();
	}

	public virtual void rotate () {}

	protected void instantiateBricks (bool isHighRes)
	{
		if (width <= 0) {
			Debug.LogError ("Instantiating an un-initialized piece.");
			return;
		}
		
		Brick b;
		int height = content.Count / width;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				int cnt = i * width + j;
				if (content[cnt] != 0) {
					b = (Instantiate (brick) as Transform).GetComponent<Brick> ();
					b.transform.parent = transform;
					b.transform.localPosition = new Vector3 (j, (-1) * i, 0);
					b.initialize (color, isHighRes);
					bricks.Add (b);
				}
			}
		}

		return;
	}

	public void updatePosition ()
	{
		if (width <= 0) {
			Debug.LogError ("Updating an un-initialized piece.");
			return;
		}
		
		int cnt = 0;
		int height = content.Count / width;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (content[i * width + j] != 0) {
					if (bricks.Count <= cnt) {
						Debug.LogError ("Update pieces failed: brick count not fit.");
						return;
					}
					bricks[cnt].transform.localPosition = new Vector3 (j, (-1) * i, 0);
					cnt++;
				}
			}
		}

		Vector3 newPosition = new Vector3 ();
		newPosition.x = x + 1;
		newPosition.y = Board.height - y;
		newPosition.z = 0;
		transform.position = newPosition + board.transform.position;

		return;
	}

	public bool checkAvailable (int offsetX, int offsetY)
	{
		if (width <= 0) {
			Debug.LogError ("Performing a position check to an un-initialized piece.");
			return false;
		}

		bool result = true;
		int height = content.Count / width;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (content[i * width + j] != 0) {
					int boardX = x + j + offsetX;
					int boardY = y + i + offsetY;
					if (!isXonboard (boardX) || !isYonboard (boardY) || board.content[boardY, boardX] != 0) {
						result = false;
						break;
					}
				}
			}
		}

		return result;
	}

	bool isXonboard (int x)
	{
		return (x >= 0 && x < Board.width);
	}

	bool isYonboard (int y)
	{
		return (y >= 0 && y <Board.height);
	}

	public bool apply ()
	{
		if (!checkAvailable (0, 0)) {
			return false;
		}

		int height = content.Count / width;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (content[i * width + j] != 0) {
					board.content[y + i, x + j] = 1;
				}
			}
		}

		foreach (Brick b in bricks) {
			b.transform.parent = board.transform;
			board.bricks.Add (b);
		}
		Destroy (gameObject);
		return true;
	}
	
	public void setResolution (bool isHighRes)
	{
		foreach (Brick b in bricks) {
			b.setResolution (isHighRes);
		}
	}

	public void show ()
	{
		foreach (Brick b in bricks) {
			b.renderer.enabled = true;
		}
	}
	
	public void hide ()
	{
		foreach (Brick b in bricks) {
			b.renderer.enabled = false;
		}
	}
}
