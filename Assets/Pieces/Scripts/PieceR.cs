using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceR : Piece
{
	const int pieceWidth = 5;
	
	public override void initialize (bool isHighRes)
	{
		base.initialize (isHighRes);
		content.AddRange (new int[] {
			0,0,0,0,0,
			0,0,0,0,0,
			0,0,1,0,0,
			0,0,0,0,0,
			0,0,0,0,0,
		});
		int r = 5 + Random.Range (0, 1) + Random.Range (0, 1);
		for (int i = 0; i < r; i++) {
			generateRandomOneBrick ();
		}
		x = 4;
		y = 0;
		width = pieceWidth;
		color = new Color (0.1f, 0.1f, 0.1f);
		instantiateBricks (isHighRes);
	}

	void generateRandomOneBrick ()
	{
		int r = Random.Range (0, pieceWidth * pieceWidth);
		bool ok = false;
		while (!ok) {
			r = Random.Range (0, pieceWidth * pieceWidth);
			if (touch (r)) {
				ok = true;
			}
		}
		content[r] = 1;
	}
	
	bool touch (int r)
	{
		if (content[r] == 1) {
			return false;
		}
		if ((r - 1 >= 0) && content[r - 1] == 1 && (r % pieceWidth != 0)) {
			return true;
		}
		if ((r + 1 < content.Count) && content[r + 1] == 1 && (r % pieceWidth != pieceWidth - 1)) {
			return true;
		}
		if ((r - pieceWidth >= 0) && content[r - pieceWidth] == 1) {
			return true;
		}
		if ((r + pieceWidth < content.Count) && content[r + pieceWidth] == 1) {
			return true;
		}

		return false;
	}

	
	public override void rotate ()
	{
		// Back-up
		List<int> backup = new List<int> ();
		backup.AddRange (content);
		
		// Rotate
		direction = (direction + 1) % 4;
		int c1, c2;
		int center = pieceWidth / 2;
		for (int i = 0 - center; i < pieceWidth - center; i++) {
			for (int j = 0 - center; j < pieceWidth - center; j++) {
				c1 = (center + i) * pieceWidth + (center + j);
				c2 = (center + j) * pieceWidth + (center - i);
				content[c1] = backup[c2];
			}
		}
		
		// Check kicks
		if (checkAvailable (0, 0)) {
		} else if (checkAvailable (1, 0)) {
			x += 1;
		} else if (checkAvailable (-1, 0)) {
			x -= 1;
		} else if (checkAvailable (0, -1)) {
			y -= 1;
		} else {
			direction = (direction + 3) % 4;
			content.Clear ();
			content.AddRange (backup);
		}
		
		// Update brick local position
		updatePosition ();
	}
}
