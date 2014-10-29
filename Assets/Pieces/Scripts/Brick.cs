using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
	Material mat;
	Color color = new Color (0.66f, 0.66f, 0.66f, 1);
	public Material loResMat;
	public Material hiResMat;

	public void initialize ()
	{
		initialize (Color.gray);
	}

	public void initialize (Color color, bool isHighRes = false)
	{
		this.color = color;

		setResolution (isHighRes);
		setColor (color);
	}

	public void setResolution (bool isHighRes)
	{
		renderer.material = (isHighRes)? hiResMat: loResMat;
		mat = renderer.material;
		mat.color = color;
	}

	public void setColor (Color color)
	{
		mat.color = color;
	}

	public static int sortByPosition (Brick x, Brick y)
	{
		return y.transform.position.y.CompareTo (x.transform.position.y);
	}
}
