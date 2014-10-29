using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
	public const int width = 11;
	public const int height = 21;
	public int[,] content = new int[height, width];
	public List<Brick> bricks = new List<Brick> ();

	void Awake ()
	{
		reset ();
	}

	public void reset ()
	{
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				content[i, j] = 0;
			}
		}
		bricks.Clear ();
	}

	public List<int> findFullLines ()
	{
		List<int> output = new List<int> ();
		bool currentLineIsFull;

		for (int i = 0; i < height; i++) {
			currentLineIsFull = true;
			for (int j = 0; j < width; j++) {
				if (content[i, j] == 0) {
					currentLineIsFull = false;
					break;
				}
			}

			if (currentLineIsFull) {
				output.Add (i);
			}
		}

		return output;
	}

	public IEnumerator clearLine (List<int> lines)
	{
		List<Brick> targets = new List<Brick> ();
		bricks.Sort (Brick.sortByPosition);

		int i, j = 0;
		for (i = 0; i < bricks.Count; i++) {
			if (Board.height - bricks[i].transform.position.y == lines[j]) {
				targets.AddRange (bricks.GetRange (i, Board.width));
				if (j >= lines.Count - 1) {
					break;
				}
				j++;
			}
		}

		for (i = 0; i < 6; i++) {
			foreach (Brick b in targets) {
				b.renderer.material.color *= 1.5f;
			}
			yield return null;
		}

		foreach (Brick b in targets) {
			bricks.Remove (b);
			Destroy (b.gameObject);
		}

		for (i = lines.Count - 1; i >= 0; i--) {
			removeLine (lines[i]);
			for (j = 0; j < lines.Count; j++) {
				lines[j]++;
			}
		}
	}

	void removeLine (int line)
	{
		int i, j;
		for (i = line; i > 0; i--) {
			for (j = 0; j < Board.width; j++) {
				content[i, j] = content[i - 1, j];
			}
		}
		for (i = 0; i < Board.width; i++) {
			content[0, i] = 0;
		}

		int y = Board.height - line;
		foreach (Brick b in bricks) {
			if (b.transform.position.y > y) {
				b.transform.position += Vector3.down;
			}
		}
	}

	public IEnumerator gameover ()
	{
		bricks.Sort (Brick.sortByPosition);
		for (int i = bricks.Count - 1; i >= 0; i--) {
			bricks[i].setColor (new Color (0.66f, 0.66f, 0.66f));
			yield return new WaitForSeconds (0.05f);
		}
	}

	public void setResolution (bool isHighRes)
	{
		Transform border = transform.Find ("Border");
		foreach (Transform t in border) {
			Brick b = t.GetComponent<Brick> ();
			if (b != null) {
				b.setResolution(isHighRes);
			}
		}

		foreach (Transform t in transform) {
			Brick b = t.GetComponent<Brick> ();
			if (b != null) {
				b.setResolution(isHighRes);
			}
		}
	}
}
