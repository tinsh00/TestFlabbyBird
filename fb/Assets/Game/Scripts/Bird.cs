using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Singleton<Bird>
{
    [SerializeField] float jumpHeight = 150f;
    [SerializeField] float jumpDuration = 3f;
    [SerializeField] float startFallForce = 200f;
    public float currFallForce;
    bool isJumping = false;
    bool isAlive = true;

    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] GameObject fireObject;
    RectTransform rect;
	private void Start()
	{
        rect = GetComponent<RectTransform>();
	}

    public void SetBirdStatus(bool status) { isAlive = status; }
    float GetMaxYBird => transform.localPosition.y + rect.rect.height / 2;
    float getMinYBird => transform.localPosition.y - rect.rect.height / 2;
    public float GetMaxXBird => transform.localPosition.x + rect.rect.width / 2;
    public float getMinXBird => transform.localPosition.x - rect.rect.width / 2;
    public bool IsInAreaY => GetMaxYBird < GameManager.Instance.Height/2 && getMinYBird > -GameManager.Instance.Height / 2;
    public void CheckPosition(walls wall)
    {
        bool deadStatus = transform.localPosition.y + 50 > wall.maxHeightDoor || transform.localPosition.y - 50 < wall.minHeightDoor;
        if (!deadStatus) return;
        SetBirdStatus(false);
        SoundMN.Dead();
        GameManager.Instance.ChangeStateGame(State.GameOver);
    }
    public void ResetBird()
	{
        SetBirdStatus(true);
        transform.localPosition = Vector3.zero;
	}
    IEnumerator Jump()
    {
        isJumping = true;
        fireObject.SetActive(true);
        SoundMN.Jump();
        // Calculate the jump trajectory
        float time = 0.0f;
        Vector3 startPos = transform.localPosition;
        while (time < jumpDuration)
        {
            float height = jumpCurve.Evaluate(time / jumpDuration) * jumpHeight;
            transform.localPosition = new Vector3(startPos.x, startPos.y + height, startPos.z);
            time += Time.deltaTime;
            yield return null;
        }
        currFallForce = startFallForce;
        fireObject.SetActive(false);
        isJumping = false;
    }
    void Update()
    {
        if (GameManager.Instance.currState == State.IsPlaying)
        {
            if (!IsInAreaY)
            {
                //GameOver
                GameManager.Instance.ChangeStateGame(State.GameOver);
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && isAlive)
            {
                StartCoroutine(Jump());
            }
            //Fall
            currFallForce += startFallForce * Time.deltaTime;
            transform.localPosition -= Vector3.up * currFallForce * Time.deltaTime;
        }
    }
}
