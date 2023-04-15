using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverPanel : MonoBehaviour
{
    [SerializeField] Button btnRestart, btnQuit;
	[SerializeField] Text txtScore;
	private void Start()
	{
		btnRestart.onClick.AddListener(Restart);
		btnQuit.onClick.AddListener(QuitGame);
	}
	public void InitData(int score)
	{
		txtScore.text = score.ToString();
		gameObject.SetActive(true);
	}
	public void Restart()
	{
		GameManager.Instance.ChangeStateGame(State.Waiting);
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
