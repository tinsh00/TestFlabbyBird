using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIPanel : MonoBehaviour
{
	[SerializeField] Text highScoreTxt;
	[SerializeField] Text yourScoreTxt;
	public void InitPanel(int highScore,int currScore = 0)
	{
		gameObject.SetActive(true);
		highScoreTxt.text = highScore.ToString();
		yourScoreTxt.text = currScore.ToString();
	}
	public void UpdateHighScore(int score)
	{
		highScoreTxt.text = score.ToString();
	}
	public void UpdateCurrentScore(int score)
	{
		yourScoreTxt.text = score.ToString();
	}

	public void DisablePanel()
	{
		gameObject.SetActive(false);
	}
}
