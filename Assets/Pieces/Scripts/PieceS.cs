using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceS : Piece
{
	public override void initialize (bool isHighRes)
	{
		base.initialize (isHighRes);
		content.AddRange (new int[] {
			0,0,0,
			0,1,1,
			1,1,0,
		});
		x = 4;
		y = 0;
		width = 3;
		color = new Color (1.0f, 0.3f, 1.0f);
		instantiateBricks (isHighRes);
	}
	
	public override void rotate ()
	{
		// Back-up
		List<int> backup = new List<int> ();
		backup.AddRange (content);
		
		// Rotate
		direction = (direction + 1) % 2;
		content.Clear ();
		if (direction == 1) {
			content.AddRange (new int[] {
				1,0,0,
				1,1,0,
				0,1,0,
			});
		} else {
			content.AddRange (new int[] {
				0,0,0,
				0,1,1,
				1,1,0,
			});
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
