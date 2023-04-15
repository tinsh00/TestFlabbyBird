using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] walls wallPref;
	[SerializeField] Transform wallContainer;
	[SerializeField] List<walls> poolWall = new List<walls>();
	[SerializeField] GameOverPanel gameOverPanel;
	[SerializeField] WaitingPanel waitingPanel;
	[SerializeField] InGameUIPanel IngameUIPanel;
	[SerializeField] float startIntervalWall = 3f;
	[SerializeField] int amountScoreToUpgradeLevel=10;
	float currentIntervalWall;
	float currentTime = 0f;
	public int currGameLevel { get; private set; } = 1;
	public int maxGameLevel = 10;
	int highScore = 0;
	int currScore = 0;
	readonly string HIGHSCORE = "HighScore";
	bool isChangeHighScore = false;
	bool hasSaveData = false;

	public State currState { get; private set; } = State.Waiting;
	RectTransform rect;

	private void Start()
	{
		rect = GetComponent<RectTransform>();
		ChangeStateGame(State.Waiting);
		LoadData();
	}

	public float Height => rect.rect.height;
	public void IncreateGameLevel()
	{
		if (currGameLevel < maxGameLevel)
		{
			currGameLevel++;
			currentIntervalWall -= currGameLevel*4 / maxGameLevel;
			if (currentIntervalWall < 0.5f) currentIntervalWall = 0.5f;
		}
	}
	public void ChangeStateGame(State state)
	{
        currState = state;
		if(state == State.Waiting)
        {
			ResetGame();
        }
	}
	public void LoadData()
	{
		if (PlayerPrefs.HasKey(HIGHSCORE)) highScore = PlayerPrefs.GetInt(HIGHSCORE);
	}
	public void SaveData()
	{
		if (!isChangeHighScore) return;
		PlayerPrefs.SetInt(HIGHSCORE, highScore);
		PlayerPrefs.Save();
	}
	public void UpdateScore()
	{
		currScore++;
		SoundMN.Score();
		IngameUIPanel.UpdateCurrentScore(currScore);
		if (currScore > highScore)
		{
			IngameUIPanel.UpdateHighScore(currScore);
			highScore = currScore;
			isChangeHighScore = true;
		}
		if(currScore > amountScoreToUpgradeLevel * currGameLevel)
		{
			IncreateGameLevel();
		}
	}
	public void ResetGame()
    {
		waitingPanel.gameObject.SetActive(true);
		currScore = 0;
		currGameLevel = 1;
		currentIntervalWall = startIntervalWall;
		Bird.Instance.ResetBird();
		gameOverPanel.gameObject.SetActive(false);
		foreach(walls wall in poolWall)
        {
			if (wall.gameObject.activeSelf) wall.gameObject.SetActive(false);
        }
	}
	public void CreateNewWall()
	{
		int y = UnityEngine.Random.Range(-200, 200);
		int x = 1050;
		foreach (walls walls in poolWall)
		{
			if (!walls.gameObject.activeSelf)
			{
				walls.InitData(new Vector3(x, y, 0), currGameLevel);
				return;
			}
		}
		walls wall = Instantiate(wallPref, wallContainer);
		wall.InitData(new Vector3(x, y, 0), currGameLevel);
	}

	private void Update()
	{
		if (!hasSaveData && currState == State.GameOver)
		{
			hasSaveData = true;
			SaveData();
			gameOverPanel.InitData(currScore);
			IngameUIPanel.DisablePanel();
		}
		if (currState == State.Waiting)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{

				ChangeStateGame(State.IsPlaying);
				IngameUIPanel.InitPanel(highScore);
				waitingPanel.gameObject.SetActive(false);
			}
		}
		if (currState == State.IsPlaying)
		{
			currentTime += Time.deltaTime;
			
			if (currentTime > currentIntervalWall)
			{
				currentTime = 0;
				CreateNewWall();
			}

		}
	}
}
public enum State
{
    Waiting,
    IsPlaying,
    GameOver
}
