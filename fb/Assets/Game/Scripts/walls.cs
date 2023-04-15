using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walls : MonoBehaviour
{
    [SerializeField] GameObject wallUp,wallDown;
    public float startDistanceBettwenWalls = 400f;
    public int startSpeed = 100;
    int currentSpeed;
	int currentLevelGame;
	float minWallUpY;
	float maxWallDownY;
	bool hasCountScore = false;
	float minX => transform.localPosition.x - rect.rect.width / 2;
	float maxX => transform.localPosition.x + rect.rect.width / 2;
	public float maxHeightDoor => minWallUpY;
	public float minHeightDoor => maxWallDownY;
	
	RectTransform rect;
	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}
	void SetPositionTwoWall(int levelGame)
	{ 
		minWallUpY = transform.localPosition.y + startDistanceBettwenWalls / 2 - levelGame * 10;
		maxWallDownY = transform.localPosition.y - startDistanceBettwenWalls / 2 + levelGame * 10;
		wallUp.transform.localPosition = new Vector3(0, startDistanceBettwenWalls / 2 - levelGame * 10, 0);
		wallDown.transform.localPosition = new Vector3(0, -startDistanceBettwenWalls / 2 + levelGame * 10, 0);
    }
    public void InitData(Vector3 localPos,int levelGame)
	{
		hasCountScore = false;
		transform.localPosition = localPos;
        SetPositionTwoWall(levelGame);
		SetSpeed(levelGame);
		if (!gameObject.activeSelf) gameObject.SetActive(true);
	}
	public void SetSpeed(int levelGame)
	{
		currentLevelGame = levelGame;
		currentSpeed = startSpeed + startSpeed * levelGame;
	}
    public void DisableWall()
	{
		gameObject.SetActive(false);
	}
    private void Update()
    {
		if (GameManager.Instance.currState == State.IsPlaying)
		{
			//Change Speed follow gamelevel
			if(currentLevelGame!= GameManager.Instance.currGameLevel)
			{
				SetSpeed(GameManager.Instance.currGameLevel);
			}
			//move left
			transform.localPosition -= new Vector3(1, 0, 0) * currentSpeed * Time.deltaTime;
			//check ball position when wall move in x ball area 
			if (minX < Bird.Instance.GetMaxXBird && maxX > Bird.Instance.getMinXBird)
			{
				Bird.Instance.CheckPosition(this);
			}
			//check wall out of x ball area => score
			if (!hasCountScore && minX < -50 )
			{
				hasCountScore = true;
				GameManager.Instance.UpdateScore();
			}
			// out of range -> diable
			if (maxX <= -1050)
			{
				DisableWall();
			}
		}

	}
}
