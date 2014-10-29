using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceO : Piece
{
	public override void initialize (bool isHighRes)
	{
		base.initialize (isHighRes);
		content.AddRange (new int[] {
			0,0,
			1,1,
			1,1
		});
		x = 5;
		y = 0;
		width = 2;
		color = new Color (1.0f, 1.0f, 0.4f);
		instantiateBricks (isHighRes);
	}
}
