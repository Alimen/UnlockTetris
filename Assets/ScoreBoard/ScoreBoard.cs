using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour
{
	TextMesh score;
	Transform coin;

	public Material loResMat;
	public Material hiResMat;

	const float speed = 100.0f;
	float currentScore;
	float targetScore;

	void Awake ()
	{
		score = transform.Find ("Score").GetComponent<TextMesh> ();
		coin = transform.Find ("Coin");
	}

	void Update ()
	{
		if (targetScore == currentScore) {
			return;
		}

		float delta = targetScore - currentScore;
		if (Mathf.Abs (delta) < speed * Time.deltaTime) {
			currentScore = targetScore;
		} else {
			currentScore += Mathf.Sign (delta) * speed * Time.deltaTime;
		}

		updateScore ();
	}

	void updateScore ()
	{
		score.text = currentScore.ToString ("000000");
	}

	public void setResolution (bool hiRes)
	{
		if (hiRes) {
			coin.renderer.material = hiResMat;
		} else {
			coin.renderer.material = loResMat;
		}
	}

	public void setScore (int target, bool immed = false)
	{
		if (immed) {
			currentScore = target;
		}
		targetScore = target;
	}
}
