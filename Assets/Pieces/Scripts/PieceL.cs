using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceL : Piece
{
	public override void initialize ()
	{
		base.initialize ();
		content.AddRange (new int[] {
			0,0,0,
			1,1,1,
			1,0,0,
		});
		x = 4;
		y = 0;
		width = 3;
		color = new Color (1.0f, 0.7f, 0.2f);
		instantiateBricks ();
	}
	
	public override void rotate ()
	{
		// Back-up
		List<int> backup = new List<int> ();
		backup.AddRange (content);
		
		// Rotate
		direction = (direction + 1) % 4;
		content.Clear ();
		if (direction == 1) {
			content.AddRange (new int[] {
				1,1,0,
				0,1,0,
				0,1,0,
			});
		} else if (direction == 2) {
			content.AddRange (new int[] {
				0,0,0,
				0,0,1,
				1,1,1
			});
		} else if (direction == 3) {
			content.AddRange (new int[] {
				0,1,0,
				0,1,0,
				0,1,1,
			});
		} else {
			content.AddRange (new int[] {
				0,0,0,
				1,1,1,
				1,0,0,
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
