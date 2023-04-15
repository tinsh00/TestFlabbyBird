using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Clound : MonoBehaviour
{
	RectTransform rect;
	private void Start()
	{
		rect = GetComponent<RectTransform>();
		int duration = Random.Range(2, 4);
		rect.DOLocalMoveX(rect.localPosition.x+50f, duration, true).SetLoops(-1,LoopType.Yoyo);
	}

}
